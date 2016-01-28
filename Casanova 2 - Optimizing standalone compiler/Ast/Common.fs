module Common

open Microsoft.FSharp.Text.Lexing
open System

let is_running_unity = false
let is_running_lego = false
let enable_dependency_analysis = false
let enable_query_optimization = false
let run_profiler = false
let run_debugger = false
let print_compilation_times = false

type [<CustomComparison; CustomEquality>] Position = Position of  Microsoft.FSharp.Text.Lexing.Position
  with
    override this.ToString() = 
      let (Position(p)) = this
      "(" + string p.Line + ":" + string p.Column + ")" 

    static member Empty = Position Microsoft.FSharp.Text.Lexing.Position.Empty
    interface IComparable<Position> with
      member this.CompareTo _ = 0
    interface IComparable with
        member this.CompareTo obj = 0
    interface IEquatable<Position> with
        member this.Equals _ = true
    override this.Equals obj = true
    override this.GetHashCode () = 0


 

exception CompilationError of Position * string

let raise position msg =  
  raise(CompilationError(position, msg))

let failwith = raise
    
let failwithf = raise


type [<CustomComparison; CustomEquality>]
    Id = 
      { idText : string; 
        idRange : Position } 
            override this.ToString() = "Id: " + this.idText + "  Pos: " + this.idRange.ToString()
            
            interface IComparable<Id> with
              member this.CompareTo { idText = text; idRange = range } =
                  compare this.idText text
            interface IComparable with
                member this.CompareTo obj =
                    match obj with
                    | :? Id as other -> (this :> IComparable<_>).CompareTo other
                    | _                    -> invalidArg "obj" "not a Category"
            interface IEquatable<Id> with
                member this.Equals { idText = text; idRange = range } = this.idText = text
            override this.Equals obj =
                match obj with
                | :? Id as other -> (this :> IEquatable<_>).Equals other
                | _                    -> invalidArg "obj" "not a Category"
            override this.GetHashCode () = hash this.idText



