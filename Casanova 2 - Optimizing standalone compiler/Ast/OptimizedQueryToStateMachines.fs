module OptimizedQueryToStateMachines

open StateMachinesAST
open Common
open IdSelectionContext
open GotoContext
let mutable query_optimization_counter = 0

//t lb, lb_expr = get_fresh_label t_expr.Position e
//
//  let _id = id.idText.Split('.') |> Seq.toList |> List.rev |> List.head
//  let p = t_expr.Position
//  let entity = e.Classes.[{idText = tp.TypeName; idRange = Position.Empty}]
//  let res =
//    if entity.ContainsKey({idText = _id; idRange = Position.Empty}) then
//      let field = entity.[{idText = _id; idRange = Position.Empty}]
//      if not field.QueryOptimized then

let traverse_add_query  (expr : OptimizedQueryAST.TypedExpression) (e : Environment) raise (traverseTrivialTypedExpr : OptimizedQueryAST.TypedExpression -> TypedExpression)  =
  let (t_expr, OptimizedQueryAST.Expression.AddToQuery(e1, (TypedAST.Query(tp), OptimizedQueryAST.Id(id)))) = expr

  query_optimization_counter <- query_optimization_counter + 1

//  let lb, lb_expr = get_fresh_label t_expr.Position e

  let _id = id.idText.Split('.') |> Seq.toList |> List.rev |> List.head
  let p = t_expr.Position
  let entity = e.Classes.[fst e.CurrentEntity]
  let res =
    if entity.ContainsKey({idText = _id; idRange = Position.Empty}) then
      let field = entity.[{idText = _id; idRange = Position.Empty}]
      if not field.QueryOptimized then
          [tp, Expression.ConcatQuery([traverseTrivialTypedExpr e1; traverseTrivialTypedExpr (TypedAST.Query(tp), OptimizedQueryAST.Id(id))])] , false
      else
        let pointer_to_collection = {idText = "__" + _id + string query_optimization_counter; idRange = Position.Empty}
        let _let = TypedAST.Query(tp), Expression.Let(pointer_to_collection,TypedAST.Query(tp), traverseTrivialTypedExpr e1 |> Some)
        let _for =
          let i = {idText = "i"; idRange = Position.Empty}
          let bool_tp = TypedAST.TypeDecl.ImportedType(typeof<bool>, p)
          let int_tp = TypedAST.TypeDecl.ImportedType(typeof<int>, p)

          let block = 
            
            [ for f in e.Classes.[{idText = tp.TypeName; idRange = Position.Empty}] do
                let f = f.Value
                let current_entity = (fst e.CurrentEntity).idText
                if f.IsQueryIndex && f.Type.TypeName = current_entity then
                  let _get = (tp, Expression.DoGet((tp, Expression.IndexOf((tp ,Id(pointer_to_collection)), (int_tp, Id(i)))), f.Name.Id))
              
                  yield  TypedAST.Unit(p),
                          Expression.SetExpression(_get,
                                                  (tp, Id({idText = "this"; idRange = p})))
                  yield  TypedAST.Unit(p),
                          Expression.Call(Call.Instance(id, {idText = "Add"; idRange = p}, [(tp, OptimizedQueryAST.Expression.IndexOf((tp , OptimizedQueryAST.Id(pointer_to_collection)), (int_tp, OptimizedQueryAST.Id(i))))]))]

          TypedAST.Unit(p), Expression.AtomicFor(i, (bool_tp, Expression.Greater((int_tp, Id({idText = pointer_to_collection.idText + ".Count"; idRange = pointer_to_collection.idRange})),(int_tp, Id(i)))), (TypedAST.TypeDecl.Unit(p), Expression.Incr(int_tp, Id(i))), block)

        [_let; _for], true
    else
      raise "Error. file:OptimizedQueryToStateMachine.fs line:52" |> ignore
      [], false
  res


let rec contains_if_with_suspension found_if (b : OptimizedQueryAST.Block) =
  let rec AUX_contains_suspension found_if (expr : OptimizedQueryAST.TypedExpression)  =    
    match expr with
    | tp, OptimizedQueryAST.While(_,b) -> 
      if found_if then List.exists (AUX_contains_suspension found_if) b
      else false
    | tp, OptimizedQueryAST.For(_,_,b, _) -> 
      if found_if then List.exists (AUX_contains_suspension found_if) b
      else false
    | tp, OptimizedQueryAST.IfThen(_,b,_) -> List.exists (AUX_contains_suspension true)  b
    | tp, OptimizedQueryAST.IfThenElse(_,t,e) -> List.exists (AUX_contains_suspension true) t && List.exists (AUX_contains_suspension true) e
    | tp, OptimizedQueryAST.Yield(_)
    | tp, OptimizedQueryAST.Wait(_) -> found_if
    | _ -> false
  match b with
  [] -> false
  | (tp, OptimizedQueryAST.While(_,b)) :: _-> false
  | (tp, OptimizedQueryAST.For(_,_,b, _)) :: _ -> false
  | (tp, OptimizedQueryAST.IfThen(_,b,_)) :: xs -> 
    if contains_if_with_suspension true b then true
    else contains_if_with_suspension false xs
  | (tp, OptimizedQueryAST.IfThenElse(_,t,e)) :: xs -> 
    if contains_if_with_suspension true t && contains_if_with_suspension true e then true
    else contains_if_with_suspension false xs
  | (tp, OptimizedQueryAST.Yield(_)) :: xs -> found_if
  | (tp, OptimizedQueryAST.Wait(_)) :: xs -> found_if
  | _::xs -> contains_if_with_suspension found_if xs


let rec build_rule_codomains (game_entities : List<Id * List<TypedAST.Field>>) (entity_fields : Id * List<TypedAST.Field>) (ids : List<Id>) =
  [for x in ids do
    let id = x
    let ids = x.idText.Split('.') |> Seq.toList    
    let rec check_id (ids : string list) (entity_fields : Id * List<TypedAST.Field>) = 
      match ids with
      | [] -> []
      | [x] -> 
        match entity_fields |> snd |> Seq.tryFind(fun f -> f.Name.idText = x) with
        | None -> []
        | Some f ->  [fst entity_fields, f.Name.Id, false, id]
      | x::xs ->         
        match entity_fields |> snd |> Seq.tryFind(fun f -> f.Name.idText = x) with
        | None -> []
        | Some f ->
          match game_entities |> Seq.tryFind(fun (e, fs) -> e.idText = f.Type.TypeName) with
          | None -> [fst entity_fields, f.Name.Id, true, id] // external
          | Some (e,fs) -> check_id xs (e,fs)
    yield! check_id ids entity_fields]

let rec make_one_condition (conditions : OptimizedQueryAST.TypedExpression list) =
        match conditions with
        | [] -> failwith (Position.Empty) "..."
        | [c] -> c
        | (t, c)::cs ->
          t, OptimizedQueryAST.Or((t, c), make_one_condition cs)

