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

(Enumerable.Range(0,(1) + ((400) - (0))).ToList<System.Int32>()).Select(__ContextSymbol0 => new { ___s01 = __ContextSymbol0 })
.Select(__ContextSymbol1 => new SnowFlake(___camera00.Position,(new Just<System.Single>((___camera00.Position.y) + (UnityEngine.Random.Range(-5f,3f))))))
.ToList<SnowFlake>()).ToList<SnowFlake>();
		MaxSnowFlakes = 11000;
		MainCamera = ___camera00;
		
}
		public MainCamera MainCamera;
	public System.Int32 MaxSnowFlakes;
	public List<SnowFlake> SnowFlakes;
	public System.Int32 ___ns00;
	public System.Int32 counter30;
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

	goto case 2;	}
	case 2:
	
	counter30 = -1;
	if((((Enumerable.Range(0,((1) + (((100) - (0))))).ToList<System.Int32>()).Count) == (0)))
	{

	goto case 0;	}else
	{

	___ns00 = (Enumerable.Range(0,((1) + (((100) - (0))))).ToList<System.Int32>())[0];
	goto case 3;	}
	case 3:
	counter30 = ((counter30) + (1));
	if((((((Enumerable.Range(0,((1) + (((100) - (0))))).ToList<System.Int32>()).Count) == (counter30))) || (((counter30) > ((Enumerable.Range(0,((1) + (((100) - (0))))).ToList<System.Int32>()).Count)))))
	{

	goto case 0;	}else
	{

	___ns00 = (Enumerable.Range(0,((1) + (((100) - (0))))).ToList<System.Int32>())[counter30];
	goto case 4;	}
	case 4:
	SnowFlakes = new Cons<SnowFlake>(new SnowFlake(MainCamera.Position,(new Nothing<System.Single>())), (SnowFlakes)).ToList<SnowFlake>();
	s0 = 3;
return;
	case 0:
	count_down1 = UnityEngine.Random.Range(0.1f,1f);
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

(SnowFlakes).Select(__ContextSymbol2 => new { ___s10 = __ContextSymbol2 })
.Where(__ContextSymbol3 => !(__ContextSymbol3.___s10.Destroyed))
.Select(__ContextSymbol4 => __ContextSymbol4.___s10)
.ToList<SnowFlake>()).ToList<SnowFlake>();
	s1 = -1;
return;	
	default: return;}}
	





}
public class SnowFlake{
public int frame;
public bool JustEntered = true;
private UnityEngine.Vector3 camera_position;
private Option<System.Single> y_pos;
	public int ID;
public SnowFlake(UnityEngine.Vector3 camera_position, Option<System.Single> y_pos)
	{JustEntered = false;
 frame = World.frame;
		System.Single ___y_pos00;
		if(y_pos.IsSome)
			{
			___y_pos00 = y_pos.Value;
			}else
			{
			___y_pos00 = ((camera_position.y) + (2f)) + (UnityEngine.Random.Range(0f,3f));
			}
		UnitySnowFlake = UnitySnowFlake.Instantiate(new UnityEngine.Vector3((camera_position.x) + (UnityEngine.Random.Range(-20f,20f)),___y_pos00,(camera_position.z) + (UnityEngine.Random.Range(-20f,20f))));
		Timer = UnityEngine.Random.Range(-10f,10f);
		RotationZ = Rotation.z;
		RotationY = Rotation.y;
		RotationX = Rotation.x;
		RandomZ = 0;
		RandomY = 0;
		RandomX = 0;
		
}
		public System.Boolean Destroyed{  get { return UnitySnowFlake.Destroyed; }
  set{UnitySnowFlake.Destroyed = value; }
 }
	public UnityEngine.Vector3 Position{  get { return UnitySnowFlake.Position; }
  set{UnitySnowFlake.Position = value; }
 }
	public System.Int32 RandomX;
	public System.Int32 RandomY;
	public System.Int32 RandomZ;
	public UnityEngine.Quaternion Rotation{  get { return UnitySnowFlake.Rotation; }
  set{UnitySnowFlake.Rotation = value; }
 }
	public System.Single RotationX;
	public System.Single RotationY;
	public System.Single RotationZ;
	public System.Single Timer;
	public UnitySnowFlake UnitySnowFlake;
	public System.Boolean enabled{  get { return UnitySnowFlake.enabled; }
  set{UnitySnowFlake.enabled = value; }
 }
	public UnityEngine.GameObject gameObject{  get { return UnitySnowFlake.gameObject; }
 }
	public UnityEngine.HideFlags hideFlags{  get { return UnitySnowFlake.hideFlags; }
  set{UnitySnowFlake.hideFlags = value; }
 }
	public System.Boolean isActiveAndEnabled{  get { return UnitySnowFlake.isActiveAndEnabled; }
 }
	public System.String name{  get { return UnitySnowFlake.name; }
  set{UnitySnowFlake.name = value; }
 }
	public System.String tag{  get { return UnitySnowFlake.tag; }
  set{UnitySnowFlake.tag = value; }
 }
	public UnityEngine.Transform transform{  get { return UnitySnowFlake.transform; }
 }
	public System.Boolean useGUILayout{  get { return UnitySnowFlake.useGUILayout; }
  set{UnitySnowFlake.useGUILayout = value; }
 }
	public System.Single count_down2;
	public System.Single count_down3;
	public System.Single count_down4;
	public void Update(float dt, World world) {
frame = World.frame;		this.Rule3(dt, world);

		this.Rule0(dt, world);
		this.Rule1(dt, world);
		this.Rule2(dt, world);
		this.Rule4(dt, world);
		this.Rule5(dt, world);
		this.Rule6(dt, world);
		this.Rule7(dt, world);
		this.Rule8(dt, world);
		this.Rule9(dt, world);
	}

