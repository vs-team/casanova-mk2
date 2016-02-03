module StateMachinesToIntermediate
open Common
open IntermediateContext
open IntermediateAST

//wait/for/ vars *remember rule name index in set expression
//
let rec private traverseGenericEntity is_world_class (entity_name : Id) (entity_body : StateMachinesAST.EntityBody) (table : EntityTable) (context : DependencyContext) : Class =

  {
    IsWorldClass  = is_world_class
    Name = entity_name;
    Body =
      {
        Fields                      = ResizeArray()
        AtomicRules                 = ResizeArray()
        StateMachineRules           = ResizeArray()
        ParallelMethods             = ResizeArray()
        ConcurrentMethods           = ResizeArray()
        SuspendedRules              = ResizeArray()
        Create                      =
          {
            Args      = entity_body.Create.Args
            Body      = entity_body.Create.Body
            Position  = entity_body.Create.Position 
          }
      }
  }

and traverseAtomicRules (classes : Class list) (_class : Class) (entity_body : StateMachinesAST.EntityBody) (dependencies : IntermediateAST.DataDependencies) (table : EntityTable) (context : DependencyContext) =
  let rules = entity_body.Rules 
  let let_to_add : ResizeArray<StateMachinesAST.TypeDecl*Id> = ResizeArray()
  let suspended_rules, automated_rules, state_machine_rules = ResizeArray<SuspendedRule>(), ResizeArray<AutomatedRule>(), ResizeArray<StateMachineRule>()

  for r in rules do     
    match r.Body with 
    | StateMachinesAST.Atomic(a) -> automated_rules.Add({ Domain = r.Domain |> List.map(fun (_,_,_,id) -> id); 
                                                          Id = {idText = string r.Index; idRange = r.Position}; Body = a; Flags = r.Flags})
    | _ ->  ()

  let fields = ResizeArray()
  _class.Body.Fields.AddRange(          
            [for let_tp ,let_id in let_to_add do
              yield
                {
                  Name              = let_id
                  IsStatic          = false
                  Type              = let_tp
                  UpdateNotificationsOnChange = false
                  Update            = false
                  IsReference       = true
                  IsExternal        = None
                  CodeToInjectOnSet  = ""
                  UpdateField   = None
                }])

  _class.Body.AtomicRules.AddRange(automated_rules)

and traverseInterruptibleChoice (classes : Class list) (_class : Class) (entity_body : StateMachinesAST.EntityBody) (dependencies : IntermediateAST.DataDependencies) (table : EntityTable) (context : DependencyContext) =
  let rules = entity_body.ConcurrentMethods
  let let_to_add : ResizeArray<StateMachinesAST.TypeDecl*Id> = ResizeArray()
  let suspended_rules, automated_rules, state_machine_rules = ResizeArray<SuspendedRule>(), ResizeArray<AutomatedRule>(), ResizeArray<StateMachineRule>()

  for r,id,_done,current_state in rules do
    match r with     
    | StateMachinesAST.Atomic(b) -> raise (Position.Empty) "..."
    | StateMachinesAST.StateMachine(b) -> if b.Body.Length = 0 then raise Position.Empty (sprintf "Internal error at %s(%s)." __SOURCE_FILE__ __LINE__) |> ignore
                                          _class.Body.ConcurrentMethods.Add({idText = id; idRange = (fst b.Body.Head).Position},
                                                                            {idText = _done; idRange = (fst b.Body.Head).Position},
                                                                            {idText = current_state; idRange = (fst b.Body.Head).Position},
                                                                            traverseBlock let_to_add (b.Body))

  let fields = ResizeArray()
  _class.Body.Fields.AddRange(          
            [for let_tp ,let_id in let_to_add do
              yield
                {
                  Name              = let_id
                  IsStatic          = false
                  Type              = let_tp
                  Update            = false
                  IsReference       = true
                  IsExternal        = None
                  UpdateNotificationsOnChange = false                  
                  CodeToInjectOnSet  = ""
                  UpdateField   = None
                }])

