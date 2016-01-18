module TypecheckContext
open Common


type TypecheckContext = Map<string, TypedAST.TypeDecl>
type GlobalTypecheckContext = Map<string, Map<string, TypedAST.TypeDecl> * List<TypedAST.TypeDecl>>
type PriorInformationType = Instance | Static | Unknown


type IDTResolutionContext = 
  Uknown | Type of TypedAST.TypeDecl
  with static member buildType t = Type(TypedAST.ImportedType(t,Position.Empty))
       static member buildGenericType t = Type(t)

//let rec TrySolveIdFromInheritedTypes (id : Common.Id) =  
//  match id.idText.Split('.') |> Seq.toList with
//  | x :: _ -> 
//    let res =
//      
//      inherited_types |> Seq.tryFind(fun t -> t.GetProperties() |> Seq.exists(fun p -> p.Name = x) ||
//                                              t.GetFields() |> Seq.exists(fun p -> p.Name = x))
//    match res with
//    | None ->
//    
//      
//     
//      match TryResolveTypeName id.idText with
//      | Some _ -> id
//      | None -> id
//    | Some t -> { idText = t.Name + "." + id.idText; idRange = id.idRange }
//  | _ -> failwith "Unexpected id"
//    



let Lookup (id:string) (suffixes : string list)  (pos : Position) (globalContext:GlobalTypecheckContext) (context:TypecheckContext) (information : PriorInformationType) : TypedAST.TypeDecl =
  let ids = id.Split [|'.'|] |> Seq.toList
  if ids.Length = 0 then
    raise pos (sprintf "Type error. Identifier %s not found." id)

  let get_external_type_from_id (ids : string list) (tp : Option<System.Type>) (is_static : bool) =
    match tp with
    | None -> 
      //raise pos (sprintf "Type error. Identifier %s not found." id)
      IDTResolutionContext.Uknown
    | Some tp ->
      let mutable t_id = IDTResolutionContext.buildType tp
      for id in ids do
        match t_id with
        | IDTResolutionContext.Type(TypedAST.ImportedType(tp, _)) ->

          let a = tp.GetProperties()
          let b = tp.GetNestedTypes()
          let c = tp.GetMembers()
          if is_static && (tp.GetNestedTypes() |> Seq.exists(fun t -> t.Name = id)) then
            t_id <- IDTResolutionContext.Type(TypedAST.ImportedType((tp.GetNestedTypes() |> Seq.find(fun t -> t.Name = id)), Position.Empty))
          else
            let f_id = 
              if is_static then tp.GetField(id, System.Reflection.BindingFlags.Static ||| System.Reflection.BindingFlags.Public)
              else tp.GetField(id,System.Reflection.BindingFlags.Instance ||| System.Reflection.BindingFlags.Public)
            if f_id <> null then
              t_id <- IDTResolutionContext.Type(TypedAST.ImportedType(f_id.FieldType, Position.Empty))
            else
              let p_id = 
                if is_static then tp.GetProperty(id, System.Reflection.BindingFlags.Static ||| System.Reflection.BindingFlags.Public)
                else tp.GetProperty(id, System.Reflection.BindingFlags.Instance ||| System.Reflection.BindingFlags.Public)
              if p_id <> null then
                t_id <- IDTResolutionContext.Type(TypedAST.ImportedType(p_id.PropertyType, Position.Empty))
              else
                t_id <- IDTResolutionContext.Uknown
        | e -> 
          t_id <- IDTResolutionContext.Uknown
      t_id

  let check_instance (ids : string list) (t_id : TypedAST.TypeDecl) is_static = 
    if ids.IsEmpty then IDTResolutionContext.Uknown
    else
      //if ids.Head = "___p02" then printf "hi"
      if context |> Map.containsKey ids.Head || is_static then
        let t_id = t_id |> ref    
        let rec eval_ids ids : IDTResolutionContext =
          match ids with
          | [] -> IDTResolutionContext.buildGenericType t_id.Value
          | id::ids ->
            match !t_id with
            | TypedAST.ImportedType(tp, _) when tp.Name.StartsWith("Tuple`") ->
              let generic_arguments1 = tp.GetGenericArguments()
              let tp =
                match id with
                | "Item1" -> TypedAST.TypeDecl.ImportedType(generic_arguments1.[0], pos)
                | "Item2" -> TypedAST.TypeDecl.ImportedType(generic_arguments1.[1], pos)
                | _ -> raise pos "Not supported operation on option"  
              t_id := tp
              let res = eval_ids ids
              res

            | TypedAST.Tuple(tps) ->
              let tp =
                match id with
                | "Item1" -> tps.Head
                | "Item2" -> tps.Tail.Head
                | _ -> raise pos "Not supported operation on option"  
              t_id := tp
              let res = eval_ids ids
              res

            | TypedAST.ImportedType(tp, _) -> get_external_type_from_id (id :: ids) (Some tp) is_static
            
            | TypedAST.EntityName(entityName) when globalContext.ContainsKey entityName.idText && (fst globalContext.[entityName.idText]).ContainsKey id -> 
              t_id := (fst globalContext.[entityName.idText]).[id]
              eval_ids ids


            | TypedAST.TypeDecl.MaybeType(TypedAST.Just(tp)) ->
              let tp =
                match id with
                | "IsSome" | "IsNone" -> TypedAST.TypeDecl.ImportedType(typeof<bool>, pos)
                | "Value" -> tp
                | _ -> raise pos "Not supported operation on option"  
              t_id := tp
              let res = eval_ids ids
              res
            | TypedAST.Query(tp) when id = "Head" -> 
              t_id := tp
              let res = eval_ids ids
              res
            | TypedAST.Query(_) when id = "Tail" -> 
