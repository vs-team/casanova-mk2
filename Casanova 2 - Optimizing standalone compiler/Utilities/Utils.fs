module Internals.Utils
open Microsoft.FSharp.Text.Lexing
open Microsoft.FSharp.Text.Parsing
open System.Collections.Generic

let rec last l = match l with [] -> failwith "last" | [h] -> h | _::t -> last t

let parenGet = ".()"
let parenSet = ".()<-"
let qmark = "?"
let qmarkSet = "?<-"

let unionRanges (m1:Position) (m2:Position) = m1

let private opNameTable = 
    [ ("[]", "op_Nil");
    ("::", "op_ColonColon");
    ("+", "op_Addition");
    ("~%", "op_Splice");
    ("~%%", "op_SpliceUntyped");
    ("~++", "op_Increment");
    ("~--", "op_Decrement");
    ("-", "op_Subtraction");
    ("*", "op_Multiply");
    ("**", "op_Exponentiation");
    ("/", "op_Division");
    ("@", "op_Append");
    ("^", "op_Concatenate");
    ("%", "op_Modulus");
    ("&&&", "op_BitwiseAnd");
    ("|||", "op_BitwiseOr");
    ("^^^", "op_ExclusiveOr");
    ("<<<", "op_LeftShift");
    ("~~~", "op_LogicalNot");
    (">>>", "op_RightShift");
    ("~+", "op_UnaryPlus");
    ("~-", "op_UnaryNegation");
    ("~&", "op_AddressOf");
    ("~&&", "op_IntegerAddressOf");
    ("&&", "op_BooleanAnd");
    ("||", "op_BooleanOr");
    ("<=", "op_LessThanOrEqual");
    ("=","op_Equality");
    ("<>","op_Inequality");
    (">=", "op_GreaterThanOrEqual");
    ("<", "op_LessThan");
    (">", "op_GreaterThan");
    ("|>", "op_PipeRight");
    ("||>", "op_PipeRight2");
    ("|||>", "op_PipeRight3");
    ("<|", "op_PipeLeft");
    ("<||", "op_PipeLeft2");
    ("<|||", "op_PipeLeft3");
    ("!", "op_Dereference");
    (">>", "op_ComposeRight");
    ("<<", "op_ComposeLeft");
    ("<< >>", "op_TypedQuotationUnicode");
    ("<<| |>>", "op_ChevronsBar");
    ("<@ @>", "op_Quotation");
    ("<@@ @@>", "op_QuotationUntyped");
    ("+=", "op_AdditionAssignment");
    ("-=", "op_SubtractionAssignment");
    ("*=", "op_MultiplyAssignment");
    ("/=", "op_DivisionAssignment");
    ("..", "op_Range");
    (".. ..", "op_RangeStep"); 
    ("?", "op_Dynamic");
    ("?<-", "op_DynamicAssignment");
    (parenGet, "op_ArrayLookup");
    (parenSet, "op_ArrayAssign");
    ]

let private opCharTranslateTable =
    [ ( '>', "Greater");
    ( '<', "Less"); 
    ( '+', "Plus");
    ( '-', "Minus");
    ( '*', "Multiply");
    ( '=', "Equals");
    ( '~', "Twiddle");
    ( '%', "Percent");
    ( '.', "Dot");
    ( '$', "Dollar");
    ( '&', "Amp");
    ( '|', "Bar");
    ( '@', "At");
    ( '#', "Hash");
    ( '^', "Hat");
    ( '!', "Bang");
    ( '?', "Qmark");
    ( '/', "Divide");
    ( ':', "Colon");
    ( '(', "LParen");
    ( ',', "Comma");
    ( ')', "RParen");
    ( ' ', "Space");
    ( '[', "LBrack");
    ( ']', "RBrack"); ]

let private opCharDict = 
    let t = new Dictionary<_,_>()
    for (c,_) in opCharTranslateTable do 
        t.Add(c,1)
    t
let IsOpName (n:string) =
    let rec loop i = (i < n.Length && (opCharDict.ContainsKey(n.[i]) || loop (i+1)))
    loop 0