and traverseParallelMethods (classes : Class list) (_class : Class) (entity_body : StateMachinesAST.EntityBody) (dependencies : IntermediateAST.DataDependencies) (table : EntityTable) (context : DependencyContext) =
  let rules = entity_body.ParallelMethods
  let let_to_add : ResizeArray<StateMachinesAST.TypeDecl*Id> = ResizeArray()
  let suspended_rules, automated_rules, state_machine_rules = ResizeArray<SuspendedRule>(), ResizeArray<AutomatedRule>(), ResizeArray<StateMachineRule>()

  for r,id in rules do
    match r with     
    | StateMachinesAST.Atomic(b) -> raise (Position.Empty) "..." 
    | StateMachinesAST.StateMachine(b) -> 
      if b.Body.Length = 0 then raise Position.Empty (sprintf "Internal error at %s(%s)." __SOURCE_FILE__ __LINE__) |> ignore
      _class.Body.ParallelMethods.Add({idText = id; idRange = (fst b.Body.Head).Position}, traverseBlock let_to_add (b.Body))

  let fields = ResizeArray()
  _class.Body.Fields.AddRange(          
            [for let_tp ,let_id in let_to_add do
              yield
                {
                  UpdateNotificationsOnChange = false
                  IsStatic          = false
                  Name              = let_id
                  Type              = let_tp
                  Update            = false
                  IsReference       = true
                  IsExternal        = None
                  CodeToInjectOnSet = ""
                  UpdateField   = None
                }])




and traverseSuspRules (classes : Class list) (_class : Class) (entity_body : StateMachinesAST.EntityBody) (dependencies : IntermediateAST.DataDependencies) (table : EntityTable) (context : DependencyContext) (ruleCodomains : RuleCodomains) (complete_dependencies : IntermediateAST.DataDependencies) =
  let rules = entity_body.Rules 
  let let_to_add : ResizeArray<StateMachinesAST.TypeDecl*Id> = ResizeArray()
  let suspended_rules, automated_rules, state_machine_rules = ResizeArray<SuspendedRule>(), ResizeArray<AutomatedRule>(), ResizeArray<StateMachineRule>()


  //and then the others
  for r in rules do 
    match r.Body with 
    | StateMachinesAST.StateMachine(s) ->
      match s.Dependencies with 
      | Some (d,_) when d = [] -> 
        state_machine_rules.Add({ Id = {idText = string r.Index; idRange = r.Position}; Body = traverseBlock let_to_add s.Body; Domain = r.Domain|> List.map(fun (_,_,_,id) -> id); Flags = r.Flags})
      | None -> 
        state_machine_rules.Add({ Id = {idText = string r.Index; idRange = r.Position}; Body = traverseBlock let_to_add s.Body; Domain = r.Domain|> List.map(fun (_,_,_,id) -> id); Flags = r.Flags})
      | Some (ds,_) ->   
            let res = 
              match complete_dependencies.TryByTarget(_class, {idText = string r.Index; idRange = r.Position}) with
              | None -> true
              | Some ds -> 
                let mutable depends_from_atomic_rule = false
                for dc,df,_ in ds do
                  match ruleCodomains.TryByTarget(dc,df) with
                  | None -> ()
                  | Some rs -> 
                    for (_,_,is_atomic) in rs do
                      if is_atomic then depends_from_atomic_rule <- true
                depends_from_atomic_rule          
                
            if res then state_machine_rules.Add({ Id = {idText = string r.Index; idRange = r.Position}; Body = traverseBlock let_to_add s.Body; Domain = r.Domain |> List.map(fun (_,_,_,id) -> id); Flags = r.Flags })
            else suspended_rules.Add({ SuspendedRule.Id = {idText = string r.Index; idRange = r.Position}; Body = traverseBlock let_to_add s.Body; Domain = r.Domain |> List.map(fun (_,_,_,id) -> id); Flags = r.Flags})
    | _ -> ()


  let fields = ResizeArray()
  _class.Body.Fields.AddRange(          
            [for let_tp ,let_id in let_to_add do
              yield
                {
                  Name              = let_id
                  IsStatic          = false
                  Type              = let_tp
                  Update            = false
                  IsReference       = true
                  IsExternal        = None
                  UpdateNotificationsOnChange = false
                  CodeToInjectOnSet  = ""
                  UpdateField   = None
                }])

  _class.Body.StateMachineRules.AddRange(state_machine_rules)
  _class.Body.SuspendedRules.AddRange(suspended_rules)



and private traverseBlock let_to_add (block : StateMachinesAST.Block) : IntermediateAST.Block =
  [for e in block do yield traverseTypedExpression let_to_add e]


