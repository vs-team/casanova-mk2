type Program<'a,'S,'I> = D of 'a * 'S | W of (('I -> M<'S,'a,'I>) * 'S) | R of 'S * M<'S,'a,'I> * 'S
and M<'S,'a,'I> = 'S -> Program<'a,'S,'I>

type ProgramSemantics() = 
    member this.Bind<'a,'b,'S,'I>(e:M<'S,'a,'I>, p:'a->M<'S,'b,'I>):M<'S,'b,'I> = 
        fun s ->
            match e s with
            | D(x,s') -> p x s'
            | W(f,s') -> W((fun i -> this.Bind(f i, p)), s')
            | R(sr,e',s') -> R(sr, this.Bind(e',p), s')
    member this.Return<'a,'S,'I>(x):M<'S,'a,'I> = fun s -> D(x,s)
    member this.Zero<'S,'I>():M<'S,Unit,'I> = fun s -> D((),s)
let Program = ProgramSemantics()
    
let store = fun sr -> fun s -> R(sr, Program{()}, s)
let P x:Program<Unit,int,Unit> = 
    x |>
    Program{
        do! store 0
        return ()
    }

open System.Threading.Tasks

type InteractiveThread<'S,'I> = D | W of ('I -> Task<InteractiveThread<'S,'I>>) | R of ('S * (Unit -> Task<InteractiveThread<'S,'I>>))
let rec run (p:Program<Unit,'S,'I>) =
    match p with
    | Program.D(_,_) -> D
    | Program.W(f,s) -> W(fun i -> async{return run (f i s)} |> Async.StartAsTask)
    | Program.R(sr,e',s) -> R(sr, (fun () -> async{return run (e' s)} |> Async.StartAsTask))

let update (i:'I) (x:Task<InteractiveThread<'S,'I>>) =
    if x.IsCompleted = false then None,x
    else
        match x.Result with
        | D -> None,x
        | W(f) -> None,f i
        | R(s,f) -> Some(s), f()
        
type AsyncEntity<'S,'I> = {Value:'S; Next:Task<InteractiveThread<'S,'I>>}
with
    member this.Update(i:'I):(AsyncEntity<'S,'I>*Option<'I>) =
        let (v',t') = update i this.Next
        let this' = 
            match v' with
            | None -> {this with Next=t'}
            | Some(v') -> {Value=v'; Next=t'}
        in this',None