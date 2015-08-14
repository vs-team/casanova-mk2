module GotoContext
open System.Collections.Generic
open StateMachinesAST
open Common

type Environment() =
  let mutable classes : Map<Common.Id, (System.Collections.Generic.Dictionary<Common.Id, TypedAST.Field>)> = Map.empty
  let mutable current_entity : (Id * OptimizedQueryAST.EntityBody) option = None
  let mutable countdown_counter = 0
  let mutable label_counter = 0
  let mutable parallel_nesting = 0
  let mutable concurrent_nesting = 0
  let (parallel_aux_methods : ResizeArray<RuleBody * string>) = ResizeArray()
  let (concurrent_aux_methods : ResizeArray<RuleBody * string * string * string>) = ResizeArray()

  
  member this.Classes
    with get() = classes
    and set(value) = classes <- value

  member this.CurrentEntity
    with get() = current_entity.Value
    and set(value) = current_entity <- Some value

  member this.LabelCounter
    with get() = label_counter
    and set(value) = label_counter <- value

  member this.GetAndResetLabelCounter() =
    let old_counter = label_counter
    label_counter <- 0
    old_counter

  member this.CountdownCounter
    with get() = countdown_counter
    and set(value) = countdown_counter <- value

  member this.ParallelNesting
    with get() = parallel_nesting
    and set(value) = parallel_nesting <- value

  member this.ConcurrentNesting
    with get() = concurrent_nesting
    and set(value) = concurrent_nesting <- value

  member this.ParallelAuxMethods
    with get() = parallel_aux_methods

  member this.ConcurrentAuxMethods
    with get() = concurrent_aux_methods

let get_fresh_label : Position -> Environment -> Label * StateMachinesAST.TypedExpression = 
  fun p e -> 
    let label_id = Label.Create(e.LabelCounter,p)
    e.LabelCounter <- e.LabelCounter + 1    
    label_id, (TypeDecl.Unit p, Label(label_id))


let rec reduceGoto (block : Block) : Block =
  match block with
  | [] -> []
  | (_, Expression.Label(label)) :: exprs when label.RefCounter = 0 -> reduceGoto block.Tail
  | (tp, Expression.IfThen(cond, block)) :: xs -> 
    (tp, Expression.IfThen(cond, reduceGoto block)) :: reduceGoto xs
  | (tp, Expression.IfThenElse(cond, expr_to_exchange, block1, block2)) :: xs -> 
    (tp, Expression.IfThenElse(cond, expr_to_exchange, reduceGoto block1, reduceGoto block2)) :: reduceGoto xs
  | (_, Expression.Goto(id)) :: (_, Expression.Label(label)) :: exprs when id.Label.Id = label.Id && label.RefCounter = 1 -> 
    id.Label.RemoveRef()
    reduceGoto block.Tail
  | _ -> 
    if block.Length = 0 then raise Position.Empty (sprintf "Internal error at %s(%s)." __SOURCE_FILE__ __LINE__) |> ignore
    
    block.Head :: reduceGoto block.Tail

and reduceLabel (block : Block) : Block =  
  match block with
  | [] -> []
  | (_, Expression.Label(label)) :: exprs when label.RefCounter = 0 -> reduceLabel block.Tail
  | (tp, Expression.IfThen(cond, block)) :: xs -> 
    (tp, Expression.IfThen(cond, reduceLabel block)) :: reduceLabel xs
  | (tp, Expression.IfThenElse(cond, expr_to_exchange, block1, block2)) :: xs -> 
    (tp, Expression.IfThenElse(cond, expr_to_exchange, reduceLabel block1, reduceLabel block2)) :: reduceLabel xs
  | _ -> 
    if block.Length = 0 then raise Position.Empty (sprintf "Internal error at %s(%s)." __SOURCE_FILE__ __LINE__) |> ignore    
    block.Head :: reduceLabel block.Tail

and compress_and_build_mask (current_label : Option<int>) (label_dicitonary : Dictionary<int, int>) (block : Block) : Block =  
  match block with
  | [] -> []
  | (_, Expression.Label(label)) :: exprs ->
    match current_label with
    | None -> 
      label_dicitonary.Add(label.Id, label.Id)
      if block.Length = 0 then raise Position.Empty (sprintf "Internal error at %s(%s)." __SOURCE_FILE__ __LINE__) |> ignore       
      block.Head :: compress_and_build_mask (Some(label.Id)) label_dicitonary exprs
    | Some l ->
      label_dicitonary.Add(label.Id, l)
      compress_and_build_mask current_label label_dicitonary exprs
  | (tp, Expression.IfThen(cond, block)) :: xs -> 
    (tp, Expression.IfThen(cond, compress_and_build_mask None label_dicitonary block)) :: compress_and_build_mask None label_dicitonary xs
  | (tp, Expression.IfThenElse(cond, expr_to_exchange, block1, block2)) :: xs -> 
    (tp, Expression.IfThenElse(cond, expr_to_exchange, compress_and_build_mask None label_dicitonary block1, compress_and_build_mask None label_dicitonary block2)) :: compress_and_build_mask None label_dicitonary xs
  | _ -> 
    if block.Length = 0 then raise Position.Empty (sprintf "Internal error at %s(%s)." __SOURCE_FILE__ __LINE__) |> ignore
    block.Head :: compress_and_build_mask None label_dicitonary block.Tail

