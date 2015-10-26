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
		Ship ___ship00;
		___ship00 = new Ship();
		UnityParticleSystem = UnityParticleSystem.Find();
		Ship = ___ship00;
		Explosions = (

Enumerable.Empty<Explosion>()).ToList<Explosion>();
		CollidingAsteroidsProjectile = (

Enumerable.Empty<Casanova.Prelude.Tuple<Beam, Asteroid>>()).ToList<Casanova.Prelude.Tuple<Beam, Asteroid>>();
		Camera = new MainCamera(___ship00.Position);
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
	public List<Explosion> Explosions;
	public Ship Ship;
	public UnityParticleSystem UnityParticleSystem;
	public System.Single count_down1;
	public List<Explosion> ___explo60;

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
	CollidingAsteroidsProjectile = (

(Beams).Select(__ContextSymbol4 => new { ___b10 = __ContextSymbol4 })
.SelectMany(__ContextSymbol5=> (Asteroids).Select(__ContextSymbol6 => new { ___a10 = __ContextSymbol6,
                                                      prev = __ContextSymbol5 })
.Where(__ContextSymbol7 => ((1f) > (UnityEngine.Vector3.Distance(__ContextSymbol7.___a10.Position,__ContextSymbol7.prev.___b10.Position))))
.Select(__ContextSymbol8 => new Casanova.Prelude.Tuple<Beam, Asteroid>(__ContextSymbol8.prev.___b10,__ContextSymbol8.___a10))
.ToList<Casanova.Prelude.Tuple<Beam, Asteroid>>())).ToList<Casanova.Prelude.Tuple<Beam, Asteroid>>();
	s1 = -1;
