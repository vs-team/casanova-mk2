#pragma warning disable 162,108,618
using Casanova.Prelude;
using System.Linq;
using System;
using System.Collections.Generic;
using Animation;
using UnityEngine;
namespace Game {public class World : MonoBehaviour{
public static int frame;
void Update () { Update(Time.deltaTime, this); 
 frame++; }
public bool JustEntered = true;


public void Start()
	{
		System.Int32 ___AmountOfPlayers00;
		___AmountOfPlayers00 = UnityMenu.AmountOfPlayers;
		GameConstants ___constants00;
		___constants00 = new GameConstants();
		System.Collections.Generic.List<UnityPlanet> ___unityPlanets00;
		___unityPlanets00 = UnityPlanet.FindAllPlanets(___AmountOfPlayers00);
		System.Collections.Generic.List<UnityLink> ___unityLinks00;
		___unityLinks00 = UnityLink.FindLinks();
		System.Collections.Generic.List<UnityStarSystem> ___unityStarSystem00;
		___unityStarSystem00 = UnityStarSystem.FindAllStarSystems();
		UnityRouting.findRouter(___unityPlanets00,___unityLinks00);
		Faction ___Types00;
		___Types00 = ___constants00.Aggressive;
		List<Commander> ___commanders00;
		___commanders00 = (

(Enumerable.Range(1,(1) + ((___AmountOfPlayers00) - (1))).ToList<System.Int32>()).Select(__ContextSymbol15 => new { ___a01 = __ContextSymbol15 })
.Select(__ContextSymbol16 => new Commander(__ContextSymbol16.___a01,___Types00))
.ToList<Commander>()).ToList<Commander>();
		List<System.Single> ___MinPlanetPositionX00;
		___MinPlanetPositionX00 = (

(___unityPlanets00).Select(__ContextSymbol17 => new { ___p01 = __ContextSymbol17 })
.Select(__ContextSymbol18 => new {___other00 = (

(___unityPlanets00).Select(__ContextSymbol19 => new { ___p100 = __ContextSymbol19,prev = __ContextSymbol18 })
.Where(__ContextSymbol20 => ((((__ContextSymbol20.prev.___p01.Position.x) > (__ContextSymbol20.___p100.Position.x))) && (!(((__ContextSymbol20.prev.___p01) == (__ContextSymbol20.___p100))))))
.Select(__ContextSymbol21 => __ContextSymbol21.___p100)
.ToList<UnityPlanet>()).ToList<UnityPlanet>(), prev = __ContextSymbol18 })
.Where(__ContextSymbol22 => ((__ContextSymbol22.___other00.Count) == (0)))
.Select(__ContextSymbol23 => __ContextSymbol23.prev.___p01.Position.x)
.ToList<System.Single>()).ToList<System.Single>();
		List<System.Single> ___MinPlanetPositionY00;
		___MinPlanetPositionY00 = (

(___unityPlanets00).Select(__ContextSymbol24 => new { ___p02 = __ContextSymbol24 })
.Select(__ContextSymbol25 => new {___other01 = (

(___unityPlanets00).Select(__ContextSymbol26 => new { ___p101 = __ContextSymbol26,prev = __ContextSymbol25 })
.Where(__ContextSymbol27 => ((((__ContextSymbol27.prev.___p02.Position.y) > (__ContextSymbol27.___p101.Position.y))) && (!(((__ContextSymbol27.prev.___p02) == (__ContextSymbol27.___p101))))))
.Select(__ContextSymbol28 => __ContextSymbol28.___p101)
.ToList<UnityPlanet>()).ToList<UnityPlanet>(), prev = __ContextSymbol25 })
.Where(__ContextSymbol29 => ((__ContextSymbol29.___other01.Count) == (0)))
.Select(__ContextSymbol30 => __ContextSymbol30.prev.___p02.Position.y)
.ToList<System.Single>()).ToList<System.Single>();
		List<System.Single> ___MinPlanetPositionZ00;
		___MinPlanetPositionZ00 = (

(___unityPlanets00).Select(__ContextSymbol31 => new { ___p03 = __ContextSymbol31 })
.Select(__ContextSymbol32 => new {___other02 = (

(___unityPlanets00).Select(__ContextSymbol33 => new { ___p102 = __ContextSymbol33,prev = __ContextSymbol32 })
.Where(__ContextSymbol34 => ((((__ContextSymbol34.prev.___p03.Position.z) > (__ContextSymbol34.___p102.Position.z))) && (!(((__ContextSymbol34.prev.___p03) == (__ContextSymbol34.___p102))))))
.Select(__ContextSymbol35 => __ContextSymbol35.___p102)
.ToList<UnityPlanet>()).ToList<UnityPlanet>(), prev = __ContextSymbol32 })
.Where(__ContextSymbol36 => ((__ContextSymbol36.___other02.Count) == (0)))
.Select(__ContextSymbol37 => __ContextSymbol37.prev.___p03.Position.z)
.ToList<System.Single>()).ToList<System.Single>();
		List<System.Single> ___MaxPlanetPositionX00;
		___MaxPlanetPositionX00 = (

(___unityPlanets00).Select(__ContextSymbol38 => new { ___p04 = __ContextSymbol38 })
.Select(__ContextSymbol39 => new {___other03 = (

(___unityPlanets00).Select(__ContextSymbol40 => new { ___p103 = __ContextSymbol40,prev = __ContextSymbol39 })
.Where(__ContextSymbol41 => ((((__ContextSymbol41.___p103.Position.x) > (__ContextSymbol41.prev.___p04.Position.x))) && (!(((__ContextSymbol41.prev.___p04) == (__ContextSymbol41.___p103))))))
.Select(__ContextSymbol42 => __ContextSymbol42.___p103)
.ToList<UnityPlanet>()).ToList<UnityPlanet>(), prev = __ContextSymbol39 })
.Where(__ContextSymbol43 => ((__ContextSymbol43.___other03.Count) == (0)))
.Select(__ContextSymbol44 => __ContextSymbol44.prev.___p04.Position.x)
.ToList<System.Single>()).ToList<System.Single>();
		List<System.Single> ___MaxPlanetPositionZ00;
		___MaxPlanetPositionZ00 = (

(___unityPlanets00).Select(__ContextSymbol45 => new { ___p05 = __ContextSymbol45 })
.Select(__ContextSymbol46 => new {___other04 = (

(___unityPlanets00).Select(__ContextSymbol47 => new { ___p104 = __ContextSymbol47,prev = __ContextSymbol46 })
.Where(__ContextSymbol48 => ((((__ContextSymbol48.___p104.Position.z) > (__ContextSymbol48.prev.___p05.Position.z))) && (!(((__ContextSymbol48.prev.___p05) == (__ContextSymbol48.___p104))))))
.Select(__ContextSymbol49 => __ContextSymbol49.___p104)
.ToList<UnityPlanet>()).ToList<UnityPlanet>(), prev = __ContextSymbol46 })
.Where(__ContextSymbol50 => ((__ContextSymbol50.___other04.Count) == (0)))
.Select(__ContextSymbol51 => __ContextSymbol51.prev.___p05.Position.z)
.ToList<System.Single>()).ToList<System.Single>();
		MainCamera ___camera00;
		___camera00 = new MainCamera(___MinPlanetPositionX00.Head(),___MaxPlanetPositionX00.Head(),___MinPlanetPositionY00.Head(),___MinPlanetPositionZ00.Head(),___MaxPlanetPositionZ00.Head());
		List<Planet> ___planets00;
		___planets00 = (

(___unityPlanets00).Select(__ContextSymbol52 => new { ___p06 = __ContextSymbol52 })
.Select(__ContextSymbol53 => new {___owner00 = Utils.IfThenElse<Option<Commander>>((()=> ((__ContextSymbol53.___p06.IsStartingPlanet) && (((__ContextSymbol53.___p06.CommanderIndex) > (-1))))), (()=>	(new Just<Commander>(___commanders00[__ContextSymbol53.___p06.CommanderIndex]))
),(()=>	(new Nothing<Commander>())
)), prev = __ContextSymbol53 })
.Select(__ContextSymbol54 => new Planet(___constants00,__ContextSymbol54.prev.___p06,UnityRouting.getHopTable(__ContextSymbol54.prev.___p06,___unityPlanets00),__ContextSymbol54.___owner00))
.ToList<Planet>()).ToList<Planet>();
		List<Link> ___links100;
		___links100 = (

(___unityLinks00).Select(__ContextSymbol55 => new { ___l01 = __ContextSymbol55 })
.SelectMany(__ContextSymbol56=> (___planets00).Select(__ContextSymbol57 => new { ___p07 = __ContextSymbol57,
                                                      prev = __ContextSymbol56 })
.SelectMany(__ContextSymbol58=> (___planets00).Select(__ContextSymbol59 => new { ___q00 = __ContextSymbol59,
                                                      prev = __ContextSymbol58 })
.Where(__ContextSymbol60 => ((((__ContextSymbol60.prev.prev.___l01.startPlanet) == (__ContextSymbol60.prev.___p07.UnityPlanet))) && (((__ContextSymbol60.prev.prev.___l01.endPlanet) == (__ContextSymbol60.___q00.UnityPlanet)))))
.Select(__ContextSymbol61 => new Link(__ContextSymbol61.prev.prev.___l01,__ContextSymbol61.prev.___p07,__ContextSymbol61.___q00,false))
.ToList<Link>()))).ToList<Link>();
		List<Link> ___links200;
		___links200 = (

(___unityLinks00).Select(__ContextSymbol62 => new { ___l02 = __ContextSymbol62 })
.SelectMany(__ContextSymbol63=> (___planets00).Select(__ContextSymbol64 => new { ___p08 = __ContextSymbol64,
                                                      prev = __ContextSymbol63 })
.SelectMany(__ContextSymbol65=> (___planets00).Select(__ContextSymbol66 => new { ___q01 = __ContextSymbol66,
                                                      prev = __ContextSymbol65 })
.Where(__ContextSymbol67 => ((((__ContextSymbol67.prev.prev.___l02.startPlanet) == (__ContextSymbol67.prev.___p08.UnityPlanet))) && (((__ContextSymbol67.prev.prev.___l02.endPlanet) == (__ContextSymbol67.___q01.UnityPlanet)))))
.Select(__ContextSymbol68 => new Link(__ContextSymbol68.prev.prev.___l02,__ContextSymbol68.___q01,__ContextSymbol68.prev.___p08,true))
.ToList<Link>()))).ToList<Link>();
		List<Link> ___links300;
		___links300 = (___links100).Concat(___links200).ToList<Link>();
		List<Link> ___SSlinks00;
		___SSlinks00 = (

(___links300).Select(__ContextSymbol69 => new { ___link00 = __ContextSymbol69 })
.Where(__ContextSymbol70 => __ContextSymbol70.___link00.SSLink)
.Select(__ContextSymbol71 => __ContextSymbol71.___link00)
.ToList<Link>()).ToList<Link>();
		System.Collections.Generic.List<UnityStarSystem> ___star_systems00;
		___star_systems00 = UnityStarSystem.FindAllStarSystems();
		List<StarSystem> ___casanova_star_systems00;
		___casanova_star_systems00 = (

(___star_systems00).Select(__ContextSymbol72 => new { ___ss00 = __ContextSymbol72 })
.Select(__ContextSymbol73 => new StarSystem(__ContextSymbol73.___ss00,___planets00,___links300))
.ToList<StarSystem>()).ToList<StarSystem>();
		TargetedPlanet = (new Nothing<Planet>());
		StarSystems = ___casanova_star_systems00;
		Ships = (

Enumerable.Empty<Ship>()).ToList<Ship>();
		SelectionManager = new SelectionManager();
		SelectedPlanets = (

Enumerable.Empty<Planet>()).ToList<Planet>();
		ResourceBar = new ResourceBar(___commanders00);
		Planets = ___planets00;
		MiniMap = new MiniMap(___MinPlanetPositionX00.Head(),___MinPlanetPositionY00.Head(),___MinPlanetPositionZ00.Head(),___MaxPlanetPositionX00.Head(),___MaxPlanetPositionZ00.Head());
		MainCamera = ___camera00;
		Links = ___SSlinks00;
		InputMonitor = new InputController();
		Constants = ___constants00;
		Commanders = ___commanders00;
		AutoHopManager = (new Nothing<AutoHopManager>());
		AllLinks = ___links300;
		
}
		public List<Link> __AllLinks;
	public List<Link> AllLinks{  get { return  __AllLinks; }
  set{ __AllLinks = value;
 foreach(var e in value){if(e.JustEntered){ e.JustEntered = false;
}
} }
 }
	public Option<AutoHopManager> AutoHopManager;
	public List<Commander> Commanders;
	public GameConstants Constants;
	public InputController __InputMonitor;
	public InputController InputMonitor{  get { return  __InputMonitor; }
  set{ __InputMonitor = value;
 if(!value.JustEntered) __InputMonitor = value; 
 else{ value.JustEntered = false;
}
 }
 }
	public List<Link> __Links;
	public List<Link> Links{  get { return  __Links; }
  set{ __Links = value;
 foreach(var e in value){if(e.JustEntered){ e.JustEntered = false;
}
} }
 }
	public MainCamera MainCamera;
	public MiniMap MiniMap;
	public List<Planet> __Planets;
	public List<Planet> Planets{  get { return  __Planets; }
  set{ __Planets = value;
 foreach(var e in value){if(e.JustEntered){ e.JustEntered = false;
}
} }
 }
	public ResourceBar ResourceBar;
	public List<Planet> __SelectedPlanets;
	public List<Planet> SelectedPlanets{  get { return  __SelectedPlanets; }
  set{ __SelectedPlanets = value;
 foreach(var e in value){if(e.JustEntered){ e.JustEntered = false;
}
} }
 }
	public SelectionManager __SelectionManager;
	public SelectionManager SelectionManager{  get { return  __SelectionManager; }
  set{ __SelectionManager = value;
 if(!value.JustEntered) __SelectionManager = value; 
 else{ value.JustEntered = false;
}
 }
 }
	public List<Ship> Ships;
	public List<StarSystem> StarSystems;
	public Option<Planet> __TargetedPlanet;
	public Option<Planet> TargetedPlanet{  get { return  __TargetedPlanet; }
  set{ __TargetedPlanet = value;
 if(value.IsSome){if(!value.Value.JustEntered) __TargetedPlanet = value; 
 else{ value.Value.JustEntered = false;
}
} }
 }
	public AutoHopManager ___z00;
	public List<Planet> ___x10;

System.DateTime init_time = System.DateTime.Now;
	public void Update(float dt, World world) {
var t = System.DateTime.Now;		this.Rule2(dt, world);

if(AutoHopManager.IsSome){ 		AutoHopManager.Value.Update(dt, world);
 } 
		for(int x0 = 0; x0 < Commanders.Count; x0++) { 
			Commanders[x0].Update(dt, world);
		}
		Constants.Update(dt, world);
		InputMonitor.Update(dt, world);
		for(int x0 = 0; x0 < Links.Count; x0++) { 
			Links[x0].Update(dt, world);
		}
		MainCamera.Update(dt, world);
		MiniMap.Update(dt, world);
		for(int x0 = 0; x0 < Planets.Count; x0++) { 
			Planets[x0].Update(dt, world);
		}
		ResourceBar.Update(dt, world);
		SelectionManager.Update(dt, world);
		for(int x0 = 0; x0 < Ships.Count; x0++) { 
			Ships[x0].Update(dt, world);
		}
		for(int x0 = 0; x0 < StarSystems.Count; x0++) { 
			StarSystems[x0].Update(dt, world);
		}
		this.Rule0(dt, world);
		this.Rule1(dt, world);
	}

	public void Rule2(float dt, World world) 
	{
	SelectedPlanets = (

(Planets).Select(__ContextSymbol76 => new { ___p20 = __ContextSymbol76 })
.Where(__ContextSymbol77 => __ContextSymbol77.___p20.Selected)
.Select(__ContextSymbol78 => __ContextSymbol78.___p20)
.ToList<Planet>()).ToList<Planet>();
	}
	




	int s0=-1;
	public void Rule0(float dt, World world){ 
	switch (s0)
	{

	case -1:
	if(!(((InputMonitor.ShiftKey) && (InputMonitor.RightMouseButton))))
	{

	s0 = -1;
return;	}else
	{

	goto case 6;	}
	case 6:
	AutoHopManager = AutoHopManager;
	s0 = 0;
return;
	case 0:
	if(((((SelectedPlanets.Count) > (0))) && (TargetedPlanet.IsSome)))
	{

	goto case 1;	}else
	{

	s0 = -1;
return;	}
	case 1:
	___z00 = new AutoHopManager(SelectedPlanets,TargetedPlanet.Value);
	AutoHopManager = (new Just<AutoHopManager>(___z00));
	s0 = 3;
return;
	case 3:
	if(!(AutoHopManager.Value.IsDone))
	{

	s0 = 3;
return;	}else
	{

	goto case 2;	}
	case 2:
	AutoHopManager = (new Nothing<AutoHopManager>());
	s0 = -1;
return;	
	default: return;}}
	

