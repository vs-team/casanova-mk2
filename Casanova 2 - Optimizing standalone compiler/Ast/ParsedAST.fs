module CasanovaCompiler.ParseAST

open Microsoft.FSharp.Text.Lexing
open Microsoft.FSharp.Text.Parsing
open System.Collections.Generic
open Internals.Utils


type Program = 
  { 
    ReferencedLibraries : seq<System.Type>
    ModuleStatement     : LongIdentWithDots
    WorldOrEntityDecls  : SynModuleDecls
    Range               : Position
  }
  static member Empty() =
    { 
      ReferencedLibraries = Seq.empty
      ModuleStatement     = LongIdentWithDots([Ident("empty", Position.Empty)], [])
      WorldOrEntityDecls  = []
      Range               = Position.Empty
    }  
and 
  [<NoEquality; NoComparison;RequireQualifiedAccess>]
  SynModuleDecl =
  | World of SynTypeDefn * Position
  | Types of (SynTypeDefn * bool) list * Position
  | Open of LongIdentWithDots * Position
  | OpenLibrary of string * Position
  | Import of string * Position
  | EntryPoint of SynAttributes * SynExpr * Position  
  member d.SynTypeDefn = 
      match d with 
      | SynModuleDecl.Types(s,m) -> s |> List.map(fun s -> fst s)
      | SynModuleDecl.World(s,m) -> [s]
      | _ -> Common.failwith (Common.Position d.Range) "Casanova internals. expecting entity"
  member d.Range = 
      match d with 
      | SynModuleDecl.Types(_,m)
      | Import(_, m)
      | SynModuleDecl.Open (_,m) -> m
      | EntryPoint(_,_,m) -> m
      | OpenLibrary(_, m) -> m
      | _ -> Common.failwith (Common.Position d.Range) "Casanova internals. Unexpected match."

and SynModuleDecls = SynModuleDecl list

and 
    [<NoEquality; NoComparison>]
    SynTypeDefn =
    //            info tipo           campi             regole    inherits
    | TypeDefn of LongIdent * SynTypeDefnRepr * SynMemberDefns * Ident list * Position
    member this.SynTypeDefnRepr =
        match this with
        | TypeDefn(_,s,_,_,_) -> s
    member this.Name =
        match this with
        | TypeDefn(l,_,_,_,_) -> l
    member this.Range =
        match this with
        | TypeDefn(_,_,_,_,m) -> m
    member this.SynMemberDefns =
        match this with
        | TypeDefn(_,_,s,_,_) -> s

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
    | NoSequencePointAtDoBinding
    | NoSequencePointAtLetBinding
    | NoSequencePointAtStickyBinding
    | NoSequencePointAtInvisibleBinding
    
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
    | Atomic = 0
    | NonAtomic = 1

and SequencePointInfoForSeq = 
    | SequencePointsAtSeq
    | SuppressSequencePointOnExprOfSequential
    | SuppressSequencePointOnStmtOfSequential

and SynSimplePatAlternativeIdInfo = 
    | Undecided of Ident
    | Decided of Ident

