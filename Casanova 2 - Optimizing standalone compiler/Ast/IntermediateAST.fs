module IntermediateAST

open Common
open StateMachinesAST
open System


type Program = 
  {
    Module    : Id
    Imports   : List<Id>
    Classes           : List<Class>
    DataDependencies  : DataDependencies
    RuleCodomains     : RuleCodomains
    
  }  
  with 
    member this.Position = 
      match this.Classes with
      | [] -> raise Position.Empty (sprintf "Program without classes. Internal error at %s(%s)" __SOURCE_FILE__ __LINE__)
      | _ -> this.Classes.Head.Position

and [<CustomComparison; CustomEquality>] Class = 
  {
    IsWorldClass  : bool
    Name          : Id
    Body          : ClassBody
  } with 
    interface IComparable<Class> with
      member this.CompareTo c1 =
          compare this.Name c1.Name
    interface IComparable with
        member this.CompareTo obj =
            match obj with
            | :? Class as other -> (this :> IComparable<_>).CompareTo other
            | _                    -> invalidArg "obj" "not a Category"
    interface IEquatable<Class> with
        member this.Equals c1 = c1.Name = this.Name
    override this.Equals obj =
        match obj with
        | :? Class as other -> (this :> IEquatable<_>).Equals other
        | _                    -> invalidArg "obj" "not a Category"
    override this.GetHashCode () = hash this.Name
    member this.Position = this.Name.idRange

and ClassBody = 
  {
    Fields                      : ResizeArray<Field>
    AtomicRules                 : ResizeArray<AutomatedRule>
    StateMachineRules           : ResizeArray<StateMachineRule>
    ParallelMethods             : ResizeArray<Id * Block>
    ConcurrentMethods           : ResizeArray<Id * Id * Id * Block>
    SuspendedRules              : ResizeArray<SuspendedRule>
    Create                      : Create
  }

and Create =
  {
    Args      : List<Id * TypeDecl>
    Body      : AtomicBlock
    Position  : Position
  }

and AutomatedRule = 
  {
    Domain    : List<Id>
    Id        : Id
    Body      : AtomicBlock
    Flag      : CasanovaCompiler.ParseAST.Flag
  } with member this.Position = this.Id.idRange

and StateMachineRule = 
  {
    Domain    : List<Id>    
    Id        : Id
    Body      : Block
    Flag      : CasanovaCompiler.ParseAST.Flag
  } with member this.Position = this.Id.idRange

and SuspendedRule = 
  {
    Id        : Id
    Domain    : List<Id>    
    Body      : Block
    Flag      : CasanovaCompiler.ParseAST.Flag
  } with member this.Position = this.Id.idRange

and [<CustomComparison; CustomEquality>] Field = 
  {
    Name              : Id
    Type              : TypeDecl
    IsStatic          : bool
    mutable UpdateNotificationsOnChange : bool
    Update            : bool    
    IsReference       : bool
    IsExternal        : Option<Id * bool * bool> 
    CodeToInjectOnSet  : string
    UpdateField   : Option<Field>
  }
  with 
    interface IComparable<Field> with
      member this.CompareTo c1 = compare this.Name c1.Name             
    interface IComparable with
        member this.CompareTo obj =
            match obj with
            | :? Field as other -> (this :> IComparable<_>).CompareTo other
            | _                    -> invalidArg "obj" "not a Category"
    interface IEquatable<Field> with
        member this.Equals c1 = (c1.Name = this.Name) && (c1.Type = this.Type)
    override this.Equals obj =
        match obj with
        | :? Field as other -> (this :> IEquatable<_>).Equals other
        | _                    -> invalidArg "obj" "not a Category"
    override this.GetHashCode () = hash this.Name

    member this.IsImportedType = this.Type.IsSystemType
    member this.Updateables = this
    member this.Position = this.Name.idRange

