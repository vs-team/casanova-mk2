module RulesPrettyPrinter
open Common
open QueryTraverseContext


let rec traverse_query_expr (query_symbols_context : bool * Option<QueryContext>) (tp : TypedAST.TypeDecl) (e : StateMachinesAST.AtomicQueryExpression) (return_ids : Id list) tabs = 
  
  let (last_expr_let : bool), query_symbols_context = query_symbols_context

  let t_name = 
      match tp with
      | TypedAST.Query(t) -> t.TypeName
      | _ -> tp.TypeName

  let (OptimizedQueryAST.QueryStatements(qs)) = e
  let rec eval_query q query_symbols_context =
    let prev_query_symbols_context = query_symbols_context        
    let traverse query_symbols_context expr last_expr_let =  traverse_atomic_expr true expr (last_expr_let, Some query_symbols_context) [] "" true
    
    "\n" +
    match q with 
    | [] -> ""
    | e :: qs ->
      let query_symbols_context = QueryContext.GetFreshContext(query_symbols_context, 0)
      match e with
      | OptimizedQueryAST.InnerQueryExpression.For(id, tp, e) -> 
          query_symbols_context.AddId(id)
          query_symbols_context.LastForId <- Some id
          let e query_symbols_context = traverse query_symbols_context e false
          match query_symbols_context.PrevContextSymbol, last_expr_let with
          | None, _
          | _, true ->
            let prev_context = match query_symbols_context.PrevContextSymbol with | None -> "" | Some c -> ",prev = " + c
            "(" + e (if prev_query_symbols_context.IsSome then prev_query_symbols_context.Value else query_symbols_context) + ").Select(" + query_symbols_context.CurrentContextSymbol + " => " +
                                  "new { " + id.idText + " = " + query_symbols_context.CurrentContextSymbol + prev_context + " })"
            + eval_query qs (Some query_symbols_context)
          | Some c, _ ->
           
            let query_symbols_context' = QueryContext.GetFreshContext(Some query_symbols_context, 0)
            let traverse expr =  traverse_atomic_expr true expr (false, Some query_symbols_context') [] "" true
 
            ".SelectMany(" + query_symbols_context.CurrentContextSymbol + "=> " +
                        "(" + e query_symbols_context + ").Select(" + query_symbols_context'.CurrentContextSymbol + " => " +
                                              "new { " + id.idText + " = " + query_symbols_context'.CurrentContextSymbol + ",
                                                      prev = " + query_symbols_context.CurrentContextSymbol + " })" + eval_query qs (Some query_symbols_context) + ")" 
      
          
      | OptimizedQueryAST.Select(e) -> 
        
        ".Select(" + query_symbols_context.CurrentContextSymbol + " => " + traverse query_symbols_context e false + ")" + eval_query qs (Some query_symbols_context) + (if qs = [] then ".ToList<" + t_name + ">()" else "")


      | OptimizedQueryAST.Where(t, OptimizedQueryAST.Expression.Query(OptimizedQueryAST.QueryExpression.QueryStatements(OptimizedQueryAST.InnerQueryExpression.For(i,l,b) :: qs))) -> 
        match query_symbols_context.LastForId with
        | Some _ ->

          let for_id = query_symbols_context.LastForId.Value
          let e = t, OptimizedQueryAST.Expression.Query(OptimizedQueryAST.QueryExpression.QueryStatements(OptimizedQueryAST.InnerQueryExpression.For(i,l,b) :: qs))
          query_symbols_context.DecrPadding()
          let res = ".Where(" + query_symbols_context.CurrentContextSymbol + " => " + traverse query_symbols_context e true + ")"
          let res = res + ".Select(ctxt => ctxt." + for_id.idText + ")"
          res
        | _ -> raise e.Position (sprintf "Internal error at %s(%s). " __SOURCE_FILE__ __LINE__)

      | OptimizedQueryAST.Where(be) -> 
        let res = 
          ".Where(" + query_symbols_context.CurrentContextSymbol + " => " + 
          traverse query_symbols_context be false + ")" 
        query_symbols_context.DecrPadding()          
        res + eval_query qs (Some query_symbols_context)
      

      | OptimizedQueryAST.Empty((tp),p) -> "Enumerable.Empty<" + tp.TypeName + ">()"
      | OptimizedQueryAST.LiteralList(qs) -> 
        let t_name = 
          match tp with
          | TypedAST.Query(t) -> t.TypeName
          | _ -> tp.TypeName            
        let rec concat_queries (qs : OptimizedQueryAST.TypedExpression list) =
          match qs, tp with
          | [], TypedAST.TypeDecl.Query(tp) -> "(new Empty<" + t_name + ">()).ToList<" + t_name + ">()"
          | (t, q) :: qs, _ -> "(new Cons<" + t_name + ">(" + traverse query_symbols_context (t, q) false + "," + concat_queries qs + ")).ToList<" + t_name + ">()"
          | _ -> raise tp.Position "Unexpected case"
        concat_queries qs

      | OptimizedQueryAST.InnerQueryExpression.Let(id, e, (t, OptimizedQueryAST.Query(OptimizedQueryAST.QueryStatements(OptimizedQueryAST.InnerQueryExpression.For(a,b,c)::xs)))) -> 


        query_symbols_context.AddId(id)
        query_symbols_context.Padding <- query_symbols_context.Padding - 1
        let p = (t, OptimizedQueryAST.Query(OptimizedQueryAST.QueryStatements(OptimizedQueryAST.InnerQueryExpression.For(a,b,c)::xs)))
//        query_symbols_context.DecrPadding()
        let p = traverse query_symbols_context p true
//        query_symbols_context.IncrPadding()
        query_symbols_context.Padding <- query_symbols_context.Padding + 1
        let res = 
          ".Select(" + query_symbols_context.CurrentContextSymbol  + " => new {" + id.idText + " = " + p + ", " + 
                                                                                 "prev = " + query_symbols_context.CurrentContextSymbol  + " })" + eval_query qs  (Some query_symbols_context)
        res

         
      | OptimizedQueryAST.InnerQueryExpression.Let(id, e, p) -> 
        query_symbols_context.AddId(id)
//        query_symbols_context.DecrPadding()
        let p = traverse query_symbols_context p true
