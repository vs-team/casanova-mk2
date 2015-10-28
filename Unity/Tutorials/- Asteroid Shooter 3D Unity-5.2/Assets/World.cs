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
		Ship ___ship01;
		___ship01 = new Ship();
		UnityParticleSystem = UnityParticleSystem.Find();
		ShipExplosions = (

Enumerable.Empty<ShipExplosion>()).ToList<ShipExplosion>();
		Ship = ___ship01;
		ScoreCounter = UnityScore.Find();
		LifeCounter = UnityLifeBar.Find();
		Explosions = (

Enumerable.Empty<Explosion>()).ToList<Explosion>();
		CollidingAsteroidsShip = (

Enumerable.Empty<Casanova.Prelude.Tuple<Ship, Asteroid>>()).ToList<Casanova.Prelude.Tuple<Ship, Asteroid>>();
		CollidingAsteroidsProjectile = (

Enumerable.Empty<Casanova.Prelude.Tuple<Beam, Asteroid>>()).ToList<Casanova.Prelude.Tuple<Beam, Asteroid>>();
		Camera = new MainCamera(___ship01.Position);
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
	public MainCamera Camera;
	public List<Casanova.Prelude.Tuple<Beam, Asteroid>> CollidingAsteroidsProjectile;
	public List<Casanova.Prelude.Tuple<Ship, Asteroid>> CollidingAsteroidsShip;
	public List<Explosion> Explosions;
	public UnityLifeBar LifeCounter;
	public UnityScore ScoreCounter;
	public Ship Ship;
	public List<ShipExplosion> ShipExplosions;
	public UnityParticleSystem UnityParticleSystem;
	public System.Single count_down1;
	public List<Explosion> ___explo70;
	public List<ShipExplosion> ___explo91;
	public System.Single count_down2;

