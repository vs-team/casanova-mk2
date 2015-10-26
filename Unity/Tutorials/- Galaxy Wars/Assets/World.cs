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
		System.Collections.Generic.List<UnityPlanet> ___unity_planets00;
		___unity_planets00 = UnityPlanet.FindAll();
		List<Player> ___players00;
		___players00 = (

(UnityPlayer.FindAll()).Select(__ContextSymbol0 => new { ___p00 = __ContextSymbol0 })
.Select(__ContextSymbol1 => new Player(__ContextSymbol1.___p00.Name,new GameStatistic(1f,1f,1f,1f)))
.ToList<Player>()).ToList<Player>();
		List<Planet> ___planets00;
		___planets00 = (

(___unity_planets00).Select(__ContextSymbol2 => new { ___unity_planet00 = __ContextSymbol2 })
.Select(__ContextSymbol3 => new {___owner00 = (

(___players00).Select(__ContextSymbol4 => new { ___player00 = __ContextSymbol4,prev = __ContextSymbol3 })
.Where(__ContextSymbol5 => ((((__ContextSymbol5.prev.___unity_planet00.InitialOwnerName) == (__ContextSymbol5.___player00.Name))) && (!(((__ContextSymbol5.prev.___unity_planet00.InitialOwnerName) == (""))))))
.Select(__ContextSymbol6 => __ContextSymbol6.___player00)
.ToList<Player>()).ToList<Player>(), prev = __ContextSymbol3 })
.Select(__ContextSymbol7 => new {___owner01 = Utils.IfThenElse<Option<Player>>((()=> ((__ContextSymbol7.___owner00.Count) > (0))), (()=>	(new Just<Player>(__ContextSymbol7.___owner00.Head()))
),(()=>	(new Nothing<Player>())
)), prev = __ContextSymbol7 })
.Select(__ContextSymbol8 => new Planet(__ContextSymbol8.prev.prev.___unity_planet00,new GameStatistic(1f,1f,1f,1f),__ContextSymbol8.___owner01))
.ToList<Planet>()).ToList<Planet>();
		System.Collections.Generic.List<UnityLine> ___unity_links00;
		___unity_links00 = UnityLine.FindAll();
		List<Link> ___links100;
		___links100 = (

(___unity_links00).Select(__ContextSymbol9 => new { ___unity_link00 = __ContextSymbol9 })
.SelectMany(__ContextSymbol10=> (___planets00).Select(__ContextSymbol11 => new { ___source00 = __ContextSymbol11,
                                                      prev = __ContextSymbol10 })
.SelectMany(__ContextSymbol12=> (___planets00).Select(__ContextSymbol13 => new { ___destination00 = __ContextSymbol13,
                                                      prev = __ContextSymbol12 })
.Where(__ContextSymbol14 => ((((__ContextSymbol14.prev.prev.___unity_link00.f) == (__ContextSymbol14.prev.___source00.UnityPlanet.gameObject))) && (((__ContextSymbol14.prev.prev.___unity_link00.t) == (__ContextSymbol14.___destination00.UnityPlanet.gameObject)))))
.Select(__ContextSymbol15 => new Link(__ContextSymbol15.prev.prev.___unity_link00,__ContextSymbol15.prev.___source00,__ContextSymbol15.___destination00))
.ToList<Link>()))).ToList<Link>();
		List<Link> ___links200;
		___links200 = (

(___unity_links00).Select(__ContextSymbol16 => new { ___unity_link01 = __ContextSymbol16 })
.SelectMany(__ContextSymbol17=> (___planets00).Select(__ContextSymbol18 => new { ___source01 = __ContextSymbol18,
                                                      prev = __ContextSymbol17 })
.SelectMany(__ContextSymbol19=> (___planets00).Select(__ContextSymbol20 => new { ___destination01 = __ContextSymbol20,
                                                      prev = __ContextSymbol19 })
.Where(__ContextSymbol21 => ((((__ContextSymbol21.prev.prev.___unity_link01.f) == (__ContextSymbol21.prev.___source01.UnityPlanet.gameObject))) && (((__ContextSymbol21.prev.prev.___unity_link01.t) == (__ContextSymbol21.___destination01.UnityPlanet.gameObject)))))
.Select(__ContextSymbol22 => new Link(__ContextSymbol22.prev.prev.___unity_link01,__ContextSymbol22.___destination01,__ContextSymbol22.prev.___source01))
.ToList<Link>()))).ToList<Link>();
		UnityEngine.Debug.Log(___planets00.Count);
		UnityEngine.Debug.Log(___unity_links00.Count);
		UnityEngine.Debug.Log(___links100.Count);
		UnityEngine.Debug.Log(___links200.Count);
		Planets = ___planets00;
		Links = (___links100).Concat(___links200).ToList<Link>();
		
}
		public List<Link> __Links;
	public List<Link> Links{  get { return  __Links; }
  set{ __Links = value;
 foreach(var e in value){if(e.JustEntered){ e.JustEntered = false;
}
} }
 }
	public List<Planet> __Planets;
	public List<Planet> Planets{  get { return  __Planets; }
  set{ __Planets = value;
 foreach(var e in value){if(e.JustEntered){ e.JustEntered = false;
}
} }
 }