//        query_symbols_context.IncrPadding()
        ".Select(" + query_symbols_context.CurrentContextSymbol  + " => new {" + id.idText + " = " + p + ", " + 
                                                                               "prev = " + query_symbols_context.CurrentContextSymbol  + " })" + eval_query qs  (Some query_symbols_context)

      | OptimizedQueryAST.FindBy (e) -> 
        match query_symbols_context.LastForId with
        | Some _ ->
          ".First(" + query_symbols_context.CurrentContextSymbol + " => " + traverse query_symbols_context e false + ")" + 
          "." + query_symbols_context.LastForId.Value.idText + eval_query qs (Some query_symbols_context) 
        | _ -> raise (snd e).Position (sprintf "Internal error at %s(%s). " __SOURCE_FILE__ __LINE__)
      | OptimizedQueryAST.Exists (e) -> ".Contains(" + traverse_atomic_expr true e (false, None) [] "" true + ")" + eval_query qs (Some query_symbols_context) 
      | OptimizedQueryAST.MinBy (e) -> ".Min(" + query_symbols_context.CurrentContextSymbol + " => " + traverse query_symbols_context e false + ")" + eval_query qs (Some query_symbols_context) 
      | OptimizedQueryAST.MaxBy (e) -> ".Max(" + query_symbols_context.CurrentContextSymbol + " => " + traverse query_symbols_context e false + ")" + eval_query qs (Some query_symbols_context) 
      | OptimizedQueryAST.ForAll (e) -> ".All(" + query_symbols_context.CurrentContextSymbol + " => " + traverse query_symbols_context e false + ")" + eval_query qs (Some query_symbols_context) 
      | OptimizedQueryAST.Sum (_) -> ".Aggregate(default(" + t_name + "), (acc, __x) => acc + __x)"
      | OptimizedQueryAST.Min (_) -> ".Min()"
      | OptimizedQueryAST.Max (_) -> ".Max()"
        //".Sum()" + eval_query qs (Some query_symbols_context) 
      | OptimizedQueryAST.GroupBy (e) -> ".GroupBy(" + query_symbols_context.CurrentContextSymbol + " => " + traverse query_symbols_context e false + ")" + eval_query qs (Some query_symbols_context) 
      | _ -> failwith tp.Position "Not supported query operator."

  "\n" + eval_query qs query_symbols_context

and query_traverse_typed_atomic_block (block : OptimizedQueryAST.Block) (return_ids : Id list) (query_context : bool * Option<QueryContext>) (tabs : string) = 
  let last = block |> Seq.skip(block.Length - 1) |> Seq.head
  let lst_but_last = block |> Seq.take(block.Length - 1) |> Seq.toList
  query_traverse_atomic_block (lst_but_last, last) return_ids query_context tabs

and query_traverse_atomic_block (b : StateMachinesAST.AtomicBlock) (return_ids : Id list) query_context tabs = 
//  [B, E]_a
//  [B], [E]_a
  let block1 = [for e in fst b do yield tabs + (traverse_atomic_expr true e query_context [] tabs false)]
  let block = 
    match block1 with 
    | [] -> ""; 
    |[x] -> x + "\n" 
    | block -> 
      if block.Length > 0 then 
        Seq.fold(fun a b -> a + "\n" + b) block.Head block.Tail + "\n"
      else block.Head  + "\n"



  let return_expr = tabs + (traverse_atomic_expr true (b |> snd) query_context return_ids tabs true)
  block + 
  return_expr + "\n"

and traverse_typed_atomic_block (block : OptimizedQueryAST.Block) (return_ids : Id list) tabs = 
  let last = block |> Seq.skip(block.Length - 1) |> Seq.head
  let lst_but_last = block |> Seq.take(block.Length - 1) |> Seq.toList
  traverse_atomic_block (lst_but_last, last) return_ids tabs

and traverse_atomic_block (b : StateMachinesAST.AtomicBlock) (return_ids : Id list) tabs = 
//  [B, E]_a
//  [B], [E]_a
  let block1 = [for e in fst b do yield tabs + (traverse_atomic_expr true e (false, None ) [] tabs false)]
  let block = match block1 with | [] -> ""; |[x] -> x + "\n" | block -> (block |> Seq.reduce(fun a b -> a + "\n" + b)) + "\n"
  let return_expr = tabs + (traverse_atomic_expr true (b |> snd) (false, None) return_ids tabs false)
  "\n"
  + tabs + "{\n" + 
            block + 
            return_expr + "\n"
  + tabs + "}"

and traverse_create_block (is_world : bool)
                          (dp:IntermediateAST.DataDependencies) 
                          (classes : IntermediateAST.Class list)
                          (initialization_block : string) 
                          (b : StateMachinesAST.AtomicBlock) 
                          (class_fields : IntermediateAST.Field seq) 
                          (tabs : string) = 
//  [B, E]_a
//  [B], [E]_a
  let block1 = [for e in (fst b) @ [snd b] do yield tabs + (traverse_atomic_expr  true e (false, None) [] tabs false)]
  let block = match block1 with | [] -> ""; |[x] -> x + "\n" | block -> (block |> Seq.reduce(fun a b -> a + "\n" + b)) + "\n"
      
  initialization_block + 
  block + 
  (class_fields |> Seq.map(fun (f : IntermediateAST.Field) ->
    let c = 
      match f.Type with
      | StateMachinesAST.TypeDecl.EntityName c ->
        let c = classes |> Seq.find(fun c' -> c.Id = c'.Name)
        Some c
      | StateMachinesAST.TypeDecl.Query (StateMachinesAST.TypeDecl.EntityName c) ->
        let c = classes |> Seq.find(fun c' -> c.Id = c'.Name)
        Some c
      | _ -> None
    match c with
    | Some c when dp.IsSourceOrTarget c -> ""
    | _ -> "") |> Seq.reduce(+)) +
  (if Common.is_networked_game && is_world then 
    let (OptimizedQueryAST.NewEntity(assignments)) = snd (snd b)
    let print_infos entity_name entity_to_add =
      "NetworkAPI." + entity_name + "Infos.Add(" + entity_to_add + ".Net_ID, new NetworkInfo<" + entity_name + ">(" + entity_to_add + ", true));"
    assignments |> Seq.filter(fun expr -> (fst expr).idText <> "Connected")
                |> Seq.map(fun expr -> 
                              let id = (fst expr).idText
                              let tp = expr |> snd |> Seq.head |> fst
                              match tp with
                              | TypedAST.TypeDecl.EntityName(name) -> print_infos name.idText id
                              | TypedAST.TypeDecl.Query(TypedAST.TypeDecl.EntityName(name)) -> 
                                "foreach(var entity in " + id + ") {
                                    " + print_infos name.idText "entity" + ";
                                 }"
                              | _ -> 
                                printfn "WARNING: NETWORK SYMBOLS FOR %s NOT GENERATED." tp.TypeName 
                                "") |> Seq.fold (+) ""
   else "")
  

