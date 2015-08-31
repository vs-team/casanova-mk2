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
	{		List<Planet> ___planets00;
		___planets00 = (

(Enumerable.Range(1,(1) + ((50) - (1))).ToList<System.Int32>()).Select(__ContextSymbol0 => new { ___i00 = __ContextSymbol0 })
.Select(__ContextSymbol1 => new {___pos00 = new UnityEngine.Vector3(UnityEngine.Random.Range(-140f,140f),0f,UnityEngine.Random.Range(-140f,140f)), prev = __ContextSymbol1 })
.Select(__ContextSymbol2 => new {___mass01 = UnityEngine.Random.Range(3f,350f), prev = __ContextSymbol2 })
.Select(__ContextSymbol3 => new {___rotv01 = new UnityEngine.Vector3(UnityEngine.Random.Range(-50f,50f),UnityEngine.Random.Range(-50f,50f),UnityEngine.Random.Range(-50f,50f)), prev = __ContextSymbol3 })
.Select(__ContextSymbol4 => new Planet(__ContextSymbol4.prev.prev.___pos00,__ContextSymbol4.prev.___mass01,Vector3.zero,__ContextSymbol4.___rotv01))
.ToList<Planet>()).ToList<Planet>();
		Planets = ___planets00;
		MainCamera = new GameCamera();
		}
		public GameCamera MainCamera;
	public List<Planet> __Planets;
	public List<Planet> Planets{  get { return  __Planets; }
  set{  foreach(var e in value){if(e.JustEntered){ e.JustEntered = false;
}
} }
 }
	public System.Single count_down1;
	public System.Single ___posx10;
	public System.Single ___posz10;
	public System.Single ___posx11;
	public System.Single ___posz11;
	public System.Single ___mass10;
	public UnityEngine.Vector3 ___position10;
	public UnityEngine.Vector3 ___velocity10;
	public UnityEngine.Vector3 ___rotv10;
	public Planet ___newPlanet10;
	public void Update(float dt, World world) {
		MainCamera.Update(dt, world);
		for(int x0 = 0; x0 < Planets.Count; x0++) { 
			Planets[x0].Update(dt, world);
		}
		this.Rule0(dt, world);

		this.Rule1(dt, world);

	}
	public void Rule0(float dt, World world) 
	{
	Planets = (

(Planets).Select(__ContextSymbol5 => new { ___p00 = __ContextSymbol5 })
.Where(__ContextSymbol6 => !(__ContextSymbol6.___p00.OutOfBounds))
.Select(__ContextSymbol7 => __ContextSymbol7.___p00)
.ToList<Planet>()).ToList<Planet>();
	}
	




	int s1=-1;
	public void Rule1(float dt, World world){ 
	switch (s1)
	{

	case -1:
	count_down1 = 1f;
	goto case 11;
	case 11:
	if(((count_down1) > (0f)))
	{

	count_down1 = ((count_down1) - (dt));
	s1 = 11;
return;	}else
	{

	goto case 9;	}
	case 9:
	___posx10 = UnityEngine.Random.Range(175f,200f);
	___posz10 = UnityEngine.Random.Range(175f,200f);
	if(((UnityEngine.Random.value) > (0.5f)))
	{

	___posx11 = ___posx10;	}else
	{

	___posx11 = ((___posx10) * (-1f));	}
	if(((UnityEngine.Random.value) > (0.5f)))
	{

	___posz11 = ___posz10;	}else
	{

	___posz11 = ((___posz10) * (-1f));	}
	___mass10 = UnityEngine.Random.Range(3f,350f);
	___position10 = new UnityEngine.Vector3(___posx11,0f,___posz11);
	___velocity10 = ((UnityEngine.Vector3.Normalize((___position10) * (-1f))) * (UnityEngine.Random.Range(1f,6f)));
	___rotv10 = new UnityEngine.Vector3(UnityEngine.Random.Range(-50f,50f),UnityEngine.Random.Range(-50f,50f),UnityEngine.Random.Range(-50f,50f));
	___newPlanet10 = new Planet(___position10,___mass10,___velocity10,___rotv10);
	Planets = new Cons<Planet>(___newPlanet10, (Planets)).ToList<Planet>();
	s1 = -1;
return;	
	default: return;}}
	






}
public class GameCamera{
public int frame;
public bool JustEntered = true;
	public int ID;
public GameCamera()
	{JustEntered = false;		UnityCamera = UnityCamera.CreateMainCamera();
		}
		public UnityEngine.Vector3 CameraPosition{  get { return UnityCamera.CameraPosition; }
  set{UnityCamera.CameraPosition = value; }
 }
	public System.Single CameraSize{  get { return UnityCamera.CameraSize; }
  set{UnityCamera.CameraSize = value; }
 }
	public System.Boolean Quit{  set{UnityCamera.Quit = value; }
 }
	public UnityCamera UnityCamera;
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
	public System.Single ___adjustment10;
	public System.Single ___adjustment11;
	public System.Single ___adjustment12;
	public System.Single ___adjustment13;
	public System.Single ___sensitivity00;
	public System.Single ___sensitivity11;
	public void Update(float dt, World world) {
frame = World.frame;

		this.Rule0(dt, world);
		this.Rule1(dt, world);
		this.Rule2(dt, world);
	}



