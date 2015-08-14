module TypedToMetaCompiler
//
//open System
//open Expr
//open TypedAST
//
////meta-compiler global keywords
//let emptyList = !!"nil"
//
//
////TRAVERSE CODE
//let rec compose (operator : string) (_base : Expr<string>) (exprs : Expr<string> list)  =
//  match exprs with
//  | [e1;e2] -> Composition(operator, [Composition(operator,[e1;e2]);_base])
//  | expr :: exprs -> Composition(operator, [expr;(compose operator _base exprs)])
//  | _ -> _base
//
//let rec convertProgram (program : TypedAST.Program) =
//  let world = program.World
//  let entities = program.Entities
//  let convertedImports =
//    [for import in program.Imports do
//        yield convertId import]
//  let convertedWorld = convertWorld world
//  let convertedEntities = 
//    [for e in entities do
//        yield convertEntity e]
//  let convertedPos = ExternalType({Value = (program.Position)})
//  Composition("program",[compose ";" emptyList convertedImports;convertedWorld;compose ";" emptyList convertedEntities;convertedPos])
//
//
//and convertWorld (world : TypedAST.World) : Expr<string> =
//  let worldId = convertId world.Name
//  let body = convertBody world.Body
//  let convertedPos = ExternalType({Value = (world.Position)})
//  Composition("world",[worldId;body;convertedPos])
//
//  
//and convertEntity (entity : TypedAST.Entity) : Expr<string> =
//  let entityId = convertId entity.Name
//  let body = convertBody entity.Body
//  let convertedPos = ExternalType({Value = (entity.Position)})
//  Composition("entity",[entityId;body;convertedPos])
//
//and convertBody (body : TypedAST.EntityBody) : Expr<string> =
//  let convertedFields = 
//    [for f in body.Fields do
//        yield convertField f.Value]
//  let convertedRules = 
//    [for r in body.Rules do
//        yield convertRule r]
//  let convertedCons = convertCons body.Create
//  let fieldComposition = convertedFields |> compose ";" emptyList
//  let ruleComposition = convertedRules |> compose ";" emptyList
//  Composition(",",[Composition(",",[fieldComposition;ruleComposition]);convertedCons])
//
//and convertRule (rule : TypedAST.Rule) : Expr<string> =
//  let convertedFields =
//    [for f in rule.Domain do
//        yield convertId f]
//  let convertedBlock = convertBlock rule.Body
//  let convertedPos = ExternalType({Value = (rule.Position)})
//  let fieldComposition = convertedFields |> compose ";" emptyList
//  Composition("rule",[fieldComposition;convertedBlock;convertedPos])
//
//and convertCons (cons : TypedAST.Create) : Expr<string> =
//  let convertedArgs =
//    [for arg in cons.Args do
//      let id,t = arg
//      yield Composition(",",[convertId id;convertTypeDecl t])]
//  let convertedBody = convertBlock cons.Body
//  let argsComposition = convertedArgs |> compose ";" emptyList
//  let convertedPos = ExternalType({Value = cons.Position})
//  Composition("create",[argsComposition;convertedBody;convertedPos])
//
//
//and convertField (field : TypedAST.Field) : Expr<string> =
//  let fieldId = convertId field.Name
//  let convertedType = convertTypeDecl field.Type
//  let convertedPos = ExternalType({Value = field.Position})
//  if field.IsReference then
//    Composition("ref",[Composition(",",[Composition(":",[fieldId;convertedType]);convertedPos])])
//  else
//    Composition(",",[Composition(":",[fieldId;convertedType]);convertedPos])
//
//and convertId (id : Common.Id) : Expr<string> =
//  let convertedRange = ExternalType({Value = id.idRange})
//  let idMap = Microsoft.FSharp.Collections.Map([(String "idText",!(id.idText));(String "idRange",convertedRange)] |> List.toSeq)
//  Map({Reified = idMap;Computed = None})
//
//and convertTypeDecl (t : TypedAST.TypeDecl) : Expr<string> =
//  let convertedRange = ExternalType({Value = t.Position})
//  let typeDeclMap = Map({
//                          Reified = Microsoft.FSharp.Collections.Map(
//                                      [
//                                        (String "isGeneric",Bool t.IsGeneric);
//                                        (String "isSystemType",Bool t.IsSystemType);
//                                        (String "Position",convertedRange);
//                                        (String "TypeName",String t.TypeName)
//                                      ])
//                          Computed = None
//                        })
//
//  match t with
//  | EntityName id -> Composition(",",[convertId id;typeDeclMap])
//  | ImportedType(t1,pos) ->
//      let convertedPos = ExternalType {Value = pos}
//      let convertedType = ExternalType {Value = t1}
//      Composition(",",[Composition(",",[typeDeclMap;convertedType]);convertedPos])
//  | UnionType(typeDecls) ->
//      let convertedTypes =
//        [for t1 in typeDecls do
//            yield convertTypeDecl t1]
//      Composition(",",[typeDeclMap;convertedTypes |> compose ";" emptyList])
//  | TypeDecl.Tuple(typeDecls) ->
//      let convertedTypes =
//        [for t1 in typeDecls do
//            yield convertTypeDecl t1]
//      Composition(",",[typeDeclMap;convertedTypes |> compose ";" emptyList])
//  | TypeDecl.Query(t1) ->
//      Composition(",",[typeDeclMap;convertTypeDecl t1])
//  | TypeDecl.MaybeType(mt) ->
//      Composition(",",[typeDeclMap;convertMaybeType mt])
//  | Unit(pos) ->
//      Composition(",",[typeDeclMap;ExternalType {Value = pos}])
//  | GenericType(t1,tds) ->
//      let c_t1 = convertTypeDecl t1
//      let convertedTypes =
//        [for td in tds do
//            yield convertTypeDecl td]
//      let composedTypeDecls = convertedTypes |> compose ";" emptyList
//      Composition(",",[Composition(",",[typeDeclMap;c_t1]);composedTypeDecls])
//      
//   
//and convertMaybeType (mt : MaybeType) : Expr<string> =
//  match mt with
//  | Just t -> convertTypeDecl t
//  | Nothing(gt,pos) ->
//      match gt.Value.Type with
//      | Some tDecl -> 
//          Composition(",",[convertTypeDecl tDecl;ExternalType {Value = pos}])
//      | None -> failwith "Translation to Meta Compiler failed: Option genericType must be instantiated at this point"
//
//
//and convertBlock (block : TypedAST.Block) : Expr<string> =
//  let convertedTypedExpr =
//    [for typedExpr in block do
//        yield convertTypedExpr typedExpr]
//  convertedTypedExpr |> compose ";" emptyList
//
//and convertTypedExpr (typedExpr : TypedExpression) : Expr<string> =
//  let typeDecl,expr = typedExpr
//  Composition(",",[convertTypeDecl typeDecl;convertExpr expr])
//
//and convertExpr (expr : Expression) : Expr<string> =
//  let exprPos = ExternalType {Value = expr.Position}
//  match expr with
//  | Add(te1,te2) ->
//      Composition(",",[Composition("+",[convertTypedExpr te1;convertTypedExpr te2]);exprPos])
//  | Sub(te1,te2) ->
//      Composition(",",[Composition("-",[convertTypedExpr te1;convertTypedExpr te2]);exprPos])
//  | Div(te1,te2) ->
//      Composition(",",[Composition("/",[convertTypedExpr te1;convertTypedExpr te2]);exprPos])
//  | Mul(te1,te2) ->
//      Composition(",",[Composition("*",[convertTypedExpr te1;convertTypedExpr te2]);exprPos])
//  | Not(te) ->
//      Composition(",",[Composition("!",[convertTypedExpr te]);exprPos])
//  | And(te1,te2) ->
//      Composition(",",[Composition("&&",[convertTypedExpr te1;convertTypedExpr te2]);exprPos])
//  | Or(te1,te2) ->
//      Composition(",",[Composition("||",[convertTypedExpr te1;convertTypedExpr te2]);exprPos])
//  | Equals(te1,te2) ->
//      Composition(",",[Composition("=",[convertTypedExpr te1;convertTypedExpr te2]);exprPos])
//  | Greater(te1,te2) ->
//      Composition(",",[Composition(">",[convertTypedExpr te1;convertTypedExpr te2]);exprPos])
//  | Lambda(args,block) ->
//      let convertedArgs =
//        [for id,tdOpt in args do
//            match tdOpt with
//            | Some td -> yield Composition(",",[convertId id;convertTypeDecl td])
//            | None -> yield convertId id
//        ]
//      let composedArgs = convertedArgs |> compose ";" emptyList
//      let convertedBlock = convertBlock block
//      Composition(",",[Composition("fun",[composedArgs;convertedBlock]);exprPos])
//  | Maybe(maybeExpr) ->
//      match maybeExpr with
//      | JustExpr(te) -> Composition(",",[Composition("Just",[convertTypedExpr te]);exprPos])
//      | NothingExpr(pos) -> Composition(",",[Composition("Nothing",[ExternalType {Value = pos}]);exprPos])
//  | ConcatQuery(typedExprs) ->
//      let convertedExprs =
//        [for typedExpr in typedExprs do
//            yield convertTypedExpr typedExpr]
//      let composedExprs = convertedExprs |> compose ";" emptyList
//      Composition(",",[Composition("concatQuery",[composedExprs]);exprPos])
//  | AppendToQuery(te1,te2) ->
//      Composition(",",[Composition("addToQuery",[convertTypedExpr te1;convertTypedExpr te2]);exprPos])
//  | IndexOf(id,te) ->
//      Composition(",",[Composition("indexOf",[convertId id;convertTypedExpr te]);exprPos])
//  | Choice(interruptible,args,_) ->
//      let convertedArgs =
//        [for te,block,pos in args do
//            yield Composition(",",[Composition(",",[convertTypedExpr te;convertBlock block]);ExternalType {Value = pos}])]
//      let composedArgs = convertedArgs |> compose ";" emptyList
//      if interruptible then
//        Composition(",",[Composition("!|",[composedArgs]);exprPos])
//      else
//        Composition(",",[Composition(".!",[composedArgs]);exprPos])
//  | Parallel(args,_) ->
//      let convertedArgs =
//        [for block,pos in args do
//            yield Composition(",",[convertBlock block;ExternalType {Value = pos}])]
//      let composedArgs = convertedArgs |> compose ";" emptyList
//      Composition(",",[Composition(".&",[composedArgs]);exprPos])
//        
//
//
//
//
//
//  