and traverse_atomic_expr 
    (use_ret : bool)
    (expr  : StateMachinesAST.AtomicTypedExpression)
    (query_symbols_context : bool * Option<QueryContext>)
    (return_ids : Id list) 
    (tabs : string)
    (is_arg_expr : bool) : string = 

  let semicolon = if is_arg_expr then "" else ";"
  let ret = 
    if use_ret then
      match return_ids with |[x] -> x.idText + " = "; | _ -> ""    
    else ""
  match expr with
  |(TypedAST.TypeDecl.MaybeType(TypedAST.Just(t))), OptimizedQueryAST.Maybe(OptimizedQueryAST.MaybeExpr.JustExpr(e)) -> 
    let e1 = e 
    let e = traverse_atomic_expr false e query_symbols_context return_ids tabs true
    ret + "(new Just<" + t.TypeName + ">(" + e + "))" + semicolon

  |(TypedAST.TypeDecl.MaybeType(TypedAST.Nothing(t, _))), OptimizedQueryAST.Maybe(OptimizedQueryAST.MaybeExpr.NothingExpr(p)) when t.Value.Type.IsNone -> 
    ret + "(new Nothing())" + semicolon

  |(TypedAST.TypeDecl.MaybeType(TypedAST.Nothing(t, _))), OptimizedQueryAST.Maybe(OptimizedQueryAST.MaybeExpr.NothingExpr(p)) -> 
    ret + "(new Nothing<" + t.Value.Type.Value.TypeName + ">())" + semicolon

  |t, OptimizedQueryAST.ConcatQuery(qs) -> 
    let t_name = 
      match t with
      | TypedAST.Query(t) -> t.TypeName
      | _ -> t.TypeName
    let rec concat_queries qs return_ids =
      match qs with
      | [q] -> traverse_atomic_expr use_ret q query_symbols_context return_ids tabs true 
      | [q1;q2] -> "(" + traverse_atomic_expr  use_ret q1 query_symbols_context [] tabs true + ").Concat(" + traverse_atomic_expr  use_ret q2 query_symbols_context [] tabs true + ")"
      |q1::q2::qs -> "(" + concat_queries [q1;q2] [] + ").Concat(" + concat_queries qs [] + ")"
    ret + concat_queries qs [] + ".ToList<" + t_name + ">()" + semicolon

  |t, OptimizedQueryAST.AppendToQuery(e, l) ->
    let t_name = 
      match t with
      | TypedAST.Query(t) -> t.TypeName
      | _ -> t.TypeName
    ret +
      "new Cons<" + t_name + ">(" + traverse_atomic_expr  use_ret e query_symbols_context [] tabs true + ", (" + 
                                    traverse_atomic_expr  use_ret l query_symbols_context [] tabs true + ")).ToList()" + semicolon
//  ----------------------------------------------
//  tp, [let b1,...,bn : T1 * ... * Tn = E]_a

//  T1 b1;
//  ...
//  Tn bn;
//  [E]_b1..bn
  |_, OptimizedQueryAST.Let(id, tp, Some e,_) ->
    tp.TypeName + " " + id.idText + ";\n" +
    tabs + traverse_atomic_expr  use_ret e query_symbols_context [id] tabs false
  |_, OptimizedQueryAST.Let(id, tp, None,_) ->
    tp.TypeName + " " + id.idText + ";\n"


  | _, OptimizedQueryAST.Modulus(e1, e2) -> ret + "(" + traverse_atomic_expr  use_ret e1 query_symbols_context [] tabs true + ") % (" + traverse_atomic_expr  use_ret e2 query_symbols_context [] tabs true + ")" + semicolon
  | _, OptimizedQueryAST.Add(e1, e2) -> ret + "(" + traverse_atomic_expr  use_ret e1 query_symbols_context [] tabs true + ") + (" + traverse_atomic_expr  use_ret e2 query_symbols_context [] tabs true + ")" + semicolon
  | _, OptimizedQueryAST.Sub(e1, e2) -> ret + "(" + traverse_atomic_expr  use_ret e1 query_symbols_context [] tabs true + ") - (" + traverse_atomic_expr  use_ret e2 query_symbols_context [] tabs true + ")" + semicolon
  | _, OptimizedQueryAST.Mul(e1, e2) -> ret + "(" + traverse_atomic_expr  use_ret e1 query_symbols_context [] tabs true + ") * (" + traverse_atomic_expr  use_ret e2 query_symbols_context [] tabs true + ")" + semicolon
  | _, OptimizedQueryAST.Div(e1, e2) -> ret + "(" + traverse_atomic_expr  use_ret e1 query_symbols_context [] tabs true + ") / (" + traverse_atomic_expr  use_ret e2 query_symbols_context [] tabs true + ")" + semicolon

  |_, OptimizedQueryAST.Not(b) -> ret + "!(" + traverse_atomic_expr  use_ret b query_symbols_context [] tabs true + ")" + semicolon
  |_, OptimizedQueryAST.And(b1, b2) -> ret + "((" + traverse_atomic_expr  use_ret b1 query_symbols_context [] tabs true + ") && (" + traverse_atomic_expr  use_ret b2 query_symbols_context [] tabs true + "))" + semicolon
  |_, OptimizedQueryAST.Or(b1, b2) -> ret + "((" + traverse_atomic_expr  use_ret b1 query_symbols_context [] tabs true + ") || (" + traverse_atomic_expr  use_ret b2 query_symbols_context [] tabs true + "))" + semicolon
  |_, OptimizedQueryAST.Equals(b1, b2) -> ret + "((" + traverse_atomic_expr  use_ret b1 query_symbols_context [] tabs true + ") == (" + traverse_atomic_expr  use_ret b2 query_symbols_context [] tabs true + "))" + semicolon
  |_, OptimizedQueryAST.Greater(e1, e2) -> ret + "((" + traverse_atomic_expr  use_ret e1 query_symbols_context [] tabs true + ") > (" + traverse_atomic_expr  use_ret e2 query_symbols_context [] tabs true + "))" + semicolon
