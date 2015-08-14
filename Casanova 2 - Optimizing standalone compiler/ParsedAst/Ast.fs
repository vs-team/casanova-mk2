module CasanovaCompiler.ParseAST
//open Microsoft.FSharp.Compiler
open Microsoft.FSharp.Text.Lexing
open Microsoft.FSharp.Text.Parsing
//open Microsoft.FSharp.Compiler.PrettyNaming
//open Microsoft.FSharp.Compiler.Range

open System.Collections.Generic

exception (*internal*) RecoverableParseError
exception (*internal*) Accept of obj

let rec last l = match l with [] -> failwith "last" | [h] -> h | _::t -> last t

let parenGet = ".()"
let parenSet = ".()<-"
let qmark = "?"
let qmarkSet = "?<-"

//let a : Position = 1
//a.Column
//Microsoft.FSharp.Text.Lexing.


// Note, this is deliberately written in an allocation-free way, i.e. m1.Start, m1.End etc. are not called
let unionRanges (m1:Position) (m2:Position) = m1
//let a : ParseErrorContext<_> = 1
//
//a.ParseState.ResultRange


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

type Program = 
  { 
    ModuleStatement     : LongIdentWithDots
    WorldOrEntityDecls  : SynModuleDecls
    Range               : Position
  }
  static member Empty() =
    { 
      ModuleStatement     = LongIdentWithDots([Ident("empty", Position.Empty)], [])
      WorldOrEntityDecls  = []
      Range               = Position.Empty
    }  
and 
  [<NoEquality; NoComparison;RequireQualifiedAccess>]
  SynModuleDecl =
  | World of SynTypeDefn * Position
  | Types of SynTypeDefn list * Position
  | Open of LongIdentWithDots * Position
  | Import of string * Position
  | EntryPoint of SynAttributes * SynExpr * Position
    
//  | Let of bool * SynBinding list * Position
//  | Attributes of SynAttributes * Position
  member d.SynTypeDefn = 
      match d with 
      | SynModuleDecl.Types(s,m) -> s
      | SynModuleDecl.World(s,m) -> [s]
      | _ -> failwith "expecting entity"
  member d.Range = 
      match d with 
      | SynModuleDecl.Types(_,m)
      | Import(_, m)
      | SynModuleDecl.Open (_,m) -> m
      | EntryPoint(_,_,m) -> m

and SynModuleDecls = SynModuleDecl list

and 
    [<NoEquality; NoComparison>]
    SynTypeDefn =
    //            info tipo           campi             regole
    | TypeDefn of LongIdent * SynTypeDefnRepr * SynMemberDefns * Position
    member this.SynTypeDefnRepr =
        match this with
        | TypeDefn(_,s,_,_) -> s
    member this.Name =
        match this with
        | TypeDefn(l,_,_,_) -> l
    member this.Range =
        match this with
        | TypeDefn(_,_,_,m) -> m
    member this.SynMemberDefns =
        match this with
        | TypeDefn(_,_,s,_) -> s

and 
    [<NoEquality; NoComparison; RequireQualifiedAccess>]
    SynMemberDefn = Member of SynBinding * Position                            
    with 
      member d.GetSynBinding =
        match d with 
          | SynMemberDefn.Member(s, _) -> s
      member d.Range = 
          match d with 
          | SynMemberDefn.Member(_, m) -> m
and  
    [<NoEquality; NoComparison>]
    SynValData = 
    | SynValData of MemberFlags option * SynValInfo * Ident option
    with member d.GetMemberFlags = let (SynValData(m,_,_)) = d in m
      
and  
    [<NoEquality; NoComparison>]
    SynBinding = 
    | Binding of  
        SynPat *    //old arg 8
        SynExpr *   //old arg 10
        SynValData *
        Position       //old arg 11
    // no member just named "Range", as that would be confusing: 
    //  - for everything else, the 'Position' member that appears last/second-to-last is the 'full Position' of the whole tree construct
    //  - but for Binding, the 'Position' is only the Position of the left-hand-side, the right-hand-side Position is in the SynExpr
    //  - so we use explicit names to avoid confusion
    member x.GetSynValData = let (Binding(_,_,s,_)) = x in s
    member x.GetExpr = let (Binding(_,e,_,_)) = x in e
    member x.SynPat = let (Binding(s,_,_,_)) = x in s
    member x.GetRange = let (Binding(_,_,_,r)) = x in r
    member x.RangeOfBindingSansRhs = let (Binding(_,_,_,m)) = x in m
    member x.RangeOfBindingAndRhs = let (Binding(_,e,_,m)) = x in m

and BlockSeparator = Position * Position option
and RecordFieldName = LongIdentWithDots * bool

and LexCont = LexerWhitespaceContinuation
and LexerIfdefStackEntry = IfDefIf | IfDefElse 
and LexerIfdefStackEntries = (LexerIfdefStackEntry * Position) list
and LexerIfdefStack = LexerIfdefStackEntries ref

and  
    [<NoEquality; NoComparison; RequireQualifiedAccess>]
    SynAttribute = 
    { TypeName: LongIdentWithDots;
      ArgExpr: SynExpr 
      /// Target specifier, e.g. "assembly","module",etc.
      Target: Ident option 
      /// Is this attribute being applied to a property getter or setter?
      AppliesToGetterAndSetter: bool
      Range: Position } 

/// Specifies how the 'endline' function in the lexer should continue after
/// it reaches end of line or eof. The options are to continue with 'token' function
/// or to continue with 'skip' function.
and LexerEndlineContinuation = 
    | Token of LexerIfdefStackEntries
    | Skip of LexerIfdefStackEntries * int * Position
    member x.LexerIfdefStack = 
      match x with 
      | LexerEndlineContinuation.Token(ifd) 
      | LexerEndlineContinuation.Skip(ifd, _, _) -> ifd


/// The parser defines a number of tokens for whitespace and
/// comments eliminated by the lexer.  These carry a specification of
/// a continuation for the lexer for continued processing after we've dealt with
/// the whitespace.
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


and SequencePointInfoForWhileLoop = 
    | SequencePointAtWhileLoop of Position
    | NoSequencePointAtWhileLoop

and SequencePointInfoForForLoop = 
    | SequencePointAtForLoop of Position
    | NoSequencePointAtForLoop

and SequencePointInfoForTarget = 
    | SequencePointAtTarget
    | SuppressSequencePointAtTarget


and SeqExprOnly = 
    | SeqExprOnly of bool

and  
    [<NoEquality; NoComparison;RequireQualifiedAccess>]
    SynPat =
    | Const of SynConst * Position
    | Wild of Position
    | Named of  SynPat * Ident *  bool (* true if 'this' variable *) * Position
    | Typed of  SynPat * SynType * Position
    | Or of  SynPat * SynPat * Position
    | Ands of  SynPat list * Position
    //1 2 4 6
    | LongIdent of LongIdentWithDots * (* holds additional ident for tooling *) Ident option * SynPat list * Position
    | Tuple of  SynPat list * Position
    | Paren of  SynPat * Position
    | ArrayOrList of  bool * SynPat list * Position
    | Record of ((LongIdent * Ident) * SynPat) list * Position
    /// 'null'
    | Null of Position
    /// A pattern arising from a parse error
    | FromParseError of SynPat * Position
    member p.TryGetLongIdent =
      match p with
      |SynPat.LongIdent(l,_,_,_) -> Some l
      |_ -> None
    member p.TryGetNamed =
      match p with
      |SynPat.Named(_,l,_,_) -> Some l
      |_ -> None
    member p.TryGetTuple =
      match p with
      |SynPat.Tuple(l,_) -> Some l
      |_ -> None
    member p.Range = 
      match p with 
      | SynPat.Const(_,m) | SynPat.Wild m | SynPat.Named (_,_,_,m) | SynPat.Or (_,_,m) | SynPat.Ands (_,m) 
      | SynPat.LongIdent (_,_,_,m) | SynPat.ArrayOrList(_,_,m) | SynPat.Tuple (_,m) |SynPat.Typed(_,_,m)
      | SynPat.Record (_,m) | SynPat.Null m | SynPat.Paren(_,m) 
      | SynPat.FromParseError (_,m) -> m

