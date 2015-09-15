module QueryActivePatterns


let (|FromWhereX|_|) (e : TypedAST.TypedExpression) = 
  match e with
  | TypedAST.TypeDecl.Query(_), TypedAST.Expression.Query(q) ->
    match q with
    |TypedAST.QueryStatements(lst) -> 
      match lst with
      |TypedAST.InnerQueryExpression.For(_) ::
       TypedAST.InnerQueryExpression.Where(cond) :: rest ->
        Some (lst.Head, cond, rest)
      | _ -> None
  | _ -> None


let (|ConcatFromWhereX|_|) (e : TypedAST.TypedExpression) = 
  match e with
  | TypedAST.TypeDecl.Query(_), TypedAST.Expression.ConcatQuery([q1; q2]) ->
    match q1 with
    |FromWhereX(TypedAST.InnerQueryExpression.For(i,l,p), _where, _rest) ->
      Some (TypedAST.InnerQueryExpression.For(i,l,p), _where, _rest)      
    | _ -> None
  | _ -> None


let (|AndOnSameObject|_|) (e : TypedAST.TypedExpression) =
  let tp = ref None

  let rec check_expr e =
    match e with
    | t, TypedAST.Expression.IdExpr(id) when id.Tp.Length = 1 && id.idText <> "this" ->
      tp := None 
      None      
    | t, TypedAST.Expression.IdExpr(id) when id.idText = "this" ->
      Some e
    | t, TypedAST.Expression.IdExpr(id) ->
      if tp.Value.IsNone then tp := Some id.Tp.Head
      if tp.Value.IsSome && tp.Value.Value <> id.Tp.Head then 
        tp := None 
        None
      else Some e
    | t, TypedAST.Expression.Not(e) ->
      match check_expr e with
      | None -> 
        tp := None
        None
      | Some _ -> Some (t, TypedAST.Expression.Not(e))
    | t, TypedAST.Expression.Equals(e1, e2) ->
      match check_expr e1, check_expr e2 with
      | Some e1, Some e2 ->  Some (e)
      | _ -> 
        tp := None
        None
    | _ -> None

  let rec check_ands e =
    match e with
    | t, TypedAST.Expression.And(e1, (t1, TypedAST.Expression.And(e2, e3))) ->
      match check_expr e1 with
      | None -> []
      | Some e1 -> 
        e1 :: check_ands (t1, TypedAST.Expression.And(e2, e3))
    
    | t, TypedAST.Expression.And(e1, e2) ->
      match check_expr e1, check_expr e2 with
      | Some e1, Some e2 -> 
        e1 :: e2 :: []
      | _ -> []

    | t, e1 ->
      match check_expr e with
      | Some e1 -> 
        e1 :: []
      | _ -> []

  let e = check_ands e
  
  match !tp with
  | None -> None
  | Some tp -> Some (tp, e)


let (|SimpleQuery|_|) (e : TypedAST.Block) = 
  let before_query = System.Collections.Generic.List<TypedAST.TypedExpression>()

  let rec iterate_exprs (exprs : TypedAST.Block) =
    match exprs with
    | [] -> None
    | (t, TypedAST.Expression.Query(q)) :: xs ->

//      let action (lst : OptimizedQueryAST.TypedExpression) = 
//        match q with
//        |TypedAST.QueryStatements(lst) -> 
//          match lst with
//          |TypedAST.InnerQueryExpression.For(i, t, _) ::
//            TypedAST.InnerQueryExpression.Where(cond) :: rest ->
//            (t, OptimizedQueryAST.Expression.Query(
//              OptimizedQueryAST.QueryStatements(OptimizedQueryAST.InnerQueryExpression.For(i.Id, t, lst) ::
//                                       OptimizedQueryAST.InnerQueryExpression.Where(cond) :: rest)))
//          | _ -> exprs.Head

      Some (before_query |> Seq.toList, exprs.Head, xs, fun (x : OptimizedQueryAST.TypedExpression) -> x)
    | e :: xs ->
      before_query.Add e
      iterate_exprs xs
  
  iterate_exprs e