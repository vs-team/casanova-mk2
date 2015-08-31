#pragma warning disable 162,108,618
using Casanova.Prelude;
using System.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;
public class World : MonoBehaviour{
  void OnApplicationQuit() { file.Close(); }public static int frame;
void Update () { Update(Time.deltaTime, this); 
 frame++; }
public bool JustEntered = true;
static public int CubeCounter;
static public Dictionary<int, Tuple<Cube, RuleTable>> CubeWithActiveRules;
static public List<int> CubeWithActiveRulesToRemove;
	static public  Dictionary<int, List<Cube>> NotifySlotCubeDelayFactorCube0;

System.IO.StreamWriter file;
public void Start()
	{
file = new System.IO.StreamWriter(System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(),"test.csv"));CubeCounter = 0;
	NotifySlotCubeDelayFactorCube0 = new Dictionary<int, List<Cube>>();
CubeWithActiveRules = new Dictionary<int, Tuple<Cube, RuleTable>>();
CubeWithActiveRulesToRemove = new List<int>();
		World1 = new List<Cube>();
		Timer = false;
		Seed = new System.Random(0);
		DelayedCubes = (

Enumerable.Empty<Cube>()).ToList<Cube>();
		Cubes = (

Enumerable.Empty<Cube>()).ToList<Cube>();
		
		World1 = new List<Cube>();
		List<Cube> q;
		q = (

(Cubes).Select(__ContextSymbol2 => new { ___c11 = __ContextSymbol2 })
.Where(__ContextSymbol3 => ((__ContextSymbol3.___c11.DelayFactor) > (0.7f)))
.Select(__ContextSymbol4 => __ContextSymbol4.___c11)
.ToList<Cube>()).ToList<Cube>();
		World1.AddRange(q);
}
		public List<Cube> __Cubes;
	public List<Cube> Cubes{  get { return  __Cubes; }
  set{ __Cubes = value;
 foreach(var e in value){if(e.JustEntered){ e.JustEntered = false;
	World.NotifySlotCubeDelayFactorCube0[e.ID].Add(e);
}
} }
 }
	public List<Cube> __DelayedCubes;
	public List<Cube> DelayedCubes{  get { return  __DelayedCubes; }
  set{ __DelayedCubes = value;
 foreach(var e in value){if(e.JustEntered){ e.JustEntered = false;
	World.NotifySlotCubeDelayFactorCube0[e.ID].Add(e);
}
} }
 }
	public System.Random Seed;
	public System.Boolean Timer;
	public System.Boolean World1;
	public Cube s;
	public System.Int32 counter3;
	public System.Single count_down1;
	public System.Single count_down2;
	public System.Single count_down3;