and SequencePointInfoForBinding = 
    | SequencePointAtBinding of Position
    // Indicates the ommission of a sequence point for a binding for a 'do expr' 
    | NoSequencePointAtDoBinding
    // Indicates the ommission of a sequence point for a binding for a 'let e = expr' where 'expr' has immediate control flow
    | NoSequencePointAtLetBinding
    // Indicates the ommission of a sequence point for a compiler generated binding
    // where we've done a local expansion of some construct into something that involves
    // a 'let'. e.g. we've inlined a function and bound its arguments using 'let'
    // The let bindings are 'sticky' in that the inversion of the inlining would involve
    // replacing the entire expression with the original and not just the let bindings alone.
    | NoSequencePointAtStickyBinding
    // Given 'let v = e1 in e2', where this is a compiler generated binding, 
    // we are sometimes forced to generate a sequence point for the expression anyway based on its
    // overall Position. If the let binding is given the flag below then it is asserting that
    // the binding has no interesting side effects and can be totally ignored and the Position
    // of the inner expression is used instead
    | NoSequencePointAtInvisibleBinding
    
    // Don't drop sequence points when combining sequence points
    member x.Combine(y:SequencePointInfoForBinding) = 
        match x,y with 
        | SequencePointAtBinding _ as g, _  -> g
        | _, (SequencePointAtBinding _ as g)  -> g
        | _ -> x


and  
    [<NoEquality; NoComparison>]
    SynMatchClause = 
    | Clause of SynPat * SynExpr option *  SynExpr * Position * SequencePointInfoForTarget
    member this.RangeOfGuardAndRhs =
        match this with
        | Clause(_,eo,e,_,_) ->
            match eo with
            | None -> e.Range
            | Some x -> e.Range
    member this.Range =
        match this with
        | Clause(_,eo,e,m,_) ->
            match eo with
            | None -> e.Range
            | Some x -> e.Range

and ExprAtomicFlag =
    /// Says that the expression is an atomic expression, i.e. is of a form that has no whitespace unless 
    /// enclosed in parantheses, e.g. 1, "3", ident, ident.[expr] and (expr). If an atomic expression has
    /// type T, then the largest expression ending at the same Position as the atomic expression also has type T.
    | Atomic = 0
    | NonAtomic = 1

and SequencePointInfoForSeq = 
    | SequencePointsAtSeq
    // This means "suppress a in 'a;b'" and "suppress b in 'a before b'"
    | SuppressSequencePointOnExprOfSequential
    // This means "suppress b in 'a;b'" and "suppress a in 'a before b'"
    | SuppressSequencePointOnStmtOfSequential

and SynSimplePatAlternativeIdInfo = 
    /// We have not decided to use an alternative name in tha pattern and related expression 
    | Undecided of Ident
    /// We have decided to use an alternative name in tha pattern and related expression 
    | Decided of Ident

