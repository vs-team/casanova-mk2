module ObjectiveMonad

#load "StateMonad.fsx"
open StateMonad

type ('a,'s) Field(get:('s->'a*'s), set:('a->'s->Unit*'s)) =
  member this.get = get
  member this.set = set
  static member (+) (f:Field<_,_>,x) = Field((fun s -> let v,s' = f.get s in (v+x),s'), f.set)

open System
open Microsoft.FSharp.Reflection
open System
open Microsoft.FSharp.Reflection
let mk_accessors<'s,'a> (s:String) = 
  let t = typeof<'s>
  if t |> FSharpType.IsRecord then
    let ps = t |> FSharpType.GetRecordFields
    let pi = ps |> Seq.find (fun pi -> pi.Name = s)
    let si = ps |> Seq.findIndex (fun pi -> pi.Name = s)
    let get = pi.GetGetMethod()
    let set (v:'a) (s:'s) = 
      let vs = FSharpValue.GetRecordFields(s)
      do vs.[si] <- v:>obj
      FSharpValue.MakeRecord(t,vs) :?> 's
    in (fun (s:'s) -> get.Invoke(s, [||]) :?> 'a),set
  else failwith "Automatic getters and setters require record types: " + t.Name + " is not a record type."

let mk_field<'s,'a> (field_name:String) = 
  let mk_field(r:'s->'a, w:'a->'s->'s) = Field((fun (s:'s) ->r s,s), (fun v s -> (), w v s))
  let get,set = mk_accessors<'s,'a> field_name
  in mk_field(get,set)

let (<=) (p:Field<_,_>) m =
  state{
      let! v = p.get
      let r,v' = m v
      do! p.set v'
      return r}

let convert (self:Field<_,_>) (field:Field<_,_>) = Field(self <= field.get, (fun v -> self <= (field.set v)))
