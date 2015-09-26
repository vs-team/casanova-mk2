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
		Game2 = new BoidsSimulation();
		Game1 = new PatrolLighSwitch();
		
}
		public PatrolLighSwitch __Game1;
	public PatrolLighSwitch Game1{  get { return  __Game1; }
  set{ __Game1 = value;
 if(!value.JustEntered) __Game1 = value; 
 else{ value.JustEntered = false;
}
 }
 }
	public BoidsSimulation Game2;

System.DateTime init_time = System.DateTime.Now;
	public void Update(float dt, World world) {
var t = System.DateTime.Now;

		Game1.Update(dt, world);
		Game2.Update(dt, world);


	}











}
public class BoidsSimulation{
public int frame;
public bool JustEntered = true;
	public int ID;
public BoidsSimulation()
	{JustEntered = false;
 frame = World.frame;
		BoidsLeader = new BoidsLeader();
		Boids = (

Enumerable.Empty<Boid>()).ToList<Boid>();
		
}
		public List<Boid> Boids;
	public BoidsLeader BoidsLeader;
	public System.Single count_down1;
	public void Update(float dt, World world) {
frame = World.frame;		this.Rule1(dt, world);

		for(int x0 = 0; x0 < Boids.Count; x0++) { 
			Boids[x0].Update(dt, world);
		}
		BoidsLeader.Update(dt, world);
		this.Rule0(dt, world);

	}

	public void Rule1(float dt, World world) 
	{
	Boids = (

(Boids).Select(__ContextSymbol2 => new { ___b10 = __ContextSymbol2 })
.Where(__ContextSymbol3 => !(__ContextSymbol3.___b10.Destroyed))
.Select(__ContextSymbol4 => __ContextSymbol4.___b10)
.ToList<Boid>()).ToList<Boid>();
	}
	




	int s0=-1;
	public void Rule0(float dt, World world){ 
	switch (s0)
	{

	case -1:
	count_down1 = 0.1f;
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
	Boids = new Cons<Boid>(new Boid(BoidsLeader.Position), (Boids)).ToList<Boid>();
	s0 = -1;
return;	
	default: return;}}
	






}
public class BoidsLeader{
public int frame;
public bool JustEntered = true;
	public int ID;
public BoidsLeader()
	{JustEntered = false;
 frame = World.frame;
		Velocity = Vector3.zero;
		UnityBall = UnityBall.Find("/BoidPatrol");
		IsPressed = false;
		
}
		public System.Boolean ClickedOver{  get { return UnityBall.ClickedOver; }
 }
	public UnityEngine.Color Color{  get { return UnityBall.Color; }
  set{UnityBall.Color = value; }
 }
	public System.Boolean Destroyed{  get { return UnityBall.Destroyed; }
  set{UnityBall.Destroyed = value; }
 }
	public System.Boolean IsPressed;
	public UnityEngine.Vector3 MousePosition{  get { return UnityBall.MousePosition; }
 }
	public UnityEngine.Vector3 Position{  get { return UnityBall.Position; }
  set{UnityBall.Position = value; }
 }
	public UnityBall UnityBall;
	public UnityEngine.Vector3 Velocity;
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
	public System.Single count_down3;
	public System.Single count_down2;
	public void Update(float dt, World world) {
frame = World.frame;		this.Rule2(dt, world);

		this.Rule0(dt, world);
		this.Rule1(dt, world);
		this.Rule3(dt, world);
	}

	public void Rule2(float dt, World world) 
	{
	Position = (Position) + ((Velocity) * (dt));
	}
	




	int s0=-1;
	public void Rule0(float dt, World world){ 
	switch (s0)
	{

	case -1:
	if(UnityEngine.Input.GetMouseButton(0))
	{

	goto case 3;	}else
	{

	goto case 4;	}
	case 3:
	if(((ClickedOver) || (IsPressed)))
	{

	goto case 6;	}else
	{

	goto case 7;	}
	case 6:
	IsPressed = true;
	Color = Color.red;
	s0 = -1;
return;
	case 7:
	IsPressed = false;
	Color = Color.green;
	s0 = -1;
return;
	case 4:
	IsPressed = false;
	Color = Color.green;
	s0 = -1;
return;	
	default: return;}}
	

