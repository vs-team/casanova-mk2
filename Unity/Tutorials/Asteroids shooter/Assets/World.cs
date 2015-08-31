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
		Ship = new Ship();
		Projectiles = (

Enumerable.Empty<Projectile>()).ToList<Projectile>();
		Asteroids = (

Enumerable.Empty<Asteroid>()).ToList<Asteroid>();
		
}
		public List<Asteroid> __Asteroids;
	public List<Asteroid> Asteroids{  get { return  __Asteroids; }
  set{ __Asteroids = value;
 foreach(var e in value){if(e.JustEntered){ e.JustEntered = false;
}
} }
 }
	public List<Projectile> __Projectiles;
	public List<Projectile> Projectiles{  get { return  __Projectiles; }
  set{ __Projectiles = value;
 foreach(var e in value){if(e.JustEntered){ e.JustEntered = false;
}
} }
 }
	public Ship Ship;
	public System.Single count_down1;

System.DateTime init_time = System.DateTime.Now;
	public void Update(float dt, World world) {
var t = System.DateTime.Now;		this.Rule0(dt, world);

		for(int x0 = 0; x0 < Asteroids.Count; x0++) { 
			Asteroids[x0].Update(dt, world);
		}
		for(int x0 = 0; x0 < Projectiles.Count; x0++) { 
			Projectiles[x0].Update(dt, world);
		}
		Ship.Update(dt, world);
		this.Rule1(dt, world);
		this.Rule2(dt, world);
	}

	public void Rule0(float dt, World world) 
	{
		Asteroids = (

(Asteroids).Select(__ContextSymbol2 => new { ___a00 = __ContextSymbol2 })
.Where(__ContextSymbol3 => !(__ContextSymbol3.___a00.Destroyed))
.Select(__ContextSymbol4 => __ContextSymbol4.___a00)
.ToList<Asteroid>()).ToList<Asteroid>();
	Projectiles = (

(Projectiles).Select(__ContextSymbol5 => new { ___p00 = __ContextSymbol5 })
.Where(__ContextSymbol6 => !(__ContextSymbol6.___p00.Destroyed))
.Select(__ContextSymbol7 => __ContextSymbol7.___p00)
.ToList<Projectile>()).ToList<Projectile>();
	}
	




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
	Projectiles = new Cons<Projectile>(new Projectile(Ship.Position), (Projectiles)).ToList<Projectile>();
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
	Asteroids = new Cons<Asteroid>(new Asteroid(), (Asteroids)).ToList<Asteroid>();
	s2 = -1;
