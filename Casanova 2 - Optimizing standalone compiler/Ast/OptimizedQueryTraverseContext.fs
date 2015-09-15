module OptimizedQueryTraverseContext

type ExpressionContext =
  {
    CurrentEntity : Common.Id * OptimizedQueryAST.EntityBody
    Type : TypedAST.TypeDecl
    Expression : TypedAST.Expression
    GlobalContext : GlobalContext
    CurrentRule : int
    WorldName : OptimizedQueryAST.World
    OptimizeQuery : bool
    Action : OptimizedQueryAST.TypedExpression -> OptimizedQueryAST.TypedExpression 
  } with
  member this.TypedExpression = this.Type, this.Expression
  static member Empty(currentEntity, world_name, globalContext, p) =
    {
      CurrentEntity = currentEntity
      Type = TypedAST.Unit(p)
      Expression = TypedAST.Expression.Literal(BasicAST.Bool(false, p))
      GlobalContext = globalContext
      CurrentRule = -1
      WorldName = world_name
      OptimizeQuery = false
      Action = id
    }

  static member Build(currentEntity, globalContext, (t, e) : TypedAST.TypedExpression, current_rule, optimize_query, action) =
     {
      CurrentEntity = currentEntity
      OptimizeQuery = optimize_query 
      GlobalContext = globalContext
      Type = t
      Expression = e
      CurrentRule = current_rule
      WorldName = globalContext.WorldName
      Action = action
     }

  static member Build(currentEntity, globalContext, (t, e) : TypedAST.TypedExpression, current_rule) =
     ExpressionContext.Build(currentEntity, globalContext, (t, e), current_rule, true, id)

  static member Build(currentEntity, globalContext, (t, e) : TypedAST.TypedExpression, current_rule, action : OptimizedQueryAST.TypedExpression -> OptimizedQueryAST.TypedExpression) =
     ExpressionContext.Build(currentEntity, globalContext, (t, e), current_rule, true, action)

  static member Build(currentEntity, globalContext, (t, e) : TypedAST.TypedExpression, current_rule, optimize_query : bool) =
     ExpressionContext.Build(currentEntity, globalContext, (t, e), current_rule, optimize_query, id)

  static member Build(prev_context : ExpressionContext, (t,e) : TypedAST.TypedExpression, optimize_query : bool) =
    let q1 =  ExpressionContext.Build(prev_context, (t,e))
    {q1 with OptimizeQuery = optimize_query}

  static member Build(prev_context : ExpressionContext, e : TypedAST.Expression) =
    {prev_context with Expression = e}
  static member Build(prev_context : ExpressionContext, (t,e) : TypedAST.TypedExpression) =
    {prev_context with Expression = e; Type = t}
  
     
and GlobalContext =
  {
    Table : Map<Common.Id, OptimizedQueryAST.EntityBody>
    WorldName : OptimizedQueryAST.World
    OptimizedExpressions : System.Collections.Generic.Dictionary<Common.Id, System.Collections.Generic.Dictionary<Common.Id, OptimizedQueryAST.MutableExpression * int>>

  }

//let (|Even|Odd|) input = if input % 2 = 0 then Even else Odd
//
//let testNumber n =
//  match n with
//  | Even -> printfn "Even!"
//  | Odd -> printfn "Odd!"
//
//testNumber 1
//testNumber 2
//testNumber 3
// 


//let (|Integer|_|) (str : string) =
//  let mutable n = 0
//  if System.Int32.TryParse(str, &n) then Some n
//  else None
//
//let (|Float|_|) (str : string) =
//  let mutable dn = 0.0
//  if System.Double.TryParse(str, &dn) then Some dn
//  else None
//
//let parseNumeric str =
//  match str with
//  | Integer i -> printf "Integer: %d" i
//  | Float f -> printf "Integer: %f" f
//  | _ -> printf "no match"
//
//parseNumeric "1.0"
//parseNumeric "1"
