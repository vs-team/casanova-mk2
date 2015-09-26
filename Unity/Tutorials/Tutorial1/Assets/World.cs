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
		UnityCube = UnityCube.Find();
		B = 0;
		A = 1;
		
}
		public System.Int32 A;
	public System.Int32 B;
	public UnityEngine.Vector3 Position{  get { return UnityCube.Position; }
  set{UnityCube.Position = value; }
 }
	public UnityCube UnityCube;
	public System.Boolean enabled{  get { return UnityCube.enabled; }
  set{UnityCube.enabled = value; }
 }
	public UnityEngine.GameObject gameObject{  get { return UnityCube.gameObject; }
 }
	public UnityEngine.HideFlags hideFlags{  get { return UnityCube.hideFlags; }
  set{UnityCube.hideFlags = value; }
 }
	public System.Boolean isActiveAndEnabled{  get { return UnityCube.isActiveAndEnabled; }
 }
	public System.String name{  get { return UnityCube.name; }
  set{UnityCube.name = value; }
 }
	public System.String tag{  get { return UnityCube.tag; }
  set{UnityCube.tag = value; }
 }
	public UnityEngine.Transform transform{  get { return UnityCube.transform; }
 }
	public System.Boolean useGUILayout{  get { return UnityCube.useGUILayout; }
  set{UnityCube.useGUILayout = value; }
 }

System.DateTime init_time = System.DateTime.Now;
	public void Update(float dt, World world) {
var t = System.DateTime.Now;		this.Rule0(dt, world);



	}

	public void Rule0(float dt, World world) 
	{
	Position = (Position) + (new UnityEngine.Vector3(0.1f,0f,0f));
	}
	










}
}     