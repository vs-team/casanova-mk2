module TypedToOptimizedQuery

open OptimizedQueryAST
open Common
open System.Collections.Generic
open OptimizedQueryTraverseContext
open QueryActivePatterns
let mutable tmp_counter = 0

let rec traverseRule current_entity (context : GlobalContext) (rule : TypedAST.Rule) current_rule : Rule =
  {
    Domain    = rule.Domain |> List.map(fun d -> d.Id)
    Body      = traverseBlock (current_entity, None, context) rule.Body current_rule true
    Position  = rule.Position
  }


and traverseExpression (context : ExpressionContext) =
  let traverse (e : TypedAST.TypedExpression) = traverseTypedExpression (ExpressionContext.Build(context, e))
  match context.Expression with
  | TypedAST.Expression.Wait(c) -> Expression.Wait(traverse c, None)
  | TypedAST.Add(e1, e2) -> Expression.Add(traverse e1, traverse e2)
  | TypedAST.Sub(e1, e2) -> Expression.Sub(traverse e1, traverse e2)
  | TypedAST.Div(e1, e2) -> Expression.Div(traverse e1, traverse e2)
  | TypedAST.Modulus(e1, e2) -> Expression.Modulus(traverse e1, traverse e2)
  | TypedAST.Mul(e1, e2) -> Expression.Mul(traverse e1, traverse e2)
  
  | TypedAST.Not(e1) -> Expression.Not(traverse e1)
  | TypedAST.And(e1, e2) -> Expression.And(traverse e1, traverse e2)
  | TypedAST.Or(e1, e2) -> Expression.Or(traverse e1, traverse e2)
  | TypedAST.Equals(e1, e2) -> Expression.Equals(traverse e1, traverse e2)
  | TypedAST.Greater(e1, e2) -> Expression.Greater(traverse e1, traverse e2)

  | TypedAST.Lambda(args, b) -> Lambda(args |> List.map(fun (id, t) -> id.Id, t), traverseBlockWithContext context b)
  | TypedAST.Maybe(TypedAST.JustExpr(e)) -> Maybe(JustExpr(traverse e))
  | TypedAST.Maybe(TypedAST.NothingExpr(p)) -> Maybe(NothingExpr(p))    
  | TypedAST.ConcatQuery(exprs) -> ConcatQuery(List.map traverse exprs)
  | TypedAST.AppendToQuery(e1, e2) -> Expression.AppendToQuery(traverse e1, traverse e2)

  | TypedAST.IndexOf(id, e) -> IndexOf((context.Type, Id(id.Id)), traverse e)
  | TypedAST.Choice(b, exprs, p) -> Choice(b, List.map(fun (e, b, p) -> traverse e, traverseBlockWithContext context b, p ) exprs, p)
  | TypedAST.Parallel(b, p) -> Parallel(List.map(fun (b, p) -> traverseBlockWithContext context b, p ) b, p)
  | TypedAST.Range(e1, e2, p) -> Range(traverse e1, traverse e2, p)
  | TypedAST.NewEntity(exprs) -> NewEntity(exprs |> List.map(fun (id, b) -> id.Id, traverseBlockWithContext context b))
  | TypedAST.Let(id, tp, e) -> Expression.Let(id.Id, tp, traverse e |> Some, true)


  | TypedAST.IfThenElse(c, b1, b2) -> IfThenElse(traverse c, traverseBlockWithContext context b1, traverseBlockWithContext context b2)
  | TypedAST.IfThen(c, b1) -> IfThen(traverse c, traverseBlockWithContext context b1,false)
  | TypedAST.Yield(e) -> Yield(traverse e)


  | TypedAST.Expression.Tuple(b) -> Tuple(traverseBlockWithContext context b)

  | TypedAST.For(ids, e, b) -> Expression.For(ids |> List.map(fun id -> id.Id), traverse e, traverseBlockWithContext context b, false)
  | TypedAST.While(c, b) -> While(traverse c, traverseBlockWithContext context b)
  | TypedAST.Expression.Query(q) -> Query(traverseQuery q context)

  | TypedAST.Call(TypedAST.Constructor(t, b)) -> Call(Constructor(t, traverseBlockWithContext context b))
  | TypedAST.Call(TypedAST.Static(t, id, b)) -> Call(Static(t, id.Id, traverseBlockWithContext context b))
  | TypedAST.Call(TypedAST.Instance(id1, id2, b)) -> Call(Instance(id1.Id, id2.Id, traverseBlockWithContext context b))

  | TypedAST.Literal (l) -> Literal(l)
  | TypedAST.IdExpr(id) -> Id(id.Id)
  | TypedAST.Cast(t, e, p) -> Cast(t, traverse e, p)
  | TypedAST.SubtractToQuery(e1, e2) -> Expression.SubtractToQuery(traverse e1, traverse e2)
  | TypedAST.AddToQuery(e1, e2) -> Expression.AddToQuery(traverse e1, traverse e2)


and traverseQuery (q : TypedAST.QueryExpression) context =
  match q with
  | TypedAST.QueryStatements(qs) -> QueryStatements(List.map (traveserInnerQueryExpression context ) qs)