return;	
	default: return;}}
	





}
public class Asteroid{
public int frame;
public bool JustEntered = true;
	public int ID;
public Asteroid()
	{JustEntered = false;
 frame = World.frame;
		Velocity = new UnityEngine.Vector3(0f,(UnityEngine.Random.value) * (-1f),0f);
		UnityAsteroid = UnityAsteroid.Instantiate(new UnityEngine.Vector3((-7f) + ((UnityEngine.Random.value) * (17f)),4f,0f));
		
}
		public System.Boolean Destroyed{  get { return UnityAsteroid.Destroyed; }
  set{UnityAsteroid.Destroyed = value; }
 }
	public UnityEngine.Vector3 Position{  get { return UnityAsteroid.Position; }
  set{UnityAsteroid.Position = value; }
 }
	public UnityAsteroid UnityAsteroid;
	public UnityEngine.Vector3 Velocity;
	public UnityEngine.Animation animation{  get { return UnityAsteroid.animation; }
 }
	public UnityEngine.AudioSource audio{  get { return UnityAsteroid.audio; }
 }
	public UnityEngine.Camera camera{  get { return UnityAsteroid.camera; }
 }
	public UnityEngine.Collider collider{  get { return UnityAsteroid.collider; }
 }
	public UnityEngine.Collider2D collider2D{  get { return UnityAsteroid.collider2D; }
 }
	public UnityEngine.ConstantForce constantForce{  get { return UnityAsteroid.constantForce; }
 }
	public System.Boolean enabled{  get { return UnityAsteroid.enabled; }
  set{UnityAsteroid.enabled = value; }
 }
	public UnityEngine.GameObject gameObject{  get { return UnityAsteroid.gameObject; }
 }
	public UnityEngine.GUIElement guiElement{  get { return UnityAsteroid.guiElement; }
 }
	public UnityEngine.GUIText guiText{  get { return UnityAsteroid.guiText; }
 }
	public UnityEngine.GUITexture guiTexture{  get { return UnityAsteroid.guiTexture; }
 }
	public UnityEngine.HideFlags hideFlags{  get { return UnityAsteroid.hideFlags; }
  set{UnityAsteroid.hideFlags = value; }
 }
	public UnityEngine.HingeJoint hingeJoint{  get { return UnityAsteroid.hingeJoint; }
 }
	public UnityEngine.Light light{  get { return UnityAsteroid.light; }
 }
	public System.String name{  get { return UnityAsteroid.name; }
  set{UnityAsteroid.name = value; }
 }
	public UnityEngine.ParticleEmitter particleEmitter{  get { return UnityAsteroid.particleEmitter; }
 }
	public UnityEngine.ParticleSystem particleSystem{  get { return UnityAsteroid.particleSystem; }
 }
	public UnityEngine.Renderer renderer{  get { return UnityAsteroid.renderer; }
 }
	public UnityEngine.Rigidbody rigidbody{  get { return UnityAsteroid.rigidbody; }
 }
	public UnityEngine.Rigidbody2D rigidbody2D{  get { return UnityAsteroid.rigidbody2D; }
 }
	public System.String tag{  get { return UnityAsteroid.tag; }
  set{UnityAsteroid.tag = value; }
 }
	public UnityEngine.Transform transform{  get { return UnityAsteroid.transform; }
 }
	public System.Boolean useGUILayout{  get { return UnityAsteroid.useGUILayout; }
  set{UnityAsteroid.useGUILayout = value; }
 }
	public List<Projectile> ___colliding_projectiles10;
	public System.Single count_down2;
	public void Update(float dt, World world) {
frame = World.frame;		this.Rule2(dt, world);

		this.Rule0(dt, world);
		this.Rule1(dt, world);
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
	if(!(((-4f) > (Position.y))))
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
	___colliding_projectiles10 = (

(world.Projectiles).Select(__ContextSymbol8 => new { ___p11 = __ContextSymbol8 })
.Where(__ContextSymbol9 => ((1f) > (UnityEngine.Vector3.Distance(Position,__ContextSymbol9.___p11.Position))))
.Select(__ContextSymbol10 => __ContextSymbol10.___p11)
.ToList<Projectile>()).ToList<Projectile>();
	if(((___colliding_projectiles10.Count) > (0)))
	{

	goto case 3;	}else
	{

	s1 = -1;
return;	}
	case 3:
	count_down2 = dt;
	goto case 6;
	case 6:
	if(((count_down2) > (0f)))
	{

	count_down2 = ((count_down2) - (dt));
	s1 = 6;
return;	}else
	{

	goto case 4;	}
	case 4:
	Destroyed = true;
	s1 = -1;
return;	
	default: return;}}
	





}
public class Projectile{
public int frame;
public bool JustEntered = true;
private UnityEngine.Vector3 p;
	public int ID;
public Projectile(UnityEngine.Vector3 p)
	{JustEntered = false;
 frame = World.frame;
		Velocity = new UnityEngine.Vector3(0f,1f,0f);
		UnityProjectile = UnityProjectile.Instantiate(p);
		
}
		public System.Boolean Destroyed{  get { return UnityProjectile.Destroyed; }
  set{UnityProjectile.Destroyed = value; }
 }
	public UnityEngine.Vector3 Position{  get { return UnityProjectile.Position; }
  set{UnityProjectile.Position = value; }
 }
	public UnityProjectile UnityProjectile;
	public UnityEngine.Vector3 Velocity;
	public UnityEngine.Animation animation{  get { return UnityProjectile.animation; }
 }
	public UnityEngine.AudioSource audio{  get { return UnityProjectile.audio; }
 }
	public UnityEngine.Camera camera{  get { return UnityProjectile.camera; }
 }
	public UnityEngine.Collider collider{  get { return UnityProjectile.collider; }
 }
	public UnityEngine.Collider2D collider2D{  get { return UnityProjectile.collider2D; }
 }
	public UnityEngine.ConstantForce constantForce{  get { return UnityProjectile.constantForce; }
 }
	public System.Boolean enabled{  get { return UnityProjectile.enabled; }
  set{UnityProjectile.enabled = value; }
 }
	public UnityEngine.GameObject gameObject{  get { return UnityProjectile.gameObject; }
 }
	public UnityEngine.GUIElement guiElement{  get { return UnityProjectile.guiElement; }
 }
	public UnityEngine.GUIText guiText{  get { return UnityProjectile.guiText; }
 }
	public UnityEngine.GUITexture guiTexture{  get { return UnityProjectile.guiTexture; }
 }
	public UnityEngine.HideFlags hideFlags{  get { return UnityProjectile.hideFlags; }
  set{UnityProjectile.hideFlags = value; }
 }
	public UnityEngine.HingeJoint hingeJoint{  get { return UnityProjectile.hingeJoint; }
 }
	public UnityEngine.Light light{  get { return UnityProjectile.light; }
 }
	public System.String name{  get { return UnityProjectile.name; }
  set{UnityProjectile.name = value; }
 }
	public UnityEngine.ParticleEmitter particleEmitter{  get { return UnityProjectile.particleEmitter; }
 }
	public UnityEngine.ParticleSystem particleSystem{  get { return UnityProjectile.particleSystem; }
 }
	public UnityEngine.Renderer renderer{  get { return UnityProjectile.renderer; }
 }
	public UnityEngine.Rigidbody rigidbody{  get { return UnityProjectile.rigidbody; }
 }
	public UnityEngine.Rigidbody2D rigidbody2D{  get { return UnityProjectile.rigidbody2D; }
 }
	public System.String tag{  get { return UnityProjectile.tag; }
  set{UnityProjectile.tag = value; }
 }
	public UnityEngine.Transform transform{  get { return UnityProjectile.transform; }
 }
	public System.Boolean useGUILayout{  get { return UnityProjectile.useGUILayout; }
  set{UnityProjectile.useGUILayout = value; }
 }
	public List<Asteroid> ___colliding_asteroids10;
	public System.Single count_down3;
	public void Update(float dt, World world) {
frame = World.frame;		this.Rule2(dt, world);

		this.Rule0(dt, world);
		this.Rule1(dt, world);
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
	if(!(((Position.y) > (4f))))
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
	___colliding_asteroids10 = (

(world.Asteroids).Select(__ContextSymbol11 => new { ___a11 = __ContextSymbol11 })
.Where(__ContextSymbol12 => ((1f) > (UnityEngine.Vector3.Distance(Position,__ContextSymbol12.___a11.Position))))
.Select(__ContextSymbol13 => __ContextSymbol13.___a11)
.ToList<Asteroid>()).ToList<Asteroid>();
	if(((___colliding_asteroids10.Count) > (0)))
	{

	goto case 3;	}else
	{

	s1 = -1;
return;	}
	case 3:
	count_down3 = dt;
	goto case 6;
	case 6:
	if(((count_down3) > (0f)))
	{

	count_down3 = ((count_down3) - (dt));
	s1 = 6;
return;	}else
	{

	goto case 4;	}
	case 4:
	Destroyed = true;
	s1 = -1;
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
		UnityShip = UnityShip.Find();
		
}
		public UnityEngine.Vector3 Position{  get { return UnityShip.Position; }
  set{UnityShip.Position = value; }
 }
	public UnityShip UnityShip;
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
	public void Update(float dt, World world) {
frame = World.frame;

		this.Rule0(dt, world);
		this.Rule1(dt, world);
	}





	int s0=-1;
	public void Rule0(float dt, World world){ 
	switch (s0)
	{

	case -1:
	if(!(UnityEngine.Input.GetKey(KeyCode.D)))
	{

	s0 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	Position = ((Position) + (((new UnityEngine.Vector3(3f,0f,0f)) * (dt))));
	s0 = -1;
return;	
	default: return;}}
	

	int s1=-1;
	public void Rule1(float dt, World world){ 
	switch (s1)
	{

	case -1:
	if(!(UnityEngine.Input.GetKey(KeyCode.A)))
	{

	s1 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	Position = ((Position) + (((new UnityEngine.Vector3(-3f,0f,0f)) * (dt))));
	s1 = -1;
return;	
	default: return;}}
	





}
    