	int s1=-1;
	public void Rule1(float dt, World world){ 
	switch (s1)
	{

	case -1:
	___x10 = (

(Planets).Select(__ContextSymbol79 => new { ___a10 = __ContextSymbol79 })
.Where(__ContextSymbol80 => __ContextSymbol80.___a10.Targeted)
.Select(__ContextSymbol81 => __ContextSymbol81.___a10)
.ToList<Planet>()).ToList<Planet>();
	if(((___x10.Count) > (0)))
	{

	goto case 8;	}else
	{

	goto case 9;	}
	case 8:
	TargetedPlanet = (new Just<Planet>(___x10.Head()));
	s1 = 11;
return;
	case 11:
	TargetedPlanet = TargetedPlanet;
	s1 = -1;
return;
	case 9:
	TargetedPlanet = (new Nothing<Planet>());
	s1 = -1;
return;	
	default: return;}}
	





}
public class InputController{
public int frame;
public bool JustEntered = true;
	public int ID;
public InputController()
	{JustEntered = false;
 frame = World.frame;
		UnityController = new UnityController();
		
}
		public System.Boolean A{  get { return UnityController.A; }
 }
	public System.Boolean Alpha0{  get { return UnityController.Alpha0; }
 }
	public System.Boolean Alpha1{  get { return UnityController.Alpha1; }
 }
	public System.Boolean Alpha2{  get { return UnityController.Alpha2; }
 }
	public System.Boolean Alpha3{  get { return UnityController.Alpha3; }
 }
	public System.Boolean Alpha4{  get { return UnityController.Alpha4; }
 }
	public System.Boolean Alpha5{  get { return UnityController.Alpha5; }
 }
	public System.Boolean Alpha6{  get { return UnityController.Alpha6; }
 }
	public System.Boolean Alpha7{  get { return UnityController.Alpha7; }
 }
	public System.Boolean Alpha8{  get { return UnityController.Alpha8; }
 }
	public System.Boolean Alpha9{  get { return UnityController.Alpha9; }
 }
	public System.Boolean ControlKey{  get { return UnityController.ControlKey; }
 }
	public System.Boolean D{  get { return UnityController.D; }
 }
	public System.Boolean DownArrow{  get { return UnityController.DownArrow; }
 }
	public System.Boolean LeftArrow{  get { return UnityController.LeftArrow; }
 }
	public System.Boolean LeftMouseButton{  get { return UnityController.LeftMouseButton; }
 }
	public System.Boolean MinusKey{  get { return UnityController.MinusKey; }
 }
	public UnityEngine.Vector3 MousePositionInWorld{  get { return UnityController.MousePositionInWorld; }
 }
	public UnityEngine.Vector3 MousePositionOnScreen{  get { return UnityController.MousePositionOnScreen; }
 }
	public System.Boolean MouseScrollDown{  get { return UnityController.MouseScrollDown; }
 }
	public System.Boolean MouseScrollUp{  get { return UnityController.MouseScrollUp; }
 }
	public System.Boolean PlusKey{  get { return UnityController.PlusKey; }
 }
	public System.Boolean RightArrow{  get { return UnityController.RightArrow; }
 }
	public System.Boolean RightMouseButton{  get { return UnityController.RightMouseButton; }
 }
	public System.Boolean RightMouseButtonUp{  get { return UnityController.RightMouseButtonUp; }
 }
	public System.Boolean S{  get { return UnityController.S; }
 }
	public System.Boolean ShiftKey{  get { return UnityController.ShiftKey; }
 }
	public System.Boolean Tab{  get { return UnityController.Tab; }
 }
	public UnityController UnityController;
	public System.Boolean UpArrow{  get { return UnityController.UpArrow; }
 }
	public System.Boolean W{  get { return UnityController.W; }
 }
	public UnityEngine.Animation animation{  get { return UnityController.animation; }
 }
	public UnityEngine.AudioSource audio{  get { return UnityController.audio; }
 }
	public UnityEngine.Camera camera{  get { return UnityController.camera; }
 }
	public UnityEngine.Collider collider{  get { return UnityController.collider; }
 }
	public UnityEngine.Collider2D collider2D{  get { return UnityController.collider2D; }
 }
	public UnityEngine.ConstantForce constantForce{  get { return UnityController.constantForce; }
 }
	public System.Boolean enabled{  get { return UnityController.enabled; }
  set{UnityController.enabled = value; }
 }
	public UnityEngine.GameObject gameObject{  get { return UnityController.gameObject; }
 }
	public UnityEngine.GUIElement guiElement{  get { return UnityController.guiElement; }
 }
	public UnityEngine.GUIText guiText{  get { return UnityController.guiText; }
 }
	public UnityEngine.GUITexture guiTexture{  get { return UnityController.guiTexture; }
 }
	public UnityEngine.HideFlags hideFlags{  get { return UnityController.hideFlags; }
  set{UnityController.hideFlags = value; }
 }
	public UnityEngine.HingeJoint hingeJoint{  get { return UnityController.hingeJoint; }
 }
	public UnityEngine.Light light{  get { return UnityController.light; }
 }
	public System.String name{  get { return UnityController.name; }
  set{UnityController.name = value; }
 }
	public UnityEngine.ParticleEmitter particleEmitter{  get { return UnityController.particleEmitter; }
 }
	public UnityEngine.ParticleSystem particleSystem{  get { return UnityController.particleSystem; }
 }
	public UnityEngine.Renderer renderer{  get { return UnityController.renderer; }
 }
	public UnityEngine.Rigidbody rigidbody{  get { return UnityController.rigidbody; }
 }
	public UnityEngine.Rigidbody2D rigidbody2D{  get { return UnityController.rigidbody2D; }
 }
	public System.String tag{  get { return UnityController.tag; }
  set{UnityController.tag = value; }
 }
	public UnityEngine.Transform transform{  get { return UnityController.transform; }
 }
	public System.Boolean useGUILayout{  get { return UnityController.useGUILayout; }
  set{UnityController.useGUILayout = value; }
 }
	public void Update(float dt, World world) {
frame = World.frame;



	}











}
public class SelectionManager{
public int frame;
public bool JustEntered = true;
	public int ID;
public SelectionManager()
	{JustEntered = false;
 frame = World.frame;
		SelectionRectangleToDestroy = (new Nothing<SelectionRectangle>());
		SelectionRectangle = (new Nothing<SelectionRectangle>());
		SelectAll = false;
		
}
		public System.Boolean SelectAll;
	public Option<SelectionRectangle> SelectionRectangle;
	public Option<SelectionRectangle> SelectionRectangleToDestroy;
	public UnityEngine.Vector3 ___p19;
	public System.Single count_down1;
	public Option<SelectionRectangle> ___rect10;
	public void Update(float dt, World world) {
frame = World.frame;

if(SelectionRectangle.IsSome){ 		SelectionRectangle.Value.Update(dt, world);
 } 
		this.Rule0(dt, world);
		this.Rule1(dt, world);
	}





	int s0=-1;
	public void Rule0(float dt, World world){ 
	switch (s0)
	{

	case -1:
	if(!(SelectionRectangleToDestroy.IsSome))
	{

	s0 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	SelectionRectangleToDestroy.Value.Destroyed = true;
	s0 = -1;
return;	
	default: return;}}
	

	int s1=-1;
	public void Rule1(float dt, World world){ 
	switch (s1)
	{

	case -1:
	if(!(((SelectionRectangle.IsNone) && (UnityEngine.Input.GetMouseButton(0)))))
	{

	s1 = -1;
return;	}else
	{

	goto case 10;	}
	case 10:
	___p19 = world.InputMonitor.MousePositionInWorld;
	count_down1 = 0.1f;
	goto case 9;
	case 9:
	if(((count_down1) > (0f)))
	{

	count_down1 = ((count_down1) - (dt));
	s1 = 9;
return;	}else
	{

	goto case 0;	}
	case 0:
	if(((SelectionRectangle.IsNone) && (UnityEngine.Input.GetMouseButton(0))))
	{

	goto case 1;	}else
	{

	s1 = -1;
return;	}
	case 1:
	SelectionRectangle = (new Just<SelectionRectangle>(new SelectionRectangle(world,___p19)));
	SelectionRectangleToDestroy = (new Nothing<SelectionRectangle>());
	SelectAll = false;
	s1 = 6;
return;
	case 6:
	if(!(UnityEngine.Input.GetMouseButtonUp(0)))
	{

	s1 = 6;
return;	}else
	{

	goto case 5;	}
	case 5:
	___rect10 = SelectionRectangle;
	SelectionRectangle = SelectionRectangle;
	SelectionRectangleToDestroy = (new Nothing<SelectionRectangle>());
	SelectAll = true;
	s1 = 3;
return;
	case 3:
	SelectionRectangle = (new Nothing<SelectionRectangle>());
	SelectionRectangleToDestroy = ___rect10;
	SelectAll = false;
	s1 = 2;
return;
	case 2:
	SelectionRectangle = (new Nothing<SelectionRectangle>());
	SelectionRectangleToDestroy = (new Nothing<SelectionRectangle>());
	SelectAll = false;
	s1 = -1;
return;	
	default: return;}}
	





}
public class SelectionRectangle{
public int frame;
public bool JustEntered = true;
private World world;
private UnityEngine.Vector3 StartP;
	public int ID;
public SelectionRectangle(World world, UnityEngine.Vector3 StartP)
	{JustEntered = false;
 frame = World.frame;
		UnitySelectionRectangle = UnitySelectionRectangle.Instantiate(StartP);
		StartPosition = StartP;
		EndPosition = StartP;
		
}
		public System.Boolean Destroyed{  get { return UnitySelectionRectangle.Destroyed; }
  set{UnitySelectionRectangle.Destroyed = value; }
 }
	public UnityEngine.Vector3 EndPosition;
	public UnityEngine.Vector3 Position{  get { return UnitySelectionRectangle.Position; }
  set{UnitySelectionRectangle.Position = value; }
 }
	public UnityEngine.Vector3 Scale{  get { return UnitySelectionRectangle.Scale; }
  set{UnitySelectionRectangle.Scale = value; }
 }
	public UnityEngine.Vector3 StartPosition;
	public UnitySelectionRectangle UnitySelectionRectangle;
	public UnityEngine.Animation animation{  get { return UnitySelectionRectangle.animation; }
 }
	public UnityEngine.AudioSource audio{  get { return UnitySelectionRectangle.audio; }
 }
	public UnityEngine.Camera camera{  get { return UnitySelectionRectangle.camera; }
 }
	public UnityEngine.Collider collider{  get { return UnitySelectionRectangle.collider; }
 }
	public UnityEngine.Collider2D collider2D{  get { return UnitySelectionRectangle.collider2D; }
 }
	public UnityEngine.ConstantForce constantForce{  get { return UnitySelectionRectangle.constantForce; }
 }
	public System.Boolean enabled{  get { return UnitySelectionRectangle.enabled; }
  set{UnitySelectionRectangle.enabled = value; }
 }
	public UnityEngine.GameObject gameObject{  get { return UnitySelectionRectangle.gameObject; }
 }
	public UnityEngine.GUIElement guiElement{  get { return UnitySelectionRectangle.guiElement; }
 }
	public UnityEngine.GUIText guiText{  get { return UnitySelectionRectangle.guiText; }
 }
	public UnityEngine.GUITexture guiTexture{  get { return UnitySelectionRectangle.guiTexture; }
 }
	public UnityEngine.HideFlags hideFlags{  get { return UnitySelectionRectangle.hideFlags; }
  set{UnitySelectionRectangle.hideFlags = value; }
 }
	public UnityEngine.HingeJoint hingeJoint{  get { return UnitySelectionRectangle.hingeJoint; }
 }
	public UnityEngine.Light light{  get { return UnitySelectionRectangle.light; }
 }
	public System.String name{  get { return UnitySelectionRectangle.name; }
  set{UnitySelectionRectangle.name = value; }
 }
	public UnityEngine.ParticleEmitter particleEmitter{  get { return UnitySelectionRectangle.particleEmitter; }
 }
	public UnityEngine.ParticleSystem particleSystem{  get { return UnitySelectionRectangle.particleSystem; }
 }
	public UnityEngine.Renderer renderer{  get { return UnitySelectionRectangle.renderer; }
 }
	public UnityEngine.Rigidbody rigidbody{  get { return UnitySelectionRectangle.rigidbody; }
 }
	public UnityEngine.Rigidbody2D rigidbody2D{  get { return UnitySelectionRectangle.rigidbody2D; }
 }
	public System.String tag{  get { return UnitySelectionRectangle.tag; }
  set{UnitySelectionRectangle.tag = value; }
 }
	public UnityEngine.Transform transform{  get { return UnitySelectionRectangle.transform; }
 }
	public System.Boolean useGUILayout{  get { return UnitySelectionRectangle.useGUILayout; }
  set{UnitySelectionRectangle.useGUILayout = value; }
 }
	public System.Single ___StartX10;
	public System.Single ___DestinationX10;
	public System.Single ___StartZ10;
	public System.Single ___DestinationZ10;
	public UnityEngine.Vector3 ___NewP10;
	public UnityEngine.Vector3 ___NewS10;
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
	EndPosition = world.InputMonitor.MousePositionInWorld;
	s0 = -1;
return;	
	default: return;}}
	

	int s1=-1;
	public void Rule1(float dt, World world){ 
	switch (s1)
	{

	case -1:
	if(((StartPosition.x) > (EndPosition.x)))
	{

	___StartX10 = EndPosition.x;	}else
	{

	___StartX10 = StartPosition.x;	}
	if(((StartPosition.x) > (EndPosition.x)))
	{

	___DestinationX10 = StartPosition.x;	}else
	{

	___DestinationX10 = EndPosition.x;	}
	if(((StartPosition.z) > (EndPosition.z)))
	{

	___StartZ10 = EndPosition.z;	}else
	{

	___StartZ10 = StartPosition.z;	}
	if(((StartPosition.z) > (EndPosition.z)))
	{

	___DestinationZ10 = StartPosition.z;	}else
	{

	___DestinationZ10 = EndPosition.z;	}
	___NewP10 = new UnityEngine.Vector3((((___DestinationX10) - (___StartX10)) / (2f)) + (___StartX10),Position.y,(((___DestinationZ10) - (___StartZ10)) / (2f)) + (___StartZ10));
	___NewS10 = new UnityEngine.Vector3((___DestinationX10) - (___StartX10),Scale.y,(___DestinationZ10) - (___StartZ10));
	Position = ___NewP10;
	Scale = ___NewS10;
	s1 = -1;
return;	
	default: return;}}
	





}
public class MiniMap{
public int frame;
public bool JustEntered = true;
private System.Single Xmin;
private System.Single Ymin;
private System.Single Zmin;
private System.Single Xmax;
private System.Single Zmax;
	public int ID;
public MiniMap(System.Single Xmin, System.Single Ymin, System.Single Zmin, System.Single Xmax, System.Single Zmax)
	{JustEntered = false;
 frame = World.frame;
		cameraSizeOffset = 4f;
		UnityMiniMap = UnityMiniMap.CreateMiniMapCamera();
		MinZ = Zmin;
		MinY = Ymin;
		MinX = Xmin;
		MaxZ = Zmax;
		MaxX = Xmax;
		
}
		public UnityEngine.Vector3 CameraPosition{  get { return UnityMiniMap.CameraPosition; }
  set{UnityMiniMap.CameraPosition = value; }
 }
	public System.Single CameraSize{  get { return UnityMiniMap.CameraSize; }
  set{UnityMiniMap.CameraSize = value; }
 }
	public System.Boolean Hide{  get { return UnityMiniMap.Hide; }
  set{UnityMiniMap.Hide = value; }
 }
	public System.Single MaxX;
	public System.Single MaxZ;
	public System.Single MinX;
	public System.Single MinY;
	public System.Single MinZ;
	public UnityMiniMap UnityMiniMap;
	public UnityEngine.Animation animation{  get { return UnityMiniMap.animation; }
 }
	public UnityEngine.AudioSource audio{  get { return UnityMiniMap.audio; }
 }
	public UnityEngine.Camera camera{  get { return UnityMiniMap.camera; }
 }
	public System.Single cameraSizeOffset;
	public UnityEngine.Collider collider{  get { return UnityMiniMap.collider; }
 }
	public UnityEngine.Collider2D collider2D{  get { return UnityMiniMap.collider2D; }
 }
	public UnityEngine.ConstantForce constantForce{  get { return UnityMiniMap.constantForce; }
 }
	public System.Boolean enabled{  get { return UnityMiniMap.enabled; }
  set{UnityMiniMap.enabled = value; }
 }
	public UnityEngine.GameObject gameObject{  get { return UnityMiniMap.gameObject; }
 }
	public UnityEngine.GUIElement guiElement{  get { return UnityMiniMap.guiElement; }
 }
	public UnityEngine.GUIText guiText{  get { return UnityMiniMap.guiText; }
 }
	public UnityEngine.GUITexture guiTexture{  get { return UnityMiniMap.guiTexture; }
 }
	public UnityEngine.HideFlags hideFlags{  get { return UnityMiniMap.hideFlags; }
  set{UnityMiniMap.hideFlags = value; }
 }
	public UnityEngine.HingeJoint hingeJoint{  get { return UnityMiniMap.hingeJoint; }
 }
	public UnityEngine.Light light{  get { return UnityMiniMap.light; }
 }
	public System.String name{  get { return UnityMiniMap.name; }
  set{UnityMiniMap.name = value; }
 }
	public UnityEngine.ParticleEmitter particleEmitter{  get { return UnityMiniMap.particleEmitter; }
 }
	public UnityEngine.ParticleSystem particleSystem{  get { return UnityMiniMap.particleSystem; }
 }
	public UnityEngine.Renderer renderer{  get { return UnityMiniMap.renderer; }
 }
	public UnityEngine.Rigidbody rigidbody{  get { return UnityMiniMap.rigidbody; }
 }
	public UnityEngine.Rigidbody2D rigidbody2D{  get { return UnityMiniMap.rigidbody2D; }
 }
	public System.String tag{  get { return UnityMiniMap.tag; }
  set{UnityMiniMap.tag = value; }
 }
	public UnityEngine.Transform transform{  get { return UnityMiniMap.transform; }
 }
	public System.Boolean useGUILayout{  get { return UnityMiniMap.useGUILayout; }
  set{UnityMiniMap.useGUILayout = value; }
 }
	public System.Single ___midX10;
	public System.Single ___midY10;
	public System.Single ___midZ10;
	public UnityEngine.Vector3 ___position10;
	public System.Single ___width10;
	public System.Single ___height10;
	public System.Single ___SizeW10;
	public System.Single ___SizeH10;
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
	if(!(world.InputMonitor.Tab))
	{

	s0 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	Hide = !(Hide);
	s0 = -1;
return;	
	default: return;}}
	

	int s1=-1;
	public void Rule1(float dt, World world){ 
	switch (s1)
	{

	case -1:
	___midX10 = ((((MinX) + (MaxX))) / (2f));
	___midY10 = ((MinY) + (2f));
	___midZ10 = ((((MinZ) + (MaxZ))) / (2f));
	___position10 = new UnityEngine.Vector3(___midX10,___midY10,___midZ10);
	___width10 = ((MaxX) - (MinX));
	___height10 = ((MaxZ) - (MinZ));
	___SizeW10 = ((___width10) / (2f));
	___SizeH10 = ((___height10) / (2f));
	if(((___SizeW10) > (___SizeH10)))
	{

	goto case 2;	}else
	{

	goto case 3;	}
	case 2:
	CameraSize = ((___SizeW10) + (cameraSizeOffset));
	CameraPosition = ___position10;
	s1 = -1;
return;
	case 3:
	CameraSize = ((___SizeH10) + (cameraSizeOffset));
	CameraPosition = ___position10;
	s1 = -1;
return;	
	default: return;}}
	





}
public class MainCamera{
public int frame;
public bool JustEntered = true;
private System.Single minX;
private System.Single maxX;
private System.Single minY;
private System.Single minZ;
private System.Single maxZ;
	public int ID;
public MainCamera(System.Single minX, System.Single maxX, System.Single minY, System.Single minZ, System.Single maxZ)
	{JustEntered = false;
 frame = World.frame;
		System.Single ___maxY00;
		___maxY00 = (MinY) + (80f);
		UnityEngine.Vector2 ___MinX00;
		___MinX00 = new UnityEngine.Vector2(MinX,0f);
		UnityEngine.Vector2 ___MaxX00;
		___MaxX00 = new UnityEngine.Vector2(MaxX,0f);
		System.Single ___xDistance00;
		___xDistance00 = UnityEngine.Vector2.Distance(___MinX00,___MaxX00);
		System.Single ___cameraX00;
		___cameraX00 = (___MinX00.x) + ((___xDistance00) / (2f));
		UnityEngine.Vector2 ___MinZ00;
		___MinZ00 = new UnityEngine.Vector2(MinZ,0f);
		UnityEngine.Vector2 ___MaxZ00;
		___MaxZ00 = new UnityEngine.Vector2(MaxZ,0f);
		System.Single ___zDistance00;
		___zDistance00 = UnityEngine.Vector2.Distance(___MinZ00,___MaxZ00);
		System.Single ___cameraZ00;
		___cameraZ00 = (___MinZ00.x) + ((___zDistance00) / (2f));
		ZoomSensitivity = 10;
		ZoomLevel = 3;
		ZoomFactor = 0f;
		UnityCamera = UnityCamera.CreateMainCamera(new UnityEngine.Vector3(___cameraX00,___maxY00,___cameraZ00));
		ScreenWidth = Screen.width;
		ScreenHeight = Screen.height;
		RotationDegree = 7f;
		MinZ = minZ;
		MinY = (minY) + (5f);
		MinX = minX;
		MaxZoomLevel = 3;
		MaxZ = maxZ;
		MaxY = ___maxY00;
		MaxX = maxX;
		IsZooming = false;
		CameraMovementSensitivity = 0.25f;
		BoundaryY = (ScreenHeight) / (25);
		BoundaryX = (ScreenWidth) / (25);
		
}
		public System.Int32 BoundaryX;
	public System.Int32 BoundaryY;
	public System.Single CameraMovementSensitivity;
	public System.Single CameraSize{  get { return UnityCamera.CameraSize; }
  set{UnityCamera.CameraSize = value; }
 }
	public UnityEngine.Vector3 Forward{  get { return UnityCamera.Forward; }
  set{UnityCamera.Forward = value; }
 }
	public System.Boolean Hide{  get { return UnityCamera.Hide; }
  set{UnityCamera.Hide = value; }
 }
	public System.Boolean IsZooming;
	public System.Single MaxX;
	public System.Single MaxY;
	public System.Single MaxZ;
	public System.Int32 MaxZoomLevel;
	public System.Single MinX;
	public System.Single MinY;
	public System.Single MinZ;
	public UnityEngine.Vector3 Position{  get { return UnityCamera.Position; }
  set{UnityCamera.Position = value; }
 }
	public UnityEngine.Vector3 Right{  get { return UnityCamera.Right; }
  set{UnityCamera.Right = value; }
 }
	public UnityEngine.Vector3 Rotation{  get { return UnityCamera.Rotation; }
  set{UnityCamera.Rotation = value; }
 }
	public System.Single RotationDegree;
	public System.Int32 ScreenHeight;
	public System.Int32 ScreenWidth;
	public UnityCamera UnityCamera;
	public UnityEngine.Vector3 Up{  get { return UnityCamera.Up; }
  set{UnityCamera.Up = value; }
 }
	public System.Single ZoomFactor;
	public System.Int32 ZoomLevel;
	public System.Int32 ZoomSensitivity;
	public UnityEngine.Animation animation{  get { return UnityCamera.animation; }
 }
	public UnityEngine.AudioSource audio{  get { return UnityCamera.audio; }
 }
	public UnityEngine.Camera camera{  get { return UnityCamera.camera; }
 }
	public UnityEngine.Collider collider{  get { return UnityCamera.collider; }
 }
	public UnityEngine.Collider2D collider2D{  get { return UnityCamera.collider2D; }
 }
	public UnityEngine.ConstantForce constantForce{  get { return UnityCamera.constantForce; }
 }
	public System.Boolean enabled{  get { return UnityCamera.enabled; }
  set{UnityCamera.enabled = value; }
 }
	public UnityEngine.GameObject gameObject{  get { return UnityCamera.gameObject; }
 }
	public UnityEngine.GUIElement guiElement{  get { return UnityCamera.guiElement; }
 }
	public UnityEngine.GUIText guiText{  get { return UnityCamera.guiText; }
 }
	public UnityEngine.GUITexture guiTexture{  get { return UnityCamera.guiTexture; }
 }
	public UnityEngine.HideFlags hideFlags{  get { return UnityCamera.hideFlags; }
  set{UnityCamera.hideFlags = value; }
 }
	public UnityEngine.HingeJoint hingeJoint{  get { return UnityCamera.hingeJoint; }
 }
	public UnityEngine.Light light{  get { return UnityCamera.light; }
 }
	public System.String name{  get { return UnityCamera.name; }
  set{UnityCamera.name = value; }
 }
	public UnityEngine.ParticleEmitter particleEmitter{  get { return UnityCamera.particleEmitter; }
 }
	public UnityEngine.ParticleSystem particleSystem{  get { return UnityCamera.particleSystem; }
 }
	public UnityEngine.Renderer renderer{  get { return UnityCamera.renderer; }
 }
	public UnityEngine.Rigidbody rigidbody{  get { return UnityCamera.rigidbody; }
 }
	public UnityEngine.Rigidbody2D rigidbody2D{  get { return UnityCamera.rigidbody2D; }
 }
	public System.String tag{  get { return UnityCamera.tag; }
  set{UnityCamera.tag = value; }
 }
	public UnityEngine.Transform transform{  get { return UnityCamera.transform; }
 }
	public System.Boolean useGUILayout{  get { return UnityCamera.useGUILayout; }
  set{UnityCamera.useGUILayout = value; }
 }
	public System.Single ___levelLength00;
	public System.Single ___nextY00;
	public System.Single ___prevRotation00;
	public System.Single ___prevY00;
	public System.Single ___nextRotation00;
	public System.Single ___newRotation00;
	public System.Single ___newY00;
	public System.Single ___levelLength01;
	public System.Single ___nextY01;
	public System.Single ___prevY01;
	public System.Single ___prevRotation01;
	public System.Single ___nextRotation01;
	public System.Single ___newRotation01;
	public System.Single ___newY01;
	public UnityEngine.Vector3 ___newPosition10;
	public System.Single ___biggestFloat10;
	public UnityEngine.Vector3 ___newPosition11;
	public System.Single ___biggestFloat11;
	public UnityEngine.Vector3 ___newPosition12;
	public System.Single ___biggestFloat12;
	public UnityEngine.Vector3 ___newPosition13;
	public System.Single ___biggestFloat13;
	public System.Single ___adjustment10;
	public System.Single ___adjustment11;
	public System.Single ___adjustment12;
	public System.Single ___adjustment13;
	public void Update(float dt, World world) {
frame = World.frame;

		this.Rule0(dt, world);
		this.Rule1(dt, world);
	}