and  
    [<NoEquality; NoComparison;RequireQualifiedAccess>]
    SynExpr =
    | Paren of SynExpr * Position * Position option * Position  
    | Const of SynConst * Position
    | Typed of  SynExpr * SynType * Position
    | Tuple of  SynExpr list * Position list * Position  // "Position list" is for interstitial commas, these only matter for parsing/design-time tooling, the typechecker may munge/discard them
    | ArrayOrList of  bool * SynExpr list * Position 
    | Record of (SynType * SynExpr * Position * BlockSeparator option * Position) option * (SynExpr * BlockSeparator) option * (RecordFieldName * (SynExpr option) * BlockSeparator option) list * Position
    | New of bool * SynType * SynExpr * Position
    | While of SequencePointInfoForWhileLoop * SynExpr * SynExpr * Position
    | For of SequencePointInfoForForLoop * Ident * SynExpr * bool * SynExpr * SynExpr * Position
    | ForEach of SequencePointInfoForForLoop * SeqExprOnly * bool * SynPat * SynExpr * SynExpr * Position
    | ArrayOrListOfSeqExpr of bool * SynExpr * Position
    | CompExpr of bool * bool ref * SynExpr * Position
    | Match of  SequencePointInfoForBinding * SynExpr * SynMatchClause list * bool * Position (* bool indicates if this is an exception match in a computation expression which throws unmatched exceptions *)
    | Choice of bool * (SynExpr * SynExpr * Position) list
    /// First bool indicates if lambda originates from a method. Patterns here are always "simple" 
    /// Second bool indicates if this is a "later" part of an iterated sequence of lambdas
    ///
    /// F# syntax: fun pat -> expr
    | Lambda of  bool * bool * SynSimplePats * SynExpr * Position 
    | Parallel of (SynExpr * Position) list
    | Do of  SynExpr * Position
    | App of ExprAtomicFlag * bool * SynExpr * SynExpr * Position
    | TypeApp of SynExpr * Position * SynType list * Position list * Position option * Position * Position
    | LetOrUse of bool * bool * SynBinding list * SynExpr * Position
    | Sequential of SequencePointInfoForSeq * bool * SynExpr * SynExpr * Position 
    | IfThenElse of SynExpr * SynExpr * SynExpr option * SequencePointInfoForBinding * bool * Position * Position
    | WaitStatement of SynExpr * Position
    | WaitUntilStatement of SynExpr * Position
    | LongIdentSet of LongIdentWithDots * SynExpr * Position    
    | YieldStatement of SynExpr * Position
    | YieldAssignment of SynExpr * SynExpr * Position
    | Ident of Ident 
    | LongIdent of bool * LongIdentWithDots * SynSimplePatAlternativeIdInfo ref option * Position  
    | DotGet of SynExpr * Position * LongIdentWithDots * Position
    | DotIndexedGet of SynExpr * SynExpr list * Position * Position
    | Upcast of  SynExpr * SynType * Position
    | InferredUpcast of  SynExpr * Position
    | Null of Position
    | JoinIn of SynExpr * Position * SynExpr * Position 
    | ImplicitZero of Position 
    | YieldOrReturn   of (bool * bool) * SynExpr * Position
    | ArbitraryAfterError  of (*debugStr:*) string * Position  
    | FromParseError  of SynExpr * Position  
    | DiscardAfterMissingQualificationAfterDot  of SynExpr * Position  
    member e.Range = 
        match e with 
        | SynExpr.Paren(_,_,_,m) | SynExpr.Const(_,m) | SynExpr.Typed (_,_,m) | SynExpr.Tuple (_,_,m) | SynExpr.ArrayOrList (_,_,m)
        | SynExpr.Record (_,_,_,m) | SynExpr.New (_,_,_,m) | SynExpr.While (_,_,_,m) | SynExpr.For (_,_,_,_,_,_,m) | SynExpr.ForEach (_,_,_,_,_,_,m)
        | SynExpr.CompExpr (_,_,_,m) | SynExpr.ArrayOrListOfSeqExpr (_,_,m) | SynExpr.Match (_,_,_,_,m)  |  SynExpr.Do (_,m) | SynExpr.App (_,_,_,_,m)
        | SynExpr.TypeApp (_,_,_,_,_,_,m) | SynExpr.LetOrUse (_,_,_,_,m) | SynExpr.Sequential (_,_,_,_,m) | SynExpr.ArbitraryAfterError(_,m)
        | SynExpr.FromParseError (_,m) | SynExpr.DiscardAfterMissingQualificationAfterDot (_,m)  | SynExpr.IfThenElse (_,_,_,_,_,_,m)
        | SynExpr.LongIdent (_,_,_,m) | SynExpr.DotIndexedGet (_,_,_,m) | SynExpr.DotGet (_,_,_,m) | SynExpr.Upcast (_,_,m) | SynExpr.JoinIn (_,_,_,m)
        | SynExpr.InferredUpcast (_,m) | SynExpr.Null m | SynExpr.ImplicitZero (m) | SynExpr.YieldOrReturn (_,_,m) -> m
        | SynExpr.Ident id -> id.idRange
        | SynExpr.Choice(_,l) ->
            match l with
            |[] -> Common.raise (Common.Position Position.Empty)  (sprintf "Choice without body. Error at %s(%s)" __SOURCE_FILE__ __LINE__)
            | _ ->
              let (_,_,m) = l.Head
              m
        | SynExpr.Parallel(l) ->
            match l with
            |[] -> Common.raise (Common.Position Position.Empty)  (sprintf "Parallel without body. Error at %s(%s)" __SOURCE_FILE__ __LINE__)
            | _ ->
              let (_,m) = l.Head
              m
        | SynExpr.WaitStatement (_,m) | SynExpr.WaitUntilStatement (_,m) | SynExpr.YieldStatement (_,m) | SynExpr.YieldAssignment(_,_,m) -> m
        | _ -> Common.failwith (Common.Position e.Range) "Unexpected match."
    member e.RangeSansAnyExtraDot = 
        match e with 
        | SynExpr.Paren(_,_,_,m)  | SynExpr.Const(_,m)  | SynExpr.Typed (_,_,m) | SynExpr.Tuple (_,_,m) | SynExpr.ArrayOrList (_,_,m) | SynExpr.Record (_,_,_,m) 
        | SynExpr.New (_,_,_,m) | SynExpr.While (_,_,_,m) | SynExpr.For (_,_,_,_,_,_,m) | SynExpr.ForEach (_,_,_,_,_,_,m) | SynExpr.CompExpr (_,_,_,m)
        | SynExpr.ArrayOrListOfSeqExpr (_,_,m) | SynExpr.Match (_,_,_,_,m) |SynExpr.Do (_,m) | SynExpr.App (_,_,_,_,m) | SynExpr.TypeApp (_,_,_,_,_,_,m)
        | SynExpr.LetOrUse (_,_,_,_,m) | SynExpr.Sequential (_,_,_,_,m) | SynExpr.ArbitraryAfterError(_,m) | SynExpr.FromParseError (_,m) | SynExpr.IfThenElse (_,_,_,_,_,_,m)
        | SynExpr.DotIndexedGet (_,_,_,m) | SynExpr.Upcast (_,_,m) | SynExpr.JoinIn (_,_,_,m) | SynExpr.InferredUpcast (_,m) | SynExpr.Null m | SynExpr.ImplicitZero (m)
        | SynExpr.YieldOrReturn (_,_,m) -> m
        | SynExpr.DotGet (expr,_,lidwd,m) -> m
        | SynExpr.LongIdent (_,lidwd,_,_) -> lidwd.RangeSansAnyExtraDot 
        | SynExpr.DiscardAfterMissingQualificationAfterDot (expr,_) -> expr.Range
        | SynExpr.Ident id -> id.idRange
        | SynExpr.Choice(_,l) ->
            match l with
            |[] -> Common.raise (Common.Position Position.Empty)  (sprintf "Choice without body. Error at %s(%s)" __SOURCE_FILE__ __LINE__)
            | _ ->            
              let (_,_,m) = l.Head
              m
        | SynExpr.Parallel(l) ->
            match l with
            |[] -> Common.raise (Common.Position Position.Empty)  (sprintf "Parallel without body. Error at %s(%s)" __SOURCE_FILE__ __LINE__)
            | _ ->            
              let (_,m) = l.Head
              m
        | SynExpr.WaitStatement (_,m) | SynExpr.WaitUntilStatement (_,m) | SynExpr.YieldStatement (_,m) | SynExpr.YieldAssignment(_,_,m) -> m
        | _ -> Common.failwith (Common.Position e.Range) "Unexpected match."
    member e.RangeOfFirstPortion = 
        match e with 
        | SynExpr.Const(_,m)  | SynExpr.Typed (_,_,m) | SynExpr.Tuple (_,_,m) | SynExpr.ArrayOrList (_,_,m) | SynExpr.Record (_,_,_,m) | SynExpr.New (_,_,_,m)
        | SynExpr.While (_,_,_,m) | SynExpr.For (_,_,_,_,_,_,m) | SynExpr.CompExpr (_,_,_,m) | SynExpr.ArrayOrListOfSeqExpr (_,_,m) | SynExpr.Match (_,_,_,_,m)
        | SynExpr.Do (_,m) | SynExpr.TypeApp (_,_,_,_,_,_,m) | SynExpr.LetOrUse (_,_,_,_,m) | SynExpr.ArbitraryAfterError(_,m) | SynExpr.FromParseError (_,m) 
        | SynExpr.DiscardAfterMissingQualificationAfterDot (_,m) | SynExpr.IfThenElse (_,_,_,_,_,_,m) | SynExpr.LongIdent (_,_,_,m) | SynExpr.DotGet (_,_,_,m)
        | SynExpr.Upcast (_,_,m) | SynExpr.JoinIn (_,_,_,m) | SynExpr.InferredUpcast (_,m) | SynExpr.Null m | SynExpr.ImplicitZero (m) | SynExpr.YieldOrReturn (_,_,m) -> m
        | SynExpr.Paren(_,m,_,_) -> m
        | SynExpr.Sequential (_,_,e1,_,_) | SynExpr.App (_,_,e1,_,_) -> e1.RangeOfFirstPortion 
        | SynExpr.ForEach (_,_,_,pat,_,_,m) -> m
        | SynExpr.Ident id -> id.idRange
        | SynExpr.Choice(_,l) ->
            match l with
            |[] -> Common.raise (Common.Position Position.Empty)  (sprintf "Choice without body. Error at %s(%s)" __SOURCE_FILE__ __LINE__)
            | _ ->            
              let (_,_,m) = l.Head
              m
        | SynExpr.Parallel(l) ->
            match l with
            |[] -> Common.raise (Common.Position Position.Empty)  (sprintf "Parallel without body. Error at %s(%s)" __SOURCE_FILE__ __LINE__)
            | _ ->            
              let (_,m) = l.Head
              m
        | SynExpr.WaitStatement (_,m) | SynExpr.YieldStatement (_,m) | SynExpr.WaitUntilStatement (_,m)  | SynExpr.YieldAssignment(_,_,m) -> m
        | _ -> Common.failwith (Common.Position e.Range) "Unexpected match."