	public void Rule3(float dt, World world) 
	{
	Rotation = ((UnityEngine.Quaternion.Euler(RotationX,0f,0f)) * (UnityEngine.Quaternion.Euler(0f,RotationY,0f))) * (UnityEngine.Quaternion.Euler(0f,0f,RotationZ));
	}
	




	int s0=-1;
	public void Rule0(float dt, World world){ 
	switch (s0)
	{

	case -1:
	count_down2 = UnityEngine.Random.Range(1,3);
	goto case 2;
	case 2:
	if(((count_down2) > (0f)))
	{

	count_down2 = ((count_down2) - (dt));
	s0 = 2;
return;	}else
	{

	goto case 0;	}
	case 0:
	RandomZ = UnityEngine.Random.Range(-30,30);
	s0 = -1;
return;	
	default: return;}}
	

	int s1=-1;
	public void Rule1(float dt, World world){ 
	switch (s1)
	{

	case -1:
	count_down3 = UnityEngine.Random.Range(1,3);
	goto case 2;
	case 2:
	if(((count_down3) > (0f)))
	{

	count_down3 = ((count_down3) - (dt));
	s1 = 2;
return;	}else
	{

	goto case 0;	}
	case 0:
	RandomY = UnityEngine.Random.Range(-30,30);
	s1 = -1;
return;	
	default: return;}}
	

	int s2=-1;
	public void Rule2(float dt, World world){ 
	switch (s2)
	{

	case -1:
	count_down4 = UnityEngine.Random.Range(1,3);
	goto case 2;
	case 2:
	if(((count_down4) > (0f)))
	{

	count_down4 = ((count_down4) - (dt));
	s2 = 2;
return;	}else
	{

	goto case 0;	}
	case 0:
	RandomX = UnityEngine.Random.Range(-30,30);
	s2 = -1;
return;	
	default: return;}}
	

	int s4=-1;
	public void Rule4(float dt, World world){ 
	switch (s4)
	{

	case -1:
	RotationZ = ((RotationZ) + (((RandomZ) * (dt))));
	s4 = -1;
return;	
	default: return;}}
	

	int s5=-1;
	public void Rule5(float dt, World world){ 
	switch (s5)
	{

	case -1:
	RotationY = ((RotationY) + (((RandomY) * (dt))));
	s5 = -1;
return;	
	default: return;}}
	

	int s6=-1;
	public void Rule6(float dt, World world){ 
	switch (s6)
	{

	case -1:
	RotationX = ((RotationX) + (((RandomX) * (dt))));
	s6 = -1;
return;	
	default: return;}}
	

	int s7=-1;
	public void Rule7(float dt, World world){ 
	switch (s7)
	{

	case -1:
	if(!(((UnityEngine.Vector3.Distance(Position,world.MainCamera.Position)) > (40f))))
	{

	s7 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	Destroyed = true;
	s7 = -1;
return;	
	default: return;}}
	

	int s8=-1;
	public void Rule8(float dt, World world){ 
	switch (s8)
	{

	case -1:
	Position = ((Position) + (((new UnityEngine.Vector3(UnityEngine.Mathf.Cos(Timer),-0.65f,0)) * (dt))));
	s8 = -1;
return;	
	default: return;}}
	

	int s9=-1;
	public void Rule9(float dt, World world){ 
	switch (s9)
	{

	case -1:
	Timer = ((Timer) + (dt));
	s9 = -1;
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
		HorizontalSpeed = 2f;
		
}
		public UnityEngine.Vector3 Forward{  get { return UnityCamera.Forward; }
 }
	public System.Single HorizontalSpeed;
	public UnityEngine.Vector3 Position{  get { return UnityCamera.Position; }
 }
	public UnityEngine.Quaternion Rotation{  get { return UnityCamera.Rotation; }
  set{UnityCamera.Rotation = value; }
 }
	public UnityCamera UnityCamera;
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



	}

	public void Rule0(float dt, World world) 
	{
	Rotation = ((UnityEngine.Quaternion.Euler(0f,(UnityEngine.Input.GetAxis("Mouse X")) * (HorizontalSpeed),0f)) * (UnityCamera.Rotation)) * (UnityEngine.Quaternion.Euler((UnityEngine.Input.GetAxis("Mouse Y")) * (VerticalSpeed),0f,0f));
	}
	










}
}       