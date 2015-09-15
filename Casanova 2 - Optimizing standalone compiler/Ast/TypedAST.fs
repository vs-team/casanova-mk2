module TypedAST
open System
open Common

type [<CustomComparison; CustomEquality>] Id = 
  { Id : Common.Id 
    Tp : TypeDecl list } with 
  static member buildFrom (id : Common.Id) = 
      { Id = id
        Tp = [] } 
  static member buildFrom (id : Common.Id, tps) = 
      { Id = id
        Tp = tps } 
  member this.idRange = this.Id.idRange
  member this.idText = this.Id.idText
  override this.ToString() = "Id: " + this.idText + "  Pos: " + this.idRange.ToString()
            
  interface IComparable<Id> with
    member this.CompareTo id' =
        compare this.Id id'.Id
  interface IComparable with
      member this.CompareTo obj =
          match obj with
          | :? Id as other -> (this :> IComparable<_>).CompareTo other
          | _                    -> invalidArg "obj" "not a Category"
  interface IEquatable<Id> with
      member this.Equals id' = this.Id = id'.Id
  override this.Equals obj =
      match obj with
      | :? Id as other -> (this :> IEquatable<_>).Equals other
      | _                    -> invalidArg "obj" "not a Category"
  override this.GetHashCode () = hash this.idText


and Program = 
  {
    Module    : Id
    Imports   : List<Id>
    World     : World
    Entities  : List<Entity>
  } with member this.Position = this.World.Name.idRange

and World = 
  {
    Name      : Id
    Body      : EntityBody
  } with member this.Position = this.Name.idRange

and Entity = 
  {
    Name      : Id
    Body      : EntityBody
  } with member this.Position = this.Name.idRange


and EntityBody = 
  {
    Fields    : Map<Id, Field>
    Rules     : List<Rule>
    Create    : Create
  }
  

and Create =
  {
    Args      : List<Id * TypeDecl>
    mutable Body      : Block
    Position  : Position
  }

and Field = 
  {
    Name      : Id
    IsStatic  : bool
    Type      : TypeDecl    
    mutable UpdateNotificationsOnChange : bool
    IsReference   : bool
    IsExternal    : Option<Id * bool * bool> 
    mutable CodeToInjectOnSet  : string
    mutable QueryOptimized     : bool
    mutable IsQueryIndex       : bool
    UpdateField   : Option<Field>
  } with member this.Position = this.Name.idRange

and GenericType =
  { mutable Type : Option<TypeDecl> 
    mutable AlreadyAssigned : bool}


and [<CustomComparison; CustomEquality>] MaybeType =
  | Just of TypeDecl 
  | Nothing of Ref<GenericType> * Position
  member this.Position = 
    match this with 
    | Just t -> t.Position
    | Nothing(_,p) -> p
  interface IComparable<MaybeType> with
    member this.CompareTo t =
        match this, t with
        | Just(t_d1), Just(t_d2) when t_d1 <> t_d2 -> -1
        | Nothing(t_p,_), Just(t_d2)
        | Just(t_d2), Nothing(t_p,_) when t_p.Value.Type.IsNone  -> 
          t_p.Value.Type <- Some t_d2
          0
        | Nothing(t_p1,_), Nothing(t_p2,_) ->
          match t_p1.Value.Type, t_p2.Value.Type with
          | Some t1, Some t2 when t1 <> t2 -> -1
          | None, Some t2 ->
            t_p1 := !t_p2
            0
          | Some t1, None -> 
            t_p2 := !t_p2
            0
          | None, None ->
            if (!t_p1).AlreadyAssigned then
              t_p2 := !t_p1
            elif (!t_p2).AlreadyAssigned then
              t_p1 := !t_p2
            else
              let t = { Type = None; AlreadyAssigned = true}
              t_p1 := t
              t_p2 := !t_p1
            0
          | _ -> 0
        | _ -> 0
            
          



  interface IComparable with
      member this.CompareTo obj =
          match obj with
          | :? MaybeType as other -> (this :> IComparable<_>).CompareTo other
          | _                    -> invalidArg "obj" "not a Category"
  interface IEquatable<MaybeType> with
      member this.Equals t = 
        (this :> IComparable<_>).CompareTo t = 0

  override this.Equals obj =
      match obj with
      | :? MaybeType as other ->(this :> IComparable<_>).CompareTo other = 0
      | _                     -> invalidArg "obj" "not a Category"
  override this.GetHashCode () = hash this



