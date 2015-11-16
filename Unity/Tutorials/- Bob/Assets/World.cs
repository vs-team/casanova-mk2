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
		bobbies = (

(new Cons<Bob>(new Bob("1"),(new Cons<Bob>(new Bob("2"),(new Cons<Bob>(new Bob("3"),(new Empty<Bob>()).ToList<Bob>())).ToList<Bob>())).ToList<Bob>())).ToList<Bob>()).ToList<Bob>();
		MainCamera = new MainCamera();
		
}
		public MainCamera MainCamera;
	public List<Bob> __bobbies;
	public List<Bob> bobbies{  get { return  __bobbies; }
  set{ __bobbies = value;
 foreach(var e in value){if(e.JustEntered){ e.JustEntered = false;
}
} }
 }

System.DateTime init_time = System.DateTime.Now;
	public void Update(float dt, World world) {
var t = System.DateTime.Now;

		MainCamera.Update(dt, world);
		for(int x0 = 0; x0 < bobbies.Count; x0++) { 
			bobbies[x0].Update(dt, world);
		}


	}











}
public class Bob{
public int frame;
public bool JustEntered = true;
private System.String nameBob;
	public int ID;
public Bob(System.String nameBob)
	{JustEntered = false;
 frame = World.frame;
		UnityBob = UnityBob.Find(nameBob);
		
}
		public System.Collections.Generic.List<UnityEngine.Vector3> Checkpoints{  get { return UnityBob.Checkpoints; }
 }
	public BobAnimation CurrentAnimation{  set{UnityBob.CurrentAnimation = value; }
 }
	public UnityEngine.Vector3 Forward{  get { return UnityBob.Forward; }
 }
	public System.Boolean IsHit{  get { return UnityBob.IsHit; }
 }
	public System.Boolean MouseHover{  get { return UnityBob.MouseHover; }
  set{UnityBob.MouseHover = value; }
 }
	public UnityEngine.Vector3 Position{  get { return UnityBob.Position; }
  set{UnityBob.Position = value; }
 }
	public System.Boolean Quit{  set{UnityBob.Quit = value; }
 }
	public System.Boolean Selected{  get { return UnityBob.Selected; }
  set{UnityBob.Selected = value; }
 }
	public UnityBob UnityBob;
	public UnityEngine.Vector3 Velocity{  get { return UnityBob.Velocity; }
  set{UnityBob.Velocity = value; }
 }
	public System.Boolean enabled{  get { return UnityBob.enabled; }
  set{UnityBob.enabled = value; }
 }
	public UnityEngine.GameObject gameObject{  get { return UnityBob.gameObject; }
 }
	public UnityEngine.HideFlags hideFlags{  get { return UnityBob.hideFlags; }
  set{UnityBob.hideFlags = value; }
 }
	public System.Boolean isActiveAndEnabled{  get { return UnityBob.isActiveAndEnabled; }
 }
	public System.String name{  get { return UnityBob.name; }
  set{UnityBob.name = value; }
 }
	public System.String tag{  get { return UnityBob.tag; }
  set{UnityBob.tag = value; }
 }
	public UnityEngine.Transform transform{  get { return UnityBob.transform; }
 }
	public System.Boolean useGUILayout{  get { return UnityBob.useGUILayout; }
  set{UnityBob.useGUILayout = value; }
 }
	public void Update(float dt, World world) {
frame = World.frame;		this.Rule0(dt, world);

		this.Rule1(dt, world);
		this.Rule2(dt, world);
		this.Rule3(dt, world);
	}

	public void Rule0(float dt, World world) 
	{
	Position = (Position) + ((Velocity) * (dt));
	}
	




