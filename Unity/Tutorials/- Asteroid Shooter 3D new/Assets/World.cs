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
		Ship = new Ship();
		Explosions = (

Enumerable.Empty<Explosion>()).ToList<Explosion>();
		CollidingAsteroidsProjectile = (

Enumerable.Empty<Casanova.Prelude.Tuple<Beam, Asteroid>>()).ToList<Casanova.Prelude.Tuple<Beam, Asteroid>>();
		Beams = (

Enumerable.Empty<Beam>()).ToList<Beam>();
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
	public List<Beam> __Beams;
	public List<Beam> Beams{  get { return  __Beams; }
  set{ __Beams = value;
 foreach(var e in value){if(e.JustEntered){ e.JustEntered = false;
}
} }
 }
	public List<Casanova.Prelude.Tuple<Beam, Asteroid>> CollidingAsteroidsProjectile;
	public List<Explosion> Explosions;
	public Ship Ship;
	public System.Single count_down1;
	public System.Int32 ___a32;
	public System.Int32 counter13;
	public List<Explosion> ___explo50;

System.DateTime init_time = System.DateTime.Now;
	public void Update(float dt, World world) {
var t = System.DateTime.Now;

		for(int x0 = 0; x0 < Asteroids.Count; x0++) { 
			Asteroids[x0].Update(dt, world);
		}
		for(int x0 = 0; x0 < Beams.Count; x0++) { 
			Beams[x0].Update(dt, world);
		}
		Ship.Update(dt, world);
		this.Rule0(dt, world);
		this.Rule1(dt, world);
		this.Rule2(dt, world);
		this.Rule3(dt, world);
		this.Rule4(dt, world);
		this.Rule5(dt, world);
		this.Rule6(dt, world);
	}





	int s0=-1;
	public void Rule0(float dt, World world){ 
	switch (s0)
	{

	case -1:
	CollidingAsteroidsProjectile = (

(Beams).Select(__ContextSymbol4 => new { ___b00 = __ContextSymbol4 })
.SelectMany(__ContextSymbol5=> (Asteroids).Select(__ContextSymbol6 => new { ___a00 = __ContextSymbol6,
                                                      prev = __ContextSymbol5 })
.Where(__ContextSymbol7 => ((1f) > (UnityEngine.Vector3.Distance(__ContextSymbol7.___a00.Position,__ContextSymbol7.prev.___b00.Position))))
.Select(__ContextSymbol8 => new Casanova.Prelude.Tuple<Beam, Asteroid>(__ContextSymbol8.prev.___b00,__ContextSymbol8.___a00))
.ToList<Casanova.Prelude.Tuple<Beam, Asteroid>>())).ToList<Casanova.Prelude.Tuple<Beam, Asteroid>>();
	s0 = -1;
return;	
	default: return;}}
	

	int s1=-1;
	public void Rule1(float dt, World world){ 
	switch (s1)
	{

	case -1:
	Beams = (

(Beams).Select(__ContextSymbol9 => new { ___b11 = __ContextSymbol9 })
.Where(__ContextSymbol10 => !(__ContextSymbol10.___b11.Destroyed))
.Select(__ContextSymbol11 => __ContextSymbol11.___b11)
.ToList<Beam>()).ToList<Beam>();
	s1 = -1;
return;	
	default: return;}}
	

	int s2=-1;
	public void Rule2(float dt, World world){ 
	switch (s2)
	{

	case -1:
	Asteroids = (

(Asteroids).Select(__ContextSymbol12 => new { ___a21 = __ContextSymbol12 })
.Where(__ContextSymbol13 => !(__ContextSymbol13.___a21.Destroyed))
.Select(__ContextSymbol14 => __ContextSymbol14.___a21)
.ToList<Asteroid>()).ToList<Asteroid>();
	s2 = -1;
return;	
	default: return;}}
	

	int s3=-1;
	public void Rule3(float dt, World world){ 
	switch (s3)
	{

	case -1:
	count_down1 = 3f;
	goto case 5;
	case 5:
	if(((count_down1) > (0f)))
	{

	count_down1 = ((count_down1) - (dt));
	s3 = 5;
return;	}else
	{

	goto case 0;	}
	case 0:
	
	counter13 = -1;
	if((((Enumerable.Range(0,((1) + (((1) - (0))))).ToList<System.Int32>()).Count) == (0)))
	{

	s3 = -1;
return;	}else
	{

	___a32 = (Enumerable.Range(0,((1) + (((1) - (0))))).ToList<System.Int32>())[0];
	goto case 1;	}
	case 1:
	counter13 = ((counter13) + (1));
	if((((((Enumerable.Range(0,((1) + (((1) - (0))))).ToList<System.Int32>()).Count) == (counter13))) || (((counter13) > ((Enumerable.Range(0,((1) + (((1) - (0))))).ToList<System.Int32>()).Count)))))
	{

	s3 = -1;
return;	}else
	{

	___a32 = (Enumerable.Range(0,((1) + (((1) - (0))))).ToList<System.Int32>())[counter13];
	goto case 2;	}
	case 2:
	Asteroids = new Cons<Asteroid>(new Asteroid(new UnityEngine.Vector3(UnityEngine.Random.Range(-10f,10f),1f,UnityEngine.Random.Range(15f,20f))), (Asteroids)).ToList<Asteroid>();
	s3 = 1;
return;	
	default: return;}}
	

	int s4=-1;
	public void Rule4(float dt, World world){ 
	switch (s4)
	{

	case -1:
	if(!(UnityEngine.Input.GetKeyDown(KeyCode.Space)))
	{

	s4 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	Beams = new Cons<Beam>(new Beam(), (Beams)).ToList<Beam>();
	s4 = -1;
return;	
	default: return;}}
	

	int s5=-1;
	public void Rule5(float dt, World world){ 
	switch (s5)
	{

	case -1:
	___explo50 = (

(CollidingAsteroidsProjectile).Select(__ContextSymbol15 => new { ___x50 = __ContextSymbol15 })
.Select(__ContextSymbol16 => new Explosion(__ContextSymbol16.___x50.Item1.Position))
.ToList<Explosion>()).ToList<Explosion>();
	if(((___explo50.Count) > (0)))
	{

	goto case 2;	}else
	{

	goto case 3;	}
	case 2:
	Explosions = (___explo50).Concat(Explosions).ToList<Explosion>();
	s5 = -1;
return;
	case 3:
	Explosions = Explosions;
	s5 = -1;
return;	
	default: return;}}
	

	int s6=-1;
	public void Rule6(float dt, World world){ 
	switch (s6)
	{

	case -1:
	Explosions = (

(Explosions).Select(__ContextSymbol17 => new { ___e60 = __ContextSymbol17 })
.Where(__ContextSymbol18 => !(__ContextSymbol18.___e60.Destroyed))
.Select(__ContextSymbol19 => __ContextSymbol19.___e60)
.ToList<Explosion>()).ToList<Explosion>();
	s6 = -1;
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
		MaxVelocity = 7f;
		
}
		public UnityEngine.Vector3 Backward{  get { return UnityShip.Backward; }
 }
	public UnityEngine.Vector3 Down{  get { return UnityShip.Down; }
 }
	public UnityEngine.Vector3 Forward{  get { return UnityShip.Forward; }
 }
	public UnityEngine.Vector3 Left{  get { return UnityShip.Left; }
 }
	public System.Single MaxVelocity;
	public UnityEngine.Vector3 Position{  get { return UnityShip.Position; }
  set{UnityShip.Position = value; }
 }
	public UnityEngine.Vector3 Right{  get { return UnityShip.Right; }
 }
	public UnityEngine.Quaternion Rotation{  get { return UnityShip.Rotation; }
  set{UnityShip.Rotation = value; }
 }
	public UnityShip UnityShip;
	public UnityEngine.Vector3 Up{  get { return UnityShip.Up; }
 }
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
	if(!(UnityEngine.Input.GetKey(KeyCode.Q)))
	{

	s0 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	Position = ((Position) + (((((Down) * (dt))) * (MaxVelocity))));
	s0 = -1;
return;	
	default: return;}}
	

	int s1=-1;
	public void Rule1(float dt, World world){ 
	switch (s1)
	{

	case -1:
	if(!(UnityEngine.Input.GetKey(KeyCode.E)))
	{

	s1 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	Position = ((Position) + (((((Up) * (dt))) * (MaxVelocity))));
	s1 = -1;
return;	
	default: return;}}
	

	int s2=-1;
	public void Rule2(float dt, World world){ 
	switch (s2)
	{

	case -1:
	if(!(UnityEngine.Input.GetKey(KeyCode.S)))
	{

	s2 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	Position = ((Position) + (((((Backward) * (dt))) * (MaxVelocity))));
	s2 = -1;
return;	
	default: return;}}
	

	int s3=-1;
	public void Rule3(float dt, World world){ 
	switch (s3)
	{

	case -1:
	if(!(UnityEngine.Input.GetKey(KeyCode.W)))
	{

	s3 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	Position = ((Position) + (((((Forward) * (dt))) * (MaxVelocity))));
	s3 = -1;
return;	
	default: return;}}
	

	int s4=-1;
	public void Rule4(float dt, World world){ 
	switch (s4)
	{

	case -1:
	if(!(UnityEngine.Input.GetKey(KeyCode.D)))
	{

	s4 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	Position = ((Position) + (((((Right) * (dt))) * (MaxVelocity))));
	s4 = -1;
return;	
	default: return;}}
	

	int s5=-1;
	public void Rule5(float dt, World world){ 
	switch (s5)
	{

	case -1:
	if(!(UnityEngine.Input.GetKey(KeyCode.A)))
	{

	s5 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	Position = ((Position) + (((((Left) * (dt))) * (MaxVelocity))));
	s5 = -1;
return;	
	default: return;}}
	





}
public class Asteroid{
public int frame;
public bool JustEntered = true;
private UnityEngine.Vector3 position;
	public int ID;
public Asteroid(UnityEngine.Vector3 position)
	{JustEntered = false;
 frame = World.frame;
		UnityAsteroid = UnityAsteroid.Instantiate(position);
		RotationZ = Rotation.z;
		RotationY = Rotation.y;
		RotationX = Rotation.x;
		RandomZ = 0;
		RandomY = 0;
		RandomX = 0;
		
}
		public System.Boolean Destroyed{  get { return UnityAsteroid.Destroyed; }
  set{UnityAsteroid.Destroyed = value; }
 }
	public UnityEngine.Vector3 Position{  get { return UnityAsteroid.Position; }
  set{UnityAsteroid.Position = value; }
 }
	public System.Int32 RandomX;
	public System.Int32 RandomY;
	public System.Int32 RandomZ;
	public UnityEngine.Quaternion Rotation{  get { return UnityAsteroid.Rotation; }
  set{UnityAsteroid.Rotation = value; }
 }
	public System.Single RotationX;
	public System.Single RotationY;
	public System.Single RotationZ;
	public UnityAsteroid UnityAsteroid;
	public System.Boolean enabled{  get { return UnityAsteroid.enabled; }
  set{UnityAsteroid.enabled = value; }
 }
	public UnityEngine.GameObject gameObject{  get { return UnityAsteroid.gameObject; }
 }
	public UnityEngine.HideFlags hideFlags{  get { return UnityAsteroid.hideFlags; }
  set{UnityAsteroid.hideFlags = value; }
 }
	public System.Boolean isActiveAndEnabled{  get { return UnityAsteroid.isActiveAndEnabled; }
 }
	public System.String name{  get { return UnityAsteroid.name; }
  set{UnityAsteroid.name = value; }
 }
	public System.String tag{  get { return UnityAsteroid.tag; }
  set{UnityAsteroid.tag = value; }
 }
	public UnityEngine.Transform transform{  get { return UnityAsteroid.transform; }
 }
	public System.Boolean useGUILayout{  get { return UnityAsteroid.useGUILayout; }
  set{UnityAsteroid.useGUILayout = value; }
 }
	public List<Asteroid> ___b02;
	public System.Single count_down2;
	public System.Single count_down3;
	public System.Single count_down4;
	public void Update(float dt, World world) {
frame = World.frame;		this.Rule2(dt, world);

		this.Rule0(dt, world);
		this.Rule1(dt, world);
		this.Rule3(dt, world);
		this.Rule4(dt, world);
		this.Rule5(dt, world);
		this.Rule6(dt, world);
		this.Rule7(dt, world);
		this.Rule8(dt, world);
		this.Rule9(dt, world);
	}

	public void Rule2(float dt, World world) 
	{
	Rotation = ((UnityEngine.Quaternion.Euler(RotationX,0f,0f)) * (UnityEngine.Quaternion.Euler(0f,RotationY,0f))) * (UnityEngine.Quaternion.Euler(0f,0f,RotationZ));
	}
	




	int s0=-1;
	public void Rule0(float dt, World world){ 
	switch (s0)
	{

	case -1:
	___b02 = (

(world.CollidingAsteroidsProjectile).Select(__ContextSymbol20 => new { ___ba00 = __ContextSymbol20 })
.Where(__ContextSymbol21 => ((this) == (__ContextSymbol21.___ba00.Item2)))
.Select(__ContextSymbol22 => __ContextSymbol22.___ba00.Item2)
.ToList<Asteroid>()).ToList<Asteroid>();
	if(((___b02.Count) > (0)))
	{

	goto case 3;	}else
	{

	s0 = -1;
return;	}
	case 3:
	Destroyed = true;
	s0 = -1;
return;	
	default: return;}}
	

	int s1=-1;
	public void Rule1(float dt, World world){ 
	switch (s1)
	{

	case -1:
	if(!(((-20f) > (Position.z))))
	{

	s1 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	Destroyed = true;
	s1 = -1;
return;	
	default: return;}}
	

	int s3=-1;
	public void Rule3(float dt, World world){ 
	switch (s3)
	{

	case -1:
	RotationZ = ((RotationZ) + (((RandomZ) * (dt))));
	s3 = -1;
return;	
	default: return;}}
	

	int s4=-1;
	public void Rule4(float dt, World world){ 
	switch (s4)
	{

	case -1:
	RotationY = ((RotationY) + (((RandomY) * (dt))));
	s4 = -1;
return;	
	default: return;}}
	

	int s5=-1;
	public void Rule5(float dt, World world){ 
	switch (s5)
	{

	case -1:
	RotationX = ((RotationX) + (((RandomX) * (dt))));
	s5 = -1;
return;	
	default: return;}}
	

	int s6=-1;
	public void Rule6(float dt, World world){ 
	switch (s6)
	{

	case -1:
	RandomZ = UnityEngine.Random.Range(-30,30);
	s6 = 0;
return;
	case 0:
	count_down2 = UnityEngine.Random.Range(1,3);
	goto case 1;
	case 1:
	if(((count_down2) > (0f)))
	{

	count_down2 = ((count_down2) - (dt));
	s6 = 1;
return;	}else
	{

	s6 = -1;
return;	}	
	default: return;}}
	

	int s7=-1;
	public void Rule7(float dt, World world){ 
	switch (s7)
	{

	case -1:
	RandomY = UnityEngine.Random.Range(-30,30);
	s7 = 0;
return;
	case 0:
	count_down3 = UnityEngine.Random.Range(1,3);
	goto case 1;
	case 1:
	if(((count_down3) > (0f)))
	{

	count_down3 = ((count_down3) - (dt));
	s7 = 1;
return;	}else
	{

	s7 = -1;
return;	}	
	default: return;}}
	

	int s8=-1;
	public void Rule8(float dt, World world){ 
	switch (s8)
	{

	case -1:
	RandomX = UnityEngine.Random.Range(-30,30);
	s8 = 0;
return;
	case 0:
	count_down4 = UnityEngine.Random.Range(1,3);
	goto case 1;
	case 1:
	if(((count_down4) > (0f)))
	{

	count_down4 = ((count_down4) - (dt));
	s8 = 1;
return;	}else
	{

	s8 = -1;
return;	}	
	default: return;}}
	

	int s9=-1;
	public void Rule9(float dt, World world){ 
	switch (s9)
	{

	case -1:
	Position = ((Position) + (((new UnityEngine.Vector3(0f,0f,-6f)) * (dt))));
	s9 = -1;
return;	
	default: return;}}
	





}
public class Beam{
public int frame;
public bool JustEntered = true;
	public int ID;
public Beam()
	{JustEntered = false;
 frame = World.frame;
		UnityBeam = UnityBeam.Instantiate();
		
}
		public System.Boolean Destroyed{  get { return UnityBeam.Destroyed; }
  set{UnityBeam.Destroyed = value; }
 }
	public UnityEngine.Vector3 Position{  get { return UnityBeam.Position; }
  set{UnityBeam.Position = value; }
 }
	public UnityBeam UnityBeam;
	public System.Boolean enabled{  get { return UnityBeam.enabled; }
  set{UnityBeam.enabled = value; }
 }
	public UnityEngine.GameObject gameObject{  get { return UnityBeam.gameObject; }
 }
	public UnityEngine.HideFlags hideFlags{  get { return UnityBeam.hideFlags; }
  set{UnityBeam.hideFlags = value; }
 }
	public System.Boolean isActiveAndEnabled{  get { return UnityBeam.isActiveAndEnabled; }
 }
	public System.String name{  get { return UnityBeam.name; }
  set{UnityBeam.name = value; }
 }
	public System.String tag{  get { return UnityBeam.tag; }
  set{UnityBeam.tag = value; }
 }
	public UnityEngine.Transform transform{  get { return UnityBeam.transform; }
 }
	public System.Boolean useGUILayout{  get { return UnityBeam.useGUILayout; }
  set{UnityBeam.useGUILayout = value; }
 }
	public List<Beam> ___b03;
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
	___b03 = (

(world.CollidingAsteroidsProjectile).Select(__ContextSymbol23 => new { ___ba01 = __ContextSymbol23 })
.Where(__ContextSymbol24 => ((this) == (__ContextSymbol24.___ba01.Item1)))
.Select(__ContextSymbol25 => __ContextSymbol25.___ba01.Item1)
.ToList<Beam>()).ToList<Beam>();
	if(((___b03.Count) > (0)))
	{

	goto case 2;	}else
	{

	s0 = -1;
return;	}
	case 2:
	Destroyed = true;
	s0 = -1;
return;	
	default: return;}}
	

	int s1=-1;
	public void Rule1(float dt, World world){ 
	switch (s1)
	{

	case -1:
	if(!(((Position.z) > (20f))))
	{

	s1 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	Destroyed = true;
	s1 = -1;
return;	
	default: return;}}
	

	int s2=-1;
	public void Rule2(float dt, World world){ 
	switch (s2)
	{

	case -1:
	Position = ((Position) + (((new UnityEngine.Vector3(0f,0f,10f)) * (dt))));
	s2 = -1;
return;	
	default: return;}}
	





}
public class Explosion{
public int frame;
public bool JustEntered = true;
private UnityEngine.Vector3 position;
	public int ID;
public Explosion(UnityEngine.Vector3 position)
	{JustEntered = false;
 frame = World.frame;
		UnityExplosion = UnityExplosion.Instantiate(position);
		
}
		public System.Boolean Destroyed{  get { return UnityExplosion.Destroyed; }
  set{UnityExplosion.Destroyed = value; }
 }
	public UnityEngine.Vector3 Position{  get { return UnityExplosion.Position; }
  set{UnityExplosion.Position = value; }
 }
	public UnityEngine.ParticleSystem Ps{  get { return UnityExplosion.Ps; }
 }
	public UnityExplosion UnityExplosion;
	public System.Boolean destroyed{  get { return UnityExplosion.destroyed; }
  set{UnityExplosion.destroyed = value; }
 }
	public System.Boolean enabled{  get { return UnityExplosion.enabled; }
  set{UnityExplosion.enabled = value; }
 }
	public UnityEngine.GameObject gameObject{  get { return UnityExplosion.gameObject; }
 }
	public UnityEngine.HideFlags hideFlags{  get { return UnityExplosion.hideFlags; }
  set{UnityExplosion.hideFlags = value; }
 }
	public System.Boolean isActiveAndEnabled{  get { return UnityExplosion.isActiveAndEnabled; }
 }
	public System.String name{  get { return UnityExplosion.name; }
  set{UnityExplosion.name = value; }
 }
	public System.String tag{  get { return UnityExplosion.tag; }
  set{UnityExplosion.tag = value; }
 }
	public UnityEngine.Transform transform{  get { return UnityExplosion.transform; }
 }
	public System.Boolean useGUILayout{  get { return UnityExplosion.useGUILayout; }
  set{UnityExplosion.useGUILayout = value; }
 }
	public List<Explosion> ___e01;
	public void Update(float dt, World world) {
frame = World.frame;

		this.Rule0(dt, world);

	}





	int s0=-1;
	public void Rule0(float dt, World world){ 
	switch (s0)
	{

	case -1:
	___e01 = (

(world.Explosions).Select(__ContextSymbol26 => new { ___ex00 = __ContextSymbol26 })
.Where(__ContextSymbol27 => ((this) == (__ContextSymbol27.___ex00)))
.Select(__ContextSymbol28 => __ContextSymbol28.___ex00)
.ToList<Explosion>()).ToList<Explosion>();
	if(((___e01.Count) > (0)))
	{

	goto case 2;	}else
	{

	s0 = -1;
return;	}
	case 2:
	Destroyed = true;
	s0 = -1;
return;	
	default: return;}}
	






}
}               