and SynMemberDefns = SynMemberDefn list    
and [<NoEquality; NoComparison;RequireQualifiedAccess>]
  SynSimplePats =
  | SimplePats of SynSimplePat list * Position
  | Typed of  SynSimplePats * SynType * Position
and  
  [<NoEquality; NoComparison; RequireQualifiedAccess>]
  SynSimplePat =
  | Id of  Ident * SynSimplePatAlternativeIdInfo ref option * bool * bool *  bool * Position
  | Typed of  SynSimplePat * SynType * Position

  with member this.Range =
          match this with
          | Id (_,_,_,_,_,position) -> position
          | Typed (_,_,position) -> position

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
    member this.Range =
        match this with
        | Union(_,m) -> m
        | Record(_,m) -> m

and SynUnionCases = SynUnionCase list

and 
    [<NoEquality; NoComparison>]
    SynUnionCase = 
    | UnionCase of Ident * SynUnionCaseType * Position
    member this.Range = match this with | UnionCase(_,_,m) -> m

and 
    [<NoEquality; NoComparison>]
    SynUnionCaseType = 
    | UnionCaseFields of SynField list  
    | UnionCaseFullType of (SynType * SynValInfo)     

and SynFields = SynField list
and 
    [<NoEquality; NoComparison>]
    SynField = 
    | Field of  Ident option * SynType * bool * Position
    member this.Ident = match this with | Field(i,_,_,_) -> i
    member this.GetType = match this with  | Field(_,t,_,_) -> t
    member this.IsReferenceType = match this with  | Field(_,_,r,_) -> r

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
    with member this.Range = match this with | Typar(id,_,_) -> id.idRange