let rec private make_statemachine_transition  (world_name : string)
                                              (current_entity : (Id * OptimizedQueryAST.EntityBody))
                                              (game_entities : List<Id * List<TypedAST.Field>>) 
                                              exit rule_idx (rule_ids : List<Id>) 
                                              (block : OptimizedQueryAST.Block) 
                                              (entity_fields : Id * List<TypedAST.Field>) 
                                              (e : Environment): StateMachine option =
  
  e.CurrentEntity <- current_entity
  if contains_if_with_suspension false block then 
    let new_expr = traverseBlock world_name rule_idx rule_ids exit false block e
    let new_expr_compressed = compress_block new_expr exit
    Some{
          Dependencies  = None
          Body          = new_expr_compressed
        }
  else
    let before_guard, guard, after_guard = select_guard block  
    let make_transition guard tp expr_to_change_with_the_guard is_guard_either_a_yield_or_parallaler =
      do reset_context(e)
      if is_guard_either_a_yield_or_parallaler |> not then
        let guard, before_guard = compress_lets_and_guard before_guard guard


        let interested_ids = get_guard_ids guard (entity_fields |> snd |> List.map(fun f -> f.Name.Id))
        let interested_ids = Microsoft.FSharp.Collections.Set interested_ids

        let rec build_dependency_object (interested_ids : List<Id>) acc : (Id * Id * string) list list=
          match interested_ids with
            [] -> acc
            | x :: xs ->
              let ids = x.idText.Split('.') |> Seq.toList
              let rec check_id (prev_id : string) (ids : string list) (current_entity_fields : Id * List<TypedAST.Field>) acc count : (Id * Id * string) list = 
                match ids with
                | [] -> []
                | [x] -> 
                  match current_entity_fields |> snd |> Seq.tryFind(fun f -> f.Name.idText = x) with
                  | None -> []
                  | Some f -> 
                    if f.Type.IsGeneric then []
                    else (f.Name.Id, fst current_entity_fields, (if count = 0 then "" else prev_id)) :: acc
                | x::xs -> 
                  let prev_id = if prev_id = "" then "" else prev_id + "."
              
                  match current_entity_fields |> snd |> Seq.tryFind(fun f -> f.Name.idText = x) with
                  | None -> []
                  | Some f ->
                    if f.Type.IsCasanovaEntity then
                      match game_entities |> Seq.tryFind(fun (e, fs) -> e.idText = f.Type.TypeName) with
                      | None -> []
                      | Some (e,fs) ->                   
                        check_id (prev_id + x) xs (e,fs) ((f.Name.Id, fst current_entity_fields, if count = 0 then "" else prev_id + x) :: acc) (count + 1)
                    else
                      [(f.Name.Id, fst current_entity_fields, (if count = 0 then "" else prev_id))]
                    

              match check_id "" ids entity_fields [] 0 with
              [] -> []
              | res -> let res = res |> List.rev in build_dependency_object xs (res::acc)

        let dependencies = build_dependency_object (interested_ids |> Seq.toList) []
      
        let guard = expr_to_change_with_the_guard guard
        let new_expr =  traverseBlock world_name rule_idx rule_ids exit false (before_guard @ (guard :: after_guard)) e
        let new_expr_compressed = compress_block new_expr exit
        let dependencies =  if dependencies.Length = 0 then None else Some <| (dependencies, guard) 
        Some{
              Dependencies  = dependencies
              Body          = new_expr_compressed
            }
      else
        let new_expr = traverseBlock world_name rule_idx rule_ids exit false (before_guard @ (expr_to_change_with_the_guard guard :: after_guard)) e
        let new_expr_compressed = compress_block new_expr exit
        Some{
              Dependencies  = None
              Body          = new_expr_compressed
            }

    match guard with  
    | Some (tp, OptimizedQueryAST.Wait(guard, expr_to_exchange)) ->     
      let res = make_transition guard tp (fun guard -> tp, OptimizedQueryAST.Wait(guard, expr_to_exchange)) false
      res
    | Some (tp, OptimizedQueryAST.Yield(guard)) -> make_transition guard tp (fun guard -> tp, OptimizedQueryAST.Yield(guard)) true
    | Some (tp, OptimizedQueryAST.While(c, b)) -> make_transition c tp (fun guard -> tp, OptimizedQueryAST.While(c, b)) true
    | Some (tp, OptimizedQueryAST.IfThen(c, b,res)) -> make_transition c tp (fun guard -> tp, OptimizedQueryAST.IfThen(c, b,res)) true
    | Some (tp, OptimizedQueryAST.For(i,l , b, is_atomic)) -> make_transition l tp (fun guard -> tp, OptimizedQueryAST.For(i,l, b, is_atomic)) true
    | Some (tp, OptimizedQueryAST.Choice(interruptible,choices,p)) -> 
      let guard = 
        let conditions = choices |> List.map(fun (c,_,_) -> c)      
        make_one_condition conditions

      make_transition guard tp (fun _ -> tp, OptimizedQueryAST.Choice(interruptible,choices,p)) false
    | Some (tp, OptimizedQueryAST.Parallel(_parallel,pos)) ->
        make_transition (TypedAST.TypeDecl.Unit(tp.Position),OptimizedQueryAST.Literal(BasicAST.LUnit(tp.Position))) tp (fun (b,p) -> tp, OptimizedQueryAST.Parallel(_parallel,pos)) true
  
    | _ -> None
    
and private convertRule (world_name : string)
                        (current_entity : (Id * OptimizedQueryAST.EntityBody))
                        (game_entities : List<Id * List<TypedAST.Field>>) 
                        rule_idx 
                        (rule : OptimizedQueryAST.Rule) 
                        (fields : Id * List<TypedAST.Field>) 
                        (e : Environment) : StateMachinesAST.Rule =
  let domain = rule.Domain |> List.map(fun id -> id)
  {
    Position = rule.Position
    Index = rule_idx
    Domain = build_rule_codomains game_entities fields domain
    Body = traverseBody world_name current_entity game_entities rule_idx domain rule.Body fields e
  }

and private traverseBody  (world_name : string)
                          (current_entity : (Id * OptimizedQueryAST.EntityBody))
                          (game_entities : List<Id * List<TypedAST.Field>>)  
                          rule_idx (rule_ids : List<Id>) 
                          (block : OptimizedQueryAST.Block) 
                          (fields : Id * List<TypedAST.Field>)
                          (e : Environment) : StateMachinesAST.RuleBody =  
  if block.Length = 0 then raise Position.Empty (sprintf "Internal error at %s(%s)." __SOURCE_FILE__ __LINE__) |> ignore
  let exit = Label.Create(-1, (fst block.Head).Position)
  match make_statemachine_transition world_name current_entity game_entities exit  rule_idx rule_ids block fields e with
  | None -> Atomic (traverseAtomicBlock block)
  | Some sm -> StateMachine sm


