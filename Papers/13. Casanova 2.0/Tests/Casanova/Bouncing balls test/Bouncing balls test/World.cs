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
	{		Balls = (

(Enumerable.Range(1,(1) + ((10000) - (1))).ToList<System.Int32>()).Select(__ContextSymbol0 => new { ___i00 = __ContextSymbol0 })
.Select(__ContextSymbol1 => new Ball())
.ToList<Ball>()).ToList<Ball>();
		}
		public List<Ball> Balls;
	public void Update(float dt, World world) {
		for(int x0 = 0; x0 < Balls.Count; x0++) { 
			Balls[x0].Update(dt, world);
		}




	}










}
public class Ball{
public int frame;
public bool JustEntered = true;
	public int ID;
public Ball()
	{JustEntered = false;		V = new UnityEngine.Vector2(0.1f,0f);
		P = new UnityEngine.Vector2(0f,0f);
		}
		public UnityEngine.Vector2 P;
	public UnityEngine.Vector2 V;
	public void Update(float dt, World world) {
frame = World.frame;		this.Rule1(dt, world);

		this.Rule0(dt, world);

	}

	public void Rule1(float dt, World world) 
	{
	P = (P) + (V);
	}
	




	int s0=-1;
	public void Rule0(float dt, World world){ 
	switch (s0)
	{

	case -1:
	if(!(((((P.x) > (500f))) || (((P.x) == (500f))))))
	{

	s0 = -1;
return;	}else
	{

	goto case 2;	}
	case 2:
	V = ((V) * (-1f));
	s0 = 1;
return;
	case 1:
	if(!(!(((P.x) > (-500f)))))
	{

	s0 = 1;
return;	}else
	{

	goto case 0;	}
	case 0:
	V = ((V) * (-1f));
	s0 = -1;
return;	
	default: return;}}
	






}
    