and private traverseTypedExpression (let_to_add : ResizeArray<StateMachinesAST.TypeDecl*Id>) (expr : StateMachinesAST.TypedExpression) : IntermediateAST.TypedExpression =
  let tp = fst expr
  
  let bool_type = TypedAST.ImportedType(typeof<bool>, tp.Position)
//and private traverseExpression (expr : StateMachinesAST.Expression) : IntermediateAST.Expression =
  match snd expr with
  | StateMachinesAST.Label(l) -> tp, IntermediateAST.Label (l.Id, l.Position)
  | StateMachinesAST.GotoSuspend(l) -> tp, IntermediateAST.GotoSuspend (l.Label.Id, l.Label.Position)
  | StateMachinesAST.Goto(l) -> tp, IntermediateAST.Goto (l.Label.Id, l.Label.Position)
  | StateMachinesAST.Set(id, expr) -> tp, IntermediateAST.Set(id, traverseTypedExpression let_to_add expr)
  | StateMachinesAST.Expression.IndexOf(id, expr) -> tp, IntermediateAST.IndexOf(traverseTypedExpression let_to_add id, traverseTypedExpression let_to_add expr)
  | StateMachinesAST.Expression.NewEntity(ids_block) -> tp, IntermediateAST.NewEntity([for id, b in ids_block do yield id, traverseBlock let_to_add b])
  | StateMachinesAST.Expression.IfThenElse(b_expr, None , b1, b2) -> tp, IntermediateAST.IfThenElse(traverseTypedExpression let_to_add (b_expr), None, traverseBlock let_to_add b1, traverseBlock let_to_add b2)
  | StateMachinesAST.Expression.IfThenElse(b_expr, Some expr_to_exchange, b1, b2) -> tp, IntermediateAST.IfThenElse(traverseTypedExpression let_to_add (b_expr), traverseTypedExpression let_to_add expr_to_exchange |> Some, traverseBlock let_to_add b1, traverseBlock let_to_add b2)
  | StateMachinesAST.Expression.IfThen(b_expr, b1) -> tp, IntermediateAST.IfThen(traverseTypedExpression let_to_add (b_expr), traverseBlock let_to_add b1)
  | StateMachinesAST.Expression.Tuple(b) -> tp, IntermediateAST.Tuple(traverseBlock let_to_add b)
  | StateMachinesAST.Expression.Query(q) -> tp, IntermediateAST.Query(q)
  | StateMachinesAST.Expression.Call(call) -> tp, IntermediateAST.Call(call)
  
  | StateMachinesAST.Expression.Not(e) -> tp, IntermediateAST.Not(traverseTypedExpression let_to_add e)
  | StateMachinesAST.Expression.Greater(e1, e2) -> tp, IntermediateAST.Greater(traverseTypedExpression let_to_add e1, traverseTypedExpression let_to_add e2)
  | StateMachinesAST.Expression.And(e1, e2) -> tp, IntermediateAST.And(traverseTypedExpression let_to_add e1, traverseTypedExpression let_to_add e2)
  | StateMachinesAST.Expression.Equals(e1, e2) -> tp, IntermediateAST.Equals(traverseTypedExpression let_to_add e1, traverseTypedExpression let_to_add e2)
  | StateMachinesAST.Expression.Or(e1, e2) -> tp, IntermediateAST.Or(traverseTypedExpression let_to_add e1, traverseTypedExpression let_to_add e2)
  
  
  | StateMachinesAST.Expression.Add(e1, e2) -> tp, IntermediateAST.Add(traverseTypedExpression let_to_add e1, traverseTypedExpression let_to_add e2)
  | StateMachinesAST.Expression.Sub(e1, e2) -> tp, IntermediateAST.Sub(traverseTypedExpression let_to_add e1, traverseTypedExpression let_to_add e2)
  | StateMachinesAST.Expression.Div(e1, e2) -> tp, IntermediateAST.Div(traverseTypedExpression let_to_add e1, traverseTypedExpression let_to_add e2)
  | StateMachinesAST.Expression.Mul(e1, e2) -> tp, IntermediateAST.Mul(traverseTypedExpression let_to_add e1, traverseTypedExpression let_to_add e2)
  | StateMachinesAST.Expression.Modulus(e1, e2) -> tp, IntermediateAST.Modulus(traverseTypedExpression let_to_add e1, traverseTypedExpression let_to_add e2)
  
  | StateMachinesAST.Expression.Range(e1, e2, p) -> tp, IntermediateAST.Range(traverseTypedExpression let_to_add e1, traverseTypedExpression let_to_add e2, p)

  | StateMachinesAST.Expression.ConcatQuery(qs) -> tp, IntermediateAST.ConcatQuery([for q in qs do yield traverseTypedExpression let_to_add q])
  | StateMachinesAST.Expression.AppendToQuery(e1, e2) -> tp, IntermediateAST.AppendToQuery(traverseTypedExpression let_to_add e1, traverseTypedExpression let_to_add e2)

  | StateMachinesAST.Expression.Maybe(StateMachinesAST.JustExpr(e)) -> tp, IntermediateAST.Maybe(IntermediateAST.JustExpr(traverseTypedExpression let_to_add e))
  | StateMachinesAST.Expression.Maybe(StateMachinesAST.NothingExpr(p)) -> tp, IntermediateAST.Maybe(IntermediateAST.NothingExpr(p))



  | StateMachinesAST.Expression.Literal(l) -> tp, IntermediateAST.Literal(l)
  | StateMachinesAST.Expression.Id(id) -> tp, IntermediateAST.Id(id)
  | StateMachinesAST.Expression.Let(id, tp, Some expr) -> 
    let_to_add.Add (tp, id)
    tp, IntermediateAST.Set(id, traverseTypedExpression let_to_add expr)
  | StateMachinesAST.Expression.Let(id, tp, None) -> 
    let_to_add.Add (tp, id)
    TypedAST.TypeDecl.Unit(id.idRange), Expression.DoNothing(id.idRange)

  | StateMachinesAST.Expression.Var(id, tp, Some expr) -> 
    let_to_add.Add (tp, id)
    tp, IntermediateAST.Set(id, traverseTypedExpression let_to_add expr)
  | StateMachinesAST.Expression.Var(id, tp, _) -> 
    let_to_add.Add (tp, id)
    TypedAST.TypeDecl.Unit(id.idRange), Expression.DoNothing(id.idRange)
  | StateMachinesAST.Expression.LetIfThen(id, tp, c, e) -> 
    let_to_add.Add (tp, id)
    tp, IntermediateAST.IfThen(traverseTypedExpression let_to_add (c), traverseBlock let_to_add e)
  | StateMachinesAST.Expression.LetIfThenElse(id, tp, c, e1, e2) -> 
    if id.IsSome then  let_to_add.Add (tp, id.Value)
    tp, IntermediateAST.IfThenElse(traverseTypedExpression let_to_add (c), None, traverseBlock let_to_add e1, traverseBlock let_to_add e2)
  | StateMachinesAST.Expression.DoGet(typedExpression, id) -> tp, IntermediateAST.Expression.DoGet(traverseTypedExpression let_to_add typedExpression, id)
  | StateMachinesAST.Expression.Cast(tp, e, p) -> tp, IntermediateAST.Expression.Cast(tp, traverseTypedExpression let_to_add e, p)
  | StateMachinesAST.Expression.AtomicFor(id, cond, update, block) -> tp, IntermediateAST.Expression.AtomicFor(id, traverseTypedExpression let_to_add cond, traverseTypedExpression let_to_add update, traverseBlock let_to_add block)
  | StateMachinesAST.Expression.Incr(e) -> tp, IntermediateAST.Expression.Incr(traverseTypedExpression let_to_add e)
  | StateMachinesAST.Expression.SetExpression(e1, e2) -> tp, IntermediateAST.Expression.SetExpression(traverseTypedExpression let_to_add e1, traverseTypedExpression let_to_add e2)
  | StateMachinesAST.Expression.Receive(t, p) -> tp, IntermediateAST.Expression.Receive(t, p)
  | StateMachinesAST.Expression.ReceiveMany(t, p) -> tp, IntermediateAST.Expression.ReceiveMany(t, p)
  | StateMachinesAST.Expression.Send(t, e, p) -> tp, IntermediateAST.Expression.Send(t,traverseTypedExpression let_to_add e, p)
  | StateMachinesAST.Expression.SendReliable(t, e, p) -> tp, IntermediateAST.Expression.SendReliable(t,traverseTypedExpression let_to_add e, p)