	int s000=-1;
	public void parallelMethod000(float dt, World world){ 
	switch (s000)
	{

	case -1:
	if(!(((((!(IsZooming)) && (((3) > (ZoomLevel))))) && (((world.InputMonitor.MinusKey) || (world.InputMonitor.MouseScrollDown))))))
	{

	s000 = -1;
return;	}else
	{

	goto case 11;	}
	case 11:
	Position = Position;
	ZoomLevel = ((ZoomLevel) + (1));
	ZoomFactor = 0f;
	IsZooming = true;
	Rotation = Rotation;
	s000 = 10;
return;
	case 10:
	___levelLength00 = ((((MaxY) - (MinY))) / (((System.Single)MaxZoomLevel)));
	___nextY00 = ((((___levelLength00) * (ZoomLevel))) + (MinY));
	___prevRotation00 = Rotation.x;
	___prevY00 = Position.y;
	___nextRotation00 = ((Rotation.x) + (RotationDegree));
	goto case 1;
	case 1:
	if(!(((1f) > (ZoomFactor))))
	{

	goto case 0;	}else
	{

	goto case 2;	}
	case 2:
	___newRotation00 = UnityEngine.Mathf.Lerp(___prevRotation00,___nextRotation00,ZoomFactor);
	___newY00 = UnityEngine.Mathf.Lerp(___prevY00,___nextY00,ZoomFactor);
	Position = new UnityEngine.Vector3(Position.x,___newY00,Position.z);
	ZoomLevel = ZoomLevel;
	ZoomFactor = ((ZoomFactor) + (((dt) * (ZoomSensitivity))));
	IsZooming = IsZooming;
	Rotation = new UnityEngine.Vector3(___newRotation00,Rotation.y,Rotation.z);
	s000 = 1;
return;
	case 0:
	Position = new UnityEngine.Vector3(Position.x,___nextY00,Position.z);
	ZoomLevel = ZoomLevel;
	ZoomFactor = ZoomFactor;
	IsZooming = false;
	Rotation = new UnityEngine.Vector3(___nextRotation00,Rotation.y,Rotation.z);
	s000 = -1;
return;	
	default: return;}}
	

	int s001=-1;
	public void parallelMethod001(float dt, World world){ 
	switch (s001)
	{

	case -1:
	if(!(((((!(IsZooming)) && (((ZoomLevel) > (0))))) && (((world.InputMonitor.PlusKey) || (world.InputMonitor.MouseScrollUp))))))
	{

	s001 = -1;
return;	}else
	{

	goto case 11;	}
	case 11:
	Position = Position;
	ZoomLevel = ((ZoomLevel) - (1));
	ZoomFactor = 0f;
	IsZooming = true;
	Rotation = Rotation;
	s001 = 10;
return;
	case 10:
	___levelLength01 = ((((MaxY) - (MinY))) / (((System.Single)MaxZoomLevel)));
	___nextY01 = ((((___levelLength01) * (ZoomLevel))) + (MinY));
	___prevY01 = Position.y;
	___prevRotation01 = Rotation.x;
	___nextRotation01 = ((Rotation.x) - (RotationDegree));
	goto case 1;
	case 1:
	if(!(((1f) > (ZoomFactor))))
	{

	goto case 0;	}else
	{

	goto case 2;	}
	case 2:
	___newRotation01 = UnityEngine.Mathf.Lerp(___prevRotation01,___nextRotation01,ZoomFactor);
	___newY01 = UnityEngine.Mathf.Lerp(___prevY01,___nextY01,ZoomFactor);
	Position = new UnityEngine.Vector3(Position.x,___newY01,Position.z);
	ZoomLevel = ZoomLevel;
	ZoomFactor = ((ZoomFactor) + (((dt) * (ZoomSensitivity))));
	IsZooming = IsZooming;
	Rotation = new UnityEngine.Vector3(___newRotation01,Rotation.y,Rotation.z);
	s001 = 1;
return;
	case 0:
	Position = new UnityEngine.Vector3(Position.x,___nextY01,Position.z);
	ZoomLevel = ZoomLevel;
	ZoomFactor = ZoomFactor;
	IsZooming = false;
	Rotation = new UnityEngine.Vector3(___nextRotation01,Rotation.y,Rotation.z);
	s001 = -1;
return;	
	default: return;}}
	

	int s110=-1;
	public void parallelMethod110(float dt, World world){ 
	switch (s110)
	{

	case -1:
	if(!(world.InputMonitor.W))
	{

	s110 = -1;
return;	}else
	{

	goto case 2;	}
	case 2:
	___newPosition10 = ((Position) + (((new UnityEngine.Vector3(0f,0f,10f)) * (dt))));
	___biggestFloat10 = UnityEngine.Mathf.Min(MaxZ,___newPosition10.z);
	Position = new UnityEngine.Vector3(___newPosition10.x,___newPosition10.y,___biggestFloat10);
	s110 = -1;
return;	
	default: return;}}
	

	int s111=-1;
	public void parallelMethod111(float dt, World world){ 
	switch (s111)
	{

	case -1:
	if(!(world.InputMonitor.S))
	{

	s111 = -1;
return;	}else
	{

	goto case 2;	}
	case 2:
	___newPosition11 = ((Position) - (((new UnityEngine.Vector3(0f,0f,10f)) * (dt))));
	___biggestFloat11 = UnityEngine.Mathf.Max(MinZ,___newPosition11.z);
	Position = new UnityEngine.Vector3(___newPosition11.x,___newPosition11.y,___biggestFloat11);
	s111 = -1;
return;	
	default: return;}}
	

	int s112=-1;
	public void parallelMethod112(float dt, World world){ 
	switch (s112)
	{

	case -1:
	if(!(world.InputMonitor.D))
	{

	s112 = -1;
return;	}else
	{

	goto case 2;	}
	case 2:
	___newPosition12 = ((Position) + (((new UnityEngine.Vector3(10f,0f,0f)) * (dt))));
	___biggestFloat12 = UnityEngine.Mathf.Min(MaxX,___newPosition12.x);
	Position = new UnityEngine.Vector3(___biggestFloat12,___newPosition12.y,___newPosition12.z);
	s112 = -1;
return;	
	default: return;}}
	

	int s113=-1;
	public void parallelMethod113(float dt, World world){ 
	switch (s113)
	{

	case -1:
	if(!(world.InputMonitor.A))
	{

	s113 = -1;
return;	}else
	{

	goto case 2;	}
	case 2:
	___newPosition13 = ((Position) - (((new UnityEngine.Vector3(10f,0f,0f)) * (dt))));
	___biggestFloat13 = UnityEngine.Mathf.Max(MinX,___newPosition13.x);
	Position = new UnityEngine.Vector3(___biggestFloat13,___newPosition13.y,___newPosition13.z);
	s113 = -1;
return;	
	default: return;}}
	

	int s114=-1;
	public void parallelMethod114(float dt, World world){ 
	switch (s114)
	{

	case -1:
	if(!(((((BoundaryX) > (world.InputMonitor.MousePositionOnScreen.x))) && (((Position.x) > (MinX))))))
	{

	s114 = -1;
return;	}else
	{

	goto case 1;	}
	case 1:
	___adjustment10 = ((CameraMovementSensitivity) * (-1f));
	Position = ((Position) + (new UnityEngine.Vector3(___adjustment10,0f,0f)));
	s114 = -1;
return;	
	default: return;}}
	

	int s115=-1;
	public void parallelMethod115(float dt, World world){ 
	switch (s115)
	{

	case -1:
	if(!(((((world.InputMonitor.MousePositionOnScreen.x) > (((ScreenWidth) - (BoundaryX))))) && (((MaxX) > (Position.x))))))
	{

	s115 = -1;
return;	}else
	{

	goto case 1;	}
	case 1:
	___adjustment11 = CameraMovementSensitivity;
	Position = ((Position) + (new UnityEngine.Vector3(___adjustment11,0f,0f)));
	s115 = -1;
return;	
	default: return;}}
	

	int s116=-1;
	public void parallelMethod116(float dt, World world){ 
	switch (s116)
	{

	case -1:
	if(!(((((BoundaryY) > (world.InputMonitor.MousePositionOnScreen.y))) && (((Position.z) > (MinZ))))))
	{

	s116 = -1;
return;	}else
	{

	goto case 1;	}
	case 1:
	___adjustment12 = ((CameraMovementSensitivity) * (-1f));
	Position = ((Position) + (new UnityEngine.Vector3(0f,0f,___adjustment12)));
	s116 = -1;
return;	
	default: return;}}
	

	int s117=-1;
	public void parallelMethod117(float dt, World world){ 
	switch (s117)
	{

	case -1:
	if(!(((((world.InputMonitor.MousePositionOnScreen.y) > (((ScreenHeight) - (BoundaryY))))) && (((MaxZ) > (Position.z))))))
	{

	s117 = -1;
return;	}else
	{

	goto case 1;	}
	case 1:
	___adjustment13 = CameraMovementSensitivity;
	Position = ((Position) + (new UnityEngine.Vector3(0f,0f,___adjustment13)));
	s117 = -1;
return;	
	default: return;}}
	

	int s0=-1;
	public void Rule0(float dt, World world){ 
	switch (s0)
	{

	case -1:
	this.parallelMethod000(dt,world);
	this.parallelMethod001(dt,world);
	s0 = -1;
return;	
	default: return;}}
	

