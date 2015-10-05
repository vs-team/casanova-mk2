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


public void Start()
	{
		UnityParticleSystem = UnityParticleSystem.Find();
		Ship = new Ship();
		
}
		public Ship Ship;
	public UnityParticleSystem UnityParticleSystem;

System.DateTime init_time = System.DateTime.Now;
	public void Update(float dt, World world) {
var t = System.DateTime.Now;

		Ship.Update(dt, world);
		this.Rule0(dt, world);

	}





	int s0=-1;
	public void Rule0(float dt, World world){ 
	switch (s0)
	{

	case -1:
	if(((Ship.FrontEngine.VelocityFactor.z) == (0)))
	{

	goto case 0;	}else
	{

	goto case 1;	}
	case 0:
	UnityParticleSystem.Speed = 10f;
	s0 = -1;
return;
	case 1:
	UnityParticleSystem.Speed = ((1000f) * (Ship.FrontEngine.VelocityFactor.z));
	s0 = -1;
return;	
	default: return;}}
	






}
public class Ship{
public int frame;
public bool JustEntered = true;
	public int ID;
public Ship()
	{JustEntered = false;
 frame = World.frame;
		UnityShip = UnityShip.Instantiate();
		SideEngine = new Engine((

(new Cons<Casanova.Prelude.Tuple<UnityEngine.KeyCode, Casanova.Prelude.Tuple<System.Single, UnityEngine.Vector3>>>(new Casanova.Prelude.Tuple<UnityEngine.KeyCode, Casanova.Prelude.Tuple<System.Single, UnityEngine.Vector3>>(KeyCode.A,new Casanova.Prelude.Tuple<System.Single, UnityEngine.Vector3>(20f,new UnityEngine.Vector3(-1f,0f,0f))),(new Cons<Casanova.Prelude.Tuple<UnityEngine.KeyCode, Casanova.Prelude.Tuple<System.Single, UnityEngine.Vector3>>>(new Casanova.Prelude.Tuple<UnityEngine.KeyCode, Casanova.Prelude.Tuple<System.Single, UnityEngine.Vector3>>(KeyCode.D,new Casanova.Prelude.Tuple<System.Single, UnityEngine.Vector3>(-20f,new UnityEngine.Vector3(1f,0f,0f))),(new Empty<Casanova.Prelude.Tuple<UnityEngine.KeyCode, Casanova.Prelude.Tuple<System.Single, UnityEngine.Vector3>>>()).ToList<Casanova.Prelude.Tuple<UnityEngine.KeyCode, Casanova.Prelude.Tuple<System.Single, UnityEngine.Vector3>>>())).ToList<Casanova.Prelude.Tuple<UnityEngine.KeyCode, Casanova.Prelude.Tuple<System.Single, UnityEngine.Vector3>>>())).ToList<Casanova.Prelude.Tuple<UnityEngine.KeyCode, Casanova.Prelude.Tuple<System.Single, UnityEngine.Vector3>>>()).ToList<Casanova.Prelude.Tuple<UnityEngine.KeyCode, Casanova.Prelude.Tuple<System.Single, UnityEngine.Vector3>>>());
		FrontEngine = new Engine((

(new Cons<Casanova.Prelude.Tuple<UnityEngine.KeyCode, Casanova.Prelude.Tuple<System.Single, UnityEngine.Vector3>>>(new Casanova.Prelude.Tuple<UnityEngine.KeyCode, Casanova.Prelude.Tuple<System.Single, UnityEngine.Vector3>>(KeyCode.X,new Casanova.Prelude.Tuple<System.Single, UnityEngine.Vector3>(25f,new UnityEngine.Vector3(0f,0f,3f))),(new Cons<Casanova.Prelude.Tuple<UnityEngine.KeyCode, Casanova.Prelude.Tuple<System.Single, UnityEngine.Vector3>>>(new Casanova.Prelude.Tuple<UnityEngine.KeyCode, Casanova.Prelude.Tuple<System.Single, UnityEngine.Vector3>>(KeyCode.W,new Casanova.Prelude.Tuple<System.Single, UnityEngine.Vector3>(20f,new UnityEngine.Vector3(0f,0f,1f))),(new Cons<Casanova.Prelude.Tuple<UnityEngine.KeyCode, Casanova.Prelude.Tuple<System.Single, UnityEngine.Vector3>>>(new Casanova.Prelude.Tuple<UnityEngine.KeyCode, Casanova.Prelude.Tuple<System.Single, UnityEngine.Vector3>>(KeyCode.S,new Casanova.Prelude.Tuple<System.Single, UnityEngine.Vector3>(-20f,new UnityEngine.Vector3(0f,0f,-1f))),(new Empty<Casanova.Prelude.Tuple<UnityEngine.KeyCode, Casanova.Prelude.Tuple<System.Single, UnityEngine.Vector3>>>()).ToList<Casanova.Prelude.Tuple<UnityEngine.KeyCode, Casanova.Prelude.Tuple<System.Single, UnityEngine.Vector3>>>())).ToList<Casanova.Prelude.Tuple<UnityEngine.KeyCode, Casanova.Prelude.Tuple<System.Single, UnityEngine.Vector3>>>())).ToList<Casanova.Prelude.Tuple<UnityEngine.KeyCode, Casanova.Prelude.Tuple<System.Single, UnityEngine.Vector3>>>())).ToList<Casanova.Prelude.Tuple<UnityEngine.KeyCode, Casanova.Prelude.Tuple<System.Single, UnityEngine.Vector3>>>()).ToList<Casanova.Prelude.Tuple<UnityEngine.KeyCode, Casanova.Prelude.Tuple<System.Single, UnityEngine.Vector3>>>());
		
}
		public Engine FrontEngine;
	public UnityEngine.Vector3 Position{  get { return UnityShip.Position; }
  set{UnityShip.Position = value; }
 }
	public UnityEngine.Quaternion Rotation{  get { return UnityShip.Rotation; }
  set{UnityShip.Rotation = value; }
 }
	public Engine SideEngine;
	public UnityShip UnityShip;
	public System.Boolean enabled{  get { return UnityShip.enabled; }
  set{UnityShip.enabled = value; }
 }
	public UnityEngine.GameObject gameObject{  get { return UnityShip.gameObject; }
 }
	public UnityEngine.HideFlags hideFlags{  get { return UnityShip.hideFlags; }
  set{UnityShip.hideFlags = value; }
 }
	public System.Boolean isActiveAndEnabled{  get { return UnityShip.isActiveAndEnabled; }
 }
	public System.String name{  get { return UnityShip.name; }
  set{UnityShip.name = value; }
 }
	public System.String tag{  get { return UnityShip.tag; }
  set{UnityShip.tag = value; }
 }
	public UnityEngine.Transform transform{  get { return UnityShip.transform; }
 }
	public System.Boolean useGUILayout{  get { return UnityShip.useGUILayout; }
  set{UnityShip.useGUILayout = value; }
 }
	public void Update(float dt, World world) {
frame = World.frame;

		FrontEngine.Update(dt, world);
		SideEngine.Update(dt, world);
		this.Rule0(dt, world);
		this.Rule1(dt, world);
	}





