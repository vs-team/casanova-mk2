module IdSelectionContext
open StateMachinesAST
open Common


let rec select_guard (expr_list : OptimizedQueryAST.Block) : OptimizedQueryAST.Block * Option<OptimizedQueryAST.TypedExpression> * OptimizedQueryAST.Block =
  let guard_idx = ref None
  let rec select_expr_before_guard expr_list idx =
    match expr_list with
    | [] -> []
    | (_, (OptimizedQueryAST.Expression.Parallel(_))) :: exprs
    | (_, (OptimizedQueryAST.Expression.Choice(_))) :: exprs
    | (_, (OptimizedQueryAST.Expression.Yield(_))) :: exprs ->
      guard_idx := Some idx; []

    | (_, (OptimizedQueryAST.Expression.Wait(expr,_))) :: exprs when fst expr = TypeDecl.ImportedType(typeof<bool>, Position.Empty) || 
                                                          fst expr = TypeDecl.ImportedType(typeof<single>, Position.Empty) ->      
      guard_idx := Some idx; []
    | (t, (OptimizedQueryAST.Expression.While(c,b))) :: exprs ->
      guard_idx := Some idx; []
    
    | (t, (OptimizedQueryAST.Expression.For(i,l,b, _))) :: exprs ->
      guard_idx := Some idx; []
    
    | expr :: exprs -> expr :: (select_expr_before_guard exprs (idx + 1))
  
  let before_guard = select_expr_before_guard expr_list 0
  match !guard_idx with
  | None -> expr_list , None, []
  | Some idx ->
    let guard, after_guard = 
      let seq = Seq.skip idx expr_list |> List.ofSeq
      if seq.Length = 0 then raise (Position.Empty) (sprintf "Interal error %s(%s)." __SOURCE_FILE__ __LINE__) |> ignore

      seq.Head, seq.Tail
    before_guard, Some guard, after_guard 

    
let rec reduce_expr expr (table : List<Id * OptimizedQueryAST.TypedExpression>) : OptimizedQueryAST.TypedExpression =  
  let try_find id expr = match table |> List.tryFind(fun (id1,_) -> id = id1) with | None -> expr | Some (_,expr) -> expr
  let try_find_expr id = match table |> List.tryFind(fun (id1,_) -> id = id1) with | None -> None | Some (_,expr) -> Some expr

  let rec AUX_reduce_expr (expr : OptimizedQueryAST.TypedExpression) : OptimizedQueryAST.TypedExpression =
      match expr with
      | t, OptimizedQueryAST.Add(e1, e2) -> t, OptimizedQueryAST.Add(AUX_reduce_expr e1, AUX_reduce_expr e2)
      | t, OptimizedQueryAST.Mul(e1, e2) -> t, OptimizedQueryAST.Mul(AUX_reduce_expr e1, AUX_reduce_expr e2)
      | t, OptimizedQueryAST.Div(e1, e2) -> t, OptimizedQueryAST.Div(AUX_reduce_expr e1, AUX_reduce_expr e2)
      | t, OptimizedQueryAST.Sub(e1, e2) -> t, OptimizedQueryAST.Sub(AUX_reduce_expr e1, AUX_reduce_expr e2)
      | t, OptimizedQueryAST.And(a,b) -> t, OptimizedQueryAST.And(AUX_reduce_expr a, AUX_reduce_expr b)
      | t, OptimizedQueryAST.Or(a,b) -> t, OptimizedQueryAST.Or(AUX_reduce_expr a,AUX_reduce_expr b)
      | t, OptimizedQueryAST.Equals(a,b) -> t, OptimizedQueryAST.Equals(AUX_reduce_expr a,AUX_reduce_expr b)
      | t, OptimizedQueryAST.Greater(a,b) -> t, OptimizedQueryAST.Greater(AUX_reduce_expr a,AUX_reduce_expr b)
      | t, OptimizedQueryAST.Not(a) -> t, OptimizedQueryAST.Not(AUX_reduce_expr a)
      | t, OptimizedQueryAST.Id(id) -> 
        let ids = id.idText.Split([|'.'|]) |> Seq.toList
        let id = ids.Head
        let ids = ids.Tail
        match try_find_expr ({idText = id; idRange= Position.Empty}) with
        | Some (_, f_expr) when ids.Length = 0 -> t, f_expr
        | _ -> expr
      | _ -> expr


  match expr with
  | tp, OptimizedQueryAST.Let(id, t_id, Some expr, compress) when compress  -> 
    let expr = reduce_expr expr table
    tp, OptimizedQueryAST.Let(id, t_id, Some expr, compress)
  | tp, OptimizedQueryAST.Let(id, t_id, None, compress) when compress  -> 
    tp, OptimizedQueryAST.Let(id, t_id, None, compress)
  | tp, OptimizedQueryAST.Id(id) -> try_find id expr
  | tp, OptimizedQueryAST.IfThenElse(booleanExpression, block1, block2) ->
    tp, OptimizedQueryAST.IfThenElse(booleanExpression, AUX_compress_lets block1 table |> snd, AUX_compress_lets block2 table|> snd)
  | tp, OptimizedQueryAST.IfThen(booleanExpression, block,res) ->
    tp, OptimizedQueryAST.IfThen(booleanExpression, AUX_compress_lets block table|> snd,res)
  | tp, OptimizedQueryAST.Expression.Tuple(block) -> tp, OptimizedQueryAST.Expression.Tuple(AUX_compress_lets block table|> snd)

  | tp, OptimizedQueryAST.Expression.Query(q_expr) -> tp, OptimizedQueryAST.Expression.Query(q_expr)   
  | tp, OptimizedQueryAST.Literal(literal) -> tp, OptimizedQueryAST.Literal(literal) 
  | tp, expr -> AUX_reduce_expr (tp, expr)