	int s1=-1;
	public void Rule1(float dt, World world){ 
	switch (s1)
	{

	case -1:
	this.parallelMethod110(dt,world);
	this.parallelMethod111(dt,world);
	this.parallelMethod112(dt,world);
	this.parallelMethod113(dt,world);
	this.parallelMethod114(dt,world);
	this.parallelMethod115(dt,world);
	this.parallelMethod116(dt,world);
	this.parallelMethod117(dt,world);
	s1 = -1;
return;	
	default: return;}}
	





}
public class GameConstants{
public int frame;
public bool JustEntered = true;
	public int ID;
public GameConstants()
	{JustEntered = false;
 frame = World.frame;
		Neutral = new Faction(new FactionType(0),6f,6f,6f,6f);
		MineralPlanet = new PlanetStatModifier(1f,10f,1f,1f,5f,1f,3f,20f,1f,1f,18f,1f);
		EarthPlanet = new PlanetStatModifier(1f,10f,1f,1f,10f,1f,1f,10f,1f,1f,10f,1f);
		Defensive = new Faction(new FactionType(2),4f,8f,5f,7f);
		Asteroid = new PlanetStatModifier(1f,10f,1f,1f,18f,1f,2f,5f,1f,1f,5f,1f);
		Aggressive = new Faction(new FactionType(1),9f,3f,7f,5f);
		
}
		public Faction Aggressive;
	public PlanetStatModifier Asteroid;
	public Faction Defensive;
	public PlanetStatModifier EarthPlanet;
	public PlanetStatModifier MineralPlanet;
	public Faction Neutral;
	public void Update(float dt, World world) {
frame = World.frame;

		Aggressive.Update(dt, world);
		Asteroid.Update(dt, world);
		Defensive.Update(dt, world);
		EarthPlanet.Update(dt, world);
		MineralPlanet.Update(dt, world);
		Neutral.Update(dt, world);


	}











}
public class PlanetStatModifier{
public int frame;
public bool JustEntered = true;
private System.Single atkMin;
private System.Single atkMax;
private System.Single atkDelta;
private System.Single defMin;
private System.Single defMax;
private System.Single defDelta;
private System.Single prodMin;
private System.Single prodMax;
private System.Single prodDelta;
private System.Single resMin;
private System.Single resMax;
private System.Single resDelta;
	public int ID;
public PlanetStatModifier(System.Single atkMin, System.Single atkMax, System.Single atkDelta, System.Single defMin, System.Single defMax, System.Single defDelta, System.Single prodMin, System.Single prodMax, System.Single prodDelta, System.Single resMin, System.Single resMax, System.Single resDelta)
	{JustEntered = false;
 frame = World.frame;
		ResMin = resMin;
		ResMax = resMax;
		ResDelta = resDelta;
		ProdMin = prodMin;
		ProdMax = prodMax;
		ProdDelta = prodDelta;
		DefMin = defMin;
		DefMax = defMax;
		DefDelta = defDelta;
		AtkMin = atkMin;
		AtkMax = atkMax;
		AtkDelta = atkDelta;
		
}
		public System.Single AtkDelta;
	public System.Single AtkMax;
	public System.Single AtkMin;
	public System.Single DefDelta;
	public System.Single DefMax;
	public System.Single DefMin;
	public System.Single ProdDelta;
	public System.Single ProdMax;
	public System.Single ProdMin;
	public System.Single ResDelta;
	public System.Single ResMax;
	public System.Single ResMin;
	public void Update(float dt, World world) {
frame = World.frame;



	}











}
public class Commander{
public int frame;
public bool JustEntered = true;
private System.Int32 i;
private Faction f;
	public int ID;
public Commander(System.Int32 i, Faction f)
	{JustEntered = false;
 frame = World.frame;
		Research = f.Research;
		Production = f.Production;
		Name = ("Player ") + (i.ToString());
		Faction = f;
		Defense = f.Defense;
		CommanderNumber = i;
		Attack = f.Attack;
		Allies = (

Enumerable.Empty<Commander>()).ToList<Commander>();
		
}
		public List<Commander> Allies;
	public System.Single Attack;
	public System.Int32 CommanderNumber;
	public System.Single Defense;
	public Faction Faction;
	public System.String Name;
	public System.Single Production;
	public System.Single Research;
	public void Update(float dt, World world) {
frame = World.frame;

		Faction.Update(dt, world);


	}











}
public class Faction{
public int frame;
public bool JustEntered = true;
private FactionType n;
private System.Single atk;
private System.Single def;
private System.Single prod;
private System.Single res;
	public int ID;
public Faction(FactionType n, System.Single atk, System.Single def, System.Single prod, System.Single res)
	{JustEntered = false;
 frame = World.frame;
		UnityFaction = new UnityFaction();
		Research = res;
		Production = prod;
		FactionType = n;
		Defense = def;
		Attack = atk;
		
}
		public System.Single Attack;
	public System.Single Defense;
	public FactionType FactionType;
	public System.Single Production;
	public System.Single Research;
	public UnityFaction UnityFaction;
	public UnityEngine.Animation animation{  get { return UnityFaction.animation; }
 }
	public UnityEngine.AudioSource audio{  get { return UnityFaction.audio; }
 }
	public UnityEngine.Camera camera{  get { return UnityFaction.camera; }
 }
	public UnityEngine.Collider collider{  get { return UnityFaction.collider; }
 }
	public UnityEngine.Collider2D collider2D{  get { return UnityFaction.collider2D; }
 }
	public UnityEngine.ConstantForce constantForce{  get { return UnityFaction.constantForce; }
 }
	public System.Boolean enabled{  get { return UnityFaction.enabled; }
  set{UnityFaction.enabled = value; }
 }
	public UnityEngine.GameObject gameObject{  get { return UnityFaction.gameObject; }
 }
	public UnityEngine.GUIElement guiElement{  get { return UnityFaction.guiElement; }
 }
	public UnityEngine.GUIText guiText{  get { return UnityFaction.guiText; }
 }
	public UnityEngine.GUITexture guiTexture{  get { return UnityFaction.guiTexture; }
 }
	public UnityEngine.HideFlags hideFlags{  get { return UnityFaction.hideFlags; }
  set{UnityFaction.hideFlags = value; }
 }
	public UnityEngine.HingeJoint hingeJoint{  get { return UnityFaction.hingeJoint; }
 }
	public UnityEngine.Light light{  get { return UnityFaction.light; }
 }
	public System.String name{  get { return UnityFaction.name; }
  set{UnityFaction.name = value; }
 }
	public UnityEngine.ParticleEmitter particleEmitter{  get { return UnityFaction.particleEmitter; }
 }
	public UnityEngine.ParticleSystem particleSystem{  get { return UnityFaction.particleSystem; }
 }
	public UnityEngine.Renderer renderer{  get { return UnityFaction.renderer; }
 }
	public UnityEngine.Rigidbody rigidbody{  get { return UnityFaction.rigidbody; }
 }
	public UnityEngine.Rigidbody2D rigidbody2D{  get { return UnityFaction.rigidbody2D; }
 }
	public System.String tag{  get { return UnityFaction.tag; }
  set{UnityFaction.tag = value; }
 }
	public UnityEngine.Transform transform{  get { return UnityFaction.transform; }
 }
	public System.Boolean useGUILayout{  get { return UnityFaction.useGUILayout; }
  set{UnityFaction.useGUILayout = value; }
 }
	public void Update(float dt, World world) {
frame = World.frame;

		FactionType.Update(dt, world);


	}











}
public class FactionType{
public int frame;
public bool JustEntered = true;
private System.Int32 n;
	public int ID;
public FactionType(System.Int32 n)
	{JustEntered = false;
 frame = World.frame;
		Option<System.Int32> ___neutral00;
		if(((n) == (0)))
			{
			___neutral00 = (new Just<System.Int32>(n));
			}else
			{
			___neutral00 = (new Nothing<System.Int32>());
			}
		Option<System.Int32> ___aggressive00;
		if(((n) == (1)))
			{
			___aggressive00 = (new Just<System.Int32>(n));
			}else
			{
			___aggressive00 = (new Nothing<System.Int32>());
			}
		Option<System.Int32> ___defensive00;
		if(((n) == (2)))
			{
			___defensive00 = (new Just<System.Int32>(n));
			}else
			{
			___defensive00 = (new Nothing<System.Int32>());
			}
		Neutral = ___neutral00;
		Defensive = ___defensive00;
		Aggressive = ___aggressive00;
		
}
		public Option<System.Int32> Aggressive;
	public Option<System.Int32> Defensive;
	public Option<System.Int32> Neutral;
	public void Update(float dt, World world) {
frame = World.frame;

if(Aggressive.IsSome){  } 
if(Defensive.IsSome){  } 
if(Neutral.IsSome){  } 


	}











}
public class Planet{
public int frame;
public bool JustEntered = true;
private GameConstants constants;
private UnityPlanet p;
private List<Tuple<UnityPlanet, UnityPlanet>> n;
private Option<Commander> c;
	public int ID;
public Planet(GameConstants constants, UnityPlanet p, List<Tuple<UnityPlanet, UnityPlanet>> n, Option<Commander> c)
	{JustEntered = false;
 frame = World.frame;
		UnityPlanet = p;
		Targeted = false;
		StarSystem = (new Nothing<StarSystem>());
		ShipsToForward = (

Enumerable.Empty<Ship>()).ToList<Ship>();
		ShipToSend = (

Enumerable.Empty<TravelingShip>()).ToList<TravelingShip>();
		Selected = false;
		PlanetStats = (new Nothing<PlanetInfo>());
		Owner = c;
		Neighbours = n;
		LandingShips = (

Enumerable.Empty<LandingShip>()).ToList<LandingShip>();
		LandedShips = 0;
		InboundShips = (

Enumerable.Empty<Ship>()).ToList<Ship>();
		Constants = constants;
		Battle = (new Nothing<Battle>());
		Auto_Hop_Timer = false;
		ActiveHopsCounter = 0;
		
}
		public System.Boolean ActionLight{  get { return UnityPlanet.ActionLight; }
  set{UnityPlanet.ActionLight = value; }
 }
	public System.Int32 ActiveHopsCounter;
	public System.Boolean Auto_Hop_Timer;
	public Option<Battle> Battle;
	public System.Int32 CommanderIndex{  get { return UnityPlanet.CommanderIndex; }
  set{UnityPlanet.CommanderIndex = value; }
 }
	public GameConstants Constants;
	public List<Ship> InboundShips;
	public System.Boolean IsFrontier{  get { return UnityPlanet.IsFrontier; }
  set{UnityPlanet.IsFrontier = value; }
 }
	public System.Boolean IsStartingPlanet{  get { return UnityPlanet.IsStartingPlanet; }
  set{UnityPlanet.IsStartingPlanet = value; }
 }
	public System.Int32 LandedShips;
	public List<LandingShip> LandingShips;
	public UnityEngine.Color LightColor{  get { return UnityPlanet.LightColor; }
  set{UnityPlanet.LightColor = value; }
 }
	public UnityEngine.Color MiniMapColor{  get { return UnityPlanet.MiniMapColor; }
  set{UnityPlanet.MiniMapColor = value; }
 }
	public System.String Name{  get { return UnityPlanet.Name; }
  set{UnityPlanet.Name = value; }
 }
	public List<Tuple<UnityPlanet, UnityPlanet>> Neighbours;
	public System.Boolean OnMouseOver{  get { return UnityPlanet.OnMouseOver; }
 }
	public Option<Commander> Owner;
	public Option<PlanetInfo> PlanetStats;
	public UnityEngine.Vector3 Position{  get { return UnityPlanet.Position; }
  set{UnityPlanet.Position = value; }
 }
	public System.Boolean Selected;
	public List<TravelingShip> ShipToSend;
	public List<Ship> ShipsToForward;
	public Option<StarSystem> StarSystem;
	public System.Boolean Targeted;
	public UnityPlanet.PlanetType Type{  get { return UnityPlanet.Type; }
  set{UnityPlanet.Type = value; }
 }
	public UnityPlanet UnityPlanet;
	public UnityEngine.Animation animation{  get { return UnityPlanet.animation; }
 }
	public UnityEngine.AudioSource audio{  get { return UnityPlanet.audio; }
 }
	public UnityEngine.Camera camera{  get { return UnityPlanet.camera; }
 }
	public UnityEngine.Collider collider{  get { return UnityPlanet.collider; }
 }
	public UnityEngine.Collider2D collider2D{  get { return UnityPlanet.collider2D; }
 }
	public UnityEngine.ConstantForce constantForce{  get { return UnityPlanet.constantForce; }
 }
	public System.Boolean enabled{  get { return UnityPlanet.enabled; }
  set{UnityPlanet.enabled = value; }
 }
	public UnityEngine.GameObject gameObject{  get { return UnityPlanet.gameObject; }
 }
	public UnityEngine.GUIElement guiElement{  get { return UnityPlanet.guiElement; }
 }
	public UnityEngine.GUIText guiText{  get { return UnityPlanet.guiText; }
 }
	public UnityEngine.GUITexture guiTexture{  get { return UnityPlanet.guiTexture; }
 }
	public UnityEngine.HideFlags hideFlags{  get { return UnityPlanet.hideFlags; }
  set{UnityPlanet.hideFlags = value; }
 }
	public UnityEngine.HingeJoint hingeJoint{  get { return UnityPlanet.hingeJoint; }
 }
	public UnityEngine.Light light{  get { return UnityPlanet.light; }
 }
	public System.String name{  get { return UnityPlanet.name; }
  set{UnityPlanet.name = value; }
 }
	public UnityEngine.ParticleEmitter particleEmitter{  get { return UnityPlanet.particleEmitter; }
 }
	public UnityEngine.ParticleSystem particleSystem{  get { return UnityPlanet.particleSystem; }
 }
	public UnityEngine.Renderer renderer{  get { return UnityPlanet.renderer; }
 }
	public UnityEngine.Rigidbody rigidbody{  get { return UnityPlanet.rigidbody; }
 }
	public UnityEngine.Rigidbody2D rigidbody2D{  get { return UnityPlanet.rigidbody2D; }
 }
	public System.String tag{  get { return UnityPlanet.tag; }
  set{UnityPlanet.tag = value; }
 }
	public UnityEngine.Transform transform{  get { return UnityPlanet.transform; }
 }
	public System.Boolean useGUILayout{  get { return UnityPlanet.useGUILayout; }
  set{UnityPlanet.useGUILayout = value; }
 }
	public List<StarSystem> ___ss01;
	public Commander ___owner11;
	public System.Int32 ___commander_number10;
	public UnityEngine.Color ___C10;
	public System.Int32 ___planetType20;
	public PlanetInfo ___pStats20;
	public List<Ship> ___is130;
	public List<Ship> ___is230;
	public List<Ship> ___x61;
	public List<Ship> ___inbounds90;
	public Battle ___StartBattle90;
	public UnitySelectionRectangle ___selecRec120;
	public System.Boolean ___isInRec120;
	public System.Int32 ___amount_of_ships150;
	public Planet ___target151;
	public List<Planet> ___next_hop150;
	public System.Single count_down2;
	public void Update(float dt, World world) {
frame = World.frame;

if(Battle.IsSome){ 		Battle.Value.Update(dt, world);
 } 
		Constants.Update(dt, world);
		for(int x0 = 0; x0 < InboundShips.Count; x0++) { 
			InboundShips[x0].Update(dt, world);
		}
		for(int x0 = 0; x0 < LandingShips.Count; x0++) { 
			LandingShips[x0].Update(dt, world);
		}
if(PlanetStats.IsSome){ 		PlanetStats.Value.Update(dt, world);
 } 
		for(int x0 = 0; x0 < ShipToSend.Count; x0++) { 
			ShipToSend[x0].Update(dt, world);
		}
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
		this.Rule10(dt, world);
		this.Rule11(dt, world);
		this.Rule12(dt, world);
		this.Rule13(dt, world);
		this.Rule14(dt, world);
		this.Rule15(dt, world);
		this.Rule16(dt, world);
	}





	int s0=-1;
	public void Rule0(float dt, World world){ 
	switch (s0)
	{

	case -1:
	___ss01 = (

(world.StarSystems).Select(__ContextSymbol92 => new { ___ss100 = __ContextSymbol92 })
.SelectMany(__ContextSymbol93=> (__ContextSymbol93.___ss100.Planets).Select(__ContextSymbol94 => new { ___p010 = __ContextSymbol94,
                                                      prev = __ContextSymbol93 })
.Where(__ContextSymbol95 => ((__ContextSymbol95.___p010) == (this)))
.Select(__ContextSymbol96 => __ContextSymbol96.prev.___ss100)
.ToList<StarSystem>())).ToList<StarSystem>();
	StarSystem = (new Just<StarSystem>(___ss01.Head()));
	s0 = 0;
return;
	case 0:
	if(!(false))
	{

	s0 = 0;
return;	}else
	{

	s0 = -1;
return;	}	
	default: return;}}
	