and private traverseWorld (world : StateMachinesAST.World) (table : EntityTable) (context : DependencyContext) : Class =
  traverseGenericEntity true world.Name world.Body table context

and private traverseEntity (entity : StateMachinesAST.Entity) (table : EntityTable) (context : DependencyContext) : Class =
  traverseGenericEntity false entity.Name entity.Body table context

and private traverseClasses (classes : Class list) (world : StateMachinesAST.World) (entities : StateMachinesAST.Entity list) (dependencies : IntermediateAST.DataDependencies) (table : EntityTable) (context : DependencyContext) ruleCodomains =
  for c in classes do
    let e = if world.Name = c.Name then world.Body 
            else (entities |> Seq.find(fun e -> e.Name = c.Name)).Body
    traverseAtomicRules classes c e dependencies table context
    traverseParallelMethods classes c e dependencies table context
    traverseInterruptibleChoice classes c e dependencies table context
  do dependencies.UpdateMaps()

  let complete_dependencies = dependencies.Clone()
  do dependencies.UpdateDependencies classes ruleCodomains
  do dependencies.UpdateMaps()

  for c in classes do
    let e = if world.Name = c.Name then world.Body else (entities |> Seq.find(fun e -> e.Name = c.Name)).Body
    traverseSuspRules classes c e dependencies table context ruleCodomains complete_dependencies
    





