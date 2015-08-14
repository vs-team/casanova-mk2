type 'a C(x:'a) = 
  member this.X with get() = x
  static member inline (+)(x:'b C,y:'b C) = C(x.X+y.X)


let x = C(0)
let y = C(1)
let z = x+y


let a = C("hello ")
let b = C("world!")
let c = a+b