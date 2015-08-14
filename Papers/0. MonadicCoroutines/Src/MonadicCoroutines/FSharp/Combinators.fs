module Combinators
  open ScriptMonad
  type Script<'a> = Script<'a,Unit>

  
  (*
   * TIER 0
   *)    
      
  let lift1_ (f:'a -> 'b) (s1:Script<'a>) : Script<'b> =
    script {
      let! x = s1
      return f x
    }
    
  let not_ (s:Script<bool>) : Script<bool> =
    lift1_ not s
  
  let ignore_ (s:Script<'a>) =
    lift1_ (fun x -> ()) s
  
  let lift2_ (f:'a -> 'b -> 'c) (s1:Script<'a>) (s2:Script<'b>) : Script<'c> =
    script {
      let! x = s1
      let! y = s2
      return f x y
    }
  
  let and_ (s1:Script<bool>) (s2:Script<bool>) : Script<bool> =
    lift2_ (&&) s1 s2
  
  let or_ (s1:Script<bool>) (s2:Script<bool>) : Script<bool> =
    lift2_ (||) s1 s2
   
  let rec atomic_ (p:Script<'a>) : Script<'a> =
    fun s ->
      match p s with
      | Return x -> Return x
      | Continue k -> atomic_ k s
     
  
 
  (*
   * TIER 1
   *)    
   
  let rec parallel_many_ (ss:list<Script<'a>>) : Script<list<'a>> =
    fun s ->
      let ss' = ss |> List.map (fun p -> p s)
      if ss' |> Seq.forall (function Return _ -> true | _ -> false) then
        Return(ss' |> List.map (function Return x -> x))
      else
        Continue(parallel_many_ (ss' |> List.map (function Return x -> (fun _ -> Return x) | Continue k -> k)))
  
  let parallel_ (s1:Script<'a>) (s2:Script<'b>) : Script<'a * 'b> =
    let s1' = lift1 (Left) s1
    let s2' = lift1 (Right) s2
    script {
      let! [Left x;Right y] = parallel_many_ [s1';s2']
      return (x,y)
    }
  
  let rec parallel_many_first_ (ss:list<Script<'a>>) : Script<'a> =
    fun s ->
      let ss' = ss |> List.map (fun p -> p s)
      match ss' |> Seq.tryFind (function Return x -> true | _ -> false) with
      | Some(Return x) -> Return x
      | None -> Continue(parallel_many_first_ (ss' |> List.map (function Continue k -> k)))
    
  let parallel_first_ (s1:Script<'a>) (s2:Script<'b>) : Script<Either<'a,'b>> =
    let s1' = lift1 (Left) s1
    let s2' = lift1 (Right) s2
    parallel_many_first_ [s1';s2']
  
  
  let rec while_ (cond:Script<bool>) (body:Script<Unit>) : Script<Unit> =
    script {
      let! c = cond
      if not c then
        return ()
      else
        do! body
        do! while_ cond body
    }
             
  let wait_condition_ (c:Script<bool>) : Script<Unit> =
    while_ c (script{ return () })
  
  let wait_and_do_ (dt:float) (action:Script<Unit>) : Script<Unit> =
    script{
      let! t0 = time
      do! while_
            (script{
              let! t = time
              return ((t-t0).TotalSeconds < dt)
             }) 
             action         
    }
    
  let wait_ (dt:float) : Script<Unit> =
    wait_and_do_ dt (script{ return () })
    
  let forever_ (p:Script<Unit>) : Script<Unit> =
    while_ (script{ return true }) p
    
  let wait_interrupt_ (cond:Script<bool>) (s:Script<Unit>) : Script<Unit> =
    parallel_first_ (wait_condition_ cond) (forever_ s)
    |> ignore_

           
  (*
   * TIER 2
   *)    
   
  let level_skeleton (init:Script<'a>) (logic:'a -> Script<'b>) (cleanup:'b -> Script<'c>) : Script<'c> =
    script{
      let! x = init |> atomic_
      let! y = logic x
      return! cleanup y |> atomic_ 
    }
                                 
  let while_not_won (is_victory:Script<bool>) (logic:Script<Unit>) = 
    level_skeleton (script{ return() })
                   (fun () -> wait_interrupt_ is_victory logic)
                   (fun () -> script{ return() })
  
  let wait_victory (is_victory:Script<bool>) =
    while_not_won is_victory (script{ return () })
       