	int s1=-1;
	public void Rule1(float dt, World world){ 
	switch (s1)
	{

	case -1:
	if(!(UnityEngine.Input.GetMouseButtonDown(0)))
	{

	s1 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	Selected = IsHit;
	s1 = -1;
return;	
	default: return;}}
	

	int s2=-1;
	public void Rule2(float dt, World world){ 
	switch (s2)
	{

	case -1:
	MouseHover = IsHit;
	s2 = -1;
return;	
	default: return;}}
	

	int s3=-1;
	public void Rule3(float dt, World world){ 
	switch (s3)
	{

	case -1:
	if(!(((!(Selected)) || (true))))
	{

	s3 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	if(!(Selected))
	{

	goto case 2;	}else
	{

	if(true)
	{

	goto case 3;	}else
	{

	s3 = 0;
return;	}	}
	case 2:
	Velocity = Vector3.zero;
	CurrentAnimation = BobAnimation.Idle;
	s3 = -1;
return;
	case 3:
	Velocity = ((Velocity) + (((Forward) * (dt))));
	CurrentAnimation = BobAnimation.Walk;
	s3 = -1;
return;	
	default: return;}}
	





}
public class MainCamera{
public int frame;
public bool JustEntered = true;
	public int ID;
public MainCamera()
	{JustEntered = false;
 frame = World.frame;
		UnityCamera = UnityCamera.Find();
		PlusSpeed = 2f;
		MinusSpeed = -2f;
		MaxVelocity = 8f;
		
}
		public UnityEngine.Vector3 Backward{  get { return UnityCamera.Backward; }
 }
	public UnityEngine.Vector3 Down{  get { return UnityCamera.Down; }
 }
	public UnityEngine.Vector3 Forward{  get { return UnityCamera.Forward; }
 }
	public System.Single MaxVelocity;
	public System.Single MinusSpeed;
	public System.Single PlusSpeed;
	public UnityEngine.Vector3 Position{  get { return UnityCamera.Position; }
  set{UnityCamera.Position = value; }
 }
	public UnityEngine.Quaternion Rotation{  get { return UnityCamera.Rotation; }
  set{UnityCamera.Rotation = value; }
 }
	public UnityCamera UnityCamera;
	public UnityEngine.Vector3 Up{  get { return UnityCamera.Up; }
 }
	public System.Boolean enabled{  get { return UnityCamera.enabled; }
  set{UnityCamera.enabled = value; }
 }
	public UnityEngine.GameObject gameObject{  get { return UnityCamera.gameObject; }
 }
	public UnityEngine.HideFlags hideFlags{  get { return UnityCamera.hideFlags; }
  set{UnityCamera.hideFlags = value; }
 }
	public System.Boolean isActiveAndEnabled{  get { return UnityCamera.isActiveAndEnabled; }
 }
	public System.String name{  get { return UnityCamera.name; }
  set{UnityCamera.name = value; }
 }
	public System.String tag{  get { return UnityCamera.tag; }
  set{UnityCamera.tag = value; }
 }
	public UnityEngine.Transform transform{  get { return UnityCamera.transform; }
 }
	public System.Boolean useGUILayout{  get { return UnityCamera.useGUILayout; }
  set{UnityCamera.useGUILayout = value; }
 }
	public void Update(float dt, World world) {
frame = World.frame;

		this.Rule0(dt, world);
		this.Rule1(dt, world);
		this.Rule2(dt, world);
		this.Rule3(dt, world);
		this.Rule4(dt, world);
		this.Rule5(dt, world);
	}





	int s0=-1;
	public void Rule0(float dt, World world){ 
	switch (s0)
	{

	case -1:
	if(!(UnityEngine.Input.GetKey(KeyCode.W)))
	{

	s0 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	Rotation = ((((UnityEngine.Quaternion.Euler(0f,0f,0f)) * (UnityCamera.Rotation))) * (UnityEngine.Quaternion.Euler(MinusSpeed,0f,0f)));
	s0 = -1;
return;	
	default: return;}}
	

	int s1=-1;
	public void Rule1(float dt, World world){ 
	switch (s1)
	{

	case -1:
	if(!(UnityEngine.Input.GetKey(KeyCode.S)))
	{

	s1 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	Rotation = ((((UnityEngine.Quaternion.Euler(0f,0f,0f)) * (UnityCamera.Rotation))) * (UnityEngine.Quaternion.Euler(PlusSpeed,0f,0f)));
	s1 = -1;
return;	
	default: return;}}
	

	int s2=-1;
	public void Rule2(float dt, World world){ 
	switch (s2)
	{

	case -1:
	if(!(UnityEngine.Input.GetKey(KeyCode.D)))
	{

	s2 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	Rotation = ((((UnityEngine.Quaternion.Euler(0f,PlusSpeed,0f)) * (UnityCamera.Rotation))) * (UnityEngine.Quaternion.Euler(0f,0f,0f)));
	s2 = -1;
return;	
	default: return;}}
	

	int s3=-1;
	public void Rule3(float dt, World world){ 
	switch (s3)
	{

	case -1:
	if(!(UnityEngine.Input.GetKey(KeyCode.A)))
	{

	s3 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	Rotation = ((((UnityEngine.Quaternion.Euler(0f,MinusSpeed,0f)) * (UnityCamera.Rotation))) * (UnityEngine.Quaternion.Euler(0f,0f,0f)));
	s3 = -1;
return;	
	default: return;}}
	

	int s4=-1;
	public void Rule4(float dt, World world){ 
	switch (s4)
	{

	case -1:
	if(!(UnityEngine.Input.GetKey(KeyCode.Space)))
	{

	s4 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	Position = ((Position) + (((((Backward) * (dt))) * (MaxVelocity))));
	s4 = -1;
return;	
	default: return;}}
	

	int s5=-1;
	public void Rule5(float dt, World world){ 
	switch (s5)
	{

	case -1:
	if(!(UnityEngine.Input.GetKey(KeyCode.LeftShift)))
	{

	s5 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	Position = ((Position) + (((((Forward) * (dt))) * (MaxVelocity))));
	s5 = -1;
return;	
	default: return;}}
	





}
}    