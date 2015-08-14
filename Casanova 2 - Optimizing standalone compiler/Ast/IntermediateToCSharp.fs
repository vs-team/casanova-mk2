module IntermediateToCSharp

open IntermediateAST
open Common
open RulesPrettyPrinter



let GenerateParallelMethod (world_name : string) ((id,b) : Id * Block) = 
  let tabs = "\t"
  "\n\tint s" + id.idText + "=-1;\n\tpublic void parallelMethod" + id.idText + "(float dt, " + world_name + " world){ \n\tswitch (s" + id.idText + ")" + traverse_state_machine_block' b ("s" + id.idText) false tabs (ref false) false + "}\n\t"
  
let GenerateConcurrentSelectlMethod (world_name : string) ((id, _done, state, b) : Id * Id * Id * Block) = 
  if b.Length = 0 then raise Position.Empty (sprintf "Internal error at %s(%s)." __SOURCE_FILE__ __LINE__) |> ignore
  let tabs = "\t"
  let block_header = [b.Head]
  let switch_code = "\n\tswitch (s" + id.idText + ")"
  let block_continuation = b.Tail
  "\n\tint " + state.idText + ", s" + id.idText + "=-1;\n\tbool " + _done.idText + " = false;\n\tpublic void concurrentSelectMethod" + id.idText + "(float dt, " + world_name + " world){"+ traverse_state_machine_block block_header ("s" + id.idText) false tabs (ref false) false + 
  "\n\tswitch (s" + id.idText + ")" + traverse_state_machine_block block_continuation ("s" + id.idText) false tabs (ref false) false + "}\n\t"

let GenerateConcurrentMethods (world_name : string) rs =
  let rules = 
    match [for r in rs do yield GenerateConcurrentSelectlMethod world_name r] with
    | [] -> "\n"
    | [x] -> x + "\n"
    | xs -> xs |> Seq.reduce(fun a b -> a + "\n" + b)
  rules + "\n"

let GenerateParallelMethods (world_name : string) rs =
  let rules = 
    match [for r in rs do yield GenerateParallelMethod world_name r] with
    | [] -> "\n"
    | [x] -> x + "\n"
    | xs -> xs |> Seq.reduce(fun a b -> a + "\n" + b)
  rules + "\n"

let GenerateAutomatedRule (world_name : string) (r : IntermediateAST.AutomatedRule) = 
  let tabs = "\t"
  "\n\tpublic void Rule" + r.Id.idText + "(float dt, " + world_name + " world) " + traverse_atomic_block r.Body r.Domain tabs + "\n\t"

let GenerateAutomatedRules (world_name : string) rs =
  let rules = 
    match [for r in rs do yield GenerateAutomatedRule world_name r] with
    | [] -> "\n"
    | [x] -> x + "\n"
    | xs -> xs |> Seq.reduce(fun a b -> a + "\n" + b)
  rules + "\n"

let StepAutomatedRules(b : IntermediateAST.ClassBody) = 
  let body =
    match 
      [for r in b.AtomicRules do
        yield "\t\tthis.Rule" + r.Id.idText + "(dt, world);" ] with
    | [] -> "\n"
    | [x] -> x + "\n"
    | xs -> xs |> Seq.reduce(fun a b -> a + "\n" + b)
  body + "\n"

let StepStateMachineRules(b : IntermediateAST.ClassBody) = 
  let body =
    match 
      [for r in b.StateMachineRules do
        yield "\t\tthis.Rule" + r.Id.idText + "(dt, world);" ] with
    | [] -> "\n"
    | [x] -> x + "\n"
    | xs -> xs |> Seq.reduce(fun a b -> a + "\n" + b)
  body + "\n"


let GenerateStateMachineRule (world_name : string) (r : IntermediateAST.StateMachineRule) = 
  let tabs = "\t"
  "\n\tint s" + r.Id.idText + "=-1;\n\tpublic void Rule" + r.Id.idText + "(float dt, " + world_name + " world){ \n\tswitch (s" + r.Id.idText + ")" + traverse_state_machine_block' r.Body ("s" + r.Id.idText) false tabs (ref false) false + "}\n\t"

let GenerateStateMachineRules (world_name : string) rs =
  let rules = 
    match [for r in rs do yield GenerateStateMachineRule world_name r] with
    | [] -> "\n"
    | [x] -> x + "\n"
    | xs -> xs |> Seq.reduce(fun a b -> a + "\n" + b)
  rules + "\n"
  