and RuleCodomains =
  {
    _Dependencies : ResizeArray<RuleCodomain>
    mutable _BySource      : Map<Class * Id * bool, (Class * Field) list>
    mutable _ByTarget      : Map<Class * Field, (Class * Id * bool) list>
  } with 

    member this.UpdateMaps() =
      let dict_source : System.Collections.Generic.Dictionary<Class * Id * bool, ResizeArray<Class * Field>> = System.Collections.Generic.Dictionary<Class * Id * bool, ResizeArray<Class * Field>>()
      let dict_target : System.Collections.Generic.Dictionary<Class * Field, ResizeArray<Class * Id * bool>> = System.Collections.Generic.Dictionary<Class * Field, ResizeArray<Class * Id * bool>>()
      for d in this._Dependencies do
        
        if dict_source.ContainsKey(d.SourceClass,d.SourceRule, d.IsAtomic) then
          if dict_source.[d.SourceClass,d.SourceRule, d.IsAtomic].Contains(d.Target) |> not then
            dict_source.[d.SourceClass,d.SourceRule, d.IsAtomic].Add(d.Target)
        else
          dict_source.Add((d.SourceClass,d.SourceRule, d.IsAtomic),ResizeArray())
          dict_source.[d.SourceClass,d.SourceRule, d.IsAtomic].Add(d.Target)


      this._BySource <- (dict_source |> Seq.map(fun elem -> elem.Key,(elem.Value |> Seq.toList)) |> Map.ofSeq)
      for elem in this._BySource do
        let source = elem.Key
        let targets = elem.Value
        for tc,tf in targets do
          let t = tc,tf
          for d in this._Dependencies do
            if (d.Target) = t then
              if dict_target.ContainsKey(tc,tf) then
                if dict_target.[tc,tf].Contains(d.SourceClass,d.SourceRule, d.IsAtomic) |> not then
                  dict_target.[tc,tf].Add(d.SourceClass,d.SourceRule, d.IsAtomic)
              else
                dict_target.Add((tc,tf), ResizeArray())
                dict_target.[tc,tf].Add(d.SourceClass,d.SourceRule, d.IsAtomic)
      this._ByTarget <- (dict_target |> Seq.map(fun elem -> elem.Key,(elem.Value |> Seq.toList)) |> Map.ofSeq)


    member ds.TryBySource
      with get(c:Class,r:Id) =
        if ds._BySource.ContainsKey(c,r,true) then Some ds._BySource.[c,r,true] 
        else 
          if ds._BySource.ContainsKey(c,r,false) then Some ds._BySource.[c,r,false]
          else None

    member ds.TryByTarget
      with get(c:Class,f:Field) = 
        if ds._ByTarget.ContainsKey(c,f) then Some ds._ByTarget.[c,f] else None

and DataDependencies = 
  {
    _Dependencies : ResizeArray<DataDependency>
    // given an entity and field A, returns all the entities and their rules that depend on A
    mutable _BySource      : Map<Class * Field, (Class * Id * string) list>
    // given a rule and its entity A, returns all the entities and their fields that influence A
    mutable _ByTarget      : Map<Class * Id, (Class * Field * string) list> 
  } with 
    static member PrintNotifyFieldName (sc : Class) (sf : Id) (tc : Class) tr = ("NotifySlot" + sc.Name.idText + sf.idText + tc.Name.idText + tr.idText)
    member this.Clone() =
      let dp = 
        {
          _Dependencies = ResizeArray<DataDependency>(this._Dependencies)
          _BySource      = Map.empty
          _ByTarget      = Map.empty
        } 
      dp.UpdateMaps()
      dp
    member this.UpdateMaps() =
      let dict_source : System.Collections.Generic.Dictionary<Class * Field, ResizeArray<Class * Id * string>> = System.Collections.Generic.Dictionary<Class * Field, ResizeArray<Class * Id * string>>()
      let dict_target : System.Collections.Generic.Dictionary<Class * Id, ResizeArray<Class * Field * string>> = System.Collections.Generic.Dictionary<Class * Id, ResizeArray<Class * Field * string>>()
      for d in this._Dependencies do
        
        if dict_source.ContainsKey(d.SourceClass,d.SourceField) then
          if dict_source.[d.SourceClass,d.SourceField].Contains(d.Target) |> not then
            dict_source.[d.SourceClass,d.SourceField].Add(d.Target)
        else
          dict_source.Add((d.SourceClass,d.SourceField),ResizeArray())
          dict_source.[d.SourceClass,d.SourceField].Add(d.Target)


      this._BySource <- (dict_source |> Seq.map(fun elem -> elem.Key,(elem.Value |> Seq.toList)) |> Map.ofSeq)
      for elem in this._BySource do
        let source = elem.Key
        let targets = elem.Value
        for tc,tr,str in targets do
          let t = tc,tr,str
          for d in this._Dependencies do
            if d.Target = t then
              if dict_target.ContainsKey(tc,tr) then
                if dict_target.[tc,tr].Contains(d.SourceClass,d.SourceField,str) |> not then
                  dict_target.[tc,tr].Add(d.SourceClass,d.SourceField,str)
              else
                dict_target.Add((tc,tr), ResizeArray())
                dict_target.[tc,tr].Add(d.SourceClass,d.SourceField,str)
      this._ByTarget <- (dict_target |> Seq.map(fun elem -> elem.Key,(elem.Value |> Seq.toList)) |> Map.ofSeq)

    member this.UpdateDependencies (classes : Class list) (rdp : RuleCodomains) =
      let atomic_rules = [for c in classes do 
                            for r in c.Body.AtomicRules do
                              yield c, r.Id]

      let dependencies = ResizeArray()

      for (d : DataDependency) in this.Dependencies do
        
        let sc, sf = d.SourceClass, d.SourceField
        match rdp.TryByTarget(sc, sf) with
        | None -> () //constant ?
        | Some targets ->         
          let mutable exists_atomic_rule = false
          for tc, tr, _ in targets do     
              match atomic_rules |> Seq.tryFind(fun (c,r) -> c = tc && r = tr) with
              | None -> ()
              | Some _ -> exists_atomic_rule <- true
          if exists_atomic_rule |> not then dependencies.Add(d)
      this._Dependencies.Clear()
      this._Dependencies.AddRange dependencies
      
    member this.Dependencies = this._Dependencies
    static member Empty = 
      { _Dependencies = ResizeArray()
        _BySource     = Map.empty
        _ByTarget     = Map.empty }
    member ds.IsSource(c : Class) =
      (ds._BySource |> Seq.tryFind (fun e -> fst e.Key = c) = None) |> not

    member ds.IsTarget(c : Class) =
      (ds._ByTarget |> Seq.tryFind (fun e -> fst e.Key = c) = None) |> not

    member ds.IsSourceOrTarget(c : Class) =
      ds.IsSource c || ds.IsTarget c


    member ds.TryBySource
      with get(c:Class,f:Field) =
        if ds._BySource.ContainsKey(c,f) then Some ds._BySource.[c,f] else None
    member ds.TryByTarget
      with get(c:Class,r:Id) = 
        if ds._ByTarget.ContainsKey(c,r) then Some ds._ByTarget.[c,r] else None