	int s1=-1;
	public void Rule1(float dt, World world){ 
	switch (s1)
	{

	case -1:
	if(!(IsPressed))
	{

	s1 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	Position = MousePosition;
	s1 = -1;
return;	
	default: return;}}
	

	int s3=-1;
	public void Rule3(float dt, World world){ 
	switch (s3)
	{

	case -1:
	this.concurrentSelectMethod30(dt,world);
	if(__m_done30)
	{

	__m_current_state30 = -1;
	__m_done30 = false;
	s3 = -1;
return;	}else
	{

	s3 = -1;
return;	}	
	default: return;}}
	

	int __m_current_state30, s30=-1;
	bool __m_done30 = false;
	public void concurrentSelectMethod30(float dt, World world){
	{

	if(IsPressed)
	{

	if(!(((__m_current_state30) == (0))))
	{

	__m_current_state30 = -1;
	__m_done30 = false;
	s30 = 0;
	__m_current_state30 = 0;	}	}else
	{

	if(true)
	{

	if(!(((__m_current_state30) == (1))))
	{

	__m_current_state30 = -1;
	__m_done30 = false;
	s30 = 1;
	__m_current_state30 = 1;	}	}else
	{

	if(((!(IsPressed)) && (!(true))))
	{

	s30 = 3;	}	}	}	}
	switch (s30)
	{

	case 3:
	s30 = 0;
return;
	goto case 1;
	case 0:
	Velocity = Vector3.zero;
	s30 = 2;
return;
	case 1:
	Velocity = new UnityEngine.Vector3(0f,1f,0f);
	s30 = 8;
return;
	case 8:
	count_down3 = 3f;
	goto case 9;
	case 9:
	if(((count_down3) > (0f)))
	{

	count_down3 = ((count_down3) - (dt));
	s30 = 9;
return;	}else
	{

	goto case 7;	}
	case 7:
	Velocity = new UnityEngine.Vector3(0f,-1f,0f);
	s30 = 5;
return;
	case 5:
	count_down2 = 3f;
	goto case 6;
	case 6:
	if(((count_down2) > (0f)))
	{

	count_down2 = ((count_down2) - (dt));
	s30 = 6;
return;	}else
	{

	s30 = 2;
return;	}
	case 2:
	__m_done30 = true;
	goto case 3;	}}
	




}
public class Boid{
public int frame;
public bool JustEntered = true;
private UnityEngine.Vector3 leader;
	public int ID;
public Boid(UnityEngine.Vector3 leader)
	{JustEntered = false;
 frame = World.frame;
		Velocity = Vector3.up;
		UnityBall = UnityBall.Instantiate(leader);
		Steer = Vector3.zero;
		Separation = Vector3.zero;
		Ray = 1f;
		MaxSpeed = 2f;
		MaxForce = 2f;
		Align = Vector3.zero;
		Acceleration = Vector3.zero;
		
}
		public UnityEngine.Vector3 Acceleration;
	public UnityEngine.Vector3 Align;
	public System.Boolean ClickedOver{  get { return UnityBall.ClickedOver; }
 }
	public UnityEngine.Color Color{  get { return UnityBall.Color; }
  set{UnityBall.Color = value; }
 }
	public System.Boolean Destroyed{  get { return UnityBall.Destroyed; }
  set{UnityBall.Destroyed = value; }
 }
	public System.Single MaxForce;
	public System.Single MaxSpeed;
	public UnityEngine.Vector3 MousePosition{  get { return UnityBall.MousePosition; }
 }
	public UnityEngine.Vector3 Position{  get { return UnityBall.Position; }
  set{UnityBall.Position = value; }
 }
	public System.Single Ray;
	public UnityEngine.Vector3 Separation;
	public UnityEngine.Vector3 Steer;
	public UnityBall UnityBall;
	public UnityEngine.Vector3 Velocity;
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
	public System.Single count_down4;
	public System.Single ___neighbordist20;
	public List<UnityEngine.Vector3> ___new_align20;
	public UnityEngine.Vector3 ___sum20;
	public UnityEngine.Vector3 ___sum21;
	public System.Boolean ___slowdown30;
	public UnityEngine.Vector3 ___target30;
	public UnityEngine.Vector3 ___desired30;
	public System.Single ___d32;
	public UnityEngine.Vector3 ___desired31;
	public UnityEngine.Vector3 ___steer30;
	public UnityEngine.Vector3 ___v40;
	public UnityEngine.Vector3 ___acc40;
	public UnityEngine.Vector3 ___v41;
	public void Update(float dt, World world) {
frame = World.frame;		this.Rule1(dt, world);
		this.Rule6(dt, world);
		this.Rule0(dt, world);
		this.Rule2(dt, world);
		this.Rule3(dt, world);
		this.Rule4(dt, world);
		this.Rule5(dt, world);
	}

