module CSharpPrelude

let SuspendedRuleStructName = "OnEventRule" 

let private fastStack = 
    @"public class FastStack
{
  public int[] Elements;
  public int Top;

  public FastStack(int elems)
  {
    Top = 0;
    Elements = new int[elems];
  }

  public void Clear() { Top = 0; }
  public void Push(int x) { Elements[Top++] = x; }
}" 

let private ruleTableClass = 
    @"public class RuleTable
{
  public RuleTable(int elems)
  {
    ActiveIndices = new FastStack(elems);
    SupportStack = new FastStack(elems);
    ActiveSlots = new bool[elems];
    SupportSlots = new bool[elems];
  }

  public FastStack ActiveIndices;
  public FastStack SupportStack;
  public bool[] ActiveSlots;
  public bool[] SupportSlots;

  public void Clear()
  {
    for (int i = 0; i < ActiveSlots.Length; i++)
    {
      ActiveSlots[i] = false;
    }
  }
  public void Add(int i)
  {
    if (!ActiveSlots[i])
    {
      ActiveSlots[i] = true;
      ActiveIndices.Push(i);
    }
  }
}"

let StepAllSuspendedRules (c : IntermediateAST.Class) =
  "if (ActiveRules.ActiveIndices.Top > 0" + (if not c.IsWorldClass then " && frame == World.frame)" else ")") + "\n{
  for (int i = 0; i < ActiveRules.ActiveIndices.Top; i++)
  {
    var x = ActiveRules.ActiveIndices.Elements[i];
    switch (x)
    {" + 
  ([for r in c.Body.SuspendedRules do
      yield "case " + r.Id.idText + ":
        if (this.Rule" + r.Id.idText + "(dt, world) == RuleResult.Done)
        {
          ActiveRules.ActiveSlots[i] = false;
          ActiveRules.ActiveIndices.Top--;
        }
        else{          
          ActiveRules.SupportSlots[" + r.Id.idText + "] = true;
          ActiveRules.SupportStack.Push(x);
        }
        break;\n"] |> Seq.fold(+) "") + "
      default:
        break;
    }
  }
  ActiveRules.ActiveIndices.Clear();
  ActiveRules.Clear();

  var tmp = ActiveRules.SupportStack;
  var tmp1 = ActiveRules.SupportSlots;

  ActiveRules.SupportStack = ActiveRules.ActiveIndices;
  ActiveRules.SupportSlots = ActiveRules.ActiveSlots;


  ActiveRules.ActiveIndices = tmp;
  ActiveRules.ActiveSlots = tmp1;

  if (ActiveRules.ActiveIndices.Top == 0)
    toRemove.Add(ID);
}" + (if c.IsWorldClass then "" else "else
{
  if (this.frame != World.frame)
    toRemove.Add(ID);
}")

let ruleResult = "using System.Linq;\nusing System;\nusing System.Collections.Generic;\npublic enum RuleResult { Done, Working }"
let Prelude = ruleResult + CasanovaPrelude.casanova_prelude  + "\n\n" + fastStack + "\n\n" + ruleTableClass + "\n\n"
let FieldNotifications (dp : IntermediateAST.DataDependencies) (world_name : string) (source : IntermediateAST.Class) (f : Common.Id)  target =  


  let dependencies =
        let flag = ref false //bad name
        [for r in source.Body.SuspendedRules do
          match dp.TryByTarget(source,r.Id) with
          | None -> ()
          | Some res -> 
            if not !flag then 
              yield "entity.JustEntered = true;\n"
              flag := true
            for c, f, _ in res do           
              yield world_name + "." + IntermediateAST.DataDependencies.PrintNotifyFieldName c f.Name source r.Id + ".Remove(entity.ID);\n"] |> Seq.fold(fun a b -> a + b) ""

  let (tc,tr,_) = target
  "\t\t\tfor(int i = 0; i < " + world_name + "." + IntermediateAST.DataDependencies.PrintNotifyFieldName source f tc tr + "[ID].Count; i++) {
var entity = " + world_name + "." + IntermediateAST.DataDependencies.PrintNotifyFieldName source f tc tr + "[ID][i]; \n" +
  ((if source.Name.idText = world_name then "" else "if (entity.frame == World.frame){ ")  + 
   ("if(!" + world_name + "." + tc.Name.idText + "WithActiveRules.ContainsKey(entity.ID))
   " + world_name + "." + tc.Name.idText + "WithActiveRules.Add(entity.ID,new Tuple<" + tc.Name.idText + ", RuleTable>(entity, new RuleTable(" + string (source.Body.SuspendedRules.Count + source.Body.AtomicRules.Count + source.Body.StateMachineRules.Count + source.Body.ParallelMethods.Count + source.Body.ConcurrentMethods.Count) + ")));") 
   + world_name + "." + tc.Name.idText + "WithActiveRules[entity.ID].Item2.Add(" + tr.idText + ");\n" + 
    (if source.Name.idText = world_name then "" else "}else{ "+ dependencies + "\t\t\t }")) + "}\n"

//        if (entity.Frame == World.frame) World.BallsWithActiveRules[entity.ID].Item2.Add(0);
//        else
//        {
//          World.NotifySlotPBall0.Remove(entity.ID);
//        }

let StepSuspendedRules (c : IntermediateAST.Class) (world_name : string) (ds : IntermediateAST.DataDependencies) =
    let targets = 
      [for r in c.Body.SuspendedRules do
        match ds.TryByTarget(c,r.Id) with
        | None -> ()
        | Some res -> yield! res]
    match targets with 
    | [] -> ""
    | _ ->  "\tpublic void UpdateSuspendedRules(float dt, " + world_name + " world, List<int> toRemove, RuleTable ActiveRules) { " + StepAllSuspendedRules c + " }\n"

let ActiveRuleDeclarations (c : IntermediateAST.Class) (ds : IntermediateAST.DataDependencies) = 
  let references_to_c =
    [for r in c.Body.SuspendedRules do
      match ds.TryByTarget(c,r.Id) with
      | None -> ()
      | Some res -> yield! res]
  if references_to_c.Length > 0 then "\tpublic RuleTable ActiveRules = new RuleTable(" + string references_to_c.Length + ");\n" else ""