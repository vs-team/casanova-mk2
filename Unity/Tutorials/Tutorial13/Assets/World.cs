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
	{
		System.Collections.Generic.List<UnityPlanet> ___unityPlanets00;
		___unityPlanets00 = UnityPlanet.FindAllPlanets();
		List<Planet> ___planets00;
		___planets00 = (

(___unityPlanets00).Select(__ContextSymbol0 => new { ___p00 = __ContextSymbol0 })
.Select(__ContextSymbol1 => new Planet(__ContextSymbol1.___p00))
.ToList<Planet>()).ToList<Planet>();
		List<Link> ___links00;
		___links00 = (

(___planets00).Select(__ContextSymbol2 => new { ___p100 = __ContextSymbol2 })
.SelectMany(__ContextSymbol3=> (___planets00).Select(__ContextSymbol4 => new { ___p200 = __ContextSymbol4,
                                                      prev = __ContextSymbol3 })
.Where(__ContextSymbol5 => !(((__ContextSymbol5.prev.___p100) == (__ContextSymbol5.___p200))))
.Select(__ContextSymbol6 => new Link(__ContextSymbol6.prev.___p100,__ContextSymbol6.___p200))
.ToList<Link>())).ToList<Link>();
		SelectedPlanets = (new Nothing<Planet>());
		Planets = ___planets00;
		Links = ___links00;
		
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
	public Option<Planet> __SelectedPlanets;
	public Option<Planet> SelectedPlanets{  get { return  __SelectedPlanets; }
  set{ __SelectedPlanets = value;
 if(value.IsSome){if(!value.Value.JustEntered) __SelectedPlanets = value; 
 else{ value.Value.JustEntered = false;
}
} }
 }
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
public class Link{
public int frame;
public bool JustEntered = true;
private Planet p1;
private Planet p2;
	public int ID;
public Link(Planet p1, Planet p2)
	{JustEntered = false;
 frame = World.frame;
		Start = p1;
		Ships = (

Enumerable.Empty<Ship>()).ToList<Ship>();
		End = p2;
		Active = false;
		
}
		public System.Boolean Active;
	public Planet End;
	public List<Ship> Ships;
	public Planet Start;
	public System.Single count_down1;
	public void Update(float dt, World world) {
frame = World.frame;		this.Rule0(dt, world);

		for(int x0 = 0; x0 < Ships.Count; x0++) { 
			Ships[x0].Update(dt, world);
		}
		this.Rule1(dt, world);
		this.Rule2(dt, world);
	}

	public void Rule0(float dt, World world) 
	{
	Ships = (

(Ships).Select(__ContextSymbol9 => new { ___t00 = __ContextSymbol9 })
.Where(__ContextSymbol10 => ((__ContextSymbol10.___t00.Destroyed) == (false)))
.Select(__ContextSymbol11 => __ContextSymbol11.___t00)
.ToList<Ship>()).ToList<Ship>();
	}
	




	int s1=-1;
	public void Rule1(float dt, World world){ 
	switch (s1)
	{

	case -1:
	if(!(Active))
	{

	s1 = -1;
return;	}else
	{

	goto case 2;	}
	case 2:
	count_down1 = 2f;
	goto case 3;
	case 3:
	if(((count_down1) > (0f)))
	{

	count_down1 = ((count_down1) - (dt));
	s1 = 3;
return;	}else
	{

	goto case 1;	}
	case 1:
	if(!(((Start.UnityPlanet.Ships) > (0))))
	{

	s1 = 1;
return;	}else
	{

	goto case 0;	}
	case 0:
	Ships = new Cons<Ship>(new Ship(Start,End), (Ships)).ToList<Ship>();
	s1 = -1;
return;	
	default: return;}}
	

	int s2=-1;
	public void Rule2(float dt, World world){ 
	switch (s2)
	{

	case -1:
	if(!(((Start.Selected) && (End.Targeted))))
	{

	s2 = -1;
return;	}else
	{

	goto case 1;	}
	case 1:
	Active = !(Active);
	s2 = 0;
return;
	case 0:
	if(!(((!(Start.Selected)) || (!(End.Targeted)))))
	{

	s2 = 0;
return;	}else
	{

	s2 = -1;
return;	}	
	default: return;}}
	





}
public class Ship{
public int frame;
public bool JustEntered = true;
private Planet p1;
private Planet p2;
	public int ID;
public Ship(Planet p1, Planet p2)
	{JustEntered = false;
 frame = World.frame;
		UnityEngine.Vector3 ___v00;
		___v00 = (p2.Position) - (p1.Position);
		UnityShip = UnityShip.Instantiate((p1.UnityPlanet.Position) + (___v00.normalized),p2.UnityPlanet.Position,___v00);
		Start = p1;
		ReachedDestination = false;
		End = p2;
		ClosestObject = new Collider();
		
}
		public Collider ClosestObject;
	public System.Boolean Destroyed{  get { return UnityShip.Destroyed; }
  set{UnityShip.Destroyed = value; }
 }
	public Planet End;
	public System.Collections.Generic.List<Collider> PlanetsAndStars{  get { return UnityShip.PlanetsAndStars; }
  set{UnityShip.PlanetsAndStars = value; }
 }
	public UnityEngine.Vector3 Position{  get { return UnityShip.Position; }
  set{UnityShip.Position = value; }
 }
	public System.Boolean ReachedDestination;
	public System.Single Scale{  get { return UnityShip.Scale; }
  set{UnityShip.Scale = value; }
 }
	public Planet Start;
	public UnityShip UnityShip;
	public UnityEngine.Vector3 Velocity{  get { return UnityShip.Velocity; }
  set{UnityShip.Velocity = value; }
 }
	public System.Single alpha{  get { return UnityShip.alpha; }
  set{UnityShip.alpha = value; }
 }
	public UnityEngine.Animation animation{  get { return UnityShip.animation; }
 }
	public UnityEngine.AudioSource audio{  get { return UnityShip.audio; }
 }
	public UnityEngine.Vector3 avoidance{  get { return UnityShip.avoidance; }
  set{UnityShip.avoidance = value; }
 }
	public UnityEngine.Camera camera{  get { return UnityShip.camera; }
 }
	public System.String closestPlanetName{  get { return UnityShip.closestPlanetName; }
  set{UnityShip.closestPlanetName = value; }
 }
	public UnityEngine.Collider collider{  get { return UnityShip.collider; }
 }
	public UnityEngine.Collider2D collider2D{  get { return UnityShip.collider2D; }
 }
	public UnityEngine.ConstantForce constantForce{  get { return UnityShip.constantForce; }
 }
	public System.Single dist{  get { return UnityShip.dist; }
  set{UnityShip.dist = value; }
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
	public UnityEngine.Vector3 ___target_dir50;
	public UnityEngine.Vector3 ___closest_planet_dir50;
	public UnityEngine.Vector3 ___avoidance_dir50;
	public System.Single ___closest_planet_distance50;
	public System.Single ___alpha50;
	public Collider ___p61;
	public System.Int32 counter1;
	public System.Single ___closest_planet_distance61;
	public System.Single ___p_distance60;
	public void Update(float dt, World world) {
frame = World.frame;		this.Rule3(dt, world);
		this.Rule4(dt, world);
		this.Rule0(dt, world);
		this.Rule1(dt, world);
		this.Rule2(dt, world);
		this.Rule5(dt, world);
		this.Rule6(dt, world);
	}

	public void Rule3(float dt, World world) 
	{
	ReachedDestination = ((0f) > (UnityEngine.Vector3.Dot(Velocity,(End.Position) - (Position))));
	}
	

	public void Rule4(float dt, World world) 
	{
	Position = (Position) + ((Velocity) * (dt));
	}
	



	int s0=-1;
	public void Rule0(float dt, World world){ 
	switch (s0)
	{

	case -1:
	if(!(((1f) > (Scale))))
	{

	goto case 3;	}else
	{

	goto case 5;	}
	case 5:
	Scale = UnityEngine.Mathf.Min(1f,(Scale) + ((dt) * (10f)));
	s0 = -1;
return;
	case 3:
	if(!(((1f) > (UnityEngine.Vector3.Distance(End.Position,Position)))))
	{

	s0 = 3;
return;	}else
	{

	goto case 0;	}
	case 0:
	if(!(true))
	{

	s0 = -1;
return;	}else
	{

	goto case 1;	}
	case 1:
	Scale = UnityEngine.Mathf.Max(0f,(Scale) - ((dt) * (10f)));
	s0 = 0;
return;	
	default: return;}}
	

	int s1=-1;
	public void Rule1(float dt, World world){ 
	switch (s1)
	{

	case -1:
	if(!(ReachedDestination))
	{

	s1 = -1;
return;	}else
	{

	goto case 0;	}
	case 0:
	Velocity = Vector3.zero;
	Destroyed = true;
	s1 = -1;
return;	
	default: return;}}
	

	int s2=-1;
	public void Rule2(float dt, World world){ 
	switch (s2)
	{

	case -1:
	Start.UnityPlanet.Ships = ((Start.UnityPlanet.Ships) - (1));
	End.UnityPlanet.Ships = End.UnityPlanet.Ships;
	s2 = 2;
return;
	case 2:
	if(!(ReachedDestination))
	{

	s2 = 2;
return;	}else
	{

	goto case 1;	}
	case 1:
	Start.UnityPlanet.Ships = Start.UnityPlanet.Ships;
	End.UnityPlanet.Ships = ((End.UnityPlanet.Ships) + (1));
	s2 = 0;
return;
	case 0:
	if(!(Destroyed))
	{

	s2 = 0;
return;	}else
	{

	s2 = -1;
return;	}	
	default: return;}}
	

	int s5=-1;
	public void Rule5(float dt, World world){ 
	switch (s5)
	{

	case -1:
	if(!(((!(((ClosestObject.Position) == (Start.Position)))) && (!(((ClosestObject.Position) == (End.Position)))))))
	{

	s5 = -1;
return;	}else
	{

	goto case 5;	}
	case 5:
	___target_dir50 = UnityEngine.Vector3.Normalize((End.Position) - (Position));
	___closest_planet_dir50 = UnityEngine.Vector3.Normalize((Position) - (ClosestObject.Position));
	___avoidance_dir50 = ((___closest_planet_dir50) - (((___target_dir50) * (UnityEngine.Vector3.Dot(___target_dir50,___closest_planet_dir50)))));
	___closest_planet_distance50 = UnityEngine.Vector3.Distance(ClosestObject.Position,Position);
	___alpha50 = ((1f) - (UnityEngine.Mathf.Clamp((___closest_planet_distance50) / ((ClosestObject.Radius) + (2f)),0f,1f)));
	Velocity = UnityEngine.Vector3.Lerp(___target_dir50,___avoidance_dir50.normalized,___alpha50);
	alpha = ___alpha50;
	dist = ___closest_planet_distance50;
	avoidance = ___avoidance_dir50;
	s5 = -1;
return;	
	default: return;}}
	

	int s6=-1;
	public void Rule6(float dt, World world){ 
	switch (s6)
	{

	case -1:
	
	counter1 = -1;
	if((((PlanetsAndStars).Count) == (0)))
	{

	s6 = -1;
return;	}else
	{

	___p61 = (PlanetsAndStars)[0];
	goto case 1;	}
	case 1:
	counter1 = ((counter1) + (1));
	if((((((PlanetsAndStars).Count) == (counter1))) || (((counter1) > ((PlanetsAndStars).Count)))))
	{

	s6 = -1;
return;	}else
	{

	___p61 = (PlanetsAndStars)[counter1];
	goto case 2;	}
	case 2:
	if(!(((___p61) == (ClosestObject))))
	{

	goto case 4;	}else
	{

	s6 = 1;
return;	}
	case 4:
	___closest_planet_distance61 = UnityEngine.Vector3.Distance(ClosestObject.Position,Position);
	___p_distance60 = UnityEngine.Vector3.Distance(___p61.Position,Position);
	if(((___closest_planet_distance61) > (___p_distance60)))
	{

	goto case 6;	}else
	{

	s6 = 1;
return;	}
	case 6:
	ClosestObject = ___p61;
	closestPlanetName = ___p61.gameObject.transform.name;
	s6 = 1;
return;	
	default: return;}}
	





}
public class Planet{
public int frame;
public bool JustEntered = true;
private UnityPlanet p;
	public int ID;
public Planet(UnityPlanet p)
	{JustEntered = false;
 frame = World.frame;
		UnityPlanet = p;
		Targeted = false;
		
}
		public System.Boolean ClickedOver{  get { return UnityPlanet.ClickedOver; }
 }
	public UnityEngine.Vector3 Position{  get { return UnityPlanet.Position; }
  set{UnityPlanet.Position = value; }
 }
	public System.Boolean Selected{  get { return UnityPlanet.Selected; }
  set{UnityPlanet.Selected = value; }
 }
	public System.Int32 Ships{  get { return UnityPlanet.Ships; }
  set{UnityPlanet.Ships = value; }
 }
	public System.Boolean Targeted;
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
	public System.Single count_down2;
	public List<Link> ___connections10;
	public void Update(float dt, World world) {
frame = World.frame;

		this.Rule0(dt, world);
		this.Rule1(dt, world);
		this.Rule2(dt, world);
	}





	int s0=-1;
	public void Rule0(float dt, World world){ 
	switch (s0)
	{

	case -1:
	count_down2 = 1f;
	goto case 2;
	case 2:
	if(((count_down2) > (0f)))
	{

	count_down2 = ((count_down2) - (dt));
	s0 = 2;
return;	}else
	{

	goto case 0;	}
	case 0:
	Ships = ((Ships) + (1));
	s0 = -1;
return;	
	default: return;}}
	

	int s1=-1;
	public void Rule1(float dt, World world){ 
	switch (s1)
	{

	case -1:
	if(!(UnityEngine.Input.GetMouseButtonDown(1)))
	{

	s1 = -1;
return;	}else
	{

	goto case 7;	}
	case 7:
	___connections10 = (

(world.Links).Select(__ContextSymbol12 => new { ___l10 = __ContextSymbol12 })
.Where(__ContextSymbol13 => ((((__ContextSymbol13.___l10.End) == (this))) && (__ContextSymbol13.___l10.Start.Selected)))
.Select(__ContextSymbol14 => __ContextSymbol14.___l10)
.ToList<Link>()).ToList<Link>();
	if(((UnityPlanet.ClickedOver) && (Targeted)))
	{

	goto case 0;	}else
	{

	goto case 1;	}
	case 0:
	Targeted = true;
	s1 = -1;
return;
	case 1:
	if(((((UnityPlanet.ClickedOver) && (!(Targeted)))) && (((___connections10.Count) > (0)))))
	{

	goto case 5;	}else
	{

	s1 = -1;
return;	}
	case 5:
	Targeted = true;
	s1 = -1;
return;	
	default: return;}}
	

	int s2=-1;
	public void Rule2(float dt, World world){ 
	switch (s2)
	{

	case -1:
	if(!(UnityEngine.Input.GetMouseButtonDown(0)))
	{

	s2 = -1;
return;	}else
	{

	goto case 2;	}
	case 2:
	if(UnityPlanet.ClickedOver)
	{

	goto case 0;	}else
	{

	goto case 1;	}
	case 0:
	Selected = true;
	world.SelectedPlanets = (new Just<Planet>(this));
	Targeted = false;
	s2 = -1;
return;
	case 1:
	if(world.SelectedPlanets.IsSome)
	{

	goto case 5;	}else
	{

	s2 = -1;
return;	}
	case 5:
	Selected = false;
	world.SelectedPlanets = world.SelectedPlanets;
	Targeted = false;
	s2 = -1;
return;	
	default: return;}}
	





}
    