	int s1=-1;
	public void Rule1(float dt, World world){ 
	switch (s1)
	{

	case -1:
	if(Owner.IsSome)
	{

	goto case 4;	}else
	{

	s1 = -1;
return;	}
	case 4:
	___owner11 = Owner.Value;
	___commander_number10 = ___owner11.CommanderNumber;
	MiniMapColor = new UnityEngine.Color((0.2f) * (___commander_number10),0.1f,0.3f,1);
	s1 = 6;
return;
	case 6:
	___C10 = MiniMapColor;
	goto case 5;
	case 5:
	if(!(!(((___commander_number10) == (Owner.Value.CommanderNumber)))))
	{

	s1 = 5;
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
	if(!(((Owner.IsSome) && (PlanetStats.IsNone))))
	{

	s2 = -1;
return;	}else
	{

	goto case 2;	}
	case 2:
	___planetType20 = UnityEngine.Random.Range(0,3);
	if(((___planetType20) == (0)))
	{

	___pStats20 = new PlanetInfo(this,Constants.Asteroid);	}else
	{

	if(((___planetType20) == (1)))
	{

	___pStats20 = new PlanetInfo(this,Constants.MineralPlanet);	}else
	{

	___pStats20 = new PlanetInfo(this,Constants.EarthPlanet);	}	}
	PlanetStats = (new Just<PlanetInfo>(___pStats20));
	s2 = -1;
return;	
	default: return;}}
	

	int s3=-1;
	public void Rule3(float dt, World world){ 
	switch (s3)
	{

	case -1:
	___is130 = (

(world.Links).Select(__ContextSymbol97 => new { ___l33 = __ContextSymbol97 })
.SelectMany(__ContextSymbol98=> (__ContextSymbol98.___l33.ShipsToRemove).Select(__ContextSymbol99 => new { ___s30 = __ContextSymbol99,
                                                      prev = __ContextSymbol98 })
.Where(__ContextSymbol100 => ((__ContextSymbol100.___s30.Target) == (this)))
.Select(__ContextSymbol101 => __ContextSymbol101.___s30.BaseShip)
.ToList<Ship>())).ToList<Ship>();
	___is230 = (

(StarSystem.Value.Links).Select(__ContextSymbol102 => new { ___ss32 = __ContextSymbol102 })
.SelectMany(__ContextSymbol103=> (__ContextSymbol103.___ss32.ShipsToRemove).Select(__ContextSymbol104 => new { ___s31 = __ContextSymbol104,
                                                      prev = __ContextSymbol103 })
.Where(__ContextSymbol105 => ((__ContextSymbol105.___s31.Target) == (this)))
.Select(__ContextSymbol106 => __ContextSymbol106.___s31.BaseShip)
.ToList<Ship>())).ToList<Ship>();
	InboundShips = (___is130).Concat(___is230).ToList<Ship>();
	s3 = -1;
return;	
	default: return;}}
	

	int s4=-1;
	public void Rule4(float dt, World world){ 
	switch (s4)
	{

	case -1:
	if(Targeted)
	{

	goto case 8;	}else
	{

	goto case 4;	}
	case 8:
	LightColor = Color.red;
	s4 = 4;
return;
	case 4:
	if(Selected)
	{

	goto case 5;	}else
	{

	goto case 3;	}
	case 5:
	LightColor = Color.white;
	s4 = 3;
return;
	case 3:
	LightColor = LightColor;
	s4 = -1;
return;	
	default: return;}}
	

	int s5=-1;
	public void Rule5(float dt, World world){ 
	switch (s5)
	{

	case -1:
	if(!(Battle.IsSome))
	{

	s5 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	Battle.Value.ShipsToMerge = (

(InboundShips).Select(__ContextSymbol107 => new { ___i50 = __ContextSymbol107 })
.Select(__ContextSymbol108 => new {___ships_to_merge50 = (

(Battle.Value.AttackingShips).Select(__ContextSymbol109 => new { ___i250 = __ContextSymbol109,prev = __ContextSymbol108 })
.Where(__ContextSymbol110 => ((__ContextSymbol110.prev.___i50.Source) == (__ContextSymbol110.___i250.BaseShip.Source)))
.Select(__ContextSymbol111 => __ContextSymbol111.___i250.BaseShip)
.ToList<Ship>()).ToList<Ship>(), prev = __ContextSymbol108 })
.Where(__ContextSymbol112 => ((__ContextSymbol112.___ships_to_merge50.Count) > (0)))
.Select(__ContextSymbol113 => new MergingShip(__ContextSymbol113.prev.___i50))
.ToList<MergingShip>()).ToList<MergingShip>();
	s5 = -1;
return;	
	default: return;}}
	

	int s6=-1;
	public void Rule6(float dt, World world){ 
	switch (s6)
	{

	case -1:
	if(!(Battle.IsSome))
	{

	s6 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	if(((Battle.Value.ShipsToMerge.Count) > (0)))
	{

	goto case 1;	}else
	{

	s6 = -1;
return;	}
	case 1:
	___x61 = (

(InboundShips).Select(__ContextSymbol114 => new { ___i61 = __ContextSymbol114 })
.Select(__ContextSymbol115 => new {___i_do_not_know60 = (

(Battle.Value.ShipsToMerge).Select(__ContextSymbol116 => new { ___s62 = __ContextSymbol116,prev = __ContextSymbol115 })
.Where(__ContextSymbol117 => ((__ContextSymbol117.___s62.BaseShip.Source) == (__ContextSymbol117.prev.___i61.Source)))
.Select(__ContextSymbol118 => __ContextSymbol118.prev.___i61)
.ToList<Ship>()).ToList<Ship>(), prev = __ContextSymbol115 })
.Where(__ContextSymbol119 => ((__ContextSymbol119.___i_do_not_know60.Count) == (0)))
.Select(__ContextSymbol120 => __ContextSymbol120.prev.___i61)
.ToList<Ship>()).ToList<Ship>();
	InboundShips = ___x61;
	s6 = -1;
return;	
	default: return;}}
	

	int s7=-1;
	public void Rule7(float dt, World world){ 
	switch (s7)
	{

	case -1:
	if(Owner.IsSome)
	{

	goto case 6;	}else
	{

	s7 = -1;
return;	}
	case 6:
	LandingShips = (

(InboundShips).Select(__ContextSymbol121 => new { ___i72 = __ContextSymbol121 })
.Where(__ContextSymbol122 => ((((__ContextSymbol122.___i72.Owner) == (Owner.Value))) && (((__ContextSymbol122.___i72.FinalTarget) == (this)))))
.Select(__ContextSymbol123 => new LandingShip(__ContextSymbol123.___i72))
.ToList<LandingShip>()).ToList<LandingShip>();
	s7 = -1;
return;	
	default: return;}}
	

	int s8=-1;
	public void Rule8(float dt, World world){ 
	switch (s8)
	{

	case -1:
	if(!(Battle.IsSome))
	{

	s8 = -1;
return;	}else
	{

	goto case 2;	}
	case 2:
	if(!(((Battle.Value.AttackingShips.Count) == (0))))
	{

	s8 = 2;
return;	}else
	{

	goto case 1;	}
	case 1:
	Battle.Value.StopAnimation = true;
	s8 = 0;
return;
	case 0:
	if(!(Battle.IsNone))
	{

	s8 = 0;
return;	}else
	{

	s8 = -1;
return;	}	
	default: return;}}
	

	int s9=-1;
	public void Rule9(float dt, World world){ 
	switch (s9)
	{

	case -1:
	___inbounds90 = (

(InboundShips).Select(__ContextSymbol124 => new { ___s93 = __ContextSymbol124 })
.Where(__ContextSymbol125 => ((Owner.IsNone) || (!(((__ContextSymbol125.___s93.Owner.CommanderNumber) == (Owner.Value.CommanderNumber))))))
.Select(__ContextSymbol126 => __ContextSymbol126.___s93)
.ToList<Ship>()).ToList<Ship>();
	if(((Battle.IsNone) && (((___inbounds90.Count) > (0)))))
	{

	goto case 5;	}else
	{

	s9 = -1;
return;	}
	case 5:
	___StartBattle90 = new Battle(this);
	Battle = (new Just<Battle>(___StartBattle90));
	s9 = 8;
return;
	case 8:
	if(!(((Battle.Value.AttackingShips.Count) == (0))))
	{

	s9 = 8;
return;	}else
	{

	goto case 7;	}
	case 7:
	Battle = Battle;
	s9 = 6;
return;
	case 6:
	Battle = (new Nothing<Battle>());
	s9 = -1;
return;	
	default: return;}}
	

	int s10=-1;
	public void Rule10(float dt, World world){ 
	switch (s10)
	{

	case -1:
	ActionLight = ((Selected) || (Targeted));
	s10 = -1;
return;	
	default: return;}}
	

	int s11=-1;
	public void Rule11(float dt, World world){ 
	switch (s11)
	{

	case -1:
	if(((world.SelectedPlanets.Count) > (0)))
	{

	goto case 1;	}else
	{

	goto case 2;	}
	case 1:
	if(world.InputMonitor.RightMouseButton)
	{

	goto case 4;	}else
	{

	goto case 5;	}
	case 4:
	if(((OnMouseOver) && (!(Selected))))
	{

	goto case 7;	}else
	{

	goto case 8;	}
	case 7:
	Targeted = true;
	s11 = 10;
return;
	case 10:
	Targeted = false;
	s11 = -1;
return;
	case 8:
	Targeted = false;
	s11 = -1;
return;
	case 5:
	Targeted = Targeted;
	s11 = -1;
return;
	case 2:
	Targeted = false;
	s11 = -1;
return;	
	default: return;}}
	

	int s12=-1;
	public void Rule12(float dt, World world){ 
	switch (s12)
	{

	case -1:
	if(!(((world.SelectionManager.SelectionRectangle.IsSome) && (world.SelectionManager.SelectAll))))
	{

	s12 = -1;
return;	}else
	{

	goto case 18;	}
	case 18:
	___selecRec120 = world.SelectionManager.SelectionRectangle.Value.UnitySelectionRectangle;
	___isInRec120 = ((((((((Position.x) > (((___selecRec120.Position.x) - (((___selecRec120.Scale.x) / (2f))))))) && (((((___selecRec120.Position.x) + (((___selecRec120.Scale.x) / (2f))))) > (Position.x))))) && (((Position.z) > (((___selecRec120.Position.z) - (((___selecRec120.Scale.z) / (2f))))))))) && (((((___selecRec120.Position.z) + (((___selecRec120.Scale.z) / (2f))))) > (Position.z))));
	if(!(world.InputMonitor.ControlKey))
	{

	goto case 0;	}else
	{

	goto case 1;	}
	case 0:
	if(((___isInRec120) && (Owner.IsSome)))
	{

	goto case 3;	}else
	{

	goto case 4;	}
	case 3:
	Selected = true;
	s12 = -1;
return;
	case 4:
	Selected = false;
	s12 = -1;
return;
	case 1:
	if(((((world.InputMonitor.ControlKey) && (___isInRec120))) && (Owner.IsSome)))
	{

	goto case 8;	}else
	{

	goto case 9;	}
	case 8:
	if(Selected)
	{

	goto case 11;	}else
	{

	goto case 12;	}
	case 11:
	Selected = Selected;
	s12 = -1;
return;
	case 12:
	Selected = true;
	s12 = -1;
return;
	case 9:
	Selected = Selected;
	s12 = -1;
return;	
	default: return;}}
	

	int s13=-1;
	public void Rule13(float dt, World world){ 
	switch (s13)
	{

	case -1:
	if(((world.InputMonitor.LeftMouseButton) && (!(world.InputMonitor.ControlKey))))
	{

	goto case 20;	}else
	{

	goto case 21;	}
	case 20:
	if(((OnMouseOver) && (Owner.IsSome)))
	{

	goto case 23;	}else
	{

	goto case 24;	}
	case 23:
	Selected = true;
	Targeted = false;
	s13 = -1;
return;
	case 24:
	Selected = false;
	Targeted = Targeted;
	s13 = -1;
return;
	case 21:
	if(((((((world.InputMonitor.LeftMouseButton) && (world.InputMonitor.ControlKey))) && (OnMouseOver))) && (Owner.IsSome)))
	{

	goto case 28;	}else
	{

	goto case 29;	}
	case 28:
	if(Selected)
	{

	goto case 31;	}else
	{

	goto case 32;	}
	case 31:
	Selected = false;
	Targeted = false;
	s13 = -1;
return;
	case 32:
	Selected = true;
	Targeted = false;
	s13 = -1;
return;
	case 29:
	Selected = Selected;
	Targeted = Targeted;
	s13 = -1;
return;	
	default: return;}}
	

	int s14=-1;
	public void Rule14(float dt, World world){ 
	switch (s14)
	{

	case -1:
	if(((((Owner.IsSome) && (Battle.IsNone))) && (((InboundShips.Count) > (0)))))
	{

	goto case 38;	}else
	{

	s14 = -1;
return;	}
	case 38:
	ShipsToForward = (

(InboundShips).Select(__ContextSymbol127 => new { ___i143 = __ContextSymbol127 })
.Where(__ContextSymbol128 => ((((__ContextSymbol128.___i143.Owner) == (Owner.Value))) && (!(((__ContextSymbol128.___i143.FinalTarget) == (this))))))
.Select(__ContextSymbol129 => __ContextSymbol129.___i143)
.ToList<Ship>()).ToList<Ship>();
	s14 = 39;
return;
	case 39:
	ShipsToForward = (

Enumerable.Empty<Ship>()).ToList<Ship>();
	s14 = -1;
return;	
	default: return;}}
	

	int s15=-1;
	public void Rule15(float dt, World world){ 
	switch (s15)
	{

	case -1:
	if(Selected)
	{

	goto case 42;	}else
	{

	s15 = -1;
return;	}
	case 42:
	if(((LandedShips) > (0)))
	{

	goto case 44;	}else
	{

	s15 = -1;
return;	}
	case 44:
	if(((((world.TargetedPlanet.IsSome) && (Battle.IsNone))) && (!(world.InputMonitor.ShiftKey))))
	{

	goto case 46;	}else
	{

	s15 = -1;
return;	}
	case 46:
	if(world.InputMonitor.ControlKey)
	{

	___amount_of_ships150 = LandedShips;	}else
	{

	___amount_of_ships150 = ((System.Int32)((((LandedShips) + (1))) / (2)));	}
	___target151 = world.TargetedPlanet.Value;
	___next_hop150 = (

(this.Neighbours).Select(__ContextSymbol131 => new { ___h150 = __ContextSymbol131 })
.SelectMany(__ContextSymbol132=> (world.Planets).Select(__ContextSymbol133 => new { ___q152 = __ContextSymbol133,
                                                      prev = __ContextSymbol132 })
.Where(__ContextSymbol134 => ((((__ContextSymbol134.prev.___h150.Item1) == (world.TargetedPlanet.Value.UnityPlanet))) && (((__ContextSymbol134.prev.___h150.Item2) == (__ContextSymbol134.___q152.UnityPlanet)))))
.Select(__ContextSymbol135 => __ContextSymbol135.___q152)
.ToList<Planet>())).ToList<Planet>();
	ShipToSend = (

(new Cons<TravelingShip>(new TravelingShip(___next_hop150.Head(),this,new Ship(Owner.Value,Position,___amount_of_ships150,world.TargetedPlanet.Value,___next_hop150.Head(),this,(new Nothing<UnityShip>()))),(new Empty<TravelingShip>()).ToList<TravelingShip>())).ToList<TravelingShip>()).ToList<TravelingShip>();
	LandedShips = 0;
	s15 = 47;
return;
	case 47:
	ShipToSend = (

Enumerable.Empty<TravelingShip>()).ToList<TravelingShip>();
	LandedShips = LandedShips;
	s15 = -1;
return;	
	default: return;}}
	

	int s16=-1;
	public void Rule16(float dt, World world){ 
	switch (s16)
	{

	case -1:
	count_down2 = 3f;
	goto case 3;
	case 3:
	if(((count_down2) > (0f)))
	{

	count_down2 = ((count_down2) - (dt));
	s16 = 3;
return;	}else
	{

	goto case 1;	}
	case 1:
	Auto_Hop_Timer = true;
	s16 = 0;
return;
	case 0:
	Auto_Hop_Timer = false;
	s16 = -1;
return;	
	default: return;}}
	





}
public class Link{
public int frame;
public bool JustEntered = true;
private UnityLink l;
private Planet p1;
private Planet p2;
private System.Boolean are_sources_reversed;
	public int ID;
public Link(UnityLink l, Planet p1, Planet p2, System.Boolean are_sources_reversed)
	{JustEntered = false;
 frame = World.frame;
		UnityLink = l;
		TravelingShips = (

Enumerable.Empty<TravelingShip>()).ToList<TravelingShip>();
		Target = p2;
		SourcesReversed = are_sources_reversed;
		Source = p1;
		ShipsToRemove = (

Enumerable.Empty<TravelingShip>()).ToList<TravelingShip>();
		AutoHopActive = false;
		
}
		public System.Boolean AutoHopActive;
	public UnityEngine.Vector3 Position{  get { return UnityLink.Position; }
  set{UnityLink.Position = value; }
 }
	public System.Boolean SSLink{  get { return UnityLink.SSLink; }
  set{UnityLink.SSLink = value; }
 }
	public List<TravelingShip> ShipsToRemove;
	public Planet Source;
	public System.Boolean SourcesReversed;
	public Planet Target;
	public List<TravelingShip> TravelingShips;
	public System.Boolean UnityAutoHopActive{  get { return UnityLink.UnityAutoHopActive; }
  set{UnityLink.UnityAutoHopActive = value; }
 }
	public UnityLink UnityLink;
	public System.Boolean UnitySourcesReversed{  get { return UnityLink.UnitySourcesReversed; }
  set{UnityLink.UnitySourcesReversed = value; }
 }
	public UnityEngine.Animation animation{  get { return UnityLink.animation; }
 }
	public UnityEngine.AudioSource audio{  get { return UnityLink.audio; }
 }
	public UnityEngine.Camera camera{  get { return UnityLink.camera; }
 }
	public UnityEngine.Collider collider{  get { return UnityLink.collider; }
 }
	public UnityEngine.Collider2D collider2D{  get { return UnityLink.collider2D; }
 }
	public UnityEngine.ConstantForce constantForce{  get { return UnityLink.constantForce; }
 }
	public System.Boolean enabled{  get { return UnityLink.enabled; }
  set{UnityLink.enabled = value; }
 }
	public UnityPlanet endPlanet{  get { return UnityLink.endPlanet; }
  set{UnityLink.endPlanet = value; }
 }
	public UnityEngine.GameObject gameObject{  get { return UnityLink.gameObject; }
 }
	public UnityEngine.GUIElement guiElement{  get { return UnityLink.guiElement; }
 }
	public UnityEngine.GUIText guiText{  get { return UnityLink.guiText; }
 }
	public UnityEngine.GUITexture guiTexture{  get { return UnityLink.guiTexture; }
 }
	public UnityEngine.HideFlags hideFlags{  get { return UnityLink.hideFlags; }
  set{UnityLink.hideFlags = value; }
 }
	public UnityEngine.HingeJoint hingeJoint{  get { return UnityLink.hingeJoint; }
 }
	public UnityEngine.Light light{  get { return UnityLink.light; }
 }
	public System.String name{  get { return UnityLink.name; }
  set{UnityLink.name = value; }
 }
	public UnityEngine.ParticleEmitter particleEmitter{  get { return UnityLink.particleEmitter; }
 }
	public UnityEngine.ParticleSystem particleSystem{  get { return UnityLink.particleSystem; }
 }
	public UnityEngine.Renderer renderer{  get { return UnityLink.renderer; }
 }
	public UnityEngine.Rigidbody rigidbody{  get { return UnityLink.rigidbody; }
 }
	public UnityEngine.Rigidbody2D rigidbody2D{  get { return UnityLink.rigidbody2D; }
 }
	public UnityPlanet startPlanet{  get { return UnityLink.startPlanet; }
  set{UnityLink.startPlanet = value; }
 }
	public System.String tag{  get { return UnityLink.tag; }
  set{UnityLink.tag = value; }
 }
	public UnityEngine.Transform transform{  get { return UnityLink.transform; }
 }
	public System.Boolean useGUILayout{  get { return UnityLink.useGUILayout; }
  set{UnityLink.useGUILayout = value; }
 }
	public List<Ship> ___query30;
	public List<Tuple<Planet, Ship>> ___next_hops30;
	public List<TravelingShip> ___ships_to_send30;
	public List<Tuple<Planet, Planet>> ___ahm_ih_240;
	public List<Tuple<Planet, Planet>> ___elems40;
	public System.Int32 ___amount_of_active_targets50;
	public System.Int32 ___amount_of_ships_to_send_per_target50;
	public void Update(float dt, World world) {
frame = World.frame;

		for(int x0 = 0; x0 < TravelingShips.Count; x0++) { 
			TravelingShips[x0].Update(dt, world);
		}
		this.Rule0(dt, world);
		this.Rule1(dt, world);
		this.Rule2(dt, world);
		this.Rule3(dt, world);
		this.Rule4(dt, world);
		this.Rule5(dt, world);
		this.Rule6(dt, world);
	}





	int s0=-1;
	public void Rule0(float dt, World world){ 
	switch (s0)
	{

	case -1:
	ShipsToRemove = (

(TravelingShips).Select(__ContextSymbol142 => new { ___s04 = __ContextSymbol142 })
.Where(__ContextSymbol143 => !(((UnityEngine.Vector3.Distance(__ContextSymbol143.___s04.BaseShip.Position,Target.Position)) > (1.5f))))
.Select(__ContextSymbol144 => __ContextSymbol144.___s04)
.ToList<TravelingShip>()).ToList<TravelingShip>();
	s0 = -1;
return;	
	default: return;}}
	

	int s1=-1;
	public void Rule1(float dt, World world){ 
	switch (s1)
	{

	case -1:
	TravelingShips = (

(TravelingShips).Select(__ContextSymbol145 => new { ___s15 = __ContextSymbol145 })
.Where(__ContextSymbol146 => ((UnityEngine.Vector3.Distance(__ContextSymbol146.___s15.BaseShip.Position,Target.Position)) > (1.5f)))
.Select(__ContextSymbol147 => __ContextSymbol147.___s15)
.ToList<TravelingShip>()).ToList<TravelingShip>();
	s1 = -1;
return;	
	default: return;}}
	

	int s2=-1;
	public void Rule2(float dt, World world){ 
	switch (s2)
	{

	case -1:
	TravelingShips = ((

(Source.ShipToSend).Select(__ContextSymbol148 => new { ___s26 = __ContextSymbol148 })
.Where(__ContextSymbol149 => ((__ContextSymbol149.___s26.Target) == (Target)))
.Select(__ContextSymbol150 => __ContextSymbol150.___s26)
.ToList<TravelingShip>()).ToList<TravelingShip>()).Concat(TravelingShips).ToList<TravelingShip>();
	s2 = -1;
return;	
	default: return;}}
	

	int s3=-1;
	public void Rule3(float dt, World world){ 
	switch (s3)
	{

	case -1:
	if(((Source.ShipsToForward.Count) > (0)))
	{

	goto case 2;	}else
	{

	s3 = -1;
return;	}
	case 2:
	___query30 = (

(Source.ShipsToForward).Select(__ContextSymbol151 => new { ___s37 = __ContextSymbol151 })
.Where(__ContextSymbol152 => ((__ContextSymbol152.___s37.Target.Name) == (Source.Name)))
.Select(__ContextSymbol153 => __ContextSymbol153.___s37)
.ToList<Ship>()).ToList<Ship>();
	___next_hops30 = (

(___query30).Select(__ContextSymbol154 => new { ___s38 = __ContextSymbol154 })
.SelectMany(__ContextSymbol155=> (Source.Neighbours).Select(__ContextSymbol156 => new { ___h31 = __ContextSymbol156,
                                                      prev = __ContextSymbol155 })
.SelectMany(__ContextSymbol157=> (world.Planets).Select(__ContextSymbol158 => new { ___q33 = __ContextSymbol158,
                                                      prev = __ContextSymbol157 })
.Where(__ContextSymbol159 => ((((__ContextSymbol159.prev.___h31.Item1) == (__ContextSymbol159.prev.prev.___s38.FinalTarget.UnityPlanet))) && (((__ContextSymbol159.prev.___h31.Item2) == (__ContextSymbol159.___q33.UnityPlanet)))))
.Select(__ContextSymbol160 => new Casanova.Prelude.Tuple<Planet,Ship>(__ContextSymbol160.___q33,__ContextSymbol160.prev.prev.___s38))
.ToList<Tuple<Planet, Ship>>()))).ToList<Tuple<Planet, Ship>>();
	___ships_to_send30 = (

(___query30).Select(__ContextSymbol161 => new { ___s39 = __ContextSymbol161 })
.SelectMany(__ContextSymbol162=> (___next_hops30).Select(__ContextSymbol163 => new { ___next_hop31 = __ContextSymbol163,
                                                      prev = __ContextSymbol162 })
.Where(__ContextSymbol164 => ((((__ContextSymbol164.prev.___s39) == (__ContextSymbol164.___next_hop31.Item2))) && (((__ContextSymbol164.___next_hop31.Item1.Name) == (Target.Name)))))
.Select(__ContextSymbol165 => new TravelingShip(__ContextSymbol165.___next_hop31.Item1,Source,new Ship(Source.Owner.Value,Source.Position,__ContextSymbol165.prev.___s39.AmountOfShips,__ContextSymbol165.prev.___s39.FinalTarget,__ContextSymbol165.___next_hop31.Item1,Source,(new Just<UnityShip>(__ContextSymbol165.prev.___s39.UnityShip)))))
.ToList<TravelingShip>())).ToList<TravelingShip>();
	TravelingShips = (___ships_to_send30).Concat(TravelingShips).ToList<TravelingShip>();
	s3 = -1;
return;	
	default: return;}}
	

	int s4=-1;
	public void Rule4(float dt, World world){ 
	switch (s4)
	{

	case -1:
	if(!(((world.AutoHopManager.IsSome) && (world.AutoHopManager.Value.IsDone))))
	{

	s4 = -1;
return;	}else
	{

	goto case 2;	}
	case 2:
	if(((world.AutoHopManager.Value.IntermediateHops.Count) == (1)))
	{

	goto case 0;	}else
	{

	goto case 1;	}
	case 0:
	___ahm_ih_240 = world.AutoHopManager.Value.IntermediateHops;
	if(((((___ahm_ih_240.Head().Item1) == (Source))) && (((___ahm_ih_240.Head().Item2) == (Target)))))
	{

	goto case 4;	}else
	{

	s4 = -1;
return;	}
	case 4:
	UnityEngine.Debug.Log(((((("I am link': ") + (Source.Name)) + (", ")) + (Target.Name)) + ("AHA ")) + (AutoHopActive));
	if(((AutoHopActive) == (true)))
	{

	goto case 5;	}else
	{

	goto case 6;	}
	case 5:
	UnitySourcesReversed = SourcesReversed;
	AutoHopActive = false;
	UnityAutoHopActive = false;
	Source.ActiveHopsCounter = ((Source.ActiveHopsCounter) - (1));
	s4 = -1;
return;
	case 6:
	UnitySourcesReversed = SourcesReversed;
	AutoHopActive = true;
	UnityAutoHopActive = true;
	Source.ActiveHopsCounter = ((Source.ActiveHopsCounter) + (1));
	s4 = -1;
return;
	case 1:
	___elems40 = (

(world.AutoHopManager.Value.IntermediateHops).Select(__ContextSymbol166 => new { ___a42 = __ContextSymbol166 })
.Where(__ContextSymbol167 => ((((__ContextSymbol167.___a42.Item1) == (Source))) && (((__ContextSymbol167.___a42.Item2) == (Target)))))
.Select(__ContextSymbol168 => __ContextSymbol168.___a42)
.ToList<Tuple<Planet, Planet>>()).ToList<Tuple<Planet, Planet>>();
	if(((___elems40.Count) > (0)))
	{

	goto case 13;	}else
	{

	s4 = -1;
return;	}
	case 13:
	UnityEngine.Debug.Log(((("I am link: ") + (Source.Name)) + (", ")) + (Target.Name));
	UnitySourcesReversed = SourcesReversed;
	AutoHopActive = true;
	UnityAutoHopActive = true;
	Source.ActiveHopsCounter = ((Source.ActiveHopsCounter) + (1));
	s4 = -1;
return;	
	default: return;}}
	

	int s5=-1;
	public void Rule5(float dt, World world){ 
	switch (s5)
	{

	case -1:
	if(!(((Source.Auto_Hop_Timer) && (AutoHopActive))))
	{

	s5 = -1;
return;	}else
	{

	goto case 5;	}
	case 5:
	___amount_of_active_targets50 = Source.ActiveHopsCounter;
	___amount_of_ships_to_send_per_target50 = ((System.Int32)((Source.LandedShips) / (___amount_of_active_targets50)));
	if(((___amount_of_ships_to_send_per_target50) > (0)))
	{

	goto case 1;	}else
	{

	s5 = -1;
return;	}
	case 1:
	UnityEngine.Debug.Log(Source.ActiveHopsCounter);
	TravelingShips = new Cons<TravelingShip>(new TravelingShip(Target,Source,new Ship(Source.Owner.Value,Source.Position,___amount_of_ships_to_send_per_target50,Target,Target,Source,(new Nothing<UnityShip>()))), (TravelingShips)).ToList<TravelingShip>();
	Source.LandedShips = ((Source.LandedShips) - (___amount_of_ships_to_send_per_target50));
	s5 = -1;
return;	
	default: return;}}
	

	int s6=-1;
	public void Rule6(float dt, World world){ 
	switch (s6)
	{

	case -1:
	if(!(world.InputMonitor.RightMouseButton))
	{

	s6 = -1;
return;	}else
	{

	goto case 1;	}
	case 1:
	if(((((((world.InputMonitor.ShiftKey) && (Source.Selected))) && (AutoHopActive))) && (world.TargetedPlanet.IsNone)))
	{

	goto case 2;	}else
	{

	goto case 0;	}
	case 2:
	UnitySourcesReversed = SourcesReversed;
	AutoHopActive = false;
	UnityAutoHopActive = false;
	Source.ActiveHopsCounter = ((Source.ActiveHopsCounter) - (1));
	s6 = 0;
return;
	case 0:
	if(!(world.InputMonitor.RightMouseButtonUp))
	{

	s6 = 0;
return;	}else
	{

	s6 = -1;
return;	}	
	default: return;}}
	





}
public class AutoHopManager{
public int frame;
public bool JustEntered = true;
private List<Planet> sources;
private Planet target;
	public int ID;
public AutoHopManager(List<Planet> sources, Planet target)
	{JustEntered = false;
 frame = World.frame;
		Temp = (new Nothing<Planet>());
		Target = target;
		Sources = sources;
		IsDone = false;
		IntermediateHops = (

Enumerable.Empty<Tuple<Planet, Planet>>()).ToList<Tuple<Planet, Planet>>();
		
}
		public List<Tuple<Planet, Planet>> IntermediateHops;
	public System.Boolean IsDone;
	public List<Planet> Sources;
	public Planet Target;
	public Option<Planet> Temp;
	public Planet ___s010;
	public System.Int32 counter30;
	public List<Tuple<Planet, Planet>> ___temp_in_IntermediateHops00;
	public List<Planet> ___next_hop02;
	public Planet ___next_hop03;
	public List<Tuple<Planet, Planet>> ___lst00;
	public void Update(float dt, World world) {
frame = World.frame;

		for(int x0 = 0; x0 < IntermediateHops.Count; x0++) { 
			IntermediateHops[x0].Item1.Update(dt, world);
			IntermediateHops[x0].Item2.Update(dt, world);
		}
		for(int x0 = 0; x0 < Sources.Count; x0++) { 
			Sources[x0].Update(dt, world);
		}
		Target.Update(dt, world);
if(Temp.IsSome){ 		Temp.Value.Update(dt, world);
 } 
		this.Rule0(dt, world);

	}





	int s0=-1;
	public void Rule0(float dt, World world){ 
	switch (s0)
	{

	case -1:
	
	counter30 = -1;
	if((((Sources).Count) == (0)))
	{

	goto case 1;	}else
	{

	___s010 = (Sources)[0];
	goto case 3;	}
	case 3:
	counter30 = ((counter30) + (1));
	if((((((Sources).Count) == (counter30))) || (((counter30) > ((Sources).Count)))))
	{

	goto case 1;	}else
	{

	___s010 = (Sources)[counter30];
	goto case 4;	}
	case 4:
	IntermediateHops = IntermediateHops;
	Temp = (new Just<Planet>(___s010));
	IsDone = IsDone;
	s0 = 16;
return;
	case 16:
	___temp_in_IntermediateHops00 = (

(IntermediateHops).Select(__ContextSymbol171 => new { ___i04 = __ContextSymbol171 })
.Where(__ContextSymbol172 => ((__ContextSymbol172.___i04.Item1) == (Temp.Value)))
.Select(__ContextSymbol173 => __ContextSymbol173.___i04)
.ToList<Tuple<Planet, Planet>>()).ToList<Tuple<Planet, Planet>>();
	goto case 5;
	case 5:
	if(!(((((!(((Target) == (Temp.Value)))) && (((Temp.Value.Owner) == (___s010.Owner))))) && (((___temp_in_IntermediateHops00.Count) == (0))))))
	{

	s0 = 3;
return;	}else
	{

	goto case 6;	}
	case 6:
	___next_hop02 = (

(Temp.Value.Neighbours).Select(__ContextSymbol174 => new { ___h02 = __ContextSymbol174 })
.SelectMany(__ContextSymbol175=> (world.Planets).Select(__ContextSymbol176 => new { ___p011 = __ContextSymbol176,
                                                      prev = __ContextSymbol175 })
.Where(__ContextSymbol177 => ((((__ContextSymbol177.prev.___h02.Item1) == (Target.UnityPlanet))) && (((__ContextSymbol177.___p011.UnityPlanet) == (__ContextSymbol177.prev.___h02.Item2)))))
.Select(__ContextSymbol178 => __ContextSymbol178.___p011)
.ToList<Planet>())).ToList<Planet>();
	___next_hop03 = ___next_hop02.Head();
	if(((___next_hop03.Owner) == (___s010.Owner)))
	{

	goto case 7;	}else
	{

	goto case 8;	}
	case 7:
	___lst00 = ((

(new Cons<Tuple<Planet, Planet>>(new Casanova.Prelude.Tuple<Planet,Planet>(Temp.Value,___next_hop03),(new Empty<Tuple<Planet, Planet>>()).ToList<Tuple<Planet, Planet>>())).ToList<Tuple<Planet, Planet>>()).ToList<Tuple<Planet, Planet>>()).Concat(IntermediateHops).ToList<Tuple<Planet, Planet>>();
	UnityEngine.Debug.Log(((Temp.Value.Name) + (" -> ")) + (___next_hop03.Name));
	IntermediateHops = ___lst00;
	Temp = (new Just<Planet>(___next_hop03));
	IsDone = IsDone;
	s0 = 5;
return;
	case 8:
	IntermediateHops = IntermediateHops;
	Temp = (new Just<Planet>(___next_hop03));
	IsDone = IsDone;
	s0 = 5;
return;
	case 1:
	IntermediateHops = IntermediateHops;
	Temp = Temp;
	IsDone = true;
	s0 = 0;
return;
	case 0:
	if(!(false))
	{

	s0 = 0;
return;	}else
	{

	s0 = -1;
return;	}	
	default: return;}}
	






}
public class PlanetInfo{
public int frame;
public bool JustEntered = true;
private Planet p;
private PlanetStatModifier psm;
	public int ID;
public PlanetInfo(Planet p, PlanetStatModifier psm)
	{JustEntered = false;
 frame = World.frame;
		UpgradeState = (new Nothing<System.Int32>());
		UnityStat = UnityStat.FindStat(p.UnityPlanet);
		StatModifiers = psm;
		ResearchState = ResearchNeutral;
		ResearchOn = 0.25f;
		ResearchOff = -0.25f;
		ResearchNeutral = -0.1f;
		ResearchCost = -0.5f;
		Research = p.Owner.Value.Research;
		ProductivityState = ResearchNeutral;
		Productivity = p.Owner.Value.Production;
		Planet = p;
		DefenseState = ResearchNeutral;
		Defense = p.Owner.Value.Defense;
		AttackState = ResearchNeutral;
		Attack = p.Owner.Value.Attack;
		
}
		public System.Single Attack;
	public System.Single AttackState;
	public System.String AttackText{  get { return UnityStat.AttackText; }
  set{UnityStat.AttackText = value; }
 }
	public System.Single Defense;
	public System.Single DefenseState;
	public System.String DefenseText{  get { return UnityStat.DefenseText; }
  set{UnityStat.DefenseText = value; }
 }
	public System.Boolean Enabled{  get { return UnityStat.Enabled; }
  set{UnityStat.Enabled = value; }
 }
	public System.String NextShipInText{  get { return UnityStat.NextShipInText; }
  set{UnityStat.NextShipInText = value; }
 }
	public System.String OwnerText{  get { return UnityStat.OwnerText; }
  set{UnityStat.OwnerText = value; }
 }
	public Planet Planet;
	public System.Single Productivity;
	public System.Single ProductivityState;
	public System.String ProductivityText{  get { return UnityStat.ProductivityText; }
  set{UnityStat.ProductivityText = value; }
 }
	public System.Single Research;
	public System.Single ResearchCost;
	public System.Single ResearchNeutral;
	public System.Single ResearchOff;
	public System.Single ResearchOn;
	public System.Single ResearchState;
	public System.String ResearchText{  get { return UnityStat.ResearchText; }
  set{UnityStat.ResearchText = value; }
 }
	public UnityEngine.Vector3 Rotation{  get { return UnityStat.Rotation; }
  set{UnityStat.Rotation = value; }
 }
	public System.String ShipCountText{  get { return UnityStat.ShipCountText; }
  set{UnityStat.ShipCountText = value; }
 }
	public PlanetStatModifier StatModifiers;
	public UnityStat UnityStat;
	public Option<System.Int32> UpgradeState;
	public UnityEngine.Animation animation{  get { return UnityStat.animation; }
 }
	public UnityEngine.AudioSource audio{  get { return UnityStat.audio; }
 }
	public UnityEngine.Camera camera{  get { return UnityStat.camera; }
 }
	public UnityEngine.Collider collider{  get { return UnityStat.collider; }
 }
	public UnityEngine.Collider2D collider2D{  get { return UnityStat.collider2D; }
 }
	public UnityEngine.ConstantForce constantForce{  get { return UnityStat.constantForce; }
 }
	public System.Boolean enabled{  get { return UnityStat.enabled; }
  set{UnityStat.enabled = value; }
 }
	public UnityEngine.GameObject gameObject{  get { return UnityStat.gameObject; }
 }
	public UnityEngine.GUIElement guiElement{  get { return UnityStat.guiElement; }
 }
	public UnityEngine.GUIText guiText{  get { return UnityStat.guiText; }
 }
	public UnityEngine.GUITexture guiTexture{  get { return UnityStat.guiTexture; }
 }
	public UnityEngine.HideFlags hideFlags{  get { return UnityStat.hideFlags; }
  set{UnityStat.hideFlags = value; }
 }
	public UnityEngine.HingeJoint hingeJoint{  get { return UnityStat.hingeJoint; }
 }
	public UnityEngine.Light light{  get { return UnityStat.light; }
 }
	public System.String name{  get { return UnityStat.name; }
  set{UnityStat.name = value; }
 }
	public UnityEngine.ParticleEmitter particleEmitter{  get { return UnityStat.particleEmitter; }
 }
	public UnityEngine.ParticleSystem particleSystem{  get { return UnityStat.particleSystem; }
 }
	public UnityEngine.Renderer renderer{  get { return UnityStat.renderer; }
 }
	public UnityEngine.Rigidbody rigidbody{  get { return UnityStat.rigidbody; }
 }
	public UnityEngine.Rigidbody2D rigidbody2D{  get { return UnityStat.rigidbody2D; }
 }
	public UnityEngine.Color setColor{  get { return UnityStat.setColor; }
  set{UnityStat.setColor = value; }
 }
	public System.String tag{  get { return UnityStat.tag; }
  set{UnityStat.tag = value; }
 }
	public UnityEngine.Transform transform{  get { return UnityStat.transform; }
 }
	public System.Boolean useGUILayout{  get { return UnityStat.useGUILayout; }
  set{UnityStat.useGUILayout = value; }
 }
	public System.Single ___cameraRotation00;
	public System.Single count_down3;
	public System.Single ___on20;
	public System.Single ___off20;
	public System.Single ___cost20;
	public System.Int32 ___state30;
	public System.Single ___attack80;
	public System.Single ___defense80;
	public System.Single ___productivity80;
	public System.Single ___research80;
	public Commander ___owner112;
	public System.Int32 ___commander_number111;
	public System.Single ___res120;
	public System.Single ___AttackRound130;
	public System.Single ___DefenseRound130;
	public System.Single ___ProductivityRound130;
	public System.Single ___ResearchRound130;
	public void Update(float dt, World world) {
frame = World.frame;

		StatModifiers.Update(dt, world);
if(UpgradeState.IsSome){  } 
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
		this.Rule10(dt, world);
		this.Rule11(dt, world);
		this.Rule12(dt, world);
		this.Rule13(dt, world);
		this.Rule14(dt, world);
		this.Rule15(dt, world);
	}





	int s0=-1;
	public void Rule0(float dt, World world){ 
	switch (s0)
	{

	case -1:
	___cameraRotation00 = world.MainCamera.Rotation.x;
	Rotation = new UnityEngine.Vector3(___cameraRotation00,Rotation.y,Rotation.z);
	s0 = -1;
return;	
	default: return;}}
	

	int s1=-1;
	public void Rule1(float dt, World world){ 
	switch (s1)
	{

	case -1:
	if(((Planet.Owner.IsSome) && (Planet.Battle.IsNone)))
	{

	goto case 3;	}else
	{

	s1 = -1;
return;	}
	case 3:
	count_down3 = ((3f) / (((1f) + (((0.05f) * (Productivity))))));
	goto case 6;
	case 6:
	if(((count_down3) > (0f)))
	{

	count_down3 = ((count_down3) - (dt));
	s1 = 6;
return;	}else
	{

	goto case 4;	}
	case 4:
	Planet.LandedShips = ((Planet.LandedShips) + (1));
	s1 = -1;
return;	
	default: return;}}
	

	int s2=-1;
	public void Rule2(float dt, World world){ 
	switch (s2)
	{

	case -1:
	___on20 = ((0.25f) + (((0.01f) * (Research))));
	___off20 = ((-0.25f) + (((0.01f) * (Research))));
	___cost20 = ((-0.5f) + (((0.01f) * (Research))));
	ResearchOn = ___on20;
	ResearchOff = ___off20;
	ResearchCost = ___cost20;
	s2 = -1;
return;	
	default: return;}}
	

	int s3=-1;
	public void Rule3(float dt, World world){ 
	switch (s3)
	{

	case -1:
	if(!(UpgradeState.IsSome))
	{

	s3 = -1;
return;	}else
	{

	goto case 2;	}
	case 2:
	___state30 = UpgradeState.Value;
	Attack = ((Attack) + (ResearchCost));
	Defense = ((Defense) + (ResearchCost));
	Productivity = ((Productivity) + (ResearchCost));
	Research = ((Research) + (ResearchCost));
	s3 = 0;
return;
	case 0:
	if(!(((UpgradeState.IsNone) || (((UpgradeState.IsSome) && (!(((UpgradeState.Value) == (___state30)))))))))
	{

	s3 = 0;
return;	}else
	{

	s3 = -1;
return;	}	
	default: return;}}
	

	int s4=-1;
	public void Rule4(float dt, World world){ 
	switch (s4)
	{

	case -1:
	if(((StatModifiers.ResMin) > (Research)))
	{

	goto case 4;	}else
	{

	goto case 5;	}
	case 4:
	Research = StatModifiers.ResMin;
	s4 = -1;
return;
	case 5:
	if(((Research) > (StatModifiers.ResMax)))
	{

	goto case 9;	}else
	{

	s4 = -1;
return;	}
	case 9:
	Research = StatModifiers.ResMax;
	s4 = -1;
return;	
	default: return;}}
	

	int s5=-1;
	public void Rule5(float dt, World world){ 
	switch (s5)
	{

	case -1:
	if(((StatModifiers.ProdMin) > (Productivity)))
	{

	goto case 11;	}else
	{

	goto case 12;	}
	case 11:
	Productivity = StatModifiers.ProdMin;
	s5 = -1;
return;
	case 12:
	if(((Productivity) > (StatModifiers.ProdMax)))
	{

	goto case 16;	}else
	{

	s5 = -1;
return;	}
	case 16:
	Productivity = StatModifiers.ProdMax;
	s5 = -1;
return;	
	default: return;}}
	

	int s6=-1;
	public void Rule6(float dt, World world){ 
	switch (s6)
	{

	case -1:
	if(((StatModifiers.DefMin) > (Defense)))
	{

	goto case 18;	}else
	{

	goto case 19;	}
	case 18:
	Defense = StatModifiers.DefMin;
	s6 = -1;
return;
	case 19:
	if(((Defense) > (StatModifiers.DefMax)))
	{

	goto case 23;	}else
	{

	s6 = -1;
return;	}
	case 23:
	Defense = StatModifiers.DefMax;
	s6 = -1;
return;	
	default: return;}}
	

	int s7=-1;
	public void Rule7(float dt, World world){ 
	switch (s7)
	{

	case -1:
	if(((StatModifiers.AtkMin) > (Attack)))
	{

	goto case 25;	}else
	{

	goto case 26;	}
	case 25:
	Attack = StatModifiers.AtkMin;
	s7 = -1;
return;
	case 26:
	if(((Attack) > (StatModifiers.AtkMax)))
	{

	goto case 30;	}else
	{

	s7 = -1;
return;	}
	case 30:
	Attack = StatModifiers.AtkMax;
	s7 = -1;
return;	
	default: return;}}
	

	int s8=-1;
	public void Rule8(float dt, World world){ 
	switch (s8)
	{

	case -1:
	___attack80 = ((Attack) + (((((StatModifiers.AtkDelta) * (dt))) * (AttackState))));
	___defense80 = ((Defense) + (((((StatModifiers.DefDelta) * (dt))) * (DefenseState))));
	___productivity80 = ((Productivity) + (((((StatModifiers.ProdDelta) * (dt))) * (ProductivityState))));
	___research80 = ((Research) + (((((StatModifiers.ResDelta) * (dt))) * (ResearchState))));
	Attack = ___attack80;
	Defense = ___defense80;
	Productivity = ___productivity80;
	Research = ___research80;
	s8 = -1;
return;	
	default: return;}}
	

	int s9=-1;
	public void Rule9(float dt, World world){ 
	switch (s9)
	{

	case -1:
	if(((UpgradeState.IsSome) && (((UpgradeState.Value) == (1)))))
	{

	goto case 5;	}else
	{

	goto case 6;	}
	case 5:
	AttackState = ResearchOn;
	DefenseState = ResearchOff;
	ProductivityState = ResearchOff;
	ResearchState = ResearchOff;
	s9 = -1;
return;
	case 6:
	if(((UpgradeState.IsSome) && (((UpgradeState.Value) == (2)))))
	{

	goto case 9;	}else
	{

	goto case 10;	}
	case 9:
	AttackState = ResearchOff;
	DefenseState = ResearchOn;
	ProductivityState = ResearchOff;
	ResearchState = ResearchOff;
	s9 = -1;
return;
	case 10:
	if(((UpgradeState.IsSome) && (((UpgradeState.Value) == (3)))))
	{

	goto case 13;	}else
	{

	goto case 14;	}
	case 13:
	AttackState = ResearchOff;
	DefenseState = ResearchOff;
	ProductivityState = ResearchOn;
	ResearchState = ResearchOff;
	s9 = -1;
return;
	case 14:
	if(((UpgradeState.IsSome) && (((UpgradeState.Value) == (4)))))
	{

	goto case 17;	}else
	{

	goto case 18;	}
	case 17:
	AttackState = ResearchOff;
	DefenseState = ResearchOff;
	ProductivityState = ResearchOff;
	ResearchState = ResearchOn;
	s9 = -1;
return;
	case 18:
	if(UpgradeState.IsNone)
	{

	goto case 22;	}else
	{

	s9 = -1;
return;	}
	case 22:
	AttackState = ResearchNeutral;
	DefenseState = ResearchNeutral;
	ProductivityState = ResearchNeutral;
	ResearchState = ResearchNeutral;
	s9 = -1;
return;	
	default: return;}}
	

	int s10=-1;
	public void Rule10(float dt, World world){ 
	switch (s10)
	{

	case -1:
	if(Planet.Owner.IsSome)
	{

	goto case 25;	}else
	{

	s10 = -1;
return;	}
	case 25:
	ShipCountText = (("Ships ") + (Planet.LandedShips));
	s10 = -1;
return;	
	default: return;}}
	

	int s11=-1;
	public void Rule11(float dt, World world){ 
	switch (s11)
	{

	case -1:
	if(Planet.Owner.IsSome)
	{

	goto case 27;	}else
	{

	goto case 28;	}
	case 27:
	___owner112 = Planet.Owner.Value;
	___commander_number111 = Planet.Owner.Value.CommanderNumber;
	OwnerText = (((((("Commander ") + (___commander_number111))) + (", Name "))) + (Planet.Name));
	s11 = -1;
return;
	case 28:
	OwnerText = "Neutral";
	s11 = -1;
return;	
	default: return;}}
	

	int s12=-1;
	public void Rule12(float dt, World world){ 
	switch (s12)
	{

	case -1:
	if(Planet.Owner.IsSome)
	{

	goto case 34;	}else
	{

	goto case 35;	}
	case 34:
	___res120 = ((3f) / (((1f) + (((0.05f) * (Productivity))))));
	NextShipInText = ___res120.ToString("0.##");
	s12 = -1;
return;
	case 35:
	NextShipInText = "";
	s12 = -1;
return;	
	default: return;}}
	

	int s13=-1;
	public void Rule13(float dt, World world){ 
	switch (s13)
	{

	case -1:
	___AttackRound130 = ((UnityEngine.Mathf.Round((Attack) * (10f))) / (10f));
	___DefenseRound130 = ((UnityEngine.Mathf.Round((Defense) * (10f))) / (10f));
	___ProductivityRound130 = ((UnityEngine.Mathf.Round((Productivity) * (10f))) / (10f));
	___ResearchRound130 = ((UnityEngine.Mathf.Round((Research) * (10f))) / (10f));
	AttackText = ___AttackRound130.ToString();
	DefenseText = ___DefenseRound130.ToString();
	ProductivityText = ___ProductivityRound130.ToString();
	ResearchText = ___ResearchRound130.ToString();
	s13 = -1;
return;	
	default: return;}}
	

	int s14=-1;
	public void Rule14(float dt, World world){ 
	switch (s14)
	{

	case -1:
	if(((Planet.Selected) && (world.InputMonitor.Alpha0)))
	{

	goto case 5;	}else
	{

	goto case 6;	}
	case 5:
	UpgradeState = (new Nothing<System.Int32>());
	s14 = -1;
return;
	case 6:
	if(((Planet.Selected) && (world.InputMonitor.Alpha1)))
	{

	goto case 9;	}else
	{

	goto case 10;	}
	case 9:
	UpgradeState = (new Just<System.Int32>(1));
	s14 = -1;
return;
	case 10:
	if(((Planet.Selected) && (world.InputMonitor.Alpha2)))
	{

	goto case 13;	}else
	{

	goto case 14;	}
	case 13:
	UpgradeState = (new Just<System.Int32>(2));
	s14 = -1;
return;
	case 14:
	if(((Planet.Selected) && (world.InputMonitor.Alpha3)))
	{

	goto case 17;	}else
	{

	goto case 18;	}
	case 17:
	UpgradeState = (new Just<System.Int32>(3));
	s14 = -1;
return;
	case 18:
	if(((Planet.Selected) && (world.InputMonitor.Alpha4)))
	{

	goto case 22;	}else
	{

	s14 = -1;
return;	}
	case 22:
	UpgradeState = (new Just<System.Int32>(4));
	s14 = -1;
return;	
	default: return;}}
	

	int s15=-1;
	public void Rule15(float dt, World world){ 
	switch (s15)
	{

	case -1:
	Enabled = Planet.Owner.IsSome;
	s15 = -1;
return;	
	default: return;}}
	





}
public class StarSystem{
public int frame;
public bool JustEntered = true;
private UnityStarSystem ss;
private List<Planet> Planets_3;
private List<Link> Links1;
	public int ID;
public StarSystem(UnityStarSystem ss, List<Planet> Planets_3, List<Link> Links1)
	{JustEntered = false;
 frame = World.frame;
		List<Planet> ___planets_100;
		___planets_100 = (

(ss.Planets_2).Select(__ContextSymbol180 => new { ___ss_p00 = __ContextSymbol180 })
.SelectMany(__ContextSymbol181=> (Planets_3).Select(__ContextSymbol182 => new { ___p012 = __ContextSymbol182,
                                                      prev = __ContextSymbol181 })
.Where(__ContextSymbol183 => ((__ContextSymbol183.___p012.UnityPlanet) == (__ContextSymbol183.prev.___ss_p00)))
.Select(__ContextSymbol184 => __ContextSymbol184.___p012)
.ToList<Planet>())).ToList<Planet>();
		List<Link> ___links000;
		___links000 = (

(Links1).Select(__ContextSymbol185 => new { ___l04 = __ContextSymbol185 })
.SelectMany(__ContextSymbol186=> (___planets_100).Select(__ContextSymbol187 => new { ___p013 = __ContextSymbol187,
                                                      prev = __ContextSymbol186 })
.Where(__ContextSymbol188 => ((((__ContextSymbol188.prev.___l04.SSLink) == (false))) && (((__ContextSymbol188.prev.___l04.Source) == (__ContextSymbol188.___p013)))))
.Select(__ContextSymbol189 => __ContextSymbol189.prev.___l04)
.ToList<Link>())).ToList<Link>();
		UnityStarSystem = ss;
		Planets = ___planets_100;
		Links = ___links000;
		
}
		public List<Link> Links;
	public List<Planet> Planets;
	public System.Collections.Generic.List<UnityPlanet> Planets_2{  get { return UnityStarSystem.Planets_2; }
 }
	public UnityStarSystem UnityStarSystem;
	public UnityEngine.Animation animation{  get { return UnityStarSystem.animation; }
 }
	public UnityEngine.AudioSource audio{  get { return UnityStarSystem.audio; }
 }
	public UnityEngine.Camera camera{  get { return UnityStarSystem.camera; }
 }
	public UnityEngine.Collider collider{  get { return UnityStarSystem.collider; }
 }
	public UnityEngine.Collider2D collider2D{  get { return UnityStarSystem.collider2D; }
 }
	public UnityEngine.ConstantForce constantForce{  get { return UnityStarSystem.constantForce; }
 }
	public System.Boolean enabled{  get { return UnityStarSystem.enabled; }
  set{UnityStarSystem.enabled = value; }
 }
	public UnityEngine.GameObject gameObject{  get { return UnityStarSystem.gameObject; }
 }
	public UnityEngine.GUIElement guiElement{  get { return UnityStarSystem.guiElement; }
 }
	public UnityEngine.GUIText guiText{  get { return UnityStarSystem.guiText; }
 }
	public UnityEngine.GUITexture guiTexture{  get { return UnityStarSystem.guiTexture; }
 }
	public UnityEngine.HideFlags hideFlags{  get { return UnityStarSystem.hideFlags; }
  set{UnityStarSystem.hideFlags = value; }
 }
	public UnityEngine.HingeJoint hingeJoint{  get { return UnityStarSystem.hingeJoint; }
 }
	public UnityEngine.Light light{  get { return UnityStarSystem.light; }
 }
	public System.String name{  get { return UnityStarSystem.name; }
  set{UnityStarSystem.name = value; }
 }
	public UnityEngine.ParticleEmitter particleEmitter{  get { return UnityStarSystem.particleEmitter; }
 }
	public UnityEngine.ParticleSystem particleSystem{  get { return UnityStarSystem.particleSystem; }
 }
	public UnityEngine.Renderer renderer{  get { return UnityStarSystem.renderer; }
 }
	public UnityEngine.Rigidbody rigidbody{  get { return UnityStarSystem.rigidbody; }
 }
	public UnityEngine.Rigidbody2D rigidbody2D{  get { return UnityStarSystem.rigidbody2D; }
 }
	public System.String tag{  get { return UnityStarSystem.tag; }
  set{UnityStarSystem.tag = value; }
 }
	public UnityEngine.Transform transform{  get { return UnityStarSystem.transform; }
 }
	public System.Boolean useGUILayout{  get { return UnityStarSystem.useGUILayout; }
  set{UnityStarSystem.useGUILayout = value; }
 }
	public void Update(float dt, World world) {
frame = World.frame;

		for(int x0 = 0; x0 < Links.Count; x0++) { 
			Links[x0].Update(dt, world);
		}


	}











}
public class Battle{
public int frame;
public bool JustEntered = true;
private Planet n;
	public int ID;
public Battle(Planet n)
	{JustEntered = false;
 frame = World.frame;
		UnityBattle = UnityBattle.Instantiate(n.Position);
		SmallestFleet = (new Nothing<AttackingShip>());
		ShipsToMerge = (

Enumerable.Empty<MergingShip>()).ToList<MergingShip>();
		Planet = n;
		NextPossibleOwner = (new Nothing<Commander>());
		AttackingShips = (

Enumerable.Empty<AttackingShip>()).ToList<AttackingShip>();
		
}
		public List<AttackingShip> AttackingShips;
	public System.Boolean Destroyed{  get { return UnityBattle.Destroyed; }
  set{UnityBattle.Destroyed = value; }
 }
	public Option<Commander> NextPossibleOwner;
	public Planet Planet;
	public List<MergingShip> ShipsToMerge;
	public Option<AttackingShip> SmallestFleet;
	public System.Boolean StopAnimation{  get { return UnityBattle.StopAnimation; }
  set{UnityBattle.StopAnimation = value; }
 }
	public System.Collections.Generic.List<UnityShip> UnityAttackingShips{  set{UnityBattle.UnityAttackingShips = value; }
 }
	public UnityBattle UnityBattle;
	public UnityEngine.Animation animation{  get { return UnityBattle.animation; }
 }
	public UnityEngine.AudioSource audio{  get { return UnityBattle.audio; }
 }
	public UnityEngine.Camera camera{  get { return UnityBattle.camera; }
 }
	public UnityEngine.Collider collider{  get { return UnityBattle.collider; }
 }
	public UnityEngine.Collider2D collider2D{  get { return UnityBattle.collider2D; }
 }
	public UnityEngine.ConstantForce constantForce{  get { return UnityBattle.constantForce; }
 }
	public System.Boolean enabled{  get { return UnityBattle.enabled; }
  set{UnityBattle.enabled = value; }
 }
	public UnityEngine.GameObject gameObject{  get { return UnityBattle.gameObject; }
 }
	public UnityEngine.GUIElement guiElement{  get { return UnityBattle.guiElement; }
 }
	public UnityEngine.GUIText guiText{  get { return UnityBattle.guiText; }
 }
	public UnityEngine.GUITexture guiTexture{  get { return UnityBattle.guiTexture; }
 }
	public UnityEngine.HideFlags hideFlags{  get { return UnityBattle.hideFlags; }
  set{UnityBattle.hideFlags = value; }
 }
	public UnityEngine.HingeJoint hingeJoint{  get { return UnityBattle.hingeJoint; }
 }
	public UnityEngine.Light light{  get { return UnityBattle.light; }
 }
	public System.String name{  get { return UnityBattle.name; }
  set{UnityBattle.name = value; }
 }
	public UnityEngine.ParticleEmitter particleEmitter{  get { return UnityBattle.particleEmitter; }
 }
	public UnityEngine.ParticleSystem particleSystem{  get { return UnityBattle.particleSystem; }
 }
	public UnityEngine.Renderer renderer{  get { return UnityBattle.renderer; }
 }
	public UnityEngine.Rigidbody rigidbody{  get { return UnityBattle.rigidbody; }
 }
	public UnityEngine.Rigidbody2D rigidbody2D{  get { return UnityBattle.rigidbody2D; }
 }
	public System.String tag{  get { return UnityBattle.tag; }
  set{UnityBattle.tag = value; }
 }
	public UnityEngine.Transform transform{  get { return UnityBattle.transform; }
 }
	public System.Boolean useGUILayout{  get { return UnityBattle.useGUILayout; }
  set{UnityBattle.useGUILayout = value; }
 }
	public List<AttackingShip> ___filter10;
	public System.String ___inbounds21;
	public System.String ___debug230;
	public System.Int32 ___AmountOfAttackingShips30;
	public System.Single count_down4;
	public System.Single ___RandomNumber30;
	public System.Single ___FlankingBonus30;
	public System.Single ___DamageFormula30;
	public System.String ___FlankingBonusDebug30;
	public System.Int32 ___res31;
	public System.Int32 ___Damage30;
	public List<AttackingShip> ___new_attackingShips30;
	public List<AttackingShip> ___smallest40;
	public System.Single count_down5;
	public System.Int32 ___RandomNumber51;
	public System.Int32 ___DamageFormula51;
	public System.Int32 ___res52;
	public System.Int32 ___Damage51;
	public void Update(float dt, World world) {
frame = World.frame;

		for(int x0 = 0; x0 < AttackingShips.Count; x0++) { 
			AttackingShips[x0].Update(dt, world);
		}
		for(int x0 = 0; x0 < ShipsToMerge.Count; x0++) { 
			ShipsToMerge[x0].Update(dt, world);
		}
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
	if(!(((AttackingShips.Count) > (0))))
	{

	s0 = -1;
return;	}else
	{

	goto case 1;	}
	case 1:
	NextPossibleOwner = (new Just<Commander>(AttackingShips.Head().BaseShip.Owner));
	s0 = 0;
return;
	case 0:
	if(!(((((AttackingShips.Count) > (0))) && (!(((AttackingShips.Head().BaseShip.Owner) == (NextPossibleOwner.Value)))))))
	{

	s0 = 0;
return;	}else
	{

	s0 = -1;
return;	}	
	default: return;}}
	

	int s1=-1;
	public void Rule1(float dt, World world){ 
	switch (s1)
	{

	case -1:
	___filter10 = (

(AttackingShips).Select(__ContextSymbol194 => new { ___a13 = __ContextSymbol194 })
.Where(__ContextSymbol195 => !(__ContextSymbol195.___a13.BaseShip.Destroyed))
.Select(__ContextSymbol196 => __ContextSymbol196.___a13)
.ToList<AttackingShip>()).ToList<AttackingShip>();
	AttackingShips = ___filter10;
	s1 = -1;
return;	
	default: return;}}
	

	int s2=-1;
	public void Rule2(float dt, World world){ 
	switch (s2)
	{

	case -1:
	___inbounds21 = Planet.InboundShips.Count.ToString();
	AttackingShips = ((

(Planet.InboundShips).Select(__ContextSymbol197 => new { ___s211 = __ContextSymbol197 })
.Where(__ContextSymbol198 => ((Planet.Owner.IsNone) || (!(((__ContextSymbol198.___s211.Owner.CommanderNumber) == (Planet.Owner.Value.CommanderNumber))))))
.Select(__ContextSymbol199 => new AttackingShip(__ContextSymbol199.___s211,__ContextSymbol199.___s211.Target))
.ToList<AttackingShip>()).ToList<AttackingShip>()).Concat(AttackingShips).ToList<AttackingShip>();
	s2 = -1;
return;	
	default: return;}}
	

	int s3=-1;
	public void Rule3(float dt, World world){ 
	switch (s3)
	{

	case -1:
	___debug230 = AttackingShips.Count.ToString();
	___AmountOfAttackingShips30 = (

(AttackingShips).Select(__ContextSymbol200 => new { ___i35 = __ContextSymbol200 })
.Where(__ContextSymbol201 => ((NextPossibleOwner.IsSome) && (((__ContextSymbol201.___i35.BaseShip.Owner) == (NextPossibleOwner.Value)))))
.Select(__ContextSymbol202 => __ContextSymbol202.___i35.BaseShip.AmountOfShips)
.Aggregate(default(System.Int32), (acc, __x) => acc + __x));
	if(((((Planet.LandedShips) > (0))) && (((AttackingShips.Count) > (0)))))
	{

	goto case 2;	}else
	{

	goto case 3;	}
	case 2:
	count_down4 = 1f;
	goto case 19;
	case 19:
	if(((count_down4) > (0f)))
	{

	count_down4 = ((count_down4) - (dt));
	s3 = 19;
return;	}else
	{

	goto case 17;	}
	case 17:
	___RandomNumber30 = UnityEngine.Random.Range(3f,7f);
	___FlankingBonus30 = ((((1f) + (((0.3f) * (AttackingShips.Count))))) - (0.3f));
	___DamageFormula30 = ((((((((((System.Single)___AmountOfAttackingShips30)) * (6f))) * (___RandomNumber30))) * (___FlankingBonus30))) / (500f));
	UnityEngine.Debug.Log(("Predamage ") + ((((((System.Single)___AmountOfAttackingShips30)) * (6f)) * (___RandomNumber30)) * (___FlankingBonus30)));
	___FlankingBonusDebug30 = ___FlankingBonus30.ToString();
	UnityEngine.Debug.Log(("FlankingBonus ") + (___FlankingBonus30));
	___res31 = ((System.Int32)___DamageFormula30);
	___Damage30 = ((___res31) + (1));
	if(((Planet.LandedShips) > (___Damage30)))
	{

	goto case 5;	}else
	{

	goto case 6;	}
	case 5:
	Planet.LandedShips = ((Planet.LandedShips) - (___Damage30));
	Planet.Owner = Planet.Owner;
	s3 = -1;
return;
	case 6:
	Planet.LandedShips = 0;
	Planet.Owner = Planet.Owner;
	s3 = -1;
return;
	case 3:
	if(((((((Planet.LandedShips) == (0))) && (((AttackingShips.Count) > (0))))) && (NextPossibleOwner.IsSome)))
	{

	goto case 21;	}else
	{

	s3 = -1;
return;	}
	case 21:
	___new_attackingShips30 = (

(AttackingShips).Select(__ContextSymbol204 => new { ___i36 = __ContextSymbol204 })
.Where(__ContextSymbol205 => !(((__ContextSymbol205.___i36.BaseShip.Owner) == (NextPossibleOwner.Value))))
.Select(__ContextSymbol206 => __ContextSymbol206.___i36)
.ToList<AttackingShip>()).ToList<AttackingShip>();
	if(((___new_attackingShips30.Count) > (0)))
	{

	goto case 22;	}else
	{

	goto case 23;	}
	case 22:
	Planet.LandedShips = ___AmountOfAttackingShips30;
	Planet.Owner = (new Just<Commander>(NextPossibleOwner.Value));
	s3 = -1;
return;
	case 23:
	Planet.LandedShips = ___AmountOfAttackingShips30;
	Planet.Owner = (new Just<Commander>(NextPossibleOwner.Value));
	s3 = -1;
return;	
	default: return;}}
	

	int s4=-1;
	public void Rule4(float dt, World world){ 
	switch (s4)
	{

	case -1:
	if(((AttackingShips.Count) > (0)))
	{

	goto case 30;	}else
	{

	goto case 31;	}
	case 30:
	___smallest40 = (

(AttackingShips).Select(__ContextSymbol207 => new { ___a44 = __ContextSymbol207 })
.Select(__ContextSymbol208 => new {___other45 = (

(AttackingShips).Select(__ContextSymbol209 => new { ___b40 = __ContextSymbol209,prev = __ContextSymbol208 })
.Where(__ContextSymbol210 => ((((__ContextSymbol210.prev.___a44.BaseShip.AmountOfShips) > (__ContextSymbol210.___b40.BaseShip.AmountOfShips))) && (!(((__ContextSymbol210.prev.___a44) == (__ContextSymbol210.___b40))))))
.Select(__ContextSymbol211 => __ContextSymbol211.___b40)
.ToList<AttackingShip>()).ToList<AttackingShip>(), prev = __ContextSymbol208 })
.Where(__ContextSymbol212 => ((__ContextSymbol212.___other45.Count) == (0)))
.Select(__ContextSymbol213 => __ContextSymbol213.prev.___a44)
.ToList<AttackingShip>()).ToList<AttackingShip>();
	SmallestFleet = (new Just<AttackingShip>(___smallest40.Head()));
	s4 = -1;
return;
	case 31:
	SmallestFleet = (new Nothing<AttackingShip>());
	s4 = -1;
return;	
	default: return;}}
	

	int s5=-1;
	public void Rule5(float dt, World world){ 
	switch (s5)
	{

	case -1:
	count_down5 = 1f;
	goto case 10;
	case 10:
	if(((count_down5) > (0f)))
	{

	count_down5 = ((count_down5) - (dt));
	s5 = 10;
return;	}else
	{

	goto case 8;	}
	case 8:
	___RandomNumber51 = UnityEngine.Random.Range(3,7);
	___DamageFormula51 = ((((((Planet.LandedShips) * (10))) * (___RandomNumber51))) / (500));
	___res52 = ((System.Int32)___DamageFormula51);
	___Damage51 = ((___res52) + (1));
	if(((SmallestFleet.IsSome) && (((SmallestFleet.Value.BaseShip.AmountOfShips) > (___Damage51)))))
	{

	goto case 0;	}else
	{

	goto case 1;	}
	case 0:
	SmallestFleet.Value.BaseShip.AmountOfShips = ((SmallestFleet.Value.BaseShip.AmountOfShips) - (___Damage51));
	s5 = -1;
return;
	case 1:
	SmallestFleet.Value.BaseShip.AmountOfShips = 0;
	s5 = -1;
return;	
	default: return;}}
	

	int s6=-1;
	public void Rule6(float dt, World world){ 
	switch (s6)
	{

	case -1:
	if(!(((AttackingShips.Count) == (0))))
	{

	s6 = -1;
return;	}else
	{

	goto case 1;	}
	case 1:
	Destroyed = false;
	s6 = 0;
return;
	case 0:
	Destroyed = true;
	s6 = -1;
return;	
	default: return;}}
	

	int s7=-1;
	public void Rule7(float dt, World world){ 
	switch (s7)
	{

	case -1:
	UnityAttackingShips = (

(AttackingShips).Select(__ContextSymbol214 => new { ___s712 = __ContextSymbol214 })
.Select(__ContextSymbol215 => __ContextSymbol215.___s712.BaseShip.UnityShip)
.ToList<UnityShip>()).ToList<UnityShip>();
	s7 = -1;
return;	
	default: return;}}
	





}
public class Ship{
public int frame;
public bool JustEntered = true;
private Commander o;
private UnityEngine.Vector3 pos;
private System.Int32 amount;
private Planet final_target;
private Planet target;
private Planet source;
private Option<UnityShip> BaseShip;
	public int ID;
public Ship(Commander o, UnityEngine.Vector3 pos, System.Int32 amount, Planet final_target, Planet target, Planet source, Option<UnityShip> BaseShip)
	{JustEntered = false;
 frame = World.frame;
		UnityShip ___unityShip00;
		if(BaseShip.IsSome)
			{
			___unityShip00 = BaseShip.Value;
			}else
			{
			___unityShip00 = UnityShip.Instantiate(pos,target.Position);
			}
		UnityShip = ___unityShip00;
		Target = target;
		Source = source;
		Owner = o;
		InitialPosition = pos;
		FinalTarget = final_target;
		AmountOfShips = amount;
		
}
		public System.Int32 AmountOfShips;
	public System.Boolean Destroyed{  get { return UnityShip.Destroyed; }
  set{UnityShip.Destroyed = value; }
 }
	public System.Boolean ExplodeAndDestroyed{  get { return UnityShip.ExplodeAndDestroyed; }
  set{UnityShip.ExplodeAndDestroyed = value; }
 }
	public Planet FinalTarget;
	public UnityEngine.Vector3 InitialPosition;
	public UnityEngine.Color MiniMapColor{  get { return UnityShip.MiniMapColor; }
  set{UnityShip.MiniMapColor = value; }
 }
	public Commander Owner;
	public UnityEngine.Vector3 Position{  get { return UnityShip.Position; }
  set{UnityShip.Position = value; }
 }
	public UnityEngine.Vector3 Rotation{  set{UnityShip.Rotation = value; }
 }
	public System.Single Scale{  get { return UnityShip.Scale; }
  set{UnityShip.Scale = value; }
 }
	public System.String ShipOwnerText{  get { return UnityShip.ShipOwnerText; }
  set{UnityShip.ShipOwnerText = value; }
 }
	public UnityEngine.Vector3 ShipOwnerTextRotation{  get { return UnityShip.ShipOwnerTextRotation; }
  set{UnityShip.ShipOwnerTextRotation = value; }
 }
	public Planet Source;
	public Planet Target;
	public UnityShip UnityShip;
	public UnityEngine.Animation animation{  get { return UnityShip.animation; }
 }
	public UnityEngine.AudioSource audio{  get { return UnityShip.audio; }
 }
	public UnityEngine.Camera camera{  get { return UnityShip.camera; }
 }
	public UnityEngine.Collider collider{  get { return UnityShip.collider; }
 }
	public UnityEngine.Collider2D collider2D{  get { return UnityShip.collider2D; }
 }
	public UnityEngine.ConstantForce constantForce{  get { return UnityShip.constantForce; }
 }
	public System.Boolean enabled{  get { return UnityShip.enabled; }
  set{UnityShip.enabled = value; }
 }
	public UnityEngine.GameObject gameObject{  get { return UnityShip.gameObject; }
 }
	public UnityEngine.GUIElement guiElement{  get { return UnityShip.guiElement; }
 }
	public UnityEngine.GUIText guiText{  get { return UnityShip.guiText; }
 }
	public UnityEngine.GUITexture guiTexture{  get { return UnityShip.guiTexture; }
 }
	public UnityEngine.HideFlags hideFlags{  get { return UnityShip.hideFlags; }
  set{UnityShip.hideFlags = value; }
 }
	public UnityEngine.HingeJoint hingeJoint{  get { return UnityShip.hingeJoint; }
 }
	public UnityEngine.Light light{  get { return UnityShip.light; }
 }
	public System.String name{  get { return UnityShip.name; }
  set{UnityShip.name = value; }
 }
	public UnityEngine.ParticleEmitter particleEmitter{  get { return UnityShip.particleEmitter; }
 }
	public UnityEngine.ParticleSystem particleSystem{  get { return UnityShip.particleSystem; }
 }
	public UnityEngine.Renderer renderer{  get { return UnityShip.renderer; }
 }
	public UnityEngine.Rigidbody rigidbody{  get { return UnityShip.rigidbody; }
 }
	public UnityEngine.Rigidbody2D rigidbody2D{  get { return UnityShip.rigidbody2D; }
 }
	public System.String tag{  get { return UnityShip.tag; }
  set{UnityShip.tag = value; }
 }
	public UnityEngine.Transform transform{  get { return UnityShip.transform; }
 }
	public System.Boolean useGUILayout{  get { return UnityShip.useGUILayout; }
  set{UnityShip.useGUILayout = value; }
 }
	public System.Single ___cameraRotation01;
	public System.Int32 ___commander_number22;
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
	___cameraRotation01 = world.MainCamera.Rotation.x;
	ShipOwnerTextRotation = new UnityEngine.Vector3(___cameraRotation01,ShipOwnerTextRotation.y,ShipOwnerTextRotation.z);
	s0 = -1;
return;	
	default: return;}}
	