and traveserInnerQueryExpression context  iq   =
  match iq with
  | TypedAST.InnerQueryExpression.For(id, t, e) -> InnerQueryExpression.For(id.Id, t, traverseTypedExpression (ExpressionContext.Build(context, e)))
  | TypedAST.InnerQueryExpression.Select(e) -> InnerQueryExpression.Select(traverseTypedExpression (ExpressionContext.Build(context, e)))
  | TypedAST.InnerQueryExpression.FindBy(e) -> InnerQueryExpression.FindBy(traverseTypedExpression (ExpressionContext.Build(context, e)))
  | TypedAST.InnerQueryExpression.Exists(e) -> InnerQueryExpression.Exists(traverseTypedExpression (ExpressionContext.Build(context, e)))
  | TypedAST.InnerQueryExpression.MinBy(e) -> InnerQueryExpression.MinBy(traverseTypedExpression (ExpressionContext.Build(context, e)))
  | TypedAST.InnerQueryExpression.ForAll(e) -> InnerQueryExpression.ForAll(traverseTypedExpression (ExpressionContext.Build(context, e)))
  | TypedAST.InnerQueryExpression.MaxBy(e) -> InnerQueryExpression.MaxBy(traverseTypedExpression (ExpressionContext.Build(context, e)))
  | TypedAST.InnerQueryExpression.Where(e) -> InnerQueryExpression.Where(traverseTypedExpression (ExpressionContext.Build(context, e)))
  | TypedAST.InnerQueryExpression.LiteralList(es) -> InnerQueryExpression.LiteralList(es |> List.map(fun e -> traverseTypedExpression (ExpressionContext.Build(context, e))) )
  | TypedAST.InnerQueryExpression.GroupBy(e) -> InnerQueryExpression.GroupBy(traverseTypedExpression (ExpressionContext.Build(context, e)))
  | TypedAST.InnerQueryExpression.GroupByInto(e, id) -> InnerQueryExpression.GroupByInto(traverseTypedExpression (ExpressionContext.Build(context, e)), id.Id)
  | TypedAST.InnerQueryExpression.Let(id, t, e) -> InnerQueryExpression.Let(id.Id, t, traverseTypedExpression (ExpressionContext.Build(context, e)))
  | TypedAST.InnerQueryExpression.Sum(p) -> InnerQueryExpression.Sum(p)
  | TypedAST.InnerQueryExpression.Min(p) -> InnerQueryExpression.Min(p)
  | TypedAST.InnerQueryExpression.Max(p) -> InnerQueryExpression.Max(p)
  | TypedAST.InnerQueryExpression.Empty(t, p) -> InnerQueryExpression.Empty(t, p)

    
and traverseTypedExpression (context : ExpressionContext) : TypedExpression =
  context.Type, traverseExpression context

and traverseQuery' (context : ExpressionContext) : TypedExpression list =
  if not context.OptimizeQuery then 
    match context.TypedExpression with
    | t, TypedAST.Expression.Query(q) -> [t, Query(traverseQuery q context)]
  else
    match context.TypedExpression with
    | FromWhereX(TypedAST.InnerQueryExpression.For(_,t,e), _where, _rest)
    | ConcatFromWhereX(TypedAST.InnerQueryExpression.For(_,t,e), _where, _rest) ->
      let t, e = traverseTypedExpression (ExpressionContext.Build(context, e))
      let t' = t
      let source = context.CurrentEntity
      tmp_counter <- tmp_counter + 1

      
      let rec get_source entity_name lst tp =
          match lst with
          | [] -> {idText = entity_name; idRange = Position.Empty}, tp, lst
          | [x] -> {idText = entity_name; idRange = Position.Empty}, tp, [x]
          | x::xs ->
            let entity_name = tp.Fields.[{idText = x; idRange = Position.Empty}].Type.TypeName
            let tp = context.GlobalContext.Table.[{idText = entity_name; idRange = Position.Empty}]
            
            get_source entity_name xs tp

      let q_temp = {idText = "q_temp" + string tmp_counter; idRange = Position.Empty}

      let query_collection_container , query_collection =

        match e with
        | Id(id) -> 
          let _ids = id.idText.Split('.') |> Seq.toList

        
          let (entity_name : Common.Id), source, id =
  //          if _ids.Length = 1 then snd source
  //          else
              get_source (fst source).idText _ids (snd source)
          let id = {idText = id.Head; idRange = Position.Empty}
          source.Fields.[id].CodeToInjectOnSet <- source.Fields.[id].CodeToInjectOnSet + "\n" + q_temp.idText + " = true;\n" 
          source.Fields.[id].QueryOptimized <- true
          entity_name, id

        | _ -> failwith Position.Empty "error TypedToOptimizedQuery.fs"
               

      match _where with
      | AndOnSameObject(t, exprs) -> 
        let t : TypedAST.TypeDecl = t
//        tmp_counter <- tmp_counter + 1
        let _where = snd _where

        match t with          
        | TypedAST.EntityName(dest) ->         
          let p = _where.Position
          let id_dest = dest.Id
          let dest = context.GlobalContext.Table.[id_dest]
        
          let index_idx, already_optimized = 
            if context.GlobalContext.OptimizedExpressions.ContainsKey(query_collection_container) &&
               context.GlobalContext.OptimizedExpressions.[query_collection_container].ContainsKey(query_collection) then
               snd context.GlobalContext.OptimizedExpressions.[query_collection_container].[query_collection], true
            else context.CurrentRule, false
          
