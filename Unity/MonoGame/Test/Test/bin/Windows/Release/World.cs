#pragma warning disable 162,108,618
using Casanova.Prelude;
using System.Linq;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
namespace AsteroidShooter {public class World{
public static int frame;

public bool JustEntered = true;


public void Start()
	{
		State = Microsoft.Xna.Framework.Input.Keyboard.GetState();
		Ship = new Ship();
		RandomGenerator = new System.Random();
		Asteroids = (

Enumerable.Empty<Asteroid>()).ToList<Asteroid>();
		
}
		public List<Asteroid> Asteroids;
	public System.Random RandomGenerator;
	public Ship Ship;
	public Microsoft.Xna.Framework.Input.KeyboardState State;
	public System.Single count_down1;

System.DateTime init_time = System.DateTime.Now;
	public void Update(float dt, World world) {
var t = System.DateTime.Now;		this.Rule0(dt, world);
		this.Rule2(dt, world);
		for(int x0 = 0; x0 < Asteroids.Count; x0++) { 
			Asteroids[x0].Update(dt, world);
		}
		Ship.Update(dt, world);
		this.Rule1(dt, world);

	}

	public void Rule0(float dt, World world) 
	{
	Asteroids = (

(Asteroids).Select(__ContextSymbol1 => new { ___a00 = __ContextSymbol1 })
.Where(__ContextSymbol2 => ((800f) > (__ContextSymbol2.___a00.Position.Y)))
.Select(__ContextSymbol3 => __ContextSymbol3.___a00)
.ToList<Asteroid>()).ToList<Asteroid>();
	}
	

	public void Rule2(float dt, World world) 
	{
	State = Microsoft.Xna.Framework.Input.Keyboard.GetState();
	}
	



	int s1=-1;
	public void Rule1(float dt, World world){ 
	switch (s1)
	{

	case -1:
	Asteroids = new Cons<Asteroid>(new Asteroid(new Microsoft.Xna.Framework.Vector2(250f,-50f)), (Asteroids)).ToList<Asteroid>();
	s1 = 0;
return;
	case 0:
	count_down1 = 3f;
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
public class Ship{
public int frame;
public bool JustEntered = true;
	public int ID;
public Ship()
	{JustEntered = false;
 frame = World.frame;
		Projectiles = (

Enumerable.Empty<Projectile>()).ToList<Projectile>();
		Position = new Microsoft.Xna.Framework.Vector2(750f,1000f);
		
}
		public Microsoft.Xna.Framework.Vector2 Position;
	public List<Projectile> Projectiles;
	public Microsoft.Xna.Framework.Vector2 ___vy20;
	public Microsoft.Xna.Framework.Vector2 ___vy31;
	public void Update(float dt, World world) {
frame = World.frame;		this.Rule0(dt, world);

		this.Rule1(dt, world);
		this.Rule2(dt, world);
		this.Rule3(dt, world);
		for(int x0 = 0; x0 < Projectiles.Count; x0++) { 
			Projectiles[x0].Update(dt, world);
		}
	}

	public void Rule0(float dt, World world) 
	{
	Projectiles = (

(Projectiles).Select(__ContextSymbol6 => new { ___p00 = __ContextSymbol6 })
.Where(__ContextSymbol7 => ((__ContextSymbol7.___p00.Position.Y) > (-50f)))
.Select(__ContextSymbol8 => __ContextSymbol8.___p00)
.ToList<Projectile>()).ToList<Projectile>();
	}
	




	int s1=-1;
	public void Rule1(float dt, World world){ 
	switch (s1)
	{

	case -1:
	if(!(world.State.IsKeyDown(Keys.Space)))
	{

	s1 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	Projectiles = new Cons<Projectile>(new Projectile(Position), (Projectiles)).ToList<Projectile>();
	s1 = -1;
return;	
	default: return;}}
	

	int s2=-1;
	public void Rule2(float dt, World world){ 
	switch (s2)
	{

	case -1:
	___vy20 = new Microsoft.Xna.Framework.Vector2(300f,0f);
	goto case 1;
	case 1:
	if(!(world.State.IsKeyDown(Keys.D)))
	{

	s2 = 1;
return;	}else
	{

	goto case 0;	}
	case 0:
	Position = ((Position) + (((___vy20) * (dt))));
	s2 = -1;
return;	
	default: return;}}
	

	int s3=-1;
	public void Rule3(float dt, World world){ 
	switch (s3)
	{

	case -1:
	___vy31 = new Microsoft.Xna.Framework.Vector2(-300f,0f);
	goto case 1;
	case 1:
	if(!(world.State.IsKeyDown(Keys.A)))
	{

	s3 = 1;
return;	}else
	{

	goto case 0;	}
	case 0:
	Position = ((Position) + (((___vy31) * (dt))));
	s3 = -1;
return;	
	default: return;}}
	





}
public class Projectile{
public int frame;
public bool JustEntered = true;
private Microsoft.Xna.Framework.Vector2 p;
	public int ID;
public Projectile(Microsoft.Xna.Framework.Vector2 p)
	{JustEntered = false;
 frame = World.frame;
		Position = p;
		
}
		public Microsoft.Xna.Framework.Vector2 Position;
	public void Update(float dt, World world) {
frame = World.frame;		this.Rule0(dt, world);



	}

	public void Rule0(float dt, World world) 
	{
	Position = (Position) + ((new Microsoft.Xna.Framework.Vector2(0f,-500f)) * (dt));
	}
	










}
public class Asteroid{
public int frame;
public bool JustEntered = true;
private Microsoft.Xna.Framework.Vector2 p;
	public int ID;
public Asteroid(Microsoft.Xna.Framework.Vector2 p)
	{JustEntered = false;
 frame = World.frame;
		Position = p;
		
}
		public Microsoft.Xna.Framework.Vector2 Position;
	public void Update(float dt, World world) {
frame = World.frame;		this.Rule0(dt, world);



	}

	public void Rule0(float dt, World world) 
	{
	Position = (Position) + ((new Microsoft.Xna.Framework.Vector2(0f,250f)) * (dt));
	}
	










}
}