and private traverseField (field : TypedAST.Field) (owner : Class) (classes : List<Class>) (context : DependencyContext) : Field =
  let notify_targets = if context.ContainsKey(owner.Name, field.Name.Id) then context.[owner.Name, field.Name.Id] else ResizeArray()

  {
    Name              = field.Name.Id
    Type              = field.Type
    IsStatic          = field.IsStatic
    Update            = true    
    IsReference       = field.IsReference
    IsExternal        = 
      if field.IsExternal.IsNone then None
      else
        let id, b, c = field.IsExternal.Value
        Some (id.Id, b, c)
    UpdateNotificationsOnChange = false
    CodeToInjectOnSet = field.CodeToInjectOnSet
    UpdateField   = if field.UpdateField.IsNone then None else Some (traverseField field.UpdateField.Value owner classes context)
  }

and convertProgram (program : StateMachinesAST.Program) : Program =  
  let context : DependencyContext = System.Collections.Generic.Dictionary<Id * Id, ResizeArray<Id * Id>>()
  
  let table : EntityTable =
    [yield program.World.Name, program.World.Body.Fields
     for field in program.World.Body.Fields do context.Add((program.World.Name, field.Name.Id), ResizeArray())
     for entity in program.Entities do
      for field in entity.Body.Fields do context.Add((entity.Name, field.Name.Id), ResizeArray())
      yield entity.Name, entity.Body.Fields] |> Map.ofList  
  
  let world = traverseWorld program.World table context
  let entities = [for entity in program.Entities do 
                    yield traverseEntity entity table context ]
  
  let classes = world :: entities

  for field in program.World.Body.Fields do 
    world.Body.Fields.Add (traverseField field world classes context)

  
  if entities.Length <> program.Entities.Length then
    raise (Position.Empty) (sprintf "Internal error at %s(%s): unmatching number of entities." __SOURCE_FILE__ __LINE__)
  for entity, program_entity in List.zip entities program.Entities do
    for field in program_entity.Body.Fields do 
      entity.Body.Fields.Add (traverseField field entity classes context)

  let dependencies =
    let build_dependencies entity_name (b : StateMachinesAST.EntityBody) = 
      [for r in b.Rules do
        match r.Body with 
        | StateMachinesAST.StateMachine(s) ->
          match s.Dependencies with 
          | Some (ds,_) when ds <> [] -> 
            let d_c = classes |> Seq.find(fun c -> c.Name = entity_name)
            for d in ds do
              for (s_f, s_c, str) in d do
                let a = s_c
                let b = s_f
                let s_c = classes |> Seq.find(fun c -> c.Name = s_c)
                let s_f = s_c.Body.Fields |> Seq.find(fun f -> f.Name = s_f)
                yield 
                  s_c,s_f,d_c,{idText = string r.Index; idRange = r.Position},str
          | _ -> ()
        | _ -> ()]
    
    let dependencies =
      [yield! build_dependencies program.World.Name program.World.Body
       for e in program.Entities do yield! build_dependencies e.Name e.Body]

    dependencies |> List.map(fun (sc,sf,tc,tr,str) -> 
                                {
                                  SourceClass         = sc
                                  SourceField         = sf
                                  Target              = tc, tr, str
                                })

  

  let check_in_dependencies tp =
    dependencies
      |> Seq.exists(fun d ->
                      let rec match_name tp =
                        match tp with
                        | StateMachinesAST.TypeDecl.EntityName(name) -> 
                          let target,_,_ = d.Target
                          let source = d.SourceClass
                          name.Id = source.Name || name.Id = target.Name
                        | StateMachinesAST.TypeDecl.Query(tp) -> match_name tp
                        | StateMachinesAST.TypeDecl.MaybeType(TypedAST.Just(tp)) -> match_name tp
                        | StateMachinesAST.TypeDecl.MaybeQuery(TypedAST.Just(tp)) -> match_name tp
                        | _ -> false
                      match_name tp)

  for f in world.Body.Fields do
    if f.IsExternal.IsNone && 
       not f.Type.IsGeneric && 
       not f.IsImportedType && 
       check_in_dependencies f.Type then
        f.UpdateNotificationsOnChange <- true
  for e in program.Entities do
    for f in e.Body.Fields do
      if f.IsExternal.IsNone && 
         not f.Type.IsGeneric && 
         check_in_dependencies f.Type then
          f.UpdateNotificationsOnChange <- true

