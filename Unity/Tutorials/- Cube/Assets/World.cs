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
		
}
		public Ship Ship;

System.DateTime init_time = System.DateTime.Now;
	public void Update(float dt, World world) {
var t = System.DateTime.Now;

		Ship.Update(dt, world);


	}











}
public class Ship{
public int frame;
public bool JustEntered = true;
	public int ID;
public Ship()
	{JustEntered = false;
 frame = World.frame;
		Velocity = new UnityEngine.Vector3(0f,0f,2f);
		UnityShip = UnityShip.Find();
		
}
		public UnityEngine.Vector3 Position{  get { return UnityShip.Position; }
  set{UnityShip.Position = value; }
 }
	public UnityShip UnityShip;
	public UnityEngine.Vector3 Velocity;
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
frame = World.frame;		this.Rule0(dt, world);



	}

	public void Rule0(float dt, World world) 
	{
	Position = (Position) + ((Velocity) * (dt));
	}
	










}
}          