//          let source_id = {idText = (fst source).idText + string context.CurrentRule; idRange = p}
          let source_id = {idText = (fst source).idText + string index_idx; idRange = p}

          
          let bool_tp = TypedAST.TypeDecl.ImportedType(typeof<bool>, p)

          let updateCond (cond : TypedAST.TypedExpression list) update map =
            
            let rec flat lst =
              let f e =
                match e with
                | t, TypedAST.IdExpr(e) -> 
                    let id = e.Id 
                    t, OptimizedQueryAST.Id(id)
                  | t, TypedAST.Not(t1, TypedAST.IdExpr(e)) ->                       
                    let id = e.Id 
                    t, OptimizedQueryAST.Not(t, OptimizedQueryAST.Id(id))
                  | t, TypedAST.Equals((t1, TypedAST.IdExpr(e1)), (t2, TypedAST.IdExpr(e2))) ->
                    let id1 = e1.Id 
                    let id2 = e2.Id 
                    t, OptimizedQueryAST.Equals((t1, OptimizedQueryAST.Id(id1)), (t2, OptimizedQueryAST.Id(id2))) 
              match lst with
              [x] -> f x

              |x::xs -> bool_tp, (Expression.And(f x, flat lst))

            let cond = flat cond

            if context.GlobalContext.OptimizedExpressions.ContainsKey query_collection_container then
              if context.GlobalContext.OptimizedExpressions.[query_collection_container].ContainsKey(query_collection) then
                if update then
                  let mutable_expr,_ = context.GlobalContext.OptimizedExpressions.[query_collection_container].[query_collection]
                  let expr1 = bool_tp, (Expression.And(cond, mutable_expr.Expr))
                  mutable_expr.Expr <- expr1
                  bool_tp, MutableExpression({MutableExpression = mutable_expr ; Map = map})
                else 
                  let mutable_expr,_ = context.GlobalContext.OptimizedExpressions.[query_collection_container].[query_collection]
                  bool_tp, MutableExpression({MutableExpression = mutable_expr ; Map = map})

              else
                let expr = {Expr = cond}
                context.GlobalContext.OptimizedExpressions.[query_collection_container].Add(query_collection, (expr, index_idx))
                bool_tp, MutableExpression({MutableExpression = expr ; Map = map})
                    
            else
              let expr = {Expr = cond}
              context.GlobalContext.OptimizedExpressions.Add(query_collection_container, new Dictionary<_,_>())
              context.GlobalContext.OptimizedExpressions.[query_collection_container].Add(query_collection, (expr, index_idx))
              bool_tp, MutableExpression({MutableExpression = expr ; Map = map})

          let field_id1 = {TypedAST.Id = {idText = "_" + (fst source).idText; idRange =p }; TypedAST.Tp = []}
    
//          let rebuilt_AND_expr exprs =
//                let rec AUX_rebuilt_and_expr exprs =
//                  
//
//                  match exprs with
//                  | [] -> raise p "TypedToOptimizedQuery.fs (line:264) Internal error" 
//                  | (t, x) :: [] ->
//                    remove_head_from_id (t, x)
//                  | (t, x) :: xs ->
//                    let id = remove_head_from_id (t, x)
//                    bool_tp, OptimizedQueryAST.And(id, AUX_rebuilt_and_expr xs)
//                AUX_rebuilt_and_expr exprs           

//          let exprs_list = exprs
//          let exprs = rebuilt_AND_expr exprs



          let source_block =
            let condition_destination_type_list = TypedAST.TypeDecl.Query(TypedAST.TypeDecl.EntityName({ Id = id_dest; Tp = []}))

            let get_query_from_index = 
                condition_destination_type_list, Id(source_id)

            let q1 = 
              let q1_type = condition_destination_type_list
              let q1_expr = OptimizedQueryAST.Let(q_temp, 
                                                  bool_tp,                                                  
                                                  None, false)

              let new_dict = q1_type, OptimizedQueryAST.Call(Call.Constructor(q1_type, []))
              let set_new_dict = TypedAST.TypeDecl.Unit(p),  OptimizedQueryAST.Set(source_id, new_dict)

              let create_expr = 
                match context.WorldName.Body.Create.Body |> List.rev with
                | [] -> [set_new_dict ]
                | x::xs -> (x :: set_new_dict :: xs) |> List.rev
              
              //context.WorldName.Body.Create.Body <- create_expr

              q1_type, q1_expr
            

//
//            match e with
//            | Id(id) -> 
//              (snd source).Fields.[id].CodeToInjectOnSet <- (snd source).Fields.[id].CodeToInjectOnSet + "\n" + q_temp.idText + " = true;\n" 
//              (snd source).Fields.[id].QueryOptimized <- true
//              ()
//            | _ -> ()

            
            if not already_optimized then
              (snd source).Fields.Add(
                  source_id,
                  {
                    Name      = {Id = {idText = (fst source).idText + string index_idx; idRange = p}; Tp = []}
  //                  FieldBody = ""
                    IsStatic  = false
                    Type      = fst q1
                    UpdateNotificationsOnChange = true
                    IsReference   = true
                    IsExternal    = None
                    CodeToInjectOnSet  = 
                      (match e with
                       | Id(id) -> id.idText + "= __" + source_id.idText + ";\n"
                       | _ -> "")        
                    UpdateField        = None
                    QueryOptimized     = true
                    IsQueryIndex       = false
                  })




            let wait =
              TypedAST.TypeDecl.Unit(p),
              OptimizedQueryAST.Wait((bool_tp, OptimizedQueryAST.Equals((t, e), (t, e))),
                                     Some((bool_tp, Expression.Id(q_temp))))


            let yield_expr = 
