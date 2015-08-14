type S<'a,'s,'i> = ('s*'i)->('a*'s)
type AS<'s> = 's->'s

type StateBuilder() =
  member this.Bind(e:S<'a,'s,'i>, p:'a->S<'b,'s,'i>):S<'b,'s,'i> =
    fun (s,i) -> let x,s' = e (s,i) in p x (s',i)

  member this.Bind(e:AS<'s>, p:Unit->AS<'s>):S<Unit,'s,'i> =
    fun (s,i) -> let s' = e s in (),p () s'

  member this.Bind(e:AS<'s>, p:Unit->S<'a,'s,'i>):S<'a,'s,'i> =
    fun (s,i) -> let s' = e s in p () (s',i)

  member this.Bind(e:S<'a,'s,'i>, p:'a->AS<'s>):S<Unit,'s,'i> =
    fun (s,i) -> let x,s' = e (s,i) in (),p x s'

  member this.Return(x):S<'a,'s,'i> = fun (s,i) -> (x,s)

let state = StateBuilder()

let set_state =
  (Set.singleton 0,()) |>
  state{
    do! Set.add 1
    do! (fun (s,i) -> (),Set.union s (Set.singleton 2))
    return ()
  }

//> set_state;;
//val it : unit * Set<int> = (null, seq [0; 1; 2])