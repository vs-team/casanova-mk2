#pragma warning disable 162,108,618
using Casanova.Prelude;
using System.Linq;
using System;
using System.Collections.Generic;
using Lidgren.Network;
using UnityEngine;
namespace Game {public class World : MonoBehaviour{
public static int frame;
void Update () { Update(Time.deltaTime, this); 
 frame++; }
public bool JustEntered = true;


public void Start()
	{
		Player ___p100;
		___p100 = new Player(0);
		SpawnAmount = 2f;
		ScoreText = new CnvText("Text/Score",true);
		Players = (

(new Cons<Player>(___p100,(new Empty<Player>()).ToList<Player>())).ToList<Player>()).ToList<Player>();
		GameReady = false;
		GameOverText = new CnvText("Text/GameOver",false);
		GameOver = false;
		CurrentPlayer = ___p100;
		Asteroids = (

Enumerable.Empty<Asteroid>()).ToList<Asteroid>();
		
}
		public List<Asteroid> __Asteroids;
	public List<Asteroid> Asteroids{  get { return  __Asteroids; }
  set{ __Asteroids = value;
 foreach(var e in value){if(e.JustEntered){ e.JustEntered = false;
}
} }
 }
	public Player __CurrentPlayer;
	public Player CurrentPlayer{  get { return  __CurrentPlayer; }
  set{ __CurrentPlayer = value;
 if(!value.JustEntered) __CurrentPlayer = value; 
 else{ value.JustEntered = false;
}
 }
 }
	public System.Boolean GameOver;
	public CnvText GameOverText;
	public System.Boolean GameReady;
	public List<Player> __Players;
	public List<Player> Players{  get { return  __Players; }
  set{ __Players = value;
 foreach(var e in value){if(e.JustEntered){ e.JustEntered = false;
}
} }
 }
	public CnvText ScoreText;
	public System.Single SpawnAmount;
	public System.Single ___fadingFactor00;
	public System.Single count_down2;
	public System.Single count_down1;
	public System.Int32 ___currentScore10;
	public System.Single count_down3;
	public System.Single count_down4;
	public System.Int32 ___x50;

System.DateTime init_time = System.DateTime.Now;
	public void Update(float dt, World world) {
var t = System.DateTime.Now;		this.Rule3(dt, world);

		for(int x0 = 0; x0 < Asteroids.Count; x0++) { 
			Asteroids[x0].Update(dt, world);
		}
		GameOverText.Update(dt, world);
		for(int x0 = 0; x0 < Players.Count; x0++) { 
			Players[x0].Update(dt, world);
		}
		ScoreText.Update(dt, world);
		this.Rule0(dt, world);
		this.Rule1(dt, world);
		this.Rule2(dt, world);
		this.Rule4(dt, world);
		this.Rule5(dt, world);
	}

	public void Rule3(float dt, World world) 
	{
	Asteroids = (

(Asteroids).Select(__ContextSymbol2 => new { ___a30 = __ContextSymbol2 })
.Where(__ContextSymbol3 => ((__ContextSymbol3.___a30.Destroyed) == (false)))
.Select(__ContextSymbol4 => __ContextSymbol4.___a30)
.ToList<Asteroid>()).ToList<Asteroid>();
	}
	




	int s0=-1;
	public void Rule0(float dt, World world){ 
	switch (s0)
	{

	case -1:
	if(!(((((((CurrentPlayer.PlayerShip.HitCounter) > (3))) || (((CurrentPlayer.PlayerShip.HitCounter) == (3))))) && (!(GameOver)))))
	{

	s0 = -1;
return;	}else
	{

	goto case 13;	}
	case 13:
	___fadingFactor00 = 0.025f;
	goto case 10;
	case 10:
	if(!(((1f) > (GameOverText.Alpha))))
	{

	goto case 9;	}else
	{

	goto case 11;	}
	case 11:
	GameOverText.Alpha = ((GameOverText.Alpha) + (___fadingFactor00));
	GameOver = false;
	s0 = 10;
return;
	case 9:
	GameOverText.Alpha = 1f;
	GameOver = false;
	s0 = 7;
return;
	case 7:
	count_down2 = 1f;
	goto case 8;
	case 8:
	if(((count_down2) > (0f)))
	{

	count_down2 = ((count_down2) - (dt));
	s0 = 8;
return;	}else
	{

	goto case 4;	}
	case 4:
	if(!(((GameOverText.Alpha) > (0f))))
	{

	goto case 3;	}else
	{

	goto case 5;	}
	case 5:
	GameOverText.Alpha = ((GameOverText.Alpha) - (___fadingFactor00));
	GameOver = false;
	s0 = 4;
return;
	case 3:
	GameOverText.Alpha = 0f;
	GameOver = false;
	s0 = 1;
return;
	case 1:
	count_down1 = 0.5f;
	goto case 2;
	case 2:
	if(((count_down1) > (0f)))
	{

	count_down1 = ((count_down1) - (dt));
	s0 = 2;
return;	}else
	{

	goto case 0;	}
	case 0:
	GameOverText.Alpha = GameOverText.Alpha;
	GameOver = true;
	s0 = -1;
return;	
	default: return;}}
	