//(if is_on_casanova_entity then 
//  dependencies.Dependencies |> Seq.exists(fun d ->
//                                            let rec match_name tp =
//                                              match tp with
//                                              | StateMachinesAST.TypeDecl.EntityName(name) -> 
//                                                let target,_,_ = d.Target
//                                                let source = d.SourceClass
//                                                name = source.Name || name = target.Name
//                                              | StateMachinesAST.TypeDecl.Query(tp) -> match_name tp
//                                              | StateMachinesAST.TypeDecl.MaybeType(TypedAST.Just(tp)) -> match_name tp
//                                              | StateMachinesAST.TypeDecl.MaybeQuery(TypedAST.Just(tp)) -> match_name tp
//                                              | _ -> false
//                                            match_name tp)
//else false)
  let rule_codomains =
    let build_codomains b_name (b : StateMachinesAST.EntityBody) = 
      [for r in b.Rules do
        for t_c,t_f,_,_ in r.Domain do
          let s_c = classes |> Seq.find(fun c -> c.Name = b_name)
          let s_r = {Id.idText = string r.Index; Id.idRange = Position.Empty}
          let t_c = classes |> Seq.find(fun c -> c.Name = t_c)
          let t_f = t_c.Body.Fields |> Seq.find(fun f -> f.Name = t_f)
          let is_atomic =
            match r.Body with
            | StateMachinesAST.RuleBody.Atomic(_) -> true
            | _ -> false
          yield s_c,s_r,t_c,t_f, is_atomic]

    
    let rule_codomains =
      [yield! build_codomains program.World.Name program.World.Body
       for e in program.Entities do yield! build_codomains e.Name e.Body]

    rule_codomains |> List.map(fun (s_c, s_r, t_c, t_f, is_atomic) -> 
                                {
                                  SourceClass         = s_c
                                  SourceRule          = s_r
                                  IsAtomic            = is_atomic
                                  Target              = t_c, t_f
                                })
    
//  printfn ""
//  for d in dependencies do
//    let sc, sf = d.SourceClass,d.SourceField
//    for tc,tr,e in d.Targets do
//      printfn "%s.%s -> %s.Rule%s expression: %A" sc.Name.idText sf.Name.idText tc.Name.idText tr.idText e
//  printfn ""


  let intermediate_program = 
    { Module            = program.Module   
      Imports           = program.Imports
      Classes           = classes
      DataDependencies  = 
        { 
          _Dependencies = ResizeArray  (if Common.enable_dependency_analysis then dependencies else [])
          _BySource      = Map.empty
          _ByTarget      = Map.empty
        } 
      RuleCodomains =
        {
          _Dependencies = ResizeArray(rule_codomains)
          _BySource     = Map.empty
          _ByTarget     = Map.empty
        }
    }

  intermediate_program.RuleCodomains.UpdateMaps()  
  traverseClasses classes program.World program.Entities intermediate_program.DataDependencies table context intermediate_program.RuleCodomains
  intermediate_program 