and private AUX_compress_lets (block : OptimizedQueryAST.TypedExpression list) table =
  let mutable table : List<Id * OptimizedQueryAST.TypedExpression> = table
  let mutable new_block = []
  for expr in block do
    let expr = reduce_expr expr table
    match expr with
    | _, OptimizedQueryAST.Let(id, _, Some expr, compress) when compress -> 
      table <- (id, expr) :: table
    |_ -> ()  
    new_block <- expr :: new_block
  table, new_block |> List.rev

let compress_lets_and_guard (block : OptimizedQueryAST.TypedExpression list) (guard : OptimizedQueryAST.TypedExpression) =
  let new_table, new_block = AUX_compress_lets block []
  let new_guard = reduce_expr guard new_table
  new_guard, new_block

let rec get_guard_ids (guard : OptimizedQueryAST.TypedExpression) (entity_fields : List<Id>) : List<Id> =
  let check_id id =
    let ids = id.idText.Split([|'.'|]) |> Seq.toList
    match entity_fields |> List.tryFind ((=) {idText = ids.[0]; idRange = Position.Empty}) with
    | Some entity -> [id]
    | None when ids.[0] = "world" -> [id]
    | _ -> []

  let rec get_id (expr : OptimizedQueryAST.TypedExpression) =
      match expr with
      | t, OptimizedQueryAST.Add(e1, e2)
      | t, OptimizedQueryAST.Modulus(e1, e2)
      | t, OptimizedQueryAST.Mul(e1, e2)
      | t, OptimizedQueryAST.Div(e1, e2)
      | t, OptimizedQueryAST.Sub(e1, e2) -> get_id e1 @ get_id e2
      | t, OptimizedQueryAST.Id(id) -> check_id id
      | t, OptimizedQueryAST.Literal(_) -> []
      | t, OptimizedQueryAST.And(a,b)
      | t, OptimizedQueryAST.Or(a,b)
      | t, OptimizedQueryAST.Greater(a,b)
      | t, OptimizedQueryAST.Equals(a,b) -> get_id a @ get_id b
      | t, OptimizedQueryAST.Not(a) -> get_id a
      | t, OptimizedQueryAST.Call(OptimizedQueryAST.Instance(instance_id, method_name, b)) when method_name.idText.ToLower() = "comparestruct"-> 
        let id1 = check_id instance_id
        let b_id = b |> List.map get_id |> List.concat
        id1 @ b_id
      | _ -> []
    
  get_id guard
//  match guard with
//  | TypedAST.ImportedType(t, p), e when t = typeof<bool> -> get_id guard
//  | tp, TypedAST.Literal(literal) -> []
//  | tp, TypedAST.Expression.Sub(e1, e2)
//  | tp, TypedAST.Expression.Add(e1, e2)
//  | tp, TypedAST.Expression.Mul(e1, e2)
//  | tp, TypedAST.Expression.Div(e1, e2) -> 
//    get_guard_ids e1 entity_fields @ get_guard_ids e2 entity_fields
//  | _ -> raise (snd guard).Position "Not supported reduction."