	int s1=-1;
	public void Rule1(float dt, World world){ 
	switch (s1)
	{

	case -1:
	___currentScore10 = CurrentPlayer.Score;
	ScoreText.Text = (("Score: ") + (___currentScore10.ToString()));
	s1 = 0;
return;
	case 0:
	if(!(!(((___currentScore10) == (CurrentPlayer.Score)))))
	{

	s1 = 0;
return;	}else
	{

	s1 = -1;
return;	}	
	default: return;}}
	

	int s2=-1;
	public void Rule2(float dt, World world){ 
	switch (s2)
	{

	case -1:
	if(!(GameOver))
	{

	s2 = -1;
return;	}else
	{

	goto case 1;	}
	case 1:
	GameOver = GameOver;
	s2 = 0;
return;
	case 0:
	UnityEngine.Application.Quit();
	s2 = -1;
return;	
	default: return;}}
	

	int s4=-1;
	public void Rule4(float dt, World world){ 
	switch (s4)
	{

	case -1:
	count_down3 = 1f;
	goto case 6;
	case 6:
	if(((count_down3) > (0f)))
	{

	count_down3 = ((count_down3) - (dt));
	s4 = 6;
return;	}else
	{

	goto case 2;	}
	case 2:
	if(((SpawnAmount) > (0.1f)))
	{

	goto case 0;	}else
	{

	goto case 1;	}
	case 0:
	SpawnAmount = ((SpawnAmount) - (0.1f));
	s4 = -1;
return;
	case 1:
	SpawnAmount = SpawnAmount;
	s4 = -1;
return;	
	default: return;}}
	

	int s5=-1;
	public void Rule5(float dt, World world){ 
	switch (s5)
	{

	case -1:
	count_down4 = SpawnAmount;
	goto case 3;
	case 3:
	if(((count_down4) > (0f)))
	{

	count_down4 = ((count_down4) - (dt));
	s5 = 3;
return;	}else
	{

	goto case 1;	}
	case 1:
	___x50 = UnityEngine.Random.Range(-15,15);
	Asteroids = new Cons<Asteroid>(new Asteroid(new UnityEngine.Vector3(((System.Single)___x50),7f,-15f)), (Asteroids)).ToList<Asteroid>();
	s5 = -1;
return;	
	default: return;}}
	





}
public class Player{
public int frame;
public bool JustEntered = true;
private System.Int32 index;
	public int ID;
public Player(System.Int32 index)
	{JustEntered = false;
 frame = World.frame;
		Score = 0;
		PlayerShip = new Ship(new UnityEngine.Vector3((-5f) * ((index) + (1)),-2f,-15f));
		PlayerID = 1;
		Heart3 = new Life("HealthBar/Life3");
		Heart2 = new Life("HealthBar/Life2");
		Heart1 = new Life("HealthBar/Life1");
		
}
		public Life Heart1;
	public Life Heart2;
	public Life Heart3;
	public System.Int32 PlayerID;
	public Ship PlayerShip;
	public System.Int32 Score;
	public List<Bullet> ___impactBullets00;
	public void Update(float dt, World world) {
frame = World.frame;

		this.Rule0(dt, world);
		this.Rule1(dt, world);
		Heart1.Update(dt, world);
		Heart2.Update(dt, world);
		Heart3.Update(dt, world);
		PlayerShip.Update(dt, world);
	}





