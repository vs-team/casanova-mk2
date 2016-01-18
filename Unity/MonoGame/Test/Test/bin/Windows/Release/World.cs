#pragma warning disable 162,108,618
using Casanova.Prelude;
using System.Linq;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using Utilities;
namespace AsteroidShooter {public class World{
public static int frame;

public bool JustEntered = true;


public void Start()
	{
		State = Microsoft.Xna.Framework.Input.Keyboard.GetState();
		Ships = (

(new Cons<Ship>(new Ship(new Microsoft.Xna.Framework.Vector2(250f,1000f)),(new Cons<Ship>(new Ship(new Microsoft.Xna.Framework.Vector2(750f,1000f)),(new Empty<Ship>()).ToList<Ship>())).ToList<Ship>())).ToList<Ship>()).ToList<Ship>();
		CollidingProjectiles = (

Enumerable.Empty<Projectile>()).ToList<Projectile>();
		CollidingAsteroids = (

Enumerable.Empty<Asteroid>()).ToList<Asteroid>();
		Asteroids = (

Enumerable.Empty<Asteroid>()).ToList<Asteroid>();
		
}
		public List<Asteroid> Asteroids;
	public List<Asteroid> CollidingAsteroids;
	public List<Projectile> CollidingProjectiles;
	public List<Ship> Ships;
	public Microsoft.Xna.Framework.Input.KeyboardState State;
	public Microsoft.Xna.Framework.Vector2 ___randomPos40;
	public System.Single ___randomWait40;
	public System.Single count_down1;

System.DateTime init_time = System.DateTime.Now;
	public void Update(float dt, World world) {
var t = System.DateTime.Now;		this.Rule0(dt, world);
		this.Rule1(dt, world);
		this.Rule3(dt, world);
		this.Rule5(dt, world);
		for(int x0 = 0; x0 < Asteroids.Count; x0++) { 
			Asteroids[x0].Update(dt, world);
		}
		for(int x0 = 0; x0 < Ships.Count; x0++) { 
			Ships[x0].Update(dt, world);
		}
		this.Rule2(dt, world);
		this.Rule4(dt, world);
	}

	public void Rule0(float dt, World world) 
	{
	CollidingProjectiles = (

(Asteroids).Select(__ContextSymbol4 => new { ___a00 = __ContextSymbol4 })
.SelectMany(__ContextSymbol5=> (Ships).Select(__ContextSymbol6 => new { ___s00 = __ContextSymbol6,
                                                      prev = __ContextSymbol5 })
.SelectMany(__ContextSymbol7=> (__ContextSymbol7.___s00.Projectiles).Select(__ContextSymbol8 => new { ___p00 = __ContextSymbol8,
                                                      prev = __ContextSymbol7 })
.Where(__ContextSymbol9 => ((150f) > (Microsoft.Xna.Framework.Vector2.Distance(__ContextSymbol9.prev.prev.___a00.Position,__ContextSymbol9.___p00.Position))))
.Select(__ContextSymbol10 => __ContextSymbol10.___p00)
.ToList<Projectile>()))).ToList<Projectile>();
	}
	

	public void Rule1(float dt, World world) 
	{
	CollidingAsteroids = (

(Asteroids).Select(__ContextSymbol11 => new { ___a11 = __ContextSymbol11 })
.SelectMany(__ContextSymbol12=> (Ships).Select(__ContextSymbol13 => new { ___s11 = __ContextSymbol13,
                                                      prev = __ContextSymbol12 })
.SelectMany(__ContextSymbol14=> (__ContextSymbol14.___s11.Projectiles).Select(__ContextSymbol15 => new { ___p11 = __ContextSymbol15,
                                                      prev = __ContextSymbol14 })
.Where(__ContextSymbol16 => ((150f) > (Microsoft.Xna.Framework.Vector2.Distance(__ContextSymbol16.prev.prev.___a11.Position,__ContextSymbol16.___p11.Position))))
.Select(__ContextSymbol17 => __ContextSymbol17.prev.prev.___a11)
.ToList<Asteroid>()))).ToList<Asteroid>();
	}
	

	public void Rule3(float dt, World world) 
	{
	Asteroids = (

(Asteroids).Select(__ContextSymbol18 => new { ___a33 = __ContextSymbol18 })
.Where(__ContextSymbol19 => ((1500f) > (__ContextSymbol19.___a33.Position.Y)))
.Select(__ContextSymbol20 => __ContextSymbol20.___a33)
.ToList<Asteroid>()).ToList<Asteroid>();
	}
	

	public void Rule5(float dt, World world) 
	{
	State = Microsoft.Xna.Framework.Input.Keyboard.GetState();
	}
	



