(*type T1Res() =
  member this.Hi = "T1Res Says Hi!"

type T1() =
  member this.F = T1Res()

type T2Res() =
  member this.Hi = "T2Res Says Hi!"

type T2() =
  member this.F = T2Res()

let inline functors (arg:^a) : ^b =
  let tres = ( ^a : (member F : ^b) arg)
  in tres

let hi1 = (functors (T1())).Hi
let hi2 = (functors (T2())).Hi*)

type Zero() =
  static member inline toNat() = 0

type Succ< ^a when ^a : (static member toNat : unit -> int)>() =
  static member inline toNat() = 1 + (^a : (static member toNat : unit -> int) ())

type One    = Succ<Zero>
type Two    = Succ<One>
type Three  = Succ<Two>
type Four   = Succ<Three>
type Five   = Succ<Four>
type Six    = Succ<Five>
type Seven  = Succ<Six>
type Eight  = Succ<Seven>
type Nine   = Succ<Eight>
type Ten    = Succ<Nine>

let numbers = [One.toNat(); Two.toNat(); Three.toNat(); Four.toNat(); Five.toNat();
               Six.toNat(); Seven.toNat(); Eight.toNat(); Nine.toNat(); Ten.toNat()]
// val numbers : int list = [1; 2; 3; 4; 5; 6; 7; 8; 9; 10]

type Nil() = 
  static member inline length() = Zero()

type Cons< ^hd,^tl,^tl_len when ^tl : (static member length : unit -> ^tl_len)
                           and  ^tl_len : (static member toNat : unit -> int) >
                           (hd:^hd,tl:^tl) =
  static member inline length() = Succ< ^tl_len >()


let person = Cons(27, Nil())
//let list = Cons("John", Cons("Doe", Cons(27, Nil())))