and  
    [<NoEquality; NoComparison;RequireQualifiedAccess>]
    SynExpr =

    /// F# syntax: (expr)
    ///
    /// Paren(expr, leftParenRange, rightParenRange, wholeRangeIncludingParentheses)
    ///
    /// Parenthesized expressions. Kept in AST to distinguish A.M((x,y)) 
    /// from A.M(x,y), among other things.
    | Paren of SynExpr * Position * Position option * Position  

    /// F# syntax: 1, 1.3, () etc.
    | Const of SynConst * Position

    /// F# syntax: expr : type
    | Typed of  SynExpr * SynType * Position

    /// F# syntax: e1, ..., eN
    | Tuple of  SynExpr list * Position list * Position  // "Position list" is for interstitial commas, these only matter for parsing/design-time tooling, the typechecker may munge/discard them

    /// F# syntax: [ e1; ...; en ], [| e1; ...; en |]
    | ArrayOrList of  bool * SynExpr list * Position 

    /// F# syntax: { f1=e1; ...; fn=en }
    /// SynExpr.Record((baseType, baseCtorArgs, mBaseCtor, sepAfterBase, mInherits), (copyExpr, sepAfterCopyExpr), (recordFieldName, fieldValue, sepAfterField), mWholeExpr)
    /// inherit includes location of separator (for tooling) 
    /// copyOpt contains Position of the following WITH part (for tooling)
    /// every field includes Position of separator after the field (for tooling)
    | Record of (SynType * SynExpr * Position * BlockSeparator option * Position) option * (SynExpr * BlockSeparator) option * (RecordFieldName * (SynExpr option) * BlockSeparator option) list * Position

    /// F# syntax: new C(...)
    /// The flag is true if known to be 'family' ('protected') scope 
    | New of bool * SynType * SynExpr * Position

    /// F# syntax: 'while ... do ...'
    | While of SequencePointInfoForWhileLoop * SynExpr * SynExpr * Position

    /// F# syntax: 'for i = ... to ... do ...'
    | For of SequencePointInfoForForLoop * Ident * SynExpr * bool * SynExpr * SynExpr * Position

    /// SynExpr.ForEach (spBind, seqExprOnly, isFromSource, pat, enumExpr, bodyExpr, mWholeExpr).
    ///
    /// F# syntax: 'for ... in ... do ...'
    | ForEach of SequencePointInfoForForLoop * SeqExprOnly * bool * SynPat * SynExpr * SynExpr * Position

    /// F# syntax: [ expr ], [| expr |]
    | ArrayOrListOfSeqExpr of bool * SynExpr * Position

    /// CompExpr(isArrayOrList, isNotNakedRefCell, expr)
    ///
    /// F# syntax: { expr }
    | CompExpr of bool * bool ref * SynExpr * Position

    /// F# syntax: match expr with pat1 -> expr | ... | patN -> exprN
    | Match of  SequencePointInfoForBinding * SynExpr * SynMatchClause list * bool * Position (* bool indicates if this is an exception match in a computation expression which throws unmatched exceptions *)

    /// F# syntax: do expr 
    | Do of  SynExpr * Position

    /// App(exprAtomicFlag, isInfix, funcExpr, argExpr, m)
    ///  - exprAtomicFlag: indicates if the applciation is syntactically atomic, e.g. f.[1] is atomic, but 'f x' is not
    ///  - isInfix is true for the first app of an infix operator, e.g. 1+2 becomes App(App(+,1),2), where the inner node is marked isInfix 
    ///      (or more generally, for higher operator fixities, if App(x,y) is such that y comes before x in the source code, then the node is marked isInfix=true)
    ///
    /// F# syntax: f x
    | App of ExprAtomicFlag * bool * SynExpr * SynExpr * Position

    /// TypeApp(expr, mLessThan, types, mCommas, mGreaterThan, mTypeArgs, mWholeExpr) 
    ///     "mCommas" are the ranges for interstitial commas, these only matter for parsing/design-time tooling, the typechecker may munge/discard them
    ///
    /// F# syntax: expr<type1,...,typeN>
    | TypeApp of SynExpr * Position * SynType list * Position list * Position option * Position * Position

    /// LetOrUse(isRecursive, isUse, bindings, body, wholeRange)
    ///
    /// F# syntax: let pat = expr in expr 
    /// F# syntax: let f pat1 .. patN = expr in expr 
    /// F# syntax: let rec f pat1 .. patN = expr in expr 
    /// F# syntax: use pat = expr in expr 
    | LetOrUse of bool * bool * SynBinding list * SynExpr * Position

    /// Seq(seqPoint, isTrueSeq, e1, e2, m)
    ///  isTrueSeq: false indicates "let v = a in b; v" 
    ///
    /// F# syntax: expr; expr
    | Sequential of SequencePointInfoForSeq * bool * SynExpr * SynExpr * Position 

    ///  IfThenElse(exprGuard,exprThen,optionalExprElse,spIfToThen,isFromErrorRecovery,mIfToThen,mIfToEndOfLastBranch)
    ///
    /// F# syntax: if expr then expr
    /// F# syntax: if expr then expr else expr
    | IfThenElse of SynExpr * SynExpr * SynExpr option * SequencePointInfoForBinding * bool * Position * Position

    | WaitStatement of SynExpr * Position
    | WaitUntilStatement of SynExpr * Position

    | LongIdentSet of LongIdentWithDots * SynExpr * Position    

    | YieldStatement of SynExpr * Position
    | YieldAssignment of SynExpr * SynExpr * Position

    /// F# syntax: ident
    /// Optimized representation, = SynExpr.LongIdent(false,[id],id.idRange) 
    | Ident of Ident 

    /// F# syntax: ident.ident...ident
    /// LongIdent(isOptional, longIdent, altNameRefCell, m)
    ///   isOptional: true if preceded by a '?' for an optional named parameter 
    ///   altNameRefCell: Normally 'None' except for some compiler-generated variables in desugaring pattern matching. See SynSimplePat.Id
    | LongIdent of bool * LongIdentWithDots * SynSimplePatAlternativeIdInfo ref option * Position  

    /// DotGet(expr, rangeOfDot, lid, wholeRange)
    ///
    /// F# syntax: expr.ident.ident
    | DotGet of SynExpr * Position * LongIdentWithDots * Position

    /// F# syntax: expr.[expr,...,expr] 
    | DotIndexedGet of SynExpr * SynExpr list * Position * Position

    /// F# syntax: expr :> type 
    | Upcast of  SynExpr * SynType * Position

    /// F# syntax: upcast expr
    | InferredUpcast of  SynExpr * Position

    /// F# syntax: null
    | Null of Position

    /// F# syntax: ... in ... 
    /// Computation expressions only, based on JOIN_IN token from lex filter
    | JoinIn of SynExpr * Position * SynExpr * Position 

    /// F# syntax: <implicit>
    /// Computation expressions only, implied by final "do" or "do!"
    | ImplicitZero of Position 

    /// F# syntax: yield expr 
    /// F# syntax: return expr 
    /// Computation expressions only
    | YieldOrReturn   of (bool * bool) * SynExpr * Position
  
    /// Inserted for error recovery
    | ArbitraryAfterError  of (*debugStr:*) string * Position  

    /// Inserted for error recovery
    | FromParseError  of SynExpr * Position  

    /// Inserted for error recovery when there is "expr." and missing tokens or error recovery after the dot
    | DiscardAfterMissingQualificationAfterDot  of SynExpr * Position  
    /// Get the syntactic Position of source code covered by this construct.
    member e.Range = 
        match e with 
        | SynExpr.Paren(_,_,_,m) 
        | SynExpr.Const(_,m) 
        | SynExpr.Typed (_,_,m)
        | SynExpr.Tuple (_,_,m)
        | SynExpr.ArrayOrList (_,_,m)
        | SynExpr.Record (_,_,_,m)
        | SynExpr.New (_,_,_,m)
        | SynExpr.While (_,_,_,m)
        | SynExpr.For (_,_,_,_,_,_,m)
        | SynExpr.ForEach (_,_,_,_,_,_,m)
        | SynExpr.CompExpr (_,_,_,m)
        | SynExpr.ArrayOrListOfSeqExpr (_,_,m)
        | SynExpr.Match (_,_,_,_,m)
        | SynExpr.Do (_,m)
        | SynExpr.App (_,_,_,_,m)
        | SynExpr.TypeApp (_,_,_,_,_,_,m)
        | SynExpr.LetOrUse (_,_,_,_,m)
        | SynExpr.Sequential (_,_,_,_,m)
        | SynExpr.ArbitraryAfterError(_,m)
        | SynExpr.FromParseError (_,m)
        | SynExpr.DiscardAfterMissingQualificationAfterDot (_,m) 
        | SynExpr.IfThenElse (_,_,_,_,_,_,m)
        | SynExpr.LongIdent (_,_,_,m)
        | SynExpr.DotIndexedGet (_,_,_,m)
        | SynExpr.DotGet (_,_,_,m)
        | SynExpr.Upcast (_,_,m)
        | SynExpr.JoinIn (_,_,_,m)
        | SynExpr.InferredUpcast (_,m)
        | SynExpr.Null m
        | SynExpr.ImplicitZero (m)
        | SynExpr.YieldOrReturn (_,_,m) -> m
        | SynExpr.Ident id -> id.idRange
        | SynExpr.WaitStatement (_,m) -> m
        | SynExpr.WaitUntilStatement (_,m) -> m
        | SynExpr.YieldStatement (_,m) -> m
        | SynExpr.YieldAssignment(_,_,m) -> m

    /// Position ignoring any (parse error) extra trailing dots
    member e.RangeSansAnyExtraDot = 
        match e with 
        | SynExpr.Paren(_,_,_,m) 
        | SynExpr.Const(_,m) 
        | SynExpr.Typed (_,_,m)
        | SynExpr.Tuple (_,_,m)
        | SynExpr.ArrayOrList (_,_,m)
        | SynExpr.Record (_,_,_,m)
        | SynExpr.New (_,_,_,m)
        | SynExpr.While (_,_,_,m)
        | SynExpr.For (_,_,_,_,_,_,m)
        | SynExpr.ForEach (_,_,_,_,_,_,m)
        | SynExpr.CompExpr (_,_,_,m)
        | SynExpr.ArrayOrListOfSeqExpr (_,_,m)
        | SynExpr.Match (_,_,_,_,m)
        | SynExpr.Do (_,m)
        | SynExpr.App (_,_,_,_,m)
        | SynExpr.TypeApp (_,_,_,_,_,_,m)
        | SynExpr.LetOrUse (_,_,_,_,m)
        | SynExpr.Sequential (_,_,_,_,m)
        | SynExpr.ArbitraryAfterError(_,m)
        | SynExpr.FromParseError (_,m) 
        | SynExpr.IfThenElse (_,_,_,_,_,_,m)
        | SynExpr.DotIndexedGet (_,_,_,m)
        | SynExpr.Upcast (_,_,m)
        | SynExpr.JoinIn (_,_,_,m)
        | SynExpr.InferredUpcast (_,m)
        | SynExpr.Null m
        | SynExpr.ImplicitZero (m)
        | SynExpr.YieldOrReturn (_,_,m) -> m
        | SynExpr.DotGet (expr,_,lidwd,m) -> m
        | SynExpr.LongIdent (_,lidwd,_,_) -> lidwd.RangeSansAnyExtraDot 
        | SynExpr.DiscardAfterMissingQualificationAfterDot (expr,_) -> expr.Range
        | SynExpr.Ident id -> id.idRange
        | SynExpr.WaitStatement (_,m) -> m
        | SynExpr.WaitUntilStatement (_,m) -> m
        | SynExpr.YieldStatement (_,m) -> m
        | SynExpr.YieldAssignment(_,_,m) -> m

    /// Attempt to get the Position of the first token or initial portion only - this is extremely ad-hoc, just a cheap way to improve a certain 'query custom operation' error Position
    member e.RangeOfFirstPortion = 
        match e with 
        // haven't bothered making these cases better than just .Range
        | SynExpr.Const(_,m) 
        | SynExpr.Typed (_,_,m)
        | SynExpr.Tuple (_,_,m)
        | SynExpr.ArrayOrList (_,_,m)
        | SynExpr.Record (_,_,_,m)
        | SynExpr.New (_,_,_,m)
        | SynExpr.While (_,_,_,m)
        | SynExpr.For (_,_,_,_,_,_,m)
        | SynExpr.CompExpr (_,_,_,m)
        | SynExpr.ArrayOrListOfSeqExpr (_,_,m)
        | SynExpr.Match (_,_,_,_,m)
        | SynExpr.Do (_,m)
        | SynExpr.TypeApp (_,_,_,_,_,_,m)
        | SynExpr.LetOrUse (_,_,_,_,m)
        | SynExpr.ArbitraryAfterError(_,m)
        | SynExpr.FromParseError (_,m) 
        | SynExpr.DiscardAfterMissingQualificationAfterDot (_,m) 
        | SynExpr.IfThenElse (_,_,_,_,_,_,m)
        | SynExpr.LongIdent (_,_,_,m)
        | SynExpr.DotGet (_,_,_,m)
        | SynExpr.Upcast (_,_,m)
        | SynExpr.JoinIn (_,_,_,m)
        | SynExpr.InferredUpcast (_,m)
        | SynExpr.Null m
        | SynExpr.ImplicitZero (m)
        | SynExpr.YieldOrReturn (_,_,m) -> m
        // these are better than just .Range, and also commonly applicable inside queries
        | SynExpr.Paren(_,m,_,_) -> m
        | SynExpr.Sequential (_,_,e1,_,_)
        | SynExpr.App (_,_,e1,_,_) ->
            e1.RangeOfFirstPortion 
        | SynExpr.ForEach (_,_,_,pat,_,_,m) ->
            m
        | SynExpr.Ident id -> id.idRange
        | SynExpr.WaitStatement (_,m) -> m
        | SynExpr.YieldStatement (_,m) -> m

        | SynExpr.WaitUntilStatement (_,m) -> m
        | SynExpr.YieldAssignment(_,_,m) -> m


      