System.DateTime init_time = System.DateTime.Now;
	public void Update(float dt, World world) {
var t = System.DateTime.Now;

		for(int x0 = 0; x0 < Links.Count; x0++) { 
			Links[x0].Update(dt, world);
		}
		for(int x0 = 0; x0 < Planets.Count; x0++) { 
			Planets[x0].Update(dt, world);
		}


	}











}
public class Battle{
public int frame;
public bool JustEntered = true;
private Planet planet;
	public int ID;
public Battle(Planet planet)
	{JustEntered = false;
 frame = World.frame;
		SelectedAttackingFleet = (new Nothing<AttackingFleet>());
		MySource = planet;
		DefenceLost = (new Nothing<System.Int32>());
		AttackingFleets = (

Enumerable.Empty<AttackingFleet>()).ToList<AttackingFleet>();
		AttackLost = (new Nothing<System.Int32>());
		
}
		public Option<System.Int32> AttackLost;
	public List<AttackingFleet> AttackingFleets;
	public Option<System.Int32> DefenceLost;
	public Planet MySource;
	public Option<AttackingFleet> SelectedAttackingFleet;
	public void Update(float dt, World world) {
frame = World.frame;

if(AttackLost.IsSome){  } 
		for(int x0 = 0; x0 < AttackingFleets.Count; x0++) { 
			AttackingFleets[x0].Update(dt, world);
		}
if(DefenceLost.IsSome){  } 
if(SelectedAttackingFleet.IsSome){ 		SelectedAttackingFleet.Value.Update(dt, world);
 } 


	}











}
public class Planet{
public int frame;
public bool JustEntered = true;
private UnityPlanet up;
private GameStatistic statistics;
private Option<Player> owner;
	public int ID;
public Planet(UnityPlanet up, GameStatistic statistics, Option<Player> owner)
	{JustEntered = false;
 frame = World.frame;
		UnityPlanet = up;
		Statistics = statistics;
		Owner = owner;
		MinApproachingDist = 0.5f;
		LocalFleets = 0;
		LandingFleets = (

Enumerable.Empty<LandingFleet>()).ToList<LandingFleet>();
		InboundFleets = (

Enumerable.Empty<Fleet>()).ToList<Fleet>();
		Battle = (new Nothing<Battle>());
		
}
		public Option<Battle> Battle;
	public List<Fleet> InboundFleets;
	public System.String Info{  get { return UnityPlanet.Info; }
  set{UnityPlanet.Info = value; }
 }
	public System.String InitialOwnerName{  get { return UnityPlanet.InitialOwnerName; }
 }
	public System.Boolean IsHit{  get { return UnityPlanet.IsHit; }
 }
	public List<LandingFleet> LandingFleets;
	public System.Int32 LocalFleets;
	public System.Single MinApproachingDist;
	public Option<Player> Owner;
	public UnityEngine.Vector3 Position{  get { return UnityPlanet.Position; }
 }
	public System.Boolean RightSelected{  get { return UnityPlanet.RightSelected; }
  set{UnityPlanet.RightSelected = value; }
 }
	public System.Boolean Selected{  get { return UnityPlanet.Selected; }
  set{UnityPlanet.Selected = value; }
 }
	public GameStatistic Statistics;
	public UnityPlanet UnityPlanet;
	public System.Boolean enabled{  get { return UnityPlanet.enabled; }
  set{UnityPlanet.enabled = value; }
 }
	public UnityEngine.GameObject gameObject{  get { return UnityPlanet.gameObject; }
 }
	public UnityEngine.HideFlags hideFlags{  get { return UnityPlanet.hideFlags; }
  set{UnityPlanet.hideFlags = value; }
 }
	public UnityEngine.GameObject initial_owner{  get { return UnityPlanet.initial_owner; }
  set{UnityPlanet.initial_owner = value; }
 }
	public System.Boolean isActiveAndEnabled{  get { return UnityPlanet.isActiveAndEnabled; }
 }
	public System.String name{  get { return UnityPlanet.name; }
  set{UnityPlanet.name = value; }
 }
	public System.Boolean rightSelected{  get { return UnityPlanet.rightSelected; }
  set{UnityPlanet.rightSelected = value; }
 }
	public System.Boolean selected{  get { return UnityPlanet.selected; }
  set{UnityPlanet.selected = value; }
 }
	public System.String tag{  get { return UnityPlanet.tag; }
  set{UnityPlanet.tag = value; }
 }
	public UnityEngine.Transform transform{  get { return UnityPlanet.transform; }
 }
	public System.Boolean useGUILayout{  get { return UnityPlanet.useGUILayout; }
  set{UnityPlanet.useGUILayout = value; }
 }
	public System.Int32 ___fleets_to_add20;
	public System.Single count_down1;
	public void Update(float dt, World world) {
frame = World.frame;

if(Battle.IsSome){ 		Battle.Value.Update(dt, world);
 } 
		for(int x0 = 0; x0 < InboundFleets.Count; x0++) { 
			InboundFleets[x0].Update(dt, world);
		}
		for(int x0 = 0; x0 < LandingFleets.Count; x0++) { 
			LandingFleets[x0].Update(dt, world);
		}
		Statistics.Update(dt, world);
		this.Rule0(dt, world);
		this.Rule1(dt, world);
		this.Rule2(dt, world);
		this.Rule3(dt, world);
		this.Rule4(dt, world);
		this.Rule5(dt, world);
		this.Rule6(dt, world);
		this.Rule7(dt, world);
	}





