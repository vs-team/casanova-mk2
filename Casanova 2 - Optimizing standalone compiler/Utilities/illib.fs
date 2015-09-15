//----------------------------------------------------------------------------
//
// Copyright (c) 2002-2012 Microsoft Corporation. 
//
// This source code is subject to terms and conditions of the Apache License, Version 2.0. A 
// copy of the license can be found in the License.html file at the root of this distribution. 
// By using this source code in any fashion, you are agreeing to be bound 
// by the terms of the Apache License, Version 2.0.
//
// You must not remove this notice, or any other, from this software.
//----------------------------------------------------------------------------


module (* (*internal*) *) Microsoft.FSharp.Compiler.Internal.Library 
#nowarn "1178" 


open System
open System.Collections
open System.Collections.Generic


// Logical shift right treating int32 as unsigned integer.
// Code that uses this should probably be adjusted to use unsigned integer types.
let (>>>&) (x:int32) (n:int32) = int32 (uint32 x >>> n)

let notlazy v = Lazy.CreateFromValue v

let isSome x = match x with None -> false | _ -> true
let isNone x = match x with None -> true | _ -> false
let isNil x = match x with [] -> true | _ -> false
let nonNil x = match x with [] -> false | _ -> true
let isNull (x : 'T) = match (x :> obj) with null -> true | _ -> false
let isNonNull (x : 'T) = match (x :> obj) with null -> false | _ -> true
let nonNull msg x = if isNonNull x then x else failwith ("null: " ^ msg) 
let (===) x y = LanguagePrimitives.PhysicalEquality x y

//-------------------------------------------------------------------------
// Library: projections
//------------------------------------------------------------------------

let foldOn p f z x = f z (p x)

let notFound() = raise (KeyNotFoundException())

module Order = 
    let orderBy (p : 'T -> 'U) = 
        { new IComparer<'T> with member __.Compare(x,xx) = compare (p x) (p xx) }

    let orderOn p (pxOrder: IComparer<'U>) = 
        { new IComparer<'T> with member __.Compare(x,xx) = pxOrder.Compare (p x, p xx) }

    let toFunction (pxOrder: IComparer<'U>) x y = pxOrder.Compare(x,y)

//-------------------------------------------------------------------------
// Library: arrays,lists,options
//-------------------------------------------------------------------------

module Array = 
    

    let mapq f inp =
        match inp with
        | [| |] -> inp
        | _ -> 
            let res = Array.map f inp 
            let len = inp.Length 
            let mutable eq = true
            let mutable i = 0 
            while eq && i < len do 
                if not (inp.[i] === res.[i]) then eq <- false;
                i <- i + 1
            if eq then inp else res

    let forall2 f (arr1:'T array) (arr2:'T array) =
        let len1 = arr1.Length 
        let len2 = arr2.Length 
        if len1 <> len2 then invalidArg "Array.forall2" "len1"
        let rec loop i = (i >= len1) || (f arr1.[i] arr2.[i] && loop (i+1))
        loop 0

    let lengthsEqAndForall2 p l1 l2 = 
        Array.length l1 = Array.length l2 &&
        Array.forall2 p l1 l2

    let mapFold f s l = 
        let mutable acc = s
        let n = Array.length l
        let mutable res = Array.zeroCreate n
        for i = 0 to n - 1 do
            let h',s' = f acc l.[i]
            res.[i] <- h';
            acc <- s'
        res, acc


    // REVIEW: systematically eliminate fmap/mapFold duplication. 
    // They only differ by the tuple returned by the function.
    let fmap f s l = 
        let mutable acc = s
        let n = Array.length l
        let mutable res = Array.zeroCreate n
        for i = 0 to n - 1 do
            let s',h' = f acc l.[i]
            res.[i] <- h'
            acc <- s'
        acc, res

    let order (eltOrder: IComparer<'T>) = 
        { new IComparer<array<'T>> with 
              member __.Compare(xs,ys) = 
                  let c = compare xs.Length ys.Length 
                  if c <> 0 then c else
                  let rec loop i = 
                      if i >= xs.Length then 0 else
                      let c = eltOrder.Compare(xs.[i], ys.[i])
                      if c <> 0 then c else
                      loop (i+1)
                  loop 0 }

    let existsOne p l = 
        let rec forallFrom p l n =
          (n >= Array.length l) || (p l.[n] && forallFrom p l (n+1))

        let rec loop p l n =
            (n < Array.length l) && 
            (if p l.[n] then forallFrom (fun x -> not (p x)) l (n+1) else loop p l (n+1))
          
        loop p l 0

    
    let findFirstIndexWhereTrue (arr: _[]) p = 
        let rec look lo hi = 
            assert ((lo >= 0) && (hi >= 0))
            assert ((lo <= arr.Length) && (hi <= arr.Length))
            if lo = hi then lo
            else
                let i = (lo+hi)/2
                if p arr.[i] then 
                    if i = 0 then i 
                    else
                        if p arr.[i-1] then 
                            look lo i
                        else 
                            i
                else
                    // not true here, look after
                    look (i+1) hi
        look 0 arr.Length
      
        
module Option = 
    let mapFold f s opt = 
        match opt with 
        | None -> None,s 
        | Some x -> let x',s' = f s x in Some x',s'

    let otherwise opt dflt = 
        match opt with 
        | None -> dflt 
        | Some x -> x

    // REVIEW: systematically eliminate fmap/mapFold duplication
    let fmap f z l = 
        match l with 
        | None   -> z,None
        | Some x -> let z,x = f z x
                    z,Some x

    let fold f z x = 
        match x with 
        | None -> z 
        | Some x -> f z x


module List = 
    
    let sortWithOrder (c: IComparer<'T>) elements = List.sortWith (Order.toFunction c) elements
    
    let splitAfter n l = 
        let rec split_after_acc n l1 l2 = if n <= 0 then List.rev l1,l2 else split_after_acc (n-1) ((List.head l2):: l1) (List.tail l2) 
        split_after_acc n [] l

    let existsi f xs = 
       let rec loop i xs = match xs with [] -> false | h::t -> f i h || loop (i+1) t
       loop 0 xs
    
    let lengthsEqAndForall2 p l1 l2 = 
        List.length l1 = List.length l2 &&
        List.forall2 p l1 l2

    let rec findi n f l = 
        match l with 
        | [] -> None
        | h::t -> if f h then Some (h,n) else findi (n+1) f t

    let chop n l = 
        if n = List.length l then (l,[]) else // avoids allocation unless necessary 
        let rec loop n l acc = 
            if n <= 0 then (List.rev acc,l) else 
            match l with 
            | [] -> failwith "List.chop: overchop"
            | (h::t) -> loop (n-1) t (h::acc) 
        loop n l [] 

    let take n l = 
        if n = List.length l then l else 
        let rec loop acc n l = 
            match l with
            | []    -> List.rev acc
            | x::xs -> if n<=0 then List.rev acc else loop (x::acc) (n-1) xs

        loop [] n l

    let rec drop n l = 
        match l with 
        | []    -> []
        | _::xs -> if n=0 then l else drop (n-1) xs


    let splitChoose select l =
        let rec ch acc1 acc2 l = 
            match l with 
            | [] -> List.rev acc1,List.rev acc2
            | x::xs -> 
                match select x with
                | Choice1Of2 sx -> ch (sx::acc1) acc2 xs
                | Choice2Of2 sx -> ch acc1 (sx::acc2) xs

        ch [] [] l

    let mapq (f: 'T -> 'T) inp =
        assert not (typeof<'T>.IsValueType) 
        match inp with
        | [] -> inp
        | _ -> 
            let res = List.map f inp 
            let rec check l1 l2 = 
                match l1,l2 with 
                | h1::t1,h2::t2 -> 
                    System.Runtime.CompilerServices.RuntimeHelpers.Equals(h1,h2) && check t1 t2
                | _ -> true
            if check inp res then inp else res
        
    let frontAndBack l = 
        let rec loop acc l = 
            match l with
            | [] -> 
                System.Diagnostics.Debug.Assert(false, "empty list")
                invalidArg "l" "empty list" 
            | [h] -> List.rev acc,h
            | h::t -> loop  (h::acc) t
        loop [] l

    let tryRemove f inp = 
        let rec loop acc l = 
            match l with
            | [] -> None
            | h :: t -> if f h then Some (h, List.rev acc @ t) else loop (h::acc) t
        loop [] inp            
    //tryRemove  (fun x -> x = 2) [ 1;2;3] = Some (2, [1;3])
    //tryRemove  (fun x -> x = 3) [ 1;2;3;4;5] = Some (3, [1;2;4;5])
    //tryRemove  (fun x -> x = 3) [] = None
            
    let headAndTail l =
        match l with 
        | [] -> 
            System.Diagnostics.Debug.Assert(false, "empty list")
            failwith "List.headAndTail"
        | h::t -> h,t

    let zip4 l1 l2 l3 l4 = 
        List.zip l1 (List.zip3 l2 l3 l4) |> List.map (fun (x1,(x2,x3,x4)) -> (x1,x2,x3,x4))

    let unzip4 l = 
        let a,b,cd = List.unzip3 (List.map (fun (x,y,z,w) -> (x,y,(z,w))) l)
        let c,d = List.unzip cd
        a,b,c,d

    let rec iter3 f l1 l2 l3 = 
        match l1,l2,l3 with 
        | h1::t1, h2::t2, h3::t3 -> f h1 h2 h3; iter3 f t1 t2 t3
        | [], [], [] -> ()
        | _ -> failwith "iter3"

    let takeUntil p l =
        let rec loop acc l =
            match l with
            | [] -> List.rev acc,[]
            | x::xs -> if p x then List.rev acc, l else loop (x::acc) xs
        loop [] l

    let order (eltOrder: IComparer<'T>) =
        { new IComparer<list<'T>> with 
              member __.Compare(xs,ys) = 
                  let rec loop xs ys = 
                      match xs,ys with
                      | [],[]       ->  0
                      | [],_        -> -1
                      | _,[]       ->  1
                      | x::xs,y::ys -> let cxy = eltOrder.Compare(x,y)
                                       if cxy=0 then loop xs ys else cxy 
                  loop xs ys }


    let rec last l = match l with [] -> failwith "last" | [h] -> h | _::t -> last t
    module FrontAndBack = 
        let (|NonEmpty|Empty|) l = match l with [] -> Empty | _ -> NonEmpty(frontAndBack l)

    let replicate x n = 
        Array.toList (Array.create x n)

    let range n m = [ n .. m ]

    let indexNotFound() = raise (new System.Collections.Generic.KeyNotFoundException("An index satisfying the predicate was not found in the collection"))

    let rec assoc x l = 
        match l with 
        | [] -> indexNotFound()
        | ((h,r)::t) -> if x = h then r else assoc x t

    let rec memAssoc x l = 
        match l with 
        | [] -> false
        | ((h,_)::t) -> x = h || memAssoc x t

    let rec contains x l = match l with [] -> false | h::t -> x = h || contains x t

    let rec memq x l = 
        match l with 
        | [] -> false 
        | h::t -> LanguagePrimitives.PhysicalEquality x h || memq x t

    let mem x l = contains x l
    
    // must be tail recursive 
    let mapFold f s (l : 'b list) = 
        let ll = l
        let r1 = System.Collections.Generic.LinkedList()
        // microbenchmark suggested this implementation is faster than the simpler recursive one, and this function is called a lot
        let mutable s1 = s
        for x in l do
          let (x' : 'c), s' = f s1 x
          s1 <- s'
          r1.AddLast(x') |> ignore
        r1 |> Seq.toList, s1
//
//        let mutable s = s
//        let mutable r = []
//        let mutable l = l
//        let mutable finished = false
//        while not finished do
//          match l with
//          | x::xs -> let x',s' = f s x
//                     s <- s'
//                     r <- x' :: r
//                     l <- xs
//          | _ -> finished <- true
//        let res2 = List.rev r, s
//        res2
    // note: not tail recursive 
    let rec mapfoldBack f l s = 
        match l with 
        | [] -> ([],s)
        | h::t -> 
           let t',s = mapfoldBack f t s
           let h',s = f h s
           (h'::t', s)

    let mapNth n f xs =
        let rec mn i = function
          | []    -> []
          | x::xs -> if i=n then f x::xs else x::mn (i+1) xs
       
        mn 0 xs

    let rec until p l = match l with [] -> [] | h::t -> if p h then [] else h :: until p t 

    let count pred xs = List.fold (fun n x -> if pred x then n+1 else n) 0 xs

    let rec private repeatA n x acc = if n <= 0 then acc else repeatA (n-1) x (x::acc)
    let repeat n x = repeatA n x []

    (* WARNING: not tail-recursive *)
    let mapHeadTail fhead ftail = function
      | []    -> []
      | [x]   -> [fhead x]
      | x::xs -> fhead x :: List.map ftail xs

    let collectFold f s l = 
      let l, s = mapFold f s l
      List.concat l, s

    let singleton x = [x]

    // note: must be tail-recursive 
    let rec private fmapA f z l acc =
      match l with
      | []    -> z,List.rev acc
      | x::xs -> let z,x = f z x
                 fmapA f z xs (x::acc)
                 
    // note: must be tail-recursive 
    // REVIEW: systematically eliminate fmap/mapFold duplication
    let fmap f z l = fmapA f z l []

    let collect2 f xs ys = List.concat (List.map2 f xs ys)

    let iterSquared f xss = xss |> List.iter (List.iter f)
    let collectSquared f xss = xss |> List.collect (List.collect f)
    let mapSquared f xss = xss |> List.map (List.map f)
    let mapfoldSquared f xss = xss |> mapFold (mapFold f)
    let forallSquared f xss = xss |> List.forall (List.forall f)
    let mapiSquared f xss = xss |> List.mapi (fun i xs -> xs |> List.mapi (fun j x -> f i j x))
    let existsSquared f xss = xss |> List.exists (fun xs -> xs |> List.exists (fun x -> f x))

module String = 
    let indexNotFound() = raise (new System.Collections.Generic.KeyNotFoundException("An index for the character was not found in the string"))

    let make (n: int) (c: char) : string = new System.String(c, n)

    let get (str:string) i = str.[i]

    let sub (s:string) (start:int) (len:int) = s.Substring(start,len)

    let index (s:string) (c:char) =  
        let r = s.IndexOf(c) 
        if r = -1 then indexNotFound() else r

    let rindex (s:string) (c:char) =
        let r =  s.LastIndexOf(c) 
        if r = -1 then indexNotFound() else r

    let contains (s:string) (c:char) = 
        s.IndexOf(c,0,String.length s) <> -1

    let order = LanguagePrimitives.FastGenericComparer<string>
   
    let lowercase (s:string) =
        s.ToLowerInvariant()

    let uppercase (s:string) =
        s.ToUpperInvariant()

    let isUpper (s:string) = 
        s.Length >= 1 && System.Char.IsUpper s.[0] && not (System.Char.IsLower s.[0])
        
    let capitalize (s:string) =
        if s.Length = 0 then s 
        else uppercase s.[0..0] + s.[ 1.. s.Length - 1 ]

    let uncapitalize (s:string) =
        if s.Length = 0 then  s
        else lowercase s.[0..0] + s.[ 1.. s.Length - 1 ]


    let tryDropPrefix (s:string) (t:string) = 
        if s.StartsWith t then 
            Some s.[t.Length..s.Length - 1]
        else 
            None

    let tryDropSuffix (s:string) (t:string) = 
        if s.EndsWith t then
            Some s.[0..s.Length - t.Length - 1]
        else
            None

    let hasPrefix s t = isSome (tryDropPrefix s t)
    let dropPrefix s t = match (tryDropPrefix s t) with Some(res) -> res | None -> failwith "dropPrefix"

    let dropSuffix s t = match (tryDropSuffix s t) with Some(res) -> res | None -> failwith "dropSuffix"

module Dictionary = 

    let inline ofList l = 
        let dict = new System.Collections.Generic.Dictionary<_,_>(List.length l, HashIdentity.Structural)
        l |> List.iter (fun (k,v) -> dict.Add(k,v))
        dict


// FUTURE CLEANUP: remove this adhoc collection
type Hashset<'T> = Dictionary<'T,int>
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Hashset = 
    let create (n:int) = new Hashset<'T>(n, HashIdentity.Structural)
    let add (t: Hashset<'T>) x = if not (t.ContainsKey x) then t.[x] <- 0
    let fold f (t:Hashset<'T>) acc = Seq.fold (fun z (KeyValue(x,_)) -> f x z) acc t 
    let ofList l = 
        let t = new Hashset<'T>(List.length l, HashIdentity.Structural)
        l |> List.iter (fun x -> t.[x] <- 0)
        t
        
module Lazy = 
    let force (x: Lazy<'T>) = x.Force()

//---------------------------------------------------
// Lists as sets. This is almost always a bad data structure and should be eliminated from the compiler.  

module ListSet =
    let insert e l =
        if List.mem e l then l else e::l

//---------------------------------------------------
// Misc

/// Get an initialization hole 
let getHole r = match !r with None -> failwith "getHole" | Some x -> x

module Map = 
    let tryFindMulti k map = match Map.tryFind k map with Some res -> res | None -> []

type ResultOrException<'TResult> =
    | Result of 'TResult
    | Exception of System.Exception
                     


//---------------------------------------------------------------------------
// generate unique stamps
//---------------------------------------------------------------------------

type UniqueStampGenerator<'T when 'T : equality>() = 
    let encodeTab = new Dictionary<'T,int>(HashIdentity.Structural)
    let mutable nItems = 0
    let encode str = 
        if encodeTab.ContainsKey(str)
        then
            encodeTab.[str]
        else
            let idx = nItems
            encodeTab.[str] <- idx
            nItems <- nItems + 1
            idx
    member this.Encode(str)  = encode str

//---------------------------------------------------------------------------
// memoize tables (all entries cached, never collected)
//---------------------------------------------------------------------------
    
type MemoizationTable<'T,'U>(compute: 'T -> 'U, keyComparer: IEqualityComparer<'T>, ?canMemoize) = 
    
    let table = new System.Collections.Generic.Dictionary<'T,'U>(keyComparer) 
    member t.Apply(x) = 
        if (match canMemoize with None -> true | Some f -> f x) then 
            let mutable res = Unchecked.defaultof<'U>
            let ok = table.TryGetValue(x,&res)
            if ok then res 
            else
                lock table (fun () -> 
                    let mutable res = Unchecked.defaultof<'U> 
                    let ok = table.TryGetValue(x,&res)
                    if ok then res 
                    else
                        let res = compute x
                        table.[x] <- res;
                        res)
        else compute x


exception UndefinedException

[<AutoOpen>]
module Shim =

    open System.IO
    [<AbstractClass>]
    type FileSystem() = 
        abstract ReadAllBytesShim: fileName:string -> byte[] 
        default this.ReadAllBytesShim (fileName:string) = 
            use stream = this.FileStreamReadShim fileName
            let len = stream.Length
            let buf = Array.zeroCreate<byte> (int len)
            stream.Read(buf, 0, (int len)) |> ignore                                            
            buf

        abstract FileStreamReadShim: fileName:string -> System.IO.Stream
        abstract FileStreamCreateShim: fileName:string -> System.IO.Stream
        abstract GetFullPathShim: fileName:string -> string
        /// Take in a filename with an absolute path, and return the same filename
        /// but canonicalized with respect to extra path separators (e.g. C:\\\\foo.txt) 
        /// and '..' portions
        abstract SafeGetFullPath: fileName:string -> string
        abstract IsPathRootedShim: path:string -> bool

        abstract IsInvalidFilename: filename:string -> bool
        abstract GetTempPathShim : unit -> string
        abstract GetLastWriteTimeShim: fileName:string -> System.DateTime
        abstract SafeExists: fileName:string -> bool
        abstract FileDelete: fileName:string -> unit
        abstract AssemblyLoadFrom: fileName:string -> System.Reflection.Assembly 
        abstract AssemblyLoad: assemblyName:System.Reflection.AssemblyName -> System.Reflection.Assembly 

#if SILVERLIGHT
        default this.AssemblyLoadFrom(fileName:string) = 
              let load() = 
                  let assemblyPart = System.Windows.AssemblyPart()
                  let assemblyStream = this.FileStreamReadShim(fileName)
                  assemblyPart.Load(assemblyStream)
              if System.Windows.Deployment.Current.Dispatcher.CheckAccess() then 
                  load() 
              else
                  let resultTask = System.Threading.Tasks.TaskCompletionSource<System.Reflection.Assembly>()
                  System.Windows.Deployment.Current.Dispatcher.BeginInvoke(Action(fun () -> resultTask.SetResult (load()))) |> ignore
                  resultTask.Task.Result

        default this.AssemblyLoad(assemblyName:System.Reflection.AssemblyName) = 
            try 
               System.Reflection.Assembly.Load(assemblyName.FullName)
            with e -> 
                this.AssemblyLoadFrom(assemblyName.Name + ".dll")
#else
        default this.AssemblyLoadFrom(fileName:string) = System.Reflection.Assembly.LoadFrom fileName
        default this.AssemblyLoad(assemblyName:System.Reflection.AssemblyName) = System.Reflection.Assembly.Load assemblyName
#endif


#if SILVERLIGHT
    open System.IO.IsolatedStorage
    open System.Windows
    open System

    let mutable FileSystem = 
        { new FileSystem() with 
            member __.FileStreamReadShim (fileName:string) = 
                match Application.GetResourceStream(System.Uri(fileName,System.UriKind.Relative)) with 
                | null -> IsolatedStorageFile.GetUserStoreForApplication().OpenFile(fileName, System.IO.FileMode.Open) :> System.IO.Stream 
                | resStream -> resStream.Stream

            member __.FileStreamCreateShim (fileName:string) = 
                System.IO.IsolatedStorage.IsolatedStorageFile.GetUserStoreForApplication().CreateFile(fileName) :> Stream

            member __.GetFullPathShim (fileName:string) = fileName
            member __.IsPathRootedShim (pathName:string) = true
            member __.SafeGetFullPath (fileName:string) = fileName
            member __.IsInvalidFilename(filename:string) = 
                String.IsNullOrEmpty(filename) || filename.IndexOfAny(System.IO.Path.GetInvalidPathChars()) <> -1

            member __.GetTempPathShim() = "." 

            member __.GetLastWriteTimeShim (fileName:string) = 
                match Application.GetResourceStream(System.Uri(fileName,System.UriKind.Relative)) with 
                | null -> IsolatedStorageFile.GetUserStoreForApplication().GetLastAccessTime(fileName).LocalDateTime
                | _resStream -> System.DateTime.Today.Date
            member __.SafeExists (fileName:string) = 
                match Application.GetResourceStream(System.Uri(fileName,System.UriKind.Relative)) with 
                | null -> IsolatedStorageFile.GetUserStoreForApplication().FileExists fileName
                | resStream -> resStream.Stream <> null
            member __.FileDelete (fileName:string) = 
                match Application.GetResourceStream(System.Uri(fileName,System.UriKind.Relative)) with 
                | null -> IsolatedStorageFile.GetUserStoreForApplication().DeleteFile fileName
                | _resStream -> ()
            
          }
#else

    let mutable FileSystem = 
        { new FileSystem() with 
            override __.ReadAllBytesShim (fileName:string) = File.ReadAllBytes fileName
            member __.FileStreamReadShim (fileName:string) = new FileStream(fileName,FileMode.Open,FileAccess.Read,FileShare.ReadWrite)  :> Stream
            member __.FileStreamCreateShim (fileName:string) = new FileStream(fileName,FileMode.Create,FileAccess.Write,FileShare.Read ,0x1000,false) :> Stream
            member __.GetFullPathShim (fileName:string) = System.IO.Path.GetFullPath fileName
            member __.SafeGetFullPath (fileName:string) = 
                //System.Diagnostics.Debug.Assert(Path.IsPathRooted(fileName), sprintf "SafeGetFullPath: '%s' is not absolute" fileName)
                Path.GetFullPath fileName

            member __.IsPathRootedShim (path:string) = Path.IsPathRooted path

            member __.IsInvalidFilename(filename:string) = 
                String.IsNullOrEmpty(filename) || filename.IndexOfAny(Path.GetInvalidFileNameChars()) <> -1

            member __.GetTempPathShim() = System.IO.Path.GetTempPath()

            member __.GetLastWriteTimeShim (fileName:string) = File.GetLastWriteTime fileName
            member __.SafeExists (fileName:string) = System.IO.File.Exists fileName 
            member __.FileDelete (fileName:string) = System.IO.File.Delete fileName }

#endif        

    type System.Text.Encoding with 
        static member GetEncodingShim(n:int) = 
#if SILVERLIGHT
                System.Text.Encoding.GetEncoding(n.ToString())
#else                
                System.Text.Encoding.GetEncoding(n)
#endif                

