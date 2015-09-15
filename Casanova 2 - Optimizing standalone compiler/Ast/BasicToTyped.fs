module BasicToTyped
open TypecheckContext
open ShadowcheckContext

open Common
type Id = TypedAST.Id


let hasMatchingParameters (mi:System.Reflection.MethodInfo) (t_e1:System.Type) (t_e2:System.Type) =
  mi.GetParameters() |> Array.map (fun p -> p.ParameterType.FullName) = [| t_e1.FullName; t_e2.FullName |]

let is_bool_type t_e =
              match t_e with
              | TypedAST.ImportedType(t_e, _) when t_e.FullName = typeof<bool>.FullName -> true
              | _ -> false

let are_equal_collection t_e1 t_e2 =  
  let get_t t = 
    match t with
    | TypedAST.TypeDecl.Query(t) -> Some t
    | TypedAST.ImportedType(t,p) ->
      let t_ienum = t.GetInterface("IEnumerable`1")
      if t_ienum <> null then 
        let t_ienum_arg = t_ienum.GetGenericArguments().[0]
        let t_arg = TypedAST.ImportedType(t_ienum_arg, p)
        Some t_arg
      else None
    | _ -> None
  match get_t t_e1, get_t t_e2 with
  | Some t1, Some t2 -> t1 <> t2
  |_ -> true



let rec private convertRuleBody (is_create : bool)
                                (domain : List<Id>) (worldName:Id) (entityName:Id) 
                                (entities : Map<Id, TypedAST.TypeDecl * Map<Id, TypedAST.Field>>) // entity id -> entity type * entity fields
                                (globalContext:GlobalTypecheckContext)
                                (program : BasicAST.Program) args
                                (rule_idx : int)
                                (b:List<BasicAST.Expression>) : List<TypedAST.TypedExpression>  =

  let rec traverseQuery (p : BasicAST.Program) (q:BasicAST.QueryExpression) (shadowContext : ShadowCheckContext) (context:TypecheckContext) : TypedAST.TypeDecl * TypedAST.QueryExpression =
    let rec convertOuterLoops (q:List<BasicAST.InnerQueryExpression>) (shadowContext : ShadowCheckContext) (context:TypecheckContext) : (TypedAST.TypeDecl * TypedAST.QueryExpression) * TypecheckContext =
        let mutable result = []
        let mutable context1 = context
        let mutable shadowContext1 = shadowContext
          
        if q.Length = 0 then raise Position.Empty (sprintf "Internal error. Empty query not expected at %s(%s)" __SOURCE_FILE__ __LINE__) |> ignore

        let mutable t_q = TypedAST.TypeDecl.Query(TypedAST.TypeDecl.Unit q.Head.Position)
        for s in q do
          let raiseGeneric line file = raise s.Position (sprintf "Generic error at %A %A" line file)
          let raise = raise s.Position
          match s,t_q with
          | BasicAST.QueryFor(ids, l), TypedAST.Query(t_q_a) ->
            let ids1, shadowContext2 = shadowContext1.AddRange rule_idx (ids |> List.map (fun id -> id.idText))
            let (t_l, maybe_yield_type, l), _, _ = traverse p None shadowContext1 context1 l            
            shadowContext1 <- shadowContext2
            if maybe_yield_type.IsSome then raiseGeneric __LINE__ __SOURCE_FILE__ |> ignore
            match t_l with
            | TypedAST.TypeDecl.Query(a) ->
              let context = context1 
              context1 <- let rec add ctxt ids = match ids with [] -> ctxt | id::ids -> add (context |> Map.add id a) ids in add context ids1
              t_q <- TypedAST.TypeDecl.Query(t_l)
              if ids1.Length <> ids.Length && ids.Length > 0 then
                raise "Internal error: query build mismatch." |> ignore
              result <- result @ [TypedAST.InnerQueryExpression.For([for (id1, id) in List.zip ids1 ids do yield TypedAST.Id.buildFrom {idText = id1; idRange = id.idRange}].Head, 
                                                                    t_l, (t_l, l))]
            | TypedAST.TypeDecl.ImportedType(t_l,p) when t_l.GetInterface("IEnumerable`1") <> null ->
              let t_ienumerable = t_l.GetInterface("IEnumerable`1")
              let a = TypedAST.TypeDecl.ImportedType(t_ienumerable.GetGenericArguments().[0], p)
              let context = context1 
              context1 <- let rec add ctxt ids = match ids with [] -> ctxt | id::ids -> add (context |> Map.add id a) ids in add context ids1
              t_q <- TypedAST.TypeDecl.Query(TypedAST.TypeDecl.ImportedType(t_l,p))
              if ids1.Length <> ids.Length && ids.Length > 0 then
                raise "Internal error: query build mismatch." |> ignore
              result <- result @ [TypedAST.InnerQueryExpression.For([for (id1, id) in List.zip ids1 ids do yield TypedAST.Id.buildFrom {idText = id1; idRange = id.idRange}].Head, 
                                                                    TypedAST.TypeDecl.ImportedType(t_l, p), (TypedAST.TypeDecl.ImportedType(t_l,p), l))]
            | _ -> 
              raise "Error: type mismatch. The argument of a for loop must be enumerable." |> ignore

          | BasicAST.Select(e),TypedAST.Query(t_q_a) ->
            let (t_e,maybe_yield_type, e), _, _ = traverse p None shadowContext1 context1 e
            if maybe_yield_type.IsSome then raiseGeneric __LINE__ __SOURCE_FILE__ |> ignore
            t_q <- TypedAST.TypeDecl.Query(t_e)
            result <- result @ [TypedAST.InnerQueryExpression.Select(t_e,e)]

          | BasicAST.InnerQueryExpression.Let(id, expr),TypedAST.Query(t_q_a) ->          
            let pos = id.idRange
            let id, shadowContext2 = shadowContext1.Add rule_idx id.idText
            let (t_l, maybe_yield_type, l), _, _ = traverse p None shadowContext1 context1 expr
            if maybe_yield_type.IsSome then raiseGeneric __LINE__ __SOURCE_FILE__ |> ignore
            context1 <- context1 |> Map.add  id t_l
            shadowContext1 <- shadowContext2
            result <- result @ [TypedAST.InnerQueryExpression.Let(TypedAST.Id.buildFrom {idText = id; idRange = pos} , t_l, (t_l, l))]

          | BasicAST.ForAll(e), _ ->
            let (t, _, e), _, _ = traverse p None shadowContext1 context1 e 
            if is_bool_type t |> not then
              raise "Error: type mismatch. The argument of FORALL must be a boolean" |> ignore

            t_q <- TypedAST.TypeDecl.ImportedType(typeof<bool>, e.Position)
            result <- result @ [TypedAST.InnerQueryExpression.ForAll(t, e)]

          | BasicAST.Where(e),TypedAST.Query(t_q_a) ->
            let (t, _, e), _, _ = traverse p None shadowContext1 context1 e
            if is_bool_type t |> not then
              raise "Error: type mismatch. The argument of WHERE must be a boolean" |> ignore
            result <- result @ [TypedAST.InnerQueryExpression.Where(t, e)]

          | BasicAST.FindBy (e),TypedAST.Query(TypedAST.TypeDecl.Query(t_q_a)) ->
            let (t, _, e), _, _ = traverse p None shadowContext1 context1 e
            if is_bool_type t |> not then
              raise "Error: type mismatch. The argument of WHERE must be a boolean" |> ignore

            t_q <- t_q_a
            context1 <- context
            result <- result @ [TypedAST.InnerQueryExpression.FindBy(t, e)]
          | BasicAST.Exists(e),TypedAST.Query(t_q_a) ->
            
            let (p_t, _, p), _, _ = traverse p None shadowContext1 context1 e
            if is_bool_type p_t |> not then raise "Expression must be of type boolean" |> ignore
            t_q <- TypedAST.TypeDecl.ImportedType(typeof<bool>, s.Position)
            context1 <- context
            result <- result @ [TypedAST.InnerQueryExpression.Exists(p_t, p)]


          | BasicAST.MinBy(e),TypedAST.Query(TypedAST.TypeDecl.Query(t_q_a)) ->
            let (t_e, maybe_yield_type, e), _,_ = traverse p None shadowContext1 context1 e
            if maybe_yield_type.IsSome then raiseGeneric __LINE__ __SOURCE_FILE__|> ignore
            t_q <- t_q_a
            context1 <- context
            result <- result @ [TypedAST.InnerQueryExpression.MinBy(t_e,e)]
          | BasicAST.MaxBy(e),TypedAST.Query(TypedAST.TypeDecl.Query(t_q_a)) ->
            let (t_e, maybe_yield_type, e),_,_ = traverse p None shadowContext1 context1 e
            if maybe_yield_type.IsSome then raiseGeneric __LINE__ __SOURCE_FILE__ |> ignore
            t_q <- t_q_a
            context1 <- context
            result <- result @ [TypedAST.InnerQueryExpression.MaxBy(t_e,e)]
          | BasicAST.GroupBy(e),TypedAST.Query(t_q_a) -> 
            let (t_e, maybe_yield_type, e),_,_ = traverse p None shadowContext1 context1 e
            if maybe_yield_type.IsSome then raiseGeneric __LINE__ __SOURCE_FILE__ |> ignore
            t_q <- TypedAST.TypeDecl.Query(TypedAST.TypeDecl.Tuple[t_e; TypedAST.TypeDecl.Query(t_q_a)])
            context1 <- context
            result <- result @ [TypedAST.InnerQueryExpression.GroupBy(t_e,e)]
          | BasicAST.GroupByInto(e,id),TypedAST.Query(t_q_a) ->
            let (t_e, maybe_yield_type, e),_,_ = traverse p None shadowContext1 context1 e
            if maybe_yield_type.IsSome then raiseGeneric __LINE__ __SOURCE_FILE__ |> ignore
            t_q <- TypedAST.TypeDecl.Query(TypedAST.TypeDecl.Tuple[t_e; TypedAST.TypeDecl.Query(t_q_a)])
            context1 <- context1 |> Map.add id.idText  (TypedAST.TypeDecl.Tuple[t_e; TypedAST.TypeDecl.Query(t_q_a)])
            result <- result @ [TypedAST.InnerQueryExpression.GroupByInto((t_e,e), TypedAST.Id.buildFrom id)]

          | BasicAST.InnerQueryExpression.Empty(Some(BasicAST.Tuple(parameters)), p),TypedAST.Query(t_q_a) ->
            let parameters = [for param  in parameters do yield convertTypeDecl program param]
            match parameters with 
            | [parameter] -> 
              t_q <- TypedAST.Query(parameter)
              result <- result @ [TypedAST.InnerQueryExpression.Empty((parameter), p)]

            | parameters -> 
              t_q <- TypedAST.Query(TypedAST.Tuple(parameters))
              result <- result @ [TypedAST.InnerQueryExpression.Empty((TypedAST.Tuple(parameters)), p)]
          | BasicAST.InnerQueryExpression.Empty(None, p),TypedAST.Query(t_q_a) ->
            let tp = TypedAST.MaybeQuery(TypedAST.Nothing(ref { Type = None; AlreadyAssigned = false}, p))
            let res = TypedAST.InnerQueryExpression.Empty(tp, p)
            t_q <- TypedAST.Query(tp)
            result <- result @ [res]
            