System.DateTime init_time = System.DateTime.Now;
	public void Update(float dt, World world) {
var t = System.DateTime.Now;		this.Rule0(dt, world);

		for(int x0 = 0; x0 < Cubes.Count; x0++) { 
			Cubes[x0].Update(dt, world);
		}
		this.Rule1(dt, world);
		this.Rule2(dt, world);
		this.Rule3(dt, world);
		this.Rule4(dt, world);
if(CubeWithActiveRules.Count > 0){
foreach(var x in CubeWithActiveRules)
 x.Value.Item1.UpdateSuspendedRules(dt, this, CubeWithActiveRulesToRemove, x.Value.Item2);
if(CubeWithActiveRulesToRemove.Count > 0){
for(int i = 0; i < CubeWithActiveRulesToRemove.Count; i++)
CubeWithActiveRules.Remove(CubeWithActiveRulesToRemove[i]); 
CubeWithActiveRulesToRemove.Clear();
}
 }
var t1 = System.DateTime.Now;
file.WriteLine((t1 - t).Milliseconds + "," + (t1 - init_time).Seconds);	}

	public void Rule0(float dt, World world) 
	{
	Cubes = (

(Cubes).Select(__ContextSymbol5 => new { ___c00 = __ContextSymbol5 })
.Where(__ContextSymbol6 => ((!(DelayedCubes.Contains(__ContextSymbol6.___c00))) && (((__ContextSymbol6.___c00.UnityCube.Destroyed) == (false)))))
.Select(__ContextSymbol7 => __ContextSymbol7.___c00)
.ToList<Cube>()).ToList<Cube>();
	}
	




	int s1=-1;
	public void Rule1(float dt, World world){ 
	switch (s1)
	{

	case -1:
	if(!(!(q_temp1)))
	{

	s1 = -1;
return;	}else
	{

	goto case 2;	}
	case 2:
	
	counter3 = -1;
	if((((Cubes).Count) == (0)))
	{

	goto case 1;	}else
	{

	s = (Cubes)[0];
	goto case 3;	}
	case 3:
	counter3 = ((counter3) + (1));
	if((((((Cubes).Count) == (counter3))) || (((counter3) > ((Cubes).Count)))))
	{

	goto case 1;	}else
	{

	s = (Cubes)[counter3];
	goto case 4;	}
	case 4:
	if(((s.DelayFactor) > (0.7f)))
	{

	goto case 5;	}else
	{

	goto case 6;	}
	case 5:
	s._World = this;
	World1.Add(s._World);
	s1 = 3;
return;
	case 6:
	s._World = null;
	s1 = 3;
return;
	case 1:
	q_temp1 = false;
	DelayedCubes = World1;
	s1 = -1;
return;	
	default: return;}}
	

	int s2=-1;
	public void Rule2(float dt, World world){ 
	switch (s2)
	{

	case -1:
	count_down1 = 1f;
	goto case 2;
	case 2:
	if(((count_down1) > (0f)))
	{

	count_down1 = ((count_down1) - (dt));
	s2 = 2;
return;	}else
	{

	goto case 0;	}
	case 0:
	Cubes = (Cubes).Concat(DelayedCubes).ToList<Cube>();
	DelayedCubes = (

Enumerable.Empty<Cube>()).ToList<Cube>();
	s2 = -1;
return;	
	default: return;}}
	

	int s3=-1;
	public void Rule3(float dt, World world){ 
	switch (s3)
	{

	case -1:
	count_down2 = 2f;
	goto case 3;
	case 3:
	if(((count_down2) > (0f)))
	{

	count_down2 = ((count_down2) - (dt));
	s3 = 3;
return;	}else
	{

	goto case 1;	}
	case 1:
	Timer = true;
	s3 = 0;
return;
	case 0:
	Timer = false;
	s3 = -1;
return;	
	default: return;}}
	

	int s4=-1;
	public void Rule4(float dt, World world){ 
	switch (s4)
	{

	case -1:
	count_down3 = 2f;
	goto case 2;
	case 2:
	if(((count_down3) > (0f)))
	{

	count_down3 = ((count_down3) - (dt));
	s4 = 2;
return;	}else
	{

	goto case 0;	}
	case 0:
	Cubes = ((

(Enumerable.Range(0,(1) + ((500) - (0))).ToList<System.Int32>()).Select(__ContextSymbol9 => new { ___i40 = __ContextSymbol9 })
.Select(__ContextSymbol10 => new Cube(new UnityEngine.Vector3(-100f,(-40f) + ((__ContextSymbol10.___i40) * (2f)),80f)))
.ToList<Cube>()).ToList<Cube>()).Concat(Cubes).ToList<Cube>();
	s4 = -1;
return;	
	default: return;}}
	





}
public class Cube{
public int frame;
public bool JustEntered = true;
private UnityEngine.Vector3 p;
	public int ID;
public Cube(UnityEngine.Vector3 p)
	{JustEntered = false;
 frame = World.frame;

this.ID = World.CubeCounter++;
	World.NotifySlotCubeDelayFactorCube0.Add(ID, new List<Cube>());
	World.NotifySlotCubeDelayFactorCube0[ID].Add(this);
		List<System.Int32> ___q00;
		___q00 = (

(new Cons<System.Int32>(1,(new Empty<System.Int32>()).ToList<System.Int32>())).ToList<System.Int32>()).ToList<System.Int32>();
		Velocity = new UnityEngine.Vector3(5f,0f,0f);
		UnityCube = UnityCube.Instantiate(p,___q00);
		DelayFactor = 0f;
		
}
	public void Init() {		List<System.Int32> ___q00;
		___q00 = (

(new Cons<System.Int32>(1,(new Empty<System.Int32>()).ToList<System.Int32>())).ToList<System.Int32>()).ToList<System.Int32>();
		Velocity = new UnityEngine.Vector3(5f,0f,0f);
		UnityCube = UnityCube.Instantiate(p,___q00);
		DelayFactor = 0f;
		

}
	public UnityEngine.Color Color{  set{UnityCube.Color = value; }
 }
	public System.Single _DelayFactor;
	public System.Boolean Destroyed{  get { return UnityCube.Destroyed; }
  set{UnityCube.Destroyed = value; }
 }
	public UnityEngine.Vector3 Position{  get { return UnityCube.Position; }
  set{UnityCube.Position = value; }
 }
	public System.Single Scale{  get { return UnityCube.Scale; }
  set{UnityCube.Scale = value; }
 }
	public UnityCube UnityCube;
	public UnityEngine.Vector3 Velocity;
	public UnityEngine.Animation animation{  get { return UnityCube.animation; }
 }
	public UnityEngine.AudioSource audio{  get { return UnityCube.audio; }
 }
	public UnityEngine.Camera camera{  get { return UnityCube.camera; }
 }
	public UnityEngine.Collider collider{  get { return UnityCube.collider; }
 }
	public UnityEngine.Collider2D collider2D{  get { return UnityCube.collider2D; }
 }
	public UnityEngine.ConstantForce constantForce{  get { return UnityCube.constantForce; }
 }
	public System.Boolean enabled{  get { return UnityCube.enabled; }
  set{UnityCube.enabled = value; }
 }
	public UnityEngine.GameObject gameObject{  get { return UnityCube.gameObject; }
 }
	public UnityEngine.GUIElement guiElement{  get { return UnityCube.guiElement; }
 }
	public UnityEngine.GUIText guiText{  get { return UnityCube.guiText; }
 }
	public UnityEngine.GUITexture guiTexture{  get { return UnityCube.guiTexture; }
 }
	public UnityEngine.HideFlags hideFlags{  get { return UnityCube.hideFlags; }
  set{UnityCube.hideFlags = value; }
 }
	public UnityEngine.HingeJoint hingeJoint{  get { return UnityCube.hingeJoint; }
 }
	public UnityEngine.Light light{  get { return UnityCube.light; }
 }
	public System.String name{  get { return UnityCube.name; }
  set{UnityCube.name = value; }
 }
	public UnityEngine.ParticleEmitter particleEmitter{  get { return UnityCube.particleEmitter; }
 }
	public UnityEngine.ParticleSystem particleSystem{  get { return UnityCube.particleSystem; }
 }
	public UnityEngine.Renderer renderer{  get { return UnityCube.renderer; }
 }
	public UnityEngine.Rigidbody rigidbody{  get { return UnityCube.rigidbody; }
 }
	public UnityEngine.Rigidbody2D rigidbody2D{  get { return UnityCube.rigidbody2D; }
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
	public System.Single _cond1;
	public RuleTable ActiveRules = new RuleTable(1);
	public System.Single DelayFactor{ 
		get { return _DelayFactor; } 
		set { 
			_DelayFactor = value;
			for(int i = 0; i < World.NotifySlotCubeDelayFactorCube0[ID].Count; i++) {
var entity = World.NotifySlotCubeDelayFactorCube0[ID][i]; 
if (entity.frame == World.frame){ World.CubeWithActiveRules[entity.ID].Item2.Add(0);
if(!World.CubeWithActiveRules.ContainsKey(entity.ID))
   World.CubeWithActiveRules.Add(entity.ID,new Tuple<Cube, RuleTable>(entity, new RuleTable(4)));}else{ entity.JustEntered = true;
World.NotifySlotCubeDelayFactorCube0.Remove(entity.ID);
			 }}
		}
	}
	public void Update(float dt, World world) {
frame = World.frame;		this.Rule1(dt, world);

		this.Rule2(dt, world);
		this.Rule3(dt, world);
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

	public void Rule1(float dt, World world) 
	{
	Position = (Position) + ((Velocity) * (dt));
	}
	




	int s2=-1;
	public void Rule2(float dt, World world){ 
	switch (s2)
	{

	case -1:
	if(!(((((Position.x) > (100f))) || (((Position.x) == (100f))))))
	{

	s2 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	Destroyed = true;
	s2 = -1;
return;	
	default: return;}}
	

	int s3=-1;
	public void Rule3(float dt, World world){ 
	switch (s3)
	{

	case -1:
	if(!(world.Timer))
	{

	s3 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	DelayFactor = ((System.Single)world.Seed.NextDouble());
	s3 = -1;
return;	
	default: return;}}
	



	int s0=-1;
	public RuleResult Rule0(float dt, World world){ 
	switch (s0)
	{

	case -1:
	_cond1 = DelayFactor;
	goto case 11;
	case 11:
	if(!(!(((_cond1) == (DelayFactor)))))
	{

	s0 = 11;
return RuleResult.Done;	}else
	{

	goto case 2;	}
	case 2:
	if(((DelayFactor) > (0.7f)))
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
	


}
   