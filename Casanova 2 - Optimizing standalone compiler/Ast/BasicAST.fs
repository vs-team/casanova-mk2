module BasicAST
open System
open Common

type Program = 
  {
    Module    : Id
    ReferencedLibraries : seq<System.Type>
    Imports   : List<Id>
    World     : World
    Entities  : List<Entity>
  } with 
      member this.Position = this.World.Name.idRange


and World = 
  {
    Name      : Id
    Body      : EntityBody
  } 
    with 
      member this.Position = this.Name.idRange
      member this.Rebuild new_body =
          {
            Name      = this.Name
            Body      = new_body
          } 
      

and Entity = 
  {
    Name      : Id
    Body      : EntityBody
  } 
    with 
      member this.Position = this.Name.idRange
      member this.Rebuild new_body =
        {
          Name      = this.Name
          Body      = new_body
        } 

and EntityBody = 
  {
    Inherits  : List<Id>
    Fields    : List<Field>
    Rules     : List<Rule>
    Create    : Create
  }

and Create =
  {
    Args      : List<Id * Option<TypeDecl>>
    Body      : Block
    Position  : Position
  } with member this.Rebuild(new_body) =
                        {
                          Args      = this.Args
                          Body      = new_body
                          Position  = this.Position
                        } 

and Field = 
  {
    Name            : Id
    Type            : TypeDecl    
    IsReference     : bool
    IsExternal      : Option<Id * bool * bool> // container * public get * public set 
  } with member this.Position = this.Name.idRange

and TypeDecl = | TypeName of Id | Imported of Id * TypeDecl | UnionType of List<TypeDecl> | Tuple of List<TypeDecl> 
               | TypeNothing of Position | TypeMaybe of TypeDecl 
               | Query of TypeDecl  
               | Unit of Position
  with member this.Position = match this with | TypeName(id) -> id.idRange; | Imported(id,_) -> id.idRange; 
                                              | UnionType(ds) -> 
                                                match ds with
                                                | [] -> raise Position.Empty (sprintf "Union type error. Internal error at %s(%s)" __SOURCE_FILE__ __LINE__)
                                                | _ ->
                                                  ds.Head.Position; 
                                              | Tuple(ds) -> 
                                                if ds.Length = 0 then raise Position.Empty (sprintf "Tuple type without body. Internal error at %s(%s)" __SOURCE_FILE__ __LINE__)
                                                ds.Head.Position; | Query(d) -> d.Position; | Unit(p) -> p
                                              | TypeNothing p -> p | TypeMaybe t -> t.Position
and Rule =
  {
    Domain    : List<Id>
    Body      : Block
    Flags     : CasanovaCompiler.ParseAST.Flag list
  } 
    with member this.Position = 
              match this.Domain with
              | [] -> raise Position.Empty (sprintf "Internal error at %s(%s). Rule without domain." __SOURCE_FILE__ __LINE__)
              | _ -> this.Domain.Head.idText

and Block = List<Expression>

and Call =
  | Static of TypeDecl * Option<Id> * Block
  | MaybeInstance of Id * Id * Block
  with member this.Position = match this with | Static(t,_,_) -> t.Position | MaybeInstance(id,_,_) -> id.idRange

and Maybe = Just of Expression | Nothing of Position
  with member this.Position = match this with Just e -> e.Position | Nothing p -> p