	int s1=-1;
	public void Rule1(float dt, World world){ 
	switch (s1)
	{

	case -1:
	if(!(Destroyed))
	{

	goto case 3;	}else
	{

	s1 = -1;
return;	}
	case 3:
	ShipOwnerText = (((((((((("") + (AmountOfShips))) + (" Ships    "))) + (this.Target.Name))) + ("-"))) + (this.FinalTarget.Name));
	s1 = -1;
return;	
	default: return;}}
	

	int s2=-1;
	public void Rule2(float dt, World world){ 
	switch (s2)
	{

	case -1:
	___commander_number22 = Owner.CommanderNumber;
	MiniMapColor = new UnityEngine.Color((0.2f) * (___commander_number22),0.1f,0.3f,1);
	s2 = 0;
return;
	case 0:
	if(!(false))
	{

	s2 = 0;
return;	}else
	{

	s2 = -1;
return;	}	
	default: return;}}
	

	int s3=-1;
	public void Rule3(float dt, World world){ 
	switch (s3)
	{

	case -1:
	Position = InitialPosition;
	s3 = 0;
return;
	case 0:
	if(!(false))
	{

	s3 = 0;
return;	}else
	{

	s3 = -1;
return;	}	
	default: return;}}
	





}
public class DefendingShip{
public int frame;
public bool JustEntered = true;
private Ship baseship;
	public int ID;
public DefendingShip(Ship baseship)
	{JustEntered = false;
 frame = World.frame;
		Strength = 0f;
		BaseShip = baseship;
		
}
		public Ship BaseShip;
	public System.Single Strength;
	public void Update(float dt, World world) {
frame = World.frame;

		BaseShip.Update(dt, world);


	}











}
public class AttackingShip{
public int frame;
public bool JustEntered = true;
private Ship baseship;
private Planet target;
	public int ID;
public AttackingShip(Ship baseship, Planet target)
	{JustEntered = false;
 frame = World.frame;
		Target = target;
		Strength = 0f;
		BaseShip = baseship;
		
}
		public Ship BaseShip;
	public System.Single Strength;
	public Planet Target;
	public System.Int32 ___ships_to_add00;
	public System.String ___res100;
	public void Update(float dt, World world) {
frame = World.frame;

		BaseShip.Update(dt, world);
		this.Rule0(dt, world);
		this.Rule1(dt, world);
		this.Rule2(dt, world);
	}