//              t_id := tp
              let res = eval_ids ids
              res
            | TypedAST.Query(tp) when id = "Count" -> 
              IDTResolutionContext.buildType(typeof<int>)
            | _ -> 
              IDTResolutionContext.Uknown
        eval_ids (if is_static then ids else ids.Tail)
      else IDTResolutionContext.Uknown

  let rec check_namespace (ids : string list) (_namespace : Option<string>) = 
    match OpenContext.opened_referenced_library_type 
            |> Seq.tryFind(fun t -> if ids.Length > 0 && t.Name = ids.Head then 
                                      if _namespace.IsSome && _namespace.Value = t.Namespace then true
                                      elif _namespace.IsNone then true
                                      else false
                                    else false) with
    | None -> 
      
      match OpenContext.opened_referenced_library_type |> Seq.tryFind(fun t -> ids.Length > 0 && t.Namespace = ids.Head &&
                                                                               not ids.Tail.IsEmpty &&
                                                                               t.Name = ids.Tail.Head ) with
      | None -> IDTResolutionContext.Uknown
      | Some t -> check_namespace ids.Tail (Some t.Namespace)
    | Some t when ids.Tail.Length > 0 -> check_instance ids.Tail (TypedAST.ImportedType(t, Position.Empty)) true
    | Some t  -> IDTResolutionContext.Type(TypedAST.ImportedType(t, Position.Empty))
        

