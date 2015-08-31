#pragma warning disable 162,108,618
using Casanova.Prelude;
using System.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;
public class World : MonoBehaviour{
public static int frame;
void Update () { Update(Time.deltaTime, this); 
 frame++; }
public bool JustEntered = true;


public void Start()
	{
		MainCamera = new MainCamera();
		
}
		public MainCamera __MainCamera;
	public MainCamera MainCamera{  get { return  __MainCamera; }
  set{ __MainCamera = value;
 if(!value.JustEntered) __MainCamera = value; 
 else{ value.JustEntered = false;
}
 }
 }
	public void Update(float dt, World world) {
var t = System.DateTime.Now;

		MainCamera.Update(dt, world);


	}










}
public class MainCamera{
public int frame;
public bool JustEntered = true;
	public int ID;
public MainCamera()
	{JustEntered = false;
 frame = World.frame;
		UnityCamera = UnityCamera.Find();
		SprintFactor = 1f;
		Speed = Vector3.zero;
		Jumping = false;
		
}
		public UnityEngine.Vector3 Forward{  get { return UnityCamera.Forward; }
  set{UnityCamera.Forward = value; }
 }
	public System.Boolean Grounded{  get { return UnityCamera.Grounded; }
 }
	public System.Single Height{  get { return UnityCamera.Height; }
 }
	public System.Boolean Jumping;
	public UnityEngine.Vector3 Move{  set{UnityCamera.Move = value; }
 }
	public UnityEngine.Vector3 Right{  get { return UnityCamera.Right; }
  set{UnityCamera.Right = value; }
 }
	public UnityEngine.Quaternion Rotation{  get { return UnityCamera.Rotation; }
  set{UnityCamera.Rotation = value; }
 }
	public UnityEngine.Vector3 SimpleMove{  set{UnityCamera.SimpleMove = value; }
 }
	public UnityEngine.Vector3 Speed;
	public System.Single SprintFactor;
	public UnityCamera UnityCamera;
	public UnityEngine.Vector3 Up{  get { return UnityCamera.Up; }
  set{UnityCamera.Up = value; }
 }
	public UnityEngine.Vector3 Velocity{  get { return UnityCamera.Velocity; }
 }
	public UnityEngine.Animation animation{  get { return UnityCamera.animation; }
 }
	public UnityEngine.AudioSource audio{  get { return UnityCamera.audio; }
 }
	public UnityEngine.Camera camera{  get { return UnityCamera.camera; }
 }
	public UnityEngine.Collider collider{  get { return UnityCamera.collider; }
 }
	public UnityEngine.Collider2D collider2D{  get { return UnityCamera.collider2D; }
 }
	public UnityEngine.ConstantForce constantForce{  get { return UnityCamera.constantForce; }
 }
	public System.Boolean enabled{  get { return UnityCamera.enabled; }
  set{UnityCamera.enabled = value; }
 }
	public UnityEngine.GameObject gameObject{  get { return UnityCamera.gameObject; }
 }
	public UnityEngine.GUIElement guiElement{  get { return UnityCamera.guiElement; }
 }
	public UnityEngine.GUIText guiText{  get { return UnityCamera.guiText; }
 }
	public UnityEngine.GUITexture guiTexture{  get { return UnityCamera.guiTexture; }
 }
	public UnityEngine.HideFlags hideFlags{  get { return UnityCamera.hideFlags; }
  set{UnityCamera.hideFlags = value; }
 }
	public UnityEngine.HingeJoint hingeJoint{  get { return UnityCamera.hingeJoint; }
 }
	public UnityEngine.Light light{  get { return UnityCamera.light; }
 }
	public System.String name{  get { return UnityCamera.name; }
  set{UnityCamera.name = value; }
 }
	public UnityEngine.ParticleEmitter particleEmitter{  get { return UnityCamera.particleEmitter; }
 }
	public UnityEngine.ParticleSystem particleSystem{  get { return UnityCamera.particleSystem; }
 }
	public UnityEngine.Renderer renderer{  get { return UnityCamera.renderer; }
 }
	public UnityEngine.Rigidbody rigidbody{  get { return UnityCamera.rigidbody; }
 }
	public UnityEngine.Rigidbody2D rigidbody2D{  get { return UnityCamera.rigidbody2D; }
 }
	public System.String tag{  get { return UnityCamera.tag; }
  set{UnityCamera.tag = value; }
 }
	public UnityEngine.Transform transform{  get { return UnityCamera.transform; }
 }
	public System.Boolean useGUILayout{  get { return UnityCamera.useGUILayout; }
  set{UnityCamera.useGUILayout = value; }
 }
	public System.Single ___s20;
	public void Update(float dt, World world) {
frame = World.frame;		this.Rule0(dt, world);
		this.Rule5(dt, world);
		this.Rule1(dt, world);
		this.Rule2(dt, world);
		this.Rule3(dt, world);
		this.Rule4(dt, world);
	}

