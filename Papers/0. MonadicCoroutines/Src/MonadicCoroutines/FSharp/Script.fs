module ScriptMonad

open System
open System.Collections.Generic
open System.Linq
open System.Text.RegularExpressions
open Microsoft.FSharp.Collections

type Script<'a,'s> = 's -> ScriptResult<'a,'s>
and ScriptResult<'a,'s> = Return of 'a | Continue of Script<'a,'s>

let mutable steps = 0
let mutable start = DateTime.Now
let reset_count() = steps <- 0; start <- DateTime.Now; printf "reset\n"
let count() = steps <- steps + 1
let summarize() =
  let t = (DateTime.Now - start).TotalSeconds
  printf "performed %d steps in %f seconds: %f steps/seconds\n" steps t ((steps |> float) / t)

type ScriptBuilder() =
  member this.Delay f = f()
  member this.Return(x:'a) : Script<'a,'s> =
    fun s -> 
      count()
      Return x
  member this.Bind(p:Script<'a,'s>,k:'a->Script<'b,'s>):Script<'b,'s> =
    fun s ->
      count()
      match p s with
      | Return x -> Continue(k x)
      | Continue p' -> Continue(this.Bind(p',k))
  member this.Combine(p:Script<Unit,'s>,r:Script<'b,'s>):Script<'b,'s> =
    fun s ->
      count()
      match p s with
      | Return x -> Continue(r)
      | Continue p' -> Continue(this.Combine(p',r))
  member this.For(items:seq<'a>, body:'a -> Script<Unit,'s>) : Script<Unit,'s> =
    fun s ->
      if items |> Seq.length = 0 then 
        Return()
      else
        let hd,tl = items |> Seq.head, items |> Seq.skip 1
        Continue(this.Combine(body hd, this.For(tl, body)))
  member this.While(cond : Unit -> bool, body:Script<Unit,'s>) : Script<Unit,'s> =
    fun s ->
      if cond() then
        match body s with
        | Return() -> Continue(this.While(cond,body))
        | Continue body' -> Continue(this.Combine(body',this.While(cond, body)))
      else 
        Return()
  member this.Zero() = this.Return()
  member this.ReturnFrom p = p

let script = ScriptBuilder()

let time _ = Return (DateTime.Now)

let wait max_dt =
  let rec wait t0 =
    script{
      let! t = time
      let dt = ((t - t0) : TimeSpan).TotalSeconds |> float32
      if dt < max_dt then
        return! wait t0
    }
  script {
    let! t0 = time
    return! wait t0
  }


let rec forever_ (s : Script<Unit,_>) : Script<Unit,_> =
  script{
    do! s
    return! forever_ s
  }

let rec while_ (c : Script<bool,_>) (b:Script<Unit,_>) : Script<Unit,_> =
  script{
    let! c_v = c
    if c_v then
      do! b
      return! while_ c b
    else
      return ()
  }

let ignore_ (p:Script<'a,_>) : Script<Unit,_> =
  script{
    let! _ = p
    return ()
  }
  
let rec parallel_ (p:Script<'a,_>) (r:Script<'b,_>) : Script<'a * 'b,_> =
  fun s ->
    match p s, r s with
    | Return x, Return y -> Return(x,y)
    | Continue p', Continue r' -> Continue(parallel_ p' r')
    | Continue p', Return y -> Continue(parallel_ p' (script.Return(y)))
    | Return x, Continue r' -> Continue(parallel_ (script.Return(x)) r')

let parallel_ignore_ p1 p2 = parallel_ p1 p2 |> ignore_

let rec parallel_many_ = 
  function
  | [] -> script{return ()}
  | (t:Script<'a,_>) :: (ts:Script<'a,_> list) -> parallel_ t (parallel_many_ ts) |> ignore_

type Either<' a,' b> = Left of ' a | Right of ' b
let rec parallel_first_ (p:Script<'a,_>) (r:Script<'b,_>) : Script<Either<'a, 'b>,_> =
  fun s ->
    match p s, r s with
    | Return x, _ -> Return(Left x)
    | _, Return y -> Return(Right y)
    | Continue p', Continue r' -> Continue(parallel_first_ p' r')

let parallel_first_ignore_ p1 p2 = parallel_first_ p1 p2 |> ignore_

let rec atomic_ (p:Script<'a,_>) : Script<'a,_> =
  fun s ->
    match p s with
    | Return x -> Return x 
    | Continue p' -> atomic_ p' s

let not_ (p:Script<bool,_>) : Script<bool,_> =
  script{
    let! res = p
    return res |> not
  }

let lift0 f =
  script{
    return f()
  }
  
let (!!) f = lift0 f

let lift1 f p =
  script{
    let! x = p
    return f x
  } 

let lift2 f p1 p2 = 
  script{
    let! a = p1
    let! b = p2
    return f a b
  }
  
let (.||) p1 p2 = lift2 (||) p1 p2
let (.&&) p1 p2 = lift2 (&&) p1 p2

let run_ p =
  do reset_count()
  let rec run_ (p:Script<'a,'s>) =
    fun s ->
      match p s with
      | Return x -> x 
      | Continue p' -> run_ p' s
  fun x -> 
    let res = run_ p x
    do summarize()
    res

let wait_while_ c = while_ c (script{ return () })

let iter_ f l =
  script{
    for x in l do f x
  }