and update_with_mask (label_dicitonary : Dictionary<int, int>) (block : Block) : Block =  
  match block with
  | [] -> []
  | (t, Expression.Goto(label)) :: exprs ->
    (t, Expression.Goto(Goto.Create(Label.Create(label_dicitonary.[label.Label.Id], label.Label.Position)))) :: update_with_mask label_dicitonary exprs
  | (t, Expression.GotoSuspend(label)) :: exprs ->
    (t, Expression.GotoSuspend(GotoSuspend.Create(Label.Create(label_dicitonary.[label.Label.Id], label.Label.Position)))) :: update_with_mask label_dicitonary exprs
  | (tp, Expression.IfThen(cond, block)) :: xs -> 
    (tp, Expression.IfThen(cond, update_with_mask label_dicitonary block)) :: update_with_mask label_dicitonary xs
  | (tp, Expression.IfThenElse(cond, expr_to_exchange, block1, block2)) :: xs -> 
    (tp, Expression.IfThenElse(cond, expr_to_exchange, update_with_mask label_dicitonary block1, update_with_mask label_dicitonary block2)) :: update_with_mask label_dicitonary xs
  | _ -> 
    if block.Length = 0 then raise Position.Empty (sprintf "Internal error at %s(%s)." __SOURCE_FILE__ __LINE__) |> ignore
    block.Head :: update_with_mask label_dicitonary block.Tail


and shift_labels_and_gotos_by_one_but_minus_one (block : Block) : Block =  
  match block with
  | [] -> []
  | (t, Expression.Goto(label)) :: exprs when label.Label.Id > -1 ->
    (t, Expression.Goto(Goto.Create(Label.Create(label.Label.Id + 1, label.Label.Position)))) :: shift_labels_and_gotos_by_one_but_minus_one exprs
  | (t, Expression.GotoSuspend(label)) :: exprs when label.Label.Id > -1 ->
    (t, Expression.GotoSuspend(GotoSuspend.Create(Label.Create(label.Label.Id + 1, label.Label.Position)))) :: shift_labels_and_gotos_by_one_but_minus_one exprs
  | (t, Expression.Label(label)) :: exprs when label.Id > -1 -> (t, Expression.Label(Label.Create(label.Id + 1, label.Position))) ::  shift_labels_and_gotos_by_one_but_minus_one  exprs
  | (tp, Expression.IfThen(cond, block)) :: xs -> 
    (tp, Expression.IfThen(cond, shift_labels_and_gotos_by_one_but_minus_one block)) :: shift_labels_and_gotos_by_one_but_minus_one xs
  | (tp, Expression.IfThenElse(cond, expr_to_exchange, block1, block2)) :: xs -> 
    (tp, Expression.IfThenElse(cond, expr_to_exchange, shift_labels_and_gotos_by_one_but_minus_one block1, shift_labels_and_gotos_by_one_but_minus_one block2)) :: shift_labels_and_gotos_by_one_but_minus_one xs
  | _ -> 
    if block.Length = 0 then raise Position.Empty (sprintf "Internal error at %s(%s)." __SOURCE_FILE__ __LINE__) |> ignore
    block.Head :: shift_labels_and_gotos_by_one_but_minus_one block.Tail


let compress_block (block : Block) exit = 
  let labels_dict : Dictionary<int, ResizeArray<int>> = Dictionary<int, ResizeArray<int>>()
  let expr_with_goto_compression = reduceGoto block
  if block.Length = 0 then raise Position.Empty (sprintf "Internal error at %s(%s)." __SOURCE_FILE__ __LINE__) |> ignore
  let expr_with_label_compression = reduceLabel ((TypeDecl.Unit (fst block.Head).Position, Label(exit)) :: expr_with_goto_compression)
  
  let labels_mask = Dictionary<int,int>()
  let expr1 = compress_and_build_mask None labels_mask expr_with_label_compression
  let expr2 = update_with_mask labels_mask expr1
  
  expr2

let compress_block_no_exit (block : Block) = 
  let labels_dict : Dictionary<int, ResizeArray<int>> = Dictionary<int, ResizeArray<int>>()
  let expr_with_goto_compression = reduceGoto block
  let expr_with_label_compression = reduceLabel expr_with_goto_compression
  
  let labels_mask = Dictionary<int,int>()
  let expr1 = compress_and_build_mask None labels_mask expr_with_label_compression
  let expr2 = update_with_mask labels_mask expr1
  
  expr2


let reset_context (e : Environment) = e.LabelCounter <- 0