//              TypedAST.TypeDecl.Unit(p),
              


              [(TypedAST.TypeDecl.Unit(p), OptimizedQueryAST.Set(q_temp, 
                                                                 (TypedAST.ImportedType(typeof<bool>,p), OptimizedQueryAST.Literal(BasicAST.Bool(false, p)))));
                (TypedAST.TypeDecl.Unit(p),
                 OptimizedQueryAST.Yield(TypedAST.TypeDecl.Tuple([condition_destination_type_list]), 
                                      Tuple([context.Action (get_query_from_index)])))]
            
                
            let source1 = source
            let source = snd source
            let rev_create_body = List.rev source.Create.Body
            let new_source_body = 
              let q_expr = 
                if (context.GlobalContext.OptimizedExpressions.ContainsKey query_collection_container &&
                    context.GlobalContext.OptimizedExpressions.[query_collection_container].ContainsKey query_collection) |> not then
                          context.Type, 
                          OptimizedQueryAST.Let({idText = "q"; idRange = p}, 
                                                context.Type,
                                                traverseTypedExpression context |> Some, false)
                else
                  context.Type, OptimizedQueryAST.Set(({idText = "q"; idRange = p}), traverseTypedExpression context)
                  
              let new_source_id = 
                TypedAST.Unit(p),
                (OptimizedQueryAST.Set({idText = source_id.idText; idRange = p}, 
                                       (context.Type, OptimizedQueryAST.Call(OptimizedQueryAST.Constructor(context.Type, [t, e])))))
                 

              let add_q =
                TypedAST.Unit(p),
                OptimizedQueryAST.Set({idText = source_id.idText; idRange = p},(context.Type, Id({idText = "q"; idRange = p})))



              match rev_create_body with
              | x :: xs  ->
                   source.Create.Body @ [new_source_id ; q_expr; add_q]
              | _ -> 
                   [new_source_id ; q_expr ; add_q]

            source.Create.Body <- new_source_body
            let source_id = fst source1

            let iterate_through_sources =
              let dest_field_id = {idText = "_" + source_id.idText; idRange =p }
              let dest_tp = TypedAST.TypeDecl.EntityName({Id = id_dest; Tp = []})

              let dest_field_id1 = {TypedAST.Id = id_dest; TypedAST.Tp = []}
              let field_id_tp = TypedAST.TypeDecl.EntityName(dest_field_id1)
              let dest_query_tp = TypedAST.Query(dest_tp)
              

              
              let body =
                let _id = 
                  match context.Action (get_query_from_index) with
                  | _, Expression.Id(id) -> id



                let f id =
                  let id = id.idText.Split('.')
                  let id = 
                    if id.Length > 1 then
                      let id = id |> Seq.toList |> List.tail
                      id.Tail |> List.fold(fun s t -> s + "." + t) id.Head
                    else id.[0]
                  let id = if id = "this" then field_id1.idText else id
                  {idText = "___s" + string context.CurrentRule + "." + id; idRange = p}

                let rec remove_head_from_id x =
                  match x with
                  | t, Id(e) -> 
                    let id = f e
                    t, OptimizedQueryAST.Id(id)
                  | t, Not(t1, Id(e)) ->                       
                    let id = f e
                    t, OptimizedQueryAST.Not(t, OptimizedQueryAST.Id(id))
                  | t, Equals((t1, Id(e1)), (t2, Id(e2))) ->
                    let id1 = f e1
                    let id2 = f e2
                    t, OptimizedQueryAST.Equals((t1, OptimizedQueryAST.Id(id1)), (t2, OptimizedQueryAST.Id(id2)))
                  | t, And(e1, e2) ->                    
                    t, And(remove_head_from_id e1, remove_head_from_id e2)
                  | e -> e


                let cond = exprs
    
                    

                TypedAST.Unit(p),
                OptimizedQueryAST.IfThen(remove_head_from_id(updateCond cond true remove_head_from_id),
                   [(TypedAST.Unit(p), Expression.Set({idText = "___s" + string context.CurrentRule + "." + dest_field_id.idText; idRange = p}, 
                                                      (field_id_tp, Id({idText = "this"; idRange = p}))));
                    (TypedAST.TypeDecl.Unit(p), 
                  
                  
                      OptimizedQueryAST.Call(Call.Instance(_id, 
                                                           {idText = "Add"; idRange = p}, 
                                                           [TypedAST.Unit(p), Id({idText = "___s" + string context.CurrentRule; idRange = p})])))], true)