let GenerateSuspendedRule (world_name : string) (r : IntermediateAST.SuspendedRule) = 
  let tabs = "\t"
  "\n\tint s" + r.Id.idText + "=-1;\n\tpublic RuleResult Rule" + r.Id.idText + "(float dt, " + world_name + " world){ \n\tswitch (s" + r.Id.idText + ")" + traverse_state_machine_block' r.Body ("s" + r.Id.idText) true tabs (ref true) false + "}\n\t"

let GenerateSuspendedRules (world_name : string) rs =
  let rules = 
    match [for r in rs do yield GenerateSuspendedRule world_name r] with
    | [] -> "\n"
    | [x] -> x + "\n"
    | xs -> xs |> Seq.reduce(fun a b -> a + "\n" + b)
  rules + "\n"

let GenerateConstructor (dp:IntermediateAST.DataDependencies) (world_name : string) entities (c : Class) = 
  let args = 
    let args  = c.Body.Create.Args
    if args = [] then ""
    else
      args.Tail |> Seq.fold(fun s (id, t) -> s + ", " + t.TypeName + " " + id.idText) ((snd args.Head).TypeName + " " + (fst args.Head).idText)

  let return_expr = 
    if dp.IsSourceOrTarget c then
      (c.Body.Create.Args |> Seq.map(fun (id, tp) -> "this." + id.idText + " = " + id.idText + ";\n") |> Seq.fold(+) "")
    else
      match snd c.Body.Create.Body with
      //  tp, [(id1,e1) ... (idn,en)]_a
      //  a <- new tp(){ id1 = [e1], idn = [en]; }
      |tp, OptimizedQueryAST.NewEntity(id_exprs) -> 
        let args =
          [for id, e in id_exprs do
            let last = e |> Seq.skip(e.Length - 1) |> Seq.head
            let lst_but_last = e |> Seq.take(e.Length - 1) |> Seq.toList
            if lst_but_last <> [] then raise id.idRange "..." |> ignore
            yield id.idText + " = " + traverse_atomic_expr true last (false, None) [] "" false] |> Seq.fold (fun s e -> e + "\n\t\t" + s) ""
        args
      |tp, e -> 
        traverse_atomic_expr true (tp, e) (false, None) [] "" false



  let res1 = 
//    if dp.IsSourceOrTarget c |> not then
      traverse_create_block dp entities "" c.Body.Create.Body c.Body.Fields "\t\t"
//    else ""
//  let res2 = return_expr 
  let res3 =
    (if dp.IsSourceOrTarget c then "\nthis.ID = " + world_name + "." + c.Name.idText + "Counter++;\n" else "") +
    ([for f in c.Body.Fields do
          let fNotificationTargets = dp.TryBySource(c,f)
          match fNotificationTargets with
          | None -> ()
          | Some fNotificationTargets -> 
            if fNotificationTargets = [] then ()
            else 
              for (tc,tr,_) in fNotificationTargets do
                yield "\t" + world_name + "." + DataDependencies.PrintNotifyFieldName c f.Name tc tr + ".Add(ID, new List<" + tc.Name.idText + ">());\n"
      for tr in c.Body.SuspendedRules do
        let tr = tr.Id
        let tc = c
        let fNotificationTargets = dp.TryByTarget(c,tr)
        match fNotificationTargets with
        | None -> ()
        | Some fNotificationTargets -> 
          if fNotificationTargets = [] then ()
          else 
            for (c,f,_) in fNotificationTargets do
              yield "\t" + world_name + "." + DataDependencies.PrintNotifyFieldName c f.Name tc tr + "[ID].Add(this);\n"
              
            
            
            ] |> Seq.fold (+) "") 
  (c.Body.Create.Args |> Seq.map(fun (id, tp) -> "private " + tp.TypeName + " " + id.idText + ";\n") |> Seq.fold(+) "") +
  "\tpublic int ID;\npublic " + c.Name.idText + "(" + args + ")" + "\n\t{" + 
  "JustEntered = false;\n frame = " + world_name + ".frame;\n" +
  


  
  res3 + res1 + 
    //(if dp.IsSourceOrTarget c then "Init();" else "") + 
    "}\n\t"
  