and SynMemberDefns = SynMemberDefn list


and 
    [<NoEquality; NoComparison>]
    SynTypeDefnRepr = Simple of SynTypeDefnSimpleRepr * Position
    with member this.Range =
          match this with
          | Simple(_,m) -> m
         member this.SynTypeDefnSimpleRepr =
          match this with
          | Simple(s,_) -> s
 
and 
    [<NoEquality; NoComparison; RequireQualifiedAccess>]
    SynTypeDefnSimpleRepr =
    | Union      of SynUnionCases * Position
    | Record     of SynFields * Position
    member this.SynFields =
        match this with
        | Record(s,_) -> s
        | Union(_) -> []
        |_ -> failwith "expected record"
    member this.Range =
        match this with
        | Union(_,m) -> m
        | Record(_,m) -> m

and SynUnionCases = SynUnionCase list

and 
    [<NoEquality; NoComparison>]
    SynUnionCase = 
    /// The untyped, unchecked syntax tree for one case in a union definition.
    | UnionCase of Ident * SynUnionCaseType * Position
    member this.Range =
        match this with
        | UnionCase(_,_,m) -> m



and 
    [<NoEquality; NoComparison>]
    /// The untyped, unchecked syntax tree for the right-hand-side of union definition, excluding members,
    /// in either a signature or implementation.
    SynUnionCaseType = 
    /// Normal style declaration 
    | UnionCaseFields of SynField list  
    /// Full type spec given by 'UnionCase : ty1 * tyN -> rty'. Only used in FSharp.Core, otherwise a warning.
    | UnionCaseFullType of (SynType * SynValInfo)     



and SynFields = SynField list

and 
    [<NoEquality; NoComparison>]
    /// The untyped, unchecked syntax tree for a field declaration in a record or class
    /// The bool value indicate whether the field is a reference typeor not
    SynField = 
    | Field of  Ident option * SynType * bool * Position
    member this.Ident =
        match this with
        | Field(i,_,_,_) -> i
    member this.GetType =
        match this with
        | Field(_,t,_,_) -> t
    member this.IsReferenceType =
        match this with
        | Field(_,_,r,_) -> r

and 
    [<NoEquality; NoComparison>]
    SynStaticOptimizationConstraint =
    | WhenTyparTyconEqualsTycon of SynTypar *  SynType * Position
    | WhenTyparIsStruct of SynTypar * Position

and TyparStaticReq = 
    | NoStaticReq 
    | HeadTypeStaticReq 

and
  [<NoEquality; NoComparison>]
  SynTypar = 
    | Typar of Ident * TyparStaticReq * (* isCompGen: *) bool 
    with member this.Range =
            match this with
            | Typar(id,_,_) ->
                id.idRange


and 
    [<NoEquality; NoComparison;RequireQualifiedAccess>]
    /// The unchecked abstract syntax tree of F# types 
    SynType =
    /// F# syntax : A.B.C
    | LongIdent of LongIdentWithDots

    /// App(typeName, LESSm, typeArgs, commasm, GREATERm, isPostfix, m)
    ///
    /// F# syntax : type<type, ..., type> or type type or (type,...,type) type
    ///   isPostfix: indicates a postfix type application e.g. "int list" or "(int,string) dict"
    ///   commasm: ranges for interstitial commas, these only matter for parsing/design-time tooling, the typechecker may munge/discard them
    | App of SynType * Position option * SynType list * Position list * Position option * bool * Position
    /// F# syntax : type * ... * type
    // the bool is true if / rather than * follows the type
    | Tuple of (bool*SynType) list * Position    
    /// F# syntax : type[]
    | Array of  int * SynType * Position
    /// F# syntax : type -> type
    | Fun of  SynType * SynType * Position
    /// F# syntax : _
    | Anon of Position
    /// F# syntax : for units of measure e.g. m / s 
    | MeasureDivide of SynType * SynType * Position       
    /// F# syntax : for units of measure e.g. m^3 
    | MeasurePower of SynType * int * Position      
    /// F# syntax : 1, "abc" etc, used in parameters to type providers
    /// For the dimensionless units i.e. 1 , and static parameters to provided types
    | StaticConstant of SynConst * Position          
    /// Get the syntactic Position of source code covered by this construct.
    member x.PrettyPrint = 
      match x with 
      | LongIdent(l) -> l.Lid |> List.map (fun (x:Ident) -> x.idText) |> List.reduce (fun a b -> a + "." + b)
      | App(syn_type, _, syn_types, _, _, _, _) ->
        let syn_types = syn_types |> List.map (fun st -> st.PrettyPrint) |> List.reduce (fun a b -> a + "," + b)
        syn_type.PrettyPrint + "<" + syn_types + ">"
      | Tuple(syn_types, _) ->
        let syn_types = syn_types |> List.map (fun (_,st) -> st.PrettyPrint) |> List.reduce (fun a b -> a + "," + b)
        "(" + syn_types + ")"
      | Array(nesting, syn_type, _) ->
        "(" + syn_type.PrettyPrint + ")" + ([for i = 1 to nesting do yield "[]"] |> List.reduce (+))
      | StaticConstant(syn_const,_) -> syn_const.PrettyPrint
      | MeasureDivide(a,b,_) -> "(" + a.PrettyPrint + "/" + b.PrettyPrint + ")"
      | MeasurePower(a,n,_)  -> "(" + a.PrettyPrint + "^" + n.ToString() + ")"
      | Fun _ | Anon _ -> failwith "Unsupported type in record fields"
    member x.TryGetApp =
      match x with 
      | SynType.App(synType,_,synType_list,_,_,_,_) -> Some (synType, synType_list)
      | _ -> None
    member x.TryGetLongIdent =
      match x with 
        | SynType.LongIdent(l) -> Some l.Lid
        | _ -> None
    member x.Range = 
        match x with 
        | SynType.App(_,_,_,_,_,_,m) | SynType.Tuple(_,m) | SynType.Array(_,_,m) | SynType.Fun(_,_,m)
        | SynType.Anon m | SynType.StaticConstant(_,m) | SynType.MeasureDivide(_,_,m) | SynType.MeasurePower(_,_,m) -> m
        | LongIdent (lidwd) -> lidwd.Range

/// The syntactic elements associated with the "return" of a function or method. Some of this is
/// mostly dummy information to make the return element look like an argument,
/// the important thing is that (a) you can give a return type for the function or method, and 
/// (b) you can associate .NET attributes to return of a function or method and these get stored in .NET metadata.
and SynReturnInfo = SynReturnInfo of (SynType * SynArgInfo) * Position

/// The argument names and other metadata for a parameter for a member or function
and 
    [<NoEquality; NoComparison>]
    SynArgInfo = 
    | SynArgInfo of (*optional:*) bool *  Ident option
and SynAttributes = SynAttribute list
and
    [<NoEquality; NoComparison; RequireQualifiedAccess>]
    /// The unchecked abstract syntax tree of constants in F# types and expressions.
    SynConst = 
    /// F# syntax: ()
    | Unit
    /// F# syntax: true, false
    | Bool of bool
    /// F# syntax: 13uy, 0x40uy, 0oFFuy, 0b0111101uy
    | Byte of byte
    /// F# syntax: 13, 0x4000, 0o0777
    | Int32 of int32
    /// F# syntax: 1.30f, 1.40e10f etc.
    | Single of single
    /// F# syntax: 1.30, 1.40e10 etc.
    | Double of double
    /// F# syntax: 'a'
    | Char of char
    /// F# syntax: verbatim or regular string, e.g. "abc"
    | String of string * Position 
    /// Old comment: "we never iterate, so the const here is not another SynConst.Measure"
    | Measure of SynConst * SynMeasure 
    member x.PrettyPrint =
      match x with
      | Unit -> "()"
      | Bool(x) -> string x
      | Byte(x) -> string x
      | Int32(x) -> string x
      | Single(x) -> string x
      | Double(x) -> string x
      | Char(x) -> string x
      | String(a,_) -> a
      | Measure(c,m) -> "(" + c.PrettyPrint + "*" + m.PrettyPrint + ")"
    member c.Range dflt = 
        match c with 
        | SynConst.String (_,m0) -> m0 
        | _ -> dflt

