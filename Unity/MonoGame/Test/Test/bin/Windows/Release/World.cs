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

(new Cons<Ship>(new Ship(new Microsoft.Xna.Framework.Vector2(Utilities.Random.RandFloat(250f,1500f),1000f)),(new Empty<Ship>()).ToList<Ship>())).ToList<Ship>()).ToList<Ship>();
		CollidingProjectiles = (

Enumerable.Empty<Projectile>()).ToList<Projectile>();
		CollidingAsteroids = (

Enumerable.Empty<Asteroid>()).ToList<Asteroid>();
		ColliderShipAsteroid = (

Enumerable.Empty<Casanova.Prelude.Tuple<Ship, Asteroid>>()).ToList<Casanova.Prelude.Tuple<Ship, Asteroid>>();
		Asteroids = (

Enumerable.Empty<Asteroid>()).ToList<Asteroid>();
		
}
		public List<Asteroid> Asteroids;
	public List<Casanova.Prelude.Tuple<Ship, Asteroid>> ColliderShipAsteroid;
	public List<Asteroid> CollidingAsteroids;
	public List<Projectile> CollidingProjectiles;
	public List<Ship> Ships;
	public Microsoft.Xna.Framework.Input.KeyboardState State;
	public List<Asteroid> ___updatedAsteroids20;
	public Microsoft.Xna.Framework.Vector2 ___randomPos70;
	public System.Single ___randomWait70;
	public System.Single count_down1;

