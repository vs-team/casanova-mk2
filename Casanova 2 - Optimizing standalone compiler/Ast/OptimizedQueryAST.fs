module OptimizedQueryAST

open System.Collections.Generic
open Common

type List<'a> = 'a list

type Program = 
  {
    Module    : Id
    Imports   : Id list
    World     : World
    Entities  : ResizeArray<Entity>
  } with member this.Position = this.World.Name.idRange

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
    Fields    : Dictionary<Id, TypedAST.Field>
    Rules     : ResizeArray<Rule>
    Create    : Create
  }

and Create =
  {
    Args      : List<Id * TypedAST.TypeDecl>
    mutable Body      : Block
    Position  : Position
  }



and Rule =
  {
    Domain    : Id list
    Body      : Block
    Position  : Position
  } 

and Block = TypedExpression list

and Call =
  | Constructor of TypedAST.TypeDecl * Block
  | Static of TypedAST.TypeDecl * Id * Block
  | Instance of Id * Id * Block
  with member this.Position = match this with | Constructor(t,_) -> t.Position; | Static(t,_,_) -> t.Position; | Instance(id,_,_) -> id.idRange

and MaybeExpr = JustExpr of TypedExpression 
                | NothingExpr of Position
                with member this.Position = 
                                match this with
                                | JustExpr e -> (snd e).Position
                                | NothingExpr p -> p
and MutableExpression = { mutable Expr : TypedExpression}  with member this.Position = (snd this.Expr).Position
and [<ReferenceEquality>] MutableExpressionAndMap = { MutableExpression : MutableExpression; Map : TypedExpression -> TypedExpression } with member this.Position = this.MutableExpression.Position
and Expression =
  | MutableExpression of MutableExpressionAndMap
  | Add of TypedExpression * TypedExpression
  | Sub of TypedExpression * TypedExpression
  | Div of TypedExpression * TypedExpression
  | DoGet of TypedExpression * Id
  | Modulus of TypedExpression * TypedExpression 
  | Mul of TypedExpression * TypedExpression
  
  | Not of TypedExpression
  | And of TypedExpression * TypedExpression
  | Or of TypedExpression * TypedExpression
  | Equals of TypedExpression * TypedExpression
  | Greater of TypedExpression * TypedExpression

  | Set of Id * TypedExpression
  | Lambda of List<Id * Option<TypedAST.TypeDecl>> * Block
  | Maybe of MaybeExpr
  | ConcatQuery of List<TypedExpression>
  | AppendToQuery of TypedExpression * TypedExpression
  | SubtractToQuery of TypedExpression * TypedExpression
  | AddToQuery of TypedExpression * TypedExpression
  | IndexOf of TypedExpression * TypedExpression
  | Choice of bool * (TypedExpression * Block * Position) list * Position
  | Parallel of (Block * Position) list * Position
  | Range of TypedExpression * TypedExpression * Position
  | NewEntity of List<Id * Block>
  | Let of Id * TypedAST.TypeDecl * Option<TypedExpression> * bool // compress let
  | LetWait of Id * TypedAST.TypeDecl * Option<TypedExpression> * bool // compress let

  | IfThenElse of TypedExpression * Block * Block
  | IfThen of TypedExpression * Block * bool // do not suspend when exit
  | Yield of TypedExpression
  | Wait of TypedExpression * Option<TypedExpression>
  | Tuple of Block
  | For of List<Id> * TypedExpression * Block * bool // is atomic loop
  | While of TypedExpression * Block
  | Query of QueryExpression
  | Call of Call
  | ReEvaluateRule of Position
  | Literal of BasicAST.Literal
  | Id of Id
  | Cast of TypedAST.TypeDecl * TypedExpression * Position

  with member this.Position = 
        match this with 
        | ConcatQuery(e) -> (snd e.Head).Position
        | IndexOf(e,_) -> (snd e).Position
        | Lambda(_,_) -> Position.Empty
        | Maybe(e) -> e.Position
        | Cast(_,_,p) -> p
        | MutableExpression e -> e.Position
        | AddToQuery(_,(_,t)) -> t.Position
        | SubtractToQuery(_,(_,t)) -> t.Position
        | Set (id, _) -> id.idRange
        | ReEvaluateRule p -> p
        | Choice(_,cs, p) -> p | Range(_,_,p) -> p 
        | DoGet (_,id) -> id.idRange
        | NewEntity exprs -> 
          match exprs with
          | [] -> raise Position.Empty (sprintf "New entity without body. Internal error at %s(%s)" __SOURCE_FILE__ __LINE__)
          | _ ->(fst exprs.Head).idRange; 
        | Let(id,_,_,_) -> id.idRange; | LetWait(id,_,_,_) -> id.idRange; | IfThenElse(b,_,_) -> (snd b).Position; | IfThen(b,_,_) -> (snd b).Position; 
        | Yield(t) | Wait(t,_) -> (snd t).Position; 
        | Tuple(b) -> 
          match b with
          | [] -> raise Position.Empty (sprintf "Tuple without body. Internal error at %s(%s)" __SOURCE_FILE__ __LINE__)
          | _ ->(snd b.Head).Position; 
        | AppendToQuery (_,(_,t)) -> t.Position
        | For(ids,_,_, _) -> 
          match ids with
          | [] -> raise Position.Empty (sprintf "For without indexes. Internal error at %s(%s)" __SOURCE_FILE__ __LINE__)
          | _ ->ids.Head.idRange; 
        | While(b,_) -> (snd b).Position; | Query(q) -> q.Position; | Call(c) -> c.Position; 
        | Literal(l) -> l.Position;| Id(id) -> id.idRange
        | Add (e, _) | Sub (e, _) | Div (e, _) | Modulus (e, _) | Mul (e, _) | Not e | And (e, _) 
        | Or (e, _) | Equals (e, _) | Greater (e, _) -> (fst e).Position | Parallel(_, p) -> p

and TypedExpression = TypedAST.TypeDecl * Expression


and QueryExpression =
   | QueryStatements of List<InnerQueryExpression> with 
    member this.Position = 
      match this with 
      | QueryStatements q -> 
        match q with
        | [] -> raise Position.Empty (sprintf "Query expression without statements. Internal error at %s(%s)" __SOURCE_FILE__ __LINE__)
        | _ -> q.Head.Position

and InnerQueryExpression =
  | For of Id * TypedAST.TypeDecl * TypedExpression
  | Select of TypedExpression
  | FindBy of TypedExpression
  | Exists of TypedExpression
  | MinBy of TypedExpression
  | ForAll of TypedExpression  
  | Sum of Position
  | Min of Position
  | Max of Position
  | MaxBy of TypedExpression
  | Where of TypedExpression
  | GroupBy of TypedExpression
  | GroupByInto of TypedExpression * Id
  | Let of Id * TypedAST.TypeDecl * TypedExpression
  | Empty of TypedAST.TypeDecl * Position
  | LiteralList of List<TypedExpression> 
  with member this.Position = 
    match this with  
    | For (id,_,_) -> id.idRange; | Select(t) -> (fst t).Position; | FindBy(b) | Exists(b) | Where(b) -> (snd b).Position; | MinBy (e) | MaxBy (e) | GroupBy (e) -> (fst e).Position; | GroupByInto(_, id) -> id.idRange; | Let(ids,_,_) -> ids.idRange; 
    | Empty(t,p) -> p; 
    | Max p -> p
    | LiteralList(es) -> 
      match es with
      | [] -> raise Position.Empty (sprintf "LiteralList without body. Internal error at %s(%s)" __SOURCE_FILE__ __LINE__)
      | _ -> (fst es.Head).Position
