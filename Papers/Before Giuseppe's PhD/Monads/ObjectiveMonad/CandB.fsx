#load "StateMonad.fsx"
open StateMonad

#load "ObjectiveMonad.fsx"
open ObjectiveMonad

module B =
  type Tb = {x:int}
  let x = mk_field<Tb,int> "x"
  let cons v = {x=v}
  let M() = state.Bind((x+1).get, x.set)

open B

module C =
  type Tc = {y:int; b:Tb}
  let y = mk_field<Tc,int> "y"
  let b = mk_field<Tc,Tb> "b"
  let cons v = {y = v; b = cons v}
  let toB (self:Field<_,_>) = convert self b

open C

type Tbc = {b:Tb; c:Tc}
let c = mk_field<Tbc,Tc> "c"
let b = mk_field<Tbc,Tb> "b"

let main =
  state{
    let! v = c <= (y.get)
    do! b <= x.set (v*2)
    let b' = toB c
    do! b' <= M()
    return ()
  }

let res = snd(main {b=B.cons 0; c=C.cons 10})