	public void Rule1(float dt, World world) 
	{
	System.Single ___desiredseparation10;
	___desiredseparation10 = 5f;
	List<UnityEngine.Vector3> ___separations_count10;
	___separations_count10 = (

(world.Game2.Boids).Select(__ContextSymbol5 => new { ___other10 = __ContextSymbol5 })
.Select(__ContextSymbol6 => new {___d10 = UnityEngine.Vector3.Distance(__ContextSymbol6.___other10.Position,Position), prev = __ContextSymbol6 })
.Select(__ContextSymbol7 => new {___diff10 = (Position) - (__ContextSymbol7.prev.___other10.Position), prev = __ContextSymbol7 })
.Select(__ContextSymbol8 => new {___diff11 = UnityEngine.Vector3.Normalize(__ContextSymbol8.___diff10), prev = __ContextSymbol8 })
.Where(__ContextSymbol9 => ((((__ContextSymbol9.prev.prev.___d10) > (0))) && (((___desiredseparation10) > (__ContextSymbol9.prev.prev.___d10)))))
.Select(__ContextSymbol10 => (__ContextSymbol10.___diff11) / (__ContextSymbol10.prev.prev.___d10))
.ToList<UnityEngine.Vector3>()).ToList<UnityEngine.Vector3>();
	UnityEngine.Vector3 ___separations10;
	___separations10 = (

(___separations_count10).Select(__ContextSymbol11 => new { ___s10 = __ContextSymbol11 })
.Select(__ContextSymbol12 => __ContextSymbol12.___s10)
.Aggregate(default(UnityEngine.Vector3), (acc, __x) => acc + __x));
	if(((___separations_count10.Count) > (0)))
		{
		Separation = (___separations10) / (((System.Single)___separations_count10.Count));
		}else
		{
		Separation = Vector3.zero;
		}
	}
	

	public void Rule6(float dt, World world) 
	{
	Position = (Position) + ((Velocity) * (dt));
	}
	



	int s0=-1;
	public void Rule0(float dt, World world){ 
	switch (s0)
	{

	case -1:
	count_down4 = 10f;
	goto case 2;
	case 2:
	if(((count_down4) > (0f)))
	{

	count_down4 = ((count_down4) - (dt));
	s0 = 2;
return;	}else
	{

	goto case 0;	}
	case 0:
	Destroyed = true;
	s0 = -1;
return;	
	default: return;}}
	

	int s2=-1;
	public void Rule2(float dt, World world){ 
	switch (s2)
	{

	case -1:
	___neighbordist20 = 3f;
	___new_align20 = (

(world.Game2.Boids).Select(__ContextSymbol14 => new { ___other21 = __ContextSymbol14 })
.Select(__ContextSymbol15 => new {___d21 = UnityEngine.Vector3.Distance(Position,__ContextSymbol15.___other21.Position), prev = __ContextSymbol15 })
.Where(__ContextSymbol16 => ((((__ContextSymbol16.___d21) > (0))) && (((___neighbordist20) > (__ContextSymbol16.___d21)))))
.Select(__ContextSymbol17 => __ContextSymbol17.prev.___other21.Velocity)
.ToList<UnityEngine.Vector3>()).ToList<UnityEngine.Vector3>();
	if(((___new_align20.Count) > (0)))
	{

	goto case 3;	}else
	{

	goto case 4;	}
	case 3:
	___sum20 = (

(___new_align20).Select(__ContextSymbol18 => new { ___a20 = __ContextSymbol18 })
.Select(__ContextSymbol19 => __ContextSymbol19.___a20)
.Aggregate(default(UnityEngine.Vector3), (acc, __x) => acc + __x));
	___sum21 = ((___sum20) / (((System.Single)___new_align20.Count)));
	if(((___sum21.magnitude) > (MaxForce)))
	{

	goto case 6;	}else
	{

	goto case 7;	}
	case 6:
	___sum21.Normalize();
	Align = ((___sum21) * (MaxForce));
	s2 = -1;
return;
	case 7:
	Align = ___sum21;
	s2 = -1;
return;
	case 4:
	Align = new UnityEngine.Vector3(0,0,0);
	s2 = -1;
return;	
	default: return;}}
	