return;	
	default: return;}}
	

	int s2=-1;
	public void Rule2(float dt, World world){ 
	switch (s2)
	{

	case -1:
	Beams = (

(Beams).Select(__ContextSymbol9 => new { ___b21 = __ContextSymbol9 })
.Where(__ContextSymbol10 => !(__ContextSymbol10.___b21.Destroyed))
.Select(__ContextSymbol11 => __ContextSymbol11.___b21)
.ToList<Beam>()).ToList<Beam>();
	s2 = -1;
return;	
	default: return;}}
	

	int s3=-1;
	public void Rule3(float dt, World world){ 
	switch (s3)
	{

	case -1:
	Asteroids = (

(Asteroids).Select(__ContextSymbol12 => new { ___a31 = __ContextSymbol12 })
.Where(__ContextSymbol13 => !(__ContextSymbol13.___a31.Destroyed))
.Select(__ContextSymbol14 => __ContextSymbol14.___a31)
.ToList<Asteroid>()).ToList<Asteroid>();
	s3 = -1;
return;	
	default: return;}}
	

	int s4=-1;
	public void Rule4(float dt, World world){ 
	switch (s4)
	{

	case -1:
	count_down1 = 0.2f;
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
	Asteroids = new Cons<Asteroid>(new Asteroid(new UnityEngine.Vector3((UnityEngine.Random.Range(-8f,8f)) + (Ship.Position.x),(UnityEngine.Random.Range(-8f,8f)) + (Ship.Position.y),(UnityEngine.Random.Range(80f,120f)) + (Ship.Position.z))), (Asteroids)).ToList<Asteroid>();
	s4 = -1;
return;	
	default: return;}}
	

	int s5=-1;
	public void Rule5(float dt, World world){ 
	switch (s5)
	{

	case -1:
	if(!(UnityEngine.Input.GetKeyDown(KeyCode.Space)))
	{

	s5 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	Beams = new Cons<Beam>(new Beam(), (Beams)).ToList<Beam>();
	s5 = -1;
return;	
	default: return;}}
	

	int s6=-1;
	public void Rule6(float dt, World world){ 
	switch (s6)
	{

	case -1:
	___explo60 = (

(CollidingAsteroidsProjectile).Select(__ContextSymbol15 => new { ___x60 = __ContextSymbol15 })
.Select(__ContextSymbol16 => new Explosion(__ContextSymbol16.___x60.Item1.Position))
.ToList<Explosion>()).ToList<Explosion>();
	if(((___explo60.Count) > (0)))
	{

	goto case 2;	}else
	{

	goto case 3;	}
	case 2:
	Explosions = (___explo60).Concat(Explosions).ToList<Explosion>();
	s6 = -1;
return;
	case 3:
	Explosions = Explosions;
	s6 = -1;
return;	
	default: return;}}
	

	int s7=-1;
	public void Rule7(float dt, World world){ 
	switch (s7)
	{

	case -1:
	Explosions = (

(Explosions).Select(__ContextSymbol17 => new { ___e70 = __ContextSymbol17 })
.Where(__ContextSymbol18 => !(__ContextSymbol18.___e70.Destroyed))
.Select(__ContextSymbol19 => __ContextSymbol19.___e70)
.ToList<Explosion>()).ToList<Explosion>();
	s7 = -1;
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

(new Cons<Casanova.Prelude.Tuple<UnityEngine.KeyCode, Casanova.Prelude.Tuple<System.Single, UnityEngine.Vector3>>>(new Casanova.Prelude.Tuple<UnityEngine.KeyCode, Casanova.Prelude.Tuple<System.Single, UnityEngine.Vector3>>(KeyCode.A,new Casanova.Prelude.Tuple<System.Single, UnityEngine.Vector3>(10f,new UnityEngine.Vector3(-5f,0f,0f))),(new Cons<Casanova.Prelude.Tuple<UnityEngine.KeyCode, Casanova.Prelude.Tuple<System.Single, UnityEngine.Vector3>>>(new Casanova.Prelude.Tuple<UnityEngine.KeyCode, Casanova.Prelude.Tuple<System.Single, UnityEngine.Vector3>>(KeyCode.D,new Casanova.Prelude.Tuple<System.Single, UnityEngine.Vector3>(-10f,new UnityEngine.Vector3(5f,0f,0f))),(new Empty<Casanova.Prelude.Tuple<UnityEngine.KeyCode, Casanova.Prelude.Tuple<System.Single, UnityEngine.Vector3>>>()).ToList<Casanova.Prelude.Tuple<UnityEngine.KeyCode, Casanova.Prelude.Tuple<System.Single, UnityEngine.Vector3>>>())).ToList<Casanova.Prelude.Tuple<UnityEngine.KeyCode, Casanova.Prelude.Tuple<System.Single, UnityEngine.Vector3>>>())).ToList<Casanova.Prelude.Tuple<UnityEngine.KeyCode, Casanova.Prelude.Tuple<System.Single, UnityEngine.Vector3>>>()).ToList<Casanova.Prelude.Tuple<UnityEngine.KeyCode, Casanova.Prelude.Tuple<System.Single, UnityEngine.Vector3>>>());
		FrontEngine = new Engine((

(new Cons<Casanova.Prelude.Tuple<UnityEngine.KeyCode, Casanova.Prelude.Tuple<System.Single, UnityEngine.Vector3>>>(new Casanova.Prelude.Tuple<UnityEngine.KeyCode, Casanova.Prelude.Tuple<System.Single, UnityEngine.Vector3>>(KeyCode.X,new Casanova.Prelude.Tuple<System.Single, UnityEngine.Vector3>(25f,new UnityEngine.Vector3(0f,0f,0f))),(new Cons<Casanova.Prelude.Tuple<UnityEngine.KeyCode, Casanova.Prelude.Tuple<System.Single, UnityEngine.Vector3>>>(new Casanova.Prelude.Tuple<UnityEngine.KeyCode, Casanova.Prelude.Tuple<System.Single, UnityEngine.Vector3>>(KeyCode.W,new Casanova.Prelude.Tuple<System.Single, UnityEngine.Vector3>(0f,new UnityEngine.Vector3(0f,5f,0f))),(new Cons<Casanova.Prelude.Tuple<UnityEngine.KeyCode, Casanova.Prelude.Tuple<System.Single, UnityEngine.Vector3>>>(new Casanova.Prelude.Tuple<UnityEngine.KeyCode, Casanova.Prelude.Tuple<System.Single, UnityEngine.Vector3>>(KeyCode.S,new Casanova.Prelude.Tuple<System.Single, UnityEngine.Vector3>(0f,new UnityEngine.Vector3(0f,-5f,0f))),(new Empty<Casanova.Prelude.Tuple<UnityEngine.KeyCode, Casanova.Prelude.Tuple<System.Single, UnityEngine.Vector3>>>()).ToList<Casanova.Prelude.Tuple<UnityEngine.KeyCode, Casanova.Prelude.Tuple<System.Single, UnityEngine.Vector3>>>())).ToList<Casanova.Prelude.Tuple<UnityEngine.KeyCode, Casanova.Prelude.Tuple<System.Single, UnityEngine.Vector3>>>())).ToList<Casanova.Prelude.Tuple<UnityEngine.KeyCode, Casanova.Prelude.Tuple<System.Single, UnityEngine.Vector3>>>())).ToList<Casanova.Prelude.Tuple<UnityEngine.KeyCode, Casanova.Prelude.Tuple<System.Single, UnityEngine.Vector3>>>()).ToList<Casanova.Prelude.Tuple<UnityEngine.KeyCode, Casanova.Prelude.Tuple<System.Single, UnityEngine.Vector3>>>());
		
}
		public UnityCamera Camera{  get { return UnityShip.Camera; }
  set{UnityShip.Camera = value; }
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

(Controllers).Select(__ContextSymbol24 => new { ___c20 = __ContextSymbol24 })
.Where(__ContextSymbol25 => UnityEngine.Input.GetKey(__ContextSymbol25.___c20.Item1))
.Select(__ContextSymbol26 => __ContextSymbol26.___c20)
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
	public List<Asteroid> ___b12;
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
		this.Rule10(dt, world);
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
	___b12 = (

(world.CollidingAsteroidsProjectile).Select(__ContextSymbol27 => new { ___ba10 = __ContextSymbol27 })
.Where(__ContextSymbol28 => ((this) == (__ContextSymbol28.___ba10.Item2)))
.Select(__ContextSymbol29 => __ContextSymbol29.___ba10.Item2)
.ToList<Asteroid>()).ToList<Asteroid>();
	if(((___b12.Count) > (0)))
	{

	goto case 3;	}else
	{

	s1 = -1;
return;	}
	case 3:
	Destroyed = true;
	s1 = -1;
return;	
	default: return;}}
	

	int s2=-1;
	public void Rule2(float dt, World world){ 
	switch (s2)
	{

	case -1:
	if(!(((-10f) > (Position.z))))
	{

	s2 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	Destroyed = true;
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
	RandomZ = UnityEngine.Random.Range(-90,90);
	s7 = 0;
return;
	case 0:
	count_down2 = UnityEngine.Random.Range(2,4);
	goto case 1;
	case 1:
	if(((count_down2) > (0f)))
	{

	count_down2 = ((count_down2) - (dt));
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
	RandomY = UnityEngine.Random.Range(-90,90);
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
	RandomX = UnityEngine.Random.Range(-90,90);
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
	Position = ((Position) + (((new UnityEngine.Vector3(0f,0f,-15f)) * (dt))));
	s10 = -1;
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

(world.CollidingAsteroidsProjectile).Select(__ContextSymbol30 => new { ___ba01 = __ContextSymbol30 })
.Where(__ContextSymbol31 => ((this) == (__ContextSymbol31.___ba01.Item1)))
.Select(__ContextSymbol32 => __ContextSymbol32.___ba01.Item1)
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

(world.Explosions).Select(__ContextSymbol33 => new { ___ex00 = __ContextSymbol33 })
.Where(__ContextSymbol34 => ((this) == (__ContextSymbol34.___ex00)))
.Select(__ContextSymbol35 => __ContextSymbol35.___ex00)
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
	public System.Single count_down6;
	public System.Single ___x01;
	public System.Single ___y00;
	public System.Single ___z00;
	public System.Single count_down5;
	public void Update(float dt, World world) {
frame = World.frame;

		this.Rule0(dt, world);

	}





	int s0=-1;
	public void Rule0(float dt, World world){ 
	switch (s0)
	{

	case -1:
	if(!(((((((ShakingTime) == (0f))) && (((world.CollidingAsteroidsProjectile.Count) > (0))))) || (((!(((ShakingTime) > (0f)))) || (((ShakingTime) > (0f))))))))
	{

	s0 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	if(((((ShakingTime) == (0f))) && (((world.CollidingAsteroidsProjectile.Count) > (0)))))
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
	ShakingTime = 0.1f;
	s0 = -1;
return;
	case 3:
	Position = ((world.Ship.Position) + (DefaultPos));
	ShakingTime = 0f;
	s0 = -1;
return;
	case 4:
	count_down6 = dt;
	goto case 15;
	case 15:
	if(((count_down6) > (0f)))
	{

	count_down6 = ((count_down6) - (dt));
	s0 = 15;
return;	}else
	{

	goto case 13;	}
	case 13:
	___x01 = UnityEngine.Random.Range(-0.5f,0.5f);
	___y00 = UnityEngine.Random.Range(-0.5f,0.5f);
	___z00 = UnityEngine.Random.Range(-0.5f,0.5f);
	Position = ((Position) + (new UnityEngine.Vector3(___x01,___y00,___z00)));
	ShakingTime = ((ShakingTime) - (dt));
	s0 = 8;
return;
	case 8:
	count_down5 = dt;
	goto case 9;
	case 9:
	if(((count_down5) > (0f)))
	{

	count_down5 = ((count_down5) - (dt));
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