	int s2=-1;
	public void Rule2(float dt, World world){ 
	switch (s2)
	{

	case -1:
	if(!(((CollidingAsteroids.Count) > (0))))
	{

	s2 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	Asteroids = (

(Asteroids).Select(__ContextSymbol21 => new { ___a22 = __ContextSymbol21 })
.SelectMany(__ContextSymbol22=> (CollidingAsteroids).Select(__ContextSymbol23 => new { ___ca20 = __ContextSymbol23,
                                                      prev = __ContextSymbol22 })
.Where(__ContextSymbol24 => !(((__ContextSymbol24.prev.___a22) == (__ContextSymbol24.___ca20))))
.Select(__ContextSymbol25 => __ContextSymbol25.prev.___a22)
.ToList<Asteroid>())).ToList<Asteroid>();
	s2 = -1;
return;	
	default: return;}}
	

	int s4=-1;
	public void Rule4(float dt, World world){ 
	switch (s4)
	{

	case -1:
	___randomPos40 = new Microsoft.Xna.Framework.Vector2(Utilities.Random.RandFloat(0f,600f),-50f);
	___randomWait40 = Utilities.Random.RandFloat(2f,5f);
	Asteroids = new Cons<Asteroid>(new Asteroid(___randomPos40), (Asteroids)).ToList<Asteroid>();
	s4 = 0;
return;
	case 0:
	count_down1 = ___randomWait40;
	goto case 1;
	case 1:
	if(((count_down1) > (0f)))
	{

	count_down1 = ((count_down1) - (dt));
	s4 = 1;
return;	}else
	{

	s4 = -1;
return;	}	
	default: return;}}
	





}
public class Ship{
public int frame;
public bool JustEntered = true;
private Microsoft.Xna.Framework.Vector2 p;
	public int ID;
public Ship(Microsoft.Xna.Framework.Vector2 p)
	{JustEntered = false;
 frame = World.frame;
		Score = 0;
		Projectiles = (

Enumerable.Empty<Projectile>()).ToList<Projectile>();
		Position = p;
		Color = new Microsoft.Xna.Framework.Color(Utilities.Random.RandInt(0,256),Utilities.Random.RandInt(0,256),Utilities.Random.RandInt(0,256));
		
}
		public Microsoft.Xna.Framework.Color Color;
	public Microsoft.Xna.Framework.Vector2 Position;
	public List<Projectile> Projectiles;
	public System.Int32 Score;
	public List<Projectile> ___hits00;
	public Microsoft.Xna.Framework.Vector2 ___vy30;
	public Microsoft.Xna.Framework.Vector2 ___vy41;
	public void Update(float dt, World world) {
frame = World.frame;		this.Rule2(dt, world);

		this.Rule0(dt, world);
		this.Rule1(dt, world);
		this.Rule3(dt, world);
		this.Rule4(dt, world);
		for(int x0 = 0; x0 < Projectiles.Count; x0++) { 
			Projectiles[x0].Update(dt, world);
		}
	}

	public void Rule2(float dt, World world) 
	{
	Projectiles = (

(Projectiles).Select(__ContextSymbol28 => new { ___p23 = __ContextSymbol28 })
.Where(__ContextSymbol29 => ((__ContextSymbol29.___p23.Position.Y) > (-50f)))
.Select(__ContextSymbol30 => __ContextSymbol30.___p23)
.ToList<Projectile>()).ToList<Projectile>();
	}
	




	int s0=-1;
	public void Rule0(float dt, World world){ 
	switch (s0)
	{

	case -1:
	if(!(((world.CollidingProjectiles.Count) > (0))))
	{

	s0 = -1;
return;	}else
	{

	goto case 1;	}
	case 1:
	___hits00 = (

(Projectiles).Select(__ContextSymbol31 => new { ___p02 = __ContextSymbol31 })
.SelectMany(__ContextSymbol32=> (world.CollidingProjectiles).Select(__ContextSymbol33 => new { ___cp00 = __ContextSymbol33,
                                                      prev = __ContextSymbol32 })
.Where(__ContextSymbol34 => ((!(((__ContextSymbol34.prev.___p02) == (__ContextSymbol34.___cp00)))) && (((__ContextSymbol34.___cp00.Owner) == (this)))))
.Select(__ContextSymbol35 => __ContextSymbol35.prev.___p02)
.ToList<Projectile>())).ToList<Projectile>();
	Projectiles = ___hits00;
	Score = ((Score) + (((100) * (___hits00.Count))));
	s0 = -1;
return;	
	default: return;}}
	

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

	goto case 1;	}
	case 1:
	Projectiles = new Cons<Projectile>(new Projectile(Position,this), (Projectiles)).ToList<Projectile>();
	s1 = 0;
return;
	case 0:
	if(!(!(world.State.IsKeyDown(Keys.Space))))
	{

	s1 = 0;
return;	}else
	{

	s1 = -1;
return;	}	
	default: return;}}
	

	int s3=-1;
	public void Rule3(float dt, World world){ 
	switch (s3)
	{

	case -1:
	___vy30 = new Microsoft.Xna.Framework.Vector2(300f,0f);
	goto case 1;
	case 1:
	if(!(world.State.IsKeyDown(Keys.D)))
	{

	s3 = 1;
return;	}else
	{

	goto case 0;	}
	case 0:
	Position = ((Position) + (((___vy30) * (dt))));
	s3 = -1;
return;	
	default: return;}}
	

	int s4=-1;
	public void Rule4(float dt, World world){ 
	switch (s4)
	{

	case -1:
	___vy41 = new Microsoft.Xna.Framework.Vector2(-300f,0f);
	goto case 1;
	case 1:
	if(!(world.State.IsKeyDown(Keys.A)))
	{

	s4 = 1;
return;	}else
	{

	goto case 0;	}
	case 0:
	Position = ((Position) + (((___vy41) * (dt))));
	s4 = -1;
return;	
	default: return;}}
	





}
public class Projectile{
public int frame;
public bool JustEntered = true;
private Microsoft.Xna.Framework.Vector2 p;
private Ship owner;
	public int ID;
public Projectile(Microsoft.Xna.Framework.Vector2 p, Ship owner)
	{JustEntered = false;
 frame = World.frame;
		Position = p;
		Owner = owner;
		
}
		public Ship Owner;
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
public class TextBox{
public int frame;
public bool JustEntered = true;
private System.String t;
private Microsoft.Xna.Framework.Vector2 p;
	public int ID;
public TextBox(System.String t, Microsoft.Xna.Framework.Vector2 p)
	{JustEntered = false;
 frame = World.frame;
		Text = t;
		Position = p;
		
}
		public Microsoft.Xna.Framework.Vector2 Position;
	public System.String Text;
	public void Update(float dt, World world) {
frame = World.frame;



	}











}
}