System.DateTime init_time = System.DateTime.Now;
	public void Update(float dt, World world) {
var t = System.DateTime.Now;		this.Rule0(dt, world);
		this.Rule1(dt, world);
		this.Rule3(dt, world);
		this.Rule4(dt, world);
		this.Rule6(dt, world);
		this.Rule8(dt, world);
		for(int x0 = 0; x0 < Asteroids.Count; x0++) { 
			Asteroids[x0].Update(dt, world);
		}
		for(int x0 = 0; x0 < Ships.Count; x0++) { 
			Ships[x0].Update(dt, world);
		}
		this.Rule2(dt, world);
		this.Rule5(dt, world);
		this.Rule7(dt, world);
	}

	public void Rule0(float dt, World world) 
	{
	CollidingProjectiles = (

(Asteroids).Select(__ContextSymbol5 => new { ___a00 = __ContextSymbol5 })
.SelectMany(__ContextSymbol6=> (Ships).Select(__ContextSymbol7 => new { ___s00 = __ContextSymbol7,
                                                      prev = __ContextSymbol6 })
.SelectMany(__ContextSymbol8=> (__ContextSymbol8.___s00.Projectiles).Select(__ContextSymbol9 => new { ___p00 = __ContextSymbol9,
                                                      prev = __ContextSymbol8 })
.Where(__ContextSymbol10 => ((150f) > (Microsoft.Xna.Framework.Vector2.Distance(__ContextSymbol10.prev.prev.___a00.Position,__ContextSymbol10.___p00.Position))))
.Select(__ContextSymbol11 => __ContextSymbol11.___p00)
.ToList<Projectile>()))).ToList<Projectile>();
	}
	

	public void Rule1(float dt, World world) 
	{
	Ships = (

(Ships).Select(__ContextSymbol12 => new { ___s11 = __ContextSymbol12 })
.Where(__ContextSymbol13 => ((__ContextSymbol13.___s11.Health) > (0)))
.Select(__ContextSymbol14 => __ContextSymbol14.___s11)
.ToList<Ship>()).ToList<Ship>();
	}
	

	public void Rule3(float dt, World world) 
	{
	ColliderShipAsteroid = (

(Asteroids).Select(__ContextSymbol15 => new { ___a32 = __ContextSymbol15 })
.SelectMany(__ContextSymbol16=> (Ships).Select(__ContextSymbol17 => new { ___s32 = __ContextSymbol17,
                                                      prev = __ContextSymbol16 })
.Where(__ContextSymbol18 => ((150f) > (Microsoft.Xna.Framework.Vector2.Distance(__ContextSymbol18.prev.___a32.Position,__ContextSymbol18.___s32.Position))))
.Select(__ContextSymbol19 => new Casanova.Prelude.Tuple<Ship, Asteroid>(__ContextSymbol19.___s32,__ContextSymbol19.prev.___a32))
.ToList<Casanova.Prelude.Tuple<Ship, Asteroid>>())).ToList<Casanova.Prelude.Tuple<Ship, Asteroid>>();
	}
	

	public void Rule4(float dt, World world) 
	{
	CollidingAsteroids = (

(Asteroids).Select(__ContextSymbol20 => new { ___a43 = __ContextSymbol20 })
.SelectMany(__ContextSymbol21=> (Ships).Select(__ContextSymbol22 => new { ___s43 = __ContextSymbol22,
                                                      prev = __ContextSymbol21 })
.SelectMany(__ContextSymbol23=> (__ContextSymbol23.___s43.Projectiles).Select(__ContextSymbol24 => new { ___p41 = __ContextSymbol24,
                                                      prev = __ContextSymbol23 })
.Where(__ContextSymbol25 => ((150f) > (Microsoft.Xna.Framework.Vector2.Distance(__ContextSymbol25.prev.prev.___a43.Position,__ContextSymbol25.___p41.Position))))
.Select(__ContextSymbol26 => __ContextSymbol26.prev.prev.___a43)
.ToList<Asteroid>()))).ToList<Asteroid>();
	}
	

	public void Rule6(float dt, World world) 
	{
	Asteroids = (

(Asteroids).Select(__ContextSymbol27 => new { ___a65 = __ContextSymbol27 })
.Where(__ContextSymbol28 => ((1500f) > (__ContextSymbol28.___a65.Position.Y)))
.Select(__ContextSymbol29 => __ContextSymbol29.___a65)
.ToList<Asteroid>()).ToList<Asteroid>();
	}
	

	public void Rule8(float dt, World world) 
	{
	State = Microsoft.Xna.Framework.Input.Keyboard.GetState();
	}
	



	int s2=-1;
	public void Rule2(float dt, World world){ 
	switch (s2)
	{

	case -1:
	if(!(((ColliderShipAsteroid.Count) > (0))))
	{

	s2 = -1;
return;	}else
	{

	goto case 1;	}
	case 1:
	___updatedAsteroids20 = (

(Asteroids).Select(__ContextSymbol30 => new { ___a21 = __ContextSymbol30 })
.Select(__ContextSymbol31 => new {___colliders20 = (

(ColliderShipAsteroid).Select(__ContextSymbol32 => new { ___c20 = __ContextSymbol32,prev = __ContextSymbol31 })
.Where(__ContextSymbol33 => ((__ContextSymbol33.___c20.Item2) == (__ContextSymbol33.prev.___a21)))
.Select(__ContextSymbol34 => __ContextSymbol34.___c20.Item2)
.ToList<Asteroid>()).ToList<Asteroid>(), prev = __ContextSymbol31 })
.Where(__ContextSymbol35 => ((__ContextSymbol35.___colliders20.Count) == (0)))
.Select(__ContextSymbol36 => __ContextSymbol36.prev.___a21)
.ToList<Asteroid>()).ToList<Asteroid>();
	ColliderShipAsteroid = (

Enumerable.Empty<Casanova.Prelude.Tuple<Ship, Asteroid>>()).ToList<Casanova.Prelude.Tuple<Ship, Asteroid>>();
	Asteroids = ___updatedAsteroids20;
	s2 = -1;
return;	
	default: return;}}
	

	int s5=-1;
	public void Rule5(float dt, World world){ 
	switch (s5)
	{

	case -1:
	if(!(((CollidingAsteroids.Count) > (0))))
	{

	s5 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	Asteroids = (

(Asteroids).Select(__ContextSymbol38 => new { ___a54 = __ContextSymbol38 })
.SelectMany(__ContextSymbol39=> (CollidingAsteroids).Select(__ContextSymbol40 => new { ___ca50 = __ContextSymbol40,
                                                      prev = __ContextSymbol39 })
.Where(__ContextSymbol41 => !(((__ContextSymbol41.prev.___a54) == (__ContextSymbol41.___ca50))))
.Select(__ContextSymbol42 => __ContextSymbol42.prev.___a54)
.ToList<Asteroid>())).ToList<Asteroid>();
	s5 = -1;
return;	
	default: return;}}
	

	int s7=-1;
	public void Rule7(float dt, World world){ 
	switch (s7)
	{

	case -1:
	___randomPos70 = new Microsoft.Xna.Framework.Vector2(Utilities.Random.RandFloat(0f,1500f),-50f);
	___randomWait70 = Utilities.Random.RandFloat(0.5f,3f);
	Asteroids = new Cons<Asteroid>(new Asteroid(___randomPos70), (Asteroids)).ToList<Asteroid>();
	s7 = 0;
return;
	case 0:
	count_down1 = ___randomWait70;
	goto case 1;
	case 1:
	if(((count_down1) > (0f)))
	{

	count_down1 = ((count_down1) - (dt));
	s7 = 1;
return;	}else
	{

	s7 = -1;
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
		Health = 100;
		Color = new Microsoft.Xna.Framework.Color(Utilities.Random.RandInt(0,256),Utilities.Random.RandInt(0,256),Utilities.Random.RandInt(0,256));
		
}
		public Microsoft.Xna.Framework.Color Color;
	public System.Int32 Health;
	public Microsoft.Xna.Framework.Vector2 Position;
	public List<Projectile> Projectiles;
	public System.Int32 Score;
	public List<Projectile> ___hits00;
	public List<Projectile> ___updatedProjs00;
	public List<Casanova.Prelude.Tuple<Ship, Asteroid>> ___colliders11;
	public Microsoft.Xna.Framework.Vector2 ___vy40;
	public Microsoft.Xna.Framework.Vector2 ___vy51;
	public void Update(float dt, World world) {
frame = World.frame;		this.Rule3(dt, world);

		this.Rule0(dt, world);
		this.Rule1(dt, world);
		this.Rule2(dt, world);
		this.Rule4(dt, world);
		this.Rule5(dt, world);
		for(int x0 = 0; x0 < Projectiles.Count; x0++) { 
			Projectiles[x0].Update(dt, world);
		}
	}

	public void Rule3(float dt, World world) 
	{
	Projectiles = (

(Projectiles).Select(__ContextSymbol45 => new { ___p34 = __ContextSymbol45 })
.Where(__ContextSymbol46 => ((__ContextSymbol46.___p34.Position.Y) > (-50f)))
.Select(__ContextSymbol47 => __ContextSymbol47.___p34)
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

	goto case 2;	}
	case 2:
	___hits00 = (

(Projectiles).Select(__ContextSymbol48 => new { ___p02 = __ContextSymbol48 })
.SelectMany(__ContextSymbol49=> (world.CollidingProjectiles).Select(__ContextSymbol50 => new { ___cp00 = __ContextSymbol50,
                                                      prev = __ContextSymbol49 })
.Where(__ContextSymbol51 => ((((__ContextSymbol51.prev.___p02) == (__ContextSymbol51.___cp00))) && (((__ContextSymbol51.prev.___p02.Owner) == (this)))))
.Select(__ContextSymbol52 => __ContextSymbol52.prev.___p02)
.ToList<Projectile>())).ToList<Projectile>();
	___updatedProjs00 = (

(Projectiles).Select(__ContextSymbol53 => new { ___p03 = __ContextSymbol53 })
.Select(__ContextSymbol54 => new {___hits01 = (

(world.CollidingProjectiles).Select(__ContextSymbol55 => new { ___cp01 = __ContextSymbol55,prev = __ContextSymbol54 })
.Where(__ContextSymbol56 => ((((__ContextSymbol56.prev.___p03) == (__ContextSymbol56.___cp01))) && (((__ContextSymbol56.prev.___p03.Owner) == (this)))))
.Select(__ContextSymbol57 => __ContextSymbol57.prev.___p03)
.ToList<Projectile>()).ToList<Projectile>(), prev = __ContextSymbol54 })
.Where(__ContextSymbol58 => ((__ContextSymbol58.___hits01.Count) == (0)))
.Select(__ContextSymbol59 => __ContextSymbol59.prev.___p03)
.ToList<Projectile>()).ToList<Projectile>();
	Projectiles = ___updatedProjs00;
	Score = ((Score) + (___hits00.Count));
	s0 = -1;
return;	
	default: return;}}
	

	int s1=-1;
	public void Rule1(float dt, World world){ 
	switch (s1)
	{

	case -1:
	if(!(((world.ColliderShipAsteroid.Count) > (0))))
	{

	s1 = -1;
return;	}else
	{

	goto case 5;	}
	case 5:
	___colliders11 = (

(world.ColliderShipAsteroid).Select(__ContextSymbol60 => new { ___c11 = __ContextSymbol60 })
.Where(__ContextSymbol61 => ((__ContextSymbol61.___c11.Item1) == (this)))
.Select(__ContextSymbol62 => __ContextSymbol62.___c11)
.ToList<Casanova.Prelude.Tuple<Ship, Asteroid>>()).ToList<Casanova.Prelude.Tuple<Ship, Asteroid>>();
	if(((___colliders11.Count) > (0)))
	{

	goto case 0;	}else
	{

	goto case 1;	}
	case 0:
	Health = ((Health) - (___colliders11.Head().Item2.Damage));
	s1 = -1;
return;
	case 1:
	Health = Health;
	s1 = -1;
return;	
	default: return;}}
	

	int s2=-1;
	public void Rule2(float dt, World world){ 
	switch (s2)
	{

	case -1:
	if(!(world.State.IsKeyDown(Keys.Space)))
	{

	s2 = -1;
return;	}else
	{

	goto case 1;	}
	case 1:
	Projectiles = new Cons<Projectile>(new Projectile(Position,this), (Projectiles)).ToList<Projectile>();
	s2 = 0;
return;
	case 0:
	if(!(!(world.State.IsKeyDown(Keys.Space))))
	{

	s2 = 0;
return;	}else
	{

	s2 = -1;
return;	}	
	default: return;}}
	

	int s4=-1;
	public void Rule4(float dt, World world){ 
	switch (s4)
	{

	case -1:
	___vy40 = new Microsoft.Xna.Framework.Vector2(300f,0f);
	goto case 1;
	case 1:
	if(!(world.State.IsKeyDown(Keys.D)))
	{

	s4 = 1;
return;	}else
	{

	goto case 0;	}
	case 0:
	Position = ((Position) + (((___vy40) * (dt))));
	s4 = -1;
return;	
	default: return;}}
	

	int s5=-1;
	public void Rule5(float dt, World world){ 
	switch (s5)
	{

	case -1:
	___vy51 = new Microsoft.Xna.Framework.Vector2(-300f,0f);
	goto case 1;
	case 1:
	if(!(world.State.IsKeyDown(Keys.A)))
	{

	s5 = 1;
return;	}else
	{

	goto case 0;	}
	case 0:
	Position = ((Position) + (((___vy51) * (dt))));
	s5 = -1;
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
		Damage = Utilities.Random.RandInt(10,31);
		
}
		public System.Int32 Damage;
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