let GenerateWorldConstructor (dp:IntermediateAST.DataDependencies) (entities : Class list) (c : Class) = 
  let args = 
    let args  = c.Body.Create.Args
    if args = [] then ""
    else args.Tail |> Seq.fold(fun s (id, t) -> s + ", " + t.TypeName + " " + id.idText) ((snd args.Head).TypeName + " " + (fst args.Head).idText)
  let optimization_data_structure =
    (entities |> Seq.filter(fun e -> dp.IsTarget e) |> Seq.map(fun e -> e.Name.idText + "WithActiveRules = new Dictionary<int, Tuple<" + e.Name.idText + ", RuleTable>>();\n") |> Seq.fold (+) "") +
    (entities |> Seq.filter(fun e -> dp.IsTarget e) |> Seq.map(fun e -> e.Name.idText + "WithActiveRulesToRemove = new List<int>();\n") |> Seq.fold (+) "")
  
  (c.Body.Create.Args |> Seq.map(fun (id, tp) -> "private " + tp.TypeName + " " + id.idText + ";\n") |> Seq.fold(+) "") +
  
  (if dp.IsSourceOrTarget c then "\tint ID = 0;\n" else "") + "\n" + (if Common.run_profiler then "System.IO.StreamWriter file;" else "") + "\npublic void Start()" + "\n\t{\n"+
  (if Common.run_profiler then "file = new System.IO.StreamWriter(System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(),\"test_\" + DateTime.Now.Hour + \"_\" + DateTime.Now.Minute + \"_\" + DateTime.Now.Second + \"_\" + \".csv\"));" else "") + 
  (c.Body.Create.Args |> Seq.map(fun (id, tp) -> "this." + id.idText + " = " + id.idText + ";\n") |> Seq.fold(+) "") +  
  (entities |> Seq.filter(fun e -> dp.IsSourceOrTarget e)  |> Seq.map(fun e -> e.Name.idText + "Counter = 0;\n") |> Seq.fold (+) "") +
  (entities |>
    Seq.map(fun c -> 
      [for f in c.Body.Fields do
              let fNotificationTargets = dp.TryBySource(c,f)
              match fNotificationTargets with
              | None -> ()
              | Some fNotificationTargets -> 
                if fNotificationTargets = [] then ()
                else 
                  for (tc,tr,str) in fNotificationTargets do                    
                      yield "\t" + DataDependencies.PrintNotifyFieldName c f.Name tc tr + " = new Dictionary<int, List<" + tc.Name.idText + ">>();\n"
                      
          ] |> Seq.fold (+) "") |> Seq.fold (+) "") +
  ([for f in c.Body.Fields do
        let fNotificationTargets = dp.TryBySource(c,f)
        match fNotificationTargets with
        | None -> ()
        | Some fNotificationTargets -> 
          if fNotificationTargets = [] then ()
          else 
            for (tc,tr,_) in fNotificationTargets do
              yield "\t" + DataDependencies.PrintNotifyFieldName c f.Name tc tr + ".Add(ID, new List<" + tc.Name.idText + ">());\n"                    
    for tr in c.Body.SuspendedRules do
        let tr = tr.Id
        let tc = c
        let fNotificationTargets = dp.TryByTarget(c,tr)
        match fNotificationTargets with
        | None -> ()
        | Some fNotificationTargets -> 
          if fNotificationTargets = [] then ()
          else 
            for (c,f,_) in fNotificationTargets do
              yield "\t" + DataDependencies.PrintNotifyFieldName c f.Name tc tr + "[ID].Add(this);\n"
          ] |> Seq.fold (+) "") +
  traverse_create_block dp entities optimization_data_structure c.Body.Create.Body c.Body.Fields "\t\t" + "}\n\t"


let GenerateRules (world_name : string) (b:IntermediateAST.ClassBody) = 
  GenerateAutomatedRules world_name (b.AtomicRules) +
  GenerateParallelMethods world_name b.ParallelMethods +
  GenerateStateMachineRules world_name b.StateMachineRules +
  GenerateConcurrentMethods world_name b.ConcurrentMethods +
  GenerateSuspendedRules world_name b.SuspendedRules