	int s3=-1;
	public void Rule3(float dt, World world){ 
	switch (s3)
	{

	case -1:
	___slowdown30 = false;
	___target30 = world.Game2.BoidsLeader.Position;
	___desired30 = ((___target30) - (Position));
	___d32 = ___desired30.magnitude;
	if(((___d32) > (0)))
	{

	goto case 17;	}else
	{

	goto case 18;	}
	case 17:
	___desired30.Normalize();
	if(((4f) > (___d32)))
	{

	___desired31 = ((((___desired30) * (MaxSpeed))) * (((___d32) / (4f))));	}else
	{

	___desired31 = ((___desired30) * (MaxSpeed));	}
	___steer30 = ((___desired31) - (Velocity));
	if(((___steer30.magnitude) > (MaxForce)))
	{

	goto case 20;	}else
	{

	goto case 21;	}
	case 20:
	___steer30.Normalize();
	Steer = ((___steer30) * (MaxForce));
	s3 = -1;
return;
	case 21:
	Steer = ___steer30;
	s3 = -1;
return;
	case 18:
	Steer = new UnityEngine.Vector3(0,0,0);
	s3 = -1;
return;	
	default: return;}}
	

	int s4=-1;
	public void Rule4(float dt, World world){ 
	switch (s4)
	{

	case -1:
	___v40 = ((Velocity) + (Acceleration));
	___acc40 = Vector3.zero;
	if(((___v40.magnitude) > (MaxSpeed)))
	{

	___v40.Normalize();
	___v41 = ((___v40) * (MaxSpeed));	}else
	{

	___v41 = ___v40;	}
	Velocity = ___v41;
	s4 = -1;
return;	
	default: return;}}
	

	int s5=-1;
	public void Rule5(float dt, World world){ 
	switch (s5)
	{

	case -1:
	Acceleration = ((((((Separation) * (2f))) + (Align))) + (Steer));
	s5 = -1;
return;	
	default: return;}}
	





}
public class PatrolLighSwitch{
public int frame;
public bool JustEntered = true;
	public int ID;
public PatrolLighSwitch()
	{JustEntered = false;
 frame = World.frame;
		Patrol = new PatrolCube(this);
		LightSwitch = new LightSwitch(this);
		
}
		public LightSwitch LightSwitch;
	public PatrolCube Patrol;
	public void Update(float dt, World world) {
frame = World.frame;

		LightSwitch.Update(dt, world);
		Patrol.Update(dt, world);


	}











}
public class PatrolCube{
public int frame;
public bool JustEntered = true;
private PatrolLighSwitch patrolLighSwitch;
	public int ID;
public PatrolCube(PatrolLighSwitch patrolLighSwitch)
	{JustEntered = false;
 frame = World.frame;
		List<UnityEngine.Vector3> ___checkpoints00;
		___checkpoints00 = (

(new Cons<UnityEngine.Vector3>(new UnityEngine.Vector3(-4f,4f,0f),(new Cons<UnityEngine.Vector3>(new UnityEngine.Vector3(-8f,4f,0f),(new Cons<UnityEngine.Vector3>(new UnityEngine.Vector3(-8f,-4f,0f),(new Cons<UnityEngine.Vector3>(new UnityEngine.Vector3(-4f,-4f,0f),(new Empty<UnityEngine.Vector3>()).ToList<UnityEngine.Vector3>())).ToList<UnityEngine.Vector3>())).ToList<UnityEngine.Vector3>())).ToList<UnityEngine.Vector3>())).ToList<UnityEngine.Vector3>()).ToList<UnityEngine.Vector3>();
		Velocity = Vector3.zero;
		UnityCube = UnityCube.Find("/Cube3");
		PatrolLighSwitch = patrolLighSwitch;
		Checkpoints = ___checkpoints00;
		
}
		public List<UnityEngine.Vector3> Checkpoints;
	public UnityEngine.Color Color{  get { return UnityCube.Color; }
  set{UnityCube.Color = value; }
 }
	public PatrolLighSwitch PatrolLighSwitch;
	public UnityEngine.Vector3 Position{  get { return UnityCube.Position; }
  set{UnityCube.Position = value; }
 }
	public UnityCube UnityCube;
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
	public UnityEngine.Vector3 ___ls00;
	public System.Single count_down6;
	public System.Single count_down5;
	public UnityEngine.Vector3 ___c00;
	public System.Int32 counter150;
	public UnityEngine.Vector3 ___dir000;
	public System.Single count_down7;
	public UnityEngine.Color ____color00;
	public void Update(float dt, World world) {
frame = World.frame;		this.Rule1(dt, world);

		this.Rule0(dt, world);

	}

