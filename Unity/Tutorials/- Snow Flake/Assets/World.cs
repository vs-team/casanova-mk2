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
		MainCamera ___camera00;
		___camera00 = new MainCamera();
		SnowFlakes = (

(Enumerable.Range(0,(1) + ((10) - (0))).ToList<System.Int32>()).Select(__ContextSymbol0 => new { ___n01 = __ContextSymbol0 })
.Select(__ContextSymbol1 => new SnowFlake(___camera00.Position))
.ToList<SnowFlake>()).ToList<SnowFlake>();
		MaxSnowFlakes = 1000;
		MainCamera = ___camera00;
		
}
		public MainCamera MainCamera;
	public System.Int32 MaxSnowFlakes;
	public List<SnowFlake> SnowFlakes;
	public List<SnowFlake> ___new_snowflakes00;
	public System.Single count_down1;

System.DateTime init_time = System.DateTime.Now;
	public void Update(float dt, World world) {
var t = System.DateTime.Now;

		MainCamera.Update(dt, world);
		for(int x0 = 0; x0 < SnowFlakes.Count; x0++) { 
			SnowFlakes[x0].Update(dt, world);
		}
		this.Rule0(dt, world);
		this.Rule1(dt, world);
	}





	int s0=-1;
	public void Rule0(float dt, World world){ 
	switch (s0)
	{

	case -1:
	if(!(((MaxSnowFlakes) > (SnowFlakes.Count))))
	{

	s0 = -1;
return;	}else
	{

	goto case 3;	}
	case 3:
	___new_snowflakes00 = (

(Enumerable.Range(0,(1) + ((5) - (0))).ToList<System.Int32>()).Select(__ContextSymbol2 => new { ___n00 = __ContextSymbol2 })
.Select(__ContextSymbol3 => new SnowFlake(MainCamera.Position))
.ToList<SnowFlake>()).ToList<SnowFlake>();
	SnowFlakes = (___new_snowflakes00).Concat(SnowFlakes).ToList<SnowFlake>();
	s0 = 0;
return;
	case 0:
	count_down1 = UnityEngine.Random.Range(0.1f,2f);
	goto case 1;
	case 1:
	if(((count_down1) > (0f)))
	{

	count_down1 = ((count_down1) - (dt));
	s0 = 1;
return;	}else
	{

	s0 = -1;
return;	}	
	default: return;}}
	

	int s1=-1;
	public void Rule1(float dt, World world){ 
	switch (s1)
	{

	case -1:
	SnowFlakes = (

(SnowFlakes).Select(__ContextSymbol4 => new { ___s10 = __ContextSymbol4 })
.Where(__ContextSymbol5 => !(__ContextSymbol5.___s10.Destroyed))
.Select(__ContextSymbol6 => __ContextSymbol6.___s10)
.ToList<SnowFlake>()).ToList<SnowFlake>();
	s1 = -1;
return;	
	default: return;}}
	





}
public class SnowFlake{
public int frame;
public bool JustEntered = true;
private UnityEngine.Vector3 camera_position;
	public int ID;
public SnowFlake(UnityEngine.Vector3 camera_position)
	{JustEntered = false;
 frame = World.frame;
		UnitySnow = UnitySnow.Instantiate(new UnityEngine.Vector3((camera_position.x) + (UnityEngine.Random.Range(-10f,10f)),((camera_position.y) + (2f)) + (UnityEngine.Random.Range(0f,3f)),(camera_position.z) + (UnityEngine.Random.Range(-10f,10f))));
		Timer = UnityEngine.Random.Range(-10f,10f);
		
}
		public System.Boolean Destroyed{  get { return UnitySnow.Destroyed; }
  set{UnitySnow.Destroyed = value; }
 }
	public UnityEngine.Vector3 Position{  get { return UnitySnow.Position; }
  set{UnitySnow.Position = value; }
 }
	public System.Single Timer;
	public UnitySnow UnitySnow;
	public System.Boolean enabled{  get { return UnitySnow.enabled; }
  set{UnitySnow.enabled = value; }
 }
	public UnityEngine.GameObject gameObject{  get { return UnitySnow.gameObject; }
 }
	public UnityEngine.HideFlags hideFlags{  get { return UnitySnow.hideFlags; }
  set{UnitySnow.hideFlags = value; }
 }
	public System.Boolean isActiveAndEnabled{  get { return UnitySnow.isActiveAndEnabled; }
 }
	public System.String name{  get { return UnitySnow.name; }
  set{UnitySnow.name = value; }
 }
	public System.String tag{  get { return UnitySnow.tag; }
  set{UnitySnow.tag = value; }
 }
	public UnityEngine.Transform transform{  get { return UnitySnow.transform; }
 }
	public System.Boolean useGUILayout{  get { return UnitySnow.useGUILayout; }
  set{UnitySnow.useGUILayout = value; }
 }
	public void Update(float dt, World world) {
frame = World.frame;

		this.Rule0(dt, world);
		this.Rule1(dt, world);
		this.Rule2(dt, world);
	}





	int s0=-1;
	public void Rule0(float dt, World world){ 
	switch (s0)
	{

	case -1:
	if(!(((UnityEngine.Vector3.Distance(Position,world.MainCamera.Position)) > (20f))))
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
	

	int s1=-1;
	public void Rule1(float dt, World world){ 
	switch (s1)
	{

	case -1:
	Position = ((Position) + (((new UnityEngine.Vector3(UnityEngine.Mathf.Cos(Timer),-0.5f,0)) * (dt))));
	s1 = -1;
return;	
	default: return;}}
	

	int s2=-1;
	public void Rule2(float dt, World world){ 
	switch (s2)
	{

	case -1:
	Timer = ((Timer) + (dt));
	s2 = -1;
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
}     