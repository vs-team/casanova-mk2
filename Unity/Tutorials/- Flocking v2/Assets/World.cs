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
		Spheres = (

(Enumerable.Range(-1,(1) + ((1) - (-1))).ToList<System.Int32>()).Select(__ContextSymbol2 => new { ___x00 = __ContextSymbol2 })
.SelectMany(__ContextSymbol3=> (Enumerable.Range(-1,(1) + ((1) - (-1))).ToList<System.Int32>()).Select(__ContextSymbol4 => new { ___y00 = __ContextSymbol4,
                                                      prev = __ContextSymbol3 })
.Select(__ContextSymbol5 => new Sphere(new UnityEngine.Vector3((__ContextSymbol5.prev.___x00) * (20f),(__ContextSymbol5.___y00) * (20f),0f)))
.ToList<Sphere>())).ToList<Sphere>();
		Boids = (

(Enumerable.Range(1,(1) + ((40) - (1))).ToList<System.Int32>()).Select(__ContextSymbol0 => new { ___a00 = __ContextSymbol0 })
.Select(__ContextSymbol1 => new Boid(new UnityEngine.Vector3(UnityEngine.Random.Range(-10,10),UnityEngine.Random.Range(-10,10),0f)))
.ToList<Boid>()).ToList<Boid>();
		BoidBoss = new BoidLeader(new UnityEngine.Vector3(-10f,0f,0f));
		
}
		public BoidLeader BoidBoss;
	public List<Boid> Boids;
	public List<Sphere> Spheres;