	int s0=-1;
	public void Rule0(float dt, World world){ 
	switch (s0)
	{

	case -1:
	___impactBullets00 = (

(PlayerShip.Projectiles).Select(__ContextSymbol5 => new { ___projectile00 = __ContextSymbol5 })
.Where(__ContextSymbol6 => ((!(__ContextSymbol6.___projectile00.Removed)) && (__ContextSymbol6.___projectile00.Impact)))
.Select(__ContextSymbol7 => __ContextSymbol7.___projectile00)
.ToList<Bullet>()).ToList<Bullet>();
	Score = ((Score) + (___impactBullets00.Count));
	s0 = -1;
return;	
	default: return;}}
	

	int s1=-1;
	public void Rule1(float dt, World world){ 
	switch (s1)
	{

	case -1:
	if(((PlayerShip.HitCounter) == (0)))
	{

	goto case 12;	}else
	{

	goto case 8;	}
	case 12:
	Heart1.Active = true;
	Heart2.Active = true;
	Heart3.Active = true;
	s1 = 8;
return;
	case 8:
	if(((PlayerShip.HitCounter) == (1)))
	{

	goto case 9;	}else
	{

	goto case 5;	}
	case 9:
	Heart1.Active = true;
	Heart2.Active = true;
	Heart3.Active = false;
	s1 = 5;
return;
	case 5:
	if(((PlayerShip.HitCounter) == (2)))
	{

	goto case 6;	}else
	{

	goto case 2;	}
	case 6:
	Heart1.Active = true;
	Heart2.Active = false;
	Heart3.Active = false;
	s1 = 2;
return;
	case 2:
	if(((PlayerShip.HitCounter) == (3)))
	{

	goto case 3;	}else
	{

	s1 = -1;
return;	}
	case 3:
	Heart1.Active = false;
	Heart2.Active = false;
	Heart3.Active = false;
	s1 = -1;
return;	
	default: return;}}
	





}
public class Ship{
public int frame;
public bool JustEntered = true;
private UnityEngine.Vector3 position;
	public int ID;
public Ship(UnityEngine.Vector3 position)
	{JustEntered = false;
 frame = World.frame;
		UnityShip = UnityShip.Instantiate(position);
		Projectiles = (

Enumerable.Empty<Bullet>()).ToList<Bullet>();
		HitCounter = 0;
		
}
		public UnityShip.ShipNetworkBuffer Buffer{  get { return UnityShip.Buffer; }
 }
	public System.Boolean Hit{  get { return UnityShip.Hit; }
  set{UnityShip.Hit = value; }
 }
	public System.Int32 HitCounter;
	public UnityNetworkManager Manager{  get { return UnityShip.Manager; }
 }
	public UnityEngine.Vector3 Position{  get { return UnityShip.Position; }
  set{UnityShip.Position = value; }
 }
	public List<Bullet> Projectiles;
	public UnityShip UnityShip;
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
	public System.Single count_down5;
	public void Update(float dt, World world) {
frame = World.frame;		this.Rule1(dt, world);

		this.Rule0(dt, world);
		this.Rule2(dt, world);
		this.Rule3(dt, world);
		this.Rule4(dt, world);
		this.Rule5(dt, world);
		for(int x0 = 0; x0 < Projectiles.Count; x0++) { 
			Projectiles[x0].Update(dt, world);
		}
	}

	public void Rule1(float dt, World world) 
	{
	Projectiles = (

(Projectiles).Select(__ContextSymbol10 => new { ___p10 = __ContextSymbol10 })
.Where(__ContextSymbol11 => !(__ContextSymbol11.___p10.Destroyed))
.Select(__ContextSymbol12 => __ContextSymbol12.___p10)
.ToList<Bullet>()).ToList<Bullet>();
	}
	




	int s0=-1;
	public void Rule0(float dt, World world){ 
	switch (s0)
	{

	case -1:
	UnityNetworkManager.ReadMessage(UnityShip.Manager);
	Position = Position;
	s0 = -1;
return;	
	default: return;}}
	

