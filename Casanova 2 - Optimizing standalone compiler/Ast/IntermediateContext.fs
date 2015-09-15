module IntermediateContext
open Common
open IntermediateAST



//                                                           owner field          target rule
type DependencyContext = System.Collections.Generic.Dictionary<Id * Id, ResizeArray<Id * Id>>
type EntityTable = Map<Id, List<TypedAST.Field>>

let Lookup (id:Id) (current_rule : Id) (this : Id * List<TypedAST.Field>) (table:EntityTable) (context : DependencyContext) =
  let raise = raise id.idRange
  let initial_entity = fst this
  let ids = id.idText.Split [|'.'|] |> Seq.toList
  if ids.Length = 0 then
    raise "Intermediate error. unexpected error" |> ignore
  let rec eval_ids (this : Id * List<TypedAST.Field>) (ids : string list) =
    let this_name, this_fields = fst this, snd this 
    match ids with
    [] -> raise "Intermediate error. unexpected error" |> ignore
    | [x] -> 
      let key = this_name, (this_fields |> Seq.tryFind(fun f -> f.Name.idText = x))
      match key with
      | this_name, None -> raise (sprintf "Cannot find field %s in %s" x this_name.idText) 
      | this, Some f ->
        let key = this, f.Name.Id
        context.[key].Add(initial_entity, current_rule)
    | id::xs ->       
      let f_a = this_fields |> Seq.tryFind(fun f -> f.Name.idText = id)
      match f_a with
      | None -> 
        raise (sprintf "Cannot find field %s in %s" id this_name.idText)         
      | Some f_a -> 
        match f_a.Type with
        | TypedAST.EntityName(id) -> 
          let this = table.[id.Id]; 
          eval_ids (id.Id, this) xs
        | _ -> ()
  eval_ids this ids