	public void Rule1(float dt, World world) 
	{
	Position = (Position) + (((Velocity) * (dt)) * (0.5f));
	}
	




	int s0=-1;
	public void Rule0(float dt, World world){ 
	switch (s0)
	{

	case -1:
	____color00 = Color;
	goto case 0;
	case 0:
	this.concurrentSelectMethod01(dt,world);
	if(__m_done01)
	{

	__m_current_state01 = -1;
	__m_done01 = false;
	s0 = -1;
return;	}else
	{

	s0 = 0;
return;	}	
	default: return;}}
	


	int __m_current_state01, s01=-1;
	bool __m_done01 = false;
	public void concurrentSelectMethod01(float dt, World world){
	{

	if(((!(((UnityEngine.Vector3.Distance(Position,PatrolLighSwitch.LightSwitch.Position)) > (1.005f)))) && (PatrolLighSwitch.LightSwitch.Stop)))
	{

	if(!(((__m_current_state01) == (0))))
	{

	__m_current_state01 = -1;
	__m_done01 = false;
	s01 = 0;
	__m_current_state01 = 0;	}	}else
	{

	if(true)
	{

	if(!(((__m_current_state01) == (1))))
	{

	__m_current_state01 = -1;
	__m_done01 = false;
	s01 = 1;
	__m_current_state01 = 1;	}	}else
	{

	if(((!(((!(((UnityEngine.Vector3.Distance(Position,PatrolLighSwitch.LightSwitch.Position)) > (1.005f)))) && (PatrolLighSwitch.LightSwitch.Stop)))) && (!(true))))
	{

	s01 = 3;	}	}	}	}
	switch (s01)
	{

	case 3:
	s01 = 0;
return;
	goto case 1;
	case 0:
	___ls00 = PatrolLighSwitch.LightSwitch.Position;
	Position = new UnityEngine.Vector3(Position.x,___ls00.y,0f);
	Velocity = Vector3.zero;
	Color = Color;
	PatrolLighSwitch.LightSwitch.PatrolWaiting = (new Just<System.Single>(5f));
	s01 = 4;
return;
	case 4:
	if(!(true))
	{

	s01 = 2;
return;	}else
	{

	goto case 5;	}
	case 5:
	Position = Position;
	Velocity = Velocity;
	Color = Color.gray;
	PatrolLighSwitch.LightSwitch.PatrolWaiting = PatrolLighSwitch.LightSwitch.PatrolWaiting;
	s01 = 9;
return;
	case 9:
	count_down6 = 1f;
	goto case 10;
	case 10:
	if(((count_down6) > (0f)))
	{

	count_down6 = ((count_down6) - (dt));
	s01 = 10;
return;	}else
	{

	goto case 8;	}
	case 8:
	Position = Position;
	Velocity = Velocity;
	Color = ____color00;
	PatrolLighSwitch.LightSwitch.PatrolWaiting = PatrolLighSwitch.LightSwitch.PatrolWaiting;
	s01 = 6;
return;
	case 6:
	count_down5 = 1f;
	goto case 7;
	case 7:
	if(((count_down5) > (0f)))
	{

	count_down5 = ((count_down5) - (dt));
	s01 = 7;
return;	}else
	{

	s01 = 4;
return;	}
	case 1:
	Position = Position;
	Velocity = Velocity;
	Color = ____color00;
	PatrolLighSwitch.LightSwitch.PatrolWaiting = (new Nothing<System.Single>());
	s01 = 14;
return;
	case 14:
	
	counter150 = -1;
	if((((Checkpoints).Count) == (0)))
	{

	s01 = 2;
return;	}else
	{

	___c00 = (Checkpoints)[0];
	goto case 15;	}
	case 15:
	counter150 = ((counter150) + (1));
	if((((((Checkpoints).Count) == (counter150))) || (((counter150) > ((Checkpoints).Count)))))
	{

	s01 = 2;
return;	}else
	{

	___c00 = (Checkpoints)[counter150];
	goto case 16;	}
	case 16:
	___dir000 = ((___c00) - (Position));
	Position = Position;
	Velocity = ___dir000;
	Color = Color;
	PatrolLighSwitch.LightSwitch.PatrolWaiting = PatrolLighSwitch.LightSwitch.PatrolWaiting;
	s01 = 20;
return;
	case 20:
	if(!(((0f) > (UnityEngine.Vector3.Dot(___dir000,(___c00) - (Position))))))
	{

	s01 = 20;
return;	}else
	{

	goto case 19;	}
	case 19:
	Position = ___c00;
	Velocity = Vector3.zero;
	Color = Color;
	PatrolLighSwitch.LightSwitch.PatrolWaiting = PatrolLighSwitch.LightSwitch.PatrolWaiting;
	s01 = 17;
return;
	case 17:
	count_down7 = 1f;
	goto case 18;
	case 18:
	if(((count_down7) > (0f)))
	{

	count_down7 = ((count_down7) - (dt));
	s01 = 18;
return;	}else
	{

	s01 = 15;
return;	}
	case 2:
	__m_done01 = true;
	goto case 3;	}}
	




}
public class LightSwitch{
public int frame;
public bool JustEntered = true;
private PatrolLighSwitch patrolLighSwitch;
	public int ID;
public LightSwitch(PatrolLighSwitch patrolLighSwitch)
	{JustEntered = false;
 frame = World.frame;
		UnityCube = UnityCube.Find("/Lighswitch");
		Stop = true;
		PatrolWaiting = (new Nothing<System.Single>());
		PatrolLighSwitch = patrolLighSwitch;
		Factor = 0f;
		
}
		public UnityEngine.Color Color{  get { return UnityCube.Color; }
  set{UnityCube.Color = value; }
 }
	public System.Single Factor;
	public PatrolLighSwitch PatrolLighSwitch;
	public Option<System.Single> PatrolWaiting;
	public UnityEngine.Vector3 Position{  get { return UnityCube.Position; }
  set{UnityCube.Position = value; }
 }
	public System.Boolean Stop;
	public UnityCube UnityCube;
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
	public System.Single count_down9;
	public System.Single count_down8;
	public System.Single ___max_time10;
	public void Update(float dt, World world) {
frame = World.frame;

if(PatrolWaiting.IsSome){  } 
		this.Rule0(dt, world);
		this.Rule1(dt, world);
	}





