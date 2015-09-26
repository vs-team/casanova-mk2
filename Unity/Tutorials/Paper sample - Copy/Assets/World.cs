#pragma warning disable 162,108,618
using Casanova.Prelude;
using System.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace Game {public class World : MonoBehaviour{
public static int frame;
void Update () { Update(Time.deltaTime, this); 
 frame++; }
public bool JustEntered = true;
static public int WorldCounter;
static public int ElemCounter;
static public Dictionary<int, Tuple<World, RuleTable>> WorldWithActiveRules;
static public Dictionary<int, Tuple<Elem, RuleTable>> ElemWithActiveRules;
static public List<int> WorldWithActiveRulesToRemove;
static public List<int> ElemWithActiveRulesToRemove;
	static public  Dictionary<int, List<World>> NotifySlotWorldElemsWorld1;
	static public  Dictionary<int, List<Elem>> NotifySlotElemCounterElem0;
	static public  Dictionary<int, List<Elem>> NotifySlotElemVElem3;
	int ID = 0;


public void Start()
	{
WorldCounter = 0;
ElemCounter = 0;
	NotifySlotWorldElemsWorld1 = new Dictionary<int, List<World>>();
	NotifySlotElemCounterElem0 = new Dictionary<int, List<Elem>>();
	NotifySlotElemVElem3 = new Dictionary<int, List<Elem>>();
	NotifySlotWorldElemsWorld1.Add(ID, new List<World>());
	NotifySlotWorldElemsWorld1[ID].Add(this);
WorldWithActiveRules = new Dictionary<int, Tuple<World, RuleTable>>();
ElemWithActiveRules = new Dictionary<int, Tuple<Elem, RuleTable>>();
WorldWithActiveRulesToRemove = new List<int>();
ElemWithActiveRulesToRemove = new List<int>();
		System.Random ___seed00;
		___seed00 = new System.Random(0);
		List<Elem> ___elems00;
		___elems00 = (

(Enumerable.Range(0,(1) + ((10000) - (0))).ToList<System.Int32>()).Select(__ContextSymbol0 => new { ___i01 = __ContextSymbol0 })
.Select(__ContextSymbol1 => new Elem(___seed00))
.ToList<Elem>()).ToList<Elem>();
		Seed = ___seed00;
		Elems = ___elems00;
		
		World1 = new List<Elem>(Elems);
		List<Elem> q;
		q = (

(Elems).Select(__ContextSymbol2 => new { ___e10 = __ContextSymbol2 })
.Where(__ContextSymbol3 => !(__ContextSymbol3.___e10.Counter))
.Select(__ContextSymbol4 => __ContextSymbol4.___e10)
.ToList<Elem>()).ToList<Elem>();
		World1 = q;
}
	public void Init() {		System.Random ___seed00;
		___seed00 = new System.Random(0);
		List<Elem> ___elems00;
		___elems00 = (

(Enumerable.Range(0,(1) + ((10000) - (0))).ToList<System.Int32>()).Select(__ContextSymbol5 => new { ___i01 = __ContextSymbol5 })
.Select(__ContextSymbol6 => new Elem(___seed00))
.ToList<Elem>()).ToList<Elem>();
		Seed = ___seed00;
		Elems = ___elems00;
		
		World1 = new List<Elem>(Elems);
		List<Elem> q;
		q = (

(Elems).Select(__ContextSymbol7 => new { ___e10 = __ContextSymbol7 })
.Where(__ContextSymbol8 => !(__ContextSymbol8.___e10.Counter))
.Select(__ContextSymbol9 => __ContextSymbol9.___e10)
.ToList<Elem>()).ToList<Elem>();
		World1 = q;

}
	public List<Elem> _Elems;
	public System.Random Seed;
	public List<Elem> __World1;
	public List<Elem> World1{  get { return  __World1; }
  set{ __World1 = value;
 Elems= __World1;
foreach(var e in value){if(e.JustEntered){ e.JustEntered = false;
	World.NotifySlotElemCounterElem0[e.ID].Add(e);
	World.NotifySlotElemVElem3[e.ID].Add(e);
}
} }
 }
	public System.Single count_down1;
	public List<Elem> __Elems1;
	public System.Boolean q_temp1;
	public Elem ___s1;
	public System.Int32 counter31;
	public RuleTable ActiveRules = new RuleTable(1);
	public List<Elem> Elems{ 
		get { return _Elems; } 
		
      set { 
			_Elems = value;

q_temp1 = true;
			for(int i = 0; i < World.NotifySlotWorldElemsWorld1[ID].Count; i++) {
var entity = World.NotifySlotWorldElemsWorld1[ID][i]; 
if(!World.WorldWithActiveRules.ContainsKey(entity.ID))
   World.WorldWithActiveRules.Add(entity.ID,new Tuple<World, RuleTable>(entity, new RuleTable(2)));World.WorldWithActiveRules[entity.ID].Item2.Add(1);
}
		}
	}

System.DateTime init_time = System.DateTime.Now;
	public void Update(float dt, World world) {
var t = System.DateTime.Now;

		for(int x0 = 0; x0 < Elems.Count; x0++) { 
			Elems[x0].Update(dt, world);
		}
		this.Rule0(dt, world);

if(WorldWithActiveRules.Count > 0){
foreach(var x in WorldWithActiveRules)
 x.Value.Item1.UpdateSuspendedRules(dt, this, WorldWithActiveRulesToRemove, x.Value.Item2);
if(WorldWithActiveRulesToRemove.Count > 0){
for(int i = 0; i < WorldWithActiveRulesToRemove.Count; i++)
WorldWithActiveRules.Remove(WorldWithActiveRulesToRemove[i]); 
WorldWithActiveRulesToRemove.Clear();
}
 }
if(ElemWithActiveRules.Count > 0){
foreach(var x in ElemWithActiveRules)
 x.Value.Item1.UpdateSuspendedRules(dt, this, ElemWithActiveRulesToRemove, x.Value.Item2);
if(ElemWithActiveRulesToRemove.Count > 0){
for(int i = 0; i < ElemWithActiveRulesToRemove.Count; i++)
ElemWithActiveRules.Remove(ElemWithActiveRulesToRemove[i]); 
ElemWithActiveRulesToRemove.Clear();
}
 }
	}
	public void UpdateSuspendedRules(float dt, World world, List<int> toRemove, RuleTable ActiveRules) { if (ActiveRules.ActiveIndices.Top > 0)
{
  for (int i = 0; i < ActiveRules.ActiveIndices.Top; i++)
  {
    var x = ActiveRules.ActiveIndices.Elements[i];
    switch (x)
    {case 1:
        if (this.Rule1(dt, world) == RuleResult.Done)
        {
          ActiveRules.ActiveSlots[i] = false;
          ActiveRules.ActiveIndices.Top--;
        }
        else{          
          ActiveRules.SupportSlots[1] = true;
          ActiveRules.SupportStack.Push(x);
        }
        break;

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
} }





	int s0=-1;
	public void Rule0(float dt, World world){ 
	switch (s0)
	{

	case -1:
	count_down1 = 5f;
	goto case 2;
	case 2:
	if(((count_down1) > (0f)))
	{

	count_down1 = ((count_down1) - (dt));
	s0 = 2;
return;	}else
	{

	goto case 0;	}
	case 0:
	__Elems1 = (

(Enumerable.Range(0,(1) + ((1000) - (0))).ToList<System.Int32>()).Select(__ContextSymbol10 => new { ___i00 = __ContextSymbol10 })
.Select(__ContextSymbol11 => new Elem(Seed))
.ToList<Elem>()).ToList<Elem>();
	for(int i = 0; ((__Elems1.Count) > (i)); i++){

	{

	((__Elems1)[i])._World = this;
	Elems.Add(__Elems1[i]);	}
}

	s0 = -1;
return;	
	default: return;}}
	




	int s1=-1;
	public RuleResult Rule1(float dt, World world){ 
	switch (s1)
	{

	case -1:
	
	goto case 14;
	case 14:
	if(!(q_temp1))
	{

	s1 = 14;
return RuleResult.Done;	}else
	{

	goto case 13;	}
	case 13:
	q_temp1 = false;
	if(((World1) == (Elems)))
	{

	goto case 11;	}else
	{

	goto case 9;	}
	case 11:
	_Elems = new List<Elem>(Elems);
	goto case 9;
	case 9:
	World1.Clear();
	
	counter31 = -1;
	if((((Elems).Count) == (0)))
	{

	goto case 1;	}else
	{

	___s1 = (Elems)[0];
	goto case 3;	}
	case 3:
	counter31 = ((counter31) + (1));
	if((((((Elems).Count) == (counter31))) || (((counter31) > ((Elems).Count)))))
	{

	goto case 1;	}else
	{

	___s1 = (Elems)[counter31];
	goto case 4;	}
	case 4:
	if(!(___s1.Counter))
	{

	goto case 6;	}else
	{

	goto case 3;	}
	case 6:
	___s1._World = this;
	World1.Add(___s1);
	goto case 3;
	case 1:
	q_temp1 = false;
	Elems = World1;
	s1 = -1;
return RuleResult.Working;	
	default: return RuleResult.Done;}}
	


}
public class Elem{
public int frame;
public bool JustEntered = true;
private System.Random Seed;
	public int ID;
public Elem(System.Random Seed)
	{JustEntered = false;
 frame = World.frame;

this.ID = World.ElemCounter++;
	World.NotifySlotElemCounterElem0.Add(ID, new List<Elem>());
	World.NotifySlotElemVElem3.Add(ID, new List<Elem>());
	World.NotifySlotElemCounterElem0[ID].Add(this);
	World.NotifySlotElemVElem3[ID].Add(this);
		Velocity = Vector3.zero;
		V = -1;
		UnityCube = UnityCube.Instantiate(new UnityEngine.Vector3(((((System.Single)Seed.NextDouble())) * (500)) - (250f),((((System.Single)Seed.NextDouble())) * (300)) - (150f),0f));
		Counter = false;
		
}
	public void Init() {		Velocity = Vector3.zero;
		V = -1;
		UnityCube = UnityCube.Instantiate(new UnityEngine.Vector3(((((System.Single)Seed.NextDouble())) * (500)) - (250f),((((System.Single)Seed.NextDouble())) * (300)) - (150f),0f));
		Counter = false;
		

}
	public System.Boolean _Counter;
	public System.Boolean Destroyed{  get { return UnityCube.Destroyed; }
  set{UnityCube.Destroyed = value; }
 }
	public UnityEngine.Vector3 Position{  get { return UnityCube.Position; }
  set{UnityCube.Position = value; }
 }
	public UnityCube UnityCube;
	public System.Int32 _V;
	public UnityEngine.Vector3 Velocity;
	public System.Boolean enabled{  get { return UnityCube.enabled; }
  set{UnityCube.enabled = value; }
 }
	public UnityEngine.GameObject gameObject{  get { return UnityCube.gameObject; }
 }
	public UnityEngine.HideFlags hideFlags{  get { return UnityCube.hideFlags; }
  set{UnityCube.hideFlags = value; }
 }
	public System.Boolean isActiveAndEnabled{  get { return UnityCube.isActiveAndEnabled; }
 }
	public System.String name{  get { return UnityCube.name; }
  set{UnityCube.name = value; }
 }
	public System.String tag{  get { return UnityCube.tag; }
  set{UnityCube.tag = value; }
 }
	public UnityEngine.Transform transform{  get { return UnityCube.transform; }
 }
	public System.Boolean useGUILayout{  get { return UnityCube.useGUILayout; }
  set{UnityCube.useGUILayout = value; }
 }
	public World _World;
	public System.Boolean _cond01;
	public System.Single count_down2;
	public System.Single count_down3;
	public RuleTable ActiveRules = new RuleTable(2);
	public System.Boolean Counter{ 
		get { return _Counter; } 
		
      set { 
			_Counter = value;
			for(int i = 0; i < World.NotifySlotElemCounterElem0[ID].Count; i++) {
var entity = World.NotifySlotElemCounterElem0[ID][i]; 
if (entity.frame == World.frame){ if(!World.ElemWithActiveRules.ContainsKey(entity.ID))
   World.ElemWithActiveRules.Add(entity.ID,new Tuple<Elem, RuleTable>(entity, new RuleTable(4)));World.ElemWithActiveRules[entity.ID].Item2.Add(0);
}else{ entity.JustEntered = true;
World.NotifySlotElemCounterElem0.Remove(entity.ID);
World.NotifySlotElemVElem3.Remove(entity.ID);
			 }}
		}
	}
	public System.Int32 V{ 
		get { return _V; } 
		
      set { 
			_V = value;
			for(int i = 0; i < World.NotifySlotElemVElem3[ID].Count; i++) {
var entity = World.NotifySlotElemVElem3[ID][i]; 
if (entity.frame == World.frame){ if(!World.ElemWithActiveRules.ContainsKey(entity.ID))
   World.ElemWithActiveRules.Add(entity.ID,new Tuple<Elem, RuleTable>(entity, new RuleTable(4)));World.ElemWithActiveRules[entity.ID].Item2.Add(3);
}else{ entity.JustEntered = true;
World.NotifySlotElemCounterElem0.Remove(entity.ID);
World.NotifySlotElemVElem3.Remove(entity.ID);
			 }}
		}
	}
	public void Update(float dt, World world) {
frame = World.frame;		this.Rule2(dt, world);

		this.Rule1(dt, world);

	}
	public void UpdateSuspendedRules(float dt, World world, List<int> toRemove, RuleTable ActiveRules) { if (ActiveRules.ActiveIndices.Top > 0 && frame == World.frame)
{
  for (int i = 0; i < ActiveRules.ActiveIndices.Top; i++)
  {
    var x = ActiveRules.ActiveIndices.Elements[i];
    switch (x)
    {case 0:
        if (this.Rule0(dt, world) == RuleResult.Done)
        {
          ActiveRules.ActiveSlots[i] = false;
          ActiveRules.ActiveIndices.Top--;
        }
        else{          
          ActiveRules.SupportSlots[0] = true;
          ActiveRules.SupportStack.Push(x);
        }
        break;
case 3:
        if (this.Rule3(dt, world) == RuleResult.Done)
        {
          ActiveRules.ActiveSlots[i] = false;
          ActiveRules.ActiveIndices.Top--;
        }
        else{          
          ActiveRules.SupportSlots[3] = true;
          ActiveRules.SupportStack.Push(x);
        }
        break;

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
}else
{
  if (this.frame != World.frame)
    toRemove.Add(ID);
} }

	public void Rule2(float dt, World world) 
	{
	Position = (Position) + ((Velocity) * (dt));
	}
	




	int s1=-1;
	public void Rule1(float dt, World world){ 
	switch (s1)
	{

	case -1:
	count_down2 = ((((((System.Single)world.Seed.NextDouble())) * (6))) + (8f));
	goto case 2;
	case 2:
	if(((count_down2) > (0f)))
	{

	count_down2 = ((count_down2) - (dt));
	s1 = 2;
return;	}else
	{

	goto case 0;	}
	case 0:
	Velocity = new UnityEngine.Vector3(((((System.Single)world.Seed.NextDouble())) * (10f)) - (5f),((((System.Single)world.Seed.NextDouble())) * (10f)) - (5f),0f);
	V = 0;
	s1 = -1;
return;	
	default: return;}}
	




	int s0=-1;
	public RuleResult Rule0(float dt, World world){ 
	switch (s0)
	{

	case -1:
	_cond01 = Counter;
	goto case 11;
	case 11:
	if(!(((!(((_cond01) == (Counter)))) || (false))))
	{

	s0 = 11;
return RuleResult.Done;	}else
	{

	goto case 2;	}
	case 2:
	if(!(Counter))
	{

	goto case 0;	}else
	{

	goto case 1;	}
	case 0:
	if(!(_World.World1.Contains(this)))
	{

	goto case 3;	}else
	{

	goto case 4;	}
	case 3:
	_World.World1.Add(this);
	goto case -1;
	case 4:
	goto case -1;
	case 1:
	_World.World1.Remove(this);
	goto case -1;	
	default: return RuleResult.Done;}}
	

	int s3=-1;
	public RuleResult Rule3(float dt, World world){ 
	switch (s3)
	{

	case -1:
	if(!(((V) == (0))))
	{

	s3 = -1;
return RuleResult.Done;	}else
	{

	goto case 3;	}
	case 3:
	Counter = false;
	Destroyed = false;
	s3 = 1;
return RuleResult.Working;
	case 1:
	count_down3 = ((((((System.Single)world.Seed.NextDouble())) * (2f))) + (5f));
	goto case 2;
	case 2:
	if(((count_down3) > (0f)))
	{

	count_down3 = ((count_down3) - (dt));
	s3 = 2;
return RuleResult.Working;	}else
	{

	goto case 0;	}
	case 0:
	Counter = true;
	Destroyed = true;
	s3 = -1;
return RuleResult.Working;	
	default: return RuleResult.Done;}}
	

}
}    