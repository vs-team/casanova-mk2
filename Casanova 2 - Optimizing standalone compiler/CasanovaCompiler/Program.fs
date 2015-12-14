open System
open System.IO
open CasanovaCompiler.Compiler.Importer
open System.CodeDom.Compiler
open System.Net
open System.Net.Mail
open System.Diagnostics


let send_email e =
  try
    let file_name = "last_error_report.txt"
    let prev_e = 
      if File.Exists file_name then
        File.ReadAllText file_name
      else ""

    if e <> prev_e then
      let sc = new SmtpClient("smtp.gmail.com");
      let nc = new NetworkCredential("moham.abbadi@gmail.com", "berserk1988")
      sc.UseDefaultCredentials <- false
      sc.Credentials <- nc
      sc.EnableSsl <- true
      sc.Port <- 587
      sc.Send(new MailMessage("VerSpecTeam@gmail.com", "giuseppemag@gmail.com", "Compiler error", e));
      sc.Send(new MailMessage("VerSpecTeam@gmail.com", "mabbadi@outlook.com", "Compiler error", e));
      sc.Send(new MailMessage("VerSpecTeam@gmail.com", "fdigiacom@gmail.com", "Compiler error", e))
  with
  | e -> failwith e.Message



[<EntryPoint>]
let main argv = 
  try

       
    if Common.run_debugger then
      do System.Diagnostics.Debugger.Launch() |> ignore

    let folder = 
      if Common.is_running_unity then @"Assets\"
      else System.IO.Directory.GetCurrentDirectory()

    let default_file = "Test3.cnv"
    let default_output = "..\..\..\CSharpTestProoject\csharp.cs"
  
    let file, output = 
      match argv |> Seq.toList with
      | [] -> default_file, default_output 
//      | x :: y :: [] -> x, y
      | x :: _ -> x, Directory.GetCurrentDirectory()
//      | _ -> failwith "Not supported args in command line."

    let prelude_file = 
      if Common.is_running_unity then System.IO.Path.Combine(folder, "Prelude.cs")
      else System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.IO.Path.GetFullPath(file)), "Prelude.cs")
      

    if System.IO.File.Exists(prelude_file ) |> not then
      System.IO.File.WriteAllText(prelude_file , CSharpPrelude.Prelude)





    let loadedTypes =
        let folder = 
          if Common.is_running_unity then @"Assets\"
          else 
            System.IO.Path.GetDirectoryName(System.IO.Path.Combine(output, System.IO.Path.GetFullPath(file)))
      

        let compiler_folder = 
          if Common.is_running_unity then System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)
          else System.IO.Directory.GetCurrentDirectory()



        

        let cs_files = Directory.GetFiles(folder, "*.cs")
        let cnv_files = Directory.GetFiles(folder, "*.cnv")
      
        let actual_cs_files = 
          [| for index,cs_file in cs_files |> Seq.mapi(fun i x -> (i,x)) do
                if cnv_files |> Seq.exists(fun cnv_file -> Path.GetFileNameWithoutExtension(cs_file) = Path.GetFileNameWithoutExtension(cnv_file)) |> not then
                  yield System.IO.File.ReadAllText cs_file |]
        let output_assembly =  IO.Path.Combine(compiler_folder, "internal.dll")
        if System.IO.File.Exists(output_assembly) then
          do System.IO.File.Delete(output_assembly) |> ignore
        let codeProvider = CodeDomProvider.CreateProvider("CSharp")
        let parameters = new CompilerParameters()
        parameters.GenerateExecutable <- false
        parameters.GenerateInMemory <- true
        parameters.OutputAssembly <- output_assembly


        let possible_dlls_to_add_folder = IO.Path.Combine(folder, "DllsToImport")

        if Directory.Exists(possible_dlls_to_add_folder) then 
            for dll in Directory.GetFiles(possible_dlls_to_add_folder, "*.dll") do
                //let referenced_assembly = IO.Path.Combine(possible_dlls_to_add_folder, dll)
                do parameters.ReferencedAssemblies.Add(dll) |> ignore

        if Common.is_running_unity then
          let referenced_assembly = IO.Path.Combine(compiler_folder, "UnityEngine.dll")
          let gui_assembly        = IO.Path.Combine(compiler_folder, "UnityEngine.UI.dll")
          do parameters.ReferencedAssemblies.Add(gui_assembly) |> ignore
          do parameters.ReferencedAssemblies.Add(referenced_assembly) |> ignore

        if Common.is_running_lego then
          let referenced_assembly = IO.Path.Combine(compiler_folder, "Lego.Ev3.Desktop.dll")
          do parameters.ReferencedAssemblies.Add(referenced_assembly) |> ignore

        let dlls = Directory.GetFiles(compiler_folder) |> Seq.filter(fun f -> f.ToLower().EndsWith(".dll") && f <> "internal.dll")
        let types_to_load =
          for l in dlls do
            
            do parameters.ReferencedAssemblies.Add(l) |> ignore

        do parameters.ReferencedAssemblies.Add("System.dll") |> ignore
        do parameters.ReferencedAssemblies.Add("System.Core.dll") |> ignore
                
        let res = codeProvider.CompileAssemblyFromSource(parameters, actual_cs_files)
        for e in res.Errors do
          do Console.Error.Write(e.ToString())
        let output = res.CompiledAssembly.GetTypes()
        output


    

    
    
    let asts = loadCasanovaFileOrProject file
    OpenContext.LoadCSharpClasses loadedTypes
    for ast in asts do
    