//  ----------------------------------------------
//  tp, [if cond then A else B]_a
//  if([cond]) { [A]_a } else { [B]_a }
  |t, OptimizedQueryAST.IfThenElse(cond, _then, _else) ->
    if (snd query_symbols_context).IsSome then
      let cond = traverse_atomic_expr  use_ret cond query_symbols_context [] tabs true 
      let _else = query_traverse_typed_atomic_block _else [] query_symbols_context (tabs + "\t") 
      let _then = query_traverse_typed_atomic_block _then [] query_symbols_context (tabs + "\t")
      "Utils.IfThenElse<" + t.TypeName + ">((()=> " + cond + "), (()=>" + _then + "),(()=>" + _else + "))"
    else
      "if(" + traverse_atomic_expr  use_ret cond query_symbols_context [] tabs true + ")" + 
      traverse_typed_atomic_block  _then return_ids (tabs + "\t") +
      "else" + traverse_typed_atomic_block  _else return_ids (tabs + "\t") 
//  ----------------------------------------------
//  tp, [if cond then A]_a
//  if([cond]) { [A]_a }
  |_, OptimizedQueryAST.IfThen(cond, _then,_) ->
    "if(" + traverse_atomic_expr use_ret cond query_symbols_context [] tabs true + ")" + 
    traverse_typed_atomic_block  _then return_ids (tabs + "\t")
//  tp, [A1,...,A2]_a1..an
//  a1 <- [A1]
//  ...
//  an <- [An]
  |et, OptimizedQueryAST.Expression.Tuple(exprs) -> 
    
    if exprs.Length = 2 then
      match exprs with
      | (t1,e1) :: (t2,e2) :: [] ->
        "new " + et.TypeName + "("  + traverse_atomic_expr  use_ret (t1,e1) query_symbols_context [] tabs true + semicolon + "," +
                                            traverse_atomic_expr  use_ret (t2,e2) query_symbols_context [] tabs true + semicolon  + ")"
    else
      if return_ids.Length <> exprs.Length then
        raise (snd expr).Position (sprintf "Type error: unexpected number of return values.")
      let b = 
        [for id, e in Seq.zip return_ids exprs do
          yield tabs + id.idText + " = " + traverse_atomic_expr  use_ret e query_symbols_context [] tabs true + semicolon ] 

      if b.Length > 0 then 
        Seq.fold(fun a b -> a + "\n" + b) b.Head b.Tail
      else b.Head


//    //tabs + (traverse_atomic_expr (snd b) [] tabs)
//    match snd b with
//    //  tp, [(id1,e1) ... (idn,en)]_a
//    //  a <- new tp(){ id1 = [e1], idn = [en]; }
  |tp, OptimizedQueryAST.Expression.NewEntity(id_exprs) -> 
    [for id, e in id_exprs do
      let last = e |> Seq.skip(e.Length - 1) |> Seq.head
      let lst_but_last = e |> Seq.take(e.Length - 1) |> Seq.toList
      if lst_but_last <> [] then raise tp.Position "..."
      yield id.idText + " = " + traverse_atomic_expr  use_ret last (false, None) [] "" false] |> Seq.fold (fun s e -> e + "\n\t\t" + s) ""

  |t, OptimizedQueryAST.Cast(tp, e, p) -> 
    "((" + tp.TypeName + ")" + traverse_atomic_expr  use_ret e (false, None) [] "" true + ")" + semicolon 
        
//----------------------------------------------
//  tp, [(id1,e1) ... (idn,en)]_a
//  a <- new tp(){ id1 = [e1], idn = [en]; }
  |tp, OptimizedQueryAST.NewEntity(id_exprs) -> 
    let args =
      [for id, e in id_exprs do
        yield id.idText + " = " + traverse_typed_atomic_block e [] tabs]
    let args = 
      if args.Length > 0 then 
        Seq.fold(fun a b -> a + "," + b) args.Head args.Tail
      else ""
        
        
    ret  + "new " + tp.TypeName + "(){" + args + "}" + semicolon
//----------------------------------------------
//  tp, [id]_a
//  a <- id
  |_, OptimizedQueryAST.Id(id) ->
    match query_symbols_context with
    | _, None -> ret + (solve_id_head id).idText + semicolon
    | _, Some query_symbols_context -> ret + query_symbols_context.TrySolveId id + semicolon
//  ----------------------------------------------
//  tp, [literal]_a
//  a <- literal
  |_, OptimizedQueryAST.Literal(l) -> ret + l.Name + semicolon
//  ----------------------------------------------
////  tp, [aExpr]_a
////  a <- [aExpr]
//  |_, TypedAST.ArithmeticExpression(e) ->
//    let ret = match return_ids with |[x] -> x.idText + " = "; | _ -> ""
//    ret + traverse_arithmetic_expr e [] + semicolon
//  ----------------------------------------------
////  tp, [bExpr]_a
////  a <- bExpr
//  |_, TypedAST.BooleanExpression(e) ->
//    let ret = match return_ids with |[x] -> x.idText + " = "; | _ -> ""
//    ret + traverse_boolean_expr e []  + semicolon