	public void Rule0(float dt, World world) 
	{
	Move = Speed;
	}
	

	public void Rule5(float dt, World world) 
	{
	Rotation = ((UnityEngine.Quaternion.Euler(0f,(UnityEngine.Input.GetAxis("Mouse X")) * (4f),0f)) * (UnityCamera.Rotation)) * (UnityEngine.Quaternion.Euler((UnityEngine.Input.GetAxis("Mouse Y")) * (-4f),0f,0f));
	}
	

	int s200=-1;
	public void parallelMethod200(float dt, World world){ 
	switch (s200)
	{

	case -1:
	if(!(((UnityEngine.Input.GetKeyDown(KeyCode.Space)) && (!(Jumping)))))
	{

	s200 = -1;
return;	}else
	{

	goto case 2;	}
	case 2:
	Speed = ((Speed) + (((new UnityEngine.Vector3(0f,(Up.y) * (12.5f),0f)) * (dt))));
	Jumping = true;
	s200 = 1;
return;
	case 1:
	if(!(Grounded))
	{

	s200 = 1;
return;	}else
	{

	goto case 0;	}
	case 0:
	Speed = Speed;
	Jumping = false;
	s200 = -1;
return;	
	default: return;}}
	

	int s201=-1;
	public void parallelMethod201(float dt, World world){ 
	switch (s201)
	{

	case -1:
	if(!(!(Grounded)))
	{

	s201 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	Speed = ((Speed) - (((new UnityEngine.Vector3(0f,0.5f,0f)) * (dt))));
	Jumping = Jumping;
	s201 = -1;
return;	
	default: return;}}
	

	int s202=-1;
	public void parallelMethod202(float dt, World world){ 
	switch (s202)
	{

	case -1:
	if(!(((UnityEngine.Input.GetKey(KeyCode.W)) && (Grounded))))
	{

	s202 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	Speed = ((Speed) + (((((((new UnityEngine.Vector3(Forward.x,0f,Forward.z)) * (SprintFactor))) * (___s20))) * (dt))));
	Jumping = Jumping;
	s202 = -1;
return;	
	default: return;}}
	

	int s203=-1;
	public void parallelMethod203(float dt, World world){ 
	switch (s203)
	{

	case -1:
	if(!(((UnityEngine.Input.GetKey(KeyCode.S)) && (Grounded))))
	{

	s203 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	Speed = ((Speed) + (((((((new UnityEngine.Vector3(Forward.x,0f,Forward.z)) * (-1f))) * (___s20))) * (dt))));
	Jumping = Jumping;
	s203 = -1;
return;	
	default: return;}}
	

	int s204=-1;
	public void parallelMethod204(float dt, World world){ 
	switch (s204)
	{

	case -1:
	if(!(((UnityEngine.Input.GetKey(KeyCode.D)) && (Grounded))))
	{

	s204 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	Speed = ((Speed) + (((((new UnityEngine.Vector3(Right.x,0f,Right.z)) * (___s20))) * (dt))));
	Jumping = Jumping;
	s204 = -1;
return;	
	default: return;}}
	

	int s205=-1;
	public void parallelMethod205(float dt, World world){ 
	switch (s205)
	{

	case -1:
	if(!(((UnityEngine.Input.GetKey(KeyCode.A)) && (Grounded))))
	{

	s205 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	Speed = ((Speed) + (((((((new UnityEngine.Vector3(Right.x,0f,Right.z)) * (-1f))) * (___s20))) * (dt))));
	Jumping = Jumping;
	s205 = -1;
return;	
	default: return;}}
	

	int s1=-1;
	public void Rule1(float dt, World world){ 
	switch (s1)
	{

	case -1:
	if(!(((((Speed.magnitude) > (0f))) && (Grounded))))
	{

	s1 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	Speed = ((((Speed) * (0.5f))) * (dt));
	s1 = -1;
return;	
	default: return;}}
	

	int s2=-1;
	public void Rule2(float dt, World world){ 
	switch (s2)
	{

	case -1:
	___s20 = 5f;
	goto case 0;
	case 0:
	this.parallelMethod200(dt,world);
	this.parallelMethod201(dt,world);
	this.parallelMethod202(dt,world);
	this.parallelMethod203(dt,world);
	this.parallelMethod204(dt,world);
	this.parallelMethod205(dt,world);
	s2 = 0;
return;	
	default: return;}}
	

	int s3=-1;
	public void Rule3(float dt, World world){ 
	switch (s3)
	{

	case -1:
	if(!(UnityEngine.Input.GetKeyUp(KeyCode.LeftShift)))
	{

	s3 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	SprintFactor = 1f;
	s3 = -1;
return;	
	default: return;}}
	

	int s4=-1;
	public void Rule4(float dt, World world){ 
	switch (s4)
	{

	case -1:
	if(!(UnityEngine.Input.GetKey(KeyCode.LeftShift)))
	{

	s4 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	SprintFactor = 2f;
	s4 = -1;
return;	
	default: return;}}
	





}
    