and private traverseAtomicBlock (block : OptimizedQueryAST.Block) : StateMachinesAST.AtomicBlock =
  let last = block |> Seq.skip(block.Length - 1) |> Seq.head
  let lst_but_last = block |> Seq.take(block.Length - 1) |> Seq.toList

  lst_but_last, last
and private traverseBlock (world_name : string) rule_idx  
                                    (rule_ids : List<Id>) 
                                    (exit : Label) 
                                    (is_atomic : bool)
                                    (block : OptimizedQueryAST.Block) 
                                    (e : Environment) : StateMachinesAST.Block =
  let rec AUX_traverseBlock exprs : Label * List<TypedExpression> =    
    match exprs with
    | [] -> raise Position.Empty "Sate machines internal error. Unexpected block."
    | [x] -> 
      let p = fst x
      traverseTypedBlockExpr world_name rule_idx rule_ids x exit is_atomic (TypeDecl.Unit p.Position, (if not is_atomic then GotoSuspend.Create exit |> Expression.GotoSuspend else Goto.Create exit |> Expression.Goto)) e
    | x :: xs ->
      let p = fst x      
      let ks, xs = AUX_traverseBlock xs
      let exit, x = traverseTypedBlockExpr world_name rule_idx rule_ids x ks is_atomic (TypeDecl.Unit p.Position, Goto.Create ks |> Expression.Goto) e
      exit, x @ xs
  AUX_traverseBlock block |> snd


