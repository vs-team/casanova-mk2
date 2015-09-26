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
		Factor = 0f;
		
}
		public UnityEngine.Color Color{  set{UnityCube.Color = value; }
 }
	public System.Single Factor;
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
	public System.Single count_down1;

System.DateTime init_time = System.DateTime.Now;
	public void Update(float dt, World world) {
var t = System.DateTime.Now;		this.Rule0(dt, world);

		this.Rule1(dt, world);

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
	if(!(UnityEngine.Input.GetKeyDown(KeyCode.Space)))
	{

	s1 = -1;
return;	}else
	{

	goto case 6;	}
	case 6:
	if(!(((1f) > (Factor))))
	{

	goto case 5;	}else
	{

	goto case 7;	}
	case 7:
	Factor = ((Factor) + (dt));
	s1 = 6;
return;
	case 5:
	if(!(UnityEngine.Input.GetKeyDown(KeyCode.Space)))
	{

	s1 = 5;
return;	}else
	{

	goto case 2;	}
	case 2:
	if(!(((Factor) > (0f))))
	{

	goto case 0;	}else
	{

	goto case 3;	}
	case 3:
	Factor = ((Factor) - (dt));
	s1 = 2;
return;
	case 0:
	count_down1 = 1f;
	goto case 1;
	case 1:
	if(((count_down1) > (0f)))
	{

	count_down1 = ((count_down1) - (dt));
	s1 = 1;
return;	}else
	{

	s1 = -1;
return;	}	
	default: return;}}
	






}
}    