//                   [(TypedAST.Unit(p), Expression.Set({idText = "___s" + string context.CurrentRule + "." + dest_field_id.idText; idRange = p}, 
//                                                      (field_id_tp, Id({idText = "null"; idRange = p}))))])
                    
              

              TypedAST.Unit(p),
              Expression.For([{idText = "___s" + string context.CurrentRule; idRange = p}], 
                              (t',e), 
                              [body], true)
            let _idtp, _id = 
                  match context.Action (get_query_from_index) with
                  | t, Expression.Id(id) -> t, id
            
            let _e_idtp, _e_id =
              match t, e with
              | t, Expression.Id(id) -> t, id

            let clear_index =
              [ (TypedAST.TypeDecl.Unit(p), OptimizedQueryAST.Set(q_temp, 
                                                                 (TypedAST.ImportedType(typeof<bool>,p), OptimizedQueryAST.Literal(BasicAST.Bool(false, p)))));

                (TypedAST.TypeDecl.Unit(p), OptimizedQueryAST.IfThen((bool_tp, OptimizedQueryAST.Equals((_idtp, Id(_id)),(_idtp,e))), 
                                                                     [TypedAST.TypeDecl.Unit(p), 
                                                                      OptimizedQueryAST.Set({idText = "_" + _e_id.idText; idRange = _e_id.idRange},
                                                                                            (_e_idtp, OptimizedQueryAST.Call(OptimizedQueryAST.Constructor(TypedAST.TypeDecl.Query(_e_idtp), [_e_idtp, Id(_e_id)]))))], true));
                (TypedAST.TypeDecl.Unit(p), OptimizedQueryAST.Call(OptimizedQueryAST.Instance(_id, {idText = "Clear"; idRange = p}, [])))
              ]
//            [q1; iterate_through_sources; wait; yield_expr]
            q1 :: wait :: clear_index @ [iterate_through_sources] @ yield_expr


          let dest_block : unit =
            let bool_tp = TypedAST.TypeDecl.ImportedType(typeof<bool>, p)
//            let field_id1 = {TypedAST.Id = {idText = "_" + (fst source).idText; idRange =p }; TypedAST.Tp = []}
            let field_id_tp = TypedAST.TypeDecl.EntityName({TypedAST.Id = {idText = (fst source).idText; idRange =p }; TypedAST.Tp = []})
            if dest.Fields.ContainsKey field_id1.Id |> not then
              dest.Fields.Add(field_id1.Id, 
                              { Name      = field_id1
                                Type      = field_id_tp
                                UpdateNotificationsOnChange = false
                                IsReference   = true
                                IsStatic = false
                                IsExternal    = None 
                                CodeToInjectOnSet  = ""
                                IsQueryIndex       = true
                                UpdateField        = None
                                QueryOptimized     = false})
            let rule =

              let build_lets exprs =
                let rec AUX_build_lets exprs (n : int) =
                  match exprs with
                  | [] -> []
                  | (t, x) :: xs ->
                    let f id =
                      let id = id.idText.Split('.')
                      let id = 
                        if id.Length > 1 then
                          let id = id |> Seq.toList |> List.tail
                          id.Tail |> List.fold(fun s t -> s + "." + t) id.Head
                        else id.[0]
                      let id = if id = "this" then field_id1.idText else id
                      {idText = id; idRange = p}
                    let id1 = {Common.idText = "_cond" + string n + string context.CurrentRule; Common.idRange = p}

                    match x with
                    | TypedAST.IdExpr(e) -> 
                      let id = f e.Id
                      ((t, OptimizedQueryAST.Let(id1, t, (t, Id(id)) |> Some, false)), (t, id1, id)) :: AUX_build_lets xs (n + 1) 
                    | TypedAST.Not(t1, TypedAST.IdExpr(e)) ->                       
                      let id = f e.Id
                      ((t, OptimizedQueryAST.Let(id1, t, (t, Id(id)) |> Some, false)), (t, id1, id)) :: AUX_build_lets xs (n + 1) 
                    | TypedAST.Equals((t1, TypedAST.IdExpr(e1)), (t2, TypedAST.IdExpr(e2))) ->
                      let id' = f e1.Id
                      let id'' = f e2.Id
                      
                      ((t1, OptimizedQueryAST.Let(id1, t1, (t1, Id(id')) |> Some, false)), (t, id1, id'')) :: AUX_build_lets xs (n + 1) 
                    | _ -> raise p "Internal. Not supported operation in query optimization."

                AUX_build_lets exprs 0

//              let rebuilt_AND_expr exprs =
//                let rec AUX_rebuilt_and_expr exprs =
              let f id =
                let id = id.idText.Split('.')
                let id = 
                  if id.Length > 1 then
                    let id = id |> Seq.toList |> List.tail
                    id.Tail |> List.fold(fun s t -> s + "." + t) id.Head
                  else id.[0]
                let id = if id = "this" then field_id1.idText else id
                {idText = id; idRange = p}

              let rec remove_head_from_id (x : TypedExpression) =
                match x with
                | t, Id(e) -> 
                  let id = f e
                  t, OptimizedQueryAST.Id(id)
                | t, Not(t1, Id(e)) ->                       
                  let id = f e
                  t, OptimizedQueryAST.Not(t, OptimizedQueryAST.Id(id))
                | t, Equals((t1, Id(e1)), (t2, Id(e2))) ->
                  let id1 = f e1
                  let id2 = f e2
                  t, OptimizedQueryAST.Equals((t1, OptimizedQueryAST.Id(id1)), (t2, OptimizedQueryAST.Id(id2)))
                | t, And(e1,e2) ->
                  t, OptimizedQueryAST.And(remove_head_from_id e1, remove_head_from_id e2)
//
//                  match exprs with
//                  | [] -> raise p "TypedToOptimizedQuery.fs (line:264) Internal error" 
//                  | (t, x) :: [] ->
//                    remove_head_from_id (t, x)
//                  | (t, x) :: xs ->
//                    let id = remove_head_from_id (t, x)
//                    bool_tp, OptimizedQueryAST.And(id, AUX_rebuilt_and_expr xs)
//                AUX_rebuilt_and_expr exprs           
//
//              

              
              let init, ids = build_lets exprs |> List.unzip
              let wait =
                let e = ids |> Seq.fold(fun s (id_tp, id1, id) -> 
                                          bool_tp, 
                                          OptimizedQueryAST.Or((bool_tp,
                                                                OptimizedQueryAST.Not(bool_tp, 
                                                                                      OptimizedQueryAST.Equals((id_tp, Id(id1)), 
                                                                                                              (id_tp, Id(id))))), s)) 
                               (bool_tp, OptimizedQueryAST.Expression.Literal(BasicAST.Bool(false, p)))             
                                    
                TypedAST.TypeDecl.Unit(p),
                OptimizedQueryAST.Wait(e, None)

              let if_then_else =
              
                let cond = updateCond exprs false remove_head_from_id
                
                let dest_collection_call _method =
                  OptimizedQueryAST.Call(OptimizedQueryAST.Instance({source_id with idText = field_id1.idText + "." + source_id.idText }, 
                                                                    {idText = _method; idRange = p}, 
                                                                    [TypedAST.EntityName({TypedAST.Id = id_dest; TypedAST.Tp = []}), Id({idText = "this"; idRange = p})]))

                let inner_cond = 
                  bool_tp, OptimizedQueryAST.Not(bool_tp, dest_collection_call "Contains")
                TypedAST.TypeDecl.Unit(p),
                OptimizedQueryAST.IfThenElse(
                  cond, 
                  [(TypedAST.TypeDecl.Unit(p),
                    OptimizedQueryAST.IfThenElse(
                      inner_cond , 
                      [TypedAST.TypeDecl.Unit(p), dest_collection_call "Add"; (TypedAST.TypeDecl.Unit(p), Expression.ReEvaluateRule p) ],
                      [(TypedAST.TypeDecl.Unit(p), Expression.ReEvaluateRule p) ]))],
                  [(TypedAST.TypeDecl.Unit(p), dest_collection_call "Remove"); (TypedAST.TypeDecl.Unit(p), Expression.ReEvaluateRule p) ])
              init @ wait :: if_then_else :: []
            dest.Rules.Add({Domain = []; Body = rule; Position = p})
          

          source_block


      // a.b.c > const
      | _, TypedAST.Expression.Greater((id_tp, TypedAST.Expression.IdExpr(id)), const_expr) ->
        
        let _where = snd _where
        match id.Tp.Head with
          
        | TypedAST.EntityName(dest) ->         
          let p = _where.Position
          let id_dest = dest.Id
          let dest = context.GlobalContext.Table.[id_dest]
          let source = context.CurrentEntity
          let bool_tp = TypedAST.TypeDecl.ImportedType(typeof<bool>, p)
        
          let source_id = {idText = (fst source).idText + string context.CurrentRule; idRange = p}

//          let q_temp = {idText = "q_temp" + string tmp_counter; idRange = p}

          let source_block =
            let condition_destination_type_list = TypedAST.TypeDecl.Query(TypedAST.TypeDecl.EntityName({ Id = id_dest; Tp = []}))

            let get_query_from_index = 
                condition_destination_type_list, Id(source_id)

            let q1 = 
              let q1_type = TypedAST.TypeDecl.ImportedType(typeof<bool>, p) 
              let q1_expr = OptimizedQueryAST.Let(q_temp , 
                                                  bool_tp,                                                  
                                                  None, false)

              let new_dict = q1_type, OptimizedQueryAST.Call(Call.Constructor(condition_destination_type_list, []))
              let set_new_dict = TypedAST.TypeDecl.Unit(p),  OptimizedQueryAST.Set(source_id, new_dict)

              let create_expr = 
                match context.WorldName.Body.Create.Body |> List.rev with
                | [] -> [set_new_dict ]
                | x::xs -> (x :: set_new_dict :: xs) |> List.rev
              
              context.WorldName.Body.Create.Body <- create_expr

              q1_type, q1_expr
            

            (snd source).Fields.Add(
                source_id,
                {
                  Name      = {Id = {idText = (fst source).idText + string context.CurrentRule; idRange = p}; Tp = []}
                  IsStatic  = false
                  Type      = fst q1
                  UpdateNotificationsOnChange = true
                  IsReference   = true
                  IsExternal    = None
                  CodeToInjectOnSet  = 
                    (match e with
                     | Id(id) -> id.idText + "= __" + source_id.idText + ";\n"
                     | _ -> "")        
                  UpdateField        = None
                  QueryOptimized     = true
                  IsQueryIndex       = false
                })



            let wait =
              TypedAST.TypeDecl.Unit(p),
              OptimizedQueryAST.Wait((bool_tp, OptimizedQueryAST.Equals((t,e), (t,e))),
                                     Some(bool_tp, Expression.Id(q_temp)))
//                                         Call(Instance({idText = "q_temp" + string tmp_counter; idRange = p}, 
//                                                       {idText = "CompareStruct"; idRange = p}, 
//                                                       [t,e]))))


            let yield_expr = 
              [(TypedAST.TypeDecl.Unit(p), OptimizedQueryAST.Set(q_temp, 
                                                                 (TypedAST.ImportedType(typeof<bool>,p), OptimizedQueryAST.Literal(BasicAST.Bool(false, p)))));
                (TypedAST.TypeDecl.Unit(p),
                 OptimizedQueryAST.Yield(TypedAST.TypeDecl.Tuple([condition_destination_type_list]), 
                                         Tuple([context.Action (get_query_from_index)])))]
            
                
            let source1 = source
            let source = snd source
            let rev_create_body = List.rev source.Create.Body



            let new_source_body = 
              let q_expr = 
                          context.Type, 
                          OptimizedQueryAST.Let({idText = "q"; idRange = p}, 
                                                context.Type,
                                                traverseTypedExpression context |> Some, false)

            
              let new_source_id = 
                TypedAST.Unit(p),
                (OptimizedQueryAST.Set({idText = source_id.idText; idRange = p}, 
                                       (context.Type, OptimizedQueryAST.Call(OptimizedQueryAST.Constructor(context.Type, [t, e])))))
                 
              let add_q =
                TypedAST.Unit(p),
                OptimizedQueryAST.Set({idText = source_id.idText; idRange = p},(context.Type, Id({idText = "q"; idRange = p})))
                //OptimizedQueryAST.Call(OptimizedQueryAST.Instance({idText = source_id.idText; idRange = p}, {idText = "AddRange"; idRange = p}, [context.Type, Id({idText = "q"; idRange = p})]))



              match rev_create_body with
              | x :: xs -> source.Create.Body @ [new_source_id ;q_expr ;add_q ]
              | _ -> (new_source_id :: q_expr :: add_q :: [])
            source.Create.Body <- new_source_body
            let source_id = fst source1

            let _idtp, _id = 
                  match context.Action (get_query_from_index) with
                  | t, Expression.Id(id) -> t, id
            
            let _e_idtp, _e_id =
              match t, e with
              | t, Expression.Id(id) -> t, id

            let clear_index =
              [
                (TypedAST.TypeDecl.Unit(p), OptimizedQueryAST.Set(q_temp, 
                                                                 (TypedAST.ImportedType(typeof<bool>,p), OptimizedQueryAST.Literal(BasicAST.Bool(false, p)))));

                (TypedAST.TypeDecl.Unit(p), OptimizedQueryAST.IfThen((bool_tp, OptimizedQueryAST.Equals((_idtp, Id(_id)),(_idtp,e))), 
                                                                     [TypedAST.TypeDecl.Unit(p), 
                                                                      OptimizedQueryAST.Set({idText = "_" + _e_id.idText; idRange = _e_id.idRange},
                                                                                            (_e_idtp, OptimizedQueryAST.Call(OptimizedQueryAST.Constructor(_e_idtp, [_e_idtp, Id(_e_id)]))))], true))
                (TypedAST.TypeDecl.Unit(p), OptimizedQueryAST.Call(OptimizedQueryAST.Instance(_id, {idText = "Clear"; idRange = p}, [])))
              
              ]

            let iterate_through_sources =
              let id = id.Id.idText.Split('.')
              let id = id |> Seq.toList |> Seq.skip 1 |> Seq.toList
              let id = id.Tail |> List.fold(fun s t -> s + "." + t) id.Head
              let id = {idText = id; idRange = p}
              let cond = bool_tp, OptimizedQueryAST.Greater((id_tp, OptimizedQueryAST.Id({idText = "s." + id.idText; idRange = p})), 
                                                            (traverseTypedExpression (ExpressionContext.Build(context, const_expr))))


              
              let dest_field_id = {idText = "_" + source_id.idText; idRange =p }
              let dest_tp = TypedAST.TypeDecl.EntityName({Id = id_dest; Tp = []})

              let dest_field_id1 = {TypedAST.Id = id_dest; TypedAST.Tp = []}
              let field_id_tp = TypedAST.TypeDecl.EntityName(dest_field_id1)
              let dest_query_tp = TypedAST.Query(dest_tp)
              
              let body =

                
                
                TypedAST.Unit(p),
                OptimizedQueryAST.IfThenElse(cond,
                   [(TypedAST.Unit(p), Expression.Set({idText = "s." + dest_field_id.idText; idRange = p}, 
                                                      (field_id_tp, Id({idText = "this"; idRange = p}))));
                    (TypedAST.TypeDecl.Unit(p), 
                  
                  
                      OptimizedQueryAST.Call(Call.Instance(_id, 
                                                           {idText = "Add"; idRange = p}, 
                                                           [TypedAST.Unit(p), Id({idText = "s"; idRange = p})])))],
                   [(TypedAST.Unit(p), Expression.Set({idText = "s." + dest_field_id.idText; idRange = p}, 
                                                      (field_id_tp, Id({idText = "null"; idRange = p}))))])
                
                

              TypedAST.Unit(p),
              Expression.For([{idText = "s"; idRange = p}], 
                              (t,e), 
                              [body], true)

//            [q1; iterate_through_sources; wait; yield_expr]
            wait :: clear_index @ [iterate_through_sources] @ yield_expr

          let dest_block : unit =
            let bool_tp = TypedAST.TypeDecl.ImportedType(typeof<bool>, p)
            let field_id1 = {TypedAST.Id = {idText = "_" + (fst source).idText; idRange =p }; TypedAST.Tp = []}
            let field_id_tp = TypedAST.TypeDecl.EntityName({TypedAST.Id = {idText = (fst source).idText; idRange =p }; TypedAST.Tp = []})
            dest.Fields.Add(field_id1.Id, 
                            { Name      = field_id1
                              Type      = field_id_tp
                              UpdateNotificationsOnChange = false
                              IsQueryIndex       = true
                              IsReference   = true
                              IsStatic = false
                              IsExternal    = None
                              CodeToInjectOnSet  = ""
                              UpdateField        = None
                              QueryOptimized     = false })
            let rule =
              let id = id.Id.idText.Split('.')
              let id = id |> Seq.toList |> Seq.skip 1 |> Seq.toList
              let id = id.Tail |> List.fold(fun s t -> s + "." + t) id.Head
              let id = {idText = id; idRange = p}

              let id1 = {Common.idText = "_cond" + string context.CurrentRule; Common.idRange = p}
              let init = id_tp, OptimizedQueryAST.Let(id1, id_tp, (id_tp, Id(id)) |> Some, false)
              let wait =
                TypedAST.TypeDecl.Unit(p),
                OptimizedQueryAST.Wait((bool_tp, 
                                        OptimizedQueryAST.Not(bool_tp, 
                                                             OptimizedQueryAST.Equals((id_tp, Id(id1)), 
                                                                                       (id_tp, Id(id))))), None)
              let if_then_else =
              
                let cond = bool_tp, OptimizedQueryAST.Greater((id_tp, OptimizedQueryAST.Id({idText = id.idText; idRange = p})), 
                                                              (traverseTypedExpression (ExpressionContext.Build(context, const_expr))))
                let dest_collection_call _method =
                  OptimizedQueryAST.Call(OptimizedQueryAST.Instance({source_id with idText = field_id1.idText + "." + source_id.idText }, 
                                                                    {idText = _method; idRange = p}, 
                                                                    [TypedAST.EntityName({TypedAST.Id = id_dest; TypedAST.Tp = []}), Id({idText = "this"; idRange = p})]))

                let inner_cond = 
                  bool_tp, OptimizedQueryAST.Not(bool_tp, dest_collection_call "Contains")
                TypedAST.TypeDecl.Unit(p),
                OptimizedQueryAST.IfThenElse(
                  cond, 
                  [(TypedAST.TypeDecl.Unit(p),
                    OptimizedQueryAST.IfThenElse(
                      inner_cond , 
                      [TypedAST.TypeDecl.Unit(p), dest_collection_call "Add"; (TypedAST.TypeDecl.Unit(p), Expression.ReEvaluateRule p)],
                      [(TypedAST.TypeDecl.Unit(p), Expression.ReEvaluateRule p) ]))],
                  [(TypedAST.TypeDecl.Unit(p), dest_collection_call "Remove"); (TypedAST.TypeDecl.Unit(p), Expression.ReEvaluateRule p) ])
              init :: wait :: if_then_else :: []
            dest.Rules.Add({Domain = []; Body = rule; Position = p})
          

          source_block
        | _ -> []

      | _, TypedAST.Expression.IdExpr(id) -> []
      | _ -> traverseQuery' {context with OptimizeQuery = false}
    | _ -> []
  

and traverseBlockWithContext (context : ExpressionContext) block : Block =
  traverseBlock (context.CurrentEntity, None, context.GlobalContext) block context.CurrentRule false
and traverseBlock ((current_entity, expr_context, global_context) : (Common.Id * EntityBody) * ExpressionContext option * GlobalContext) (block : TypedAST.Block) current_rule optimize_query : Block =
  match block with
  | SimpleQuery(before, query, after, action) when optimize_query && Common.enable_query_optimization ->
    let before' = before |> List.map(fun te -> traverseTypedExpression (ExpressionContext.Build(current_entity, global_context, te, current_rule)))
    let context = (ExpressionContext.Build(current_entity, global_context, query, current_rule, action))
    let query' = traverseQuery' context
    let after' = before |> List.map(fun te -> traverseTypedExpression (ExpressionContext.Build(current_entity, global_context, te, current_rule)))

    before' @ query' @ after'
  | _ ->
    block |> List.map(fun te -> traverseTypedExpression (ExpressionContext.Build(current_entity, global_context, te, current_rule, optimize_query)))
  

let traverseBody current_entity (context : GlobalContext) (body : TypedAST.EntityBody) : EntityBody =

  let create_body = traverseBlock (current_entity, None, context) body.Create.Body -1 true
  (snd current_entity).Create.Body <- 
    if (snd current_entity).Create.Body.Length = 0 then create_body 
    else (snd current_entity).Create.Body @ create_body
  let r = ResizeArray(body.Rules |> Seq.mapi(fun i r -> traverseRule current_entity context r i))

  let v = body.Fields |> Seq.map(fun f -> f.Key.Id, f.Value) |> Map.ofSeq
  {
    Fields    = Dictionary<Id, TypedAST.Field>(v)
    Rules     = r
    Create    = 
      {
        Args      = body.Create.Args |> List.map(fun (id, t) -> id.Id, t)
        Body      = (snd current_entity).Create.Body
        Position  = body.Create.Position
      }
  }
  

let traverseEntity (context : GlobalContext) (e : TypedAST.Entity) : Entity =
  {
    Name      = e.Name.Id
    Body      = traverseBody (e.Name.Id, context.Table.[e.Name.Id]) context e.Body
  }

let traverseWorld (context : GlobalContext) (e : TypedAST.World) : World =
  {
    Name      = e.Name.Id
    Body      = traverseBody (e.Name.Id, context.Table.[e.Name.Id]) context e.Body
  }




let traverseProgram (p : TypedAST.Program) : Program =
  let p1 =
    { Module    = p.Module.Id
      Imports   = p.Imports |> List.map(fun i -> i.Id)
      World     =
        {
          Name = p.World.Name.Id
          Body =
            {
              Fields    = Dictionary<Id, TypedAST.Field>(p.World.Body.Fields |> Seq.map(fun e -> e.Key.Id, e.Value) |> Map.ofSeq)
              Rules     = ResizeArray<Rule>()
              Create    = 
                {
                  Args      = p.World.Body.Create.Args |> List.map(fun (id, t) -> id.Id, t)
                  Body      = []
                  Position  = p.World.Body.Create.Position
                }
                
            }
        }         
      Entities  = 
       ResizeArray(
         (p.Entities |> Seq.map(fun e -> 
            { Name = e.Name.Id
              Body =
                { Fields    = Dictionary<Id, TypedAST.Field>(e.Body.Fields |> Seq.map(fun e -> e.Key.Id, e.Value) |> Map.ofSeq)
                  Rules     = ResizeArray<Rule>()
                  Create    = 
                    {
                      Args      = e.Body.Create.Args |> List.map(fun (id, t) -> id.Id, t)
                      Body      = []
                      Position  = e.Body.Create.Position
                    }}})))}

    
  let table =
    let table1 = p1.Entities |> Seq.map(fun e -> e.Name, e.Body) |> Map.ofSeq
    table1.Add(p1.World.Name, p1.World.Body)
  let context = { Table = table; WorldName = p1.World; OptimizedExpressions = new System.Collections.Generic.Dictionary<_,_>() }

  let entities = p.Entities |> List.map (traverseEntity context)
  let world = traverseWorld context p.World 

  let world1 = table.[world.Name]
  world1.Rules.AddRange(world.Body.Rules)
  world1.Create.Body <- world.Body.Create.Body

  for entity in entities do
    let entity1 = table.[entity.Name]
    entity1.Rules.AddRange(entity.Body.Rules)
    entity1.Create.Body <- entity.Body.Create.Body
  p1

(*
1) optimize where of AND coditions; faster especially in the case of nested for loops

opt
  [
    A
    for x in l do
    B
    where x.f1 cmp1 v1 && ... && x.fN cmpN vN && C
    D
  ]
where l <> hashLookup* _

==>

let l_idx = hash[f1,...,fN] l v1 ... vN
opt
  [
    A
    for x in hashLookupAnd[cmp1 ... cmpN] l_idx v1 ... vN
    B
    where x.f1 cmp1 v1 && ... && x.fN cmpN vN && C
    D
  ]


2) optimize findBy; faster especially in the case of non-changing index, but changing C

opt
  [
    A
    for x in l do
    B
    findBy x.f1 cmp1 v1 && ... && x.fN cmpN vN && C
  ]
where l <> hashLookup* _

==>

let l_idx = hash[f1,...,fN] l v1 ... vN
opt
  [
    A
    for x in hashLookupAnd[cmp1 ... cmpN] l_idx v1 ... vN
    B
    findBy x.f1 cmp1 v1 && ... && x.fN cmpN vN && C
  ]


3) optimize groupBy (and similarly groupByInto); faster especially in the case of non-changing index, but changing C

opt
  [
    A
    for x in l do
    B
    groupBy x.f1 cmp1 v1 && ... && x.fN cmpN vN
  ]
where l <> hashLookup* _

==>

let l_idx = hash[f1,...,fN] l v1 ... vN
opt
  [
    A
    for x in hashLookupAndGrouped[cmp1 ... cmpN] l_idx v1 ... vN
    B
    select x
  ]


*)