	int s000=-1;
	public void parallelMethod000(float dt, World world){ 
	switch (s000)
	{

	case -1:
	if(!(((UnityEngine.Input.GetKey(KeyCode.DownArrow)) && (!(((CameraSize) > (75f)))))))
	{

	s000 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	CameraSize = System.Math.Min(75f,(CameraSize) + (___sensitivity00));
	s000 = -1;
return;	
	default: return;}}
	

	int s001=-1;
	public void parallelMethod001(float dt, World world){ 
	switch (s001)
	{

	case -1:
	if(!(((UnityEngine.Input.GetKey(KeyCode.UpArrow)) && (((((CameraSize) > (5f))) || (((CameraSize) == (5f))))))))
	{

	s001 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	CameraSize = System.Math.Max(5f,(CameraSize) - (___sensitivity00));
	s001 = -1;
return;	
	default: return;}}
	

	int s110=-1;
	public void parallelMethod110(float dt, World world){ 
	switch (s110)
	{

	case -1:
	if(!(((UnityEngine.Input.GetKey(KeyCode.A)) && (((CameraPosition.x) > (-100f))))))
	{

	s110 = -1;
return;	}else
	{

	goto case 1;	}
	case 1:
	___adjustment10 = System.Math.Max(-100f,(___sensitivity11) * (-1f));
	CameraPosition = ((CameraPosition) + (new UnityEngine.Vector3(___adjustment10,0f,0f)));
	s110 = -1;
return;	
	default: return;}}
	

	int s111=-1;
	public void parallelMethod111(float dt, World world){ 
	switch (s111)
	{

	case -1:
	if(!(((UnityEngine.Input.GetKey(KeyCode.D)) && (((100f) > (CameraPosition.x))))))
	{

	s111 = -1;
return;	}else
	{

	goto case 1;	}
	case 1:
	___adjustment11 = System.Math.Min(100f,___sensitivity11);
	CameraPosition = ((CameraPosition) + (new UnityEngine.Vector3(___sensitivity11,0f,0f)));
	s111 = -1;
return;	
	default: return;}}
	

	int s112=-1;
	public void parallelMethod112(float dt, World world){ 
	switch (s112)
	{

	case -1:
	if(!(((UnityEngine.Input.GetKey(KeyCode.S)) && (((CameraPosition.z) > (-100f))))))
	{

	s112 = -1;
return;	}else
	{

	goto case 1;	}
	case 1:
	___adjustment12 = System.Math.Max(-100f,(___sensitivity11) * (-1f));
	CameraPosition = ((CameraPosition) + (new UnityEngine.Vector3(0f,0f,___adjustment12)));
	s112 = -1;
return;	
	default: return;}}
	

	int s113=-1;
	public void parallelMethod113(float dt, World world){ 
	switch (s113)
	{

	case -1:
	if(!(((UnityEngine.Input.GetKey(KeyCode.W)) && (((100f) > (CameraPosition.z))))))
	{

	s113 = -1;
return;	}else
	{

	goto case 1;	}
	case 1:
	___adjustment13 = System.Math.Min(100f,___sensitivity11);
	CameraPosition = ((CameraPosition) + (new UnityEngine.Vector3(0f,0f,___adjustment13)));
	s113 = -1;
return;	
	default: return;}}
	

	int s0=-1;
	public void Rule0(float dt, World world){ 
	switch (s0)
	{

	case -1:
	___sensitivity00 = 0.5f;
	goto case 0;
	case 0:
	this.parallelMethod000(dt,world);
	this.parallelMethod001(dt,world);
	s0 = 0;
return;	
	default: return;}}
	

	int s1=-1;
	public void Rule1(float dt, World world){ 
	switch (s1)
	{

	case -1:
	___sensitivity11 = 1f;
	goto case 0;
	case 0:
	this.parallelMethod110(dt,world);
	this.parallelMethod111(dt,world);
	this.parallelMethod112(dt,world);
	this.parallelMethod113(dt,world);
	s1 = 0;
return;	
	default: return;}}
	

	int s2=-1;
	public void Rule2(float dt, World world){ 
	switch (s2)
	{

	case -1:
	if(!(UnityEngine.Input.GetKey(KeyCode.Escape)))
	{

	s2 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	Quit = true;
	s2 = -1;
return;	
	default: return;}}
	





}
public class Planet{
public int frame;
public bool JustEntered = true;
private UnityEngine.Vector3 pos;
private System.Single m;
private UnityEngine.Vector3 velocity;
private UnityEngine.Vector3 rotationVelocity;
	public int ID;
public Planet(UnityEngine.Vector3 pos, System.Single m, UnityEngine.Vector3 velocity, UnityEngine.Vector3 rotationVelocity)
	{JustEntered = false;		Velocity = velocity;
		UnityPlanet = UnityPlanet.Instantiate(pos,m);
		RotationVelocity = rotationVelocity;
		OutOfBounds = false;
		Mass = m;
		Acceleration = Vector3.zero;
		}
		public UnityEngine.Vector3 Acceleration;
	public System.Boolean Destroyed{  get { return UnityPlanet.Destroyed; }
  set{UnityPlanet.Destroyed = value; }
 }
	public System.Single Mass;
	public System.Boolean OutOfBounds;
	public UnityEngine.Vector3 Position{  get { return UnityPlanet.Position; }
  set{UnityPlanet.Position = value; }
 }
	public UnityEngine.Quaternion Rotation{  get { return UnityPlanet.Rotation; }
  set{UnityPlanet.Rotation = value; }
 }
	public UnityEngine.Vector3 RotationVelocity;
	public UnityPlanet UnityPlanet;
	public UnityEngine.Vector3 Velocity;
	public UnityEngine.Animation animation{  get { return UnityPlanet.animation; }
 }
	public UnityEngine.AudioSource audio{  get { return UnityPlanet.audio; }
 }
	public UnityEngine.Camera camera{  get { return UnityPlanet.camera; }
 }
	public UnityEngine.Collider collider{  get { return UnityPlanet.collider; }
 }
	public UnityEngine.Collider2D collider2D{  get { return UnityPlanet.collider2D; }
 }
	public UnityEngine.ConstantForce constantForce{  get { return UnityPlanet.constantForce; }
 }
	public System.Boolean enabled{  get { return UnityPlanet.enabled; }
  set{UnityPlanet.enabled = value; }
 }
	public UnityEngine.GameObject gameObject{  get { return UnityPlanet.gameObject; }
 }
	public UnityEngine.GUIElement guiElement{  get { return UnityPlanet.guiElement; }
 }
	public UnityEngine.GUIText guiText{  get { return UnityPlanet.guiText; }
 }
	public UnityEngine.GUITexture guiTexture{  get { return UnityPlanet.guiTexture; }
 }
	public UnityEngine.HideFlags hideFlags{  get { return UnityPlanet.hideFlags; }
  set{UnityPlanet.hideFlags = value; }
 }
	public UnityEngine.HingeJoint hingeJoint{  get { return UnityPlanet.hingeJoint; }
 }
	public UnityEngine.Light light{  get { return UnityPlanet.light; }
 }
	public System.String name{  get { return UnityPlanet.name; }
  set{UnityPlanet.name = value; }
 }
	public UnityEngine.ParticleEmitter particleEmitter{  get { return UnityPlanet.particleEmitter; }
 }
	public UnityEngine.ParticleSystem particleSystem{  get { return UnityPlanet.particleSystem; }
 }
	public UnityEngine.Renderer renderer{  get { return UnityPlanet.renderer; }
 }
	public UnityEngine.Rigidbody rigidbody{  get { return UnityPlanet.rigidbody; }
 }
	public UnityEngine.Rigidbody2D rigidbody2D{  get { return UnityPlanet.rigidbody2D; }
 }
	public System.String tag{  get { return UnityPlanet.tag; }
  set{UnityPlanet.tag = value; }
 }
	public UnityEngine.Transform transform{  get { return UnityPlanet.transform; }
 }
	public System.Boolean useGUILayout{  get { return UnityPlanet.useGUILayout; }
  set{UnityPlanet.useGUILayout = value; }
 }
	public void Update(float dt, World world) {
frame = World.frame;		this.Rule0(dt, world);
		this.Rule2(dt, world);
		this.Rule3(dt, world);
		this.Rule4(dt, world);
		this.Rule5(dt, world);
		this.Rule1(dt, world);

	}

	public void Rule0(float dt, World world) 
	{
	System.Single ___g00;
	___g00 = (6.673f) * (UnityEngine.Mathf.Pow(10f,-3f));
	List<UnityEngine.Vector3> ___accelerations00;
	___accelerations00 = (

(world.Planets).Select(__ContextSymbol8 => new { ___planet00 = __ContextSymbol8 })
.Where(__ContextSymbol9 => !(((__ContextSymbol9.___planet00) == (this))))
.Select(__ContextSymbol10 => new {___r00 = UnityEngine.Vector3.Distance(this.Position,__ContextSymbol10.___planet00.Position), prev = __ContextSymbol10 })
.Select(__ContextSymbol11 => new {___acc00 = ((___g00) * (__ContextSymbol11.prev.___planet00.Mass)) / ((__ContextSymbol11.___r00) * (__ContextSymbol11.___r00)), prev = __ContextSymbol11 })
.Select(__ContextSymbol12 => (UnityEngine.Vector3.Normalize((__ContextSymbol12.prev.prev.___planet00.Position) - (this.Position))) * (__ContextSymbol12.___acc00))
.ToList<UnityEngine.Vector3>()).ToList<UnityEngine.Vector3>();
	if(((___accelerations00.Count) > (0)))
		{
		Acceleration = (

(___accelerations00).Select(__ContextSymbol13 => new { ___a00 = __ContextSymbol13 })
.Select(__ContextSymbol14 => __ContextSymbol14.___a00)
.Aggregate( (acc, __x) => acc + __x));
		}else
		{
		Acceleration = Vector3.zero;
		}
	}
	

	public void Rule2(float dt, World world) 
	{
	OutOfBounds = ((((((((Position.x) > (250f))) || (((Position.z) > (250f))))) || (((-250f) > (Position.x))))) || (((-250f) > (Position.z))));
	}
	

	public void Rule3(float dt, World world) 
	{
	Rotation = (UnityEngine.Quaternion.Euler((RotationVelocity) * (dt))) * (Rotation);
	}
	

	public void Rule4(float dt, World world) 
	{
	Velocity = (Velocity) + ((Acceleration) * (dt));
	}
	

	public void Rule5(float dt, World world) 
	{
	Position = (Position) + ((Velocity) * (dt));
	}
	



	int s1=-1;
	public void Rule1(float dt, World world){ 
	switch (s1)
	{

	case -1:
	if(!(OutOfBounds))
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
	






}
                 