//  if context.ContainsKey (ids.Head) |> not && information <> PriorInformationType.Instance then    
//      match check_namespace ids None with
//      | IDTResolutionContext.Uknown -> raise pos (sprintf "Type error. Cant resolve %A. Try to use a verbose type annotation." ids)
//      | IDTResolutionContext.Type(t, _) -> t
//  else 
  match context.ContainsKey (ids.Head) |> not, information with
  | true, _
  | _ , PriorInformationType.Static ->
    match check_namespace (ids @ suffixes) None,   
          check_namespace ids None with
    | IDTResolutionContext.Uknown, _ -> raise pos (sprintf "Type error. Cant resolve %A. Try to use a verbose type annotation." ids)
    | IDTResolutionContext.Type(t), _ -> t
  | _, PriorInformationType.Instance ->
    match check_instance ids context.[ids.Head] false with
    | IDTResolutionContext.Uknown -> raise pos (sprintf "Type error. Cant resolve %A. Try to use a verbose type annotation." ids)
    | IDTResolutionContext.Type(t)-> t      
  | is_instance, _-> 
    //if ids.Head = "___p02" then printf "hi"
    
    match 
      check_instance (ids @ suffixes) context.[ids.Head] false, check_namespace (ids @ suffixes) None,    
      check_instance ids context.[ids.Head] false, check_namespace ids None with

    | IDTResolutionContext.Uknown, IDTResolutionContext.Uknown, _, _ 
    | _, _, IDTResolutionContext.Uknown, IDTResolutionContext.Uknown -> raise pos (sprintf "Type error. Cant resolve %A. Try to use a verbose type annotation." ids)    
    | IDTResolutionContext.Type(t1), IDTResolutionContext.Type(t2), res1, res2 when ids.Length + suffixes.Length > 1 -> 
      raise pos (sprintf "Ambiguity Type error. Cant resolve %A. Try to use a verbose type annotation. %A, %A" ids t1.TypeName t2.TypeName)

    | IDTResolutionContext.Type(_), _,
      IDTResolutionContext.Type(t1), IDTResolutionContext.Type(t2)  -> t1
    
    | _, IDTResolutionContext.Type(_),
      IDTResolutionContext.Type(t1), IDTResolutionContext.Type(t2)  -> t2

    | _, _,
      _, IDTResolutionContext.Type(t2)  -> t2

    | _, _,
      IDTResolutionContext.Type(t1), _ -> t1

let LookupCreate (tp_id : string) p (args:List<TypedAST.TypeDecl>) (globalContext:GlobalTypecheckContext) =

  if globalContext |> Map.containsKey tp_id then
    let t_args = (snd globalContext.[tp_id])
    if args.Length <> t_args.Length then
        raise p "Wrong number of arguments passed to Create." 
    else
      for arg1, arg2 in List.zip args t_args do
        if arg1 <> arg2 then
          raise p "Create type args mismatch." 
    TypedAST.TypeDecl.EntityName(TypedAST.Id.buildFrom {idText = tp_id; idRange = p})
  else
    raise p "Type error. Create not found."

let private LookupMethod (t : System.Type)
                         (pos : Position)
                         (method_info : System.Reflection.MethodBase)
                         (method_return_type : System.Type)
                         (type_args:List<TypedAST.TypeDecl>)
                         (args:List<TypedAST.TypeDecl>) (globalContext:GlobalTypecheckContext) =
  let imported_args = 
    [for a in t.GetGenericArguments() -> a.Name ]

  let generic_arguments = 
    if imported_args.Length <> type_args.Length then
      raise pos (sprintf "Wrong number of generic arguments given to %s" method_info.Name)
    List.zip imported_args type_args |> Map.ofList


  if method_info.GetParameters().Length <> args.Length then
    raise pos (sprintf "Wrong number of generic arguments given to %s. Expected %d, given %d" method_info.Name (method_info.GetParameters().Length) args.Length)
  for m_a,t_a in Seq.zip (method_info.GetParameters()) args do
    let p_a = m_a.ParameterType
    if p_a.IsGenericParameter then
      if t_a <> generic_arguments.[p_a.Name] then
        failwith t_a.Position "..."
    else
//      let rec traverse t_a (p_a:System.Type) =
//        match t_a with
//        | TypedAST.TypeDecl.ImportedType(t, _) when t.FullName = p_a.FullName -> ()
//        | TypedAST.TypeDecl.GenericType(t,args)
//        | TypedAST.TypeDecl.GenericType(t,[TypedAST.TypeDecl.Tuple(args)]) when p_a.IsGenericType ->
//          if args.Length <> p_a.GetGenericArguments().Length then
//            raise pos (sprintf "Wrong number of generic arguments given to %s" method_info.Name)
//          for arg,par_arg in Seq.zip args (p_a.GetGenericArguments()) do
//            traverse arg par_arg
//        | TypedAST.Tuple(args) ->
//          if args.Length <> p_a.GetGenericArguments().Length then
//            raise pos (sprintf "Wrong number of generic arguments given to %s" method_info.Name)
//          for arg,par_arg in Seq.zip args (p_a.GetGenericArguments()) do
//            traverse arg par_arg
//        | _ -> failwith t_a.Position  (sprintf "Cannot find match for %s" t_a.TypeName)
      //traverse t_a p_a      
      (t_a = TypedAST.TypeDecl.ImportedType(p_a, Position.Empty)) |> ignore

  TypedAST.TypeDecl.ImportedType(method_return_type, pos)
  