and [<CustomComparison; CustomEquality>] TypeDecl = 
  EntityName of Id 
  | ImportedType of System.Type * Position
  | UnionType of List<TypeDecl> 
  | Tuple of List<TypeDecl> 
  | Query of TypeDecl 
  | MaybeQuery of MaybeType 
  | MaybeType of MaybeType
  | Unit of Position
  | GenericType of TypeDecl * List<TypeDecl>
  with 
    member this.Rank = 
      match this with
      | EntityName _ -> 0
      | ImportedType _ -> 1
      | UnionType _ -> 2
      | Tuple _ -> 3
      | Query _ -> 4
      | MaybeType _ -> 5
      | Unit _ -> 6
      | GenericType _ -> 7
      | MaybeQuery _ -> 8


    interface IComparable<TypeDecl> with
      member this.CompareTo t = 
        match this, t with
        | ImportedType(t, _), _ when t.Name.ToLower() = "object" -> 0
        | _, ImportedType(t, _) when t.Name.ToLower() = "object" -> 0

        | EntityName a, EntityName b -> compare a b
        | ImportedType(t1, _), ImportedType(t2, _)  when t1.GetInterface("IEnumerable`1") <> null && t1.Name.ToLower() <> "string" &&
                                                         t2.GetInterface("IEnumerable`1") <> null && t2.Name.ToLower() <> "string" ->
          let generic_arguments1 = t1.GetGenericArguments()
          let generic_arguments2 = t2.GetGenericArguments()
          compare (ImportedType(generic_arguments1.[0], Position.Empty)) (ImportedType(generic_arguments2.[0], Position.Empty)) 
        | ImportedType(t1, _), ImportedType(t2, _)  when t1.Name.StartsWith("Tuple`") &&
                                                         t2.Name.StartsWith("Tuple`") ->
          let generic_arguments1 = t1.GetGenericArguments()
          let generic_arguments2 = t2.GetGenericArguments()
          if Seq.forall(fun (t1, t2) -> ImportedType(t1, Position.Empty) = ImportedType(t2, Position.Empty)) (Seq.zip generic_arguments1 generic_arguments2) then 0
          else compare this.Rank t.Rank

        | ImportedType(t, _), ImportedType(t', _) -> compare t.FullName t'.FullName
        | MaybeType(a), MaybeType(b) -> compare a b
        
        | MaybeQuery(a), MaybeQuery(b) -> compare a b        
        | Query(MaybeQuery(a)), Query(MaybeQuery(b)) -> compare a b
        
        | Query(MaybeQuery(a)), Query(b) -> compare a (Just(b))
        | Query(b), Query(MaybeQuery(a)) -> compare a (Just(b))

        | UnionType a, UnionType b -> compare a b
        | Tuple a, ImportedType(t1, _) when t1.Name.StartsWith("Tuple`") ->           
          let generic_arguments = t1.GetGenericArguments()
          if Seq.forall(fun (t1, t2) -> t1 = ImportedType(t2, Position.Empty)) (Seq.zip a generic_arguments) then 0
          else compare this.Rank t.Rank

        | ImportedType(t1, _), Tuple a when t1.Name.StartsWith("Tuple`")->           
          let generic_arguments = t1.GetGenericArguments()
          if Seq.forall(fun (t1, t2) -> t1 = ImportedType(t2, Position.Empty)) (Seq.zip a generic_arguments) then 0
          else compare this.Rank t.Rank

        | Tuple a, Tuple b -> compare a b
        | Query a, Query b -> compare a b

//        | ImportedType(t1, _), Query (MaybeQuery(a)) when (t1.GetInterface("IEnumerable`1") <> null || t1.Name = "IEnumerable`1") && t1.Name.ToLower() <> "string" -> 
//          let generic_arguments = t1.GetGenericArguments()
//          compare a (Just(ImportedType(generic_arguments.[0], Position.Empty)))
//        | Query (MaybeQuery(a)), ImportedType(t1, _) when (t1.GetInterface("IEnumerable`1") <> null || t1.Name = "IEnumerable`1") && t1.Name.ToLower() <> "string" -> 
//          let generic_arguments = t1.GetGenericArguments()
//          compare a (Just(ImportedType(generic_arguments.[0], Position.Empty)))

        | MaybeQuery(a), t ->
          compare a (Just(t))
        | t, MaybeQuery(a) ->
          compare a (Just(t))
        | Query a, ImportedType(t1, _) when (t1.GetInterface("IEnumerable`1") <> null || t1.Name = "IEnumerable`1") && t1.Name.ToLower() <> "string" -> 
          let generic_arguments = t1.GetGenericArguments()
          compare a (ImportedType(generic_arguments.[0], Position.Empty))
        | ImportedType(t1, _), Query a when (t1.GetInterface("IEnumerable`1") <> null || t1.Name = "IEnumerable`1") && t1.Name.ToLower() <> "string" -> 
          let generic_arguments = t1.GetGenericArguments()
          compare a (ImportedType(generic_arguments.[0], Position.Empty))


        
//          let t_ienum =   
//          if t_ienum <> null then 
//            let t_ienum_arg = t_ienum.GetGenericArguments().[0]                              
//            eval_t_gt a t1    
//          else compare this.Rank t.Rank        
//          
//        | ImportedType(t1, _), Query a -> 
//          let t_ienum = t1.GetInterface("IEnumerable`1")          
//          if t_ienum <> null then 
//            let t_ienum_arg = t_ienum.GetGenericArguments().[0]                              
//            eval_t_gt a t1    
//          else compare this.Rank t.Rank    

        | Unit _, Unit _ -> 0
        | GenericType(a,a'), GenericType(b,b') -> compare (a,a') (b,b')
        | _ -> compare this.Rank t.Rank

    interface IComparable with
        member this.CompareTo obj =
            match obj with
            | :? TypeDecl as other -> (this :> IComparable<_>).CompareTo other
            | _                    -> invalidArg "obj" "not a Category"
    interface IEquatable<TypeDecl> with
        member this.Equals t = 
          (this :> IComparable<_>).CompareTo t = 0

    override this.Equals obj =
        match obj with
        | :? TypeDecl as other ->(this :> IComparable<_>).CompareTo other = 0
        | _                     -> invalidArg "obj" "not a Category"
    override this.GetHashCode () = hash this

    member this.Position = 
        match this with
        | MaybeQuery(t) -> t.Position 
        | EntityName(id) -> id.idRange; | ImportedType(t, p) -> p; 
        | UnionType(ds) -> 
          match ds with
          | [] -> raise Position.Empty (sprintf "UnionType without body. Internal error at %s(%s)" __SOURCE_FILE__ __LINE__)
          | _ -> ds.Head.Position; 
        | Tuple(ds) -> 
          match ds with
          | [] -> raise Position.Empty (sprintf "Tuple without body. Internal error at %s(%s)" __SOURCE_FILE__ __LINE__)
          | _ ->ds.Head.Position; 
        | Query(d) -> d.Position; | Unit(p) -> p | GenericType(t,_) -> t.Position
        | MaybeType tp -> tp.Position              
    member this.IsGeneric =
      match this with 
      | GenericType _ -> true
      | _ -> false
    member this.IsCasanovaEntity =
      match this with 
      | EntityName _ -> true
      | _ -> false
    member this.IsSystemType = 
        match this with 
        ImportedType(_) -> true 
        | Unit(p) -> true
        | Query(t) -> t.IsSystemType
        | Tuple(l) -> List.forall (fun (el:TypeDecl) -> el.IsSystemType) l 
        | UnionType(l) -> List.forall (fun (el:TypeDecl) -> el.IsSystemType) l
        | GenericType(ImportedType(t,p),l) when t.GetInterface("IEnumerable`1") <> null -> List.forall (fun (el:TypeDecl) -> el.IsSystemType) l
        | GenericType(el,l) -> true // el.isImportedType && (List.forall (fun (el:TypeDecl) -> el.isImportedType) l)
        | _ -> false
    override this.ToString() = this.TypeName
    member this.TypeNameVerbose = 
      let rec traverseNames (currType:TypeDecl) =
        match currType with 
        | ImportedType(t,p) -> 
          let rec CSharpName (t:System.Type) =
            if t.IsGenericType then 
              let name = t.FullName.Substring(0, t.FullName.IndexOf('`'))
              let args = t.GetGenericArguments() |> Seq.map CSharpName |> Seq.reduce (fun x y -> x + "," + y)
              name + "[" + args + "]"
            else
              t.FullName.Replace('+','.')
          CSharpName t
        | Unit _ -> "void"
        | EntityName(id) -> id.idText
        | Tuple(l) -> 
            match l with
            | [] -> raise Position.Empty (sprintf "Parallel without body. Internal error at %s(%s)" __SOURCE_FILE__ __LINE__)
            | _ ->
              let internalTypes = (traverseNames l.Head)  + (List.fold (fun str t -> str + ", " + (traverseNames t)) "" l.Tail) in
              "Casanova.Prelude.Tuple`2[" + internalTypes + "]"
        | Query(t) -> "IEnumerable`1[" + (traverseNames t) + "]" 
        | GenericType(ImportedType(el,p),l) -> 
            match l with
            | [] -> raise Position.Empty (sprintf "Generic type without arguments. Internal error at %s(%s)" __SOURCE_FILE__ __LINE__)
            | _ ->
              let internalTypes = (traverseNames l.Head)  + (List.fold (fun str t -> str + ", " + (traverseNames t)) "" l.Tail) in
              let name = el.FullName.Substring(0, el.FullName.IndexOf('`'))
              name  + "[" + internalTypes + "]" 
        | MaybeQuery(Just(td)) -> td.TypeNameVerbose
        | MaybeQuery(Nothing(gtd,_)) when gtd.Value.Type.IsSome -> gtd.Value.Type.Value.TypeNameVerbose
        | MaybeQuery(Nothing(gtd,p)) -> raise p (sprintf "Unused list at: %A" (p.ToString()))
        | MaybeType(Nothing(gtd,_)) when gtd.Value.Type.IsNone -> "Option"
        | MaybeType(Just(td)) -> "Casanova.Prelude.Option`1[" + td.TypeNameVerbose + "]"
        | MaybeType(Nothing(gtd,_)) when gtd.Value.Type.IsSome -> "Casanova.Prelude.Option`[" + gtd.Value.Type.Value.TypeNameVerbose + "]"
      in traverseNames this

    member this.TypeName = 
      let rec traverseNames (currType:TypeDecl) =
        match currType with 
        | ImportedType(t,p) -> 
          let rec CSharpName (t:System.Type) =
            if t.IsGenericType then 
              let name = t.FullName.Substring(0, t.FullName.IndexOf('`'))
              let args = t.GetGenericArguments() |> Seq.map CSharpName |> Seq.reduce (fun x y -> x + "," + y)
              name + "<" + args + ">"
            else
              t.FullName.Replace('+','.')
          CSharpName t
        | Unit _ -> "void"
        | EntityName(id) -> id.idText
        | Tuple(l) -> 
            match l with
            | [] -> raise Position.Empty (sprintf "Parallel without body. Internal error at %s(%s)" __SOURCE_FILE__ __LINE__)
            | _ ->
              let internalTypes = (traverseNames l.Head)  + (List.fold (fun str t -> str + ", " + (traverseNames t)) "" l.Tail) in
              "Tuple<" + internalTypes + ">"
        | Query(t) -> "List<" + (traverseNames t) + ">" 
        | GenericType(ImportedType(el,p),l) -> 
            match l with
            | [] -> raise Position.Empty (sprintf "Generic type without arguments. Internal error at %s(%s)" __SOURCE_FILE__ __LINE__)
            | _ ->
              let internalTypes = (traverseNames l.Head)  + (List.fold (fun str t -> str + ", " + (traverseNames t)) "" l.Tail) in
              let name = el.FullName.Substring(0, el.FullName.IndexOf('`'))
              name + "<" + internalTypes + ">" 
        | MaybeQuery(Just(td)) -> td.TypeName
        | MaybeQuery(Nothing(gtd,_)) when gtd.Value.Type.IsSome -> gtd.Value.Type.Value.TypeName
        | MaybeQuery(Nothing(gtd,p)) -> raise p (sprintf "Unused list at: %A" (p.ToString()))
        | MaybeType(Nothing(gtd,_)) when gtd.Value.Type.IsNone -> "Option"
        | MaybeType(Just(td)) -> "Option<" + td.TypeName + ">"
        | MaybeType(Nothing(gtd,_)) when gtd.Value.Type.IsSome -> "Option<" + gtd.Value.Type.Value.TypeName + ">"

//        | _ -> failwith currType.Position (sprintf "Not supported type name %s" currType.TypeName)
      in traverseNames this

and Rule =
  {
    Domain    : List<Id>
    Body      : Block
    Position  : Position
  } 

and Block = List<TypedExpression>

and Call =
  | Constructor of TypeDecl * Block
  | Static of TypeDecl * Id * Block
  | Instance of Id * Id * Block
  with member this.Position = match this with | Constructor(t,_) -> t.Position; | Static(t,_,_) -> t.Position; | Instance(id,_,_) -> id.idRange

and MaybeExpr = JustExpr of TypedExpression 
                | NothingExpr of Position

and Expression =
  | Add of TypedExpression * TypedExpression
  | Sub of TypedExpression * TypedExpression
  | Div of TypedExpression * TypedExpression
  | Modulus of TypedExpression * TypedExpression 
  | Mul     of TypedExpression * TypedExpression
  
  | Not of TypedExpression
  | And of TypedExpression * TypedExpression
  | Or  of TypedExpression * TypedExpression
  | Equals of TypedExpression * TypedExpression
  | Greater of TypedExpression * TypedExpression

  | Lambda  of List<Id * Option<TypeDecl>> * Block
  | Maybe   of MaybeExpr
  | ConcatQuery of List<TypedExpression>
  | AppendToQuery   of TypedExpression * TypedExpression  
  | AddToQuery      of TypedExpression * TypedExpression  
  | SubtractToQuery of TypedExpression * TypedExpression  

  | IndexOf of Id * TypedExpression
  | Choice of bool * (TypedExpression * Block * Position) list * Position
  | Parallel of (Block * Position) list * Position
  | Range of TypedExpression * TypedExpression * Position
  | NewEntity of List<Id * Block>
  | Let of Id * TypeDecl * TypedExpression

  | IfThenElse of TypedExpression * Block * Block
  | IfThen of TypedExpression * Block
  | Yield of TypedExpression
  | Wait of TypedExpression
  | Tuple of Block
  | For of List<Id> * TypedExpression * Block
  | While of TypedExpression * Block
  | Query of QueryExpression
  | Call of Call
  | Literal of BasicAST.Literal
  | IdExpr of Id

  | Cast of TypeDecl * TypedExpression * Position

  with member this.Position = 
        match this with 
        | AddToQuery      (_,(p,_)) -> p.Position
        | SubtractToQuery (_,(p,_)) -> p.Position
        | Cast(_,_,p) -> p
        | Choice(_,cs, p) -> p | Range(_,_,p) -> p 
        | NewEntity exprs -> 
          match exprs with
          | [] -> raise Position.Empty (sprintf "New entity without body. Internal error at %s(%s)" __SOURCE_FILE__ __LINE__)
          | _ ->(fst exprs.Head).idRange; 
        | Let(id,_,_) -> id.idRange; | IfThenElse(b,_,_) -> (snd b).Position; | IfThen(b,_) -> (snd b).Position; 
        | Yield(t) | Wait(t) -> (fst t).Position; 
        | Tuple(b) -> 
          match b with
          | [] -> raise Position.Empty (sprintf "Tuple without body. Internal error at %s(%s)" __SOURCE_FILE__ __LINE__)
          | _ ->(fst b.Head).Position; 
        | For(ids,_,_) -> 
          match ids with
          | [] -> raise Position.Empty (sprintf "For without indexes. Internal error at %s(%s)" __SOURCE_FILE__ __LINE__)
          | _ ->ids.Head.idRange; 
        | While(b,_) -> (snd b).Position; | Query(q) -> q.Position; | Call(c) -> c.Position; 
        | Literal(l) -> l.Position;| IdExpr(id) -> id.idRange
        | Add (e, _) | Sub (e, _) | Div (e, _) | Modulus (e, _) | Mul (e, _) | Not e | And (e, _) 
        | Or (e, _) | Equals (e, _) | Greater (e, _) -> (fst e).Position | Parallel(_, p) -> p

and TypedExpression = TypeDecl * Expression

and QueryExpression =
   | QueryStatements of List<InnerQueryExpression> with 
    member this.Position = 
      match this with 
      | QueryStatements q -> 
        match q with
        | [] -> raise Position.Empty (sprintf "Query expression without statements. Internal error at %s(%s)" __SOURCE_FILE__ __LINE__)
        | _ -> q.Head.Position

and InnerQueryExpression =
  | For of Id * TypeDecl * TypedExpression
  | Select of TypedExpression
  | FindBy of TypedExpression
  | Exists of TypedExpression
  | MinBy of TypedExpression
  | ForAll of TypedExpression  
  | Sum of Position
  | Min of Position
  | Max of Position
  | MaxBy of TypedExpression
  | Where of TypedExpression
  | GroupBy of TypedExpression
  | GroupByInto of TypedExpression * Id
  | Let of Id * TypeDecl * TypedExpression
  | Empty of TypeDecl * Position
  | LiteralList of List<TypedExpression> 
  with member this.Position = 
    match this with  
    | For (id,_,_) -> id.idRange; | Select(t) -> (fst t).Position; | FindBy(b) | Exists(b) | Where(b) -> (snd b).Position; | MinBy (e) | MaxBy (e) | GroupBy (e) -> (fst e).Position; | GroupByInto(_, id) -> id.idRange; | Let(ids,_,_) -> ids.idRange; 
    | Empty(t,p) -> p; 
    | Max p -> p
    | LiteralList(es) -> 
      match es with
      | [] -> raise Position.Empty (sprintf "LiteralList without body. Internal error at %s(%s)" __SOURCE_FILE__ __LINE__)
      | _ -> (fst es.Head).Position