and  
    [<NoEquality; NoComparison; RequireQualifiedAccess>]
    /// The unchecked abstract syntax tree of F# unit of measure annotaitons. 
    /// This should probably be merged with the represenation of SynType.
    SynMeasure = 
    | Named of LongIdent * Position
    | Product of SynMeasure * SynMeasure * Position
    | Seq of SynMeasure list * Position
    | Divide of SynMeasure * SynMeasure * Position
    | Power of SynMeasure * int * Position
    | One 
    | Anon of Position
    member x.PrettyPrint : string =
      match x with
      | Named(l,_) -> l |> List.map (fun (x:Ident) -> x.idText) |> List.reduce (fun a b -> a + "." + b)
      | Product(a,b,_) -> "(" + a.PrettyPrint + "*" + b.PrettyPrint + ")"
      | Seq _ -> failwith "Unhandled SynMeasure prettification for Seq case"
      | Divide(a,b,_) -> "(" + a.PrettyPrint + "/" + b.PrettyPrint + ")"
      | Power(a,n,_)  -> "(" + a.PrettyPrint + "^" + n.ToString() + ")"
      | One -> "1"
      | _ -> "_"



and
  [<System.Diagnostics.DebuggerDisplay("{idText}")>]
  [<Sealed>]
  [<NoEquality; NoComparison>]
  Ident (text,Position) = 
    member x.idText = text
    member x.idRange = Position
    override x.ToString() = text

and LongIdent = Ident list

and 
    [<NoEquality; NoComparison>]
    SynBindingReturnInfo = 
    | SynBindingReturnInfo of SynType * Position



and LongIdentWithDots =
    /// LongIdentWithDots(lid, dotms)   
    /// Typically dotms.Length = lid.Length-1, but they may be same if (incomplete) code ends in a dot, e.g. "Foo.Bar."
    /// The dots mostly matter for parsing, and are typically ignored by the typechecker, but 
    /// if dotms.Length = lid.Length, then the parser must have reported an error, so the typechecker is allowed
    /// more freedom about typechecking these expressions.
    /// LongIdent can be empty list - it is used to denote that name of some AST element is absent (i.e. empty type name in inherit)
    | LongIdentWithDots of LongIdent * Position list
    with member this.Range =   
            match this with
            | LongIdentWithDots([],_) -> failwith "rangeOfLidwd"
            | LongIdentWithDots([id],[]) -> id.idRange
            | LongIdentWithDots([id],[m]) -> m
            | LongIdentWithDots(h::t,[]) -> h.idRange
            | LongIdentWithDots(h::t,dotms) -> h.idRange
         member this.Lid = match this with LongIdentWithDots(lid,_) -> lid
         member this.ThereIsAnExtraDotAtTheEnd = match this with LongIdentWithDots(lid,dots) -> lid.Length = dots.Length
         member this.RangeSansAnyExtraDot =
            match this with
            | LongIdentWithDots([],_) -> failwith "rangeOfLidwd"
            | LongIdentWithDots([id],_) -> id.idRange
            | LongIdentWithDots(h::t,dotms) -> 
                let nonExtraDots = if dotms.Length = t.Length then dotms else take t.Length dotms
                h.idRange

and WorldOrEntityDecl =
  { 
    Inherit             : string list * Position
    Master              : Option<(EntityBlock * EntityBlock) list * Position>
    Slave               : Option<(EntityBlock * EntityBlock) list * Position>
    MasterConnect       : Option<(EntityBlock * EntityBlock) list * Position>
    SlaveConnect        : Option<(EntityBlock * EntityBlock) list * Position>
    MasterDisconnect    : Option<(EntityBlock * EntityBlock) list * Position>
    SlaveDisconnect     : Option<(EntityBlock * EntityBlock) list * Position>
    EntityBlock         : EntityBlock
  }

and EntityBlock = FieldDecl list * RuleDecl list

and Formals = (string * option<Type> * Position) list

and FieldDecl = 
  | VirtualDecl  of string * Option<Type> * Option<SynExpr> * Position
  | AbstractDecl of string * Option<Type> * Position

and RuleDecl =
  | VirtualDecl  of string list * SynExpr * Position
  | AbstractDecl of string list * Position

and Type =
  | Int of int * Option<MeasureType>
  | Boolean of bool
  | Float of float * Option<MeasureType>
  | Vector2 of Option<MeasureType>
  | Vector3 of Option<MeasureType>
  | String of string
  | Char of char
  | List of Type
  | Generic of string
  | Id of string

and MeasureType =
  | One of int * Position
  | Id of string * Position
  | OpMul of MeasureType * MeasureType * Position
  | OpDiv of MeasureType * MeasureType * Position
  | OpPow of MeasureType * int * Position

and FieldOrRule =
  | CnvField of SynField
  | CnvRule of SynMemberDefn
  | CnvConstructor of SynMemberDefn
  | CnvConstructorWithParams of SynMemberDefn

  member this.GetField = 
    match this with
    | CnvField(x) -> x

  member this.GetRule =
    match this with
    | CnvRule(x) -> x

and 
    [<NoComparison>]
    MemberFlags =
    { IsInstance: bool;
      IsDispatchSlot: bool;
      IsOverrideOrExplicitImpl: bool;
      IsFinal: bool;
      MemberKind: MemberKind }

/// Note the member kind is actually computed partially by a syntax tree transformation in tc.fs
and 
    [<StructuralEquality; NoComparison; RequireQualifiedAccess>]
    MemberKind = 
    | ClassConstructor
    | Constructor
    | Member 
    | PropertyGet 
    | PropertySet    
    /// An artifical member kind used prior to the point where a get/set property is split into two distinct members.
    | PropertyGetSet    

/// The argument names and other metadata for a member or function
and 
    [<NoEquality; NoComparison>]
    SynValInfo = 
    /// SynValInfo(curriedArgInfos, returnInfo)
    | SynValInfo of SynArgInfo list list * SynArgInfo 
    member x.ArgInfos = (let (SynValInfo(args,_)) = x in args)

let arbExpr(debugStr,position:Position) = SynExpr.ArbitraryAfterError(debugStr,position)

/// The error raised by the parse_error_rich function, which is called by the parser engine
/// when a syntax error occurs. The first object is the ParseErrorContext which contains a dump of
/// information about the grammar at the point where the error occured, e.g. what tokens
/// are valid to shift next at that point in the grammar. This information is processed in build.fs.
[<NoEquality; NoComparison>]
exception SyntaxError of obj (* ParseErrorContext<_> *) * Position

/// Get an F# compiler position from a lexer position
let posOfLexPosition (p:Position) = p

/// Get an F# compiler Position from a lexer Position
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

let ident (s,r) = new Ident(s,r)
let textOfId (id:Ident) = id.idText
let pathOfLid lid = List.map textOfId lid
let arrPathOfLid lid = Array.ofList (List.map textOfId lid)
let textOfPath path = String.concat "." path
let textOfArrPath path = String.concat "." (List.ofArray path)
let textOfLid lid = textOfPath (pathOfLid lid)

let rangeOfLid (lid: Ident list) = 
    match lid with 
    | [] -> failwith "rangeOfLid"
    | [id] -> id.idRange
    | h::t -> h.idRange

[<RequireQualifiedAccess>]
type ScopedPragma = 
   | WarningOff of Position * int

