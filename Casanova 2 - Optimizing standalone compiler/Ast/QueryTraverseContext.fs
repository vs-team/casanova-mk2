module QueryTraverseContext
open Common



let mutable private _currentDepth = -1
type DepthQueryTree = Empty of int | Height of int

let solve_id_head id =
  let ids = id.idText.Split('.') |> Seq.toList
  let id_head = ids.Head
  let id_tail = ids.Tail

  {idText = id_tail |> Seq.map(fun s -> if s = "Head" then "Head()" else
                                        if s = "Tail" then "Tail()" else s) 
                    |> Seq.fold(fun s e -> s + "." + e) id_head; 
          idRange = id.idRange}

type QueryContext = { PrevContext         : QueryContext option
                      CurrentContextDepth : DepthQueryTree
                      mutable LastForId   : Option<Id>
                      mutable Padding     : int
                      mutable IdClosure   : Map<Id, int> }
  with 
    member this.IncrPadding() = this.Padding <- this.Padding + 1 
    member this.DecrPadding() = this.Padding <- this.Padding - 1 

    static member GetFreshContext(prev_context : QueryContext option, padding) = 
      let prev_currentDepth = _currentDepth
      _currentDepth <- _currentDepth + 1      
      match prev_context with
      | None -> { PrevContext  = None; 
                  Padding = padding
                  LastForId = None
                  IdClosure = Map.empty; 
                  CurrentContextDepth = Height _currentDepth}

      | Some prev_context -> { PrevContext = Some prev_context; 
                               Padding = prev_context.Padding + padding
                               LastForId = prev_context.LastForId 
                               IdClosure = prev_context.IdClosure;
                               CurrentContextDepth = Height _currentDepth }
                               
    member this.CurrentContextSymbol = 
      match this.CurrentContextDepth with
      Empty _ -> failwith (Position.Empty) "query context error"
      | Height n -> "__ContextSymbol" + string n
    member this.PrevContextSymbol = 
      match this.PrevContext with
      | None -> None
      | Some d  -> 
        match d.CurrentContextDepth with 
        | Empty _ -> failwith (Position.Empty) "query context error"
        | Height d ->  Some ("__ContextSymbol" + string d)
      
    member this.AddId (id : Id) = this.IdClosure <- this.IdClosure.Add(id, _currentDepth)
    member this.TrySolveId (id : Id) =
      let first_id = { idText = id.idText.Split('.').[0]; idRange = id.idRange }
      let id = solve_id_head id
      
      match this.IdClosure.TryFind(first_id) with
      | None -> id.idText
      | Some id_level ->
        let sum = ref -1
        let rec compute_prevs (ctxt : QueryContext option) = 
          match ctxt with
          | None -> ()
          | Some ctxt ->
            match ctxt.CurrentContextDepth with
            | Empty _ -> ()
            | Height n when n = id_level -> ()
            | _ -> incr sum
                   compute_prevs ctxt.PrevContext

      
        compute_prevs this.PrevContext
        this.CurrentContextSymbol + ([for _ in [id_level..(id_level + sum.Value + this.Padding)] do yield "prev."] |> Seq.fold(+) ".") + id.idText
    