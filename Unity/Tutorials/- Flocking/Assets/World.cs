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
		Boids = (

(Enumerable.Range(0,(1) + ((10) - (0))).ToList<System.Int32>()).Select(__ContextSymbol0 => new { ___b00 = __ContextSymbol0 })
.Select(__ContextSymbol1 => new Boid(new UnityEngine.Vector3(UnityEngine.Random.Range(-10,10),UnityEngine.Random.Range(-10,10),0f)))
.ToList<Boid>()).ToList<Boid>();
		BoidBoss = new BoidLeader(new UnityEngine.Vector3(0f,10f,0f));
		
}
		public BoidLeader __BoidBoss;
	public BoidLeader BoidBoss{  get { return  __BoidBoss; }
  set{ __BoidBoss = value;
 if(!value.JustEntered) __BoidBoss = value; 
 else{ value.JustEntered = false;
}
 }
 }
	public List<Boid> Boids;

System.DateTime init_time = System.DateTime.Now;
	public void Update(float dt, World world) {
var t = System.DateTime.Now;

		BoidBoss.Update(dt, world);
		for(int x0 = 0; x0 < Boids.Count; x0++) { 
			Boids[x0].Update(dt, world);
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
		Velocity = Vector3.zero;
		UnityBoid = UnityBoid.Instantiate(pos);
		Separation = Vector3.zero;
		Seek = Vector3.zero;
		MaxVelocity = 4f;
		Cohesion = Vector3.zero;
		
}
		public UnityEngine.Vector3 Cohesion;
	public System.Single MaxVelocity;
	public UnityEngine.Vector3 Position{  get { return UnityBoid.Position; }
  set{UnityBoid.Position = value; }
 }
	public UnityEngine.Vector3 Seek;
	public UnityEngine.Vector3 Separation;
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
	public System.Single ___max_dist10;
	public List<UnityEngine.Vector3> ___positions10;
	public List<UnityEngine.Vector3> ___positions11;
	public UnityEngine.Vector3 ___position_total10;
	public System.Int32 ___neighbours10;
	public UnityEngine.Vector3 ___average10;
	public UnityEngine.Vector3 ___desired10;
	public UnityEngine.Vector3 ___desired11;
	public UnityEngine.Vector3 ___steer10;
	public System.Single ___max_dist21;
	public List<UnityEngine.Vector3> ___list_neighbours20;
	public List<UnityEngine.Vector3> ___list_neighbours21;
	public UnityEngine.Vector3 ___list_position_neighbours20;
	public UnityEngine.Vector3 ___average21;
	public UnityEngine.Vector3 ___p20;
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
	Position = ((Position) + (((Velocity) * (dt))));
	s0 = -1;
return;	
	default: return;}}
	

	int s1=-1;
	public void Rule1(float dt, World world){ 
	switch (s1)
	{

	case -1:
	___max_dist10 = 5f;
	___positions10 = (

(world.Boids).Select(__ContextSymbol2 => new { ___boid10 = __ContextSymbol2 })
.Where(__ContextSymbol3 => ((!(((__ContextSymbol3.___boid10) == (this)))) && (((___max_dist10) > (UnityEngine.Vector3.Distance(this.Position,__ContextSymbol3.___boid10.Position))))))
.Select(__ContextSymbol4 => __ContextSymbol4.___boid10.Position)
.ToList<UnityEngine.Vector3>()).ToList<UnityEngine.Vector3>();
	___positions11 = new Cons<UnityEngine.Vector3>(world.BoidBoss.Position, (___positions10)).ToList<UnityEngine.Vector3>();
	___position_total10 = (

(___positions11).Select(__ContextSymbol5 => new { ___boid11 = __ContextSymbol5 })
.Select(__ContextSymbol6 => __ContextSymbol6.___boid11)
.Aggregate(default(UnityEngine.Vector3), (acc, __x) => acc + __x));
	___neighbours10 = ___positions11.Count;
	___average10 = new UnityEngine.Vector3((___position_total10.x) / (___neighbours10),(___position_total10.y) / (___neighbours10),0f);
	___desired10 = ((___average10) - (Position));
	___desired11 = ((___desired10.normalized) * (4f));
	___steer10 = ((___desired11) - (Velocity));
	Velocity = ((Velocity) + (((((___steer10.normalized) * (3f))) * (dt))));
	s1 = -1;
return;	
	default: return;}}
	

	int s2=-1;
	public void Rule2(float dt, World world){ 
	switch (s2)
	{

	case -1:
	___max_dist21 = 5f;
	___list_neighbours20 = (

(world.Boids).Select(__ContextSymbol8 => new { ___boid22 = __ContextSymbol8 })
.Where(__ContextSymbol9 => ((!(((__ContextSymbol9.___boid22) == (this)))) && (((___max_dist21) > (UnityEngine.Vector3.Distance(Position,__ContextSymbol9.___boid22.Position))))))
.Select(__ContextSymbol10 => __ContextSymbol10.___boid22.Position)
.ToList<UnityEngine.Vector3>()).ToList<UnityEngine.Vector3>();
	if(((___max_dist21) > (UnityEngine.Vector3.Distance(world.BoidBoss.Position,Position))))
	{

	___list_neighbours21 = new Cons<UnityEngine.Vector3>(world.BoidBoss.Position, (___list_neighbours20)).ToList<UnityEngine.Vector3>();	}else
	{

	___list_neighbours21 = ___list_neighbours20;	}
	___list_position_neighbours20 = (

(___list_neighbours21).Select(__ContextSymbol11 => new { ___boid23 = __ContextSymbol11 })
.Select(__ContextSymbol12 => __ContextSymbol12.___boid23)
.Aggregate(default(UnityEngine.Vector3), (acc, __x) => acc + __x));
	___average21 = new UnityEngine.Vector3((___list_position_neighbours20.x) / (___list_neighbours21.Count),(___list_position_neighbours20.y) / (___list_neighbours21.Count),0f);
	___p20 = ((Position) - (___average21));
	Velocity = ((Velocity) + (((((___p20.normalized) * (5f))) * (dt))));
	s2 = -1;
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
		MaxVelocity = 6f;
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
	public System.Single MaxVelocity;
	public UnityEngine.Vector3 Position{  get { return UnityBoidLeader.Position; }
  set{UnityBoidLeader.Position = value; }
 }
	public UnityEngine.Vector3 Right{  get { return UnityBoidLeader.Right; }
 }
	public UnityEngine.Quaternion Rotation{  get { return UnityBoidLeader.Rotation; }
  set{UnityBoidLeader.Rotation = value; }
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
	if(!(Start))
	{

	s4 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	Camera.Position = ((Position) + (new UnityEngine.Vector3(0f,1f,-20f)));
	Start = false;
	s4 = -1;
return;	
	default: return;}}
	





}
}         