and 
    [<NoEquality; NoComparison;RequireQualifiedAccess>]
    SynType =
    | LongIdent of LongIdentWithDots
    | App of SynType * Position option * SynType list * Position list * Position option * bool * Position
    | Tuple of (bool*SynType) list * Position    
    | Array of  int * SynType * Position
    | Fun of  SynType * SynType * Position
    | Anon of Position
    | MeasureDivide of SynType * SynType * Position       
    | MeasurePower of SynType * int * Position      
    | StaticConstant of SynConst * Position          
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
      | Fun _ | Anon _ -> Common.failwith (Common.Position x.Range) "Unsupported type in record fields"
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
and SynReturnInfo = SynReturnInfo of (SynType * SynArgInfo) * Position
and 
    [<NoEquality; NoComparison>]
    SynArgInfo = 
    | SynArgInfo of (*optional:*) bool *  Ident option
and SynAttributes = SynAttribute list
and
    [<NoEquality; NoComparison; RequireQualifiedAccess>]
    SynConst = 
    | Unit
    | Bool of bool
    | Byte of byte
    | Int32 of int32
    | Single of single
    | Double of double
    | Char of char
    | String of string * Position 
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
      | Seq (_,p) -> Common.failwith (Common.Position p) "Unhandled SynMeasure prettification for Seq case"
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
    | LongIdentWithDots of LongIdent * Position list
    with member this.Range =   
            match this with
            | LongIdentWithDots([],_) -> Common.failwith (Common.Position Position.Empty) "rangeOfLidwd"
            | LongIdentWithDots([id],[]) -> id.idRange
            | LongIdentWithDots([id],[m]) -> m
            | LongIdentWithDots(h::t,[]) -> h.idRange
            | LongIdentWithDots(h::t,dotms) -> h.idRange         
         member this.Lid = match this with LongIdentWithDots(lid,_) -> lid
         member this.ThereIsAnExtraDotAtTheEnd = match this with LongIdentWithDots(lid,dots) -> lid.Length = dots.Length
         member this.RangeSansAnyExtraDot =
            match this with
            | LongIdentWithDots([],_) -> Common.failwith (Common.Position Position.Empty) "rangeOfLidwd"
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
  | CnvInherit of Ident list
  | CnvConstructor of SynMemberDefn
  | CnvConstructorWithParams of SynMemberDefn
  member this.Position =
    match this with
    | CnvField f -> f.GetType.Range
    | CnvRule r -> r.Range
    | CnvInherit i -> 
      if i.Length > 0 then i.Head.idRange
      else Position.Empty
    | CnvConstructor m -> m.Range
    | CnvConstructorWithParams m -> m.Range
   
  member this.GetField = 
    match this with
    | CnvField(x) -> x
    | _ -> Common.failwith (Common.Position this.Position) "Unexpected match."

  member this.GetInherit = 
    match this with
    | CnvInherit(x) -> x
    | _ -> Common.failwith (Common.Position this.Position) "Unexpected match."

  member this.GetRule =
    match this with
    | CnvRule(x) -> x
    | _ -> Common.failwith (Common.Position this.Position) "Unexpected match."