let mkSynBindingRhs rhsExpr retInfo =
    let rhsExpr,retTyOpt = 
        match retInfo with 
        | Some (SynReturnInfo((ty,SynArgInfo(_,_)),tym)) -> SynExpr.Typed(rhsExpr,ty,rhsExpr.Range), Some(SynBindingReturnInfo(ty,tym) )
        | None -> rhsExpr,None 
    rhsExpr,retTyOpt



let mkSynId m s = Ident(s,m)
let pathToSynLid m p = List.map (mkSynId m) p
let mkRecdField lidwd = lidwd, true

// This function is called by the generated parser code. Returning initiates error recovery 
// It must be called precisely "parse_error_rich"
let pippo l three = 
    1
// record bindings returned by the recdExprBindings rule has shape:
// (binding, separator-before-this-binding)
// this function converts arguments from form
// binding1 (binding2*sep1, binding3*sep2...) sepN
// to form
// binding1*sep1, binding2*sep2
let rebindRanges first fields lastSep = 
    let rec run (name, value) l acc = 
        match l with
        | [] -> List.rev ((name, value, lastSep)::acc)
        | (f, m)::xs -> run f xs ((name, value, m)::acc)
    run first fields []
let mkSynIdGet m n = SynExpr.Ident(mkSynId m n)
let mkSynLidGet m path n = 
    let lid = pathToSynLid m path @ [mkSynId m n]
    let dots = List.replicate (lid.Length - 1) m
    SynExpr.LongIdent(false,LongIdentWithDots(lid,dots),None,m)
let mkSynIdGetWithAlt m id altInfo = 
    match altInfo with 
    | None -> SynExpr.Ident id
    | _ -> SynExpr.LongIdent(false,LongIdentWithDots([id],[]),altInfo,m)
let mkAnonField (ty: SynType) = Field(None,ty,false,ty.Range)
let mkSynCaseName m n = [mkSynId m (CompileOpName n)]
let mkSynOperator opm oper = mkSynIdGet opm (CompileOpName oper)
let mkSynPrefix opm m oper x = SynExpr.App (ExprAtomicFlag.NonAtomic, false, mkSynOperator opm oper, x,m)
let mkSynInfix opm (l:SynExpr) oper (r:SynExpr) = 
    let firstTwoRange = l.Range
    let wholeRange = r.Range
    SynExpr.App (ExprAtomicFlag.NonAtomic, false, SynExpr.App (ExprAtomicFlag.NonAtomic, true, mkSynOperator opm oper, l, firstTwoRange), r, wholeRange)
let mkSynDotBrackSliceGet  m mDot arr (x,y) = SynExpr.DotIndexedGet(arr,[x;y],mDot,m)
let mkSynDotBrackGet  m mDot a b   = SynExpr.DotIndexedGet(a,[b],mDot,m)
let mkSynDotParenGet lhsm dotm a b   = 
    match b with
    | SynExpr.Tuple ([_;_],_,_)   -> SynExpr.Const(SynConst.Unit,lhsm)
    | SynExpr.Tuple ([_;_;_],_,_) -> SynExpr.Const(SynConst.Unit,lhsm)
    | _ -> mkSynInfix dotm a parenGet b
let mkSynUnit m = SynExpr.Const(SynConst.Unit,m)
let mkSynTrifix  m oper x1 x2 x3 = SynExpr.App (ExprAtomicFlag.NonAtomic, false, SynExpr.App (ExprAtomicFlag.NonAtomic, false, SynExpr.App (ExprAtomicFlag.NonAtomic, true, mkSynOperator m oper,x1,m), x2,m), x3,m)
let mkSynPatVar (id:Ident) = SynPat.Named (SynPat.Wild id.idRange,id,false,id.idRange)
let mkSynThisPatVar (id:Ident) = SynPat.Named (SynPat.Wild id.idRange,id,true,id.idRange)
let mkSynPatMaybeVar lidwd m =  SynPat.LongIdent (lidwd,None,[],m) 

let rec mkSynDot dotm m l r = 
    match l with 
    | SynExpr.LongIdent(isOpt,LongIdentWithDots(lid,dots),None,_) -> 
        SynExpr.LongIdent(isOpt,LongIdentWithDots(lid@[r],dots@[dotm]),None,m) // REVIEW: MEMORY PERFORMANCE: This list operation is memory intensive (we create a lot of these list nodes) - an ImmutableArray would be better here
    | SynExpr.Ident id -> 
        let i = SynExpr.LongIdent(false,LongIdentWithDots([id;r],[dotm]),None,m)
        i
    | SynExpr.DotGet(e,dm,LongIdentWithDots(lid,dots),_) -> 
        SynExpr.DotGet(e,dm,LongIdentWithDots(lid@[r],dots@[dotm]),m)// REVIEW: MEMORY PERFORMANCE: This is memory intensive (we create a lot of these list nodes) - an ImmutableArray would be better here
    | expr -> 
        SynExpr.DotGet(expr,dotm,LongIdentWithDots([r],[]),m)
let rec mkSynDotMissing dotm m l = 
    match l with 
    | SynExpr.LongIdent(isOpt,LongIdentWithDots(lid,dots),None,_) -> 
        SynExpr.LongIdent(isOpt,LongIdentWithDots(lid,dots@[dotm]),None,m) // REVIEW: MEMORY PERFORMANCE: This list operation is memory intensive (we create a lot of these list nodes) - an ImmutableArray would be better here
    | SynExpr.Ident id -> 
        SynExpr.LongIdent(false,LongIdentWithDots([id],[dotm]),None,m)
    | SynExpr.DotGet(e,dm,LongIdentWithDots(lid,dots),_) -> 
        SynExpr.DotGet(e,dm,LongIdentWithDots(lid,dots@[dotm]),m)// REVIEW: MEMORY PERFORMANCE: This is memory intensive (we create a lot of these list nodes) - an ImmutableArray would be better here
    | expr -> 
        SynExpr.DiscardAfterMissingQualificationAfterDot(expr,m)

/// Match a long identifier, including the case for single identifiers which gets a more optimized node in the syntax tree.
let (|LongOrSingleIdent|_|) inp = 
    match inp with
    | SynExpr.LongIdent(isOpt,lidwd,altId,_m) -> Some (isOpt,lidwd,altId,lidwd.RangeSansAnyExtraDot)
    | SynExpr.Ident id -> Some (false,LongIdentWithDots([id],[]),None,id.idRange)
    | _ -> None



let NonVirtualMemberFlags k = { MemberKind=k;                           IsInstance=true;  IsDispatchSlot=false; IsOverrideOrExplicitImpl=false; IsFinal=false }
let CtorMemberFlags =         { MemberKind=MemberKind.Constructor;       IsInstance=false; IsDispatchSlot=false; IsOverrideOrExplicitImpl=false; IsFinal=false }
let ClassCtorMemberFlags =    { MemberKind=MemberKind.ClassConstructor;  IsInstance=false; IsDispatchSlot=false; IsOverrideOrExplicitImpl=false; IsFinal=false }
let OverrideMemberFlags k =   { MemberKind=k;                           IsInstance=true;  IsDispatchSlot=false; IsOverrideOrExplicitImpl=true;  IsFinal=false }
let AbstractMemberFlags k =   { MemberKind=k;                           IsInstance=true;  IsDispatchSlot=true;  IsOverrideOrExplicitImpl=false; IsFinal=false }
let StaticMemberFlags k =     { MemberKind=k;                           IsInstance=false; IsDispatchSlot=false; IsOverrideOrExplicitImpl=false; IsFinal=false }

/// Operations related to the syntactic analysis of arguments of value, function and member definitions and signatures.
///
/// Function and member definitions have strongly syntactically constrained arities.  We infer
/// the arity from the syntax.
///
/// For example, we record the arity for: 
/// StaticProperty --> [1]               -- for unit arg
/// this.InstanceProperty --> [1;1]        -- for unit arg
/// StaticMethod(args) --> map InferSynArgInfoFromSimplePat args
/// this.InstanceMethod() --> 1 :: map InferSynArgInfoFromSimplePat args
/// this.InstanceProperty with get(argpat) --> 1 :: [InferSynArgInfoFromSimplePat argpat]
/// StaticProperty with get(argpat) --> [InferSynArgInfoFromSimplePat argpat]
/// this.InstanceProperty with get() --> 1 :: [InferSynArgInfoFromSimplePat argpat]
/// StaticProperty with get() --> [InferSynArgInfoFromSimplePat argpat]
/// 
/// this.InstanceProperty with set(argpat)(v) --> 1 :: [InferSynArgInfoFromSimplePat argpat; 1]
/// StaticProperty with set(argpat)(v) --> [InferSynArgInfoFromSimplePat argpat; 1]
/// this.InstanceProperty with set(v) --> 1 :: [1]
/// StaticProperty with set(v) --> [1]

