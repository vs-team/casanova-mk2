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
		Cubes = (

Enumerable.Empty<Cube>()).ToList<Cube>();
		
}
		public List<Cube> __Cubes;
	public List<Cube> Cubes{  get { return  __Cubes; }
  set{ __Cubes = value;
 foreach(var e in value){if(e.JustEntered){ e.JustEntered = false;
}
} }
 }

System.DateTime init_time = System.DateTime.Now;
	public void Update(float dt, World world) {
var t = System.DateTime.Now;		this.Rule0(dt, world);

		for(int x0 = 0; x0 < Cubes.Count; x0++) { 
			Cubes[x0].Update(dt, world);
		}
		this.Rule1(dt, world);

	}

	public void Rule0(float dt, World world) 
	{
	Cubes = (

(Cubes).Select(__ContextSymbol1 => new { ___c00 = __ContextSymbol1 })
.Where(__ContextSymbol2 => ((__ContextSymbol2.___c00.UnityCube.Destroyed) == (false)))
.Select(__ContextSymbol3 => __ContextSymbol3.___c00)
.ToList<Cube>()).ToList<Cube>();
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
	Cubes = new Cons<Cube>(new Cube(), (Cubes)).ToList<Cube>();
	s1 = -1;
return;	
	default: return;}}
	






}
public class Cube{
public int frame;
public bool JustEntered = true;
	public int ID;
public Cube()
	{JustEntered = false;
 frame = World.frame;
		UnityCube = UnityCube.Instantiate();
		Factor = 0f;
		
}
		public UnityEngine.Color Color{  set{UnityCube.Color = value; }
 }
	public System.Boolean Destroyed{  get { return UnityCube.Destroyed; }
  set{UnityCube.Destroyed = value; }
 }
	public System.Single Factor;
	public System.Single Scale{  get { return UnityCube.Scale; }
  set{UnityCube.Scale = value; }
 }
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
	public void Update(float dt, World world) {
frame = World.frame;		this.Rule0(dt, world);

		this.Rule1(dt, world);
		this.Rule2(dt, world);
		this.Rule3(dt, world);
	}

	public void Rule0(float dt, World world) 
	{
	Color = Color.Lerp(Color.white,Color.blue,Factor);
	}
	




	int s1=-1;
	public void Rule1(float dt, World world){ 
	switch (s1)
	{

	case -1:
	if(!(((((Factor) > (2f))) || (((Factor) == (2f))))))
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
	Scale = ((1f) - (((Factor) / (2f))));
	s2 = -1;
return;	
	default: return;}}
	

	int s3=-1;
	public void Rule3(float dt, World world){ 
	switch (s3)
	{

	case -1:
	Factor = ((Factor) + (0.02f));
	s3 = -1;
return;	
	default: return;}}
	





}
}    