and 
    [<NoComparison>]
    MemberFlags =
    { IsInstance: bool;
      IsDispatchSlot: bool;
      IsOverrideOrExplicitImpl: bool;
      IsFinal: bool;
      MemberKind: MemberKind }
and 
    [<StructuralEquality; NoComparison; RequireQualifiedAccess>]
    MemberKind = 
    | ClassConstructor
    | Constructor
    | Member 
    | PropertyGet 
    | PropertySet    
    | PropertyGetSet    
and 
    [<NoEquality; NoComparison>]
    SynValInfo = 
    | SynValInfo of SynArgInfo list list * SynArgInfo 
    member x.ArgInfos = (let (SynValInfo(args,_)) = x in args)

let arbExpr(debugStr,position:Position) = SynExpr.ArbitraryAfterError(debugStr,position)


let ident (s,r) = new Ident(s,r)
let textOfId (id:Ident) = id.idText
let pathOfLid lid = List.map textOfId lid
let arrPathOfLid lid = Array.ofList (List.map textOfId lid)
let textOfPath path = String.concat "." path
let textOfArrPath path = String.concat "." (List.ofArray path)
let textOfLid lid = textOfPath (pathOfLid lid)

let rangeOfLid (lid: Ident list) = 
    match lid with 
    | [] -> Common.failwith (Common.Position Position.Empty) "rangeOfLid"
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


