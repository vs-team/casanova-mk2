open System
open ScriptMonad

let rec fibonacci n =
  script{
    match n with
    | 0 -> return 0
    | 1 -> return 1
    | n -> 
      let! n1 = fibonacci (n-1)
      let! n2 = fibonacci (n-2)
      return n1 + n2
      //return! lift2 (+) (fibonacci (n-1)) (fibonacci (n-2))
  }
  
let rec log i : Script<Unit,_> =
  script{
    //do! !! (fun () -> printf "\n\n%d\n" i)
    do! wait 2.0f
    return! log (i+1)
  }


let fibonacci_test() =
  let test = parallel_first_ignore_ (fibonacci 25) (log 0) : Script<_,Unit>
  do run_ test ()

let many_fibonacci_test() =
  let test = parallel_many_ ([for i in 0 .. 15 do yield fibonacci (i+5) |> ignore_]) : Script<_,Unit>
  do run_ test ()
  
type Entity = {mutable Position:float32}
type State = {mutable Entities:Entity list}
let get_state = fun s -> Return s
let add_ship mk_ship run_ship state = 
  script{
    let s = state
    let new_ship = mk_ship()
    do s.Entities <- new_ship :: s.Entities
    let! result = run_ship new_ship
    do s.Entities <- s.Entities |> List.filter ((<>) new_ship)
    return result
  }

let simple_ship self =
  while_ (fun _ -> Return(self.Position > 0.0f))
         (script{
            do! (fun _ -> Return(self.Position <- self.Position - 0.1f))
            //do! wait 0.1f
          })

let rec many_ships n state =
  if n > 0 then
    parallel_ignore_ (add_ship (fun () -> {Position = 100.0f - (n |> float32)}) simple_ship state) (many_ships (n-1) state)
  else
    script{ return () }

(*let log_ships =
  forever_ (script{
              let! s = get_state
              //do printf "there are %d ships\n" s.Entities.Length
              do! wait 2.0f
            })*)

let state_access_test() = 
  let state = {Entities = []}
  //let test = parallel_ignore_ (many_ships 200 state) log_ships
  let test = many_ships 200 state
  do run_ test state
  
//do fibonacci_test()
do many_fibonacci_test()
//do state_access_test()