	int s0=-1;
	public void Rule0(float dt, World world){ 
	switch (s0)
	{

	case -1:
	if(((Target.Battle.Value.ShipsToMerge.Count) > (0)))
	{

	goto case 3;	}else
	{

	s0 = -1;
return;	}
	case 3:
	___ships_to_add00 = (

(Target.Battle.Value.ShipsToMerge).Select(__ContextSymbol216 => new { ___i07 = __ContextSymbol216 })
.Where(__ContextSymbol217 => ((!(__ContextSymbol217.___i07.BaseShip.Destroyed)) && (((BaseShip.Source) == (__ContextSymbol217.___i07.BaseShip.Source)))))
.Select(__ContextSymbol218 => __ContextSymbol218.___i07.BaseShip.AmountOfShips)
.Aggregate(default(System.Int32), (acc, __x) => acc + __x));
	___res100 = ___ships_to_add00.ToString();
	UnityEngine.Debug.Log(("merge ") + (___res100));
	BaseShip.AmountOfShips = ((BaseShip.AmountOfShips) + (___ships_to_add00));
	s0 = -1;
return;	
	default: return;}}
	

	int s1=-1;
	public void Rule1(float dt, World world){ 
	switch (s1)
	{

	case -1:
	if(((((BaseShip.AmountOfShips) == (0))) && (Target.Owner.IsSome)))
	{

	goto case 9;	}else
	{

	s1 = -1;
return;	}
	case 9:
	BaseShip.ExplodeAndDestroyed = true;
	s1 = -1;
return;	
	default: return;}}
	