	int s0=-1;
	public void Rule0(float dt, World world){ 
	switch (s0)
	{

	case -1:
	Stop = true;
	s0 = 5;
return;
	case 5:
	if(!(PatrolWaiting.IsSome))
	{

	s0 = 5;
return;	}else
	{

	goto case 3;	}
	case 3:
	count_down9 = PatrolWaiting.Value;
	goto case 4;
	case 4:
	if(((count_down9) > (0f)))
	{

	count_down9 = ((count_down9) - (dt));
	s0 = 4;
return;	}else
	{

	goto case 2;	}
	case 2:
	Stop = false;
	s0 = 0;
return;
	case 0:
	count_down8 = 3f;
	goto case 1;
	case 1:
	if(((count_down8) > (0f)))
	{

	count_down8 = ((count_down8) - (dt));
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
	Factor = Factor;
	Color = Color.red;
	s1 = 6;
return;
	case 6:
	if(!(PatrolWaiting.IsSome))
	{

	s1 = 6;
return;	}else
	{

	goto case 5;	}
	case 5:
	___max_time10 = PatrolWaiting.Value;
	Factor = ___max_time10;
	Color = Color;
	s1 = 1;
return;
	case 1:
	if(!(Stop))
	{

	goto case 0;	}else
	{

	goto case 2;	}
	case 2:
	Factor = ((Factor) - (dt));
	Color = Color.Lerp(Color.green,Color.red,(Factor) / (___max_time10));
	s1 = 1;
return;
	case 0:
	if(!(Stop))
	{

	s1 = 0;
return;	}else
	{

	s1 = -1;
return;	}	
	default: return;}}
	





}
}         