	int s0=-1;
	public void Rule0(float dt, World world){ 
	switch (s0)
	{

	case -1:
	Position = ((((Position) + (FrontEngine.VelocityFactor))) + (SideEngine.VelocityFactor));
	s0 = -1;
return;	
	default: return;}}
	

	int s1=-1;
	public void Rule1(float dt, World world){ 
	switch (s1)
	{

	case -1:
	Rotation = UnityEngine.Quaternion.Euler(FrontEngine.RotationFactor,0f,SideEngine.RotationFactor);
	s1 = -1;
return;	
	default: return;}}
	





}
public class Engine{
public int frame;
public bool JustEntered = true;
private List<Casanova.Prelude.Tuple<UnityEngine.KeyCode, Casanova.Prelude.Tuple<System.Single, UnityEngine.Vector3>>> controllers;
	public int ID;
public Engine(List<Casanova.Prelude.Tuple<UnityEngine.KeyCode, Casanova.Prelude.Tuple<System.Single, UnityEngine.Vector3>>> controllers)
	{JustEntered = false;
 frame = World.frame;
		VelocityFactor = Vector3.zero;
		RotationFactor = 0f;
		KeyPressed = (new Nothing<Casanova.Prelude.Tuple<UnityEngine.KeyCode, Casanova.Prelude.Tuple<System.Single, UnityEngine.Vector3>>>());
		Controllers = controllers;
		
}
		public List<Casanova.Prelude.Tuple<UnityEngine.KeyCode, Casanova.Prelude.Tuple<System.Single, UnityEngine.Vector3>>> Controllers;
	public Option<Casanova.Prelude.Tuple<UnityEngine.KeyCode, Casanova.Prelude.Tuple<System.Single, UnityEngine.Vector3>>> KeyPressed;
	public System.Single RotationFactor;
	public UnityEngine.Vector3 VelocityFactor;
	public Casanova.Prelude.Tuple<UnityEngine.KeyCode, Casanova.Prelude.Tuple<System.Single, UnityEngine.Vector3>> ___key_pressed00;
	public Casanova.Prelude.Tuple<UnityEngine.KeyCode, Casanova.Prelude.Tuple<System.Single, UnityEngine.Vector3>> ___key_pressed11;
	public List<Casanova.Prelude.Tuple<UnityEngine.KeyCode, Casanova.Prelude.Tuple<System.Single, UnityEngine.Vector3>>> ___controllers20;
	public void Update(float dt, World world) {
frame = World.frame;

if(KeyPressed.IsSome){  } 
		this.Rule0(dt, world);
		this.Rule1(dt, world);
		this.Rule2(dt, world);
	}