and private traverseTrivialTypedExpr (expr : OptimizedQueryAST.TypedExpression) : StateMachinesAST.TypedExpression = 
  match expr with
  | tp, OptimizedQueryAST.Let(id, typeDecl, Some (tp', OptimizedQueryAST.IfThen(c, e1,_)), _) -> 
    let b : Block = 
      [for e in e1 |> Seq.take(e1.Length - 1) do
        yield traverseTrivialTypedExpr e
        yield TypeDecl.Unit(tp.Position), Set(id, traverseTrivialTypedExpr (e1 |> Seq.skip(e1.Length - 1) |> Seq.head))]
    tp', LetIfThen(id, typeDecl, traverseTrivialTypedExpr c, b)

  | tp, OptimizedQueryAST.Let(id, typeDecl, Some (tp', OptimizedQueryAST.IfThenElse(c, e1, e2)), _) -> 
    let rec traverse_e e first_time =
      match e with 
      | tp, OptimizedQueryAST.IfThenElse(c, e1, e2) ->
        let e1 = 
          [for e in e1 |> Seq.take(e1.Length - 1) do
            yield traverse_e e false
           let e = (e1 |> Seq.skip(e1.Length - 1) |> Seq.head)
           match e with
           |_, OptimizedQueryAST.IfThenElse(_, _, _) -> yield traverse_e e false
           |_ -> yield TypeDecl.Unit(tp.Position), Set(id, traverse_e e false)]
        let e2 = 
          [for e in e2 |> Seq.take(e2.Length - 1) do
                yield traverse_e e false
           let e = (e2 |> Seq.skip(e2.Length - 1) |> Seq.head)
           match e with
           |_, OptimizedQueryAST.IfThenElse(_, _, _) -> yield traverse_e e false
           | _ -> yield TypeDecl.Unit(tp.Position), Set(id, traverse_e e false)]
        tp', LetIfThenElse((if first_time then Some id else None), typeDecl, traverseTrivialTypedExpr c, e1, e2)
      | _ -> traverseTrivialTypedExpr e


    let res = traverse_e (tp', OptimizedQueryAST.IfThenElse(c, e1, e2)) true
    res
  
  | tp, OptimizedQueryAST.Let(id, typeDecl, Some typedExpression,_) -> tp, Expression.Let(id, typeDecl, traverseTrivialTypedExpr typedExpression |> Some)
  | tp, OptimizedQueryAST.Let(id, typeDecl, None,_) -> tp, Expression.Let(id, typeDecl, None)
  | tp, OptimizedQueryAST.Expression.Tuple(block) -> tp, Expression.Tuple([for expr in block do yield traverseTrivialTypedExpr expr])
  | tp, OptimizedQueryAST.Literal(l) -> tp, Expression.Literal(l)
  | tp, OptimizedQueryAST.Id(id) -> tp, Expression.Id(id)
  | tp, OptimizedQueryAST.Set(id, e) -> tp, Expression.Set(id, traverseTrivialTypedExpr e)
  | tp, OptimizedQueryAST.IndexOf(id, e) -> tp, Expression.IndexOf(traverseTrivialTypedExpr id, traverseTrivialTypedExpr e)
  | tp, OptimizedQueryAST.NewEntity(block) -> tp, Expression.NewEntity([for i, b in block do yield i, [for expr in b do yield traverseTrivialTypedExpr expr] ])
  | tp, OptimizedQueryAST.Call(OptimizedQueryAST.Instance(id,f, block)) -> tp, Expression.Call(Instance(id, f, [for e in block do yield e]))
  | tp, OptimizedQueryAST.Call(OptimizedQueryAST.Constructor(t,block)) -> tp, Expression.Call(Constructor(t,[for e in block do yield e]))
  | tp, OptimizedQueryAST.Call(OptimizedQueryAST.Static(t,f,block)) when block = [] -> tp, Expression.Call(StaticNoArgs(t, f))
  | tp, OptimizedQueryAST.Call(OptimizedQueryAST.Static(t,f,block)) -> tp, Expression.Call(Static(t, f, [for e in block do yield e]))
  

  | tp, OptimizedQueryAST.Add (e1,e2) -> tp, Expression.Add(traverseTrivialTypedExpr e1, traverseTrivialTypedExpr e2)
  | tp, OptimizedQueryAST.Sub (e1,e2) -> tp, Expression.Sub(traverseTrivialTypedExpr e1, traverseTrivialTypedExpr e2)
  | tp, OptimizedQueryAST.Div (e1,e2) -> tp, Expression.Div(traverseTrivialTypedExpr e1, traverseTrivialTypedExpr e2)
  | tp, OptimizedQueryAST.Modulus (e1,e2) ->  tp, Expression.Modulus(traverseTrivialTypedExpr e1, traverseTrivialTypedExpr e2)
  | tp, OptimizedQueryAST.Mul (e1,e2) -> tp, Expression.Mul(traverseTrivialTypedExpr e1, traverseTrivialTypedExpr e2)

  | tp, OptimizedQueryAST.ConcatQuery (qs) -> tp, Expression.ConcatQuery([for q in qs do yield traverseTrivialTypedExpr q])
  | tp, OptimizedQueryAST.AppendToQuery (e1,e2) -> tp, Expression.AppendToQuery(traverseTrivialTypedExpr e1, traverseTrivialTypedExpr e2)
  
  | tp, OptimizedQueryAST.Cast(t, e, p) -> tp, Expression.Cast(t, traverseTrivialTypedExpr e, p)
  | tp, OptimizedQueryAST.Not e -> tp, Expression.Not(traverseTrivialTypedExpr e)
  | tp, OptimizedQueryAST.And (e1,e2) -> tp, Expression.And(traverseTrivialTypedExpr e1, traverseTrivialTypedExpr e2)
  | tp, OptimizedQueryAST.Or (e1,e2) -> tp, Expression.Or(traverseTrivialTypedExpr e1, traverseTrivialTypedExpr e2)
  | tp, OptimizedQueryAST.Equals (e1,e2) -> tp, Expression.Equals(traverseTrivialTypedExpr e1, traverseTrivialTypedExpr e2)
  | tp, OptimizedQueryAST.Greater (e1,e2) -> tp, Expression.Greater(traverseTrivialTypedExpr e1, traverseTrivialTypedExpr e2)
  | tp, OptimizedQueryAST.Range(e1, e2, p) -> tp, Expression.Range(traverseTrivialTypedExpr e1, traverseTrivialTypedExpr e2, p)
  | tp, OptimizedQueryAST.Maybe(OptimizedQueryAST.JustExpr(e)) -> tp, Expression.Maybe(MaybeExpr.JustExpr(traverseTrivialTypedExpr e))
  | tp, OptimizedQueryAST.Maybe(OptimizedQueryAST.NothingExpr(p)) -> tp, Expression.Maybe(MaybeExpr.NothingExpr(p))
  | tp, OptimizedQueryAST.AppendToQuery (e1,e2) -> tp, Expression.Greater(traverseTrivialTypedExpr e1, traverseTrivialTypedExpr e2)
  | tp, OptimizedQueryAST.Expression.Query(q) -> tp, Expression.Query(q)
  | tp, OptimizedQueryAST.Expression.MutableExpression(e) -> traverseTrivialTypedExpr (e.Map e.MutableExpression.Expr)
  | e -> raise (snd expr).Position "Sate machines error. Not trivial expression not supported, yet."



and private traverseTypedBlockExpr  
                           (world_name : string)                         
                           (rule_idx : int)
                           (rule_ids : List<Id>)
                           (expr : OptimizedQueryAST.TypedExpression) 
                           (exit_label : Label)
                           (is_atomic : bool)
                           (exit_expr : TypedExpression)
                           (e : Environment) : Label * (TypedExpression list) =
  let raise = raise (expr |> snd).Position
  match expr with
(*
[[while C do
	  B]] _exit
=>
lb:
	if !C then
		goto _exit
  else goto _else
_else:
		[[B]] _lb
*)
  | t_expr, OptimizedQueryAST.While(b_expr : OptimizedQueryAST.TypedExpression, block) -> 
    let bool_type = TypedAST.TypeDecl.ImportedType(typeof<bool>, (fst b_expr).Position)

    let label_id, label = get_fresh_label t_expr.Position e
    let else_label_id, else_label = get_fresh_label t_expr.Position e
    let _then = exit_expr
    let _else = traverseBlock world_name rule_idx rule_ids label_id false block e
    let if_then_else = 
      TypeDecl.Unit t_expr.Position, 
      Expression.IfThenElse((bool_type, StateMachinesAST.Expression.Not(traverseTrivialTypedExpr b_expr)),None, [_then], [(TypeDecl.Unit t_expr.Position, Goto.Create else_label_id |> Expression.Goto)])
    label_id, [label; if_then_else; else_label] @ _else

(*
[[for a in A do
	B]] _exit

=>

for_lb:
  var counter = -1
  if A.length = 0 then
	  goto _exit
  else
	  var a = A.[0]
    goto lb
lb:
	counter ++
	if counter >= A.length then
		goto _exit
	else
		a = A.[counter]
    goto _else
_else:
		[[B]] _lb
*)
  | t_expr, OptimizedQueryAST.For([id], (q_t, q_expr), b_expr, is_atomic) ->

    let query_tp =
      match q_t with
      | TypeDecl.Query(query_tp) -> query_tp
      | TypedAST.TypeDecl.ImportedType(st, p) when st.GetGenericTypeDefinition() = typedefof<seq<_>> || st.GetInterface("IEnumerable`1") <> null -> 
        TypedAST.TypeDecl.ImportedType(st.GetGenericArguments().[0], p)
      | _ -> raise "Only queries and IEnumerables can be iterated in a for." |> ignore
             q_t
    let bool_type = TypedAST.TypeDecl.ImportedType(typeof<bool>, id.idRange)
    let int_type = TypedAST.TypeDecl.ImportedType(typeof<int>, id.idRange)
    let for_lb, for_lb_expr = get_fresh_label t_expr.Position e
    let expr = q_t, q_expr
    


    let lb, lb_expr = get_fresh_label t_expr.Position e
    let else_lb, else_lb_expr = get_fresh_label t_expr.Position e
    
    let goto_else_lb = (TypeDecl.Unit t_expr.Position, Goto.Create else_lb |> Expression.Goto)

    let counter_id = {idText = "counter" + string lb.Id + string rule_idx; idRange = t_expr.Position}
    let counter =  TypeDecl.ImportedType(typeof<int>, id.idRange), 
                   Expression.Var(counter_id, 
                                 TypeDecl.ImportedType(typeof<int>,id.idRange), 
                                 Some (TypeDecl.ImportedType(typeof<int>,id.idRange), Expression.Literal(Literal.Int(-1, t_expr.Position))))

    let id_expr =  TypeDecl.ImportedType(typeof<int>, id.idRange), 
                   Expression.Var(id, query_tp, None)

//    let expr_id = {idText = "list" + string for_lb.Id; idRange = t_expr.Position}
//    let var_expr = q_t, Expression.Var(expr_id, q_t, traverseTrivialTypedExpr expr |> Some)
    let expr = traverseTrivialTypedExpr expr


    let goto_lb = (TypeDecl.Unit t_expr.Position, Goto.Create lb |> Expression.Goto)

    let expr_count = int_type, StateMachinesAST.DoGet(expr, {idText = "Count"; idRange = (snd expr).Position})

    let init_if_then_else = 
      TypeDecl.Unit(id.idRange), Expression.IfThenElse((bool_type, (Equals((expr_count), 
                                                                           (int_type, Literal(BasicAST.Int(0,id.idRange)))))), 
                                           None,
                                           [exit_expr], 
                                           [TypeDecl.Unit(id.idRange), Expression.Set(id, (query_tp, Expression.IndexOf(expr , (TypeDecl.ImportedType(typeof<int>,id.idRange), Expression.Literal(Literal.Int(0, id.idRange))))));
                                            goto_lb])


    let incr_counter = 
      TypeDecl.Unit id.idRange, 
      Expression.Set(counter_id, 
                     (TypeDecl.ImportedType(typeof<int>, id.idRange), 
                      Add((int_type, Id(counter_id)), 
                          (int_type, Literal(BasicAST.Int(1, id.idRange))))))
    let if_then_else = 
      TypeDecl.Unit id.idRange, Expression.IfThenElse((bool_type, Or((bool_type, Equals(expr_count, (int_type, Id(counter_id)))),
                                                                     (bool_type, Greater((int_type, Id(counter_id)), expr_count)))), 
                                                     None,
                                                     [exit_expr], 
                                                     [TypeDecl.Unit(id.idRange), Expression.Set(id, (query_tp, Expression.IndexOf(expr , (TypeDecl.Unit(id.idRange), Id(counter_id))))); goto_else_lb])

    let else_expr = traverseBlock world_name rule_idx rule_ids lb is_atomic b_expr e

    for_lb, [for_lb_expr; id_expr; counter; init_if_then_else; lb_expr; incr_counter; if_then_else; else_lb_expr] @ else_expr
    
(*
----------------------------------------
[[if C then A else B]] _exit
=>
lb:
  if C then _then else _else
_then:
	  [[B]] _exit
_else
	  [[C]] _exit

*)
  | t_expr, OptimizedQueryAST.IfThenElse(b_expr, block1, block2) -> 

    let then_lb, then_lb_expr = get_fresh_label t_expr.Position e
    let else_lb, else_lb_expr = get_fresh_label t_expr.Position e

    let lb, lb_expr = get_fresh_label t_expr.Position e
    let _then = traverseBlock world_name rule_idx rule_ids exit_label is_atomic block1 e  
    let _else = traverseBlock world_name rule_idx rule_ids exit_label is_atomic block2 e

    let goto_then = (TypeDecl.Unit t_expr.Position, Goto.Create then_lb |> Expression.Goto)
    let goto_else = (TypeDecl.Unit t_expr.Position, Goto.Create else_lb |> Expression.Goto)



    let if_then_else = TypeDecl.Unit (fst b_expr).Position, Expression.IfThenElse(traverseTrivialTypedExpr b_expr, None,[goto_then], [goto_else])
    
    
    lb, [lb_expr; if_then_else; then_lb_expr] @ _then @ [else_lb_expr] @ _else

(*
----------------------------------------
[[if C then B]] _exit
=>
lb:
  if C then 
	  [[B]] _exit

*)
  | t_expr, OptimizedQueryAST.IfThen(b_expr , block1, goto_next_block) -> 
    let lb, lb_expr = get_fresh_label t_expr.Position e
    let then_lb, then_lb_expr = get_fresh_label t_expr.Position e

    let _then = traverseBlock world_name rule_idx rule_ids exit_label (is_atomic || goto_next_block) block1 e
    let goto_then = (TypeDecl.Unit t_expr.Position, Goto.Create then_lb |> Expression.Goto)
//
//    let goto_exit =
//      match exit_expr with
//      | t, GotoSuspend(g) when goto_next_block -> t, Goto(Goto.Create(g.Label))
//      | t, Goto(g) -> exit_expr

    let if_then = TypeDecl.Unit (fst b_expr).Position, Expression.IfThenElse(traverseTrivialTypedExpr b_expr , None, [goto_then], [exit_expr])
    lb, [lb_expr; if_then; then_lb_expr] @ _then
  

(*
----------------------------------------
[[when C]] _exit
=>
lb:
  if !C then
	  gotoSuspend _lb
  else
	  goto _exit

*)
  | t_expr, OptimizedQueryAST.Expression.Wait((tp, b_expr), expr_to_exchange) when tp = TypedAST.TypeDecl.ImportedType(typeof<bool>, tp.Position) -> 
    let bool_type = TypedAST.TypeDecl.ImportedType(typeof<bool>, tp.Position)
    let lb, lb_expr = get_fresh_label t_expr.Position e
    let _then = [(TypeDecl.Unit tp.Position, GotoSuspend.Create lb |> Expression.GotoSuspend)]
    let _else = [exit_expr]
    let cond = bool_type, Not(traverseTrivialTypedExpr (tp, b_expr))
    let expr_to_exchange =
      match expr_to_exchange with
      | None -> None
      | Some expr -> (bool_type, Not(traverseTrivialTypedExpr expr)) |> Some
    let if_then_else = TypeDecl.Unit tp.Position, Expression.IfThenElse(cond, expr_to_exchange, _then, _else)
    lb, [lb_expr; if_then_else]
(*
----------------------------------------
[[when C]] _exit
=>
lb:
  var waitn = expr()
  if waitn.IsNone then
	  gotoSuspend _lb
  else
	  goto _exit

*)
  | t_expr, OptimizedQueryAST.Expression.LetWait(id, tp, expr, _) -> 
    let bool_type = TypedAST.TypeDecl.ImportedType(typeof<bool>, tp.Position)
    let lb, lb_expr = get_fresh_label t_expr.Position e

    e.CountdownCounter <- e.CountdownCounter + 1
    let test_id = {idText = "wait" + (string e.CountdownCounter); idRange = t_expr.Position}
    let test =  tp, Expression.Var(test_id, tp, traverseTrivialTypedExpr expr.Value |> Some)


    let _then = [(TypeDecl.Unit tp.Position, GotoSuspend.Create lb |> Expression.GotoSuspend)]
    let _else = [exit_expr]
    let cond = bool_type, Expression.Id({ idText = test_id.idText + ".IsNone"; idRange = test_id.idRange })
    let if_then_else = TypeDecl.Unit tp.Position, Expression.IfThenElse(cond, None, _then, _else)
    lb, [lb_expr;test; if_then_else]

(*
----------------------------------------
[[wait T]] _exit
=>
lb:
  var count_down = T
  goto wait_lb
wait_lb:
  if count_down > 0.0 then
	  count_down -= dt
	  gotoSuspend _exit
  else
  	goto _exit

*)

  | t_expr, OptimizedQueryAST.Wait((tp, expr), _) ->
    let float_type = TypeDecl.ImportedType(typeof<float32>, t_expr.Position)
    let bool_type = TypeDecl.ImportedType(typeof<bool>, t_expr.Position)
    e.CountdownCounter <- e.CountdownCounter + 1
    let tp = TypeDecl.ImportedType(typeof<float32>, tp.Position)
    let lb, lb_expr = get_fresh_label t_expr.Position e
    let count_down_id = {idText = "count_down" + (string e.CountdownCounter); idRange = t_expr.Position}

    let f_expr = 
      match expr with
      | f_expr when tp = TypedAST.TypeDecl.ImportedType(typeof<float32>, t_expr.Position) -> f_expr
//      | i_expr when tp = TypedAST.TypeDecl.ImportedType(typeof<int>, t_expr.Position) -> TypedAST.ImplicitIntCast(i_expr)
      | _ -> raise "Sate machines error. Wait arguments can be either of type int or float."

    let count_down =  TypeDecl.ImportedType(typeof<float32>,t_expr.Position), Expression.Var(count_down_id , tp, traverseTrivialTypedExpr (tp, f_expr) |> Some)
    let wait_lb, wait_lb_expr = get_fresh_label t_expr.Position e
    let goto_wait_lb = (TypeDecl.Unit t_expr.Position, Goto.Create wait_lb |> Expression.Goto)
    let _then = 
     [(TypeDecl.Unit t_expr.Position, 
       Expression.Set(count_down_id, 
                      (float_type, 
                       StateMachinesAST.Sub((float_type, StateMachinesAST.Id(count_down_id)), 
                                            (float_type, StateMachinesAST.Id({idText = "dt"; idRange = t_expr.Position}))))));
      (TypeDecl.Unit t_expr.Position, GotoSuspend.Create wait_lb |> Expression.GotoSuspend)]
    let _else = [exit_expr]
    let cond = bool_type, Greater((float_type, Id(count_down_id)), (float_type,StateMachinesAST.Expression.Literal(BasicAST.Float(0.0f, t_expr.Position))))
    let if_then_else = TypeDecl.Unit tp.Position, Expression.IfThenElse(cond,None, _then, _else)
    lb, [lb_expr; count_down; goto_wait_lb; wait_lb_expr; if_then_else]

  | t_expr, OptimizedQueryAST.ReEvaluateRule(p) ->

    let lb, lb_expr = get_fresh_label t_expr.Position e
    let goto_exit =
      match exit_expr with
      | t, GotoSuspend(g) -> t, Goto(Goto.Create(g.Label))
      | t, Goto(g) -> exit_expr

    lb, [lb_expr ; goto_exit]

//  | t_expr, OptimizedQueryAST.Expression.SubtractToQuery(e1, (TypedAST.Query(tp), OptimizedQueryAST.Id(id))) when tp.IsCasanovaEntity ->
////  | t_expr, OptimizedQueryAST.Yield((TypedAST.TypeDecl.Tuple(_), OptimizedQueryAST.Expression.Tuple(values))) -> 
//    traverse_subtract_query expr (e : Environment) raise traverseTrivialTypedExpr

  
(*
----------------------------------------
[[yield E]] _exit
=>
_lb:
  set E
  gotoSuspend _exit
*)
  | t_expr, OptimizedQueryAST.Yield((TypedAST.TypeDecl.Tuple(_), OptimizedQueryAST.Expression.Tuple(values))) -> 
    let rec yield_to_set ids yield_values =
      match ids, yield_values with
      | [], [] -> []
      | id::ids, (value)::values ->
        match value with
        | _, OptimizedQueryAST.Expression.AddToQuery(_) ->
          let res, has_optimized = traverse_add_query value (e : Environment) raise traverseTrivialTypedExpr 
          if has_optimized then            
            res @ yield_to_set ids values
          else
            (TypedAST.TypeDecl.Unit id.idRange, Expression.Set(id, res.Head)) :: yield_to_set ids values

        | _ -> 
          (TypedAST.TypeDecl.Unit id.idRange, Expression.Set(id, traverseTrivialTypedExpr value)) :: yield_to_set ids values
      | _ -> raise "Sate machines error. Yield values and rule domain mismatch: they must be of the same length." |> ignore; []
    let lb, lb_expr = get_fresh_label t_expr.Position e
    let goto_exit = exit_expr
    let goto_exit =
      match goto_exit with
      | _, GotoSuspend(_) -> goto_exit
      | t, Goto(g) -> t, GotoSuspend(GotoSuspend.Create(g.Label))


    lb, [lb_expr] @ yield_to_set rule_ids values @ [goto_exit]


(*
----------------------------------------
[[.&& b1
  .&& b2
  ...
  .&& bn]] _exit
=>

m1 = [[[b1]]]
m2 = [[[b2]]]
...
mn = [[[bn]]]

_lb:
m1()
m2()
...
mn()
gotosuspend _lb


*)

  | t_expr, OptimizedQueryAST.Parallel(_parallel,_) ->
    let old_nesting = e.ParallelNesting
    e.ParallelNesting <- e.ParallelNesting + 1
    let lb, lb_expr = get_fresh_label t_expr.Position e
    let label_counter = e.GetAndResetLabelCounter()
    let aux_methods_list =
      [for p,_ in _parallel do
        let exit = Label.Create(-1, t_expr.Position)
        do e.GetAndResetLabelCounter() |> ignore
        yield compress_block (traverseBlock world_name rule_idx rule_ids exit false p e) exit] |> List.mapi(fun i elem -> i, elem)

    for i,b in aux_methods_list do
      let m_name = (string rule_idx) + string old_nesting + (string i)
      do e.ParallelAuxMethods.Add(StateMachine({Dependencies = None; Body = b}), m_name)

    e.LabelCounter <- label_counter
    
    let call_list : StateMachinesAST.Block =
      [for i,e in aux_methods_list do
          let m_name = (string rule_idx) + string old_nesting + (string i)
          let dt_formal = StateMachinesAST.TypeDecl.ImportedType(typeof<float32>,t_expr.Position),OptimizedQueryAST.Id({idText = "dt";idRange = t_expr.Position})
          let world_formal = (StateMachinesAST.TypeDecl.EntityName(TypedAST.Id.buildFrom {idText = world_name;idRange = t_expr.Position}),
                              OptimizedQueryAST.Id({idText = "world";idRange = t_expr.Position}))
          yield StateMachinesAST.TypeDecl.Unit(t_expr.Position),
                StateMachinesAST.Expression.Call(StateMachinesAST.Instance({idText = "this"; idRange = t_expr.Position},
                                                                           {idText = "parallelMethod" + m_name; idRange = t_expr.Position},[dt_formal;world_formal]))]
    
    lb, [lb_expr] @ call_list @ [StateMachinesAST.TypeDecl.Unit(t_expr.Position), GotoSuspend(GotoSuspend.Create(lb))]

(*
----------------------------------------
[[.|c1 -> b1
  .|c2 -> b2
  ...
  .|cn -> bn]] _exit
=>
[[wait c1||c2||..||cn]] _lb
_lb:
  if c1 then goto _c1
  elif c2 then goto _c2
  ..
  elif cn then goto _cn
  else gotoSuspend _lb
_c1:
  [[b1]] _exit
_c2:
  [[b2]] _exit
..
_cn:
  [[bn]] _ exit
*)


  | t_expr, OptimizedQueryAST.Choice(false,choices,_) -> 
  
    let conditions = choices |> List.map(fun (c,_,_) -> c)
    let wait = TypeDecl.Unit(t_expr.Position), 
               OptimizedQueryAST.Wait((TypedAST.TypeDecl.ImportedType(typeof<bool>, t_expr.Position), (make_one_condition conditions |> snd)),None)
    
    let lb, lb_expr = get_fresh_label t_expr.Position e
    let wait_lb, wait = traverseTypedBlockExpr  world_name rule_idx rule_ids wait lb is_atomic (TypeDecl.Unit t_expr.Position, Goto.Create lb |> Expression.Goto) e


    let choices_labels =
      [for c in choices do
        yield get_fresh_label t_expr.Position e]    
    let rec build_choice choices =
      match choices with
      | [] -> (TypeDecl.Unit t_expr.Position, GotoSuspend.Create lb |> Expression.GotoSuspend)
      | (i,c,_,_) :: cs ->
        let _else = build_choice cs
        let _then = (TypeDecl.Unit t_expr.Position, Goto.Create (fst choices_labels.[i]) |> Expression.Goto)
        TypeDecl.Unit t_expr.Position, Expression.IfThenElse(c, None, [_then], [_else])

    let select_cond_expr = build_choice (choices |> List.mapi(fun i (c,b,p) -> i,(traverseTrivialTypedExpr c ),b,p))

    let choices =
      [for i,l,lexpr in (choices_labels |> List.mapi(fun i (l,lexpr) -> i,l,lexpr)) do
        yield lexpr
        let _,c_body,_ = choices.[i]
        yield! traverseBlock world_name rule_idx rule_ids exit_label false c_body e]
        

    let res = wait_lb, wait @ [lb_expr; select_cond_expr] @ choices
    res

(*
----------------------------------------
[[
  !| c1 => b1
  !| c2 => b2
  ...
  !| cn => bn]] _exit
=>
m =
  m_done = false
  current_state = -1 
[[[
  if (c1 && current_state <> 1) then
    resetInnerStates 
    current_state = 1
    set lc1
  elif (c2 && current_state <> 2) then
    resetInnerStates 
    current_state = 2
    set lc2
  ...
  elif (cn && current_state <> n) then
    resetInnerStates 
    current_state = n
    set lcn
  else set _lc1
_lc1: 
  [[b1]] m_exit
_lc2:
  [[b2]] m_exit
...
_lcn:
  [[bn]] m_exit
m_exit:
  m_done = true
  gotoSuspend _lc1  
      
]]] 


----------------------A
_lb:
  m()
  if m_done then
    resetInnerStates
    goto _exit
  else
    gotoSuspend _lb
*)

  | t_expr, OptimizedQueryAST.Choice(true,choices,_) ->
    let conditions = choices |> List.mapi(fun i (c,_,_) -> i,c)
    let old_nesting = e.ConcurrentNesting
    e.ConcurrentNesting <- e.ConcurrentNesting + 1
    
    let label_counter = e.GetAndResetLabelCounter()   

    let choices_labels =
      [for c in choices do
        yield get_fresh_label t_expr.Position e]
    let m_exit_label, m_exit = get_fresh_label t_expr.Position e

    let _default_label, _default_label_expr = get_fresh_label t_expr.Position e
    let _default =
      [for i,(c,_) in choices_labels |> Seq.mapi(fun i e -> i,e) do
        if i = 0 then
          yield TypeDecl.Unit(c.Position), Expression.GotoSuspend(GotoSuspend.Create(c))
        else 
          yield TypeDecl.Unit(c.Position), Expression.Goto(Goto.Create(c))]

    let choices =
      [for i,l,lexpr in (choices_labels |> List.mapi(fun i (l,lexpr) -> i,l,lexpr)) do
        yield lexpr
        let _,c_body,_ = choices.[i]
        yield! traverseBlock world_name rule_idx rule_ids m_exit_label false c_body e]
    
    let m_done_text index = "__m_done" + (string rule_idx) + (string index)
    let reset_indices (indices : int) =
      [for i = old_nesting to e.ConcurrentNesting - 1 do
          let (reset_state : TypedExpression) = TypeDecl.Unit(t_expr.Position),
                                                Expression.Set({idText = "__m_current_state" + (string rule_idx) + (string i); idRange = t_expr.Position},
                                                               (TypeDecl.ImportedType(typeof<int>,t_expr.Position),Expression.Literal(Literal.Int(-1, t_expr.Position))))
          let (reset_done : TypedExpression) =  TypeDecl.Unit(t_expr.Position),
                                                Expression.Set({idText = m_done_text i; idRange = t_expr.Position},
                                                               (TypeDecl.ImportedType(typeof<bool>,t_expr.Position),Expression.Literal(Literal.Bool(false, t_expr.Position))))
          yield reset_state
          yield reset_done]

    let bool_type = TypedAST.TypeDecl.ImportedType(typeof<bool>, t_expr.Position)
    let int_type = TypedAST.TypeDecl.ImportedType(typeof<int>, t_expr.Position)

    let rec generate_conditions (choices : (int * OptimizedQueryAST.TypedExpression) list) =
      match choices with
      | [] ->
          let _then =
            TypeDecl.Unit(t_expr.Position),
            Expression.Set({idText = "s" + (string rule_idx) + (string old_nesting); idRange = t_expr.Position},
                            (TypeDecl.ImportedType(typeof<int>,t_expr.Position),Expression.Literal(Literal.Int(_default_label.Id, t_expr.Position))))
          
          let rec _cond conds =
            match conds with
            [_,x] -> bool_type, Not(traverseTrivialTypedExpr x)
            | (_,x)::xs -> bool_type, And((bool_type, Not(traverseTrivialTypedExpr x)), _cond xs)              
          TypeDecl.Unit t_expr.Position, Expression.IfThen(_cond conditions,[_then])
      | (i,choice) :: cs -> 
          let inner_if =
            let current_state = Id({idText = "__m_current_state" + (string rule_idx) + (string old_nesting); idRange = t_expr.Position})
            let state_comparison =
              bool_type, Not(bool_type, Equals((int_type, current_state), (int_type, Expression.Literal(BasicAST.Int(i,(fst choice).Position)))))
            let _then = 
              let (set_state : TypedExpression) = 
                TypeDecl.Unit(t_expr.Position),
                Expression.Set({idText = "__m_current_state" + (string rule_idx) + (string old_nesting); idRange = t_expr.Position},
                               (TypeDecl.ImportedType(typeof<int>,t_expr.Position),Expression.Literal(Literal.Int(i, t_expr.Position))))
              let reset_state =
                TypeDecl.Unit(t_expr.Position),
                Expression.Set({idText = "s" + (string rule_idx) + (string old_nesting); idRange = t_expr.Position},
                            (TypeDecl.ImportedType(typeof<int>,t_expr.Position),Expression.Literal(Literal.Int((fst choices_labels.[i]).Id,t_expr.Position))))
              reset_indices(i) @ [reset_state; set_state]
            TypeDecl.Unit t_expr.Position, Expression.IfThen(state_comparison,_then)
            
          let _else = [generate_conditions cs]
          TypeDecl.Unit t_expr.Position, Expression.IfThenElse(traverseTrivialTypedExpr choice, None,[inner_if],_else)


    let concurrentSelectMethodBody = 
      let select_choice = generate_conditions conditions 
      let choices = _default_label_expr :: _default @ choices      
      let set_done =
        TypeDecl.Unit(t_expr.Position),
        Expression.Set({idText = m_done_text old_nesting; idRange = t_expr.Position},
                        (TypeDecl.ImportedType(typeof<bool>,t_expr.Position),Expression.Literal(Literal.Bool(true, t_expr.Position))))
      let goto_suspend = (TypeDecl.Unit((fst expr).Position), Expression.Goto(Goto.Create(_default_label)))
      let exit_choices = [m_exit; set_done; goto_suspend]
      let state_machine = compress_block_no_exit (choices @ exit_choices)
      select_choice :: state_machine
    let concurrentSelectMethodBody = StateMachine({Dependencies = None; Body = concurrentSelectMethodBody})
    
    let m_name = (string rule_idx) + (string old_nesting)
    e.ConcurrentAuxMethods.Add (concurrentSelectMethodBody, m_name, m_done_text old_nesting, "__m_current_state" + (string rule_idx) + (string old_nesting))
    e.LabelCounter <- label_counter
//----------------------A
    let lb, lb_expr = get_fresh_label t_expr.Position e
    let m_done = bool_type, Id({idText = m_done_text old_nesting; idRange = t_expr.Position})
    
    let method_call =
      let dt_formal = StateMachinesAST.TypeDecl.ImportedType(typeof<float32>,t_expr.Position),OptimizedQueryAST.Id({idText = "dt";idRange = t_expr.Position})
      let world_formal = (StateMachinesAST.TypeDecl.EntityName(TypedAST.Id.buildFrom {idText = world_name;idRange = t_expr.Position}),
                          OptimizedQueryAST.Id({idText = "world";idRange = t_expr.Position}))
      StateMachinesAST.TypeDecl.Unit(t_expr.Position),
      StateMachinesAST.Expression.Call(StateMachinesAST.Instance({idText = "this"; idRange = t_expr.Position},
                                                                  {idText = "concurrentSelectMethod" + (string rule_idx) + (string old_nesting); idRange = t_expr.Position},[dt_formal;world_formal]))

    let _then = (reset_indices(-1)) @ [exit_expr]
    let _else = [(TypeDecl.Unit t_expr.Position, GotoSuspend.Create lb |> Expression.GotoSuspend)]
    let if_then_else = TypeDecl.Unit t_expr.Position, Expression.IfThenElse(m_done,None,_then,_else)
    lb, [lb_expr; method_call; if_then_else] 




(*
----------------------------------------
[[E]] _exit
=>
_lb:
  E
  goto _exit
*)
  | _, _ -> 
    let lb, lb_expr = get_fresh_label (fst expr).Position e
    let expr = traverseTrivialTypedExpr expr
    let goto_exit = exit_expr
    lb, [lb_expr; expr; goto_exit]



and private convertBody (world_name : string) entity_name (game_entities : List<Id * List<TypedAST.Field>>) (body : OptimizedQueryAST.EntityBody) (e : Environment) : StateMachinesAST.EntityBody =
  let body_fields = body.Fields |> Seq.map(fun f -> f.Value) |> Seq.toList
  e.ParallelAuxMethods.Clear()
  e.ConcurrentAuxMethods.Clear()
  let rules = [for  rule_idx, rule in body.Rules |> Seq.mapi(fun i r -> i,r) do
                  let game_entities = game_entities |> List.filter(fun (id,_) -> id.idText <> entity_name.idText)
                  let game_entities1 = (entity_name, body_fields) :: game_entities
                  yield convertRule world_name (entity_name, body) game_entities1 rule_idx rule (entity_name, body_fields) e]
  let parallel_methods : ResizeArray<RuleBody * string> = ResizeArray()
  let concurrent_methods : ResizeArray<RuleBody * string * string * string> = ResizeArray()
  for m in e.ParallelAuxMethods do
    parallel_methods.Add(m)
  for m in e.ConcurrentAuxMethods do
    concurrent_methods.Add(m)
    
  {
    Fields                = body.Fields |> Seq.map(fun i -> i.Value) |> Seq.toList
    Rules                 = rules
    ParallelMethods       = parallel_methods
    ConcurrentMethods     = concurrent_methods
    Create                = convertCreate body.Create
  }

and private convertCreate create : StateMachinesAST.Create =
  {
    Args  = create.Args |> List.map(fun (id, tp) -> id, tp)
    Body  = traverseAtomicBlock create.Body
    Position = create.Position
  }

and private convertEntity world_name (game_entities : List<Id * List<TypedAST.Field>>) (entity : OptimizedQueryAST.Entity) (e : Environment) : StateMachinesAST.Entity =
  {
    Name      = entity.Name
    Body      = convertBody world_name entity.Name game_entities entity.Body e
  }

and private convertWorld (game_entities : List<Id * List<TypedAST.Field>>) (world : OptimizedQueryAST.World) (e : Environment) : StateMachinesAST.World =
  {
    Name      = world.Name
    Body      = convertBody world.Name.idText world.Name game_entities world.Body e
  }


let convertProgram (program : OptimizedQueryAST.Program) (e : Environment) : StateMachinesAST.Program =
  let world_fields = program.World.Body.Fields |> Seq.map(fun f -> f.Value) |> Seq.toList
  
  let game_entities : List<Id * List<TypedAST.Field>> =  [yield program.World.Name, world_fields; 
                                                          for entity in program.Entities do
                                                            let entity_fields = entity.Body.Fields |> Seq.map(fun f -> f.Value) |> Seq.toList                                                          
                                                            yield entity.Name, entity_fields]
  
  { 
    Module    = program.Module
    Imports   = program.Imports |> List.map(fun i -> i)
    World     = convertWorld game_entities program.World e
    Entities  = [for entity in program.Entities do yield convertEntity program.World.Name.idText game_entities entity e]
  }

(*
----------------------------------------
In general:
----------------------------------------
[[A
  B]] _exit
=>
_a:
  [[A]] _b
_b:
  [[B]] _ exit
----------------------------------------
[[A
  goto _lb
_lb:
  B]] _exit
=>
[[A
_lb:
  B]] _exit
----------------------------------------
[[[A]]]           [[[...]]] initial block, while [[...]] a generic block
=>
exit:
  [[A]] _exit
=>
exit:
_a:
  [[A]] _exit
*)