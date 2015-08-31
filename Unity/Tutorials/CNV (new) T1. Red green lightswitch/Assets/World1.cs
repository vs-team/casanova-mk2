#pragma warning disable 162,108,618
using Casanova.Prelude;
using System.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace Game1 {public class World1 : MonoBehaviour{
  void OnApplicationQuit() { file.Close(); }public static int frame;
void Update () { Update(Time.deltaTime, this); 
 frame++; }
public bool JustEntered = true;

System.IO.StreamWriter file;
public void Start()
	{
file = new System.IO.StreamWriter(System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(),"test.csv"));		myA = new A();
		UnityCube = UnityCube.Find();
		
}
		public UnityEngine.Color Color{  get { return UnityCube.Color; }
  set{UnityCube.Color = value; }
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
	public System.Collections.Generic.List<System.Int32> lst{  get { return UnityCube.lst; }
  set{UnityCube.lst = value; }
 }
	public A myA;
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
	public System.Single count_down2;
	public System.Single count_down1;

System.DateTime init_time = System.DateTime.Now;
	public void Update(float dt, World1 world) {
var t = System.DateTime.Now;

		myA.Update(dt, world);
		this.Rule0(dt, world);
		this.Rule1(dt, world);
var t1 = System.DateTime.Now;
file.WriteLine((t1 - t).Milliseconds + "," + (t1 - init_time).Seconds);	}





	int s0=-1;
	public void Rule0(float dt, World1 world){ 
	switch (s0)
	{

	case -1:
	count_down2 = 0.5f;
	goto case 5;
	case 5:
	if(((count_down2) > (0f)))
	{

	count_down2 = ((count_down2) - (dt));
	s0 = 5;
return;	}else
	{

	goto case 3;	}
	case 3:
	UnityCube.Color = Color.green;
	s0 = 1;
return;
	case 1:
	count_down1 = 0.5f;
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
	UnityCube.Color = Color.red;
	s0 = -1;
return;	
	default: return;}}
	

	int s1=-1;
	public void Rule1(float dt, World1 world){ 
	switch (s1)
	{

	case -1:
	if(((myA.B.C.Elem) > (0)))
	{

	goto case 6;	}else
	{

	goto case 7;	}
	case 6:
	myA = myA;
	s1 = -1;
return;
	case 7:
	myA = new A();
	s1 = -1;
return;	
	default: return;}}
	





}
public class A{
public int frame;
public bool JustEntered = true;
	public int ID;
public A()
	{JustEntered = false;
 frame = World1.frame;
		B = new B();
		
}
		public B B;
	public void Update(float dt, World1 world) {
frame = World1.frame;

		B.Update(dt, world);
		this.Rule0(dt, world);

	}





	int s0=-1;
	public void Rule0(float dt, World1 world){ 
	switch (s0)
	{

	case -1:
	if(((B.C.Elem) > (0)))
	{

	goto case 11;	}else
	{

	goto case 12;	}
	case 11:
	B = B;
	s0 = -1;
return;
	case 12:
	B = new B();
	s0 = -1;
return;	
	default: return;}}
	






}
public class B{
public int frame;
public bool JustEntered = true;
	public int ID;
public B()
	{JustEntered = false;
 frame = World1.frame;
		C = new D();
		
}
		public D C;
	public void Update(float dt, World1 world) {
frame = World1.frame;

		C.Update(dt, world);


	}











}
public class D{
public int frame;
public bool JustEntered = true;
	public int ID;
public D()
	{JustEntered = false;
 frame = World1.frame;
		Elem = 1;
		
}
		public System.Int32 Elem;
	public void Update(float dt, World1 world) {
frame = World1.frame;



	}











}
}    