	int s2=-1;
	public void Rule2(float dt, World world){ 
	switch (s2)
	{

	case -1:
	if(((Target.Owner.IsSome) && (((BaseShip.Owner) == (Target.Owner.Value)))))
	{

	goto case 12;	}else
	{

	s2 = -1;
return;	}
	case 12:
	BaseShip.Destroyed = true;
	s2 = -1;
return;	
	default: return;}}
	





}
public class LandingShip{
public int frame;
public bool JustEntered = true;
private Ship baseship;
	public int ID;
public LandingShip(Ship baseship)
	{JustEntered = false;
 frame = World.frame;
		BaseShip = baseship;
		
}
		public Ship BaseShip;
	public void Update(float dt, World world) {
frame = World.frame;

		BaseShip.Update(dt, world);
		this.Rule0(dt, world);

	}





	int s0=-1;
	public void Rule0(float dt, World world){ 
	switch (s0)
	{

	case -1:
	if(((((BaseShip.AmountOfShips) == (0))) || (((BaseShip.Target.Owner.IsSome) && (((BaseShip.Owner) == (BaseShip.Target.Owner.Value)))))))
	{

	goto case 15;	}else
	{

	s0 = -1;
return;	}
	case 15:
	BaseShip.Destroyed = true;
	BaseShip.Target.LandedShips = ((BaseShip.AmountOfShips) + (BaseShip.Target.LandedShips));
	BaseShip.AmountOfShips = 0;
	s0 = -1;
return;	
	default: return;}}
	






}
public class MergingShip{
public int frame;
public bool JustEntered = true;
private Ship baseship;
	public int ID;
public MergingShip(Ship baseship)
	{JustEntered = false;
 frame = World.frame;
		BaseShip = baseship;
		
}
		public Ship BaseShip;
	public void Update(float dt, World world) {
frame = World.frame;

		BaseShip.Update(dt, world);
		this.Rule0(dt, world);

	}





	int s0=-1;
	public void Rule0(float dt, World world){ 
	switch (s0)
	{

	case -1:
	BaseShip.Destroyed = true;
	s0 = -1;
return;	
	default: return;}}
	






}
public class TravelingShip{
public int frame;
public bool JustEntered = true;
private Planet n;
private Planet s;
private Ship baseship;
	public int ID;
public TravelingShip(Planet n, Planet s, Ship baseship)
	{JustEntered = false;
 frame = World.frame;
		Velocity = new UnityEngine.Vector3(1f,1f,1f);
		Target = n;
		Source = s;
		BaseShip = baseship;
		
}
		public Ship BaseShip;
	public Planet Source;
	public Planet Target;
	public UnityEngine.Vector3 Velocity;
	public UnityEngine.Vector3 ___direction10;
	public UnityEngine.Vector3 ___direction11;
	public List<Planet> ___next_hop14;
	public Planet ___next_hop15;
	public void Update(float dt, World world) {
frame = World.frame;

		BaseShip.Update(dt, world);
		this.Rule0(dt, world);
		this.Rule1(dt, world);
	}





	int s0=-1;
	public void Rule0(float dt, World world){ 
	switch (s0)
	{

	case -1:
	BaseShip.Position = ((BaseShip.Position) + (((Velocity) * (dt))));
	s0 = -1;
return;	
	default: return;}}
	

	int s1=-1;
	public void Rule1(float dt, World world){ 
	switch (s1)
	{

	case -1:
	if(!(Source.PlanetStats.IsSome))
	{

	s1 = -1;
return;	}else
	{

	goto case 6;	}
	case 6:
	___direction10 = ((Target.Position) - (BaseShip.Position));
	if(((((Target.Owner.IsSome) && (((Target.Owner.Value) == (BaseShip.Owner))))) && (((Target.PlanetStats.Value.Research) > (Source.PlanetStats.Value.Research)))))
	{

	___direction11 = ((___direction10.normalized) * (((2.5f) + (((0.35f) * (Target.PlanetStats.Value.Research))))));	}else
	{

	___direction11 = ((___direction10.normalized) * (((2.5f) + (((0.35f) * (Source.PlanetStats.Value.Research))))));	}
	Velocity = ___direction11;
	BaseShip.Position = BaseShip.Position;
	Target = Target;
	s1 = 3;
return;
	case 3:
	if(!(((0f) > (UnityEngine.Vector3.Dot(___direction11,(Target.Position) - (BaseShip.Position))))))
	{

	s1 = 3;
return;	}else
	{

	goto case 2;	}
	case 2:
	___next_hop14 = (

(Target.Neighbours).Select(__ContextSymbol220 => new { ___n10 = __ContextSymbol220 })
.SelectMany(__ContextSymbol221=> (world.Planets).Select(__ContextSymbol222 => new { ___q14 = __ContextSymbol222,
                                                      prev = __ContextSymbol221 })
.Where(__ContextSymbol223 => ((((__ContextSymbol223.prev.___n10.Item1) == (Target.UnityPlanet))) && (((__ContextSymbol223.prev.___n10.Item2) == (__ContextSymbol223.___q14.UnityPlanet)))))
.Select(__ContextSymbol224 => __ContextSymbol224.___q14)
.ToList<Planet>())).ToList<Planet>();
	if(((___next_hop14.Count) > (0)))
	{

	___next_hop15 = ___next_hop14.Head();	}else
	{

	___next_hop15 = Target;	}
	Velocity = Vector3.zero;
	BaseShip.Position = Target.Position;
	Target = ___next_hop15;
	s1 = -1;
return;	
	default: return;}}
	





}
public class ResourceBar{
public int frame;
public bool JustEntered = true;
private List<Commander> coms;
	public int ID;
public ResourceBar(List<Commander> coms)
	{JustEntered = false;
 frame = World.frame;
		List<ResourceText> ___playerstats00;
		___playerstats00 = (

(Enumerable.Range(1,(1) + ((coms.Count) - (1))).ToList<System.Int32>()).Select(__ContextSymbol225 => new { ___i08 = __ContextSymbol225 })
.Select(__ContextSymbol226 => new ResourceText(coms[(__ContextSymbol226.___i08) - (1)],(__ContextSymbol226.___i08) - (1),coms.Count))
.ToList<ResourceText>()).ToList<ResourceText>();
		PlayerStats = ___playerstats00;
		
}
		public List<ResourceText> PlayerStats;
	public void Update(float dt, World world) {
frame = World.frame;

		for(int x0 = 0; x0 < PlayerStats.Count; x0++) { 
			PlayerStats[x0].Update(dt, world);
		}


	}











}
public class ResourceText{
public int frame;
public bool JustEntered = true;
private Commander n;
private System.Int32 i;
private System.Int32 maxPlayers;
	public int ID;
public ResourceText(Commander n, System.Int32 i, System.Int32 maxPlayers)
	{JustEntered = false;
 frame = World.frame;
		PlayerStats = new TextGUI("Prefabs/PlayerStats","PlayerStatGUI/StatsPanel",i,maxPlayers);
		Player = n;
		
}
		public Commander Player;
	public TextGUI PlayerStats;
	public System.String ___attack01;
	public System.String ___defense01;
	public System.String ___production00;
	public System.String ___research01;
	public void Update(float dt, World world) {
frame = World.frame;

		PlayerStats.Update(dt, world);
		this.Rule0(dt, world);

	}





	int s0=-1;
	public void Rule0(float dt, World world){ 
	switch (s0)
	{

	case -1:
	___attack01 = Player.Attack.ToString();
	___defense01 = Player.Defense.ToString();
	___production00 = Player.Production.ToString();
	___research01 = Player.Research.ToString();
	PlayerStats.Text = ((((((((((((((((Player.Name) + ("	"))) + (___attack01))) + (" "))) + (___defense01))) + (" "))) + (___production00))) + (" "))) + (___research01));
	s0 = -1;
return;	
	default: return;}}
	






}
public class TextGUI{
public int frame;
public bool JustEntered = true;
private System.String n;
private System.String canvasName;
private System.Int32 i;
private System.Int32 maxPlayers;
	public int ID;
public TextGUI(System.String n, System.String canvasName, System.Int32 i, System.Int32 maxPlayers)
	{JustEntered = false;
 frame = World.frame;
		UnityText = UnityText.Instantiate(n,canvasName,i,maxPlayers);
		
}
		public System.String Text{  get { return UnityText.Text; }
  set{UnityText.Text = value; }
 }
	public UnityText UnityText;
	public UnityEngine.Animation animation{  get { return UnityText.animation; }
 }
	public UnityEngine.AudioSource audio{  get { return UnityText.audio; }
 }
	public UnityEngine.Camera camera{  get { return UnityText.camera; }
 }
	public UnityEngine.Collider collider{  get { return UnityText.collider; }
 }
	public UnityEngine.Collider2D collider2D{  get { return UnityText.collider2D; }
 }
	public UnityEngine.ConstantForce constantForce{  get { return UnityText.constantForce; }
 }
	public System.Boolean enabled{  get { return UnityText.enabled; }
  set{UnityText.enabled = value; }
 }
	public UnityEngine.GameObject gameObject{  get { return UnityText.gameObject; }
 }
	public UnityEngine.GUIElement guiElement{  get { return UnityText.guiElement; }
 }
	public UnityEngine.GUIText guiText{  get { return UnityText.guiText; }
 }
	public UnityEngine.GUITexture guiTexture{  get { return UnityText.guiTexture; }
 }
	public UnityEngine.HideFlags hideFlags{  get { return UnityText.hideFlags; }
  set{UnityText.hideFlags = value; }
 }
	public UnityEngine.HingeJoint hingeJoint{  get { return UnityText.hingeJoint; }
 }
	public UnityEngine.Light light{  get { return UnityText.light; }
 }
	public System.String name{  get { return UnityText.name; }
  set{UnityText.name = value; }
 }
	public UnityEngine.ParticleEmitter particleEmitter{  get { return UnityText.particleEmitter; }
 }
	public UnityEngine.ParticleSystem particleSystem{  get { return UnityText.particleSystem; }
 }
	public UnityEngine.Renderer renderer{  get { return UnityText.renderer; }
 }
	public UnityEngine.Rigidbody rigidbody{  get { return UnityText.rigidbody; }
 }
	public UnityEngine.Rigidbody2D rigidbody2D{  get { return UnityText.rigidbody2D; }
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
}                                         