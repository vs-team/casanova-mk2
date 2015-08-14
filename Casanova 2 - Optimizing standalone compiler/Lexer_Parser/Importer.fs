module CasanovaCompiler.Compiler.Importer
open CasanovaCompiler.ParseAST.Merge
open System.IO
open System
open Microsoft.FSharp.Compiler.ErrorLogger
open Microsoft.FSharp.Text.Lexing
open CasanovaCompiler.Compiler.Lexhelp
open Microsoft.FSharp.Compiler.UnicodeLexing


let casanova_tokenizer (filename : string) error_logger : Microsoft.FSharp.Text.Lexing.LexBuffer<char> -> (CasanovaCompiler.Compiler.Lexhelp.lexargs -> bool -> Lexing.LexBuffer<char> -> CasanovaCompiler.Parser.token)->Lexing.LexBuffer<char>->CasanovaCompiler.Parser.token = 
        (fun lexbuf (token : (CasanovaCompiler.Compiler.Lexhelp.lexargs -> bool -> Lexing.LexBuffer<char> -> CasanovaCompiler.Parser.token)) ->
          let lightSyntaxStatus = CasanovaCompiler.Compiler.Lexhelp.LightSyntaxStatus(true,true) 
          let lexargs = CasanovaCompiler.Compiler.Lexhelp.mkLexargs (filename,[],lightSyntaxStatus,LexResourceManager(), ref [], error_logger)
          let tokenizer =
            CasanovaCompiler.Compiler.Lexfilter.LexFilter(lightSyntaxStatus, true, token lexargs true, lexbuf)
          tokenizer.Lexer)
          
let lexbuf_builder = fun filename -> UnicodeFileAsLexbuf(filename,None,false)

let load_ast_from_file error_logger filename =
  let lexbuf = lexbuf_builder filename
  resetLexbufPos filename lexbuf
  reusingLexbufForParsing 
    lexbuf 
    (fun () -> 
      let lexer = casanova_tokenizer filename error_logger lexbuf CasanovaCompiler.Lexer.token
      CasanovaCompiler.Parser.start lexer lexbuf)

let loadCasanovaFileOrProject filename : CasanovaCompiler.ParseAST.Program list =
  
  let file = if Path.IsPathRooted(filename) then filename 
             else Path.Combine(Directory.GetCurrentDirectory(), filename)

  let error_logger =
    let error_logger = ErrorLogger()
    let filteringErrorLogger = ErrorLoggerFilteringByScopedPragmas(false,[],error_logger)
    let error_logger = DelayedErrorLogger(filteringErrorLogger)
    CompileThreadStatic.ErrorLogger <- error_logger
    error_logger


  let lexbuf = lexbuf_builder filename
  let lexer = casanova_tokenizer file error_logger lexbuf CasanovaCompiler.Lexer.token      
  
  let current_directory = Path.GetDirectoryName(file)
            
  let entrypoint_ast = load_ast_from_file error_logger filename
  let cnv_files =
    Directory.GetFiles(current_directory, "*.cnv") |> Seq.toList
                                                   |> Seq.filter(fun file1 -> 
                                                                  System.IO.Path.GetFileName(file1) <> System.IO.Path.GetFileName(file))
                                                   |> Seq.toList                                      
  let ast_files =
    [for filename in cnv_files do
      yield load_ast_from_file error_logger filename ]

  let merged_impl = mergeCasanovaPrograms 
                      entrypoint_ast ast_files (load_ast_from_file error_logger) 
                      (fun p -> 
                        let p1 = 
                          if System.IO.Path.IsPathRooted(p) then p
                          else Path.Combine(Directory.GetCurrentDirectory(), p)
                        

                        let p = 
                          if File.Exists(p1) |> not then 
                            let p = IO.Path.GetFileName(p1)
                            if p.ToLower().StartsWith("unityengine") then
                              let compiler_folder = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)
                              Path.Combine(compiler_folder, p)
                            else
                              failwith ("0 - The referenced library does not exist. " + p)
                          else p1



                        let DLL = System.Reflection.Assembly.LoadFile(p);
                        
                        DLL.GetTypes() |> Seq.ofArray) 
                      current_directory

  let syntax_errors = error_logger.FilterLexerErrors()
  for error in syntax_errors do                  
    do Console.ForegroundColor <- ConsoleColor.Red
    do System.Console.WriteLine(error + ": syntax error: unrecognized token", ConsoleColor.Red)
  System.Console.ResetColor()

  merged_impl