//      do System.Diagnostics.Debugger.Launch() |> ignore
      if Common.print_compilation_times then
        let time = DateTime.Now
        let basic_ast = ParsedToBasic.ConvertProgram ast

        let message = sprintf "%s \n" ("ParsedToBasic" + (DateTime.Now - time).ToString())
        let time = DateTime.Now

        let typed_ast = BasicToTyped.ConvertProgram basic_ast

        let message = message + "\n" + sprintf "%s \n" ("BasicToTyped" + (DateTime.Now - time).ToString())

        let time = DateTime.Now

        let optimized_ast = TypedToOptimizedQuery.traverseProgram typed_ast

        let message = message + "\n" + sprintf "%s \n" ("TypedToOptimizedQuery" + (DateTime.Now - time).ToString())
        let time = DateTime.Now

        let e = GotoContext.Environment()
        e.Classes <- 
          (optimized_ast.World.Name, optimized_ast.World.Body.Fields) ::
          [for e in optimized_ast.Entities do
            yield e.Name, e.Body.Fields] |> Map.ofList
        let statemachines_ast = OptimizedQueryToStateMachines.convertProgram optimized_ast e

        let message = message + "\n" + sprintf "%s \n" ("OptimizedQueryToStateMachines" + (DateTime.Now - time).ToString())
        let time = DateTime.Now

        let intermediate_ast = StateMachinesToIntermediate.convertProgram statemachines_ast

        let message = message + "\n" + sprintf "%s \n" ("StateMachinesToIntermediate" + (DateTime.Now - time).ToString())
        let time = DateTime.Now

        let csharp_ast = IntermediateToCSharp.ConvertProgram intermediate_ast

        let message = message + "\n" + sprintf "%s \n" ("IntermediateToCSharp" + (DateTime.Now - time).ToString())
        let time = DateTime.Now

        let path = System.IO.Path.GetDirectoryName(file)
        let file_name = basic_ast.World.Name.idText + ".cs"
        let output = System.IO.Path.Combine(path, file_name)

        let res = File.WriteAllText(output,csharp_ast);

        let message = message + "\n" + sprintf "%s \n" ("WriteAllText" + (DateTime.Now - time).ToString())
        File.WriteAllText(System.IO.Path.Combine(path, "compilation_time.txt"),message);
        res
      else 
        let basic_ast = ParsedToBasic.ConvertProgram ast
        let typed_ast = BasicToTyped.ConvertProgram basic_ast
        let optimized_ast = TypedToOptimizedQuery.traverseProgram typed_ast
        let e = GotoContext.Environment()
        e.Classes <- 
          (optimized_ast.World.Name, optimized_ast.World.Body.Fields) ::
          [for e in optimized_ast.Entities do
            yield e.Name, e.Body.Fields] |> Map.ofList
        let statemachines_ast = OptimizedQueryToStateMachines.convertProgram optimized_ast e
        let intermediate_ast = StateMachinesToIntermediate.convertProgram statemachines_ast
        let csharp_ast = IntermediateToCSharp.ConvertProgram intermediate_ast

        let path = System.IO.Path.GetDirectoryName(file)
        let file_name = basic_ast.World.Name.idText + ".cs"
        let output = System.IO.Path.Combine(path, file_name)

        File.WriteAllText(output,csharp_ast);


    0
  with
  
  | Common.CompilationError(Common.Position(position), error) ->
      let error_message = sprintf "Error in file %s at %d: %s" (position.FileName) (position.Line) error
      if not Common.is_running_unity then
        File.WriteAllLines("Log.txt", [error_message])
      else
          do Console.Error.Write(error_message)
      //do send_email (error_message)
      1 
  | e ->
//        do System.Diagnostics.Debugger.Launch() |> ignore
      let error_message = sprintf "Unhandled exception in Casanova compiler:\n\nException message: %s\n\n Stack trace: %s\n\n" e.Message (e.StackTrace.ToString())
//      do send_email error_message      


      let formatted_error = error_message.Replace("\r","").Split('\n')
      if not Common.is_running_unity then
        File.WriteAllLines("Log.txt", formatted_error)
      else
        for line in formatted_error do
            do Console.Error.Write(line)

      1