let CompileOpName =
        let t = Map.ofList opNameTable
        let t2 = Map.ofList opCharTranslateTable
        fun n -> 
            match t.TryFind(n) with 
            | Some(x) -> x 
            | None -> 
                if IsOpName n then 
                  let mutable r = []
                  for i = 0 to String.length n - 1 do
                      let c = n.[i]
                      let c2 = match t2.TryFind(c) with Some(x) -> x | None -> string c
                      r <- c2 :: r 
                  "op_"^(String.concat "" (List.rev r))
                else n

let take n l = 
  if n = List.length l then l else 
  let rec loop acc n l = 
      match l with
      | []    -> List.rev acc
      | x::xs -> if n<=0 then List.rev acc else loop (x::acc) (n-1) xs

  loop [] n l


[<NoEquality; NoComparison>]
exception SyntaxError of obj (* ParseErrorContext<_> *) * Position
type LexCont = LexerWhitespaceContinuation
and LexerIfdefStackEntry = IfDefIf | IfDefElse 
and LexerIfdefStackEntries = (LexerIfdefStackEntry * Position) list
and LexerIfdefStack = LexerIfdefStackEntries ref
and LexerEndlineContinuation = 
    | Token of LexerIfdefStackEntries
    | Skip of LexerIfdefStackEntries * int * Position
    member x.LexerIfdefStack = 
      match x with 
      | LexerEndlineContinuation.Token(ifd) 
      | LexerEndlineContinuation.Skip(ifd, _, _) -> ifd
and
  [<RequireQualifiedAccess>]
  [<NoComparison; NoEquality>]
  LexerWhitespaceContinuation = 
    | Token            of LexerIfdefStackEntries
    | IfDefSkip        of LexerIfdefStackEntries * int * Position
    | String           of LexerIfdefStackEntries * Position
    | VerbatimString   of LexerIfdefStackEntries * Position
    | TripleQuoteString of LexerIfdefStackEntries * Position
    | Comment          of LexerIfdefStackEntries * int * Position
    | SingleLineComment of LexerIfdefStackEntries * int * Position
    | StringInComment    of LexerIfdefStackEntries * int * Position
    | VerbatimStringInComment   of LexerIfdefStackEntries * int * Position
    | TripleQuoteStringInComment   of LexerIfdefStackEntries * int * Position
    | MLOnly            of LexerIfdefStackEntries * Position
    | EndLine           of LexerEndlineContinuation
    
    member x.LexerIfdefStack =
        match x with 
        | LexCont.Token ifd
        | LexCont.IfDefSkip (ifd,_,_)
        | LexCont.String (ifd,_)
        | LexCont.VerbatimString (ifd,_)
        | LexCont.Comment (ifd,_,_)
        | LexCont.SingleLineComment (ifd,_,_)
        | LexCont.TripleQuoteString (ifd,_)
        | LexCont.StringInComment (ifd,_,_)
        | LexCont.VerbatimStringInComment (ifd,_,_)
        | LexCont.TripleQuoteStringInComment (ifd,_,_)
        | LexCont.MLOnly (ifd,_) -> ifd
        | LexCont.EndLine endl -> endl.LexerIfdefStack



let posOfLexPosition (p:Position) = p

let mkSynRange (p1:Position) (p2: Position) = p1

type LexBuffer<'Char> with 
    member lexbuf.LexemeRange  = mkSynRange lexbuf.StartPos lexbuf.EndPos

/// Get the Position corresponding to the result of a grammar rule while it is being reduced
let lhs (parseState: IParseState) =     
    fst parseState.ResultRange
    
/// Get the Position covering two of the r.h.s. symbols of a grammar rule while it is being reduced
let rhs2 (parseState: IParseState) i j = 
    let p1 = parseState.InputStartPosition i
    let p2 = parseState.InputEndPosition j
    mkSynRange p1 p2

/// Get the Position corresponding to one of the r.h.s. symbols of a grammar rule while it is being reduced
let rhs parseState i = rhs2 parseState i i
exception (*internal*) RecoverableParseError
exception (*internal*) Accept of obj
