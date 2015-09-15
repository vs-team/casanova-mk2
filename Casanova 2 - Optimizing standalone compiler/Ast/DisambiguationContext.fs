module ShadowcheckContext
type internal ShadowContext_LatestAddedIndex = System.Collections.Generic.Dictionary<string, int>
let private shadowContext_LatestAddedIndex = new System.Collections.Generic.Dictionary<string, int>()
type ShadowCheckContext = 
  { values : Map<string, int * int> }
  with 
    member this.Lookup (id : string) =
      let ids = id.Split('.') |> Seq.toList
      let id = ids.Head
      let ids = if ids.Tail <> [] then "." + (ids.Tail |> List.reduce(fun a b -> a + "." + b)) else ""

      if this.values.ContainsKey(id) |> not then id + ids
      else 
        let rule, idx = this.values.[id]
        if idx > -1 then
          "___" + id + string rule + string idx + ids
        else
          id + ids
    member this.Add (rule_idx : int) (id : string) : string * ShadowCheckContext =
                  if shadowContext_LatestAddedIndex.ContainsKey(id) |> not then
                    shadowContext_LatestAddedIndex.Add(id, 0)
                    "___" + id + string rule_idx + "0",
                    {this with values = this.values.Add(id, (rule_idx, 0)) }
                  else
                    let latest_index = shadowContext_LatestAddedIndex.[id]
                    let index = latest_index + 1
                    shadowContext_LatestAddedIndex.[id] <- index
                    "___" + id + string rule_idx + string index,
                    {this with values = 
                                     let values' = this.values.Remove(id)
                                     values'.Add(id, (rule_idx, index)) }

    member private this.AUXAdd rule_idx ids ids1 =
      match ids with
      | [] -> ids1, this
      | id::ids ->
        let id1, ctxt =  this.Add rule_idx id
        ctxt.AUXAdd rule_idx ids (id1 :: ids1)

    member this.AddRange (rule_idx : int) (ids : string list) = this.AUXAdd rule_idx (ids |> List.rev) []
      
