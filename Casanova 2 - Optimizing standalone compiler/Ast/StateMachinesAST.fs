module StateMachinesAST
open Common

type Program = 
  {
    Module    : Id
    Imports   : List<Id>
    World     : World
    Entities  : List<Entity>
  } with member this.Position = this.World.Position

and World = 
  {
    Name      : Id
    Body      : EntityBody
  } with member this.Position = this.Name.idRange

and Entity = 
  {
    Name      : Id
    Body      : EntityBody
  } with member this.Position = this.Name.idRange

and EntityBody = 
  {
    Fields              : List<TypedAST.Field>
    Rules               : List<Rule>
    ParallelMethods     : System.Collections.Generic.List<RuleBody * string>
    ConcurrentMethods   : System.Collections.Generic.List<RuleBody * string * string * string>
    Create              : Create
  }

and Create =
  {
    Args      : List<Id * TypeDecl>
    Body      : AtomicBlock
    Position  : Position
  }

and TypeDecl = TypedAST.TypeDecl
and Rule = 
  {
    Domain    : List<Id * Id * bool * Id> // destination_entity * destination_field * is_external_type * full_path
    Body      : RuleBody
    Index     : int
    Position  : Position
    Flags     : CasanovaCompiler.ParseAST.Flag list
  } 

and RuleBody = Atomic of AtomicBlock | StateMachine of StateMachine

and StateMachine =
  {
    Dependencies :  Option<((Id * Id * string) list list) * OptimizedQueryAST.TypedExpression>
    Body    : Block
  }
and Label = {mutable RefCounter : int; Id : int; Position : Position} 
  with static member Create(id, p) = {RefCounter = 0; Id = id; Position = p} 
       member this.AddRef() = this.RefCounter <- this.RefCounter + 1
       member this.RemoveRef() = this.RefCounter <- this.RefCounter - 1

and Goto = {Label : Label} 
  with static member Create(lb : Label) = 
        lb.AddRef(); {Label = lb}

and GotoSuspend = {Label : Label} 
  with static member Create(lb : Label) = 
        lb.AddRef(); {Label = lb} 


and Call = 
  | Constructor of TypeDecl * List<AtomicTypedExpression>
  | ConstructorNoArgs of TypeDecl
  | Static of TypeDecl * Id * List<AtomicTypedExpression>
  | StaticNoArgs of TypeDecl * Id
  | Instance of Id * Id * List<AtomicTypedExpression>
  with member this.Position = 
        match this with | Constructor(t,_) -> t.Position; | Static(t,_,_) -> t.Position; | Instance(id,_,_) -> id.idRange; | ConstructorNoArgs t -> t.Position; | StaticNoArgs (t,_) -> t.Position
     

and AtomicBlock = List<AtomicTypedExpression> * AtomicTypedExpression

and AtomicTypedExpression = OptimizedQueryAST.TypedExpression

and AtomicQueryExpression = OptimizedQueryAST.QueryExpression 

and MaybeExpr = JustExpr of TypedExpression 
                | NothingExpr of Position
and Block = List<TypedExpression>
and TypedExpression = TypeDecl * Expression
and Expression =
  | AtomicFor of Id * TypedExpression * TypedExpression * Block
  | Incr of TypedExpression
  | Label of Label
  | GotoSuspend of GotoSuspend
  | Goto of Goto
  | Var of Id * TypeDecl * Option<TypedExpression>
  | Set of Id * TypedExpression
  | SetExpression of TypedExpression * TypedExpression
  | IndexOf of TypedExpression * TypedExpression
  
  | Add of TypedExpression * TypedExpression
  | Sub of TypedExpression * TypedExpression
  | Div of TypedExpression * TypedExpression
  | Modulus of TypedExpression * TypedExpression 
  | Mul of TypedExpression * TypedExpression
  
  | Not of TypedExpression
  | And of TypedExpression * TypedExpression
  | Or of TypedExpression * TypedExpression
  | Equals of TypedExpression * TypedExpression
  | Greater of TypedExpression * TypedExpression
  
  | ConcatQuery of List<TypedExpression>
  | AppendToQuery of TypedExpression * TypedExpression
  
  | Maybe of MaybeExpr
  | Range of TypedExpression * TypedExpression * Position
  | NewEntity of List<Id * Block>
  | Let of Id * TypeDecl * Option<TypedExpression>
  | LetIfThenElse of Option<Id> * TypeDecl * TypedExpression * Block * Block
  | LetIfThen of Id * TypeDecl * TypedExpression * Block
  | IfThenElse of TypedExpression * Option<TypedExpression> * Block * Block
  | IfThen of TypedExpression * Block
  | Tuple of Block
  | Query of AtomicQueryExpression
  | Call of Call
  | DoGet of TypedExpression * Id
  | Literal of Literal
  | Id of Id

  | Cast of TypeDecl * TypedExpression * Position

  //NETWORKING
  | SendReliable of TypedAST.TypeDecl * TypedExpression * Position
  | Send of TypedAST.TypeDecl * TypedExpression * Position
  | ReceiveMany of TypedAST.TypeDecl * Position
  | Receive of TypedAST.TypeDecl * Position

  with member this.Position = 
      match this with 
      | SendReliable (_,_,p) -> p
      | Send (_,_,p) -> p
      | ReceiveMany (_, p) -> p
      | Receive (_,p) -> p

      | AtomicFor (i, c, u,_) -> i.idRange
      | Incr (_,e) -> e.Position
      | Range(_,_,p) -> p | Label l -> l.Position; | GotoSuspend g -> g.Label.Position; | Goto g -> g.Label.Position; | Var (id,_,_) -> id.idRange; | Set (id,_) -> id.idRange; | IndexOf (id,_) -> (snd id).Position; 
      | NewEntity id_blocks -> 
        match id_blocks with
        | [] -> raise Position.Empty (sprintf "New entity without body. Internal error at %s(%s)" __SOURCE_FILE__ __LINE__)
        | _ -> (fst id_blocks.Head).idRange; 
      | Let (id ,_,_) -> id.idRange; 
      | Tuple b -> 
        match b with
        | [] -> raise Position.Empty (sprintf "Typle without body. Internal error at %s(%s)" __SOURCE_FILE__ __LINE__)
        | _ -> (fst b.Head).Position; 
      | Query q -> q.Position; | Call c -> c.Position; | Literal l -> l.Position; | Id id -> id.idRange      

and Literal = BasicAST.Literal