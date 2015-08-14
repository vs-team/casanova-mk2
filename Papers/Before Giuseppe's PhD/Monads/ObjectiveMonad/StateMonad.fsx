module StateMonad


type ('a,'s) S = 's->'a*'s
type 's AS = 's->'s

type StateBuilder() =
  member this.Bind(e:S<'a,'s>, p:'a->S<'b,'s>):S<'b,'s> = fun (s) -> let x,s' = e (s) in p x (s')
  member this.Bind(e:AS<'s>, p:Unit->AS<'s>):S<Unit,'s> = fun (s) -> let s' = e s in (),p () s'
  member this.Bind(e:AS<'s>, p:Unit->S<'a,'s>):S<'a,'s> = fun (s) -> let s' = e s in p () (s')
  member this.Bind(e:S<'a,'s>, p:'a->AS<'s>):S<Unit,'s> = fun (s) -> let x,s' = e (s) in (),p x s'
  member this.Return(x):S<'a,'s> = fun (s) -> (x,s)
let state = StateBuilder()