//  ----------------------------------------------
//  tp, [call]_a
//  a <- call
  |_, OptimizedQueryAST.Call(OptimizedQueryAST.Constructor(tp, b)) when b = []->
    let res = ret + "new " + tp.TypeName + "()" + semicolon
    res
  |_, OptimizedQueryAST.Call(OptimizedQueryAST.Constructor(tp, b)) ->

    let args = 
      let args = [for e in b do yield traverse_atomic_expr use_ret e query_symbols_context [] tabs true] 
      if args.Length > 0 then 
        Seq.fold(fun a b -> a + "," + b) args.Head args.Tail
      else ""
    let res = ret + "new " + tp.TypeName + "(" + args + ")" + semicolon
    res
  |_, OptimizedQueryAST.Call(OptimizedQueryAST.Static(tp, id, b))  when  b = []->
    let res = ret + tp.TypeName + "." + id.idText + "()" + semicolon
    res
  |_, OptimizedQueryAST.Call(OptimizedQueryAST.Static(tp, id, b)) ->
    let args = 
      let args = [for e in b do yield traverse_atomic_expr  use_ret e query_symbols_context [] tabs true] 
      if args.Length > 0 then 
        Seq.fold(fun a b -> a + "," + b) args.Head args.Tail
      else ""
    let res = ret + tp.TypeName + "." + id.idText + "(" + args + ")" + semicolon
    res
  |t, OptimizedQueryAST.Call(OptimizedQueryAST.Instance(_this, m, b)) ->

    let args = 
      let args = [for e in b do yield traverse_atomic_expr  use_ret e query_symbols_context [] tabs true] 
      if args.Length > 0 then 
        Seq.fold(fun a b -> a + "," + b) args.Head args.Tail
      else ""
    
    match query_symbols_context with
    | _, None -> 
      let res = ret +  _this.idText + "." + m.idText + "(" + args + ")" + semicolon
      res
    | _, Some query_symbols_context ->
      let _this' = _this.idText.Split('.') |> Seq.toList
      let head = query_symbols_context.TrySolveId {idText = _this'.Head; idRange = _this.idRange}
      let _this' =
        if _this'.Tail = [] then head + "."
        else _this'.Tail |> Seq.fold(fun a b -> a + "." + b) head
      let res = ret +  _this' + m.idText + "(" + args + ")" + semicolon
      res

//  ----------------------------------------------
//  tp, [query]_a
//  a <- [query]

  |TypedAST.TypeDecl.Query(qt), OptimizedQueryAST.Range(f,t,p) -> 
    ret + "Enumerable.Range(" + traverse_atomic_expr  use_ret f query_symbols_context [] tabs true + "," + traverse_atomic_expr  use_ret t query_symbols_context [] tabs true + ").ToList<" + qt.TypeName + ">()" + semicolon

  |TypedAST.TypeDecl.Query(tp), OptimizedQueryAST.Expression.Query(q) ->
    ret + "(" + traverse_query_expr query_symbols_context (TypedAST.TypeDecl.Query(tp)) q [] (tabs + "\t") + ").ToList<" + tp.TypeName + ">()" + semicolon
  |tp, OptimizedQueryAST.Expression.Query(q) ->
    ret + "(" + traverse_query_expr query_symbols_context (TypedAST.TypeDecl.Query(tp)) q [] (tabs + "\t") + ")" + semicolon
//  ----------------------------------------------
//  tp, [while cond do expr]_a
//  while(cond)
//  { [expr] }
  |_, OptimizedQueryAST.While(cond, b) ->
    let tabs = tabs + "\t"

    let b = 
      [for e in b do
        yield tabs + traverse_atomic_expr  use_ret e query_symbols_context [] tabs false] 

    let b = 
      if b.Length > 0 then 
        Seq.fold(fun a b -> a + "\n" + b) b.Head b.Tail
      else b.Head


    "while(" + traverse_atomic_expr  use_ret cond query_symbols_context [] "" false + "){" + b + "}" 
//  ----------------------------------------------
//  tp, [for x1,..,xn in E do B]_a


//  for(int __index = 0; __index < [E].length; __index++)
//  { [let x1,..,xn = E.[__index]]_x1,..,xn
//    [B] }
  |_, OptimizedQueryAST.For(ids, e, b, _) ->
    let args, ids =
      match ids with
      | [x] -> (StateMachinesAST.TypeDecl.Unit (fst expr).Position ,OptimizedQueryAST.Let(x, fst e, Some e,false)), [x]
      | _ ->  failwith (e |> snd).Position "Multilple args in for expression not supported, yet"

    let tabs = tabs + "\t"
    "for(int __index = 0; __index < (" + traverse_atomic_expr  use_ret e query_symbols_context [] tabs true + ").Count; __index++){\n" + 
    tabs + traverse_atomic_expr  use_ret args query_symbols_context (ids |> List.map(fun id -> id)) tabs false + "\n" + 
    ([for e in b do
      yield tabs + traverse_atomic_expr  use_ret e query_symbols_context [] tabs false] |> Seq.reduce (fun a b -> a + "\n" + b)) +
    "}"
  //  tp, [IndexOf(id, e)]_state
//  id[ [e]_state ];
  | tp, OptimizedQueryAST.IndexOf(id,e) ->
    
    ret + traverse_atomic_expr  use_ret id query_symbols_context [] tabs true + "[" + traverse_atomic_expr  use_ret e query_symbols_context [] tabs true + "]" + semicolon

  | tp, OptimizedQueryAST.Set(id,e) ->
    
    ret + id.idText + " = " + traverse_atomic_expr  use_ret e query_symbols_context [] tabs true + semicolon

  | tp, OptimizedQueryAST.DoGet(e1,e2) ->
    
    ret + traverse_atomic_expr  use_ret e1 query_symbols_context [] tabs true + "." + e2.idText + semicolon

  | e -> failwith (e |> snd).Position "error pretty printer"

let rec traverse_state_machine_block domain_and_container (block : IntermediateAST.Block) state is_suspendable tabs (first_goto_suspend : bool ref) (is_arg_expr : bool) : string = 
//  [B, E]_a
//  [B], [E]_a
  let block = [for e in block do yield tabs + (traverse_state_machine_expr domain_and_container CasanovaCompiler.ParseAST.Flag.Nothing e (false, None) tabs state is_suspendable first_goto_suspend is_arg_expr)] |> List.fold(fun s e -> s + "\n" + e) ""
  "\n" + tabs + "{\n" + block + tabs + "}"


and traverse_state_machine_block' (domain_and_container : Option<List<Id> * IntermediateAST.Class>) flag (block : IntermediateAST.Block) state is_suspendable tabs (first_goto_suspend : bool ref) (is_arg_expr : bool) : string = 
//  [B, E]_a
//  [B], [E]_a
  let block = [for e in block do yield tabs + (traverse_state_machine_expr domain_and_container flag e (false, None) tabs state is_suspendable first_goto_suspend is_arg_expr)] |> List.fold(fun s e -> s + "\n" + e) ""
  "\n" + tabs + "{\n" + block + tabs + "\n" + tabs + "default: return" + (if is_suspendable then " RuleResult.Done" else "") + ";}"

and traverse_state_machine_expr domain_and_container
                                flag
                                (expr  : IntermediateAST.TypedExpression) 
                                (query_symbols_context : bool * Option<QueryContext>) 
                                (tabs : string) 
                                (state : string) 
                                is_suspendable 
                                (first_goto_suspend : bool ref) 
                                (is_arg_expr : bool) = 
  let semicolon = if is_arg_expr then "" else ";"
  match expr with

//  tp, [Label(i, p)]_state
//  case i:
  | tp, IntermediateAST.Label(i,p) ->
    "case " + string i + ":"
    
//  tp, [GotoSuspend(i, p)]_state
//  state = i;
//  break;
  | tp, IntermediateAST.GotoSuspend(i,p) when !first_goto_suspend && is_suspendable ->
    first_goto_suspend := false
    state + " = " + string i + ";\n" +
    "return RuleResult.Done;" 
  | tp, IntermediateAST.GotoSuspend(i,p) when is_suspendable ->
    state + " = " + string i + ";\n" +
    "return RuleResult.Working;" 
  | tp, IntermediateAST.GotoSuspend(i,p) ->
    state + " = " + string i + ";\n" +
    "return;" 

//  tp, [Goto(i,p)]_state
//  state = i; ??
//  goto case i;
  | tp, IntermediateAST.Goto(i,p) ->
    "goto case " + string i + ";"


  | tp, IntermediateAST.DoGet(e, id) -> "(" + traverse_state_machine_expr domain_and_container flag e query_symbols_context tabs state is_suspendable first_goto_suspend true  + ")" + "." +  id.idText

//  tp, [IndexOf(id, e)]_state
//  id[ [e]_state ];
  | tp, IntermediateAST.IndexOf(id,e) ->
    
    "(" + (traverse_state_machine_expr domain_and_container flag id query_symbols_context tabs state is_suspendable first_goto_suspend true ) + ")[" + traverse_state_machine_expr domain_and_container flag e query_symbols_context tabs state is_suspendable first_goto_suspend true + "]"

//  tp, [new [(id1,e1); ... ;(idn,en]]_state
//  new tp(){ id1 = [e1], idn = [en]; }
  | tp, IntermediateAST.NewEntity(id_block_list) ->
    match id_block_list with
    | [] -> "new " + tp.TypeName + "()"
    | _ -> "new " + tp.TypeName + "(){" + (List.fold(fun s (id, b) -> "," + id.idText + " = " + traverse_state_machine_block domain_and_container b state is_suspendable tabs first_goto_suspend true + s) 
                                          ((fst id_block_list.Head).idText + " = " + traverse_state_machine_block domain_and_container (snd id_block_list.Head) state is_suspendable tabs first_goto_suspend true) id_block_list.Tail) + "}"


//  tp, [IfThenElse(cond, A, B)]_state
//  if([cond]) { [A]_state } else { [B]_state }
  | tp, IntermediateAST.IfThenElse(cond, expr_to_exhange, _then, _else) ->
      
      "if(" + traverse_state_machine_expr domain_and_container flag (if expr_to_exhange.IsSome then expr_to_exhange.Value else cond) query_symbols_context "" "" false (ref false) true + ")" + 
      //(tabs : string) (state : string) is_suspendable (first_goto_suspend : bool ref) (is_arg_expr : bool
      traverse_state_machine_block domain_and_container _then state is_suspendable tabs first_goto_suspend false +
      "else" + traverse_state_machine_block domain_and_container _else state is_suspendable tabs first_goto_suspend false 
//  tp, [IfThen(cond, A)]_state
//  if([cond]) { [A]_state }
  | tp, IntermediateAST.IfThen(cond, _then) ->
    "if(" + traverse_state_machine_expr domain_and_container flag cond query_symbols_context "" "" false (ref false) true + ")" + 
    traverse_state_machine_block domain_and_container _then state is_suspendable tabs first_goto_suspend false
        
  |t, IntermediateAST.Cast(tp, e, p) -> 
    "((" + tp.TypeName + ")" + traverse_state_machine_expr domain_and_container flag e query_symbols_context "" "" false (ref false) true + ")" + semicolon 

//  tp, [literal]_statee
//  literal
  |_, IntermediateAST.Literal(l) -> 
    l.Name

//  tp, [call]_state
//  call
  |_, IntermediateAST.Call(StateMachinesAST.Constructor(tp, b)) ->
    
    let args = 
      let args = [for e in b do yield traverse_atomic_expr true  e query_symbols_context [] tabs true]
      if args.Length > 0 then 
        Seq.fold(fun a b -> a + "," + b) args.Head args.Tail
      else ""

    let res = "new " + tp.TypeName + "(" + args + ")" + semicolon
    res
  |_, IntermediateAST.Call(StateMachinesAST.ConstructorNoArgs(tp)) ->
    let res ="new " + tp.TypeName + "()" + semicolon
    res
  |_, IntermediateAST.Call(StateMachinesAST.Static(tp, id, b)) ->

    let args = 
      let args = [for e in b do yield traverse_atomic_expr true e query_symbols_context [] tabs true] 
      if args.Length > 0 then 
        Seq.fold(fun a b -> a + "," + b) args.Head args.Tail
      else ""


    let res = tp.TypeName + "." + id.idText + "(" + args + ")" + semicolon
    res
  |_, IntermediateAST.Call(StateMachinesAST.StaticNoArgs(tp, id)) ->
    let res = tp.TypeName + "." + id.idText + "()" + semicolon
    res
  |_, IntermediateAST.Call(StateMachinesAST.Instance(_this, m, b)) ->   

    let args = 
      let args = [for e in b do yield traverse_atomic_expr true e query_symbols_context [] tabs true] 
      if args.Length > 0 then 
        Seq.fold(fun a b -> a + "," + b) args.Head args.Tail
      else ""

    match query_symbols_context with
    | _, None -> 
      let res = _this.idText + "." + m.idText + "(" + args + ")" + semicolon
      res
    | _, Some query_symbols_context ->
      let _this' = _this.idText.Split('.') |> Seq.toList
      let head = query_symbols_context.TrySolveId {idText = _this'.Head; idRange = _this.idRange}
      let _this' =
        if _this'.Tail = [] then head + "."
        else _this'.Tail |> Seq.fold(fun a b -> a + "." + b) head
      let res = _this' + m.idText + "(" + args + ")" + semicolon
      res

//  tp, [A1,...,An]_state
//  [A1]_state, ..., [An]_state
  |tp, IntermediateAST.Tuple(b) ->
  
     raise tp.Position "Tuple not supported inside a rule state machine yet"

//  tp, [id]_state
  |_, IntermediateAST.Id(id) -> 
    match query_symbols_context with
    |_, None -> (solve_id_head id).idText + semicolon
    |_, Some query_symbols_context -> query_symbols_context.TrySolveId id + semicolon
  
//  tp, [query]_state
//  query
//  |_, IntermediateAST.Range(f,t,p) -> "Enumerable.Range(" + string f + "," + string t + ")" + semicolon
  
  |tp, IntermediateAST.Query(q) ->
    let tp1, cast_string = 
      match tp with
      | TypedAST.Query(t) -> t.TypeName, ".ToList<" + t.TypeName + ">()" 
      | _ -> tp.TypeName, ""

    "(" + traverse_query_expr query_symbols_context tp q [] tabs  + ")" + cast_string 

  | _, IntermediateAST.Modulus(e1, e2) -> "((" + traverse_state_machine_expr domain_and_container flag e1 query_symbols_context tabs state is_suspendable first_goto_suspend true + ") % (" + traverse_state_machine_expr domain_and_container flag e2 query_symbols_context tabs state is_suspendable first_goto_suspend true + "))"
  | _, IntermediateAST.Add(e1, e2) -> "((" + traverse_state_machine_expr domain_and_container flag e1 query_symbols_context tabs state is_suspendable first_goto_suspend true + ") + (" + traverse_state_machine_expr domain_and_container flag e2 query_symbols_context tabs state is_suspendable first_goto_suspend true + "))"
  | _, IntermediateAST.Sub(e1, e2) -> "((" + traverse_state_machine_expr domain_and_container flag e1 query_symbols_context tabs state is_suspendable first_goto_suspend true + ") - (" + traverse_state_machine_expr domain_and_container flag e2 query_symbols_context tabs state is_suspendable first_goto_suspend true + "))"
  | _, IntermediateAST.Mul(e1, e2) -> "((" + traverse_state_machine_expr domain_and_container flag e1 query_symbols_context tabs state is_suspendable first_goto_suspend true + ") * (" + traverse_state_machine_expr domain_and_container flag e2 query_symbols_context tabs state is_suspendable first_goto_suspend true + "))"
  | _, IntermediateAST.Div(e1, e2) -> "((" + traverse_state_machine_expr domain_and_container flag e1 query_symbols_context tabs state is_suspendable first_goto_suspend true + ") / (" + traverse_state_machine_expr domain_and_container flag e2 query_symbols_context tabs state is_suspendable first_goto_suspend true + "))"
  |_, IntermediateAST.Not(b) -> "!(" + traverse_state_machine_expr domain_and_container flag b query_symbols_context tabs state is_suspendable first_goto_suspend true + ")"
  |_, IntermediateAST.And(b1, b2) -> "((" + traverse_state_machine_expr domain_and_container flag b1 query_symbols_context tabs state is_suspendable first_goto_suspend true + ") && (" + traverse_state_machine_expr domain_and_container flag b2 query_symbols_context tabs state is_suspendable first_goto_suspend true + "))"
  |_, IntermediateAST.Or(b1, b2) -> "((" + traverse_state_machine_expr domain_and_container flag b1 query_symbols_context tabs state is_suspendable first_goto_suspend true + ") || (" + traverse_state_machine_expr domain_and_container flag b2 query_symbols_context tabs state is_suspendable first_goto_suspend true + "))"
  |_, IntermediateAST.Equals(b1, b2) -> "((" + traverse_state_machine_expr domain_and_container flag b1 query_symbols_context tabs state is_suspendable first_goto_suspend true + ") == (" + traverse_state_machine_expr domain_and_container flag b2 query_symbols_context tabs state is_suspendable first_goto_suspend true + "))"
  |_, IntermediateAST.Greater(e1, e2) -> "((" + traverse_state_machine_expr domain_and_container flag e1 query_symbols_context tabs state is_suspendable first_goto_suspend true + ") > (" + traverse_state_machine_expr domain_and_container flag e2 query_symbols_context tabs state is_suspendable first_goto_suspend true + "))"


  | _, IntermediateAST.DoGet(e, id) -> "(" + traverse_state_machine_expr domain_and_container flag e query_symbols_context tabs state is_suspendable first_goto_suspend true + ")" + "." +  id.idText

  |_, IntermediateAST.IndexOf(lst,e) -> "(" + traverse_state_machine_expr domain_and_container flag lst query_symbols_context tabs state is_suspendable first_goto_suspend true + ")[" + traverse_state_machine_expr domain_and_container flag e query_symbols_context tabs state is_suspendable first_goto_suspend true + "]" + semicolon
  |TypedAST.TypeDecl.Query(qt), IntermediateAST.Range(f,t,p) -> "Enumerable.Range(" + traverse_state_machine_expr domain_and_container flag f query_symbols_context tabs state is_suspendable first_goto_suspend true + "," + traverse_state_machine_expr domain_and_container flag t query_symbols_context tabs state is_suspendable first_goto_suspend true + ").ToList<" + qt.TypeName + ">()"

  
  |(TypedAST.TypeDecl.MaybeType(TypedAST.Just(t))), IntermediateAST.Maybe(IntermediateAST.MaybeExpr.JustExpr(e)) -> 
    let e = traverse_state_machine_expr domain_and_container flag e query_symbols_context tabs state is_suspendable first_goto_suspend true
    "(new Just<" + t.TypeName + ">(" + e + "))"

  |(TypedAST.TypeDecl.MaybeType(TypedAST.Nothing(t, _))), IntermediateAST.Maybe(IntermediateAST.MaybeExpr.NothingExpr(p)) -> 
    match t.Value.Type with
    | None -> raise (snd expr).Position (sprintf "Internal error at %s(%s). " __SOURCE_FILE__ __LINE__)
    | Some _ ->
      "(new Nothing<" + t.Value.Type.Value.TypeName + ">())"


  
  |t, IntermediateAST.ConcatQuery(qs) -> 
    let t_name = 
      match t with
      | TypedAST.Query(t) -> t.TypeName
      | _ -> t.TypeName
    let rec concat_queries qs =
      match qs with
      | [q] -> traverse_state_machine_expr domain_and_container flag q query_symbols_context tabs state is_suspendable first_goto_suspend true 
      | [q1;q2] -> "(" + traverse_state_machine_expr domain_and_container flag q1 query_symbols_context tabs state is_suspendable first_goto_suspend true + ").Concat(" + traverse_state_machine_expr domain_and_container flag q2 query_symbols_context tabs state is_suspendable first_goto_suspend true + ")"
      |q1::q2::qs -> "(" + concat_queries [q1;q2] + ").Concat(" + concat_queries qs + ")"
    concat_queries qs + ".ToList<" + t_name + ">()"

  |t, IntermediateAST.AtomicFor(id, cond, inc, body) -> 
    
    let cond = (traverse_state_machine_expr domain_and_container flag cond query_symbols_context tabs state is_suspendable first_goto_suspend true)
    let inc = (traverse_state_machine_expr domain_and_container flag inc query_symbols_context tabs state is_suspendable first_goto_suspend true)
    "for(int " + id.idText + " = 0; " + cond + "; " + inc + "){\n" +
    traverse_state_machine_block domain_and_container body state is_suspendable tabs first_goto_suspend false +
    "\n}\n"



  |t, IntermediateAST.SetExpression(e1, e2) -> 
    (traverse_state_machine_expr domain_and_container flag e1 query_symbols_context tabs state is_suspendable first_goto_suspend true) + " = " + 
    (traverse_state_machine_expr domain_and_container flag e2 query_symbols_context tabs state is_suspendable first_goto_suspend true) + semicolon

  |t, IntermediateAST.Incr(e) -> 
    (traverse_state_machine_expr domain_and_container flag e query_symbols_context tabs state is_suspendable first_goto_suspend true) + "++" + semicolon


  |t, IntermediateAST.AppendToQuery(e, l) ->


    let t_name = 
      match t with
      | TypedAST.Query(t) -> t.TypeName
      | _ -> t.TypeName
    "new Cons<" + t_name + ">(" + traverse_state_machine_expr domain_and_container flag e query_symbols_context tabs state is_suspendable first_goto_suspend true + ", (" + 
                                        traverse_state_machine_expr domain_and_container flag l query_symbols_context tabs state is_suspendable first_goto_suspend true + ")).ToList<" + t_name + ">()"
  | _, IntermediateAST.DoNothing(_) -> ""

  | _, IntermediateAST.ReceiveMany(t, p) -> 
    let target_field = domain_and_container.Value |> fst |> Seq.filter(fun v -> v.idText <> "Connected") |> Seq.head
    let target_field_idx = fst (snd(domain_and_container.Value).Body.Fields |> Seq.mapi(fun i f -> (i,f)) |> Seq.find(fun (i,f) -> f.Name.idText = target_field.idText))
    "NetworkAPI.Receive" + snd(domain_and_container.Value).Name.idText + target_field.idText + "Message(this.Net_ID);"

  | tp, IntermediateAST.Set(id,(_, IntermediateAST.Receive(t, p))) ->
    match flag with
    | CasanovaCompiler.ParseAST.Flag.Slave ->
        let target_field = domain_and_container.Value |> fst |> Seq.filter(fun v -> v.idText <> "Connected") |> Seq.head
        "NetworkAPI.Update" + (snd(domain_and_container.Value)).Name.idText +  target_field.idText + "Message(this.Net_ID);"
    | _ -> raise tp.Position (sprintf "Not supported networked operator %A" expr)
  | _, IntermediateAST.Send(t, e, p) -> 
    let container = (snd(domain_and_container.Value)).Name.idText
    let target_field = domain_and_container.Value |> fst |> Seq.head
    let target_field_idx = fst (snd(domain_and_container.Value).Body.Fields |> Seq.mapi(fun i f -> (i,f)) |> Seq.find(fun (i,f) -> f.Name.idText = target_field.idText))
    "Lidgren.Network.NetOutgoingMessage message = NetworkAPI.Update" + container + target_field.idText + "Message(this, NetworkAPI.Client, Net_ID, " + string(target_field_idx) + ");
NetworkAPI.Client.SendMessage(message, Lidgren.Network.NetDeliveryMethod.UnreliableSequenced);"
//UpdateShipPositionMessage
  | _, IntermediateAST.SendReliable(t, e, p) -> 
    match flag with
    | CasanovaCompiler.ParseAST.Flag.Connected ->
      match t with
      | StateMachinesAST.TypeDecl.Query(TypedAST.TypeDecl.EntityName(name)) ->
        let target_field = domain_and_container.Value |> fst |> Seq.filter(fun v -> v.idText <> "Connected") |> Seq.head
        let target_field_idx = fst (snd(domain_and_container.Value).Body.Fields |> Seq.mapi(fun i f -> (i,f)) |> Seq.find(fun (i,f) -> f.Name.idText = target_field.idText))
          
        "foreach (var entity in "+ traverse_state_machine_expr domain_and_container flag e query_symbols_context tabs state is_suspendable first_goto_suspend true + ")
        {
          if (NetworkAPI." + name.idText + "Infos.ContainsKey(entity.Net_ID) && NetworkAPI." + name.idText + "Infos[entity.Net_ID].IsLocal)
          {
            Lidgren.Network.NetOutgoingMessage entityMessage = NetworkAPI.Create" + snd(domain_and_container.Value).Name.idText + target_field.idText + "Message(entity, NetworkAPI.Client, this.Net_ID, " + string(target_field_idx) + ");
            NetworkAPI.Client.SendMessage(entityMessage, Lidgren.Network.NetDeliveryMethod.ReliableOrdered);
            NetworkAPI.ReceivedMessages.Remove(new Tuple<NetworkAPI.MessageType, NetworkAPI.EntityType, int>(NetworkAPI.MessageType.NewConnection, 0, 0));
          }
        }"
      | _ -> raise t.Position (sprintf "Not supported networked operator %A" expr)
    | CasanovaCompiler.ParseAST.Flag.Connecting ->
      match t with
      | StateMachinesAST.TypeDecl.Query(TypedAST.TypeDecl.EntityName(name)) ->
          let target_field = domain_and_container.Value |> fst |> Seq.filter(fun v -> v.idText <> "Connected") |> Seq.head
          let target_field_idx = fst (snd(domain_and_container.Value).Body.Fields |> Seq.mapi(fun i f -> (i,f)) |> Seq.find(fun (i,f) -> f.Name.idText = target_field.idText))
          "foreach (var entity in " + traverse_state_machine_expr domain_and_container flag e query_symbols_context tabs state is_suspendable first_goto_suspend true + ")
                  {
                    if (NetworkAPI." + name.idText + "Infos.ContainsKey(entity.Net_ID) && NetworkAPI." + name.idText + "Infos[entity.Net_ID].IsLocal)
                    {
                      Lidgren.Network.NetOutgoingMessage entityMessage = NetworkAPI.Create" + snd(domain_and_container.Value).Name.idText + target_field.idText + "Message(entity, NetworkAPI.Client, this.Net_ID, " + string(target_field_idx) + ");
                      NetworkAPI.Client.SendMessage(entityMessage, Lidgren.Network.NetDeliveryMethod.ReliableOrdered);
                    }
                  }"
      | _ -> raise t.Position (sprintf "Not supported networked operator %A" expr)
    | _ -> raise t.Position (sprintf "Not supported networked operator %A" expr)


//  tp, [Set(id, e)]_state
//  id = [e]_state
  | tp, IntermediateAST.Set(id,e) ->
    id.idText + " = " + traverse_state_machine_expr domain_and_container flag e query_symbols_context tabs state is_suspendable first_goto_suspend true + ";"

  | t, e -> failwith t.Position (sprintf "Error pretty printer. Cannot generate symbols for: %A" e)