	int s0=-1;
	public void Rule0(float dt, World world){ 
	switch (s0)
	{

	case -1:
	if(!(KeyPressed.IsSome))
	{

	s0 = -1;
return;	}else
	{

	goto case 19;	}
	case 19:
	___key_pressed00 = KeyPressed.Value;
	if(((___key_pressed00.Item2.Item1) > (0)))
	{

	goto case 10;	}else
	{

	goto case 11;	}
	case 10:
	if(!(((((___key_pressed00.Item2.Item1) > (RotationFactor))) && (UnityEngine.Input.GetKey(___key_pressed00.Item1)))))
	{

	s0 = 9;
return;	}else
	{

	goto case 14;	}
	case 14:
	RotationFactor = ((RotationFactor) + (((dt) * (20f))));
	s0 = 10;
return;
	case 11:
	if(!(((((RotationFactor) > (___key_pressed00.Item2.Item1))) && (UnityEngine.Input.GetKey(___key_pressed00.Item1)))))
	{

	s0 = 9;
return;	}else
	{

	goto case 17;	}
	case 17:
	RotationFactor = ((RotationFactor) - (((dt) * (20f))));
	s0 = 11;
return;
	case 9:
	if(!(!(UnityEngine.Input.GetKey(___key_pressed00.Item1))))
	{

	s0 = 9;
return;	}else
	{

	goto case 2;	}
	case 2:
	if(((RotationFactor) > (0)))
	{

	goto case 0;	}else
	{

	goto case 1;	}
	case 0:
	if(!(((((RotationFactor) > (0))) && (!(UnityEngine.Input.GetKey(___key_pressed00.Item1))))))
	{

	s0 = -1;
return;	}else
	{

	goto case 4;	}
	case 4:
	RotationFactor = ((RotationFactor) - (((dt) * (20f))));
	s0 = 0;
return;
	case 1:
	if(!(((((0) > (RotationFactor))) && (!(UnityEngine.Input.GetKey(___key_pressed00.Item1))))))
	{

	s0 = -1;
return;	}else
	{

	goto case 7;	}
	case 7:
	RotationFactor = ((RotationFactor) + (((dt) * (20f))));
	s0 = 1;
return;	
	default: return;}}
	

	int s1=-1;
	public void Rule1(float dt, World world){ 
	switch (s1)
	{

	case -1:
	if(!(KeyPressed.IsSome))
	{

	s1 = -1;
return;	}else
	{

	goto case 3;	}
	case 3:
	___key_pressed11 = KeyPressed.Value;
	VelocityFactor = ((___key_pressed11.Item2.Item2) * (dt));
	s1 = 1;
return;
	case 1:
	if(!(!(UnityEngine.Input.GetKey(___key_pressed11.Item1))))
	{

	s1 = 1;
return;	}else
	{

	goto case 0;	}
	case 0:
	VelocityFactor = Vector3.zero;
	s1 = -1;
return;	
	default: return;}}
	

	int s2=-1;
	public void Rule2(float dt, World world){ 
	switch (s2)
	{

	case -1:
	___controllers20 = (

(Controllers).Select(__ContextSymbol4 => new { ___c20 = __ContextSymbol4 })
.Where(__ContextSymbol5 => UnityEngine.Input.GetKey(__ContextSymbol5.___c20.Item1))
.Select(__ContextSymbol6 => __ContextSymbol6.___c20)
.ToList<Casanova.Prelude.Tuple<UnityEngine.KeyCode, Casanova.Prelude.Tuple<System.Single, UnityEngine.Vector3>>>()).ToList<Casanova.Prelude.Tuple<UnityEngine.KeyCode, Casanova.Prelude.Tuple<System.Single, UnityEngine.Vector3>>>();
	if(((___controllers20.Count) > (0)))
	{

	goto case 5;	}else
	{

	goto case 6;	}
	case 5:
	KeyPressed = (new Just<Casanova.Prelude.Tuple<UnityEngine.KeyCode, Casanova.Prelude.Tuple<System.Single, UnityEngine.Vector3>>>(___controllers20.Head()));
	s2 = -1;
return;
	case 6:
	KeyPressed = (new Nothing<Casanova.Prelude.Tuple<UnityEngine.KeyCode, Casanova.Prelude.Tuple<System.Single, UnityEngine.Vector3>>>());
	s2 = -1;
return;	
	default: return;}}
	





}
}