module SynInfo = 
    type SynArgNameGenerator() = 
      let mutable count = 0 
      let generatedArgNamePrefix = "_arg"

      member __.New() : string = count <- count + 1; generatedArgNamePrefix + string count
      member __.Reset() = count <- 0
    [<NoEquality; NoComparison;RequireQualifiedAccess>]
    type
      /// Represents a simple set of variable bindings a, (a,b) or (a:Type,b:Type) at a lambda,
      /// function definition or other binding point, after the elimination of pattern matching
      /// from the construct, e.g. after changing a "function pat1 -> rule1 | ..." to a 
      /// "fun v -> match v with ..."
      SynSimplePats =
      | SimplePats of SynSimplePat list * Position
      | Typed of  SynSimplePats * SynType * Position
    and  
      [<NoEquality; NoComparison; RequireQualifiedAccess>]
      SynSimplePat =

      /// Id (ident, altNameRefCell, isCompilerGenerated, isThisVar, isOptArg, Position)
      ///
      /// Indicates a simple pattern variable.
      ///
      ///   altNameRefCell 
      ///     Normally 'None' except for some compiler-generated variables in desugaring pattern matching. 
      ///     Pattern processing sets this reference for hidden variable introduced by desugaring pattern matching in arguments.
      ///     The info indicates an alternative (compiler generated) identifier to be used because the name of the identifier is already bound.
      ///     See Product Studio FSharp 1.0, bug 6389.
      ///
      ///   isCompilerGenerated : true if a compiler generated name 
      ///   isThisVar: true if 'this' variable in member  
      ///   isOptArg: true if a '?' is in front of the name
      | Id of  Ident * SynSimplePatAlternativeIdInfo ref option * bool * bool *  bool * Position

      | Typed of  SynSimplePat * SynType * Position      



    /// The argument information for an argument without a name
    let unnamedTopArg1 = SynArgInfo(false,None)

    /// The argument information for a curried argument without a name
    let unnamedTopArg = [unnamedTopArg1]

    /// The argument information for a '()' argument
    let unitArgData = unnamedTopArg

    /// The 'argument' information for a return value where no attributes are given for the return value (the normal case)
    let unnamedRetVal = SynArgInfo(false,None)

    /// The 'argument' information for the 'this'/'self' parameter in the cases where it is not given explicitly
    let selfMetadata = unnamedTopArg

    /// Check if one particular argument is an optional argument. Used when adjusting the
    /// types of optional arguments for function and member signatures.
    let IsOptionalArg (SynArgInfo(isOpt,_)) = isOpt

    /// Check if there are any optional arguments in the syntactic argument information. Used when adjusting the
    /// types of optional arguments for function and member signatures.
    let HasOptionalArgs (SynValInfo(args,_)) = List.exists (List.exists IsOptionalArg) args

    /// Add a parameter entry to the syntactic value information to represent the '()' argument to a property getter. This is 
    /// used for the implicit '()' argument in property getter signature specifications.
    let IncorporateEmptyTupledArgForPropertyGetter (SynValInfo(args,retInfo)) = SynValInfo([]::args,retInfo)

    /// Add a parameter entry to the syntactic value information to represent the 'this' argument. This is 
    /// used for the implicit 'this' argument in member signature specifications.
    let IncorporateSelfArg (SynValInfo(args,retInfo)) = SynValInfo(selfMetadata::args,retInfo)

    /// Add a parameter entry to the syntactic value information to represent the value argument for a property setter. This is 
    /// used for the implicit value argument in property setter signature specifications.
    let IncorporateSetterArg (SynValInfo(args,retInfo)) = 
         let args = 
             match args with 
             | [] -> [unnamedTopArg] 
             | [arg] -> [arg@[unnamedTopArg1]] 
             | _ -> failwith "invalid setter type" 
         SynValInfo(args,retInfo)

    /// Get the argument counts for each curried argument group. Used in some adhoc places in tc.fs.
    let AritiesOfArgs (SynValInfo(args,_)) = List.map List.length args

    /// Make sure only a solitary unit argument has unit elimination
    let AdjustArgsForUnitElimination infosForArgs = 
        match infosForArgs with 
        | [[]] -> infosForArgs 
        | _ -> infosForArgs |> List.map (function [] -> unitArgData | x -> x)

    /// Transform a property declared using '[static] member P = expr' to a method taking a "unit" argument.
    /// This is similar to IncorporateEmptyTupledArgForPropertyGetter, but applies to member definitions
    /// rather than member signatures.
    let AdjustMemberArgs memFlags infosForArgs = 
        match infosForArgs with 
        | [] when memFlags=MemberKind.Member -> [] :: infosForArgs
        | _ -> infosForArgs

    let InferSynReturnData (retInfo: SynReturnInfo option) = 
        match retInfo with 
        | None -> unnamedRetVal 
        | Some(SynReturnInfo((_,retInfo),_)) -> retInfo
    /// Infer the syntactic argument info for a single argument from a simple pattern.
    let rec InferSynArgInfoFromSimplePat p = 
        match p with 
        | SynSimplePat.Id(nm,_,isCompGen,_,isOpt,_) -> 
           SynArgInfo(isOpt, (if isCompGen then None else Some nm))
        | SynSimplePat.Typed(a,_,_) -> InferSynArgInfoFromSimplePat a
    
    let appFunOpt funOpt x = match funOpt with None -> x | Some f -> f x
    let composeFunOpt funOpt1 funOpt2 = match funOpt2 with None -> funOpt1 | Some f -> Some (fun x -> appFunOpt funOpt1 (f x))

    
    /// Push non-simple parts of a patten match over onto the r.h.s. of a lambda.
    /// Return a simple pattern and a function to build a match on the r.h.s. if the pattern is complex
    let rec SimplePatOfPat (synArgNameGenerator: SynArgNameGenerator) p =
        match p with 
        | SynPat.Typed(p',ty,m) -> 
            let p2,laterf = SimplePatOfPat synArgNameGenerator p'
            SynSimplePat.Typed(p2,ty,m), 
            laterf        
        | SynPat.Named (SynPat.Wild _, v,thisv,m) -> 
            SynSimplePat.Id (v,None,false,thisv,false,m), 
            None        
        | SynPat.Paren (p,_) -> SimplePatOfPat synArgNameGenerator p 
        | SynPat.FromParseError (p,_) -> SimplePatOfPat synArgNameGenerator p 
        | _ -> 
            let m = p.Range
            let isCompGen,altNameRefCell,id,item = 
                match p with 
                | SynPat.LongIdent(LongIdentWithDots([id],_),None,[],_) -> 
                    // The pattern is 'V' or some other capitalized identifier.
                    // It may be a real variable, in which case we want to maintain its name.
                    // But it may also be a nullary union case or some other identifier.
                    // In this case, we want to use an alternate compiler generated name for the hidden variable.
                    let altNameRefCell = Some (ref (Undecided (mkSynId m (synArgNameGenerator.New()))))
                    let item = mkSynIdGetWithAlt m id altNameRefCell
                    false,altNameRefCell,id,item
                | _ -> 
                    let nm = synArgNameGenerator.New()
                    let id = mkSynId m nm
                    let item = mkSynIdGet m nm
                    true,None,id,item
            SynSimplePat.Id (id,altNameRefCell,isCompGen,false,false,id.idRange),
            Some (fun e -> 
                    let clause = Clause(p,None,e,m,SuppressSequencePointAtTarget)
                    SynExpr.Match(NoSequencePointAtInvisibleBinding,item,[clause],false,clause.Range)) 

    let rec SimplePatsOfPat synArgNameGenerator p =
        match p with 
        | SynPat.FromParseError (p,_) -> SimplePatsOfPat synArgNameGenerator p 
        | SynPat.Typed(p',ty,m) -> 
            let p2,laterf = SimplePatsOfPat synArgNameGenerator p'
            SynSimplePats.Typed(p2,ty,m), 
            laterf
    //    | SynPat.Paren (p,m) -> SimplePatsOfPat synArgNameGenerator p 
        | SynPat.Tuple (ps,m) 
        | SynPat.Paren(SynPat.Tuple (ps,m),_) -> 
            let ps2,laterf = 
              List.foldBack 
                (fun (p',rhsf) (ps',rhsf') -> 
                  p'::ps', 
                  (composeFunOpt rhsf rhsf'))
                (List.map (SimplePatOfPat synArgNameGenerator) ps) 
                ([], None)
            SynSimplePats.SimplePats (ps2,m),
            laterf
        | SynPat.Paren(SynPat.Const (SynConst.Unit,m),_) 
        | SynPat.Const (SynConst.Unit,m) -> 
            SynSimplePats.SimplePats ([],m),
            None
        | _ -> 
            let m = p.Range
            let sp,laterf = SimplePatOfPat synArgNameGenerator p
            SynSimplePats.SimplePats ([sp],m),laterf

      
    /// Infer the syntactic argument info for one or more arguments one or more simple patterns.
    let rec InferSynArgInfoFromSimplePats x = 
        match x with 
        | SynSimplePats.SimplePats(ps,_) -> List.map InferSynArgInfoFromSimplePat ps
        | SynSimplePats.Typed(ps,_,_) -> InferSynArgInfoFromSimplePats ps

    /// Infer the syntactic argument info for one or more arguments a pattern.
    let InferSynArgInfoFromPat p = 
        // It is ok to use a fresh SynArgNameGenerator here, because compiler generated names are filtered from SynArgInfo, see InferSynArgInfoFromSimplePat above
        let sp,_ = SimplePatsOfPat (SynArgNameGenerator()) p 
        InferSynArgInfoFromSimplePats sp

    /// For 'let' definitions, we infer syntactic argument information from the r.h.s. of a definition, if it
    /// is an immediate 'fun ... -> ...' or 'function ...' expression. This is noted in the F# language specification.
    /// This does not apply to member definitions.
    let InferLambdaArgs origRhsExpr = 
        let rec loop e = 
            match e with             
            | _ -> []
        loop origRhsExpr
    
    let private emptySynValInfo = SynValInfo([],unnamedRetVal)
    
    let emptySynValData = SynValData(None,emptySynValInfo,None)
    
    let InferSynValData (memberFlagsOpt, pat, retInfo, origRhsExpr) = 

        let infosForExplicitArgs = 
            match pat with 
            | Some(SynPat.LongIdent(_,_,curriedArgs,_)) -> List.map InferSynArgInfoFromPat curriedArgs
            | _ -> []

        let retInfo = InferSynReturnData retInfo

        match memberFlagsOpt with
        | None -> 
            let infosForLambdaArgs = InferLambdaArgs origRhsExpr
            let infosForArgs = infosForExplicitArgs
            let infosForArgs = AdjustArgsForUnitElimination infosForArgs 
            SynValData(None,SynValInfo(infosForArgs,retInfo),None)

        | Some memFlags  -> 
            let infosForObjArgs = 
                if memFlags.IsInstance then [ selfMetadata ] else []

            let infosForArgs = AdjustMemberArgs memFlags.MemberKind infosForExplicitArgs
            let infosForArgs = AdjustArgsForUnitElimination infosForArgs 
            
            let argInfos = infosForObjArgs @ infosForArgs
            SynValData(Some(memFlags),SynValInfo(argInfos,retInfo),None)




  


let mkCreateSynBinding headPat (mBind,retInfo,origRhsExpr,memberFlagsOpt : MemberFlags option) =
  let memberFlagsOpt =
    match memberFlagsOpt with
    | None -> None
    | Some mk ->
       { MemberFlags.IsDispatchSlot = mk.IsDispatchSlot;
         MemberFlags.IsFinal = mk.IsFinal;
         MemberFlags.IsInstance = false;
         MemberFlags.IsOverrideOrExplicitImpl = mk.IsOverrideOrExplicitImpl;      
         MemberFlags.MemberKind = mk.MemberKind;} |> Some
  let rhsExpr,_ = mkSynBindingRhs origRhsExpr retInfo
  let info = SynInfo.InferSynValData (memberFlagsOpt, Some headPat, retInfo, origRhsExpr)
  Binding (headPat,rhsExpr,info,mBind)  

let mkSynBinding headPat (mBind,retInfo,origRhsExpr,memberFlagsOpt) =
    let rhsExpr,_ = mkSynBindingRhs origRhsExpr retInfo
    let info = SynInfo.InferSynValData (memberFlagsOpt, Some headPat, retInfo, origRhsExpr)
    Binding (headPat,rhsExpr,info,mBind)  

let exprFromParseError (e:SynExpr) = SynExpr.FromParseError(e,e.Range)

let prova m e = 
  let (SynBinding.Binding(synPat, synExpr, syn_val, r)) = m
  let (SynValData.SynValData(Some mk, arg1, arg2)) = syn_val 
  let mk =
    { MemberFlags.IsDispatchSlot = mk.IsDispatchSlot;
      MemberFlags.IsFinal = mk.IsFinal;
      MemberFlags.IsInstance = false;
      MemberFlags.IsOverrideOrExplicitImpl = mk.IsOverrideOrExplicitImpl;      
      MemberFlags.MemberKind = mk.MemberKind;}
  SynValData.SynValData(Some mk, arg1, arg2)

let make_synexpr_create m = //rhsExpr retInfo m =
//  let a = mkSynPatMaybeVar (LongIdentWithDots([Ident("create",Position.Empty)],[])) Position.Empty
//  let rhsExpr,retTyOpt = 
//        match retInfo with 
//        | Some (SynReturnInfo((ty,SynArgInfo(_,_)),tym)) -> SynExpr.Typed(rhsExpr,ty,rhsExpr.Range), Some(SynBindingReturnInfo(ty,tym) )
//        | None -> rhsExpr,None 
//  let a = rhsExpr,retTyOpt

  let (SynBinding.Binding(synPat, synExpr, syn_val, r)) = m
  let (SynValData.SynValData(Some mk, arg1, arg2)) = syn_val 
  let mk =
    { MemberFlags.IsDispatchSlot = mk.IsDispatchSlot;
      MemberFlags.IsFinal = mk.IsFinal;
      MemberFlags.IsInstance = false;
      MemberFlags.IsOverrideOrExplicitImpl = mk.IsOverrideOrExplicitImpl;      
      MemberFlags.MemberKind = MemberKind.Member }
  let syn_val = SynValData.SynValData(Some mk, arg1, arg2)
  let b = SynBinding.Binding(synPat, synExpr, syn_val, r)
  b

//                BindingSetPreAttrs(letRange, isRec, isUse, builderFunction, wholeRange)
//type BindingSet = BindingSetPreAttrs of Position * bool * bool * (SynAttributes -> SynAccess option -> SynAttributes * SynBinding list) * Position
type BindingSet = BindingSetPreAttrs of Position * bool * bool *  SynBinding list * Position

let mkEntryPoint (mWhole,BindingSetPreAttrs(_,isRec,isUse,declsPreAttrs,_bindingSetRange),attrs,attrsm) = 
    //if isUse then warning(Error(FSComp.SR.parsUseBindingsIllegalInModules(),mWhole));
//    let freeAttrs,decls = declsPreAttrs attrs


//    let letDecls = [ SynModuleDecl.Let (isRec,decls,mWhole) ] 
//    let attrDecls = if freeAttrs.Length > 0 then [ SynModuleDecl.Attributes (freeAttrs,attrsm) ] else [] 
//    attrDecls @ letDecls
    let expr = declsPreAttrs.Head.GetExpr
    [SynModuleDecl.EntryPoint(attrs,expr, mWhole)]

type SynArgNameGenerator() = 
    let mutable count = 0 
    let generatedArgNamePrefix = "_arg"

    member __.New() : string = count <- count + 1; generatedArgNamePrefix + string count
    member __.Reset() = count <- 0