//            raise "Internal. Not supported operation. GENERIC EMPTY LIST" |> ignore
//            ()
          | BasicAST.InnerQueryExpression.LiteralList(exprs),TypedAST.Query(t_q_a) ->
            let shadowContext1 = shadowContext1
            let context1 = context1
            let exprs = [for expr in exprs do 
                          let (t, maybe_yield_type, arg),_,_ = traverse p None shadowContext1 context1 expr
                          yield t, arg]

            let check_query_type result =
              let rec AUX_check_query_type init_type exprs' =
                match exprs' with
                | [] -> 
                  TypedAST.TypeDecl.Query(init_type), result @ [TypedAST.InnerQueryExpression.LiteralList(exprs)]
                | (t_e, e)::xs -> 
                  if t_e = init_type then              
                    AUX_check_query_type init_type xs
                  else
                    raise "Error: invalid query."
              match exprs with
              | [] -> raise "Error: invalid query."
              | (t_e,e)::xs -> 
                AUX_check_query_type t_e xs
            let t_q1, result1 = check_query_type result
            t_q <- t_q1
            result <- result1
          | BasicAST.Sum(p), _ ->
            //da migliorare
            let t_q1 =
              match t_q with
              | TypedAST.TypeDecl.Query(t) -> t
              | t -> t

            result <- result @ [TypedAST.InnerQueryExpression.Sum p]
            t_q <- t_q1
          | BasicAST.Min(p), _ ->
            //da migliorare
            let t_q1 =
              match t_q with
              | TypedAST.TypeDecl.Query(t) -> t
              | t -> t

            result <- result @ [TypedAST.InnerQueryExpression.Min p]
            t_q <- t_q1
          | BasicAST.Max(p), _ ->
            //da migliorare
            let t_q1 =
              match t_q with
              | TypedAST.TypeDecl.Query(t) -> t
              | t -> t

            result <- result @ [TypedAST.InnerQueryExpression.Max p]
            t_q <- t_q1
          | q -> 
            raise "Error: invalid query." |> ignore
        (t_q, TypedAST.QueryStatements(result)), context1
    convertOuterLoops q shadowContext context |> fst


  and traverseBlock p (maybe_yield_type : Option<TypedAST.TypeDecl>) (b:List<BasicAST.Expression>) (initialShadowContext : ShadowCheckContext) (initialContext:TypecheckContext)  =
    let mutable context = initialContext
    let mutable shadoContext = initialShadowContext
    let mutable result = []
    let mutable yield_t = maybe_yield_type
    for e in b do
      let (t, yield_t', e), shadoContext1, context1 = traverse p yield_t shadoContext context e
           
      do context <- context1
      do shadoContext <- shadoContext1
      do result <- result @ [t,e]
      do yield_t <- yield_t'
    result, yield_t, context

  and op_check_unary p e = op_check_binary p e e


  and op_check_binary p (e1:BasicAST.Expression) e2 op cond position maybe_yield_type res_type shadowContext context =
      let raiseGeneric line file = raise e1.Position (sprintf "Generic error at %A %A" line file)
      let raise = raise e1.Position
             
      let ((t_e1), maybe_yield_type', e1), _, context = traverse p None shadowContext context e1
      let e1 = t_e1, e1
      let ((t_e2), maybe_yield_type'', e2), _, context = traverse p None shadowContext context e2
      let e2 = t_e2, e2
      if maybe_yield_type'.IsSome || maybe_yield_type''.IsSome then
        raise "Yield option type in arithmetic expression" |> ignore
      match t_e1, t_e2 with
      | TypedAST.ImportedType(t_e1,_), TypedAST.ImportedType(t_e2,_) -> 
        
        if cond t_e1 t_e2 then 
          let res = op e1 e2
          let t_res = 
            match res_type with
            | Some res_type ->
              res_type
            | None ->
              TypedAST.ImportedType(t_e1, position)
          (t_res, maybe_yield_type, res), shadowContext, context
        else 
          raise "Invalid type for operator"
      | _, _ when t_e1 = t_e2 ->
        let res = op e1 e2
        let t_res = 
          match res_type with
          | Some res_type ->
            res_type
          | None -> t_e1
        (t_res, maybe_yield_type, res), shadowContext, context
      | _ -> raiseGeneric __LINE__ __SOURCE_FILE__

  and traverse (p : BasicAST.Program) (maybe_yield_type : Option<TypedAST.TypeDecl>) (shadowContext : ShadowCheckContext) (context:TypecheckContext) (b:BasicAST.Expression) : (TypedAST.TypeDecl * Option<TypedAST.TypeDecl> * TypedAST.Expression) * ShadowCheckContext * TypecheckContext = 
    let raiseGeneric line file = raise b.Position (sprintf "Generic error at %A %A" line file)
    let raise = raise b.Position
    match b with
    | BasicAST.NewEntity(args) ->
      let args : List<Id * TypedAST.Block> =
        [for (id, expr) in args do
          let expr = traverseBlock p None expr shadowContext context
          match expr with
          | _, (Some _), _ -> let _ = raise "yield not allowed" in ()
          | expr, None, _ ->
            yield TypedAST.Id.buildFrom id,expr]

      let (entities_fields : List<Id * TypedAST.TypeDecl * Map<Id, TypedAST.Field>>) = [for field in entities do 
                                                                                                  let tp, fields = field.Value
                                                                                                  yield field.Key, tp, fields |> Map.filter(fun f_id f -> f.IsExternal.IsNone)]

//      let rec try_match (entities_fields : List<Id * TypedAST.TypeDecl * List<TypedAST.Field>>) (args : List<Id * TypedAST.Block>) =
//        let rec AUX_try_match (entities_fields : List<Id * TypedAST.TypeDecl * List<TypedAST.Field>>) : (TypedAST.TypeDecl * (Id * TypedAST.Block) list) option =
//          match entities_fields with
//          | [] -> None
//          | entity :: entities ->
//            let rec try_match_entity (entity_fields : Id * TypedAST.TypeDecl * List<TypedAST.Field>) (args : List<Id * TypedAST.Block>) =
//              let entity_name, entity_type, entity_fields = entity_fields
//              let entity_fields = ResizeArray(entity_fields)
//
//              let rec AUX_try_match_entity (args : List<Id * TypedAST.Block>) =
//
//                match args with
//                | [] -> []
//                | (arg_id, arg_expr) :: args -> 
//                  match entity_fields.FindIndex(fun f -> f.Name = arg_id) with
//                  | -1 -> []
//                  | n -> 
//                    let elem = entity_fields.[n]
//                    entity_fields.RemoveAt(n)
//                    (elem, arg_expr) :: AUX_try_match_entity args                  
//              let res = AUX_try_match_entity args
//              if res.Length = args.Length && entity_fields.Count = 0 then
//                let res1 = ResizeArray()
//                  
//                for field, block in res do
//                  let tp_last_expr, last_expr = block |> Seq.last
//                  if tp_last_expr = field.Type then res1.Add (field.Name, block)
//                if res.Length = res1.Count then 
//                  Some (entity_type, res1 |> Seq.toList)
//                else None
//              else None
//
//            let res = try_match_entity entity args
//
//            match res, entity with
//            | None, _ -> AUX_try_match entities
//            | Some res, (_, entity_type, _) -> Some res
//
//        let res = AUX_try_match entities_fields
//        
//        res
//
//      let try_match_entity (entities_fields : List<Id * TypedAST.TypeDecl * List<TypedAST.Field>>) 
//                           (args : List<Id * TypedAST.Block>) =




      let candidate_entities_fields_and_args =
        [for (entity_id, entity_type, entity_fields) in entities_fields do
          let entity_fields = entity_fields |> Seq.map(fun f -> f.Value)
          let entity_fields = entity_fields |> Seq.sortBy(fun f -> f.Name.idText) |> Seq.map(fun f ->f.Name.idText, f) |> Map.ofSeq
          let entity_fields1 = entity_fields |> Seq.map(fun f -> TypedAST.Id.buildFrom {idText = entity_id.idText + "." + f.Value.Name.idText; idRange = f.Value.Name.idRange}) |> Seq.toList |> Set.ofSeq
          let args = args |> Seq.sortBy(fun (id, _) -> id.idText) |> Seq.toList
          let args1 = args
                      |> Seq.map(fun (arg, block) -> 
                                  match arg.idText.Split('.') |> Seq.toList with
                                  | [x] -> TypedAST.Id.buildFrom {idText = entity_id.idText + "." + x; idRange = arg.idRange}
                                  | x   -> arg) |> Set.ofSeq
          if entity_fields1.Count = args1.Count && (Set.difference entity_fields1 args1).IsEmpty then
            yield (entity_id, entity_type, Seq.zip entity_fields1 (entity_fields |> Seq.map(fun f -> f.Value))), args1 |> Seq.mapi(fun i arg -> arg, snd args.[i])]
      
      if candidate_entities_fields_and_args = [] then raise "Not match with any casanova entities found."
      else
        let initial_candidate_entities_fields_and_args = candidate_entities_fields_and_args
        let matches_found = ref 0
        //entity fields and args of the new records are ordered by name at this point
        let rec try_match_candidate_entity_with_args (candidate_entities_fields_and_args : ((Id * TypedAST.TypeDecl * seq<Id * TypedAST.Field>) * seq<Id * TypedAST.Block>) list) =
          match candidate_entities_fields_and_args with
          | [] -> 
            let x = initial_candidate_entities_fields_and_args.Head 
            let ((entity_id, entity_type, entity_fields), args) = x
            for (e_f, f), (arg, arg_block) in Seq.zip entity_fields args do
              let arg_type = arg_block |> List.rev |> List.head |> fst
              match f.Type, arg_type with
              | t1, t2 when t1 <> t2 -> 
                //an arg at this point must be: entity_ID . arg_ID
                let arg = { idText = arg.idText.Split('.').[1]; idRange = arg.idRange}
              //type error, record field names are fine but the type is wrong
                raise ("Type error, record field names are fine but the type is wrong: " + arg.ToString()) |> ignore
              | _ -> ()
            None
          | x :: xs -> 
            let same_entity = ref true
            let ((entity_id, entity_type, entity_fields), args) = x
            for (e_f, f), (arg, arg_block) in Seq.zip entity_fields args do
              if !same_entity then
                let arg_type = arg_block |> List.rev |> List.head |> fst
                match f.Type, arg_type with
                | t1, t2 when t1 <> t2 -> same_entity := false
                | _ -> ()
            if not !same_entity then try_match_candidate_entity_with_args xs
            else 
              let ((tp_id, _, _), _) = x
              if tp_id = domain.Head then Some x
              else try_match_candidate_entity_with_args xs

          
        match try_match_candidate_entity_with_args candidate_entities_fields_and_args with
        | None -> raise ("Unmatched record instantiation. Entity name: " + entityName.idText)
        | Some ((_,entity_type,_), fields) -> 
          let fields = fields |> Seq.map(fun (f, b) -> TypedAST.Id.buildFrom {idText = (f.idText.Split('.')).[1]; idRange = f.idRange}, b) |> Seq.toList
          (entity_type, maybe_yield_type, TypedAST.Expression.NewEntity(fields)), shadowContext, context
    
    | BasicAST.Expression.Add(e1, e2) -> op_check_binary p e1 e2 
                                                  (fun e1 e2 -> 
                                                    match fst e1 with
                                                    | TypedAST.TypeDecl.Query(_) ->
                                                      TypedAST.Expression.AddToQuery(e1, e2)
                                                    |_ -> TypedAST.Expression.Add(e1, e2))
                                                  (fun t_e1 t_e2 -> 
                                                      t_e1 = typeof<int> || t_e1 = typeof<float> || t_e1 = typeof<float32> || t_e1 = typeof<string> ||
                                                      t_e1.GetMethods() |> Seq.exists (fun mi -> mi.Name = "op_Addition" && hasMatchingParameters mi t_e1 t_e2))
                                                  b.Position maybe_yield_type None shadowContext context 
    
    | BasicAST.Expression.Div(e1, e2) -> op_check_binary p e1 e2 
                                                  (fun e1 e2 -> TypedAST.Expression.Div(e1, e2))
                                                  (fun t_e1 t_e2 -> t_e1 = typeof<int> || t_e1 = typeof<float> || t_e1 = typeof<float32> ||
                                                                    t_e1.GetMethods() |> Seq.exists (fun mi -> mi.Name = "op_Division" && hasMatchingParameters mi t_e1 t_e2))
                                                  b.Position maybe_yield_type None shadowContext context

    | BasicAST.Expression.Sub(e1, e2) -> op_check_binary p e1 e2 
                                                  (fun e1 e2 -> 
                                                    match fst e1 with
                                                    | TypedAST.TypeDecl.Query(_) ->
                                                      TypedAST.Expression.SubtractToQuery(e1, e2)
                                                    |_ -> TypedAST.Expression.Sub(e1, e2))
                                                  (fun t_e1 t_e2 -> t_e1 = typeof<int> || t_e1 = typeof<float> || t_e1 = typeof<float32> ||
                                                                           t_e1.GetMethods() |> Seq.exists (fun mi -> mi.Name = "op_Subtraction" && hasMatchingParameters mi t_e1 t_e2))
                                                  b.Position maybe_yield_type None shadowContext context

    | BasicAST.Expression.Mul(e1, e2) -> op_check_binary p e1 e2 
                                                  (fun e1 e2 -> TypedAST.Expression.Mul(e1, e2))
                                                  (fun t_e1 t_e2 -> 
                                                    t_e1 = typeof<int> || t_e1 = typeof<float> || t_e1 = typeof<float32> ||
                                                    t_e1.GetMethods() |> Seq.exists (fun mi -> mi.Name = "op_Multiply" && hasMatchingParameters mi t_e1 t_e2))
                                                  b.Position maybe_yield_type None shadowContext context

    | BasicAST.Expression.Modulus(e1, e2) -> op_check_binary p e1 e2 
                                                  (fun e1 e2 -> TypedAST.Expression.Modulus(e1, e2))
                                                  (fun t_e1 t_e2 -> t_e1 = typeof<int> || t_e1 = typeof<float> || t_e1 = typeof<float32> ||
                                                                           t_e1.GetMethods() |> Seq.exists (fun mi -> mi.Name = "op_Modulus" && hasMatchingParameters mi t_e1 t_e2))
                                                  b.Position maybe_yield_type None shadowContext context

    | BasicAST.Expression.And(e1, e2) -> op_check_binary p e1 e2 
                                                  (fun e1 e2 -> TypedAST.Expression.And(e1, e2))
                                                  (fun t_e1 t_e2 -> t_e1 = typeof<bool>  ||
                                                                           t_e1.GetMethods() |> Seq.exists (fun mi -> mi.Name = "op_BooleanAnd" && hasMatchingParameters mi t_e1 t_e2))
                                                  b.Position maybe_yield_type None shadowContext context

    | BasicAST.Expression.Or(e1, e2) -> op_check_binary p e1 e2 
                                                  (fun e1 e2 -> TypedAST.Expression.Or(e1, e2))
                                                  (fun t_e1 t_e2 -> t_e1 = typeof<bool>  ||
                                                                    t_e1.GetMethods() |> Seq.exists (fun mi -> mi.Name = "op_BooleanOr" && hasMatchingParameters mi t_e1 t_e2))
                                                  b.Position maybe_yield_type None shadowContext context

    | BasicAST.Expression.Equals(e1, e2) -> op_check_binary p e1 e2 
                                                  (fun e1 e2 -> TypedAST.Expression.Equals(e1, e2))
                                                  (fun t_e1 t_e2 -> true  ||
                                                                    t_e1.GetMethods() |> Seq.exists (fun mi -> mi.Name = "op_Equality" && hasMatchingParameters mi t_e1 t_e2))
                                                  b.Position maybe_yield_type (Some (TypedAST.ImportedType(typeof<bool>,b.Position))) shadowContext context

    | BasicAST.Expression.Greater(e1, e2) -> op_check_binary p e1 e2 
                                                  (fun e1 e2 -> TypedAST.Expression.Greater(e1, e2))
                                                  (fun t_e1 t_e2 -> true ||
                                                                    t_e1.GetMethods() |> Seq.exists (fun mi -> mi.Name = "op_GreaterThan" && hasMatchingParameters mi t_e1 t_e2))
                                                  b.Position maybe_yield_type (Some (TypedAST.ImportedType(typeof<bool>,b.Position))) shadowContext context

    | BasicAST.Expression.Not(e) -> op_check_unary p e 
                                                  (fun e _ -> TypedAST.Expression.Not(e))
                                                  (fun _ _ -> true )
                                                  b.Position maybe_yield_type (Some (TypedAST.ImportedType(typeof<bool>,b.Position))) shadowContext context


    | BasicAST.Expression.IndexOf(lst, idx) ->
      let pos = lst.idRange
      let (t_idx, maybe_yield_type',idx), _, context = traverse p None shadowContext context idx
      if maybe_yield_type'.IsSome then raiseGeneric __LINE__ __SOURCE_FILE__
      let lst = shadowContext.Lookup lst.idText
      let lst_t = Lookup lst [] pos globalContext context PriorInformationType.Unknown

      match lst_t with
      | TypedAST.TypeDecl.Query(t) -> 
        (t, maybe_yield_type, TypedAST.Expression.IndexOf(TypedAST.Id.buildFrom {idText = lst; idRange = pos}, (t_idx, idx))), shadowContext, context
      | TypedAST.ImportedType(t,p) ->
        let t_ienum = t.GetInterface("IEnumerable`1")
        if t_ienum <> null then
          let t_ienum_arg = t_ienum.GetGenericArguments().[0]
          let t_arg = TypedAST.ImportedType(t_ienum_arg, p)
          (t_arg, maybe_yield_type, TypedAST.Expression.IndexOf(TypedAST.Id.buildFrom {idText = lst; idRange = pos}, (t_idx, idx))), shadowContext, context
        else raiseGeneric __LINE__ __SOURCE_FILE__
      | _ -> raiseGeneric __LINE__ __SOURCE_FILE__


    | BasicAST.Expression.Let(id,e) -> 
      let pos = id.idRange
      let (t_e, maybe_yield_type', e), _, context = traverse p None shadowContext context e
      if maybe_yield_type'.IsSome then raiseGeneric __LINE__ __SOURCE_FILE__
      let id, shadowContext = shadowContext.Add rule_idx (id.idText)
      let context = context |> Map.add id  t_e
      (TypedAST.TypeDecl.Unit pos, maybe_yield_type , TypedAST.Expression.Let(TypedAST.Id.buildFrom {idText = id; idRange = pos}, t_e, (t_e,e))), shadowContext, context

    | BasicAST.Expression.IfThenElse(c,t,e) ->
      let b_t, maybe_yield_type, _ = traverseBlock p maybe_yield_type t shadowContext context
      let t_t, _ = b_t.[b_t.Length - 1]

      let b_e, maybe_yield_type, _ = traverseBlock p maybe_yield_type  e shadowContext context
      let e_t, _ = b_e.[b_e.Length - 1]

      let (t_c, _, c), _, _ = traverse p None shadowContext context c

      if is_bool_type t_c |> not then raise "The condition must be of type bool." |> ignore

      if t_t = e_t then
        (t_t, maybe_yield_type, TypedAST.Expression.IfThenElse((t_c, c),b_t,b_e)), shadowContext, context
      else
        raise "The else and then blocks should be of the same type."
    | BasicAST.Expression.IfThen(c,t) ->
      let b_t, maybe_yield_type, _ = traverseBlock p maybe_yield_type t shadowContext context
      let t_t, _ = b_t.[b_t.Length - 1]
      let (t_c, _, c), _, _ = traverse p None shadowContext context c
      if is_bool_type t_c |> not then raise "The condition must be of type bool." |> ignore
        
      (t_t, maybe_yield_type, TypedAST.Expression.IfThen((t_c, c),b_t)), shadowContext, context

    | BasicAST.Choice(interruptible,choices) ->
      let all_choices_are_not_atomic = ref true
      let program = p
      let choices1 =
        [for c, b, pos in choices do
          let b, maybe_yield_type, _ = traverseBlock program maybe_yield_type b shadowContext context
          let b_t, _ = b.[b.Length - 1]
          let (t_c, _, c), _, _ = traverse p None shadowContext context c
          
          if is_bool_type t_c |> not then raise "The condition must be of type bool." |> ignore
          
          if maybe_yield_type.IsNone then all_choices_are_not_atomic := false
          yield t_c,c,b,pos, maybe_yield_type]
      

      if choices1.Length = 0 then raise "Error in choice expression." |> ignore

      let _,_,_,_,first_choice_type = choices1.Head
      if choices1.Tail |> Seq.forall(fun (_,_,_,_,t) -> t = first_choice_type) |> not then raiseGeneric __LINE__ __SOURCE_FILE__  |> ignore
      let choices1 = choices1 |> List.map(fun (t_c,c,b,p,_) -> (t_c,c),b,p)
      (TypedAST.TypeDecl.Unit b.Position, first_choice_type, TypedAST.Expression.Choice(interruptible,choices1, b.Position)), shadowContext, context

    | BasicAST.Parallel(_parallel) ->
      let all_parallels_are_not_atomic = ref true
      let parallel1 =
        [for b, pos in _parallel do
            let b, maybe_yield_type, _ = traverseBlock p maybe_yield_type b shadowContext context
            let b_t, _ = b.[b.Length - 1]
            if maybe_yield_type.IsNone then all_parallels_are_not_atomic := false
            yield b, pos, maybe_yield_type]
      if parallel1.Length = 0 then raise "Error in parallel expression." |> ignore
      
      let _,_,first_parallel_type = parallel1.Head
      if parallel1.Tail |> Seq.forall(fun (_,_,t) -> t = first_parallel_type) |> not then raiseGeneric __LINE__ __SOURCE_FILE__ |> ignore
      let parallel1 = parallel1 |> List.map(fun(b,p,_) -> b,p)
      (TypedAST.TypeDecl.Unit b.Position, first_parallel_type, TypedAST.Expression.Parallel(parallel1, b.Position)), shadowContext, context
    
    | BasicAST.Expression.Yield(v) ->
      
      let (t_v, maybe_yield_type,v), _, context = traverse p None shadowContext context v      
      if maybe_yield_type.IsSome then raise "nested yield not allowed" |> ignore

      match t_v, v with
      | (TypedAST.TypeDecl.Tuple(_)), (TypedAST.Expression.Tuple(args)) ->
        let rec check_type (domain: List<Id>) (values_to_commit : List<TypedAST.TypedExpression>) =
          match values_to_commit, domain with
          | [],[] -> []
          | (t_x, _)::xs, y::ys -> 
            let t_y = Lookup y.idText [] b.Position globalContext context PriorInformationType.Instance
            if t_y = t_x then t_y.IsSystemType :: check_type ys xs
            else raise "Error: type mismatch in a yield statement." |> ignore
                 []
          | _ -> raise "Error: too many arguments in a yield statement." |> ignore
                 []
        let yield_tps = check_type domain args
        (TypedAST.TypeDecl.Unit v.Position, Some t_v, TypedAST.Expression.Yield(t_v,v)), shadowContext, context
      
      | _ ->
        raise "Internal Error: yield must return a tuple."


    | BasicAST.Expression.Wait(t) ->
      let (t_t, maybe_yield_type',t), _, context = traverse p None shadowContext context t
      if maybe_yield_type'.IsSome then raiseGeneric __LINE__ __SOURCE_FILE__|> ignore
      match t_t with
      | TypedAST.ImportedType(t_t',p) when t_t' = typeof<int> || t_t' = typeof<float32> || t_t' = typeof<float> || t_t' = typeof<bool>   ->
        (TypedAST.TypeDecl.Unit t.Position, maybe_yield_type, TypedAST.Expression.Wait(t_t,t)), shadowContext, context
      | _ -> raise "Error: type mismatch. The argument passed to wait is not a boolean or numeric."

    | BasicAST.Expression.Tuple(l) -> 
      let l = [for e in l do 
                  let (e,maybe_yield_type', c) = traverseBlock p None e shadowContext context
                  if maybe_yield_type'.IsSome then raiseGeneric __LINE__ __SOURCE_FILE__ |> ignore
                  yield! e]
      let t_l = TypedAST.TypeDecl.Tuple [for t_e,e in l do yield t_e ]
      (t_l, maybe_yield_type, TypedAST.Expression.Tuple(l)), shadowContext, context
    | BasicAST.Expression.For(i,l,b) ->
      let (t,maybe_yield_type',e), _, context_l = traverse p None shadowContext context l

      let i_type =
        match t with
        | TypedAST.TypeDecl.Query(t) -> t
        | TypedAST.TypeDecl.ImportedType(st, p) when st.GetGenericTypeDefinition() = typedefof<seq<_>> || st.GetInterface("IEnumerable`1") <> null -> 
          TypedAST.TypeDecl.ImportedType(st.GetGenericArguments().[0], p)

      let l_t = t,e
      if maybe_yield_type'.IsSome then raiseGeneric __LINE__ __SOURCE_FILE__ |> ignore
      let i1, shadowContext1 = shadowContext.AddRange rule_idx (i |> List.map(fun id -> id.idText))
      
      if i1.Length = 0 then raise "Error in for indexes." |> ignore
      let context = context |> Map.add i1.Head i_type
      
      let b_t, maybe_yield_type, context = traverseBlock p maybe_yield_type b shadowContext1 context

      if i.Length <> i1.Length then
        raise "Internal error: query build mismatch." |> ignore
      (TypedAST.TypeDecl.Unit i.Head.idRange, maybe_yield_type, TypedAST.Expression.For([for p,i in Seq.zip i i1 do yield TypedAST.Id.buildFrom {idText = i; idRange = p.idRange}] , l_t, b_t)), shadowContext, context
    | BasicAST.Expression.While(c,b) ->
      let (t_c, _, c_t), _, _ = traverse p None shadowContext context c
      if is_bool_type t_c |> not then raise "The condition must be of type bool." |> ignore
      let b_t, maybe_yield_type, context = traverseBlock p maybe_yield_type b shadowContext context
      (TypedAST.TypeDecl.Unit c.Position, maybe_yield_type, TypedAST.Expression.While((t_c, c_t),b_t)), shadowContext,context

    | BasicAST.Expression.Cast(id, e, pos) ->
      let (e_e, _, e_t), _, _ = traverse p None shadowContext context e
      let t = convertTypeDecl p id

      (t, maybe_yield_type, TypedAST.Expression.Cast(t, (e_e, e_t), pos)), shadowContext,context


    | BasicAST.Expression.Query(q) -> 
      let t_q, q = traverseQuery p q shadowContext context
      (t_q, maybe_yield_type, TypedAST.Expression.Query(q)), shadowContext,context
    | BasicAST.New(BasicAST.TypeDecl.Imported(id, parameters), args) ->
      let tp = convertTypeDecl program (BasicAST.TypeDecl.Imported(id, parameters))
      let args = [ for a in args do 
                      let (t, maybe_yield_type, arg), _, _ = traverse p None shadowContext context a 
                      if maybe_yield_type.IsSome then raiseGeneric __LINE__ __SOURCE_FILE__ |> ignore
                      yield t,arg ]

      let t_args = [ for a in args do 
                      let (t_arg, _) = a
                      yield t_arg ]
      (tp, maybe_yield_type, TypedAST.Expression.Call(TypedAST.Constructor(tp, args))), shadowContext, context

    | BasicAST.New(BasicAST.TypeDecl.TypeName(id), args) ->
      let args = [ for a in args do 
                      let (t, maybe_yield_type, arg), _, _ = traverse p None shadowContext context a 
                      if maybe_yield_type.IsSome then raiseGeneric __LINE__ __SOURCE_FILE__ |> ignore
                      yield t, arg ]

      let t_args = [ for a in args do 
                      let (t_arg, _) = a
                      yield t_arg ]

      if globalContext.ContainsKey(id.idText) then   
        let tp = LookupCreate id.idText id.idRange t_args globalContext 
        //(tp, maybe_yield_type, TypedAST.Expression.Call(TypedAST.Static(tp, {idText = "Create"; idRange = id.idRange}, args))), shadowContext, context
        (tp, maybe_yield_type, TypedAST.Expression.Call(TypedAST.Constructor(tp, args))), shadowContext, context

      else
        let tp = convertTypeDecl program (BasicAST.TypeDecl.TypeName(id))
        (tp, maybe_yield_type, TypedAST.Expression.Call(TypedAST.Constructor(tp, args))), shadowContext, context
        


    | BasicAST.Expression.Call(BasicAST.MaybeInstance(id,f,args)) when (f.idText.ToLower()).EndsWith("create") -> 
        let args = [ for a in args do 
                        let (t, maybe_yield_type, arg), _, _ = traverse p None shadowContext context a 
                        if maybe_yield_type.IsSome then raiseGeneric __LINE__ __SOURCE_FILE__ |> ignore
                        yield t, arg]

        let t_args = [ for a in args do 
                        let (t_arg, _) = a
                        yield t_arg ]

        let tp = LookupCreate id.idText id.idRange t_args globalContext                
        (tp, maybe_yield_type, TypedAST.Expression.Call(TypedAST.Constructor(tp, args))), shadowContext, context
//        (tp, maybe_yield_type, TypedAST.Expression.Call(TypedAST.Static(tp, f, args))), shadowContext, context

    | BasicAST.Expression.Call(BasicAST.MaybeInstance(id,f,args)) -> 
        let args = [ for a in args do 
                        let (t, maybe_yield_type, arg), _, _ = traverse p None shadowContext context a 
                        if maybe_yield_type.IsSome then raiseGeneric __LINE__ __SOURCE_FILE__ |> ignore
                        yield t, arg ]

        let t_args = [ for a in args do 
                        let (t_arg, _) = a
                        yield t_arg ]

        let ids = id.idText.Split [|'.'|]

        // [a;b;c] -> "a.b.c"
        let ids_head = shadowContext.Lookup ids.[0]
        let ids = ids_head :: (ids |> Seq.skip 1 |> Seq.toList)
        let id =  { idText = Seq.fold(fun s id -> s + "." + id) ids.Head ids.Tail; idRange = id.idRange}

        if context.ContainsKey(ids.[0]) then
          let id_tp = Lookup id.idText [] id.idRange globalContext context PriorInformationType.Instance
          let tp = LookupTypeMethod id_tp id.idRange (Some f.idText) t_args globalContext
          let range = id.idRange

          let rec id_tps (id : string) =
            let rec id_tps_AUX (id : string list) acc =
            
              match id with
              | [] -> []
              | x::xs -> Lookup (acc + "." + x) xs range globalContext context PriorInformationType.Instance :: (id_tps_AUX xs (acc + "." + x))
            let id = id.Split [|'.'|] |> Seq.toList
            (Lookup id.Head id.Tail range globalContext context PriorInformationType.Instance) :: id_tps_AUX id.Tail id.Head
          
          let id_tps = id_tps id.idText

          (tp, maybe_yield_type, TypedAST.Expression.Call(TypedAST.Instance(TypedAST.Id.buildFrom(id, id_tps), TypedAST.Id.buildFrom f, args))), shadowContext, context
        else
          match OpenContext.TryResolveTypeName id.idText with
          | None ->
            let tp = LookupIdMethod id.idText id.idRange f.idText t_args globalContext context PriorInformationType.Static
            (tp, maybe_yield_type, TypedAST.Expression.Call(TypedAST.Static(tp, TypedAST.Id.buildFrom f, args))), shadowContext, context
          | Some t ->
            let t = TypedAST.TypeDecl.ImportedType(t, id.idRange)
            let tp = LookupTypeMethod t id.idRange (Some f.idText) t_args globalContext 
            (tp, maybe_yield_type, TypedAST.Expression.Call(TypedAST.Static(t, TypedAST.Id.buildFrom f, args))), shadowContext, context

    | BasicAST.Expression.Call(BasicAST.Static(tp,f,args)) -> 
        let tp = convertTypeDecl program tp
        let args = [ for a in args do 
                        let (t, maybe_yield_type, arg), _, _ = traverse p None shadowContext context a 
                        if maybe_yield_type.IsSome then raiseGeneric __LINE__ __SOURCE_FILE__ |> ignore
                        yield t, arg ]

        let t_args = [ for a in args do 
                        let (t_arg, _) = a
                        yield t_arg ]
        match f with 
        | Some f ->
          let tp = LookupTypeMethod tp tp.Position (Some f.idText) t_args globalContext
          (tp, maybe_yield_type, TypedAST.Expression.Call(TypedAST.Static(tp, TypedAST.Id.buildFrom f, args))), shadowContext, context
        | None ->
          match tp with           
          | TypedAST.TypeDecl.EntityName(id) ->
              let tp = LookupCreate id.idText id.idRange t_args globalContext
              //(tp, maybe_yield_type, TypedAST.Expression.Call(TypedAST.Static(tp, {idText = "Create"; idRange = id.idRange}, args))), shadowContext, context
              (tp, maybe_yield_type, TypedAST.Expression.Call(TypedAST.Constructor(tp, args))), shadowContext, context
          | _ ->
            (tp, maybe_yield_type, TypedAST.Expression.Call(TypedAST.Constructor(tp, args))), shadowContext, context

    | BasicAST.Expression.Literal(BasicAST.String(s, p)) ->
      (TypedAST.TypeDecl.ImportedType(typeof<string>,p), maybe_yield_type, TypedAST.Expression.Literal(BasicAST.String(s,p))), shadowContext, context
    | BasicAST.Expression.Literal(BasicAST.Int(i, p)) ->
      (TypedAST.TypeDecl.ImportedType(typeof<int>,p), maybe_yield_type, TypedAST.Expression.Literal(BasicAST.Int(i, p))), shadowContext, context
    | BasicAST.Expression.Literal(BasicAST.Float(f, p)) ->
      (TypedAST.TypeDecl.ImportedType(typeof<float32>,p), maybe_yield_type, TypedAST.Expression.Literal(BasicAST.Float(f, p))), shadowContext, context
    | BasicAST.Expression.Literal(BasicAST.Bool(b, p)) ->
      (TypedAST.TypeDecl.ImportedType(typeof<bool>,p), maybe_yield_type, TypedAST.Expression.Literal(BasicAST.Bool(b, p))), shadowContext, context
    | BasicAST.Expression.Literal(BasicAST.LUnit(p)) ->
      (TypedAST.TypeDecl.Unit p, maybe_yield_type, TypedAST.Expression.Literal(BasicAST.LUnit(p))), shadowContext, context
    | BasicAST.Expression.Id(id) ->
      

      
      let c = entities

//      let id1 = OpenContext.TrySolveIdFromInheritedTypes id

      let p = id.idRange
      let id1 = shadowContext.Lookup id.idText

      let range = id.idRange

      let rec id_tps (id : string) =
        let rec id_tps_AUX (id : string list) acc =
            
          match id with
          | [] -> []
          | x::xs -> Lookup (acc + "." + x) xs range globalContext context PriorInformationType.Unknown :: (id_tps_AUX xs (acc + "." + x))
        let id = id.Split [|'.'|] |> Seq.toList
        (Lookup id.Head id.Tail range globalContext context PriorInformationType.Unknown) :: id_tps_AUX id.Tail id.Head
          
      let id_tps = id_tps id1



      let t_id = Lookup id1 [] p globalContext context PriorInformationType.Unknown


      (t_id, maybe_yield_type, TypedAST.IdExpr(TypedAST.Id.buildFrom({idText = id1; idRange = p}, id_tps) )), shadowContext, context

    | BasicAST.Range(f,t,pos) ->
      let (f_t, _, f), _, _ = traverse p None shadowContext context f
      let (t_t, _, t), _, _ = traverse p None shadowContext context t
      let f , t =
        match f_t, t_t with
        | TypedAST.ImportedType(f_t, fe), TypedAST.ImportedType(t_t, te) when t_t.FullName = typeof<int>.FullName && f_t.FullName = typeof<int>.FullName ->
          let f = TypedAST.ImportedType(f_t, fe), f
          let t = TypedAST.ImportedType(t_t, te), t
          let t = TypedAST.ImportedType(t_t, te), 
                  (TypedAST.Add((TypedAST.ImportedType(t_t, te), TypedAST.Expression.Literal(BasicAST.Int(1, pos))), 
                                (TypedAST.ImportedType(t_t, te), TypedAST.Sub(t, f))))
          f, t
        | _ -> raiseGeneric __LINE__ __SOURCE_FILE__
      (TypedAST.TypeDecl.Query(f_t), maybe_yield_type, TypedAST.Range(f, t, pos)), shadowContext, context

    | BasicAST.ConcatQuery(qs) ->
      let qs = 
        [for q in qs do

          let (t_q, _, q), _, _ = traverse p maybe_yield_type shadowContext context q 
          yield (t_q, q)]
      if qs.Length = 0 then raise "Error. Concat query without body." |> ignore

      let q = qs.Head
      let (t_q, _) = q
      let qs = qs.Tail

      qs |> Seq.iter(fun (t_q1, _) -> 
        if (are_equal_collection t_q t_q1) then raise (sprintf "Error: invalid query. Expected type %A, given type %A" t_q t_q1) |> ignore)

      let qs = q :: qs |> Seq.map(fun (t, q) -> t, q) |> Seq.toList

//      let ls =
//        [for i, (t, q) in qs |> Seq.mapi(fun i e -> i,e) do
//          yield t, TypedAST.Let({idText = "qi"; idRange = t.Position}, t, (t,q))]
      
      let qs = TypedAST.ConcatQuery(qs)

      (t_q, maybe_yield_type, qs), shadowContext, context

    | BasicAST.Expression.Maybe(BasicAST.Nothing(p)) -> 
      (TypedAST.MaybeType(TypedAST.Nothing(ref { Type = None; AlreadyAssigned = false}, p)), maybe_yield_type, TypedAST.Maybe(TypedAST.MaybeExpr.NothingExpr(p))), shadowContext, context
    
    
    | BasicAST.Expression.Query([BasicAST.InnerQueryExpression.Empty(None,p)]) -> 
      let tp = TypedAST.MaybeQuery(TypedAST.Nothing(ref { Type = None; AlreadyAssigned = false}, p))
      (tp, maybe_yield_type, TypedAST.Expression.Query(TypedAST.QueryStatements([TypedAST.InnerQueryExpression.Empty(tp, p)]))), shadowContext, context
    
    
    | BasicAST.Expression.Maybe(BasicAST.Just(e)) ->
      let (t_e, _, e), _, _ = traverse p None shadowContext context e
      (TypedAST.MaybeType(TypedAST.Just(t_e)), 
       maybe_yield_type, 
       TypedAST.Maybe(TypedAST.MaybeExpr.JustExpr(t_e, e))), 
      shadowContext, context
    | BasicAST.Expression.AppendToQuery(e1, e2) ->
      let (t_e1, _, e1), _, _ = traverse p None shadowContext context e1
      let (t_e2, _, e2), _, _ = traverse p None shadowContext context e2
      match t_e2 with
      | TypedAST.TypeDecl.Query(TypedAST.MaybeQuery(t)) when TypedAST.Just(t_e1) = t -> 
        ()
      | TypedAST.TypeDecl.Query(TypedAST.MaybeQuery(t)) when TypedAST.Just(t_e1) <> t ->
        raise (sprintf "Error: invalid query. Expected type %A, given type %A" t_e1 t) |> ignore        
      | TypedAST.TypeDecl.Query(t) when t_e1 <> t ->
        raise (sprintf "Error: invalid query. Expected type %A, given type %A" t_e1 t) |> ignore
      | TypedAST.TypeDecl.Query(t) when t_e1 = t -> ()
      | _ -> raise (sprintf "Error: expected query type, given type %A" t_e2) |> ignore
      
      (t_e2, 
       maybe_yield_type, 
       TypedAST.Expression.AppendToQuery((t_e1, e1), (t_e2, e2))), 
      shadowContext, context
    | BasicAST.Lambda(args, block) -> 
      let args = args |> List.map(fun (id, t) -> 
                                    match t with
                                    | None -> TypedAST.Id.buildFrom id, None
                                    | Some t -> TypedAST.Id.buildFrom id, Some (convertTypeDecl p t))
      let block, _, _ = traverseBlock p None block shadowContext context
      let t_block = block |> List.rev |> List.head |> fst
      (t_block, 
       maybe_yield_type, 
       TypedAST.Lambda(args, block)), 
      shadowContext, context

    | e -> raise (sprintf "%A" e)
  let initialContext = 
    [
      yield! args
      // world, this, fields of this
      yield "world", TypedAST.TypeDecl.EntityName(worldName)
      yield "this", TypedAST.TypeDecl.EntityName(entityName)
      yield "self", TypedAST.TypeDecl.EntityName(entityName)
      yield "dt", TypedAST.TypeDecl.ImportedType(typeof<float32>, Position.Empty)
      for f_self in fst globalContext.[entityName.idText] do
        yield f_self.Key, f_self.Value      
    ] |> Map.ofList
  
  let shadowContext =
    { values = 
      [for v in initialContext do
        yield v.Key, (-1,-1) ] |> Map.ofList }


  //look u p domain
  for d in domain do
    if d <> entityName then
      Lookup d.idText [] d.idRange globalContext initialContext |> ignore
    //(pos : Position) (globalContext:GlobalTypecheckContext) (context:TypecheckContext) : TypedAST.TypeDecl =
  let te, yield_type, context = traverseBlock program None b shadowContext initialContext

  match yield_type with
  | Some t -> ()
  | _ -> 
    let type_to_check = fst te.[te.Length-1] 
    match type_to_check, domain with
    | (TypedAST.TypeDecl.Tuple(args)), y::ys ->
      let domain = y::ys
      let rec check_type (domain: List<Id>) (tps_to_commit : List<TypedAST.TypeDecl>) =
        match tps_to_commit, domain with
        | [],[] -> ()
        | x::xs, y::ys -> 
          let y = Lookup y.idText [] y.idRange globalContext context PriorInformationType.Instance
          if x = y then check_type ys xs
          else raise x.Position "Error: type mismatch in rule."
        | _ -> raise type_to_check.Position "Error: type mismatch in rule."
      do check_type domain args
    | x, [y] ->
      let y = 
        if is_create |> not then Lookup y.idText [] y.idRange globalContext context PriorInformationType.Instance
        else TypedAST.TypeDecl.EntityName(y)
      if x <> y then raise x.Position ("Error: type mismatch in rule.")
    | _ ->
      raise type_to_check.Position "Error: type mismatch in rule." 

  te
and convertRule p fields (worldName:Id) (entityName:Id) (globalContext:GlobalTypecheckContext) rule_idx (r:BasicAST.Rule) : TypedAST.Rule =

  let domain =  [ for id in r.Domain do
                    
                    let id1 = id//OpenContext.TrySolveIdFromInheritedTypes id
                    yield TypedAST.Id.buildFrom id1  ]
  let b = convertRuleBody false domain worldName entityName fields globalContext p [] rule_idx (r.Body)
  {
    Position  = entityName.idRange
    Domain    = domain
    Body      = b
  }

and convertTypeDecl (p:BasicAST.Program) (t:BasicAST.TypeDecl) : TypedAST.TypeDecl =
  let raise = raise t.Position
  match t with
  | BasicAST.TypeDecl.TypeName (id) -> 
    let entitiesTypeNames = 
      [ 
        yield p.World.Name
        for e in p.Entities do
          yield e.Name
      ]
    match entitiesTypeNames |> Seq.tryFind ((=) id) with
    | Some id -> TypedAST.TypeDecl.EntityName (TypedAST.Id.buildFrom id )
    | _ ->
      let systemType = OpenContext.ResolveTypeName id.idText id.idRange
      if systemType = null then
        raise "Field declaration error. Cannot find specified type."
      else
        TypedAST.TypeDecl.ImportedType(systemType, id.idRange)
  | BasicAST.TypeDecl.UnionType cases -> TypedAST.TypeDecl.UnionType [for c in cases do yield convertTypeDecl p c]
  | BasicAST.TypeDecl.Tuple args -> TypedAST.TypeDecl.Tuple [for a in args do yield convertTypeDecl p a]
  | BasicAST.TypeDecl.Query a -> TypedAST.TypeDecl.Query(convertTypeDecl p a)
  | BasicAST.TypeDecl.TypeMaybe a -> TypedAST.MaybeType (TypedAST.Just(convertTypeDecl p a))
  | BasicAST.TypeDecl.TypeNothing a -> TypedAST.MaybeType (TypedAST.Nothing(ref { Type = None; AlreadyAssigned = false}, a))
  
  | BasicAST.TypeDecl.Unit(position) -> TypedAST.TypeDecl.Unit position
  | BasicAST.TypeDecl.Imported(id, BasicAST.TypeDecl.Tuple([])) when id.idText = "int" -> TypedAST.TypeDecl.ImportedType(typeof<int>, id.idRange)
  | BasicAST.TypeDecl.Imported(id, BasicAST.TypeDecl.Tuple([])) when id.idText = "float32" || id.idText = "float" -> TypedAST.TypeDecl.ImportedType(typeof<float32>, id.idRange)
  | BasicAST.TypeDecl.Imported(id, BasicAST.TypeDecl.Tuple([])) when id.idText = "bool" -> TypedAST.TypeDecl.ImportedType(typeof<bool>, id.idRange)
  | BasicAST.TypeDecl.Imported(id, BasicAST.TypeDecl.Tuple([])) when id.idText = "string" -> TypedAST.TypeDecl.ImportedType(typeof<string>, id.idRange)
  
  | BasicAST.TypeDecl.Imported(tp, BasicAST.TypeDecl.Tuple(args)) when (p.Entities |> Seq.exists(fun e -> e.Name = tp) ||
                                                                                  p.World.Name = tp) |> not  ->
    let args = args |> List.map (convertTypeDecl p)
    let pos = tp.idRange                                                                                    
    if args |> List.forall(fun arg -> match arg with |TypedAST.TypeDecl.ImportedType(_) -> true | _ -> false) then    
      let args = args |> Seq.map(fun arg -> match arg with |TypedAST.TypeDecl.ImportedType(tp,p) -> tp | _ -> raise "Unexpected error." |> ignore; typeof<Unit>) |> Seq.toArray
      let tp = if args.Length > 0 then tp.idText + "`" + string args.Length else tp.idText
      let tp = OpenContext.ResolveTypeName tp pos
      TypedAST.TypeDecl.ImportedType((if args.Length > 0 then tp.MakeGenericType(args) else tp), pos)

    else
      let tp = if args.Length > 0 then tp.idText + "`" + string args.Length else tp.idText
      let tp = OpenContext.ResolveTypeName tp pos
      TypedAST.TypeDecl.GenericType(TypedAST.TypeDecl.ImportedType(tp,pos), args)
  | BasicAST.TypeDecl.Imported(tp, BasicAST.TypeDecl.Tuple(args)) when args.Length = 0 ->
    TypedAST.TypeDecl.EntityName(TypedAST.Id.buildFrom tp)
  | BasicAST.TypeDecl.Imported(tp, _) ->
    TypedAST.TypeDecl.EntityName(TypedAST.Id.buildFrom tp)

  | BasicAST.TypeDecl.Imported(_) -> raise "Unexpected TypeDecl.Imported type."

let private convertField (globalContext:GlobalTypecheckContext) (entityName:Id) (f:BasicAST.Field) : TypedAST.Field =
  {
    Name      = TypedAST.Id.buildFrom f.Name
    IsStatic  = false
    UpdateNotificationsOnChange = false
    Type      = (fst globalContext.[entityName.idText]).[f.Name.idText]    
    IsReference = f.IsReference
    IsExternal  = 
      if f.IsExternal.IsNone then None
      else
        let id, b, c = f.IsExternal.Value
        Some (TypedAST.Id.buildFrom id, b, c)
    CodeToInjectOnSet  = ""
    UpdateField        = None
    QueryOptimized     = false
    IsQueryIndex       = false
  }

let private convertCreate (p : BasicAST.Program) (fields : Map<Id, TypedAST.TypeDecl * Map<Id, TypedAST.Field>>)  (create : BasicAST.Create) (worldName:Id) (entityName:Id) (globalContext:GlobalTypecheckContext) : TypedAST.Create =
  let raise = raise create.Position
  let args = [for arg in create.Args do
                match arg with
                | id, Some arg -> yield TypedAST.Id.buildFrom id, convertTypeDecl p arg
                | _ -> raise "Create with generic parameters not supported." |> ignore ]

  let block = convertRuleBody true [entityName] worldName entityName fields globalContext p (args |> List.map(fun (id, tp) -> id.idText, tp)) 0 create.Body
  if block.Length = 0 then raise "Empty create block in expression." |> ignore

  {
    Args  = args
    Body  = block
    Position = (fst block.Head).Position
  }

let private convertEntityBody (fields : Map<TypedAST.Id, TypedAST.TypeDecl *  Map<TypedAST.Id, TypedAST.Field>>) p (globalContext:GlobalTypecheckContext) (worldName:Id) (entityName:Id) (b:BasicAST.EntityBody) : TypedAST.EntityBody =
  let entity_fields = snd fields.[entityName]
  {
    Fields    = entity_fields
    Rules     = [ for rule_idx, r in b.Rules |> Seq.mapi(fun i r -> i,r) do yield convertRule p fields worldName entityName globalContext rule_idx r]
    Create    = convertCreate p fields b.Create worldName entityName globalContext
  }
  

let private convertEntity fields p (globalContext:GlobalTypecheckContext) (worldName:Id) (e:BasicAST.Entity) : TypedAST.Entity =
  {
    Name      = TypedAST.Id.buildFrom e.Name
    Body      = convertEntityBody fields p globalContext worldName (TypedAST.Id.buildFrom e.Name) (e.Body)
  }

let private convertWorld fields p (globalContext:GlobalTypecheckContext) (w:BasicAST.World) : TypedAST.World =
  {
    Name      = TypedAST.Id.buildFrom w.Name
    Body      = convertEntityBody fields p globalContext (TypedAST.Id.buildFrom w.Name) (TypedAST.Id.buildFrom w.Name) (w.Body)
  }

let ConvertProgram(p:BasicAST.Program) : TypedAST.Program = 

  
  for t in p.ReferencedLibraries do
    OpenContext.AddReferencedLibraryType t

  let imports = [for a in p.Imports do 
                  OpenContext.AddDirective a.idText
                  yield a]
  let globalContext = 
    let entities = 
      [ 
        yield p.World.Name.idText, p.World.Body
        for e in p.Entities do
          yield e.Name.idText, e.Body
      ]
    [
      for n_e,b_e in entities do
        let f_e = 
          [
            for f in b_e.Fields do
              yield f.Name.idText, convertTypeDecl p f.Type
          ]
        yield n_e, (f_e |> Map.ofList, [for arg in b_e.Create.Args do yield convertTypeDecl p (snd arg).Value])
    ] |> Map.ofList

  let fields = [ let fields_map = ref Map.empty
                 
                 let fields = [for f in p.World.Body.Fields do yield TypedAST.Id.buildFrom f.Name, convertField globalContext (TypedAST.Id.buildFrom p.World.Name) f]
                 for (f_id, _) in fields do
                   if fields_map.Value.ContainsKey(f_id) then raise f_id.idRange (sprintf "Duplicate definition of the field called: %s. Try to use an other name." f_id.idText)
                   else fields_map := fields_map.Value.Add (f_id, ())
                 yield TypedAST.Id.buildFrom  p.World.Name, (TypedAST.TypeDecl.EntityName(TypedAST.Id.buildFrom p.World.Name), 
                                              fields |> Map.ofList)
                 for e in p.Entities do 
                      let fields_map = ref Map.empty
                      let fields = [for f in e.Body.Fields do yield TypedAST.Id.buildFrom f.Name, convertField globalContext (TypedAST.Id.buildFrom e.Name) f]
                      for (f_id, _) in fields do
                        if fields_map.Value.ContainsKey(f_id) then raise f_id.idRange (sprintf "Duplicate definition of the field called: %s. Try to use an other name." f_id.idText)
                        else fields_map := fields_map.Value.Add (f_id, ())

                      yield TypedAST.Id.buildFrom  e.Name, (TypedAST.TypeDecl.EntityName(TypedAST.Id.buildFrom e.Name), 
                                    (fields |> Map.ofList))] |> Map.ofList
  {
    Module    = Id.buildFrom(p.Module)
    Imports   = p.Imports |> List.map(fun id -> TypedAST.Id.buildFrom id)
    World     = convertWorld fields p globalContext p.World
    Entities  = [ for e in p.Entities do yield convertEntity fields p globalContext (TypedAST.Id.buildFrom p.World.Name) e ]
  }