and Expression = 
  | Add of Expression * Expression
  | Sub of Expression * Expression
  | Div of Expression * Expression
  | Modulus of Expression * Expression 
  | Mul of Expression * Expression
  
  | Not of Expression
  | And of Expression * Expression
  | Or of Expression * Expression
  | Equals of Expression * Expression
  | Greater of Expression * Expression

  
  | ConcatQuery of List<Expression>
  | AppendToQuery of Expression * Expression
  | Maybe of Maybe
  | NewEntity of List<Id * Block>
  | Range of Expression * Expression * Position
  | Let of Id * Expression
  | LetWait of Id * Expression
  | IfThenElse of Expression * Block * Block
  | IndexOf of Id * Expression
  | IfThen of Expression * Block
  | Yield of Expression
  | Wait of Expression
  | Choice of bool * (Expression * Block * Position) list
  | Parallel of (Block * Position) list
  | Tuple of List<Block>
  | For of List<Id> * Expression * Block
  | While of Expression * Block
  | Query of QueryExpression 
  | Call of Call
  | Cast of TypeDecl * Expression * Position
  | Lambda of List<Id * Option<TypeDecl>> * Block
  | New of TypeDecl * Block
  | Literal of Literal
  | Id of Id
  with member this.Position = 
          match this with 
                          | LetWait(id, _) -> id.idRange
                          | Cast(i,e,p) -> p
                          | Parallel(ps) -> 
                            match ps with
                            | [] -> raise Position.Empty (sprintf "Parallel without body. Internal error at %s(%s)" __SOURCE_FILE__ __LINE__)
                            | _ ->let _,p = ps.Head in p  
          
                          | Choice(_,cs) -> 
                            match cs with
                            | [] -> raise Position.Empty (sprintf "Choice without body. Internal error at %s(%s)" __SOURCE_FILE__ __LINE__)
                            | _ -> let c,_,_ = cs.Head in c.Position 
                          | Range(_,_,p) -> p | Let(id,_) -> id.idRange; | IfThenElse(b,_,_) -> b.Position; | IfThen(b,_) -> b.Position; | Yield(e) | Wait(e) -> e.Position; 
                          | Tuple(e) -> 
                            if e.Length = 0 then raise Position.Empty (sprintf "Tuple without body. Internal error at %s(%s)" __SOURCE_FILE__ __LINE__)
                            e.Head.Head.Position; 
                          | Query(q) -> 
                            if q.Length = 0 then raise Position.Empty (sprintf "Query without body. Internal error at %s(%s)" __SOURCE_FILE__ __LINE__)
                            q.Head.Position; 
                          | NewEntity(ids) -> 
                            if ids.Length = 0 then raise Position.Empty (sprintf "New entity without body. Internal error at %s(%s)" __SOURCE_FILE__ __LINE__)
                            (fst ids.Head).idRange
                          | For(_,e,_) -> e.Position; | While(b,_) -> b.Position 
                          | Call(c) -> c.Position; | New(t,_) -> t.Position; | Literal(l) -> l.Position; | Id(id) -> id.idRange 
                          | Maybe(e) -> e.Position
                          | Add (e, _) | Sub (e, _) | Div (e, _) | Modulus (e, _) | Mul (e, _) | Not e | And (e, _) 
                          | Or (e, _) | Equals (e, _) | Greater (e, _) -> e.Position
                          | ConcatQuery(es) -> 
                            match es with
                            | [] -> raise Position.Empty (sprintf "Concat query without body. Internal error at %s(%s)" __SOURCE_FILE__ __LINE__)
                            | _ -> es.Head.Position
                          | IndexOf (id, e) -> id.idRange 
                          | AppendToQuery(e1,_) -> e1.Position

and Literal = String of string * Position | Int of int * Position | Float of float32 * Position | Bool of bool * Position | LUnit of Position
  with member this.Position = match this with | String(_,p) | Int(_,p) | Float(_,p) | Bool(_,p) | LUnit(p) -> p
       member this.Name = match this with | String(s,_) -> "\"" + s + "\""; | Int(i,_) -> string i; | Float(f,_) -> string f + "f"; | Bool(true,_) -> "true"; | Bool(false,_) -> "false"; | LUnit(p) -> "unit"
         
and QueryExpression = List<InnerQueryExpression>   
  
and InnerQueryExpression =
  | Empty of Option<TypeDecl> * Position
  | LiteralList of List<Expression>
  | QueryFor of List<Id> * Expression
  | Select of Expression
  | MinBy of Expression
  | Sum of Position
  | Min of Position
  | Max of Position
  | GroupBy of Expression
  | MaxBy of Expression
  | FindBy of Expression
  | Exists of Expression  
  | ForAll of Expression  
  | Where of Expression
  | GroupByInto of Expression * Id
  | Let of Id * Expression
  with member this.Position = 
    match this with 
    | Empty(t, p) -> p; 
    | Max p -> p
    | LiteralList(b) -> 
      match b with
      | [] -> raise Position.Empty (sprintf "Literals without body. Internal error at %s(%s)" __SOURCE_FILE__ __LINE__)
      | _ ->b.Head.Position     
    | QueryFor(_,e) -> e.Position | Select(e) | MinBy(e) | GroupBy(e) | MaxBy(e) -> e.Position | FindBy(b) | Exists(b) | Where(b) -> b.Position | GroupByInto(e,_) -> e.Position | Let(id,_) -> id.idRange | Sum p -> p | Min p -> p