let ClearNotifications (dp:IntermediateAST.DataDependencies) (c:IntermediateAST.Class) =
  query{ 

    for f in c.Body.Fields do 
    let fNotificationTargets = dp.TryBySource(c,f)
    where (fNotificationTargets.IsSome && fNotificationTargets.Value <> [])
    select
     (query{ 
        for (tc,tr,_) in fNotificationTargets.Value do
        select ("\t\t" + DataDependencies.PrintNotifyFieldName c f.Name tc tr + ".Clear();\n")
        } |> Seq.fold (+) "") } |> Seq.fold (+) ""  

let GenerateFieldUpdates (dp:IntermediateAST.DataDependencies) (c:IntermediateAST.Class) =
  let b = c.Body
  let rec traverseUpdates (updateableExpression:string) (currentType:StateMachinesAST.TypeDecl) (recursionDepth:int) (tabs:string) = 
    let earlyOut = false // check if any of the children of currentType is updateable; if not, stop there
    if earlyOut then ""
    else
      match currentType with 
      | StateMachinesAST.TypeDecl.MaybeType(TypedAST.Just(tp)) -> 
        "if(" + updateableExpression + ".IsSome){ " +  
        traverseUpdates (updateableExpression + ".Value") tp recursionDepth tabs + " } \n"
      | StateMachinesAST.TypeDecl.EntityName(id) -> 
        tabs + updateableExpression + ".Update(dt, world);\n"
      | StateMachinesAST.TypeDecl.Tuple(l) -> 
        let tupleArity = l.Length
        query { for i in 0 .. (tupleArity-1) do
                where (((l.Item i).IsSystemType) |> not)
                select (traverseUpdates (updateableExpression + ".Item" + (i+1).ToString()) (l.Item i) (recursionDepth) tabs) } |> Seq.fold (+) "" 
      | StateMachinesAST.TypeDecl.Query(t) -> 
        let singleElementUpdate = (traverseUpdates (updateableExpression + "[x" + recursionDepth.ToString() + "]") t (recursionDepth+1) (tabs+"\t"))  
        tabs + "for(int x" + recursionDepth.ToString() + " = 0; x" + recursionDepth.ToString() + " < " + updateableExpression + ".Count; x" + recursionDepth.ToString() + "++) { \n" + singleElementUpdate + tabs + "}\n" 
      | StateMachinesAST.TypeDecl.GenericType(StateMachinesAST.TypeDecl.ImportedType(el,p),[t]) -> 
        if el.GetInterface("IEnumerable`1") <> null then 
          let singleElementUpdate = (traverseUpdates (updateableExpression + "x" + recursionDepth.ToString() + "]") t (recursionDepth+1) (tabs+"\t"))  
          tabs + "for(int x" + recursionDepth.ToString() + " = 0; x" + recursionDepth.ToString() + " < " + updateableExpression + ".Count; x" + recursionDepth.ToString() + "++) { \n" + singleElementUpdate + tabs + "}\n" 
        else 
          "" 
      | _ -> ""
  query{ for f in b.Fields do
          where (f.Update && not f.IsImportedType && f.IsReference |> not && f.IsExternal.IsNone)
          select (traverseUpdates f.Name.idText f.Type 0 "\t\t") } |> Seq.fold (+) ""

let RegisterNotifications (dp:IntermediateAST.DataDependencies) (c:IntermediateAST.Class) =
  query{
    for r in c.Body.SuspendedRules do
    let maybe_ts = dp.TryByTarget(c,r.Id)
    where (maybe_ts.IsSome)
    for sc, sf, expr in maybe_ts.Value do
    select ((if expr <> "" then expr + "." else "") + DataDependencies.PrintNotifyFieldName c sf.Name c r.Id + ".Add(this);\n")
  } |> Seq.fold (+) ""


let GenerateUpdate (world_name : string) (dp:IntermediateAST.DataDependencies) (c:IntermediateAST.Class) = 
  let notifications = RegisterNotifications dp c


  "\tpublic void Update(float dt, " + world_name + " world) {\nframe = " + world_name + ".frame;" + 
//  ClearNotifications dp c +
//  RegisterNotifications dp c +
  StepAutomatedRules(c.Body) +
  GenerateFieldUpdates dp c +    
  StepStateMachineRules(c.Body) +
  "\t}\n" + 
  CSharpPrelude.StepSuspendedRules c world_name dp