System.DateTime init_time = System.DateTime.Now;
	public void Update(float dt, World world) {
var t = System.DateTime.Now;

		BoidBoss.Update(dt, world);
		for(int x0 = 0; x0 < Boids.Count; x0++) { 
			Boids[x0].Update(dt, world);
		}
		for(int x0 = 0; x0 < Spheres.Count; x0++) { 
			Spheres[x0].Update(dt, world);
		}


	}











}
public class Boid{
public int frame;
public bool JustEntered = true;
private UnityEngine.Vector3 pos;
	public int ID;
public Boid(UnityEngine.Vector3 pos)
	{JustEntered = false;
 frame = World.frame;
		Velocity = new UnityEngine.Vector3(0f,0f,0f);
		UnityBoid = UnityBoid.Instantiate(pos);
		SeparationImpulse = 20f;
		SeparationFactor = 100000f;
		Separation = Vector3.zero;
		SeekImpulse = 5f;
		Seek = Vector3.zero;
		MaxVelocity = 8f;
		MaxDist = (Scale.x) * (2f);
		FrictionFactor = 0.4f;
		Friction = Vector3.zero;
		Force = Vector3.zero;
		CohesionMaxDist = 30f;
		CohesionImpulse = 0.9f;
		Cohesion = Vector3.zero;
		Acceleration = Vector3.zero;
		
}
		public UnityEngine.Vector3 Acceleration;
	public UnityEngine.Vector3 Cohesion;
	public System.Single CohesionImpulse;
	public System.Single CohesionMaxDist;
	public UnityEngine.Vector3 Force;
	public UnityEngine.Vector3 Friction;
	public System.Single FrictionFactor;
	public System.Single MaxDist;
	public System.Single MaxVelocity;
	public UnityEngine.Vector3 Position{  get { return UnityBoid.Position; }
  set{UnityBoid.Position = value; }
 }
	public UnityEngine.Vector3 Scale{  get { return UnityBoid.Scale; }
 }
	public UnityEngine.Vector3 Seek;
	public System.Single SeekImpulse;
	public UnityEngine.Vector3 Separation;
	public System.Single SeparationFactor;
	public System.Single SeparationImpulse;
	public UnityBoid UnityBoid;
	public UnityEngine.Vector3 Velocity;
	public System.Boolean enabled{  get { return UnityBoid.enabled; }
  set{UnityBoid.enabled = value; }
 }
	public UnityEngine.GameObject gameObject{  get { return UnityBoid.gameObject; }
 }
	public UnityEngine.HideFlags hideFlags{  get { return UnityBoid.hideFlags; }
  set{UnityBoid.hideFlags = value; }
 }
	public System.Boolean isActiveAndEnabled{  get { return UnityBoid.isActiveAndEnabled; }
 }
	public System.String name{  get { return UnityBoid.name; }
  set{UnityBoid.name = value; }
 }
	public System.String tag{  get { return UnityBoid.tag; }
  set{UnityBoid.tag = value; }
 }
	public UnityEngine.Transform transform{  get { return UnityBoid.transform; }
 }
	public System.Boolean useGUILayout{  get { return UnityBoid.useGUILayout; }
  set{UnityBoid.useGUILayout = value; }
 }
	public UnityEngine.Vector3 ___friction30;
	public List<Casanova.Prelude.Tuple<UnityEngine.Vector3, System.Single>> ___close_boids_positions40;
	public UnityEngine.Vector3 ___close_boids_sum40;
	public System.Single ___boss_dist40;
	public System.Boolean ___is_boss_close40;
	public UnityEngine.Vector3 ___close_boids_sum41;
	public UnityEngine.Vector3 ___diff41;
	public System.Int32 ___total_count40;
	public UnityEngine.Vector3 ___avg40;
	public UnityEngine.Vector3 ___steer40;
	public System.Single ___boss_dist51;
	public System.Boolean ___is_boss_close51;
	public UnityEngine.Vector3 ___desired50;
	public UnityEngine.Vector3 ___steer51;
	public UnityEngine.Vector3 ___close_neighbors60;
	public List<Boid> ___counter60;
	public UnityEngine.Vector3 ___desired61;
	public UnityEngine.Vector3 ___steer62;
	public void Update(float dt, World world) {
frame = World.frame;

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
	Acceleration = ((((((Seek) + (Cohesion))) + (Friction))) + (Separation));
	s0 = -1;
return;	
	default: return;}}
	

	int s1=-1;
	public void Rule1(float dt, World world){ 
	switch (s1)
	{

	case -1:
	Velocity = ((Velocity) + (((Acceleration) * (dt))));
	s1 = -1;
return;	
	default: return;}}
	

	int s2=-1;
	public void Rule2(float dt, World world){ 
	switch (s2)
	{

	case -1:
	Position = ((Position) + (((Velocity) * (dt))));
	s2 = -1;
return;	
	default: return;}}
	

	int s3=-1;
	public void Rule3(float dt, World world){ 
	switch (s3)
	{

	case -1:
	___friction30 = Velocity.normalized;
	Friction = ((((___friction30) * (-1f))) * (FrictionFactor));
	s3 = -1;
return;	
	default: return;}}
	

	int s4=-1;
	public void Rule4(float dt, World world){ 
	switch (s4)
	{

	case -1:
	___close_boids_positions40 = (

(world.Boids).Select(__ContextSymbol6 => new { ___boid40 = __ContextSymbol6 })
.Select(__ContextSymbol7 => new {___d40 = UnityEngine.Vector3.Distance(Position,__ContextSymbol7.___boid40.Position), prev = __ContextSymbol7 })
.Where(__ContextSymbol8 => ((((((__ContextSymbol8.___d40) > (0))) && (((MaxDist) > (__ContextSymbol8.___d40))))) && (!(((this) == (__ContextSymbol8.prev.___boid40))))))
.Select(__ContextSymbol9 => new Casanova.Prelude.Tuple<UnityEngine.Vector3, System.Single>((Position) - (__ContextSymbol9.prev.___boid40.Position),__ContextSymbol9.___d40))
.ToList<Casanova.Prelude.Tuple<UnityEngine.Vector3, System.Single>>()).ToList<Casanova.Prelude.Tuple<UnityEngine.Vector3, System.Single>>();
	___close_boids_sum40 = (

(___close_boids_positions40).Select(__ContextSymbol10 => new { ___item40 = __ContextSymbol10 })
.Select(__ContextSymbol11 => new {___d41 = __ContextSymbol11.___item40.Item2, prev = __ContextSymbol11 })
.Select(__ContextSymbol12 => new {___diff40 = __ContextSymbol12.prev.___item40.Item1, prev = __ContextSymbol12 })
.Select(__ContextSymbol13 => (__ContextSymbol13.___diff40.normalized) / ((__ContextSymbol13.prev.___d41) / (SeparationFactor)))
.Aggregate(default(UnityEngine.Vector3), (acc, __x) => acc + __x));
	___boss_dist40 = UnityEngine.Vector3.Distance(Position,world.BoidBoss.Position);
	___is_boss_close40 = ((((___boss_dist40) > (0))) && (((world.BoidBoss.MaxDist) > (___boss_dist40))));
	if(___is_boss_close40)
	{

	___diff41 = ((Position) - (world.BoidBoss.Position));
	___close_boids_sum41 = ((((___diff41.normalized) / (((___boss_dist40) / (SeparationFactor))))) + (___close_boids_sum40));	}else
	{

	___close_boids_sum41 = ___close_boids_sum40;	}
	if(((((___close_boids_positions40.Count) > (0))) || (___is_boss_close40)))
	{

	goto case 2;	}else
	{

	goto case 3;	}
	case 2:
	if(___is_boss_close40)
	{

	___total_count40 = ((1) + (___close_boids_positions40.Count));	}else
	{

	___total_count40 = ___close_boids_positions40.Count;	}
	___avg40 = new UnityEngine.Vector3((___close_boids_sum41.x) / (___total_count40),(___close_boids_sum41.y) / (___total_count40),0f);
	___steer40 = ((___avg40.normalized) * (MaxVelocity));
	Separation = ((((___steer40) - (Velocity))) * (SeparationImpulse));
	s4 = -1;
return;
	case 3:
	Separation = Vector3.zero;
	s4 = -1;
return;	
	default: return;}}
	

	int s5=-1;
	public void Rule5(float dt, World world){ 
	switch (s5)
	{

	case -1:
	___boss_dist51 = UnityEngine.Vector3.Distance(Position,world.BoidBoss.Position);
	___is_boss_close51 = ((((___boss_dist51) > (0))) && (((world.BoidBoss.MaxDist) > (___boss_dist51))));
	if(!(___is_boss_close51))
	{

	goto case 15;	}else
	{

	goto case 16;	}
	case 15:
	___desired50 = ((world.BoidBoss.Position) - (Position));
	___steer51 = ((___desired50.normalized) * (MaxVelocity));
	Seek = ((((___steer51) - (Velocity))) * (SeekImpulse));
	s5 = -1;
return;
	case 16:
	Seek = Vector3.zero;
	s5 = -1;
return;	
	default: return;}}
	

	int s6=-1;
	public void Rule6(float dt, World world){ 
	switch (s6)
	{

	case -1:
	___close_neighbors60 = (

(world.Boids).Select(__ContextSymbol15 => new { ___boid61 = __ContextSymbol15 })
.Select(__ContextSymbol16 => new {___d62 = UnityEngine.Vector3.Distance(Position,__ContextSymbol16.___boid61.Position), prev = __ContextSymbol16 })
.Where(__ContextSymbol17 => ((((CohesionMaxDist) > (__ContextSymbol17.___d62))) && (!(((this) == (__ContextSymbol17.prev.___boid61))))))
.Select(__ContextSymbol18 => __ContextSymbol18.prev.___boid61.Position)
.Aggregate(default(UnityEngine.Vector3), (acc, __x) => acc + __x));
	___counter60 = (

(world.Boids).Select(__ContextSymbol20 => new { ___boid62 = __ContextSymbol20 })
.Select(__ContextSymbol21 => new {___d63 = UnityEngine.Vector3.Distance(Position,__ContextSymbol21.___boid62.Position), prev = __ContextSymbol21 })
.Where(__ContextSymbol22 => (((((MaxDist) * (CohesionMaxDist)) > (__ContextSymbol22.___d63))) && (!(((this) == (__ContextSymbol22.prev.___boid62))))))
.Select(__ContextSymbol23 => __ContextSymbol23.prev.___boid62)
.ToList<Boid>()).ToList<Boid>();
	if(((___counter60.Count) > (0)))
	{

	goto case 24;	}else
	{

	goto case 25;	}
	case 24:
	___desired61 = new UnityEngine.Vector3((___close_neighbors60.x) / (___counter60.Count),(___close_neighbors60.y) / (___counter60.Count),0f);
	___steer62 = ((___desired61.normalized) * (MaxVelocity));
	Cohesion = ((((___steer62) - (Velocity))) * (CohesionImpulse));
	s6 = -1;
return;
	case 25:
	Cohesion = Vector3.zero;
	s6 = -1;
return;	
	default: return;}}
	





}
public class BoidLeader{
public int frame;
public bool JustEntered = true;
private UnityEngine.Vector3 position;
	public int ID;
public BoidLeader(UnityEngine.Vector3 position)
	{JustEntered = false;
 frame = World.frame;
		UnityBoidLeader = UnityBoidLeader.Instantiate(position);
		Start = true;
		MaxVelocity = 5f;
		MaxDist = (Scale.x) * (3f);
		Camera = UnityCamera.Find();
		
}
		public UnityEngine.Vector3 Backward{  get { return UnityBoidLeader.Backward; }
 }
	public UnityCamera Camera;
	public UnityEngine.Vector3 Down{  get { return UnityBoidLeader.Down; }
 }
	public UnityEngine.Vector3 Forward{  get { return UnityBoidLeader.Forward; }
 }
	public UnityEngine.Vector3 Left{  get { return UnityBoidLeader.Left; }
 }
	public System.Single MaxDist;
	public System.Single MaxVelocity;
	public UnityEngine.Vector3 Position{  get { return UnityBoidLeader.Position; }
  set{UnityBoidLeader.Position = value; }
 }
	public UnityEngine.Vector3 Right{  get { return UnityBoidLeader.Right; }
 }
	public UnityEngine.Quaternion Rotation{  get { return UnityBoidLeader.Rotation; }
  set{UnityBoidLeader.Rotation = value; }
 }
	public UnityEngine.Vector3 Scale{  get { return UnityBoidLeader.Scale; }
 }
	public System.Boolean Start;
	public UnityBoidLeader UnityBoidLeader;
	public UnityEngine.Vector3 Up{  get { return UnityBoidLeader.Up; }
 }
	public System.Boolean enabled{  get { return UnityBoidLeader.enabled; }
  set{UnityBoidLeader.enabled = value; }
 }
	public UnityEngine.GameObject gameObject{  get { return UnityBoidLeader.gameObject; }
 }
	public UnityEngine.HideFlags hideFlags{  get { return UnityBoidLeader.hideFlags; }
  set{UnityBoidLeader.hideFlags = value; }
 }
	public System.Boolean isActiveAndEnabled{  get { return UnityBoidLeader.isActiveAndEnabled; }
 }
	public System.String name{  get { return UnityBoidLeader.name; }
  set{UnityBoidLeader.name = value; }
 }
	public System.String tag{  get { return UnityBoidLeader.tag; }
  set{UnityBoidLeader.tag = value; }
 }
	public UnityEngine.Transform transform{  get { return UnityBoidLeader.transform; }
 }
	public System.Boolean useGUILayout{  get { return UnityBoidLeader.useGUILayout; }
  set{UnityBoidLeader.useGUILayout = value; }
 }
	public void Update(float dt, World world) {
frame = World.frame;

		this.Rule0(dt, world);
		this.Rule1(dt, world);
		this.Rule2(dt, world);
		this.Rule3(dt, world);
		this.Rule4(dt, world);
	}