module SynInfo = 
    type SynArgNameGenerator() = 
      let mutable count = 0 
      let generatedArgNamePrefix = "_arg"

      member __.New() : string = count <- count + 1; generatedArgNamePrefix + string count
      member __.Reset() = count <- 0
 

    let unnamedTopArg1 = SynArgInfo(false,None)
    let unnamedTopArg = [unnamedTopArg1]
    let unitArgData = unnamedTopArg
    let unnamedRetVal = SynArgInfo(false,None)
    let selfMetadata = unnamedTopArg
    let IsOptionalArg (SynArgInfo(isOpt,_)) = isOpt
    let HasOptionalArgs (SynValInfo(args,_)) = List.exists (List.exists IsOptionalArg) args
    let IncorporateEmptyTupledArgForPropertyGetter (SynValInfo(args,retInfo)) = SynValInfo([]::args,retInfo)
    let IncorporateSelfArg (SynValInfo(args,retInfo)) = SynValInfo(selfMetadata::args,retInfo)
    let IncorporateSetterArg (SynValInfo(args,retInfo)) = 
         let args = 
             match args with 
             | [] -> [unnamedTopArg] 
             | [arg] -> [arg@[unnamedTopArg1]] 
             | _ -> Common.failwith (Common.Position Position.Empty) "invalid setter type" 
         SynValInfo(args,retInfo)
    let AritiesOfArgs (SynValInfo(args,_)) = List.map List.length args
    let AdjustArgsForUnitElimination infosForArgs = 
        match infosForArgs with 
        | [[]] -> infosForArgs 
        | _ -> infosForArgs |> List.map (function [] -> unitArgData | x -> x)
    let AdjustMemberArgs memFlags infosForArgs = 
        match infosForArgs with 
        | [] when memFlags=MemberKind.Member -> [] :: infosForArgs
        | _ -> infosForArgs

    let InferSynReturnData (retInfo: SynReturnInfo option) = 
        match retInfo with 
        | None -> unnamedRetVal 
        | Some(SynReturnInfo((_,retInfo),_)) -> retInfo
    let rec InferSynArgInfoFromSimplePat p = 
        match p with 
        | SynSimplePat.Id(nm,_,isCompGen,_,isOpt,_) -> 
           SynArgInfo(isOpt, (if isCompGen then None else Some nm))
        | SynSimplePat.Typed(a,_,_) -> InferSynArgInfoFromSimplePat a
    
    let appFunOpt funOpt x = match funOpt with None -> x | Some f -> f x
    let composeFunOpt funOpt1 funOpt2 = match funOpt2 with None -> funOpt1 | Some f -> Some (fun x -> appFunOpt funOpt1 (f x))
    let rec SimplePatOfPat p =
        match p with 
        | SynPat.Typed(p',ty,m) -> 
            let p2,laterf = SimplePatOfPat p'
            SynSimplePat.Typed(p2,ty,m), 
            laterf        
        | SynPat.Named (SynPat.Wild _, v,thisv,m) -> 
            SynSimplePat.Id (v,None,false,thisv,false,m), None        
        | SynPat.Paren (p,_) -> SimplePatOfPat p 
        | SynPat.FromParseError (p,_) -> SimplePatOfPat p 
        | SynPat.LongIdent(LongIdentWithDots([p],_),None,[],m) ->
            SynSimplePat.Id (p,None,false,false,false,m), None        
        | _ -> 
            let m = p.Range
            let isCompGen,altNameRefCell,id,item = 
                match p with 
                | SynPat.LongIdent(LongIdentWithDots([id],_),None,[],p) -> Common.raise (Common.Position p) "Internal error"
//                    let altNameRefCell = Some (ref (Undecided (mkSynId m (synArgNameGenerator.New()))))
//                    let item = mkSynIdGetWithAlt m id altNameRefCell
//                    false,altNameRefCell,id,item
                | _ -> Common.raise (Common.Position Position.Empty) "Internal error"
//                    let nm = synArgNameGenerator.New()
//                    let id = mkSynId m nm
//                    let item = mkSynIdGet m nm
//                    true,None,id,item
            SynSimplePat.Id (id,altNameRefCell,isCompGen,false,false,id.idRange),
            Some (fun e -> 
                    let clause = Clause(p,None,e,m,SuppressSequencePointAtTarget)
                    SynExpr.Match(NoSequencePointAtInvisibleBinding,item,[clause],false,clause.Range)) 

    let rec SimplePatsOfPat p =
        match p with 
        | SynPat.FromParseError (p,_) -> SimplePatsOfPat p 
        | SynPat.Typed(p',ty,m) -> 
            let p2,laterf = SimplePatsOfPat p'
            SynSimplePats.Typed(p2,ty,m), 
            laterf
        | SynPat.Tuple (ps,m) 
        | SynPat.Paren(SynPat.Tuple (ps,m),_) -> 
            let ps2,laterf = 
              List.foldBack 
                (fun (p',rhsf) (ps',rhsf') -> 
                  p'::ps', 
                  (composeFunOpt rhsf rhsf'))
                (List.map (SimplePatOfPat ) ps) 
                ([], None)
            SynSimplePats.SimplePats (ps2,m),
            laterf
        | SynPat.Paren(SynPat.Const (SynConst.Unit,m),_) 
        | SynPat.Const (SynConst.Unit,m) -> 
            SynSimplePats.SimplePats ([],m),
            None
        | _ -> 
            let m = p.Range
            let sp,laterf = SimplePatOfPat p
            SynSimplePats.SimplePats ([sp],m),laterf
    let rec InferSynArgInfoFromSimplePats x = 
        match x with 
        | SynSimplePats.SimplePats(ps,_) -> List.map InferSynArgInfoFromSimplePat ps
        | SynSimplePats.Typed(ps,_,_) -> InferSynArgInfoFromSimplePats ps
    let InferSynArgInfoFromPat p = 
        let sp,_ = SimplePatsOfPat p 
        InferSynArgInfoFromSimplePats sp
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

/// "fun (UnionCase x) (UnionCase y) -> body" 
///       ==> 
///   "fun tmp1 tmp2 -> 
///        let (UnionCase x) = tmp1 in 
///        let (UnionCase y) = tmp2 in 
///        body" 
let PushCurriedPatternsToExpr wholem isMember pats rhs =
    // Two phases
    // First phase: Fold back, from right to left, pushing patterns into r.h.s. expr
    let spatsl,rhs = 
        (pats, ([],rhs)) 
           ||> List.foldBack (fun arg (spatsl,body) -> 
              let spats,bodyf = SynInfo.SimplePatsOfPat arg
              // accumulate the body. This builds "let (UnionCase y) = tmp2 in body"
              let body = SynInfo.appFunOpt bodyf body
              // accumulate the patterns
              let spatsl = spats::spatsl
              (spatsl,body))
    // Second phase: build lambdas. Mark subsequent ones with "true" indicating they are part of an iterated sequence of lambdas
    let expr = 
        match spatsl with
        | [] -> rhs
        | h::t -> 
            let expr = List.foldBack (fun spats e -> SynExpr.Lambda (isMember,true,spats, e,wholem)) t rhs
            let expr = SynExpr.Lambda (isMember,false,h, expr,wholem)
            expr
    spatsl,expr

let mkSynFunMatchLambdas isMember wholem ps e = 
    let _,e =  PushCurriedPatternsToExpr wholem isMember ps e 
    e

//let prova m e = 
//  let (SynBinding.Binding(synPat, synExpr, syn_val, r)) = m
//  let (SynValData.SynValData(Some mk, arg1, arg2)) = syn_val 
//  let mk =
//    { MemberFlags.IsDispatchSlot = mk.IsDispatchSlot;
//      MemberFlags.IsFinal = mk.IsFinal;
//      MemberFlags.IsInstance = false;
//      MemberFlags.IsOverrideOrExplicitImpl = mk.IsOverrideOrExplicitImpl;      
//      MemberFlags.MemberKind = mk.MemberKind;}
//  SynValData.SynValData(Some mk, arg1, arg2)

let make_synexpr_create m = //rhsExpr retInfo m =
  let (SynBinding.Binding(synPat, synExpr, syn_val, r)) = m
  match syn_val with
  | SynValData.SynValData(Some mk, arg1, arg2) ->
    let mk =
      { MemberFlags.IsDispatchSlot = mk.IsDispatchSlot;
        MemberFlags.IsFinal = mk.IsFinal;
        MemberFlags.IsInstance = false;
        MemberFlags.IsOverrideOrExplicitImpl = mk.IsOverrideOrExplicitImpl;      
        MemberFlags.MemberKind = MemberKind.Member }
    let syn_val = SynValData.SynValData(Some mk, arg1, arg2)
    let b = SynBinding.Binding(synPat, synExpr, syn_val, r)
    b
  | SynValData.SynValData(None, arg1, arg2) ->
     Common.failwith (Common.Position Position.Empty) "Unexpected case"

//                BindingSetPreAttrs(letRange, isRec, isUse, builderFunction, wholeRange)
//type BindingSet = BindingSetPreAttrs of Position * bool * bool * (SynAttributes -> SynAccess option -> SynAttributes * SynBinding list) * Position
type BindingSet = BindingSetPreAttrs of Position * bool * bool *  SynBinding list * Position
let mkEntryPoint (mWhole,BindingSetPreAttrs(_,isRec,isUse,declsPreAttrs,_bindingSetRange),attrs,attrsm) =
    match declsPreAttrs with
    |[] -> Common.raise (Common.Position Position.Empty)  (sprintf "Internal error at %s(%s)" __SOURCE_FILE__ __LINE__)
    | _ ->             
      let expr = declsPreAttrs.Head.GetExpr
      [SynModuleDecl.EntryPoint(attrs,expr, mWhole)]
type SynArgNameGenerator() = 
    let mutable count = 0 
    let generatedArgNamePrefix = "_arg"
    member __.New() : string = count <- count + 1; generatedArgNamePrefix + string count
    member __.Reset() = count <- 0


// This function is called by the generated parser code. Returning initiates error recovery 
// It must be called precisely "parse_error_rich"
let parse_error_rich = Some (fun (ctxt: ParseErrorContext<_>) ->
    printfn "parse error rich" 
    Common.raise (Common.Position (snd ctxt.ParseState.ResultRange)) "Parse error"
    let r = lhs ctxt.ParseState
    let stringError = (System.IO.Path.GetFileName(r.FileName)) + "<" + (string r.Line) + "," + (string r.Column) + ">"
    Microsoft.FSharp.Compiler.ErrorLogger.errorRrich(SyntaxError(box ctxt, fst ctxt.ParseState.ResultRange), stringError))