	int s2=-1;
	public void Rule2(float dt, World world){ 
	switch (s2)
	{

	case -1:
	if(!(Hit))
	{

	s2 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	HitCounter = ((HitCounter) + (1));
	Hit = false;
	s2 = -1;
return;	
	default: return;}}
	

	int s3=-1;
	public void Rule3(float dt, World world){ 
	switch (s3)
	{

	case -1:
	if(!(UnityEngine.Input.GetKey(KeyCode.D)))
	{

	s3 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	Position = ((Position) + (new UnityEngine.Vector3((4f) * (dt),0f,0f)));
	s3 = -1;
return;	
	default: return;}}
	

	int s4=-1;
	public void Rule4(float dt, World world){ 
	switch (s4)
	{

	case -1:
	if(!(UnityEngine.Input.GetKey(KeyCode.A)))
	{

	s4 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	Position = ((Position) - (new UnityEngine.Vector3((4f) * (dt),0f,0f)));
	s4 = -1;
return;	
	default: return;}}
	

	int s5=-1;
	public void Rule5(float dt, World world){ 
	switch (s5)
	{

	case -1:
	if(!(UnityEngine.Input.GetKeyDown(KeyCode.Space)))
	{

	s5 = -1;
return;	}else
	{

	goto case 2;	}
	case 2:
	Projectiles = new Cons<Bullet>(new Bullet(Position,this), (Projectiles)).ToList<Bullet>();
	s5 = 0;
return;
	case 0:
	count_down5 = 0.1f;
	goto case 1;
	case 1:
	if(((count_down5) > (0f)))
	{

	count_down5 = ((count_down5) - (dt));
	s5 = 1;
return;	}else
	{

	s5 = -1;
return;	}	
	default: return;}}
	





}
public class Bullet{
public int frame;
public bool JustEntered = true;
private UnityEngine.Vector3 pos;
private Ship owner;
	public int ID;
public Bullet(UnityEngine.Vector3 pos, Ship owner)
	{JustEntered = false;
 frame = World.frame;
		UnityBullet = UnityBullet.Instantiate(pos);
		Removed = false;
		Owner = owner;
		
}
		public System.Boolean Destroyed{  get { return UnityBullet.Destroyed; }
  set{UnityBullet.Destroyed = value; }
 }
	public System.Boolean Impact{  get { return UnityBullet.Impact; }
  set{UnityBullet.Impact = value; }
 }
	public Ship Owner;
	public UnityEngine.Vector3 Position{  get { return UnityBullet.Position; }
  set{UnityBullet.Position = value; }
 }
	public System.Boolean Removed;
	public UnityBullet UnityBullet;
	public UnityEngine.Vector3 ViewPortPoint{  get { return UnityBullet.ViewPortPoint; }
 }
	public System.Boolean destroyed{  get { return UnityBullet.destroyed; }
  set{UnityBullet.destroyed = value; }
 }
	public System.Boolean enabled{  get { return UnityBullet.enabled; }
  set{UnityBullet.enabled = value; }
 }
	public UnityEngine.GameObject gameObject{  get { return UnityBullet.gameObject; }
 }
	public UnityEngine.HideFlags hideFlags{  get { return UnityBullet.hideFlags; }
  set{UnityBullet.hideFlags = value; }
 }
	public System.Boolean impact{  get { return UnityBullet.impact; }
  set{UnityBullet.impact = value; }
 }
	public System.Boolean isActiveAndEnabled{  get { return UnityBullet.isActiveAndEnabled; }
 }
	public UnityEngine.Camera mainCamera{  get { return UnityBullet.mainCamera; }
  set{UnityBullet.mainCamera = value; }
 }
	public System.String name{  get { return UnityBullet.name; }
  set{UnityBullet.name = value; }
 }
	public System.String tag{  get { return UnityBullet.tag; }
  set{UnityBullet.tag = value; }
 }
	public UnityEngine.Transform transform{  get { return UnityBullet.transform; }
 }
	public System.Boolean useGUILayout{  get { return UnityBullet.useGUILayout; }
  set{UnityBullet.useGUILayout = value; }
 }
	public System.Single count_down6;
	public void Update(float dt, World world) {
frame = World.frame;

		this.Rule0(dt, world);
		this.Rule1(dt, world);
	}





	int s0=-1;
	public void Rule0(float dt, World world){ 
	switch (s0)
	{

	case -1:
	if(!(!(Removed)))
	{

	s0 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	Position = ((Position) + (new UnityEngine.Vector3(0f,(5f) * (dt),0f)));
	s0 = -1;
return;	
	default: return;}}
	

	int s1=-1;
	public void Rule1(float dt, World world){ 
	switch (s1)
	{

	case -1:
	if(!(((!(Removed)) && (((Impact) || (((ViewPortPoint.y) > (1f))))))))
	{

	s1 = -1;
return;	}else
	{

	goto case 1;	}
	case 1:
	count_down6 = dt;
	goto case 2;
	case 2:
	if(((count_down6) > (0f)))
	{

	count_down6 = ((count_down6) - (dt));
	s1 = 2;
return;	}else
	{

	goto case 0;	}
	case 0:
	Destroyed = true;
	Removed = true;
	s1 = -1;
return;	
	default: return;}}
	





}
public class Asteroid{
public int frame;
public bool JustEntered = true;
private UnityEngine.Vector3 pos;
	public int ID;
public Asteroid(UnityEngine.Vector3 pos)
	{JustEntered = false;
 frame = World.frame;
		UnityAsteroid = UnityAsteroid.Instantiate(pos);
		Removed = false;
		
}
		public System.Boolean Destroyed{  get { return UnityAsteroid.Destroyed; }
  set{UnityAsteroid.Destroyed = value; }
 }
	public System.Boolean Destroying{  get { return UnityAsteroid.Destroying; }
  set{UnityAsteroid.Destroying = value; }
 }
	public System.Int32 Hash{  get { return UnityAsteroid.Hash; }
 }
	public System.Boolean Hit{  get { return UnityAsteroid.Hit; }
  set{UnityAsteroid.Hit = value; }
 }
	public UnityEngine.Vector3 Position{  get { return UnityAsteroid.Position; }
  set{UnityAsteroid.Position = value; }
 }
	public System.Boolean Removed;
	public UnityAsteroid UnityAsteroid;
	public UnityEngine.Vector3 ViewPortPoint{  get { return UnityAsteroid.ViewPortPoint; }
 }
	public System.Boolean enabled{  get { return UnityAsteroid.enabled; }
  set{UnityAsteroid.enabled = value; }
 }
	public UnityEngine.GameObject gameObject{  get { return UnityAsteroid.gameObject; }
 }
	public UnityEngine.HideFlags hideFlags{  get { return UnityAsteroid.hideFlags; }
  set{UnityAsteroid.hideFlags = value; }
 }
	public System.Boolean isActiveAndEnabled{  get { return UnityAsteroid.isActiveAndEnabled; }
 }
	public System.String name{  get { return UnityAsteroid.name; }
  set{UnityAsteroid.name = value; }
 }
	public System.String tag{  get { return UnityAsteroid.tag; }
  set{UnityAsteroid.tag = value; }
 }
	public UnityEngine.Transform transform{  get { return UnityAsteroid.transform; }
 }
	public System.Boolean useGUILayout{  get { return UnityAsteroid.useGUILayout; }
  set{UnityAsteroid.useGUILayout = value; }
 }
	public System.Single count_down7;
	public void Update(float dt, World world) {
frame = World.frame;

		this.Rule0(dt, world);
		this.Rule1(dt, world);
	}





	int s0=-1;
	public void Rule0(float dt, World world){ 
	switch (s0)
	{

	case -1:
	if(!(((!(Removed)) && (((Hit) || (((-0.4f) > (ViewPortPoint.y))))))))
	{

	s0 = -1;
return;	}else
	{

	goto case 1;	}
	case 1:
	count_down7 = dt;
	goto case 2;
	case 2:
	if(((count_down7) > (0f)))
	{

	count_down7 = ((count_down7) - (dt));
	s0 = 2;
return;	}else
	{

	goto case 0;	}
	case 0:
	Destroyed = true;
	Removed = true;
	s0 = -1;
return;	
	default: return;}}
	

	int s1=-1;
	public void Rule1(float dt, World world){ 
	switch (s1)
	{

	case -1:
	if(!(!(Removed)))
	{

	s1 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	Position = ((Position) + (new UnityEngine.Vector3(0f,(-5f) * (dt),0f)));
	s1 = -1;
return;	
	default: return;}}
	





}
public class Life{
public int frame;
public bool JustEntered = true;
private System.String n;
	public int ID;
public Life(System.String n)
	{JustEntered = false;
 frame = World.frame;
		UnityPlayerHealth = UnityPlayerHealth.Find(n);
		
}
		public System.Boolean Active{  set{UnityPlayerHealth.Active = value; }
 }
	public UnityPlayerHealth UnityPlayerHealth;
	public System.Boolean enabled{  get { return UnityPlayerHealth.enabled; }
  set{UnityPlayerHealth.enabled = value; }
 }
	public UnityEngine.GameObject gameObject{  get { return UnityPlayerHealth.gameObject; }
 }
	public UnityEngine.HideFlags hideFlags{  get { return UnityPlayerHealth.hideFlags; }
  set{UnityPlayerHealth.hideFlags = value; }
 }
	public System.Boolean isActiveAndEnabled{  get { return UnityPlayerHealth.isActiveAndEnabled; }
 }
	public System.String name{  get { return UnityPlayerHealth.name; }
  set{UnityPlayerHealth.name = value; }
 }
	public System.String tag{  get { return UnityPlayerHealth.tag; }
  set{UnityPlayerHealth.tag = value; }
 }
	public UnityEngine.Transform transform{  get { return UnityPlayerHealth.transform; }
 }
	public System.Boolean useGUILayout{  get { return UnityPlayerHealth.useGUILayout; }
  set{UnityPlayerHealth.useGUILayout = value; }
 }
	public void Update(float dt, World world) {
frame = World.frame;



	}











}
public class CnvText{
public int frame;
public bool JustEntered = true;
private System.String n;
private System.Boolean isVisible;
	public int ID;
public CnvText(System.String n, System.Boolean isVisible)
	{JustEntered = false;
 frame = World.frame;
		UnityText = UnityText.Find(n,isVisible);
		
}
		public System.Single Alpha{  get { return UnityText.Alpha; }
  set{UnityText.Alpha = value; }
 }
	public System.String Text{  get { return UnityText.Text; }
  set{UnityText.Text = value; }
 }
	public UnityText UnityText;
	public System.Boolean enabled{  get { return UnityText.enabled; }
  set{UnityText.enabled = value; }
 }
	public UnityEngine.GameObject gameObject{  get { return UnityText.gameObject; }
 }
	public UnityEngine.HideFlags hideFlags{  get { return UnityText.hideFlags; }
  set{UnityText.hideFlags = value; }
 }
	public System.Boolean isActiveAndEnabled{  get { return UnityText.isActiveAndEnabled; }
 }
	public System.String name{  get { return UnityText.name; }
  set{UnityText.name = value; }
 }
	public System.String tag{  get { return UnityText.tag; }
  set{UnityText.tag = value; }
 }
	public UnityEngine.Transform transform{  get { return UnityText.transform; }
 }
	public System.Boolean useGUILayout{  get { return UnityText.useGUILayout; }
  set{UnityText.useGUILayout = value; }
 }
	public void Update(float dt, World world) {
frame = World.frame;



	}











}
public class NetworkManager{
public int frame;
public bool JustEntered = true;
	public int ID;
public NetworkManager()
	{JustEntered = false;
 frame = World.frame;
		UnityNetworkManager = UnityNetworkManager.Instantiate();
		
}
		public Lidgren.Network.NetClient Client{  get { return UnityNetworkManager.Client; }
 }
	public UnityNetworkManager UnityNetworkManager;
	public System.Boolean enabled{  get { return UnityNetworkManager.enabled; }
  set{UnityNetworkManager.enabled = value; }
 }
	public UnityEngine.GameObject gameObject{  get { return UnityNetworkManager.gameObject; }
 }
	public UnityEngine.HideFlags hideFlags{  get { return UnityNetworkManager.hideFlags; }
  set{UnityNetworkManager.hideFlags = value; }
 }
	public System.Boolean isActiveAndEnabled{  get { return UnityNetworkManager.isActiveAndEnabled; }
 }
	public System.String name{  get { return UnityNetworkManager.name; }
  set{UnityNetworkManager.name = value; }
 }
	public System.String tag{  get { return UnityNetworkManager.tag; }
  set{UnityNetworkManager.tag = value; }
 }
	public UnityEngine.Transform transform{  get { return UnityNetworkManager.transform; }
 }
	public System.Boolean useGUILayout{  get { return UnityNetworkManager.useGUILayout; }
  set{UnityNetworkManager.useGUILayout = value; }
 }
	public void Update(float dt, World world) {
frame = World.frame;



	}











}
public class Message{
public int frame;
public bool JustEntered = true;
private System.Int32 a;
private System.Int32 b;
private System.Int32 c;
	public int ID;
public Message(System.Int32 a, System.Int32 b, System.Int32 c)
	{JustEntered = false;
 frame = World.frame;
		Test = 13;
		PropertyID = c;
		EntityTypeID = a;
		EntityID = b;
		
}
		public System.Int32 EntityID;
	public System.Int32 EntityTypeID;
	public System.Int32 PropertyID;
	public System.Int32 Test;
	public void Update(float dt, World world) {
frame = World.frame;



	}











}
}                               