	int s0=-1;
	public void Rule0(float dt, World world){ 
	switch (s0)
	{

	case -1:
	if(!(UnityEngine.Input.GetKey(KeyCode.S)))
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
	if(!(UnityEngine.Input.GetKey(KeyCode.W)))
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
	if(!(UnityEngine.Input.GetKey(KeyCode.D)))
	{

	s2 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	Position = ((Position) + (((((Right) * (dt))) * (MaxVelocity))));
	s2 = -1;
return;	
	default: return;}}
	

	int s3=-1;
	public void Rule3(float dt, World world){ 
	switch (s3)
	{

	case -1:
	if(!(UnityEngine.Input.GetKey(KeyCode.A)))
	{

	s3 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	Position = ((Position) + (((((Left) * (dt))) * (MaxVelocity))));
	s3 = -1;
return;	
	default: return;}}
	

	int s4=-1;
	public void Rule4(float dt, World world){ 
	switch (s4)
	{

	case -1:
	Camera.Position = ((Position) + (new UnityEngine.Vector3(0f,1f,-200f)));
	s4 = -1;
return;	
	default: return;}}
	





}
public class Sphere{
public int frame;
public bool JustEntered = true;
private UnityEngine.Vector3 position;
	public int ID;
public Sphere(UnityEngine.Vector3 position)
	{JustEntered = false;
 frame = World.frame;
		UnitySphere = UnitySphere.Instantiate(position);
		Speed = UnityEngine.Random.Range(15f,45f);
		RotationZ = UnityEngine.Random.Range(-50f,50f);
		
}
		public UnityEngine.Vector3 Position{  get { return UnitySphere.Position; }
  set{UnitySphere.Position = value; }
 }
	public UnityEngine.Quaternion Rotation{  get { return UnitySphere.Rotation; }
  set{UnitySphere.Rotation = value; }
 }
	public System.Single RotationZ;
	public System.Single Speed;
	public UnitySphere UnitySphere;
	public System.Boolean enabled{  get { return UnitySphere.enabled; }
  set{UnitySphere.enabled = value; }
 }
	public UnityEngine.GameObject gameObject{  get { return UnitySphere.gameObject; }
 }
	public UnityEngine.HideFlags hideFlags{  get { return UnitySphere.hideFlags; }
  set{UnitySphere.hideFlags = value; }
 }
	public System.Boolean isActiveAndEnabled{  get { return UnitySphere.isActiveAndEnabled; }
 }
	public System.String name{  get { return UnitySphere.name; }
  set{UnitySphere.name = value; }
 }
	public System.String tag{  get { return UnitySphere.tag; }
  set{UnitySphere.tag = value; }
 }
	public UnityEngine.Transform transform{  get { return UnitySphere.transform; }
 }
	public System.Boolean useGUILayout{  get { return UnitySphere.useGUILayout; }
  set{UnitySphere.useGUILayout = value; }
 }
	public void Update(float dt, World world) {
frame = World.frame;		this.Rule0(dt, world);

		this.Rule1(dt, world);

	}

	public void Rule0(float dt, World world) 
	{
	Rotation = ((UnityEngine.Quaternion.Euler(0f,0f,0f)) * (UnityEngine.Quaternion.Euler(0f,0f,0f))) * (UnityEngine.Quaternion.Euler(0f,0f,RotationZ));
	}
	




	int s1=-1;
	public void Rule1(float dt, World world){ 
	switch (s1)
	{

	case -1:
	RotationZ = ((RotationZ) + (((Speed) * (dt))));
	s1 = -1;
return;	
	default: return;}}
	






}
}      