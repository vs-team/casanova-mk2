#pragma warning disable 162,108,618
using Casanova.Prelude;
using System.Linq;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
namespace Pippo {public class World{
public static int frame;

public bool JustEntered = true;


public void Start()
	{
		Cnt = Vector2.One;
		
}
		public Microsoft.Xna.Framework.Vector2 Cnt;

System.DateTime init_time = System.DateTime.Now;
	public void Update(float dt, World world) {
var t = System.DateTime.Now;		this.Rule0(dt, world);
		this.Rule1(dt, world);


	}

	public void Rule0(float dt, World world) 
	{
	Cnt = (Cnt) + (Vector2.One);
	}
	

	public void Rule1(float dt, World world) 
	{
	Cnt = (Cnt) + (Vector2.One);
	}
	









}
}