let LookupTypeMethod (tp : TypedAST.TypeDecl) (pos : Position) (method_name : string option) (args:List<TypedAST.TypeDecl>) (globalContext:GlobalTypecheckContext) =
    match method_name with
    | Some method_name ->
      match tp with
      | TypedAST.Query(t) ->
        match method_name, args with
        | "Contains", [arg_t] when arg_t = t -> 
          TypedAST.TypeDecl.ImportedType(typeof<bool>, pos)
        | _ -> failwith pos "Lookup type method error. Not supported query operation."
      | TypedAST.TypeDecl.GenericType(TypedAST.TypeDecl.ImportedType(t, p),type_args) -> 

        let method_info = t.GetMethod method_name
        let return_type = method_info.ReturnType
        LookupMethod t pos method_info method_info.ReturnType type_args args globalContext

      | TypedAST.TypeDecl.ImportedType(t,p) -> 
//        let t_args = args |> Seq.map(fun arg -> match arg with | TypedAST.TypeDecl.ImportedType(t,p) -> t | _ -> failwith arg.Position "...")
//        let t_args_arr = args |> Seq.map (fun a -> a.TypeName) |> Seq.toArray
        match t.GetMethods() |> Seq.tryFind (fun m -> 
                                                      m.Name = method_name && 
                                                      m.GetParameters() 
                                                      |> Seq.map (fun p -> TypedAST.ImportedType(p.ParameterType, Position.Empty)) 
                                                      |> Seq.toList 
                                                       = args) with
        | Some method_info ->
          let return_type = method_info.ReturnType
          LookupMethod t pos method_info return_type [] args globalContext
//        | _ -> 
////          let t_args_arr = args |> Seq.map (fun a -> a.TypeNameVerbose) |> Seq.toArray
//          match t.GetMethods() |> Seq.tryFind(fun m -> m.Name = method_name && m.GetParameters() |> Array.map (fun p -> p.ParameterType.FullName) = t_args_arr) with
//          | Some method_info ->
//            let return_type = method_info.ReturnType
//            LookupMethod t pos method_info return_type [] args globalContext
        | _ ->  failwith pos (sprintf "Cannot resolve method name %s" method_name)        
      | _ -> failwith pos "Lookup type method error"

    | None ->
      let mutable res = None
      match tp with
      | TypedAST.TypeDecl.GenericType(TypedAST.TypeDecl.ImportedType(t,p),type_args) ->         
        let constructors = t.GetConstructors()
        for _constructor in constructors do          
          let return_type = _constructor.DeclaringType
          try
            res <- Some (LookupMethod t pos _constructor return_type type_args args globalContext)
          with _ -> ()
      | TypedAST.TypeDecl.ImportedType(t,p) -> 
        let constructors = t.GetConstructors()
        for _constructor in constructors do          
          let return_type = _constructor.DeclaringType
          try
            res <- Some (LookupMethod t pos _constructor return_type [] args globalContext)
          with _ -> ()
      match res with
        | None -> failwith pos "..."
        | Some tp -> tp

let LookupIdMethod (id : string) (pos : Position) (method_name : string) (args:List<TypedAST.TypeDecl>) (globalContext:GlobalTypecheckContext) (context: TypecheckContext) (information: PriorInformationType) =
    let t_id = Lookup id [] pos globalContext context information
    LookupTypeMethod t_id pos (Some method_name) args globalContext