System.DateTime init_time = System.DateTime.Now;
	public void Update(float dt, World world) {
var t = System.DateTime.Now;

		for(int x0 = 0; x0 < Asteroids.Count; x0++) { 
			Asteroids[x0].Update(dt, world);
		}
		for(int x0 = 0; x0 < Beams.Count; x0++) { 
			Beams[x0].Update(dt, world);
		}
		Camera.Update(dt, world);
		Ship.Update(dt, world);
		this.Rule0(dt, world);
		this.Rule1(dt, world);
		this.Rule2(dt, world);
		this.Rule3(dt, world);
		this.Rule4(dt, world);
		this.Rule5(dt, world);
		this.Rule6(dt, world);
		this.Rule7(dt, world);
		this.Rule8(dt, world);
		this.Rule9(dt, world);
		this.Rule10(dt, world);
		this.Rule11(dt, world);
		this.Rule12(dt, world);
		this.Rule13(dt, world);
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
	

	int s1=-1;
	public void Rule1(float dt, World world){ 
	switch (s1)
	{

	case -1:
	CollidingAsteroidsShip = (

(Asteroids).Select(__ContextSymbol6 => new { ___a10 = __ContextSymbol6 })
.Select(__ContextSymbol7 => new {___ship10 = Ship, prev = __ContextSymbol7 })
.Where(__ContextSymbol8 => ((((1f) > (UnityEngine.Vector3.Distance(__ContextSymbol8.prev.___a10.Position,__ContextSymbol8.___ship10.Position)))) && (!(__ContextSymbol8.prev.___a10.Destroyed))))
.Select(__ContextSymbol9 => new Casanova.Prelude.Tuple<Ship, Asteroid>(__ContextSymbol9.___ship10,__ContextSymbol9.prev.___a10))
.ToList<Casanova.Prelude.Tuple<Ship, Asteroid>>()).ToList<Casanova.Prelude.Tuple<Ship, Asteroid>>();
	s1 = -1;
return;	
	default: return;}}
	

	int s2=-1;
	public void Rule2(float dt, World world){ 
	switch (s2)
	{

	case -1:
	CollidingAsteroidsProjectile = (

(Beams).Select(__ContextSymbol10 => new { ___b20 = __ContextSymbol10 })
.SelectMany(__ContextSymbol11=> (Asteroids).Select(__ContextSymbol12 => new { ___a21 = __ContextSymbol12,
                                                      prev = __ContextSymbol11 })
.Where(__ContextSymbol13 => ((((1f) > (UnityEngine.Vector3.Distance(__ContextSymbol13.___a21.Position,__ContextSymbol13.prev.___b20.Position)))) && (!(__ContextSymbol13.___a21.Destroyed))))
.Select(__ContextSymbol14 => new Casanova.Prelude.Tuple<Beam, Asteroid>(__ContextSymbol14.prev.___b20,__ContextSymbol14.___a21))
.ToList<Casanova.Prelude.Tuple<Beam, Asteroid>>())).ToList<Casanova.Prelude.Tuple<Beam, Asteroid>>();
	s2 = -1;
return;	
	default: return;}}
	

	int s3=-1;
	public void Rule3(float dt, World world){ 
	switch (s3)
	{

	case -1:
	Asteroids = (

(Asteroids).Select(__ContextSymbol15 => new { ___a32 = __ContextSymbol15 })
.Where(__ContextSymbol16 => !(__ContextSymbol16.___a32.Destroyed))
.Select(__ContextSymbol17 => __ContextSymbol17.___a32)
.ToList<Asteroid>()).ToList<Asteroid>();
	s3 = -1;
return;	
	default: return;}}
	

	int s4=-1;
	public void Rule4(float dt, World world){ 
	switch (s4)
	{

	case -1:
	count_down1 = 0.0005f;
	goto case 2;
	case 2:
	if(((count_down1) > (0f)))
	{

	count_down1 = ((count_down1) - (dt));
	s4 = 2;
return;	}else
	{

	goto case 0;	}
	case 0:
	Asteroids = new Cons<Asteroid>(new Asteroid(new UnityEngine.Vector3((UnityEngine.Random.Range(-70f,70f)) + (Ship.Position.x),(UnityEngine.Random.Range(-70f,70f)) + (Ship.Position.y),(UnityEngine.Random.Range(70f,105f)) + (Ship.Position.z))), (Asteroids)).ToList<Asteroid>();
	s4 = -1;
return;	
	default: return;}}
	

	int s5=-1;
	public void Rule5(float dt, World world){ 
	switch (s5)
	{

	case -1:
	Beams = (

(Beams).Select(__ContextSymbol18 => new { ___b51 = __ContextSymbol18 })
.Where(__ContextSymbol19 => !(__ContextSymbol19.___b51.Destroyed))
.Select(__ContextSymbol20 => __ContextSymbol20.___b51)
.ToList<Beam>()).ToList<Beam>();
	s5 = -1;
return;	
	default: return;}}
	

	int s6=-1;
	public void Rule6(float dt, World world){ 
	switch (s6)
	{

	case -1:
	if(!(UnityEngine.Input.GetKeyDown(KeyCode.Space)))
	{

	s6 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	Beams = new Cons<Beam>(new Beam(), (Beams)).ToList<Beam>();
	s6 = -1;
return;	
	default: return;}}
	

	int s7=-1;
	public void Rule7(float dt, World world){ 
	switch (s7)
	{

	case -1:
	___explo70 = (

(CollidingAsteroidsProjectile).Select(__ContextSymbol21 => new { ___x70 = __ContextSymbol21 })
.Select(__ContextSymbol22 => new Explosion(__ContextSymbol22.___x70.Item1.Position))
.ToList<Explosion>()).ToList<Explosion>();
	if(((___explo70.Count) > (0)))
	{

	goto case 2;	}else
	{

	goto case 3;	}
	case 2:
	Explosions = (___explo70).Concat(Explosions).ToList<Explosion>();
	s7 = -1;
return;
	case 3:
	Explosions = Explosions;
	s7 = -1;
return;	
	default: return;}}
	

	int s8=-1;
	public void Rule8(float dt, World world){ 
	switch (s8)
	{

	case -1:
	Explosions = (

(Explosions).Select(__ContextSymbol23 => new { ___e80 = __ContextSymbol23 })
.Where(__ContextSymbol24 => !(__ContextSymbol24.___e80.Destroyed))
.Select(__ContextSymbol25 => __ContextSymbol25.___e80)
.ToList<Explosion>()).ToList<Explosion>();
	s8 = -1;
return;	
	default: return;}}
	

	int s9=-1;
	public void Rule9(float dt, World world){ 
	switch (s9)
	{

	case -1:
	___explo91 = (

(CollidingAsteroidsShip).Select(__ContextSymbol26 => new { ___x91 = __ContextSymbol26 })
.Select(__ContextSymbol27 => new ShipExplosion(__ContextSymbol27.___x91.Item1.Position))
.ToList<ShipExplosion>()).ToList<ShipExplosion>();
	if(((___explo91.Count) > (0)))
	{

	goto case 1;	}else
	{

	goto case 2;	}
	case 1:
	ShipExplosions = (___explo91).Concat(ShipExplosions).ToList<ShipExplosion>();
	s9 = -1;
return;
	case 2:
	ShipExplosions = ShipExplosions;
	s9 = -1;
return;	
	default: return;}}
	

	int s10=-1;
	public void Rule10(float dt, World world){ 
	switch (s10)
	{

	case -1:
	ShipExplosions = (

(ShipExplosions).Select(__ContextSymbol28 => new { ___e101 = __ContextSymbol28 })
.Where(__ContextSymbol29 => !(__ContextSymbol29.___e101.Destroyed))
.Select(__ContextSymbol30 => __ContextSymbol30.___e101)
.ToList<ShipExplosion>()).ToList<ShipExplosion>();
	s10 = -1;
return;	
	default: return;}}
	

	int s11=-1;
	public void Rule11(float dt, World world){ 
	switch (s11)
	{

	case -1:
	if(!(((CollidingAsteroidsShip.Count) > (0))))
	{

	s11 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	LifeCounter.Life = ((LifeCounter.Life) - (CollidingAsteroidsShip.Count));
	s11 = -1;
return;	
	default: return;}}
	

	int s12=-1;
	public void Rule12(float dt, World world){ 
	switch (s12)
	{

	case -1:
	if(!(((CollidingAsteroidsProjectile.Count) > (0))))
	{

	s12 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	ScoreCounter.Score = ((ScoreCounter.Score) + (((CollidingAsteroidsProjectile.Count) * (10))));
	s12 = -1;
return;	
	default: return;}}
	

	int s13=-1;
	public void Rule13(float dt, World world){ 
	switch (s13)
	{

	case -1:
	if(!(!(((LifeCounter.Life) > (0)))))
	{

	s13 = -1;
return;	}else
	{

	goto case 1;	}
	case 1:
	count_down2 = 1.5f;
	goto case 2;
	case 2:
	if(((count_down2) > (0f)))
	{

	count_down2 = ((count_down2) - (dt));
	s13 = 2;
return;	}else
	{

	goto case 0;	}
	case 0:
	LifeCounter.GameOver = true;
	s13 = -1;
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

(new Cons<Casanova.Prelude.Tuple<UnityEngine.KeyCode, Casanova.Prelude.Tuple<System.Single, UnityEngine.Vector3>>>(new Casanova.Prelude.Tuple<UnityEngine.KeyCode, Casanova.Prelude.Tuple<System.Single, UnityEngine.Vector3>>(KeyCode.A,new Casanova.Prelude.Tuple<System.Single, UnityEngine.Vector3>(10f,new UnityEngine.Vector3(-9f,0f,0f))),(new Cons<Casanova.Prelude.Tuple<UnityEngine.KeyCode, Casanova.Prelude.Tuple<System.Single, UnityEngine.Vector3>>>(new Casanova.Prelude.Tuple<UnityEngine.KeyCode, Casanova.Prelude.Tuple<System.Single, UnityEngine.Vector3>>(KeyCode.D,new Casanova.Prelude.Tuple<System.Single, UnityEngine.Vector3>(-10f,new UnityEngine.Vector3(9f,0f,0f))),(new Empty<Casanova.Prelude.Tuple<UnityEngine.KeyCode, Casanova.Prelude.Tuple<System.Single, UnityEngine.Vector3>>>()).ToList<Casanova.Prelude.Tuple<UnityEngine.KeyCode, Casanova.Prelude.Tuple<System.Single, UnityEngine.Vector3>>>())).ToList<Casanova.Prelude.Tuple<UnityEngine.KeyCode, Casanova.Prelude.Tuple<System.Single, UnityEngine.Vector3>>>())).ToList<Casanova.Prelude.Tuple<UnityEngine.KeyCode, Casanova.Prelude.Tuple<System.Single, UnityEngine.Vector3>>>()).ToList<Casanova.Prelude.Tuple<UnityEngine.KeyCode, Casanova.Prelude.Tuple<System.Single, UnityEngine.Vector3>>>());
		FrontEngine = new Engine((

(new Cons<Casanova.Prelude.Tuple<UnityEngine.KeyCode, Casanova.Prelude.Tuple<System.Single, UnityEngine.Vector3>>>(new Casanova.Prelude.Tuple<UnityEngine.KeyCode, Casanova.Prelude.Tuple<System.Single, UnityEngine.Vector3>>(KeyCode.X,new Casanova.Prelude.Tuple<System.Single, UnityEngine.Vector3>(25f,new UnityEngine.Vector3(0f,0f,0f))),(new Cons<Casanova.Prelude.Tuple<UnityEngine.KeyCode, Casanova.Prelude.Tuple<System.Single, UnityEngine.Vector3>>>(new Casanova.Prelude.Tuple<UnityEngine.KeyCode, Casanova.Prelude.Tuple<System.Single, UnityEngine.Vector3>>(KeyCode.W,new Casanova.Prelude.Tuple<System.Single, UnityEngine.Vector3>(0f,new UnityEngine.Vector3(0f,9f,0f))),(new Cons<Casanova.Prelude.Tuple<UnityEngine.KeyCode, Casanova.Prelude.Tuple<System.Single, UnityEngine.Vector3>>>(new Casanova.Prelude.Tuple<UnityEngine.KeyCode, Casanova.Prelude.Tuple<System.Single, UnityEngine.Vector3>>(KeyCode.S,new Casanova.Prelude.Tuple<System.Single, UnityEngine.Vector3>(0f,new UnityEngine.Vector3(0f,-9f,0f))),(new Empty<Casanova.Prelude.Tuple<UnityEngine.KeyCode, Casanova.Prelude.Tuple<System.Single, UnityEngine.Vector3>>>()).ToList<Casanova.Prelude.Tuple<UnityEngine.KeyCode, Casanova.Prelude.Tuple<System.Single, UnityEngine.Vector3>>>())).ToList<Casanova.Prelude.Tuple<UnityEngine.KeyCode, Casanova.Prelude.Tuple<System.Single, UnityEngine.Vector3>>>())).ToList<Casanova.Prelude.Tuple<UnityEngine.KeyCode, Casanova.Prelude.Tuple<System.Single, UnityEngine.Vector3>>>())).ToList<Casanova.Prelude.Tuple<UnityEngine.KeyCode, Casanova.Prelude.Tuple<System.Single, UnityEngine.Vector3>>>()).ToList<Casanova.Prelude.Tuple<UnityEngine.KeyCode, Casanova.Prelude.Tuple<System.Single, UnityEngine.Vector3>>>());
		
}
		public UnityCamera Camera{  get { return UnityShip.Camera; }
  set{UnityShip.Camera = value; }
 }
	public System.Boolean Destroyed{  get { return UnityShip.Destroyed; }
  set{UnityShip.Destroyed = value; }
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
	RotationFactor = ((RotationFactor) + (((dt) * (35f))));
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
	RotationFactor = ((RotationFactor) - (((dt) * (35f))));
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
	RotationFactor = ((RotationFactor) - (((dt) * (35f))));
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
	RotationFactor = ((RotationFactor) + (((dt) * (35f))));
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

(Controllers).Select(__ContextSymbol35 => new { ___c20 = __ContextSymbol35 })
.Where(__ContextSymbol36 => UnityEngine.Input.GetKey(__ContextSymbol36.___c20.Item1))
.Select(__ContextSymbol37 => __ContextSymbol37.___c20)
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
		public UnityEngine.Color Color{  set{UnityAsteroid.Color = value; }
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
	public System.Single Transparency{  get { return UnityAsteroid.Transparency; }
  set{UnityAsteroid.Transparency = value; }
 }
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
	public List<Asteroid> ___b22;
	public List<Asteroid> ___b33;
	public System.Single count_down3;
	public System.Single count_down4;
	public System.Single count_down5;
	public void Update(float dt, World world) {
frame = World.frame;		this.Rule4(dt, world);

		this.Rule0(dt, world);
		this.Rule1(dt, world);
		this.Rule2(dt, world);
		this.Rule3(dt, world);
		this.Rule5(dt, world);
		this.Rule6(dt, world);
		this.Rule7(dt, world);
		this.Rule8(dt, world);
		this.Rule9(dt, world);
		this.Rule10(dt, world);
		this.Rule11(dt, world);
	}

	public void Rule4(float dt, World world) 
	{
	Rotation = ((UnityEngine.Quaternion.Euler(RotationX,0f,0f)) * (UnityEngine.Quaternion.Euler(0f,RotationY,0f))) * (UnityEngine.Quaternion.Euler(0f,0f,RotationZ));
	}
	




	int s0=-1;
	public void Rule0(float dt, World world){ 
	switch (s0)
	{

	case -1:
	if(!(((1f) > (Transparency))))
	{

	s0 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	Transparency = ((Transparency) + (0.025f));
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
	

	int s2=-1;
	public void Rule2(float dt, World world){ 
	switch (s2)
	{

	case -1:
	___b22 = (

(world.CollidingAsteroidsProjectile).Select(__ContextSymbol38 => new { ___ba20 = __ContextSymbol38 })
.Where(__ContextSymbol39 => ((this) == (__ContextSymbol39.___ba20.Item2)))
.Select(__ContextSymbol40 => __ContextSymbol40.___ba20.Item2)
.ToList<Asteroid>()).ToList<Asteroid>();
	if(((___b22.Count) > (0)))
	{

	goto case 3;	}else
	{

	s2 = -1;
return;	}
	case 3:
	Destroyed = true;
	s2 = -1;
return;	
	default: return;}}
	

	int s3=-1;
	public void Rule3(float dt, World world){ 
	switch (s3)
	{

	case -1:
	___b33 = (

(world.CollidingAsteroidsShip).Select(__ContextSymbol41 => new { ___ba31 = __ContextSymbol41 })
.Where(__ContextSymbol42 => ((this) == (__ContextSymbol42.___ba31.Item2)))
.Select(__ContextSymbol43 => __ContextSymbol43.___ba31.Item2)
.ToList<Asteroid>()).ToList<Asteroid>();
	if(((___b33.Count) > (0)))
	{

	goto case 7;	}else
	{

	s3 = -1;
return;	}
	case 7:
	Destroyed = true;
	s3 = -1;
return;	
	default: return;}}
	

	int s5=-1;
	public void Rule5(float dt, World world){ 
	switch (s5)
	{

	case -1:
	RotationZ = ((RotationZ) + (((RandomZ) * (dt))));
	s5 = -1;
return;	
	default: return;}}
	

	int s6=-1;
	public void Rule6(float dt, World world){ 
	switch (s6)
	{

	case -1:
	RotationY = ((RotationY) + (((RandomY) * (dt))));
	s6 = -1;
return;	
	default: return;}}
	

	int s7=-1;
	public void Rule7(float dt, World world){ 
	switch (s7)
	{

	case -1:
	RotationX = ((RotationX) + (((RandomX) * (dt))));
	s7 = -1;
return;	
	default: return;}}
	

	int s8=-1;
	public void Rule8(float dt, World world){ 
	switch (s8)
	{

	case -1:
	RandomZ = UnityEngine.Random.Range(-90,90);
	s8 = 0;
return;
	case 0:
	count_down3 = UnityEngine.Random.Range(2,4);
	goto case 1;
	case 1:
	if(((count_down3) > (0f)))
	{

	count_down3 = ((count_down3) - (dt));
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
	RandomY = UnityEngine.Random.Range(-90,90);
	s9 = 0;
return;
	case 0:
	count_down4 = UnityEngine.Random.Range(2,4);
	goto case 1;
	case 1:
	if(((count_down4) > (0f)))
	{

	count_down4 = ((count_down4) - (dt));
	s9 = 1;
return;	}else
	{

	s9 = -1;
return;	}	
	default: return;}}
	

	int s10=-1;
	public void Rule10(float dt, World world){ 
	switch (s10)
	{

	case -1:
	RandomX = UnityEngine.Random.Range(-90,90);
	s10 = 0;
return;
	case 0:
	count_down5 = UnityEngine.Random.Range(2,4);
	goto case 1;
	case 1:
	if(((count_down5) > (0f)))
	{

	count_down5 = ((count_down5) - (dt));
	s10 = 1;
return;	}else
	{

	s10 = -1;
return;	}	
	default: return;}}
	

	int s11=-1;
	public void Rule11(float dt, World world){ 
	switch (s11)
	{

	case -1:
	Position = ((Position) + (((new UnityEngine.Vector3(0f,0f,-15f)) * (dt))));
	s11 = -1;
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
	public List<Beam> ___b04;
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
	___b04 = (

(world.CollidingAsteroidsProjectile).Select(__ContextSymbol44 => new { ___ba02 = __ContextSymbol44 })
.Where(__ContextSymbol45 => ((this) == (__ContextSymbol45.___ba02.Item1)))
.Select(__ContextSymbol46 => __ContextSymbol46.___ba02.Item1)
.ToList<Beam>()).ToList<Beam>();
	if(((___b04.Count) > (0)))
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
	if(!(((Position.z) > (100f))))
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
	Position = ((Position) + (((new UnityEngine.Vector3(0f,0f,50f)) * (dt))));
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
	public List<Explosion> ___e02;
	public void Update(float dt, World world) {
frame = World.frame;

		this.Rule0(dt, world);

	}





	int s0=-1;
	public void Rule0(float dt, World world){ 
	switch (s0)
	{

	case -1:
	___e02 = (

(world.Explosions).Select(__ContextSymbol47 => new { ___ex00 = __ContextSymbol47 })
.Where(__ContextSymbol48 => ((this) == (__ContextSymbol48.___ex00)))
.Select(__ContextSymbol49 => __ContextSymbol49.___ex00)
.ToList<Explosion>()).ToList<Explosion>();
	if(((___e02.Count) > (0)))
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
public class ShipExplosion{
public int frame;
public bool JustEntered = true;
private UnityEngine.Vector3 position;
	public int ID;
public ShipExplosion(UnityEngine.Vector3 position)
	{JustEntered = false;
 frame = World.frame;
		UnityShipExplosion = UnityShipExplosion.Instantiate(position);
		
}
		public System.Boolean Destroyed{  get { return UnityShipExplosion.Destroyed; }
  set{UnityShipExplosion.Destroyed = value; }
 }
	public UnityEngine.Vector3 Position{  get { return UnityShipExplosion.Position; }
  set{UnityShipExplosion.Position = value; }
 }
	public UnityEngine.ParticleSystem Ps{  get { return UnityShipExplosion.Ps; }
 }
	public UnityShipExplosion UnityShipExplosion;
	public System.Boolean destroyed{  get { return UnityShipExplosion.destroyed; }
  set{UnityShipExplosion.destroyed = value; }
 }
	public System.Boolean enabled{  get { return UnityShipExplosion.enabled; }
  set{UnityShipExplosion.enabled = value; }
 }
	public UnityEngine.GameObject gameObject{  get { return UnityShipExplosion.gameObject; }
 }
	public UnityEngine.HideFlags hideFlags{  get { return UnityShipExplosion.hideFlags; }
  set{UnityShipExplosion.hideFlags = value; }
 }
	public System.Boolean isActiveAndEnabled{  get { return UnityShipExplosion.isActiveAndEnabled; }
 }
	public System.String name{  get { return UnityShipExplosion.name; }
  set{UnityShipExplosion.name = value; }
 }
	public System.String tag{  get { return UnityShipExplosion.tag; }
  set{UnityShipExplosion.tag = value; }
 }
	public UnityEngine.Transform transform{  get { return UnityShipExplosion.transform; }
 }
	public System.Boolean useGUILayout{  get { return UnityShipExplosion.useGUILayout; }
  set{UnityShipExplosion.useGUILayout = value; }
 }
	public List<ShipExplosion> ___e03;
	public void Update(float dt, World world) {
frame = World.frame;

		this.Rule0(dt, world);

	}





	int s0=-1;
	public void Rule0(float dt, World world){ 
	switch (s0)
	{

	case -1:
	___e03 = (

(world.ShipExplosions).Select(__ContextSymbol50 => new { ___ex01 = __ContextSymbol50 })
.Where(__ContextSymbol51 => ((this) == (__ContextSymbol51.___ex01)))
.Select(__ContextSymbol52 => __ContextSymbol52.___ex01)
.ToList<ShipExplosion>()).ToList<ShipExplosion>();
	if(((___e03.Count) > (0)))
	{

	goto case 6;	}else
	{

	s0 = -1;
return;	}
	case 6:
	Destroyed = true;
	s0 = -1;
return;	
	default: return;}}
	






}
public class MainCamera{
public int frame;
public bool JustEntered = true;
private UnityEngine.Vector3 ship_pos;
	public int ID;
public MainCamera(UnityEngine.Vector3 ship_pos)
	{JustEntered = false;
 frame = World.frame;
		UnityEngine.Vector3 ___def_pos00;
		___def_pos00 = new UnityEngine.Vector3(0f,1f,-25f);
		UnityCamera = UnityCamera.Find((ship_pos) + (___def_pos00));
		ShakingTime = 0f;
		DefaultPos = ___def_pos00;
		
}
		public UnityEngine.Vector3 DefaultPos;
	public UnityEngine.Vector3 Position{  get { return UnityCamera.Position; }
  set{UnityCamera.Position = value; }
 }
	public UnityEngine.Quaternion Rotation{  get { return UnityCamera.Rotation; }
  set{UnityCamera.Rotation = value; }
 }
	public System.Single ShakingTime;
	public UnityCamera UnityCamera;
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
	public System.Single count_down7;
	public System.Single ___x02;
	public System.Single ___y00;
	public System.Single ___z00;
	public System.Single count_down6;
	public void Update(float dt, World world) {
frame = World.frame;

		this.Rule0(dt, world);

	}





	int s0=-1;
	public void Rule0(float dt, World world){ 
	switch (s0)
	{

	case -1:
	if(!(((((((ShakingTime) == (0f))) && (((((world.CollidingAsteroidsProjectile.Count) > (0))) || (((world.CollidingAsteroidsShip.Count) > (0))))))) || (((!(((ShakingTime) > (0f)))) || (((ShakingTime) > (0f))))))))
	{

	s0 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	if(((((ShakingTime) == (0f))) && (((((world.CollidingAsteroidsProjectile.Count) > (0))) || (((world.CollidingAsteroidsShip.Count) > (0)))))))
	{

	goto case 2;	}else
	{

	if(!(((ShakingTime) > (0f))))
	{

	goto case 3;	}else
	{

	if(((ShakingTime) > (0f)))
	{

	goto case 4;	}else
	{

	s0 = 0;
return;	}	}	}
	case 2:
	Position = Position;
	ShakingTime = 0.2f;
	s0 = -1;
return;
	case 3:
	Position = ((world.Ship.Position) + (DefaultPos));
	ShakingTime = 0f;
	s0 = -1;
return;
	case 4:
	count_down7 = dt;
	goto case 15;
	case 15:
	if(((count_down7) > (0f)))
	{

	count_down7 = ((count_down7) - (dt));
	s0 = 15;
return;	}else
	{

	goto case 13;	}
	case 13:
	___x02 = UnityEngine.Random.Range(-0.5f,0.5f);
	___y00 = UnityEngine.Random.Range(-0.5f,0.5f);
	___z00 = UnityEngine.Random.Range(-0.5f,0.5f);
	Position = ((Position) + (new UnityEngine.Vector3(___x02,___y00,___z00)));
	ShakingTime = ((ShakingTime) - (dt));
	s0 = 8;
return;
	case 8:
	count_down6 = dt;
	goto case 9;
	case 9:
	if(((count_down6) > (0f)))
	{

	count_down6 = ((count_down6) - (dt));
	s0 = 9;
return;	}else
	{

	goto case 7;	}
	case 7:
	Position = ((world.Ship.Position) + (DefaultPos));
	ShakingTime = ((ShakingTime) - (dt));
	s0 = -1;
return;	
	default: return;}}
	






}
} 