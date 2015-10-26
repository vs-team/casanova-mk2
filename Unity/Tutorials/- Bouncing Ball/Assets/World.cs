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
		MainCamera = new MainCamera();
		Balls = (

Enumerable.Empty<Ball>()).ToList<Ball>();
		
}
		public List<Ball> __Balls;
	public List<Ball> Balls{  get { return  __Balls; }
  set{ __Balls = value;
 foreach(var e in value){if(e.JustEntered){ e.JustEntered = false;
}
} }
 }
	public MainCamera MainCamera;

System.DateTime init_time = System.DateTime.Now;
	public void Update(float dt, World world) {
var t = System.DateTime.Now;

		for(int x0 = 0; x0 < Balls.Count; x0++) { 
			Balls[x0].Update(dt, world);
		}
		MainCamera.Update(dt, world);
		this.Rule0(dt, world);
		this.Rule1(dt, world);
	}





	int s0=-1;
	public void Rule0(float dt, World world){ 
	switch (s0)
	{

	case -1:
	Balls = (

(Balls).Select(__ContextSymbol1 => new { ___b00 = __ContextSymbol1 })
.Where(__ContextSymbol2 => ((__ContextSymbol2.___b00.UnityBall.Destroyed) == (false)))
.Select(__ContextSymbol3 => __ContextSymbol3.___b00)
.ToList<Ball>()).ToList<Ball>();
	s0 = -1;
return;	
	default: return;}}
	

	int s1=-1;
	public void Rule1(float dt, World world){ 
	switch (s1)
	{

	case -1:
	if(!(UnityEngine.Input.GetKeyDown(KeyCode.Space)))
	{

	s1 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	Balls = new Cons<Ball>(new Ball(MainCamera.Position,MainCamera.Forward), (Balls)).ToList<Ball>();
	s1 = -1;
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
		VerticalSpeed = -2f;
		UnityCamera = UnityCamera.Find();
		MaxVelocity = 2f;
		HorizontalSpeed = 2f;
		
}
		public UnityEngine.Vector3 Backward{  get { return UnityCamera.Backward; }
 }
	public UnityEngine.Vector3 Down{  get { return UnityCamera.Down; }
 }
	public UnityEngine.Vector3 Forward{  get { return UnityCamera.Forward; }
 }
	public System.Single HorizontalSpeed;
	public UnityEngine.Vector3 Left{  get { return UnityCamera.Left; }
 }
	public System.Single MaxVelocity;
	public UnityEngine.Vector3 Position{  get { return UnityCamera.Position; }
  set{UnityCamera.Position = value; }
 }
	public UnityEngine.Vector3 Right{  get { return UnityCamera.Right; }
 }
	public UnityEngine.Quaternion Rotation{  get { return UnityCamera.Rotation; }
  set{UnityCamera.Rotation = value; }
 }
	public UnityCamera UnityCamera;
	public UnityEngine.Vector3 Up{  get { return UnityCamera.Up; }
 }
	public System.Single VerticalSpeed;
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
frame = World.frame;		this.Rule0(dt, world);

		this.Rule1(dt, world);
		this.Rule2(dt, world);
		this.Rule3(dt, world);
		this.Rule4(dt, world);
		this.Rule5(dt, world);
		this.Rule6(dt, world);
	}

	public void Rule0(float dt, World world) 
	{
	Rotation = ((UnityEngine.Quaternion.Euler(0f,(UnityEngine.Input.GetAxis("Mouse X")) * (HorizontalSpeed),0f)) * (UnityCamera.Rotation)) * (UnityEngine.Quaternion.Euler((UnityEngine.Input.GetAxis("Mouse Y")) * (VerticalSpeed),0f,0f));
	}
	




	int s1=-1;
	public void Rule1(float dt, World world){ 
	switch (s1)
	{

	case -1:
	if(!(UnityEngine.Input.GetKey(KeyCode.Q)))
	{

	s1 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	Position = ((Position) + (((((Down) * (dt))) * (MaxVelocity))));
	s1 = -1;
return;	
	default: return;}}
	

	int s2=-1;
	public void Rule2(float dt, World world){ 
	switch (s2)
	{

	case -1:
	if(!(UnityEngine.Input.GetKey(KeyCode.E)))
	{

	s2 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	Position = ((Position) + (((((Up) * (dt))) * (MaxVelocity))));
	s2 = -1;
return;	
	default: return;}}
	

	int s3=-1;
	public void Rule3(float dt, World world){ 
	switch (s3)
	{

	case -1:
	if(!(UnityEngine.Input.GetKey(KeyCode.S)))
	{

	s3 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	Position = ((Position) + (((((Backward) * (dt))) * (MaxVelocity))));
	s3 = -1;
return;	
	default: return;}}
	

	int s4=-1;
	public void Rule4(float dt, World world){ 
	switch (s4)
	{

	case -1:
	if(!(UnityEngine.Input.GetKey(KeyCode.W)))
	{

	s4 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	Position = ((Position) + (((((Forward) * (dt))) * (MaxVelocity))));
	s4 = -1;
return;	
	default: return;}}
	

	int s5=-1;
	public void Rule5(float dt, World world){ 
	switch (s5)
	{

	case -1:
	if(!(UnityEngine.Input.GetKey(KeyCode.D)))
	{

	s5 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	Position = ((Position) + (((((Right) * (dt))) * (MaxVelocity))));
	s5 = -1;
return;	
	default: return;}}
	

	int s6=-1;
	public void Rule6(float dt, World world){ 
	switch (s6)
	{

	case -1:
	if(!(UnityEngine.Input.GetKey(KeyCode.A)))
	{

	s6 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	Position = ((Position) + (((((Left) * (dt))) * (MaxVelocity))));
	s6 = -1;
return;	
	default: return;}}
	





}
public class Ball{
public int frame;
public bool JustEntered = true;
private UnityEngine.Vector3 position;
private UnityEngine.Vector3 direction;
	public int ID;
public Ball(UnityEngine.Vector3 position, UnityEngine.Vector3 direction)
	{JustEntered = false;
 frame = World.frame;
		UnityBall = UnityBall.Instantiate(direction,position);
		
}
		public System.Boolean Destroyed{  get { return UnityBall.Destroyed; }
  set{UnityBall.Destroyed = value; }
 }
	public UnityEngine.Vector3 Position{  get { return UnityBall.Position; }
  set{UnityBall.Position = value; }
 }
	public UnityBall UnityBall;
	public System.Boolean enabled{  get { return UnityBall.enabled; }
  set{UnityBall.enabled = value; }
 }
	public UnityEngine.GameObject gameObject{  get { return UnityBall.gameObject; }
 }
	public UnityEngine.HideFlags hideFlags{  get { return UnityBall.hideFlags; }
  set{UnityBall.hideFlags = value; }
 }
	public System.Boolean isActiveAndEnabled{  get { return UnityBall.isActiveAndEnabled; }
 }
	public System.String name{  get { return UnityBall.name; }
  set{UnityBall.name = value; }
 }
	public System.String tag{  get { return UnityBall.tag; }
  set{UnityBall.tag = value; }
 }
	public UnityEngine.Transform transform{  get { return UnityBall.transform; }
 }
	public System.Boolean useGUILayout{  get { return UnityBall.useGUILayout; }
  set{UnityBall.useGUILayout = value; }
 }
	public void Update(float dt, World world) {
frame = World.frame;

		this.Rule0(dt, world);

	}





	int s0=-1;
	public void Rule0(float dt, World world){ 
	switch (s0)
	{

	case -1:
	if(!(((-10f) > (Position.y))))
	{

	s0 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	Destroyed = true;
	s0 = -1;
return;	
	default: return;}}
	






}
} 