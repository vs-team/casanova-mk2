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
		UnityEngine.Debug.Log(___links100.Count);
		UnityEngine.Debug.Log(___links200.Count);
		UnityEngine.Debug.Log(___planets00.Count);
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
		MySource = planet;
		FleetsToMerge = (

Enumerable.Empty<AttackingFleetToMerge>()).ToList<AttackingFleetToMerge>();
		FleetsToDestroyNextTurn = (

Enumerable.Empty<AttackingFleet>()).ToList<AttackingFleet>();
		DefenceLost = (new Nothing<System.Int32>());
		AttackingFleets = (

Enumerable.Empty<AttackingFleet>()).ToList<AttackingFleet>();
		AttackLost = (new Nothing<System.Int32>());
		
}
		public Option<System.Int32> AttackLost;
	public List<AttackingFleet> AttackingFleets;
	public Option<System.Int32> DefenceLost;
	public List<AttackingFleet> FleetsToDestroyNextTurn;
	public List<AttackingFleetToMerge> FleetsToMerge;
	public Planet MySource;
	public List<AttackingFleet> ___new_attacking_fleets20;
	public List<AttackingFleet> ___filtered_attacking_fleets20;
	public System.Single count_down1;
	public System.Single count_down2;
	public void Update(float dt, World world) {
frame = World.frame;

		this.Rule0(dt, world);
		this.Rule1(dt, world);
		this.Rule2(dt, world);
		this.Rule3(dt, world);
		this.Rule4(dt, world);
if(AttackLost.IsSome){  } 
		for(int x0 = 0; x0 < AttackingFleets.Count; x0++) { 
			AttackingFleets[x0].Update(dt, world);
		}
if(DefenceLost.IsSome){  } 
		for(int x0 = 0; x0 < FleetsToDestroyNextTurn.Count; x0++) { 
			FleetsToDestroyNextTurn[x0].Update(dt, world);
		}
		for(int x0 = 0; x0 < FleetsToMerge.Count; x0++) { 
			FleetsToMerge[x0].Update(dt, world);
		}
	}





	int s0=-1;
	public void Rule0(float dt, World world){ 
	switch (s0)
	{

	case -1:
	FleetsToMerge = (

(MySource.InboundFleets).Select(__ContextSymbol29 => new { ___i_f00 = __ContextSymbol29 })
.SelectMany(__ContextSymbol30=> (AttackingFleets).Select(__ContextSymbol31 => new { ___a_f00 = __ContextSymbol31,
                                                      prev = __ContextSymbol30 })
.Where(__ContextSymbol32 => ((!(__ContextSymbol32.___a_f00.MyFleet.Destroyed)) && (((__ContextSymbol32.prev.___i_f00.Link) == (__ContextSymbol32.___a_f00.MyFleet.Link)))))
.Select(__ContextSymbol33 => new AttackingFleetToMerge(__ContextSymbol33.prev.___i_f00,__ContextSymbol33.___a_f00))
.ToList<AttackingFleetToMerge>())).ToList<AttackingFleetToMerge>();
	s0 = -1;
return;	
	default: return;}}
	

	int s1=-1;
	public void Rule1(float dt, World world){ 
	switch (s1)
	{

	case -1:
	FleetsToDestroyNextTurn = (

(AttackingFleets).Select(__ContextSymbol34 => new { ___f10 = __ContextSymbol34 })
.Where(__ContextSymbol35 => ((MySource.Owner.IsSome) && (((__ContextSymbol35.___f10.MyFleet.Owner) == (MySource.Owner.Value)))))
.Select(__ContextSymbol36 => __ContextSymbol36.___f10)
.ToList<AttackingFleet>()).ToList<AttackingFleet>();
	s1 = -1;
return;	
	default: return;}}
	

	int s2=-1;
	public void Rule2(float dt, World world){ 
	switch (s2)
	{

	case -1:
	___new_attacking_fleets20 = (

(MySource.InboundFleets).Select(__ContextSymbol37 => new { ___f21 = __ContextSymbol37 })
.Select(__ContextSymbol38 => new {___is_ship_to_merge20 = (

(FleetsToMerge).Select(__ContextSymbol39 => new { ___flee_to_merge20 = __ContextSymbol39,prev = __ContextSymbol38 })
.Where(__ContextSymbol40 => ((__ContextSymbol40.___flee_to_merge20.MyFleet) == (__ContextSymbol40.prev.___f21)))
.Select(__ContextSymbol41 => 1)
.Aggregate(default(System.Int32), (acc, __x) => acc + __x)), prev = __ContextSymbol38 })
.Where(__ContextSymbol43 => ((((__ContextSymbol43.___is_ship_to_merge20) == (0))) && (((MySource.Owner.IsNone) || (!(((__ContextSymbol43.prev.___f21.Owner) == (MySource.Owner.Value))))))))
.Select(__ContextSymbol44 => new AttackingFleet(__ContextSymbol44.prev.___f21,this))
.ToList<AttackingFleet>()).ToList<AttackingFleet>();
	___filtered_attacking_fleets20 = (

(AttackingFleets).Select(__ContextSymbol45 => new { ___f22 = __ContextSymbol45 })
.Where(__ContextSymbol46 => ((!(__ContextSymbol46.___f22.MyFleet.Destroyed)) && (((MySource.Owner.IsSome) && (!(((__ContextSymbol46.___f22.MyFleet.Owner) == (MySource.Owner.Value))))))))
.Select(__ContextSymbol47 => __ContextSymbol47.___f22)
.ToList<AttackingFleet>()).ToList<AttackingFleet>();
	AttackingFleets = (___new_attacking_fleets20).Concat(___filtered_attacking_fleets20).ToList<AttackingFleet>();
	s2 = -1;
return;	
	default: return;}}
	

	int s3=-1;
	public void Rule3(float dt, World world){ 
	switch (s3)
	{

	case -1:
	if(!(((!(((AttackingFleets.Count) > (1)))) || (true))))
	{

	s3 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	if(!(((AttackingFleets.Count) > (1))))
	{

	goto case 2;	}else
	{

	if(true)
	{

	goto case 3;	}else
	{

	s3 = 0;
return;	}	}
	case 2:
	AttackingFleets = AttackingFleets;
	s3 = -1;
return;
	case 3:
	count_down1 = UnityEngine.Random.Range(1f,2f);
	goto case 7;
	case 7:
	if(((count_down1) > (0f)))
	{

	count_down1 = ((count_down1) - (dt));
	s3 = 7;
return;	}else
	{

	goto case 5;	}
	case 5:
	AttackingFleets = (AttackingFleets.Tail()).Concat((

(new Cons<AttackingFleet>(AttackingFleets.Head(),(new Empty<AttackingFleet>()).ToList<AttackingFleet>())).ToList<AttackingFleet>()).ToList<AttackingFleet>()).ToList<AttackingFleet>();
	s3 = -1;
return;	
	default: return;}}
	

	int s4=-1;
	public void Rule4(float dt, World world){ 
	switch (s4)
	{

	case -1:
	AttackLost = (new Nothing<System.Int32>());
	DefenceLost = (new Nothing<System.Int32>());
	s4 = 3;
return;
	case 3:
	count_down2 = 1f;
	goto case 4;
	case 4:
	if(((count_down2) > (0f)))
	{

	count_down2 = ((count_down2) - (dt));
	s4 = 4;
return;	}else
	{

	goto case 0;	}
	case 0:
	if(((AttackingFleets.Count) > (0)))
	{

	goto case 1;	}else
	{

	s4 = -1;
return;	}
	case 1:
	AttackLost = (new Just<System.Int32>(1));
	DefenceLost = (new Just<System.Int32>(1));
	s4 = -1;
return;	
	default: return;}}
	





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
	public System.Int32 ___fleets_to_add30;
	public System.Single count_down3;
	public Player ___new_owner50;
	public System.Int32 ___fleets_to_add51;
	public void Update(float dt, World world) {
frame = World.frame;

		this.Rule0(dt, world);
		this.Rule1(dt, world);
		this.Rule2(dt, world);
		this.Rule3(dt, world);
		this.Rule4(dt, world);
		this.Rule5(dt, world);
		this.Rule6(dt, world);
		this.Rule7(dt, world);
		this.Rule8(dt, world);
		this.Rule9(dt, world);
if(Battle.IsSome){ 		Battle.Value.Update(dt, world);
 } 
		for(int x0 = 0; x0 < InboundFleets.Count; x0++) { 
			InboundFleets[x0].Update(dt, world);
		}
		for(int x0 = 0; x0 < LandingFleets.Count; x0++) { 
			LandingFleets[x0].Update(dt, world);
		}
		Statistics.Update(dt, world);
	}





	int s0=-1;
	public void Rule0(float dt, World world){ 
	switch (s0)
	{

	case -1:
	InboundFleets = (

(world.Links).Select(__ContextSymbol53 => new { ___l00 = __ContextSymbol53 })
.Where(__ContextSymbol54 => ((__ContextSymbol54.___l00.Destination) == (this)))
.SelectMany(__ContextSymbol55=> (__ContextSymbol55.___l00.ArrivedFleets).Select(__ContextSymbol56 => new { ___f03 = __ContextSymbol56,
                                                      prev = __ContextSymbol55 })
.Select(__ContextSymbol57 => __ContextSymbol57.___f03.MyFleet)
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

	goto case 1;	}else
	{

	goto case 2;	}
	case 1:
	LandingFleets = (

(InboundFleets).Select(__ContextSymbol58 => new { ___inbound_fleet10 = __ContextSymbol58 })
.Where(__ContextSymbol59 => ((__ContextSymbol59.___inbound_fleet10.Owner) == (Owner.Value)))
.Select(__ContextSymbol60 => new LandingFleet(__ContextSymbol60.___inbound_fleet10))
.ToList<LandingFleet>()).ToList<LandingFleet>();
	s1 = -1;
return;
	case 2:
	LandingFleets = (

Enumerable.Empty<LandingFleet>()).ToList<LandingFleet>();
	s1 = -1;
return;	
	default: return;}}
	

	int s2=-1;
	public void Rule2(float dt, World world){ 
	switch (s2)
	{

	case -1:
	if(((((((Owner.IsNone) && (Battle.IsNone))) && (!(((LandingFleets.Count) == (InboundFleets.Count)))))) || (((Owner.IsSome) && (!(((LandingFleets.Count) == (InboundFleets.Count))))))))
	{

	goto case 6;	}else
	{

	goto case 7;	}
	case 6:
	Battle = (new Just<Battle>(new Battle(this)));
	s2 = 10;
return;
	case 10:
	if(!(!(((Battle.Value.AttackingFleets.Count) > (0)))))
	{

	s2 = 10;
return;	}else
	{

	goto case 9;	}
	case 9:
	Battle = (new Nothing<Battle>());
	s2 = -1;
return;
	case 7:
	Battle = (new Nothing<Battle>());
	s2 = -1;
return;	
	default: return;}}
	

	int s3=-1;
	public void Rule3(float dt, World world){ 
	switch (s3)
	{

	case -1:
	___fleets_to_add30 = (

(LandingFleets).Select(__ContextSymbol62 => new { ___f34 = __ContextSymbol62 })
.Select(__ContextSymbol63 => __ContextSymbol63.___f34.MyFleet.Ships)
.Aggregate(default(System.Int32), (acc, __x) => acc + __x));
	LocalFleets = ((LocalFleets) + (___fleets_to_add30));
	s3 = -1;
return;	
	default: return;}}
	

	int s4=-1;
	public void Rule4(float dt, World world){ 
	switch (s4)
	{

	case -1:
	if(!(((((Battle.IsSome) && (Battle.Value.DefenceLost.IsSome))) || (((Battle.IsSome) || (((Owner.IsNone) || (true))))))))
	{

	s4 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	if(((Battle.IsSome) && (Battle.Value.DefenceLost.IsSome)))
	{

	goto case 2;	}else
	{

	if(Battle.IsSome)
	{

	goto case 3;	}else
	{

	if(Owner.IsNone)
	{

	goto case 4;	}else
	{

	if(true)
	{

	goto case 5;	}else
	{

	s4 = 0;
return;	}	}	}	}
	case 2:
	LocalFleets = ((LocalFleets) - (Battle.Value.DefenceLost.Value));
	s4 = -1;
return;
	case 3:
	LocalFleets = LocalFleets;
	s4 = -1;
return;
	case 4:
	LocalFleets = 0;
	s4 = -1;
return;
	case 5:
	count_down3 = UnityEngine.Random.Range(1,3);
	goto case 11;
	case 11:
	if(((count_down3) > (0f)))
	{

	count_down3 = ((count_down3) - (dt));
	s4 = 11;
return;	}else
	{

	goto case 9;	}
	case 9:
	LocalFleets = ((LocalFleets) + (1));
	s4 = -1;
return;	
	default: return;}}
	

	int s5=-1;
	public void Rule5(float dt, World world){ 
	switch (s5)
	{

	case -1:
	if(((((Battle.IsSome) && (((LocalFleets) == (0))))) && (((Battle.Value.AttackingFleets.Count) > (0)))))
	{

	goto case 13;	}else
	{

	s5 = -1;
return;	}
	case 13:
	___new_owner50 = Battle.Value.AttackingFleets.Head().MyFleet.Owner;
	___fleets_to_add51 = (

(Battle.Value.AttackingFleets).Select(__ContextSymbol65 => new { ___f55 = __ContextSymbol65 })
.Where(__ContextSymbol66 => ((((__ContextSymbol66.___f55.MyFleet.Owner) == (___new_owner50))) && (((__ContextSymbol66.___f55.MyFleet.Ships) > (0)))))
.Select(__ContextSymbol67 => __ContextSymbol67.___f55.MyFleet.Ships)
.Aggregate(default(System.Int32), (acc, __x) => acc + __x));
	Owner = (new Just<Player>(Battle.Value.AttackingFleets.Head().MyFleet.Owner));
	LocalFleets = ___fleets_to_add51;
	s5 = -1;
return;	
	default: return;}}
	

	int s6=-1;
	public void Rule6(float dt, World world){ 
	switch (s6)
	{

	case -1:
	if(Owner.IsSome)
	{

	goto case 17;	}else
	{

	goto case 18;	}
	case 17:
	Info = ((Owner.Value.Name) + (" "));
	s6 = -1;
return;
	case 18:
	Info = "";
	s6 = -1;
return;	
	default: return;}}
	

	int s7=-1;
	public void Rule7(float dt, World world){ 
	switch (s7)
	{

	case -1:
	Info = ((Info) + (LocalFleets.ToString()));
	s7 = -1;
return;	
	default: return;}}
	

	int s8=-1;
	public void Rule8(float dt, World world){ 
	switch (s8)
	{

	case -1:
	if(!(UnityEngine.Input.GetMouseButtonDown(1)))
	{

	s8 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	if(IsHit)
	{

	goto case 1;	}else
	{

	s8 = -1;
return;	}
	case 1:
	RightSelected = true;
	s8 = 2;
return;
	case 2:
	RightSelected = false;
	s8 = -1;
return;	
	default: return;}}
	

	int s9=-1;
	public void Rule9(float dt, World world){ 
	switch (s9)
	{

	case -1:
	if(!(UnityEngine.Input.GetMouseButtonDown(0)))
	{

	s9 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	Selected = IsHit;
	s9 = -1;
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
private Link link;
	public int ID;
public Fleet(GameStatistic statistics, System.Int32 ships, Player owner, UnityEngine.Vector3 position, Link link)
	{JustEntered = false;
 frame = World.frame;
		UnityFleet = UnityFleet.Instantiate(position,link.Destination.Position);
		Statistics = statistics;
		Ships = ships;
		Owner = owner;
		Link = link;
		
}
		public System.Boolean Destroyed{  get { return UnityFleet.Destroyed; }
  set{UnityFleet.Destroyed = value; }
 }
	public System.String Info{  get { return UnityFleet.Info; }
  set{UnityFleet.Info = value; }
 }
	public Link Link;
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

		this.Rule0(dt, world);
		this.Rule1(dt, world);
		Statistics.Update(dt, world);
	}





	int s0=-1;
	public void Rule0(float dt, World world){ 
	switch (s0)
	{

	case -1:
	if(!(!(((Ships) > (0)))))
	{

	s0 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	Destroyed = true;
	s0 = -1;
return;	
	default: return;}}
	

	int s1=-1;
	public void Rule1(float dt, World world){ 
	switch (s1)
	{

	case -1:
	Info = Ships.ToString();
	s1 = -1;
return;	
	default: return;}}
	





}
public class AttackingFleetToMerge{
public int frame;
public bool JustEntered = true;
private Fleet fleet;
private AttackingFleet fleet_to_merge_with;
	public int ID;
public AttackingFleetToMerge(Fleet fleet, AttackingFleet fleet_to_merge_with)
	{JustEntered = false;
 frame = World.frame;
		MyFleet = fleet;
		FleetToMergeWith = fleet_to_merge_with;
		
}
		public AttackingFleet FleetToMergeWith;
	public Fleet MyFleet;
	public void Update(float dt, World world) {
frame = World.frame;

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

		this.Rule0(dt, world);
		this.Rule1(dt, world);
		this.Rule2(dt, world);
		this.Rule3(dt, world);
	}





	int s0=-1;
	public void Rule0(float dt, World world){ 
	switch (s0)
	{

	case -1:
	if(!(((MyBattle.AttackLost.IsSome) && (((MyBattle.AttackingFleets.Head()) == (this))))))
	{

	s0 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	MyFleet.Ships = ((MyFleet.Ships) - (MyBattle.AttackLost.Value));
	s0 = -1;
return;	
	default: return;}}
	

	int s1=-1;
	public void Rule1(float dt, World world){ 
	switch (s1)
	{

	case -1:
	MyFleet.Ships = (((

(MyBattle.FleetsToMerge).Select(__ContextSymbol69 => new { ___f16 = __ContextSymbol69 })
.Where(__ContextSymbol70 => ((__ContextSymbol70.___f16.FleetToMergeWith) == (this)))
.Select(__ContextSymbol71 => __ContextSymbol71.___f16.MyFleet.Ships)
.Aggregate(default(System.Int32), (acc, __x) => acc + __x))) + (MyFleet.Ships));
	s1 = -1;
return;	
	default: return;}}
	

	int s2=-1;
	public void Rule2(float dt, World world){ 
	switch (s2)
	{

	case -1:
	MyFleet.Info = MyFleet.Ships.ToString();
	s2 = -1;
return;	
	default: return;}}
	

	int s3=-1;
	public void Rule3(float dt, World world){ 
	switch (s3)
	{

	case -1:
	if(!(((((MyBattle.MySource.Owner.IsSome) && (((MyFleet.Owner) == (MyBattle.MySource.Owner.Value))))) || (!(((MyFleet.Ships) > (0)))))))
	{

	s3 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	MyFleet.Destroyed = true;
	s3 = -1;
return;	
	default: return;}}
	





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

		this.Rule0(dt, world);

		MyFleet.Update(dt, world);
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

		this.Rule0(dt, world);

		MyFleet.Update(dt, world);
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

		this.Rule0(dt, world);
		this.Rule1(dt, world);
		this.Rule2(dt, world);
		for(int x0 = 0; x0 < TravellingFleets.Count; x0++) { 
			TravellingFleets[x0].Update(dt, world);
		}
	}





	int s0=-1;
	public void Rule0(float dt, World world){ 
	switch (s0)
	{

	case -1:
	ArrivedFleets = (

(TravellingFleets).Select(__ContextSymbol77 => new { ___f07 = __ContextSymbol77 })
.Where(__ContextSymbol78 => !(((UnityEngine.Vector3.Distance(__ContextSymbol78.___f07.MyFleet.Position,Destination.Position)) > (Destination.MinApproachingDist))))
.Select(__ContextSymbol79 => __ContextSymbol79.___f07)
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

(TravellingFleets).Select(__ContextSymbol80 => new { ___f18 = __ContextSymbol80 })
.Where(__ContextSymbol81 => ((UnityEngine.Vector3.Distance(__ContextSymbol81.___f18.MyFleet.Position,Destination.Position)) > (Destination.MinApproachingDist)))
.Select(__ContextSymbol82 => __ContextSymbol82.___f18)
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
	___new_fleet20 = new Fleet(new GameStatistic(1f,1f,1f,1f),Source.LocalFleets,Source.Owner.Value,Source.Position,this);
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