and DataDependency = 
  {
    SourceClass         : Class
    SourceField         : Field
    Target              : Class * Id * string
  }
and RuleCodomain =
  {
    SourceClass         : Class
    SourceRule          : Id
    IsAtomic            : bool
    Target              : Class * Field
  }
and MaybeExpr = JustExpr of TypedExpression 
                | NothingExpr of Position
and Block = List<TypedExpression>
and TypedExpression = TypeDecl * Expression
and Expression =
  | AtomicFor of Id * TypedExpression * TypedExpression * Block
  | SetExpression of TypedExpression * TypedExpression
  | Incr of TypedExpression
  
  | DoNothing of Position
  | Range of TypedExpression * TypedExpression * Position
  | Label of int * Position
  | GotoSuspend of int * Position
  | Goto of int * Position
  | Set of Id * TypedExpression
  | IndexOf of TypedExpression * TypedExpression  
  | NewEntity of List<Id * Block>
  | IfThenElse of TypedExpression * Option<TypedExpression> * Block * Block
  | DoGet of TypedExpression * Id
  | Cast of TypeDecl * TypedExpression * Position
  | IfThen of TypedExpression * Block
  | Tuple of Block
  | Query of AtomicQueryExpression
  | Call of Call
  | Literal of Literal
  | Id of Id

  | Maybe of MaybeExpr

  | Add of TypedExpression * TypedExpression
  | Sub of TypedExpression * TypedExpression
  | Div of TypedExpression * TypedExpression
  | Modulus of TypedExpression * TypedExpression 
  | Mul of TypedExpression * TypedExpression
  
  | Not of TypedExpression
  | And of TypedExpression * TypedExpression
  | Or of TypedExpression * TypedExpression
  | Equals of TypedExpression * TypedExpression
  | Greater of TypedExpression * TypedExpression
  
  | ConcatQuery of List<TypedExpression>
  | AppendToQuery of TypedExpression * TypedExpression

  with member this.Position = 
        match this with 
        | Range(_,_,p) -> p | Label (_,p) -> p; | GotoSuspend (_,p) -> p; | Goto (_,p) -> p; | Set (id,_) -> id.idRange; | IndexOf (id,_) -> (snd id).Position; 
        | NewEntity id_blocks -> 
          match id_blocks with
          | [] -> raise Position.Empty (sprintf "New entity without body. Internal error at %s(%s)" __SOURCE_FILE__ __LINE__)
          | _ ->(fst id_blocks.Head).idRange; 
        | IfThenElse (b,_,_,_) -> (snd b).Position; | IfThen (b,_) -> (snd b).Position; 
        | Tuple b -> 
          match b with
          | [] -> raise Position.Empty (sprintf "Tuple without body. Internal error at %s(%s)" __SOURCE_FILE__ __LINE__)
          | _ ->(fst b.Head).Position; 
        | Query q -> q.Position; | Call c -> c.Position; | Literal l -> l.Position; | Id id -> id.idRange

