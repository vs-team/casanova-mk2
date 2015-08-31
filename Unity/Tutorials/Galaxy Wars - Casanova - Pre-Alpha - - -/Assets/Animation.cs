#pragma warning disable 162,108,618
using Casanova.Prelude;
using System.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace Animation {public class Animation : MonoBehaviour{
public static int frame;
void Update () { Update(Time.deltaTime, this); 
 frame++; }
public bool JustEntered = true;
private UnityEngine.Vector3 target;


public void Start(){}public Animation(UnityEngine.Vector3 target)
	{
this.target = target;
		UnityCube = new UnityCube();
		Target = target;
		StopAnimation = false;
		ObjectsToAvoid = (

Enumerable.Empty<Container>()).ToList<Container>();
		Lasers = (

Enumerable.Empty<Laser>()).ToList<Laser>();
		InitialTarget = target;
		Factor = 0f;
		Explorer = UnityCube.Find();
		AttackingShips = (

Enumerable.Empty<Ship>()).ToList<Ship>();
		
}
		public List<Ship> AttackingShips;
	public UnityEngine.Color Color{  get { return UnityCube.Color; }
  set{UnityCube.Color = value; }
 }
	public UnityCube Explorer;
	public System.Single Factor;
	public UnityEngine.Vector3 InitialTarget;
	public List<Laser> Lasers;
	public List<Container> ObjectsToAvoid;
	public UnityEngine.Vector3 Position{  get { return UnityCube.Position; }
  set{UnityCube.Position = value; }
 }
	public System.Boolean StopAnimation;
	public UnityEngine.Vector3 Target;
	public UnityCube UnityCube;
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
	public List<Container> ___attackingships_containers40;
	public System.Single count_down1;

System.DateTime init_time = System.DateTime.Now;
	public void Update(float dt, Animation world) {
var t = System.DateTime.Now;		this.Rule6(dt, world);

		for(int x0 = 0; x0 < AttackingShips.Count; x0++) { 
			AttackingShips[x0].Update(dt, world);
		}
		for(int x0 = 0; x0 < Lasers.Count; x0++) { 
			Lasers[x0].Update(dt, world);
		}
		for(int x0 = 0; x0 < ObjectsToAvoid.Count; x0++) { 
			ObjectsToAvoid[x0].Update(dt, world);
		}
		this.Rule0(dt, world);
		this.Rule1(dt, world);
		this.Rule2(dt, world);
		this.Rule3(dt, world);
		this.Rule4(dt, world);
		this.Rule5(dt, world);
	}

	public void Rule6(float dt, Animation world) 
	{
	Lasers = (

(Lasers).Select(__ContextSymbol3 => new { ___l60 = __ContextSymbol3 })
.Where(__ContextSymbol4 => !(__ContextSymbol4.___l60.Destroyed))
.Select(__ContextSymbol5 => __ContextSymbol5.___l60)
.ToList<Laser>()).ToList<Laser>();
	}
	




	int s0=-1;
	public void Rule0(float dt, Animation world){ 
	switch (s0)
	{

	case -1:
	Explorer.Position = Target;
	s0 = -1;
return;	
	default: return;}}
	

	int s1=-1;
	public void Rule1(float dt, Animation world){ 
	switch (s1)
	{

	case -1:
	UnityEngine.Debug.Log("ANIMATION");
	ObjectsToAvoid = ObjectsToAvoid;
	s1 = -1;
return;	
	default: return;}}
	

	int s2=-1;
	public void Rule2(float dt, Animation world){ 
	switch (s2)
	{

	case -1:
	Target = new UnityEngine.Vector3((UnityEngine.Mathf.Cos(Factor)) + (InitialTarget.x),InitialTarget.y,(UnityEngine.Mathf.Sin(Factor)) + (InitialTarget.z));
	s2 = -1;
return;	
	default: return;}}
	

	int s3=-1;
	public void Rule3(float dt, Animation world){ 
	switch (s3)
	{

	case -1:
	Factor = ((Factor) + (dt));
	s3 = -1;
return;	
	default: return;}}
	

	int s4=-1;
	public void Rule4(float dt, Animation world){ 
	switch (s4)
	{

	case -1:
	___attackingships_containers40 = (

(AttackingShips).Select(__ContextSymbol6 => new { ___ship40 = __ContextSymbol6 })
.Select(__ContextSymbol7 => new Container(__ContextSymbol7.___ship40.Position,__ContextSymbol7.___ship40.Velocity))
.ToList<Container>()).ToList<Container>();
	ObjectsToAvoid = new Cons<Container>(new Container(InitialTarget,new UnityEngine.Vector3(0.01f,0f,0.01f)), (___attackingships_containers40)).ToList<Container>();
	s4 = -1;
return;	
	default: return;}}
	

	int s5=-1;
	public void Rule5(float dt, Animation world){ 
	switch (s5)
	{

	case -1:
	if(!(!(world.StopAnimation)))
	{

	s5 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	if(((AttackingShips.Count) > (0)))
	{

	goto case 1;	}else
	{

	s5 = -1;
return;	}
	case 1:
	Lasers = new Cons<Laser>(new Laser(InitialTarget,AttackingShips.Head().Position), (Lasers)).ToList<Laser>();
	s5 = 2;
return;
	case 2:
	count_down1 = 1f;
	goto case 3;
	case 3:
	if(((count_down1) > (0f)))
	{

	count_down1 = ((count_down1) - (dt));
	s5 = 3;
return;	}else
	{

	s5 = -1;
return;	}	
	default: return;}}
	





}
public class Container{
public int frame;
public bool JustEntered = true;
private UnityEngine.Vector3 p;
private UnityEngine.Vector3 v;
	public int ID;
public Container(UnityEngine.Vector3 p, UnityEngine.Vector3 v)
	{JustEntered = false;
 frame = Animation.frame;
		Velocity = v;
		Position = p;
		
}
		public UnityEngine.Vector3 Position;
	public UnityEngine.Vector3 Velocity;
	public void Update(float dt, Animation world) {
frame = Animation.frame;



	}











}
public class Laser{
public int frame;
public bool JustEntered = true;
private UnityEngine.Vector3 pos;
private UnityEngine.Vector3 target;
	public int ID;
public Laser(UnityEngine.Vector3 pos, UnityEngine.Vector3 target)
	{JustEntered = false;
 frame = Animation.frame;
		Velocity = Vector3.zero;
		UnityLaser = UnityLaser.Instantiate(pos,target);
		Target = target;
		MaxSpeed = 4f;
		
}
		public System.Boolean Destroyed{  get { return UnityLaser.Destroyed; }
  set{UnityLaser.Destroyed = value; }
 }
	public System.Boolean IsCollided{  set{UnityLaser.IsCollided = value; }
 }
	public System.Single MaxSpeed;
	public UnityEngine.Vector3 Position{  get { return UnityLaser.Position; }
  set{UnityLaser.Position = value; }
 }
	public UnityEngine.Vector3 Rotation{  set{UnityLaser.Rotation = value; }
 }
	public UnityEngine.Vector3 Target;
	public UnityLaser UnityLaser;
	public UnityEngine.Vector3 Velocity;
	public UnityEngine.Animation animation{  get { return UnityLaser.animation; }
 }
	public UnityEngine.AudioSource audio{  get { return UnityLaser.audio; }
 }
	public UnityEngine.Camera camera{  get { return UnityLaser.camera; }
 }
	public UnityEngine.Collider collider{  get { return UnityLaser.collider; }
 }
	public UnityEngine.Collider2D collider2D{  get { return UnityLaser.collider2D; }
 }
	public UnityEngine.ConstantForce constantForce{  get { return UnityLaser.constantForce; }
 }
	public System.Boolean enabled{  get { return UnityLaser.enabled; }
  set{UnityLaser.enabled = value; }
 }
	public UnityEngine.GameObject gameObject{  get { return UnityLaser.gameObject; }
 }
	public UnityEngine.GUIElement guiElement{  get { return UnityLaser.guiElement; }
 }
	public UnityEngine.GUIText guiText{  get { return UnityLaser.guiText; }
 }
	public UnityEngine.GUITexture guiTexture{  get { return UnityLaser.guiTexture; }
 }
	public UnityEngine.HideFlags hideFlags{  get { return UnityLaser.hideFlags; }
  set{UnityLaser.hideFlags = value; }
 }
	public UnityEngine.HingeJoint hingeJoint{  get { return UnityLaser.hingeJoint; }
 }
	public UnityEngine.Light light{  get { return UnityLaser.light; }
 }
	public System.String name{  get { return UnityLaser.name; }
  set{UnityLaser.name = value; }
 }
	public UnityEngine.ParticleEmitter particleEmitter{  get { return UnityLaser.particleEmitter; }
 }
	public UnityEngine.ParticleSystem particleSystem{  get { return UnityLaser.particleSystem; }
 }
	public UnityEngine.Renderer renderer{  get { return UnityLaser.renderer; }
 }
	public UnityEngine.Rigidbody rigidbody{  get { return UnityLaser.rigidbody; }
 }
	public UnityEngine.Rigidbody2D rigidbody2D{  get { return UnityLaser.rigidbody2D; }
 }
	public System.String tag{  get { return UnityLaser.tag; }
  set{UnityLaser.tag = value; }
 }
	public UnityEngine.Transform transform{  get { return UnityLaser.transform; }
 }
	public System.Boolean useGUILayout{  get { return UnityLaser.useGUILayout; }
  set{UnityLaser.useGUILayout = value; }
 }
	public UnityEngine.Vector3 ___temp30;
	public void Update(float dt, Animation world) {
frame = Animation.frame;

		this.Rule0(dt, world);
		this.Rule1(dt, world);
		this.Rule2(dt, world);
		this.Rule3(dt, world);
	}





	int s0=-1;
	public void Rule0(float dt, Animation world){ 
	switch (s0)
	{

	case -1:
	if(!(((((world.AttackingShips.Count) == (0))) || (world.StopAnimation))))
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
	public void Rule1(float dt, Animation world){ 
	switch (s1)
	{

	case -1:
	if(!(((0.5f) > (UnityEngine.Vector3.Distance(Position,Target)))))
	{

	s1 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	IsCollided = true;
	s1 = -1;
return;	
	default: return;}}
	

	int s2=-1;
	public void Rule2(float dt, Animation world){ 
	switch (s2)
	{

	case -1:
	Position = ((Position) + (((Velocity) * (dt))));
	s2 = -1;
return;	
	default: return;}}
	

	int s3=-1;
	public void Rule3(float dt, Animation world){ 
	switch (s3)
	{

	case -1:
	___temp30 = ((Target) - (Position));
	___temp30.Normalize();
	Velocity = ((___temp30) * (MaxSpeed));
	s3 = -1;
return;	
	default: return;}}
	





}
public class Ship{
public int frame;
public bool JustEntered = true;
private UnityShip ship;
	public int ID;
public Ship(UnityShip ship)
	{JustEntered = false;
 frame = Animation.frame;
		Velocity = Vector3.up;
		UnityShip = ship;
		Steer = Vector3.zero;
		Separation = Vector3.zero;
		MaxSpeed = 2f;
		MaxForce = 2f;
		Align = Vector3.zero;
		Acceleration = Vector3.zero;
		
}
		public UnityEngine.Vector3 Acceleration;
	public UnityEngine.Vector3 Align;
	public System.Boolean Destroyed{  get { return UnityShip.Destroyed; }
  set{UnityShip.Destroyed = value; }
 }
	public System.Boolean ExplodeAndDestroyed{  get { return UnityShip.ExplodeAndDestroyed; }
  set{UnityShip.ExplodeAndDestroyed = value; }
 }
	public System.Single MaxForce;
	public System.Single MaxSpeed;
	public UnityEngine.Color MiniMapColor{  get { return UnityShip.MiniMapColor; }
  set{UnityShip.MiniMapColor = value; }
 }
	public UnityEngine.Vector3 Position{  get { return UnityShip.Position; }
  set{UnityShip.Position = value; }
 }
	public UnityEngine.Vector3 Rotation{  set{UnityShip.Rotation = value; }
 }
	public System.Single Scale{  get { return UnityShip.Scale; }
  set{UnityShip.Scale = value; }
 }
	public UnityEngine.Vector3 Separation;
	public System.String ShipOwnerText{  get { return UnityShip.ShipOwnerText; }
  set{UnityShip.ShipOwnerText = value; }
 }
	public UnityEngine.Vector3 ShipOwnerTextRotation{  get { return UnityShip.ShipOwnerTextRotation; }
  set{UnityShip.ShipOwnerTextRotation = value; }
 }
	public UnityEngine.Vector3 Steer;
	public UnityShip UnityShip;
	public UnityEngine.Vector3 Velocity;
	public UnityEngine.Animation animation{  get { return UnityShip.animation; }
 }
	public UnityEngine.AudioSource audio{  get { return UnityShip.audio; }
 }
	public UnityEngine.Camera camera{  get { return UnityShip.camera; }
 }
	public UnityEngine.Collider collider{  get { return UnityShip.collider; }
 }
	public UnityEngine.Collider2D collider2D{  get { return UnityShip.collider2D; }
 }
	public UnityEngine.ConstantForce constantForce{  get { return UnityShip.constantForce; }
 }
	public System.Boolean enabled{  get { return UnityShip.enabled; }
  set{UnityShip.enabled = value; }
 }
	public UnityEngine.GameObject gameObject{  get { return UnityShip.gameObject; }
 }
	public UnityEngine.GUIElement guiElement{  get { return UnityShip.guiElement; }
 }
	public UnityEngine.GUIText guiText{  get { return UnityShip.guiText; }
 }
	public UnityEngine.GUITexture guiTexture{  get { return UnityShip.guiTexture; }
 }
	public UnityEngine.HideFlags hideFlags{  get { return UnityShip.hideFlags; }
  set{UnityShip.hideFlags = value; }
 }
	public UnityEngine.HingeJoint hingeJoint{  get { return UnityShip.hingeJoint; }
 }
	public UnityEngine.Light light{  get { return UnityShip.light; }
 }
	public System.String name{  get { return UnityShip.name; }
  set{UnityShip.name = value; }
 }
	public UnityEngine.ParticleEmitter particleEmitter{  get { return UnityShip.particleEmitter; }
 }
	public UnityEngine.ParticleSystem particleSystem{  get { return UnityShip.particleSystem; }
 }
	public UnityEngine.Renderer renderer{  get { return UnityShip.renderer; }
 }
	public UnityEngine.Rigidbody rigidbody{  get { return UnityShip.rigidbody; }
 }
	public UnityEngine.Rigidbody2D rigidbody2D{  get { return UnityShip.rigidbody2D; }
 }
	public System.String tag{  get { return UnityShip.tag; }
  set{UnityShip.tag = value; }
 }
	public UnityEngine.Transform transform{  get { return UnityShip.transform; }
 }
	public System.Boolean useGUILayout{  get { return UnityShip.useGUILayout; }
  set{UnityShip.useGUILayout = value; }
 }
	public System.Single count_down2;
	public System.Single ___MaxSpeed20;
	public UnityEngine.Vector3 ___target20;
	public UnityEngine.Vector3 ___target_dir20;
	public System.Single ___target_distance20;
	public UnityEngine.Vector3 ___repulsions20;
	public UnityEngine.Vector3 ___diff20;
	public UnityEngine.Vector3 ___normalized_repulsions20;
	public UnityEngine.Vector3 ____velocity20;
	public void Update(float dt, Animation world) {
frame = Animation.frame;

		this.Rule0(dt, world);
		this.Rule1(dt, world);
		this.Rule2(dt, world);
		this.Rule3(dt, world);
	}





	int s0=-1;
	public void Rule0(float dt, Animation world){ 
	switch (s0)
	{

	case -1:
	count_down2 = 1f;
	goto case 3;
	case 3:
	if(((count_down2) > (0f)))
	{

	count_down2 = ((count_down2) - (dt));
	s0 = 3;
return;	}else
	{

	goto case 1;	}
	case 1:
	if(!(!(world.StopAnimation)))
	{

	s0 = 1;
return;	}else
	{

	goto case 0;	}
	case 0:
	world.Lasers = new Cons<Laser>(new Laser(Position,world.InitialTarget), (world.Lasers)).ToList<Laser>();
	s0 = -1;
return;	
	default: return;}}
	

	int s1=-1;
	public void Rule1(float dt, Animation world){ 
	switch (s1)
	{

	case -1:
	Rotation = ((world.InitialTarget) - (Position));
	s1 = -1;
return;	
	default: return;}}
	

	int s2=-1;
	public void Rule2(float dt, Animation world){ 
	switch (s2)
	{

	case -1:
	___MaxSpeed20 = 2f;
	___target20 = world.Target;
	___target_dir20 = UnityEngine.Vector3.Normalize((___target20) - (Position));
	___target_distance20 = UnityEngine.Vector3.Distance(Position,___target20);
	___repulsions20 = (

(world.ObjectsToAvoid).Select(__ContextSymbol8 => new { ___elem20 = __ContextSymbol8 })
.Select(__ContextSymbol9 => new {___distance20 = UnityEngine.Vector3.Distance(__ContextSymbol9.___elem20.Position,Position), prev = __ContextSymbol9 })
.Select(__ContextSymbol10 => new {___strength20 = (5f) - (UnityEngine.Mathf.Lerp(0f,5f,(__ContextSymbol10.___distance20) / (3f))), prev = __ContextSymbol10 })
.Select(__ContextSymbol11 => new {___y20 = UnityEngine.Vector3.Normalize((Position) - (__ContextSymbol11.prev.prev.___elem20.Position)), prev = __ContextSymbol11 })
.Select(__ContextSymbol12 => new {___away20 = Utils.IfThenElse<UnityEngine.Vector3>((()=> !(((UnityEngine.Vector3.Distance(Position,__ContextSymbol12.prev.prev.prev.___elem20.Position)) > (1.5f)))), (()=>	new UnityEngine.Vector3((UnityEngine.Random.value) - (0.5f),0f,(UnityEngine.Random.value) - (0.5f))
),(()=>	new UnityEngine.Vector3(__ContextSymbol12.___y20.x,0f,__ContextSymbol12.___y20.z)
)), prev = __ContextSymbol12 })
.Select(__ContextSymbol13 => (__ContextSymbol13.___away20) * (__ContextSymbol13.prev.prev.___strength20))
.Aggregate(default(UnityEngine.Vector3), (acc, __x) => acc + __x));
	___diff20 = ((___target20) - (Position));
	if(((0.1f) > (___repulsions20.magnitude)))
	{

	___normalized_repulsions20 = ___repulsions20;	}else
	{

	___normalized_repulsions20 = UnityEngine.Vector3.Normalize(___repulsions20);	}
	if(((0.1f) > (___target_distance20)))
	{

	____velocity20 = ((___normalized_repulsions20) * (___MaxSpeed20));	}else
	{

	____velocity20 = ((((___diff20.normalized) * (___MaxSpeed20))) + (((___normalized_repulsions20) * (___MaxSpeed20))));	}
	Velocity = UnityEngine.Vector3.Lerp(Velocity,____velocity20,dt);
	s2 = -1;
return;	
	default: return;}}
	

	int s3=-1;
	public void Rule3(float dt, Animation world){ 
	switch (s3)
	{

	case -1:
	Position = ((Position) + (((Velocity) * (dt))));
	s3 = -1;
return;	
	default: return;}}
	





}
}                                              