let GenerateWorldUpdate (world_name : string) (entities : IntermediateAST.Class list) (dp:IntermediateAST.DataDependencies) (c:IntermediateAST.Class) = 
  let notifications = RegisterNotifications dp c
  
  "\nSystem.DateTime init_time = System.DateTime.Now;\n\tpublic void Update(float dt, " + world_name + " world) {\n" +
  ("var t = System.DateTime.Now;") + 
//  ClearNotifications dp c +
//  RegisterNotifications dp c +
  StepAutomatedRules(c.Body) +
  GenerateFieldUpdates dp c +
  StepStateMachineRules(c.Body) +

  (entities |> Seq.map(fun e -> 
    if
      [for r in e.Body.SuspendedRules do
        match dp.TryByTarget(e,r.Id) with
        | None -> ()
        | Some res -> yield! res].Length = 0 then ""
    else
      let dependencies =
        let flag = ref false
        [for r in e.Body.SuspendedRules do
          match dp.TryByTarget(e,r.Id) with
          | None -> ()
          | Some res -> 
            for c, f, _ in res do 
              
              if not !flag then 
                yield "x.Value.Item1.JustEntered = true;\n"
                flag := true
              
              yield DataDependencies.PrintNotifyFieldName c f.Name e r.Id + ".Remove(x.Value.Item1.ID);\n"] |> Seq.fold(fun a b -> a + b) ""
      //        if (x.Value.Item1.Frame != frame)
//        {
//          NotifySlotPBall0.Remove(x.Value.Item1.ID);
//          BallsWithActiveRulesToRemove.Add(x.Value.Item1.ID);
//        }
//        else x.Value.Item1.UpdateSuspendedRules(dt, BallsWithActiveRulesToRemove, x.Value.Item2);

      ("if(" + e.Name.idText + "WithActiveRules.Count > 0){
foreach(var x in " + e.Name.idText + "WithActiveRules)\n" + (
if c.Name.idText <> world_name then ("if (x.Value.Item1.frame != frame){\n
  " + dependencies + "\n" + e.Name.idText + "WithActiveRulesToRemove.Add(x.Value.Item1.ID);" + "
  }\nelse") else "") + " x.Value.Item1.UpdateSuspendedRules(dt, this, " + e.Name.idText + "WithActiveRulesToRemove, x.Value.Item2);
if(" + e.Name.idText + "WithActiveRulesToRemove.Count > 0){
for(int i = 0; i < " + e.Name.idText + "WithActiveRulesToRemove.Count; i++)
" + e.Name.idText + "WithActiveRules.Remove("+ e.Name.idText + "WithActiveRulesToRemove[i]); \n" + e.Name.idText + "WithActiveRulesToRemove.Clear();\n" +  "}\n }\n")) |> Seq.fold (+) "")  +
  (if Common.run_profiler then ("var t1 = System.DateTime.Now;\nfile.WriteLine((t1 - t).Milliseconds + \",\" + (t1 - init_time).Seconds);\n
if ((DateTime.Now - first_time).TotalSeconds > 60)
    { UnityEditor.EditorApplication.isPlaying = false; }" ) else "") +
  "\t}\n" + (if Common.run_profiler then "\npublic DateTime first_time = DateTime.Now;\n" else "") +
  CSharpPrelude.StepSuspendedRules c c.Name.idText dp

let GenerateProperties world_name (dp:IntermediateAST.DataDependencies) (c:IntermediateAST.Class) =
  query{ 
    for f in c.Body.Fields do 
    let fNotificationTargets = dp.TryBySource(c,f)
    where (fNotificationTargets.IsSome && fNotificationTargets.Value <> [] && (not f.UpdateNotificationsOnChange || c.Name.idText = world_name))
    let notifications = 
      query{ 
        for n in fNotificationTargets.Value do
        select (CSharpPrelude.FieldNotifications dp world_name c f.Name n)
        } |> Seq.fold (+) ""
    select (
      let typeName = f.Type.TypeName
      "\tpublic " + typeName + " " + f.Name.idText + "{ \n\t\tget { return _" + f.Name.idText + "; } \n\t\t
      set { \n\t\t\t_" + f.Name.idText + " = value;\n" + f.CodeToInjectOnSet + notifications + "\t\t}\n\t}\n") } |> Seq.fold (+) ""

let GenerateFields (entities : Class list) world_name (dp:IntermediateAST.DataDependencies) (c:IntermediateAST.Class) = 
  let active_rules = CSharpPrelude.ActiveRuleDeclarations c dp
  ([ for f in c.Body.Fields do 
      match f.IsExternal with
      | None ->
        let fNotificationTargets = dp.TryBySource(c,f)
        
        let init_notifies value name  =
          let c = entities |> Seq.tryFind(fun c -> name = c.Name.idText)
          match c with
          | None -> ""
          | Some c ->
            ([for r in c.Body.SuspendedRules do
      
              let fNotificationTargets = dp.TryByTarget(c, r.Id)
              match fNotificationTargets with
              | None -> ()
              | Some fNotificationTargets -> 
                if fNotificationTargets = [] then ()
                else 
                  for (sc,sf,str) in fNotificationTargets do
                    yield "\t" + world_name + "." + DataDependencies.PrintNotifyFieldName sc sf.Name c r.Id + "[" + value + "." + (if str = "" then "" else str + ".") + "ID].Add(" + value + ");\n"
                ] |> Seq.fold (+) "")

        let check_just_entered =
          let rec match_name tp inside_for value_name =
            match tp with
            | StateMachinesAST.TypeDecl.EntityName(name) ->
              if inside_for then "if(" + value_name + ".JustEntered){ " + value_name + ".JustEntered = false;\n" + init_notifies value_name name.idText + "}\n"
              else "if(!" + value_name + ".JustEntered) __" + f.Name.idText  + " = value; \n else{ " + value_name + ".JustEntered = false;\n" + init_notifies value_name name.idText + "}\n"
            | StateMachinesAST.TypeDecl.Query(tp) -> 
              "foreach(var e in " + value_name + "){" +
              match_name tp true "e" +
              "}"
            | StateMachinesAST.TypeDecl.MaybeType(TypedAST.Just(tp)) -> 
                "if(" + value_name + ".IsSome){" + match_name tp false (value_name + ".Value") + "}"
            | StateMachinesAST.TypeDecl.MaybeQuery(TypedAST.Just(tp)) -> match_name (StateMachinesAST.TypeDecl.Query(tp)) false value_name               
            | _ -> ""
          match_name f.Type false "value"

        let get = " get { return  __" + f.Name.idText  + "; }\n "
        let set = " set{ __" + f.Name.idText  + " = value;\n " + f.CodeToInjectOnSet + check_just_entered + " }\n "
        
        let static_keyword = if f.IsStatic then " static " else ""
        match fNotificationTargets with        
        | Some fNotificationTargets when fNotificationTargets <> []-> 
          if not f.UpdateNotificationsOnChange || world_name = c.Name.idText then
            yield "\tpublic " + static_keyword + f.Type.TypeName + " _" + f.Name.idText + ";\n" 
          else
            yield "\tpublic " + static_keyword + f.Type.TypeName + " __" + f.Name.idText + ";\n"            
            yield "\tpublic " + static_keyword + "_" + f.Type.TypeName + " " + f.Name.idText + 
                              "{ " + get + set + "}\n"          
          
            
        | _ ->
          if not f.UpdateNotificationsOnChange then
            yield "\tpublic " + static_keyword + f.Type.TypeName + " " + f.Name.idText + ";\n"
          else
            yield "\tpublic " + static_keyword  + f.Type.TypeName + " __" + f.Name.idText + ";\n"
            yield "\tpublic " + static_keyword  + f.Type.TypeName + " " + f.Name.idText + 
                              "{ " + get + set + "}\n"          
            

        
      | Some (e, publicGet, publicSet) ->

        let underscore = 
          let fNotificationTargets = dp.TryBySource(c,f)
          if (fNotificationTargets.IsSome && fNotificationTargets.Value <> [] && not f.UpdateNotificationsOnChange) then "_"
          else ""
    
        
        let get = if publicGet then " get { return " + e.idText + "." + f.Name.idText + "; }\n " else ""
        let set = if publicSet then " set{"  + e.idText + "." + f.Name.idText + " = value; }\n " else ""

        yield "\tpublic " + f.Type.TypeName + " " + underscore + f.Name.idText + 
                          "{ " + get + set + "}\n"] 
    |> Seq.fold (+) "") + active_rules
let GenerateInitMethod (dp : IntermediateAST.DataDependencies) entities (c : IntermediateAST.Class) (world_name : string) =
    if dp.IsSourceOrTarget c then
      "public void Init() {" +
      
//      //notification registrer
//      let res = 
//        ([for f in c.Body.Fields do
//            let fNotificationTargets = dp.TryBySource(c,f)
//            match fNotificationTargets with
//            | None -> ()
//            | Some fNotificationTargets -> 
//              if fNotificationTargets = [] then ()
//              else 
//                for (tc,tr,_) in fNotificationTargets do
//                  yield "\t" + world_name + "." + DataDependencies.PrintNotifyFieldName c f.Name tc tr + "[ID].Add(this);\n"                    
//              ] |> Seq.fold (+) "") + "\n"
//
//
//      res + 
      traverse_create_block dp entities "" c.Body.Create.Body c.Body.Fields "\t\t" +
      "\n}\n"
    else ""
    

let ConvertClass (world_name : string) (dp:IntermediateAST.DataDependencies) (entities : Class list) (c:IntermediateAST.Class) = 
  "public class " + c.Name.idText + 
  "{\npublic int frame;"+ 
  "\npublic bool JustEntered = true;\n" +
  GenerateConstructor dp world_name entities c +
  GenerateInitMethod dp entities c world_name +
  GenerateFields entities world_name dp c +
  GenerateProperties world_name dp c +
  GenerateUpdate world_name dp c +
  GenerateRules world_name (c.Body) +
  "\n}\n"

let ConvertWorldClass (entities : Class list) (dp:IntermediateAST.DataDependencies) (c:IntermediateAST.Class) =
  
  "public class " + c.Name.idText + (if Common.is_running_unity then " : MonoBehaviour" else "") +
  "{\n" +
  (if Common.run_profiler then "  void OnApplicationQuit() { file.Close(); }" else "") +
  "public static int frame;\n" + (if Common.is_running_unity then "void Update () { Update(Time.deltaTime, this); \n frame++; }" else "") +
  "\npublic bool JustEntered = true;\n" +
  (entities |> Seq.filter(fun e -> dp.IsSourceOrTarget e) |> Seq.map(fun e -> "static public int " + e.Name.idText + "Counter;\n") |> Seq.fold (+) "") +   
  (entities |> Seq.filter(fun e -> dp.IsTarget e) |> Seq.map(fun e -> "static public Dictionary<int, Tuple<" + e.Name.idText + ", RuleTable>> " + e.Name.idText + "WithActiveRules;\n") |> Seq.fold (+) "") +
  (entities |> Seq.filter(fun e -> dp.IsTarget e) |> Seq.map(fun e -> "static public List<int> " + e.Name.idText + "WithActiveRulesToRemove;\n") |> Seq.fold (+) "") +  
  (entities |>
    Seq.map(fun c -> 
      [for f in c.Body.Fields do
              let fNotificationTargets = dp.TryBySource(c,f)
              match fNotificationTargets with
              | None -> ()
              | Some fNotificationTargets -> 
                if fNotificationTargets = [] then ()
                else 
                  for (tc,tr,_) in fNotificationTargets do
                    let res = "\tstatic public  Dictionary<int, List<" + tc.Name.idText + ">> " + DataDependencies.PrintNotifyFieldName c f.Name tc tr + ";\n"
                    yield res
          ] |> Seq.fold(+) "") |> Seq.fold(+) "") +


  GenerateWorldConstructor dp entities c +
  GenerateInitMethod dp entities c c.Name.idText +
  GenerateFields entities c.Name.idText dp c +
  GenerateProperties c.Name.idText dp c +
  GenerateWorldUpdate c.Name.idText entities dp c +
  GenerateRules c.Name.idText (c.Body) +
  "\n}\n"
  
let ConvertProgram(p:IntermediateAST.Program) =
      

  "#pragma warning disable 162,108,618\nusing Casanova.Prelude;\nusing System.Linq;\nusing System;\nusing System.Collections.Generic;\n" +
  ([for import in p.Imports do yield "using " + import.idText + ";\n"] |> Seq.fold(+) "") +
  //CSharpPrelude.Prelude + " \n " +
  "namespace " + p.Module.idText + " {" +
  let world_name = p.Classes |> Seq.find(fun c -> c.IsWorldClass)
  ([  for c in p.Classes do 
        if c.IsWorldClass then 
          yield ConvertWorldClass p.Classes p.DataDependencies c
        else  yield ConvertClass world_name.Name.idText p.DataDependencies (p.Classes |> Seq.toList) c ] |> Seq.fold (+) "") + "}"