	int s0=-1;
	public void Rule0(float dt, World world){ 
	switch (s0)
	{

	case -1:
	InboundFleets = (

(world.Links).Select(__ContextSymbol29 => new { ___l00 = __ContextSymbol29 })
.Where(__ContextSymbol30 => ((__ContextSymbol30.___l00.Destination) == (this)))
.SelectMany(__ContextSymbol31=> (__ContextSymbol31.___l00.ArrivedFleets).Select(__ContextSymbol32 => new { ___f00 = __ContextSymbol32,
                                                      prev = __ContextSymbol31 })
.Select(__ContextSymbol33 => __ContextSymbol33.___f00.MyFleet)
.ToList<Fleet>())).ToList<Fleet>();
	s0 = -1;
return;	
	default: return;}}
	

	int s1=-1;
	public void Rule1(float dt, World world){ 
	switch (s1)
	{

	case -1:
	if(Owner.IsSome)
	{

	goto case 2;	}else
	{

	s1 = -1;
return;	}
	case 2:
	LandingFleets = (

(InboundFleets).Select(__ContextSymbol34 => new { ___inbound_fleet10 = __ContextSymbol34 })
.Where(__ContextSymbol35 => ((__ContextSymbol35.___inbound_fleet10.Owner) == (Owner.Value)))
.Select(__ContextSymbol36 => new LandingFleet(__ContextSymbol36.___inbound_fleet10))
.ToList<LandingFleet>()).ToList<LandingFleet>();
	s1 = -1;
return;	
	default: return;}}
	

