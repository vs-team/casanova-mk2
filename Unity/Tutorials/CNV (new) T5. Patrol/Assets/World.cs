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
		UnityBob = UnityBob.Find();
		
}
		public System.Collections.Generic.List<UnityEngine.Vector3> Checkpoints{  get { return UnityBob.Checkpoints; }
 }
	public BobAnimation CurrentAnimation{  set{UnityBob.CurrentAnimation = value; }
 }
	public UnityEngine.Vector3 Position{  get { return UnityBob.Position; }
 }
	public System.Boolean Quit{  set{UnityBob.Quit = value; }
 }
	public UnityBob UnityBob;
	public UnityEngine.Vector3 Velocity{  get { return UnityBob.Velocity; }
  set{UnityBob.Velocity = value; }
 }
	public UnityEngine.Animation animation{  get { return UnityBob.animation; }
 }
	public UnityEngine.AudioSource audio{  get { return UnityBob.audio; }
 }
	public UnityEngine.Camera camera{  get { return UnityBob.camera; }
 }
	public UnityEngine.Collider collider{  get { return UnityBob.collider; }
 }
	public UnityEngine.Collider2D collider2D{  get { return UnityBob.collider2D; }
 }
	public UnityEngine.ConstantForce constantForce{  get { return UnityBob.constantForce; }
 }
	public System.Boolean enabled{  get { return UnityBob.enabled; }
  set{UnityBob.enabled = value; }
 }
	public UnityEngine.GameObject gameObject{  get { return UnityBob.gameObject; }
 }
	public UnityEngine.GUIElement guiElement{  get { return UnityBob.guiElement; }
 }
	public UnityEngine.GUIText guiText{  get { return UnityBob.guiText; }
 }
	public UnityEngine.GUITexture guiTexture{  get { return UnityBob.guiTexture; }
 }
	public UnityEngine.HideFlags hideFlags{  get { return UnityBob.hideFlags; }
  set{UnityBob.hideFlags = value; }
 }
	public UnityEngine.HingeJoint hingeJoint{  get { return UnityBob.hingeJoint; }
 }
	public UnityEngine.Light light{  get { return UnityBob.light; }
 }
	public System.String name{  get { return UnityBob.name; }
  set{UnityBob.name = value; }
 }
	public UnityEngine.ParticleEmitter particleEmitter{  get { return UnityBob.particleEmitter; }
 }
	public UnityEngine.ParticleSystem particleSystem{  get { return UnityBob.particleSystem; }
 }
	public UnityEngine.Renderer renderer{  get { return UnityBob.renderer; }
 }
	public UnityEngine.Rigidbody rigidbody{  get { return UnityBob.rigidbody; }
 }
	public UnityEngine.Rigidbody2D rigidbody2D{  get { return UnityBob.rigidbody2D; }
 }
	public System.String tag{  get { return UnityBob.tag; }
  set{UnityBob.tag = value; }
 }
	public UnityEngine.Transform transform{  get { return UnityBob.transform; }
 }
	public System.Boolean useGUILayout{  get { return UnityBob.useGUILayout; }
  set{UnityBob.useGUILayout = value; }
 }
	public UnityEngine.Vector3 ___c00;
	public System.Int32 counter1;
	public UnityEngine.Vector3 ___dir000;
	public System.Single count_down1;
	public void Update(float dt, World world) {
var t = System.DateTime.Now;

		this.Rule0(dt, world);

	}




	int s0=-1;
	public void Rule0(float dt, World world){ 
	switch (s0)
	{

	case -1:
	
	counter1 = -1;
	if((((Checkpoints).Count) == (0)))
	{

	s0 = -1;
return;	}else
	{

	___c00 = (Checkpoints)[0];
	goto case 1;	}
	case 1:
	counter1 = ((counter1) + (1));
	if((((((Checkpoints).Count) == (counter1))) || (((counter1) > ((Checkpoints).Count)))))
	{

	s0 = -1;
return;	}else
	{

	___c00 = (Checkpoints)[counter1];
	goto case 2;	}
	case 2:
	___dir000 = ((___c00) - (Position));
	Velocity = ___dir000;
	CurrentAnimation = BobAnimation.Walk;
	s0 = 6;
return;
	case 6:
	if(!(((0f) > (UnityEngine.Vector3.Dot(___dir000,(___c00) - (Position))))))
	{

	s0 = 6;
return;	}else
	{

	goto case 5;	}
	case 5:
	Velocity = Vector3.zero;
	CurrentAnimation = BobAnimation.Idle;
	s0 = 3;
return;
	case 3:
	count_down1 = 1f;
	goto case 4;
	case 4:
	if(((count_down1) > (0f)))
	{

	count_down1 = ((count_down1) - (dt));
	s0 = 4;
return;	}else
	{

	s0 = 1;
return;	}	
	default: return;}}
	






}
        