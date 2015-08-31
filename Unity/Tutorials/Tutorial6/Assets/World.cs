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
	{		Velocity = new UnityEngine.Vector3(0f,0f,0f);
		UnityBall = UnityBall.Find();
		Acceleration = new UnityEngine.Vector3(0f,0f,0f);
		}
		public UnityEngine.Vector3 Acceleration;
	public UnityEngine.Vector3 Position{  get { return UnityBall.Position; }
  set{UnityBall.Position = value; }
 }
	public UnityBall UnityBall;
	public UnityEngine.Vector3 Velocity;
	public UnityEngine.Animation animation{  get { return UnityBall.animation; }
 }
	public UnityEngine.AudioSource audio{  get { return UnityBall.audio; }
 }
	public UnityEngine.Camera camera{  get { return UnityBall.camera; }
 }
	public UnityEngine.Collider collider{  get { return UnityBall.collider; }
 }
	public UnityEngine.Collider2D collider2D{  get { return UnityBall.collider2D; }
 }
	public UnityEngine.ConstantForce constantForce{  get { return UnityBall.constantForce; }
 }
	public System.Boolean enabled{  get { return UnityBall.enabled; }
  set{UnityBall.enabled = value; }
 }
	public UnityEngine.GameObject gameObject{  get { return UnityBall.gameObject; }
 }
	public UnityEngine.GUIElement guiElement{  get { return UnityBall.guiElement; }
 }
	public UnityEngine.GUIText guiText{  get { return UnityBall.guiText; }
 }
	public UnityEngine.GUITexture guiTexture{  get { return UnityBall.guiTexture; }
 }
	public UnityEngine.HideFlags hideFlags{  get { return UnityBall.hideFlags; }
  set{UnityBall.hideFlags = value; }
 }
	public UnityEngine.HingeJoint hingeJoint{  get { return UnityBall.hingeJoint; }
 }
	public UnityEngine.Light light{  get { return UnityBall.light; }
 }
	public System.String name{  get { return UnityBall.name; }
  set{UnityBall.name = value; }
 }
	public UnityEngine.ParticleEmitter particleEmitter{  get { return UnityBall.particleEmitter; }
 }
	public UnityEngine.ParticleSystem particleSystem{  get { return UnityBall.particleSystem; }
 }
	public UnityEngine.Renderer renderer{  get { return UnityBall.renderer; }
 }
	public UnityEngine.Rigidbody rigidbody{  get { return UnityBall.rigidbody; }
 }
	public UnityEngine.Rigidbody2D rigidbody2D{  get { return UnityBall.rigidbody2D; }
 }
	public System.String tag{  get { return UnityBall.tag; }
  set{UnityBall.tag = value; }
 }
	public UnityEngine.Transform transform{  get { return UnityBall.transform; }
 }
	public System.Boolean useGUILayout{  get { return UnityBall.useGUILayout; }
  set{UnityBall.useGUILayout = value; }
 }
	public void Update(float dt, World world) {
		this.Rule1(dt, world);
		this.Rule2(dt, world);
		this.Rule0(dt, world);

	}
	public void Rule1(float dt, World world) 
	{
	Velocity = (Velocity) + ((Acceleration) * (dt));
	}
	

	public void Rule2(float dt, World world) 
	{
	Position = (Position) + ((Velocity) * (dt));
	}
	

	int s000=-1;
	public void parallelMethod000(float dt, World world){ 
	switch (s000)
	{

	case -1:
	if(!(UnityEngine.Input.GetKey(KeyCode.W)))
	{

	s000 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	Acceleration = ((Acceleration) + (((new UnityEngine.Vector3(0f,0f,1f)) * (dt))));
	s000 = -1;
return;	
	default: return;}}
	

	int s001=-1;
	public void parallelMethod001(float dt, World world){ 
	switch (s001)
	{

	case -1:
	if(!(UnityEngine.Input.GetKey(KeyCode.D)))
	{

	s001 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	Acceleration = ((Acceleration) + (((new UnityEngine.Vector3(1f,0f,0f)) * (dt))));
	s001 = -1;
return;	
	default: return;}}
	

	int s002=-1;
	public void parallelMethod002(float dt, World world){ 
	switch (s002)
	{

	case -1:
	if(!(UnityEngine.Input.GetKey(KeyCode.S)))
	{

	s002 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	Acceleration = ((Acceleration) + (((new UnityEngine.Vector3(0f,0f,-1f)) * (dt))));
	s002 = -1;
return;	
	default: return;}}
	

	int s003=-1;
	public void parallelMethod003(float dt, World world){ 
	switch (s003)
	{

	case -1:
	if(!(UnityEngine.Input.GetKey(KeyCode.A)))
	{

	s003 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	Acceleration = ((Acceleration) + (((new UnityEngine.Vector3(-1f,0f,0f)) * (dt))));
	s003 = -1;
return;	
	default: return;}}
	

	int s0=-1;
	public void Rule0(float dt, World world){ 
	switch (s0)
	{

	case -1:
	this.parallelMethod000(dt,world);
	this.parallelMethod001(dt,world);
	this.parallelMethod002(dt,world);
	this.parallelMethod003(dt,world);
	s0 = -1;
return;	
	default: return;}}
	






}
  