	int s2=-1;
	public void Rule2(float dt, World world){ 
	switch (s2)
	{

	case -1:
	if(Owner.IsSome)
	{

	goto case 5;	}else
	{

	s2 = -1;
return;	}
	case 5:
	___fleets_to_add20 = (

(LandingFleets).Select(__ContextSymbol37 => new { ___f21 = __ContextSymbol37 })
.Select(__ContextSymbol38 => __ContextSymbol38.___f21.MyFleet.Ships)
.Aggregate(default(System.Int32), (acc, __x) => acc + __x));
	LocalFleets = ((LocalFleets) + (___fleets_to_add20));
	s2 = -1;
return;	
	default: return;}}
	

	int s3=-1;
	public void Rule3(float dt, World world){ 
	switch (s3)
	{

	case -1:
	count_down1 = UnityEngine.Random.Range(1,3);
	goto case 2;
	case 2:
	if(((count_down1) > (0f)))
	{

	count_down1 = ((count_down1) - (dt));
	s3 = 2;
return;	}else
	{

	goto case 0;	}
	case 0:
	LocalFleets = ((LocalFleets) + (1));
	s3 = -1;
return;	
	default: return;}}
	

	int s4=-1;
	public void Rule4(float dt, World world){ 
	switch (s4)
	{

	case -1:
	if(Owner.IsSome)
	{

	goto case 3;	}else
	{

	goto case 4;	}
	case 3:
	Info = ((Owner.Value.Name) + (" "));
	s4 = -1;
return;
	case 4:
	Info = "";
	s4 = -1;
return;	
	default: return;}}
	

	int s5=-1;
	public void Rule5(float dt, World world){ 
	switch (s5)
	{

	case -1:
	Info = ((Info) + (LocalFleets.ToString()));
	s5 = -1;
return;	
	default: return;}}
	

