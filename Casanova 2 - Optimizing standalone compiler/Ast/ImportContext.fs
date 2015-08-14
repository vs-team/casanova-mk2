module OpenContext
open System.Collections.Generic
open System


let inherited_types : ResizeArray<System.Type> = List()

let opened_referenced_library_type : ResizeArray<System.Type> = List()
let private opened_types = Dictionary<string, Type>()
let private opened_directives : List<string> = List()



let rec TrySolveIdFromInheritedTypes (id : Common.Id) =  
  match id.idText.Split('.') |> Seq.toList with
  | x :: _ -> 
    let res =
      
      inherited_types |> Seq.tryFind(fun t -> t.GetProperties() |> Seq.exists(fun p -> p.Name = x) ||
                                              t.GetFields() |> Seq.exists(fun p -> p.Name = x))
    match res with
    | None ->
    
      
     
      match TryResolveTypeName id.idText with
      | Some _ -> id
      | None -> id
    | Some t -> { idText = t.Name + "." + id.idText; idRange = id.idRange }
  | _ -> failwith "Unexpected id"
    

and TryResolveTypeName (type_name) =
  let res =
    if opened_types.ContainsKey type_name then Some opened_types.[type_name]

    else
      match System.Type.GetType(type_name) with
      | null ->
        match opened_referenced_library_type |> Seq.tryFind(fun t ->   t.Name = type_name) with
        | None ->
          let rec check_tp opened_directives =
            match opened_directives with
            | [] -> None
            | x :: xs ->
              if opened_types.ContainsKey(x + "." + type_name) then
                Some opened_types.[x + "." + type_name]
              elif System.Type.GetType(x + "." + type_name) <> null then
                let tp = System.Type.GetType(x + "." + type_name)
                opened_types.Add(x + "." + type_name, tp)
                Some tp
              else 
                let maybe_tp = opened_referenced_library_type |> Seq.tryFind(fun t -> t.Name = x + "." + type_name)
                match maybe_tp with
                | None -> check_tp xs
                | Some maybe_tp ->
                  opened_types.Add(x + "." + type_name, maybe_tp)
                  Some maybe_tp

          match check_tp (opened_directives |> Seq.toList) with
          | Some tp -> Some tp
          | None -> None
        | Some tp -> 
          opened_types.Add(type_name, tp)
          Some tp
      | tp -> 
        opened_types.Add(type_name, tp)
        Some tp
  match res with
  | Some t -> Some t
  | None ->
//    System.Diagnostics.Debugger.Launch(); 
    
    let type_name = type_name.Split('.') |> Seq.toList
    match opened_types |> Seq.tryFind(fun t -> t.Value.Namespace = type_name.Head) with
    | _ when type_name.Tail.IsEmpty -> None
    | None -> 
      match inherited_types |> Seq.tryFind(fun t -> t.Namespace = type_name.Head) with
      | None -> None
      | Some _ ->
        let type_name = Seq.fold(fun s t -> s + "." + t) type_name.Tail.Head type_name.Tail.Tail
        TryResolveTypeName type_name
      | _ -> None
    | Some _ ->  
      let type_name = Seq.fold(fun s t -> s + "." + t) type_name.Tail.Head type_name.Tail.Tail
      TryResolveTypeName type_name


let ResolveTypeName type_name (position : Common.Position) =
  let type_name = if type_name = "float32" || type_name = "float" then "System.Single" else type_name


  let f () =
    match System.Type.GetType(type_name) with
    | null ->
      match opened_referenced_library_type |> Seq.tryFind(fun t -> t.Namespace + "." + t.Name = type_name || t.Name = type_name) with
      | None ->
      
        let rec check_tp opened_directives =
          match opened_directives with
          | [] -> None
          | x :: xs ->
            if opened_types.ContainsKey(x + "." + type_name) then
              Some opened_types.[x + "." + type_name]
            elif System.Type.GetType(x + "." + type_name) <> null then
              let tp = System.Type.GetType(x + "." + type_name)
              opened_types.Add(x + "." + type_name, tp)
              Some tp
            else 
              let maybe_tp = opened_referenced_library_type |> Seq.tryFind(fun t -> t.Name = x + "." + type_name)
              match maybe_tp with
              | None -> check_tp xs
              | Some maybe_tp ->
                opened_types.Add(x + "." + type_name, maybe_tp)
                Some maybe_tp

        match check_tp (opened_directives |> Seq.toList) with
        | Some tp -> tp
        | None -> 
          let (Common.Position(p)) = position
          printfn "Type error. Field or property not found. {%A, (%A,%A)} "  type_name p.Line p.Column
          raise(Common.CompilationError(position, "Type error. Field or property not found. Error when trying to resolve: " + type_name))

      | Some tp -> 
        opened_types.Add(type_name, tp)
        tp
    | tp -> 
      opened_types.Add(type_name, tp)
      tp

  if opened_types.ContainsKey type_name then opened_types.[type_name]
  else
    if(type_name.ToLower().Contains("+")) then

      let tps = type_name.Split('+') |> Seq.toList


      let tp : Ref<Option<System.Type>>= ref None


      if opened_types.ContainsKey(tps.Head) then
        tp := Some opened_types.[tps.Head]
        let rec check_tp lst =
          match lst with
          | [] -> ()
          | x :: xs ->
            let tp1 = tp.Value.Value
            let i = tp1.GetProperties()
            let k = i |> Seq.filter(fun p -> p.PropertyType.Name = x)
      
            if k |> Seq.isEmpty then

              let l = tp1.GetFields()
              let m = l |> Seq.filter(fun p -> p.FieldType.Name = x) |> Seq.toList
              if m |> Seq.isEmpty |> not then
                tp := m |> Seq.head |> (fun f -> f.FieldType) |> Some              
                check_tp xs              
              else
                let (Common.Position(p)) = position
                printfn "Type error. Field or property not found. {%A, (%A,%A)} "  type_name p.Line p.Column
                raise(Common.CompilationError(position, "Type error. Field or property not found. Error when trying to resolve: " + type_name))
            else
              tp := k |> Seq.head |> (fun f -> f.PropertyType) |> Some
              check_tp xs

        do check_tp tps.Tail
        match tp.Value with 
        | Some t -> t
        | None ->
          let (Common.Position(p)) = position
          printfn "Type error. Field or property not found. {%A, (%A,%A)} "  type_name p.Line p.Column
          raise(Common.CompilationError(position, "Type error. Field or property not found. Error when trying to resolve: " + type_name))
      else
        f()
    else f()
        

let run_time_assemblies = System.Reflection.Assembly.GetExecutingAssembly().GetTypes() |> Seq.filter(fun a -> a.IsClass) |> Seq.groupBy(fun c -> c.Namespace) |> Map.ofSeq

let AddDirective directive = 
  //to improve namespace.namespace....type not supported yet
  match run_time_assemblies.ContainsKey(directive) with  
  | false -> opened_directives.Add directive
  | true -> opened_referenced_library_type.AddRange(run_time_assemblies.[directive])
let AddReferencedLibraryType _type = opened_referenced_library_type.Add(_type)
let LoadCSharpClasses types = 
    inherited_types.AddRange types
    opened_referenced_library_type.AddRange(types)