	int s6=-1;
	public void Rule6(float dt, World world){ 
	switch (s6)
	{

	case -1:
	if(!(UnityEngine.Input.GetMouseButtonDown(1)))
	{

	s6 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	if(IsHit)
	{

	goto case 1;	}else
	{

	s6 = -1;
return;	}
	case 1:
	RightSelected = true;
	s6 = 2;
return;
	case 2:
	RightSelected = false;
	s6 = -1;
return;	
	default: return;}}
	

	int s7=-1;
	public void Rule7(float dt, World world){ 
	switch (s7)
	{

	case -1:
	if(!(UnityEngine.Input.GetMouseButtonDown(0)))
	{

	s7 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	Selected = IsHit;
	s7 = -1;
return;	
	default: return;}}
	





}
public class Fleet{
public int frame;
public bool JustEntered = true;
private GameStatistic statistics;
private System.Int32 ships;
private Player owner;
private UnityEngine.Vector3 position;
	public int ID;
public Fleet(GameStatistic statistics, System.Int32 ships, Player owner, UnityEngine.Vector3 position)
	{JustEntered = false;
 frame = World.frame;
		UnityFleet = UnityFleet.Instantiate(position);
		Statistics = statistics;
		Ships = ships;
		Owner = owner;
		
}
		public System.Boolean Destroyed{  get { return UnityFleet.Destroyed; }
  set{UnityFleet.Destroyed = value; }
 }
	public Player Owner;
	public UnityEngine.Vector3 Position{  get { return UnityFleet.Position; }
  set{UnityFleet.Position = value; }
 }
	public System.Int32 Ships;
	public GameStatistic Statistics;
	public UnityFleet UnityFleet;
	public System.Boolean enabled{  get { return UnityFleet.enabled; }
  set{UnityFleet.enabled = value; }
 }
	public UnityEngine.GameObject gameObject{  get { return UnityFleet.gameObject; }
 }
	public UnityEngine.HideFlags hideFlags{  get { return UnityFleet.hideFlags; }
  set{UnityFleet.hideFlags = value; }
 }
	public System.Boolean isActiveAndEnabled{  get { return UnityFleet.isActiveAndEnabled; }
 }
	public System.String name{  get { return UnityFleet.name; }
  set{UnityFleet.name = value; }
 }
	public System.String tag{  get { return UnityFleet.tag; }
  set{UnityFleet.tag = value; }
 }
	public UnityEngine.Transform transform{  get { return UnityFleet.transform; }
 }
	public System.Boolean useGUILayout{  get { return UnityFleet.useGUILayout; }
  set{UnityFleet.useGUILayout = value; }
 }
	public void Update(float dt, World world) {
frame = World.frame;

		Statistics.Update(dt, world);


	}











}
public class AttackingFleet{
public int frame;
public bool JustEntered = true;
private Fleet myFleet;
private Battle myBattle;
	public int ID;
public AttackingFleet(Fleet myFleet, Battle myBattle)
	{JustEntered = false;
 frame = World.frame;
		MyFleet = myFleet;
		MyBattle = myBattle;
		
}
		public Battle MyBattle;
	public Fleet MyFleet;
	public void Update(float dt, World world) {
frame = World.frame;

		MyFleet.Update(dt, world);


	}











}
public class LandingFleet{
public int frame;
public bool JustEntered = true;
private Fleet myFleet;
	public int ID;
public LandingFleet(Fleet myFleet)
	{JustEntered = false;
 frame = World.frame;
		MyFleet = myFleet;
		
}
		public Fleet MyFleet;
	public void Update(float dt, World world) {
frame = World.frame;

		MyFleet.Update(dt, world);
		this.Rule0(dt, world);

	}





	int s0=-1;
	public void Rule0(float dt, World world){ 
	switch (s0)
	{

	case -1:
	MyFleet.Destroyed = true;
	s0 = -1;
return;	
	default: return;}}
	






}
public class TravellingFleet{
public int frame;
public bool JustEntered = true;
private Fleet myfleet;
private Planet destination;
	public int ID;
public TravellingFleet(Fleet myfleet, Planet destination)
	{JustEntered = false;
 frame = World.frame;
		UnityEngine.Vector3 ___velocity00;
		___velocity00 = (destination.Position) - (myfleet.Position);
		UnityEngine.Vector3 ___velocity_norm00;
		___velocity_norm00 = ___velocity00.normalized;
		Velocity = ___velocity_norm00;
		MyFleet = myfleet;
		MaxVelocity = 1f;
		Destination = destination;
		
}
		public Planet Destination;
	public System.Single MaxVelocity;
	public Fleet MyFleet;
	public UnityEngine.Vector3 Velocity;
	public void Update(float dt, World world) {
frame = World.frame;

		MyFleet.Update(dt, world);
		this.Rule0(dt, world);

	}





	int s0=-1;
	public void Rule0(float dt, World world){ 
	switch (s0)
	{

	case -1:
	MyFleet.Position = ((MyFleet.Position) + (((((Velocity) * (MaxVelocity))) * (dt))));
	s0 = -1;
return;	
	default: return;}}
	






}
public class Link{
public int frame;
public bool JustEntered = true;
private UnityLine ul;
private Planet s;
private Planet d;
	public int ID;
public Link(UnityLine ul, Planet s, Planet d)
	{JustEntered = false;
 frame = World.frame;
		UnityLine = ul;
		TravellingFleets = (

Enumerable.Empty<TravellingFleet>()).ToList<TravellingFleet>();
		Source = s;
		Destination = d;
		ArrivedFleets = (

Enumerable.Empty<TravellingFleet>()).ToList<TravellingFleet>();
		
}
		public List<TravellingFleet> ArrivedFleets;
	public Planet Destination;
	public Planet Source;
	public List<TravellingFleet> TravellingFleets;
	public UnityLine UnityLine;
	public UnityEngine.Color c1{  get { return UnityLine.c1; }
  set{UnityLine.c1 = value; }
 }
	public UnityEngine.Color c2{  get { return UnityLine.c2; }
  set{UnityLine.c2 = value; }
 }
	public System.Boolean enabled{  get { return UnityLine.enabled; }
  set{UnityLine.enabled = value; }
 }
	public UnityEngine.GameObject f{  get { return UnityLine.f; }
  set{UnityLine.f = value; }
 }
	public UnityEngine.GameObject gameObject{  get { return UnityLine.gameObject; }
 }
	public UnityEngine.HideFlags hideFlags{  get { return UnityLine.hideFlags; }
  set{UnityLine.hideFlags = value; }
 }
	public System.Boolean isActiveAndEnabled{  get { return UnityLine.isActiveAndEnabled; }
 }
	public System.Int32 lengthOfLineRenderer{  get { return UnityLine.lengthOfLineRenderer; }
  set{UnityLine.lengthOfLineRenderer = value; }
 }
	public System.String name{  get { return UnityLine.name; }
  set{UnityLine.name = value; }
 }
	public UnityEngine.GameObject t{  get { return UnityLine.t; }
  set{UnityLine.t = value; }
 }
	public System.String tag{  get { return UnityLine.tag; }
  set{UnityLine.tag = value; }
 }
	public UnityEngine.Transform transform{  get { return UnityLine.transform; }
 }
	public System.Boolean useGUILayout{  get { return UnityLine.useGUILayout; }
  set{UnityLine.useGUILayout = value; }
 }
	public Fleet ___new_fleet20;
	public void Update(float dt, World world) {
frame = World.frame;

		for(int x0 = 0; x0 < TravellingFleets.Count; x0++) { 
			TravellingFleets[x0].Update(dt, world);
		}
		this.Rule0(dt, world);
		this.Rule1(dt, world);
		this.Rule2(dt, world);
	}





	int s0=-1;
	public void Rule0(float dt, World world){ 
	switch (s0)
	{

	case -1:
	ArrivedFleets = (

(TravellingFleets).Select(__ContextSymbol44 => new { ___f02 = __ContextSymbol44 })
.Where(__ContextSymbol45 => !(((UnityEngine.Vector3.Distance(__ContextSymbol45.___f02.MyFleet.Position,Destination.Position)) > (Destination.MinApproachingDist))))
.Select(__ContextSymbol46 => __ContextSymbol46.___f02)
.ToList<TravellingFleet>()).ToList<TravellingFleet>();
	s0 = -1;
return;	
	default: return;}}
	

	int s1=-1;
	public void Rule1(float dt, World world){ 
	switch (s1)
	{

	case -1:
	TravellingFleets = (

(TravellingFleets).Select(__ContextSymbol47 => new { ___f13 = __ContextSymbol47 })
.Where(__ContextSymbol48 => ((UnityEngine.Vector3.Distance(__ContextSymbol48.___f13.MyFleet.Position,Destination.Position)) > (Destination.MinApproachingDist)))
.Select(__ContextSymbol49 => __ContextSymbol49.___f13)
.ToList<TravellingFleet>()).ToList<TravellingFleet>();
	s1 = -1;
return;	
	default: return;}}
	

	int s2=-1;
	public void Rule2(float dt, World world){ 
	switch (s2)
	{

	case -1:
	if(!(((((((((Source.Selected) && (Destination.RightSelected))) && (Source.Owner.IsSome))) && (Source.Battle.IsNone))) && (((Source.LocalFleets) > (0))))))
	{

	s2 = -1;
return;	}else
	{

	goto case 1;	}
	case 1:
	___new_fleet20 = new Fleet(new GameStatistic(1f,1f,1f,1f),Source.LocalFleets,Source.Owner.Value,Source.Position);
	TravellingFleets = new Cons<TravellingFleet>(new TravellingFleet(___new_fleet20,Destination), (TravellingFleets)).ToList<TravellingFleet>();
	Source.LocalFleets = 0;
	s2 = -1;
return;	
	default: return;}}
	





}
public class Player{
public int frame;
public bool JustEntered = true;
private System.String name;
private GameStatistic statistics;
	public int ID;
public Player(System.String name, GameStatistic statistics)
	{JustEntered = false;
 frame = World.frame;
		Statistics = statistics;
		Name = name;
		
}
		public System.String Name;
	public GameStatistic Statistics;
	public void Update(float dt, World world) {
frame = World.frame;

		Statistics.Update(dt, world);


	}











}
public class GameStatistic{
public int frame;
public bool JustEntered = true;
private System.Single a;
private System.Single d;
private System.Single p;
private System.Single r;
	public int ID;
public GameStatistic(System.Single a, System.Single d, System.Single p, System.Single r)
	{JustEntered = false;
 frame = World.frame;
		Research = r;
		Production = p;
		Defence = d;
		Attack = a;
		
}
		public System.Single Attack;
	public System.Single Defence;
	public System.Single Production;
	public System.Single Research;
	public void Update(float dt, World world) {
frame = World.frame;



	}











}
}        