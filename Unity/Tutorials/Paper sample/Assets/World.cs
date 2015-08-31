#pragma warning disable 162,108,618
using Casanova.Prelude;
using System.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;
public class World : MonoBehaviour
{
  void OnApplicationQuit() { file.Close(); }public static int frame;
  void Update()
  {
    Update(Time.deltaTime, this);
    frame++;
  }
  public bool JustEntered = true;
  static public int StarSystemCounter;
  static public int PlanetCounter;
  static public int ShipCounter;
  static public Dictionary<int, Tuple<StarSystem, RuleTable>> StarSystemWithActiveRules;
  static public Dictionary<int, Tuple<Planet, RuleTable>> PlanetWithActiveRules;
  static public Dictionary<int, Tuple<Ship, RuleTable>> ShipWithActiveRules;
  static public List<int> StarSystemWithActiveRulesToRemove;
  static public List<int> PlanetWithActiveRulesToRemove;
  static public List<int> ShipWithActiveRulesToRemove;
  static public Dictionary<int, List<StarSystem>> NotifySlotStarSystemShipsStarSystem1;
  static public Dictionary<int, List<Planet>> NotifySlotStarSystemShipsPlanet2;
  static public Dictionary<int, List<Ship>> NotifySlotShipArrivedShip0;
  static public Dictionary<int, List<Ship>> NotifySlotShipArrivedShip1;
  static public Dictionary<int, List<Ship>> NotifySlotShipDestroyedShip0;

  System.IO.StreamWriter file;
  public void Start()
  {
    file = new System.IO.StreamWriter(System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "test.csv")); StarSystemCounter = 0;
    PlanetCounter = 0;
    ShipCounter = 0;
    NotifySlotStarSystemShipsStarSystem1 = new Dictionary<int, List<StarSystem>>();
    NotifySlotStarSystemShipsPlanet2 = new Dictionary<int, List<Planet>>();
    NotifySlotShipArrivedShip0 = new Dictionary<int, List<Ship>>();
    NotifySlotShipArrivedShip1 = new Dictionary<int, List<Ship>>();
    NotifySlotShipDestroyedShip0 = new Dictionary<int, List<Ship>>();
    StarSystemWithActiveRules = new Dictionary<int, Tuple<StarSystem, RuleTable>>();
    PlanetWithActiveRules = new Dictionary<int, Tuple<Planet, RuleTable>>();
    ShipWithActiveRules = new Dictionary<int, Tuple<Ship, RuleTable>>();
    StarSystemWithActiveRulesToRemove = new List<int>();
    PlanetWithActiveRulesToRemove = new List<int>();
    ShipWithActiveRulesToRemove = new List<int>();
    List<StarSystem> ___star_systems00;
    ___star_systems00 = (

(Enumerable.Range(0, (1) + ((10) - (0))).ToList<System.Int32>()).Select(__ContextSymbol0 => new { ___i00 = __ContextSymbol0 })
.SelectMany(__ContextSymbol1 => (Enumerable.Range(0, (1) + ((10) - (0))).ToList<System.Int32>()).Select(__ContextSymbol2 => new
{
  ___j00 = __ContextSymbol2,
  prev = __ContextSymbol1
})
.Where(__ContextSymbol3 => ((((2) > (__ContextSymbol3.prev.___i00))) || (((__ContextSymbol3.___j00) > (5)))))
.Select(__ContextSymbol4 => new StarSystem())
.ToList<StarSystem>())).ToList<StarSystem>();
    StarSystem = ___star_systems00;
    Seed = new System.Random();

  }
  public System.Random Seed;
  public List<StarSystem> __StarSystem;
  public List<StarSystem> StarSystem
  {
    get { return __StarSystem; }
    set
    {
      __StarSystem = value;
      foreach (var e in value)
      {
        if (e.JustEntered)
        {
          e.JustEntered = false;
          World.NotifySlotStarSystemShipsStarSystem1[e.ID].Add(e);
        }
      }
    }
  }

  System.DateTime init_time = System.DateTime.Now;
  public void Update(float dt, World world)
  {
    var t = System.DateTime.Now;

    for (int x0 = 0; x0 < StarSystem.Count; x0++)
    {
      StarSystem[x0].Update(dt, world);
    }


    if (StarSystemWithActiveRules.Count > 0)
    {
      foreach (var x in StarSystemWithActiveRules)
        x.Value.Item1.UpdateSuspendedRules(dt, this, StarSystemWithActiveRulesToRemove, x.Value.Item2);
      if (StarSystemWithActiveRulesToRemove.Count > 0)
      {
        for (int i = 0; i < StarSystemWithActiveRulesToRemove.Count; i++)
          StarSystemWithActiveRules.Remove(StarSystemWithActiveRulesToRemove[i]);
        StarSystemWithActiveRulesToRemove.Clear();
      }
    }
    if (PlanetWithActiveRules.Count > 0)
    {
      foreach (var x in PlanetWithActiveRules)
        x.Value.Item1.UpdateSuspendedRules(dt, this, PlanetWithActiveRulesToRemove, x.Value.Item2);
      if (PlanetWithActiveRulesToRemove.Count > 0)
      {
        for (int i = 0; i < PlanetWithActiveRulesToRemove.Count; i++)
          PlanetWithActiveRules.Remove(PlanetWithActiveRulesToRemove[i]);
        PlanetWithActiveRulesToRemove.Clear();
      }
    }
    if (ShipWithActiveRules.Count > 0)
    {
      foreach (var x in ShipWithActiveRules)
        x.Value.Item1.UpdateSuspendedRules(dt, this, ShipWithActiveRulesToRemove, x.Value.Item2);
      if (ShipWithActiveRulesToRemove.Count > 0)
      {
        for (int i = 0; i < ShipWithActiveRulesToRemove.Count; i++)
          ShipWithActiveRules.Remove(ShipWithActiveRulesToRemove[i]);
        ShipWithActiveRulesToRemove.Clear();
      }
    }
    var t1 = System.DateTime.Now;
    file.WriteLine((t1 - t).Milliseconds + "," + (t1 - init_time).Seconds);
  }











}
public class StarSystem
{
  public int frame;
  public bool JustEntered = true;
  public int ID;
  public StarSystem()
  {
    JustEntered = false;
    frame = World.frame;

    this.ID = World.StarSystemCounter++;
    World.NotifySlotStarSystemShipsStarSystem1.Add(ID, new List<StarSystem>());
    World.NotifySlotStarSystemShipsPlanet2.Add(ID, new List<Planet>());
    World.NotifySlotStarSystemShipsStarSystem1[ID].Add(this);
    Ships = (

Enumerable.Empty<Ship>()).ToList<Ship>();
    Planets = (

Enumerable.Empty<Planet>()).ToList<Planet>();

    StarSystem1 = new List<Ship>(Ships);
    List<Ship> q;
    q = (

(Ships).Select(__ContextSymbol7 => new { ___s10 = __ContextSymbol7 })
.Where(__ContextSymbol8 => ((!(__ContextSymbol8.___s10.Arrived)) && (!(__ContextSymbol8.___s10.Destroyed))))
.Select(__ContextSymbol9 => __ContextSymbol9.___s10)
.ToList<Ship>()).ToList<Ship>();
    StarSystem1 = q;
  }
  public void Init()
  {
    Ships = (

      Enumerable.Empty<Ship>()).ToList<Ship>();
    Planets = (

Enumerable.Empty<Planet>()).ToList<Planet>();

    StarSystem1 = new List<Ship>(Ships);
    List<Ship> q;
    q = (

(Ships).Select(__ContextSymbol12 => new { ___s10 = __ContextSymbol12 })
.Where(__ContextSymbol13 => ((!(__ContextSymbol13.___s10.Arrived)) && (!(__ContextSymbol13.___s10.Destroyed))))
.Select(__ContextSymbol14 => __ContextSymbol14.___s10)
.ToList<Ship>()).ToList<Ship>();
    StarSystem1 = q;

  }
  public List<Planet> Planets;
  public List<Ship> _Ships;
  public List<Ship> StarSystem1;
  public System.Boolean q_temp1;
  public Ship s;
  public System.Int32 counter3;
  public System.Single count_down2;
  public System.Single count_down1;
  public RuleTable ActiveRules = new RuleTable(1);
  public List<Ship> Ships
  {
    get { return _Ships; }

    set
    {
      _Ships = value;

      q_temp1 = true;

      //q_temp2 = true;
      for (int i = 0; i < World.NotifySlotStarSystemShipsStarSystem1[ID].Count; i++)
      {
        var entity = World.NotifySlotStarSystemShipsStarSystem1[ID][i];
        if (entity.frame == World.frame)
        {
          if (!World.StarSystemWithActiveRules.ContainsKey(entity.ID))
            World.StarSystemWithActiveRules.Add(entity.ID, new Tuple<StarSystem, RuleTable>(entity, new RuleTable(3))); World.StarSystemWithActiveRules[entity.ID].Item2.Add(1);
        }
        else
        {
          entity.JustEntered = true;
          World.NotifySlotStarSystemShipsStarSystem1.Remove(entity.ID);
        }
      }
      for (int i = 0; i < World.NotifySlotStarSystemShipsPlanet2[ID].Count; i++)
      {
        var entity = World.NotifySlotStarSystemShipsPlanet2[ID][i];
        if (entity.frame == World.frame)
        {
          if (!World.PlanetWithActiveRules.ContainsKey(entity.ID))
            World.PlanetWithActiveRules.Add(entity.ID, new Tuple<Planet, RuleTable>(entity, new RuleTable(3))); World.PlanetWithActiveRules[entity.ID].Item2.Add(2);
        }
        else
        {
          entity.JustEntered = true;
          World.NotifySlotStarSystemShipsStarSystem1.Remove(entity.ID);
        }
      }
    }
  }
  public void Update(float dt, World world)
  {
    frame = World.frame;

    for (int x0 = 0; x0 < Planets.Count; x0++)
    {
      Planets[x0].Update(dt, world);
    }
    for (int x0 = 0; x0 < Ships.Count; x0++)
    {
      Ships[x0].Update(dt, world);
    }
    this.Rule0(dt, world);
    this.Rule2(dt, world);
  }
  public void UpdateSuspendedRules(float dt, World world, List<int> toRemove, RuleTable ActiveRules)
  {
    if (ActiveRules.ActiveIndices.Top > 0 && frame == World.frame)
    {
      for (int i = 0; i < ActiveRules.ActiveIndices.Top; i++)
      {
        var x = ActiveRules.ActiveIndices.Elements[i];
        switch (x)
        {
          case 1:
            if (this.Rule1(dt, world) == RuleResult.Done)
            {
              ActiveRules.ActiveSlots[i] = false;
              ActiveRules.ActiveIndices.Top--;
            }
            else
            {
              ActiveRules.SupportSlots[1] = true;
              ActiveRules.SupportStack.Push(x);
            }
            break;

          default:
            break;
        }
      }
      ActiveRules.ActiveIndices.Clear();
      ActiveRules.Clear();

      var tmp = ActiveRules.SupportStack;
      var tmp1 = ActiveRules.SupportSlots;

      ActiveRules.SupportStack = ActiveRules.ActiveIndices;
      ActiveRules.SupportSlots = ActiveRules.ActiveSlots;


      ActiveRules.ActiveIndices = tmp;
      ActiveRules.ActiveSlots = tmp1;

      if (ActiveRules.ActiveIndices.Top == 0)
        toRemove.Add(ID);
    }
    else
    {
      if (this.frame != World.frame)
        toRemove.Add(ID);
    }
  }





  int s0 = -1;
  public void Rule0(float dt, World world)
  {
    switch (s0)
    {

      case -1:
        Planets = (

      (PointGenerator.GeneratePoints(50)).Select(__ContextSymbol15 => new { ___p00 = __ContextSymbol15 })
      .Select(__ContextSymbol16 => new Planet(__ContextSymbol16.___p00, this))
      .ToList<Planet>()).ToList<Planet>();
        s0 = 0;
        return;
      case 0:
        if (!(false))
        {

          s0 = 0;
          return;
        }
        else
        {

          s0 = -1;
          return;
        }
      default: return;
    }
  }


  int s2 = -1;
  public void Rule2(float dt, World world)
  {
    switch (s2)
    {

      case -1:
        count_down2 = 0.1f;
        goto case 4;
      case 4:
        if (((count_down2) > (0f)))
        {

          count_down2 = ((count_down2) - (dt));
          s2 = 4;
          return;
        }
        else
        {

          goto case 2;
        }
      case 2:
        Ships = ((

      (Planets).Select(__ContextSymbol17 => new { ___p120 = __ContextSymbol17 })
      .SelectMany(__ContextSymbol18 => (Planets).Select(__ContextSymbol19 => new
      {
        ___p220 = __ContextSymbol19,
        prev = __ContextSymbol18
      })
      .Where(__ContextSymbol20 => ((((!(((__ContextSymbol20.prev.___p120) == (__ContextSymbol20.___p220)))) && (__ContextSymbol20.prev.___p120.Selected))) && (__ContextSymbol20.___p220.Targeted)))
      .Select(__ContextSymbol21 => new Ship(__ContextSymbol21.prev.___p120, __ContextSymbol21.___p220))
      .ToList<Ship>())).ToList<Ship>()).Concat(Ships).ToList<Ship>();
        s2 = 0;
        return;
      case 0:
        count_down1 = 10f;
        goto case 1;
      case 1:
        if (((count_down1) > (0f)))
        {

          count_down1 = ((count_down1) - (dt));
          s2 = 1;
          return;
        }
        else
        {

          s2 = -1;
          return;
        }
      default: return;
    }
  }




  int s1 = -1;
  public RuleResult Rule1(float dt, World world)
  {
    switch (s1)
    {

      case -1:

        goto case 16;
      case 16:
        if (!(q_temp1))
        {

          s1 = 16;
          return RuleResult.Done;
        }
        else
        {

          goto case 15;
        }
      case 15:
        q_temp1 = false;
        if (((StarSystem1) == (Ships)))
        {

          goto case 13;
        }
        else
        {

          goto case 11;
        }
      case 13:
        _Ships = new List<Ship>(Ships);
        goto case 11;
      case 11:
        StarSystem1.Clear();

        counter3 = -1;
        if ((((Ships).Count) == (0)))
        {

          goto case 1;
        }
        else
        {

          s = (Ships)[0];
          goto case 3;
        }
      case 3:
        counter3 = ((counter3) + (1));
        if ((((((Ships).Count) == (counter3))) || (((counter3) > ((Ships).Count)))))
        {

          goto case 1;
        }
        else
        {

          s = (Ships)[counter3];
          goto case 4;
        }
      case 4:
        if (((!(s.Arrived)) && (!(s.Destroyed))))
        {

          goto case 5;
        }
        else
        {

          goto case 6;
        }
      case 5:
        s._StarSystem = this;
        StarSystem1.Add(s);
        goto case 3;
      case 6:
        s._StarSystem = null;
        goto case 3;
      case 1:
        q_temp1 = false;
        Ships = StarSystem1;
        s1 = -1;
        return RuleResult.Working;
      default: return RuleResult.Done;
    }
  }



}
public class Planet
{
  public int frame;
  public bool JustEntered = true;
  private UnityEngine.Vector3 p;
  private StarSystem starSystem;
  public int ID;
  public Planet(UnityEngine.Vector3 p, StarSystem starSystem)
  {
    JustEntered = false;
    frame = World.frame;

    this.ID = World.PlanetCounter++;
    World.NotifySlotStarSystemShipsPlanet2[starSystem.ID].Add(this);
    UnitySphere = UnitySphere.Instantiate(p);
    Targeted = false;
    StarSystem = starSystem;
    Selected = false;
    LandedShips = (

Enumerable.Empty<Ship>()).ToList<Ship>();

    Planet2 = new List<Ship>(StarSystem.Ships);
    List<Ship> q;
    q = (

(StarSystem.Ships).Select(__ContextSymbol23 => new { ___s21 = __ContextSymbol23 })
.Where(__ContextSymbol24 => ((__ContextSymbol24.___s21.Arrived) && (((__ContextSymbol24.___s21.Target) == (this)))))
.Select(__ContextSymbol25 => __ContextSymbol25.___s21)
.ToList<Ship>()).ToList<Ship>();
    Planet2 = q;
  }
  public void Init()
  {
    UnitySphere = UnitySphere.Instantiate(p);
    Targeted = false;
    StarSystem = starSystem;
    Selected = false;
    LandedShips = (

Enumerable.Empty<Ship>()).ToList<Ship>();

    Planet2 = new List<Ship>(StarSystem.Ships);
    List<Ship> q;
    q = (

(StarSystem.Ships).Select(__ContextSymbol27 => new { ___s21 = __ContextSymbol27 })
.Where(__ContextSymbol28 => ((__ContextSymbol28.___s21.Arrived) && (((__ContextSymbol28.___s21.Target) == (this)))))
.Select(__ContextSymbol29 => __ContextSymbol29.___s21)
.ToList<Ship>()).ToList<Ship>();
    Planet2 = q;

  }
  public List<Ship> LandedShips;
  public UnityEngine.Vector3 Position
  {
    get { return UnitySphere.Position; }
    set { UnitySphere.Position = value; }
  }
  public System.Boolean Selected;
  public StarSystem StarSystem;
  public System.Boolean Targeted;
  public UnitySphere UnitySphere;
  public UnityEngine.Animation animation
  {
    get { return UnitySphere.animation; }
  }
  public UnityEngine.AudioSource audio
  {
    get { return UnitySphere.audio; }
  }
  public UnityEngine.Camera camera
  {
    get { return UnitySphere.camera; }
  }
  public UnityEngine.Collider collider
  {
    get { return UnitySphere.collider; }
  }
  public UnityEngine.Collider2D collider2D
  {
    get { return UnitySphere.collider2D; }
  }
  public UnityEngine.ConstantForce constantForce
  {
    get { return UnitySphere.constantForce; }
  }
  public System.Boolean enabled
  {
    get { return UnitySphere.enabled; }
    set { UnitySphere.enabled = value; }
  }
  public UnityEngine.GameObject gameObject
  {
    get { return UnitySphere.gameObject; }
  }
  public UnityEngine.GUIElement guiElement
  {
    get { return UnitySphere.guiElement; }
  }
  public UnityEngine.GUIText guiText
  {
    get { return UnitySphere.guiText; }
  }
  public UnityEngine.GUITexture guiTexture
  {
    get { return UnitySphere.guiTexture; }
  }
  public UnityEngine.HideFlags hideFlags
  {
    get { return UnitySphere.hideFlags; }
    set { UnitySphere.hideFlags = value; }
  }
  public UnityEngine.HingeJoint hingeJoint
  {
    get { return UnitySphere.hingeJoint; }
  }
  public UnityEngine.Light light
  {
    get { return UnitySphere.light; }
  }
  public System.String name
  {
    get { return UnitySphere.name; }
    set { UnitySphere.name = value; }
  }
  public UnityEngine.ParticleEmitter particleEmitter
  {
    get { return UnitySphere.particleEmitter; }
  }
  public UnityEngine.ParticleSystem particleSystem
  {
    get { return UnitySphere.particleSystem; }
  }
  public UnityEngine.Renderer renderer
  {
    get { return UnitySphere.renderer; }
  }
  public UnityEngine.Rigidbody rigidbody
  {
    get { return UnitySphere.rigidbody; }
  }
  public UnityEngine.Rigidbody2D rigidbody2D
  {
    get { return UnitySphere.rigidbody2D; }
  }
  public System.String tag
  {
    get { return UnitySphere.tag; }
    set { UnitySphere.tag = value; }
  }
  public UnityEngine.Transform transform
  {
    get { return UnitySphere.transform; }
  }
  public System.Boolean useGUILayout
  {
    get { return UnitySphere.useGUILayout; }
    set { UnitySphere.useGUILayout = value; }
  }
  public List<Ship> Planet2;
  public System.Single count_down4;
  public System.Single count_down3;
  public System.Single count_down6;
  public System.Single count_down5;
  public System.Boolean q_temp2;
  public Ship s;
  public System.Int32 counter3;
  public RuleTable ActiveRules = new RuleTable(1);
  public void Update(float dt, World world)
  {
    frame = World.frame;

    this.Rule0(dt, world);
    this.Rule1(dt, world);
  }
  public void UpdateSuspendedRules(float dt, World world, List<int> toRemove, RuleTable ActiveRules)
  {
    if (ActiveRules.ActiveIndices.Top > 0 && frame == World.frame)
    {
      for (int i = 0; i < ActiveRules.ActiveIndices.Top; i++)
      {
        var x = ActiveRules.ActiveIndices.Elements[i];
        switch (x)
        {
          case 2:
            if (this.Rule2(dt, world) == RuleResult.Done)
            {
              ActiveRules.ActiveSlots[i] = false;
              ActiveRules.ActiveIndices.Top--;
            }
            else
            {
              ActiveRules.SupportSlots[2] = true;
              ActiveRules.SupportStack.Push(x);
            }
            break;

          default:
            break;
        }
      }
      ActiveRules.ActiveIndices.Clear();
      ActiveRules.Clear();

      var tmp = ActiveRules.SupportStack;
      var tmp1 = ActiveRules.SupportSlots;

      ActiveRules.SupportStack = ActiveRules.ActiveIndices;
      ActiveRules.SupportSlots = ActiveRules.ActiveSlots;


      ActiveRules.ActiveIndices = tmp;
      ActiveRules.ActiveSlots = tmp1;

      if (ActiveRules.ActiveIndices.Top == 0)
        toRemove.Add(ID);
    }
    else
    {
      if (this.frame != World.frame)
        toRemove.Add(ID);
    }
  }





  int s0 = -1;
  public void Rule0(float dt, World world)
  {
    switch (s0)
    {

      case -1:
        if (!(!(Targeted)))
        {

          goto case 1;
        }
        else
        {

          goto case 4;
        }
      case 4:
        count_down4 = 1f;
        goto case 7;
      case 7:
        if (((count_down4) > (0f)))
        {

          count_down4 = ((count_down4) - (dt));
          s0 = 7;
          return;
        }
        else
        {

          goto case 5;
        }
      case 5:
        Targeted = ((world.Seed.NextDouble()) > (0.8f));
        s0 = -1;
        return;
      case 1:
        count_down3 = world.Seed.Next(1, 5);
        goto case 2;
      case 2:
        if (((count_down3) > (0f)))
        {

          count_down3 = ((count_down3) - (dt));
          s0 = 2;
          return;
        }
        else
        {

          goto case 0;
        }
      case 0:
        Targeted = false;
        s0 = -1;
        return;
      default: return;
    }
  }


  int s1 = -1;
  public void Rule1(float dt, World world)
  {
    switch (s1)
    {

      case -1:
        if (!(!(Selected)))
        {

          goto case 1;
        }
        else
        {

          goto case 4;
        }
      case 4:
        count_down6 = 1f;
        goto case 7;
      case 7:
        if (((count_down6) > (0f)))
        {

          count_down6 = ((count_down6) - (dt));
          s1 = 7;
          return;
        }
        else
        {

          goto case 5;
        }
      case 5:
        Selected = ((world.Seed.NextDouble()) > (0.8f));
        s1 = -1;
        return;
      case 1:
        count_down5 = world.Seed.Next(1, 5);
        goto case 2;
      case 2:
        if (((count_down5) > (0f)))
        {

          count_down5 = ((count_down5) - (dt));
          s1 = 2;
          return;
        }
        else
        {

          goto case 0;
        }
      case 0:
        Selected = false;
        s1 = -1;
        return;
      default: return;
    }
  }




  int s2 = -1;
  public RuleResult Rule2(float dt, World world)
  {
    switch (s2)
    {

      case -1:

        goto case 16;
      case 16:
        if (!(q_temp2))
        {

          s2 = 16;
          return RuleResult.Done;
        }
        else
        {

          goto case 15;
        }
      case 15:
        q_temp2 = false;
        if (((Planet2) == (StarSystem.Ships)))
        {

          goto case 13;
        }
        else
        {

          goto case 11;
        }
      case 13:
        StarSystem.Ships = new List<Ship>(StarSystem.Ships);
        goto case 11;
      case 11:
        Planet2.Clear();

        counter3 = -1;
        if ((((StarSystem.Ships).Count) == (0)))
        {

          goto case 1;
        }
        else
        {

          s = (StarSystem.Ships)[0];
          goto case 3;
        }
      case 3:
        counter3 = ((counter3) + (1));
        if ((((((StarSystem.Ships).Count) == (counter3))) || (((counter3) > ((StarSystem.Ships).Count)))))
        {

          goto case 1;
        }
        else
        {

          s = (StarSystem.Ships)[counter3];
          goto case 4;
        }
      case 4:
        if (((s.Arrived) && (((s.Target) == (s._Planet)))))
        {

          goto case 5;
        }
        else
        {

          goto case 6;
        }
      case 5:
        s._Planet = this;
        Planet2.Add(s);
        goto case 3;
      case 6:
        s._Planet = null;
        goto case 3;
      case 1:
        q_temp2 = false;
        LandedShips = Planet2;
        s2 = -1;
        return RuleResult.Working;
      default: return RuleResult.Done;
    }
  }



}
public class Ship
{
  public int frame;
  public bool JustEntered = true;
  private Planet source;
  private Planet target;
  public int ID;
  public Ship(Planet source, Planet target)
  {
    JustEntered = false;
    frame = World.frame;

    this.ID = World.ShipCounter++;
    World.NotifySlotShipArrivedShip0.Add(ID, new List<Ship>());
    World.NotifySlotShipArrivedShip1.Add(ID, new List<Ship>());
    World.NotifySlotShipDestroyedShip0.Add(ID, new List<Ship>());
    World.NotifySlotShipDestroyedShip0[ID].Add(this);
    World.NotifySlotShipArrivedShip0[ID].Add(this);
    World.NotifySlotShipArrivedShip1[ID].Add(this);
    UnityEngine.Vector3 ___velocity00;
    ___velocity00 = (target.Position) - (source.Position);
    UnityEngine.Vector3 ___velocity_normalized00;
    ___velocity_normalized00 = ___velocity00.normalized;
    Velocity = (___velocity_normalized00) * (10f);
    UnityCube = UnityCube.Instantiate(source.Position);
    Target = target;
    Arrived = false;

  }
  public void Init()
  {
    UnityEngine.Vector3 ___velocity00;
    ___velocity00 = (target.Position) - (source.Position);
    UnityEngine.Vector3 ___velocity_normalized00;
    ___velocity_normalized00 = ___velocity00.normalized;
    Velocity = (___velocity_normalized00) * (10f);
    UnityCube = UnityCube.Instantiate(source.Position);
    Target = target;
    Arrived = false;


  }
  public System.Boolean _Arrived;
  public System.Boolean _Destroyed
  {
    get { return UnityCube.Destroyed; }
    set { UnityCube.Destroyed = value; }
  }
  public UnityEngine.Vector3 Position
  {
    get { return UnityCube.Position; }
    set { UnityCube.Position = value; }
  }
  public Planet Target;
  public UnityCube UnityCube;
  public UnityEngine.Vector3 Velocity;
  public UnityEngine.Animation animation
  {
    get { return UnityCube.animation; }
  }
  public UnityEngine.AudioSource audio
  {
    get { return UnityCube.audio; }
  }
  public UnityEngine.Camera camera
  {
    get { return UnityCube.camera; }
  }
  public UnityEngine.Collider collider
  {
    get { return UnityCube.collider; }
  }
  public UnityEngine.Collider2D collider2D
  {
    get { return UnityCube.collider2D; }
  }
  public UnityEngine.ConstantForce constantForce
  {
    get { return UnityCube.constantForce; }
  }
  public System.Boolean enabled
  {
    get { return UnityCube.enabled; }
    set { UnityCube.enabled = value; }
  }
  public UnityEngine.GameObject gameObject
  {
    get { return UnityCube.gameObject; }
  }
  public UnityEngine.GUIElement guiElement
  {
    get { return UnityCube.guiElement; }
  }
  public UnityEngine.GUIText guiText
  {
    get { return UnityCube.guiText; }
  }
  public UnityEngine.GUITexture guiTexture
  {
    get { return UnityCube.guiTexture; }
  }
  public UnityEngine.HideFlags hideFlags
  {
    get { return UnityCube.hideFlags; }
    set { UnityCube.hideFlags = value; }
  }
  public UnityEngine.HingeJoint hingeJoint
  {
    get { return UnityCube.hingeJoint; }
  }
  public UnityEngine.Light light
  {
    get { return UnityCube.light; }
  }
  public System.String name
  {
    get { return UnityCube.name; }
    set { UnityCube.name = value; }
  }
  public UnityEngine.ParticleEmitter particleEmitter
  {
    get { return UnityCube.particleEmitter; }
  }
  public UnityEngine.ParticleSystem particleSystem
  {
    get { return UnityCube.particleSystem; }
  }
  public UnityEngine.Renderer renderer
  {
    get { return UnityCube.renderer; }
  }
  public UnityEngine.Rigidbody rigidbody
  {
    get { return UnityCube.rigidbody; }
  }
  public UnityEngine.Rigidbody2D rigidbody2D
  {
    get { return UnityCube.rigidbody2D; }
  }
  public System.String tag
  {
    get { return UnityCube.tag; }
    set { UnityCube.tag = value; }
  }
  public UnityEngine.Transform transform
  {
    get { return UnityCube.transform; }
  }
  public System.Boolean useGUILayout
  {
    get { return UnityCube.useGUILayout; }
    set { UnityCube.useGUILayout = value; }
  }
  public StarSystem _StarSystem;
  public Planet _Planet;
  public System.Boolean _cond01;
  public System.Boolean _cond11;
  public System.Boolean _cond02;
  public Planet _cond12;
  public RuleTable ActiveRules = new RuleTable(3);
  public System.Boolean Arrived
  {
    get { return _Arrived; }

    set
    {
      _Arrived = value;
      for (int i = 0; i < World.NotifySlotShipArrivedShip0[ID].Count; i++)
      {
        var entity = World.NotifySlotShipArrivedShip0[ID][i];
        if (entity.frame == World.frame)
        {
          if (!World.ShipWithActiveRules.ContainsKey(entity.ID))
            World.ShipWithActiveRules.Add(entity.ID, new Tuple<Ship, RuleTable>(entity, new RuleTable(4))); World.ShipWithActiveRules[entity.ID].Item2.Add(0);
        }
        else
        {
          entity.JustEntered = true;
          World.NotifySlotShipDestroyedShip0.Remove(entity.ID);
          World.NotifySlotShipArrivedShip0.Remove(entity.ID);
          World.NotifySlotShipArrivedShip1.Remove(entity.ID);
        }
      }
      for (int i = 0; i < World.NotifySlotShipArrivedShip1[ID].Count; i++)
      {
        var entity = World.NotifySlotShipArrivedShip1[ID][i];
        if (entity.frame == World.frame)
        {
          if (!World.ShipWithActiveRules.ContainsKey(entity.ID))
            World.ShipWithActiveRules.Add(entity.ID, new Tuple<Ship, RuleTable>(entity, new RuleTable(4))); World.ShipWithActiveRules[entity.ID].Item2.Add(1);
        }
        else
        {
          entity.JustEntered = true;
          World.NotifySlotShipDestroyedShip0.Remove(entity.ID);
          World.NotifySlotShipArrivedShip0.Remove(entity.ID);
          World.NotifySlotShipArrivedShip1.Remove(entity.ID);
        }
      }
    }
  }
  public System.Boolean Destroyed
  {
    get { return _Destroyed; }

    set
    {
      _Destroyed = value;
      for (int i = 0; i < World.NotifySlotShipDestroyedShip0[ID].Count; i++)
      {
        var entity = World.NotifySlotShipDestroyedShip0[ID][i];
        if (entity.frame == World.frame)
        {
          if (!World.ShipWithActiveRules.ContainsKey(entity.ID))
            World.ShipWithActiveRules.Add(entity.ID, new Tuple<Ship, RuleTable>(entity, new RuleTable(4))); World.ShipWithActiveRules[entity.ID].Item2.Add(0);
        }
        else
        {
          entity.JustEntered = true;
          World.NotifySlotShipDestroyedShip0.Remove(entity.ID);
          World.NotifySlotShipArrivedShip0.Remove(entity.ID);
          World.NotifySlotShipArrivedShip1.Remove(entity.ID);
        }
      }
    }
  }
  public void Update(float dt, World world)
  {
    frame = World.frame; this.Rule2(dt, world);

    this.Rule3(dt, world);

  }
  public void UpdateSuspendedRules(float dt, World world, List<int> toRemove, RuleTable ActiveRules)
  {
    if (ActiveRules.ActiveIndices.Top > 0 && frame == World.frame)
    {
      for (int i = 0; i < ActiveRules.ActiveIndices.Top; i++)
      {
        var x = ActiveRules.ActiveIndices.Elements[i];
        switch (x)
        {
          case 0:
            if (this.Rule0(dt, world) == RuleResult.Done)
            {
              ActiveRules.ActiveSlots[i] = false;
              ActiveRules.ActiveIndices.Top--;
            }
            else
            {
              ActiveRules.SupportSlots[0] = true;
              ActiveRules.SupportStack.Push(x);
            }
            break;
          case 1:
            if (this.Rule1(dt, world) == RuleResult.Done)
            {
              ActiveRules.ActiveSlots[i] = false;
              ActiveRules.ActiveIndices.Top--;
            }
            else
            {
              ActiveRules.SupportSlots[1] = true;
              ActiveRules.SupportStack.Push(x);
            }
            break;

          default:
            break;
        }
      }
      ActiveRules.ActiveIndices.Clear();
      ActiveRules.Clear();

      var tmp = ActiveRules.SupportStack;
      var tmp1 = ActiveRules.SupportSlots;

      ActiveRules.SupportStack = ActiveRules.ActiveIndices;
      ActiveRules.SupportSlots = ActiveRules.ActiveSlots;


      ActiveRules.ActiveIndices = tmp;
      ActiveRules.ActiveSlots = tmp1;

      if (ActiveRules.ActiveIndices.Top == 0)
        toRemove.Add(ID);
    }
    else
    {
      if (this.frame != World.frame)
        toRemove.Add(ID);
    }
  }

  public void Rule2(float dt, World world)
  {
    Position = (Position) + ((Velocity) * (dt));
  }





  int s3 = -1;
  public void Rule3(float dt, World world)
  {
    switch (s3)
    {

      case -1:
        if (!(((5f) > (UnityEngine.Vector3.Distance(Position, Target.Position)))))
        {

          s3 = -1;
          return;
        }
        else
        {

          goto case 0;
        }
      case 0:
        Arrived = true;
        Position = Target.Position;
        Velocity = Vector3.zero;
        Destroyed = true;
        s3 = -1;
        return;
      default: return;
    }
  }





  int s0 = -1;
  public RuleResult Rule0(float dt, World world)
  {
    switch (s0)
    {

      case -1:
        _cond01 = Arrived;
        _cond11 = Destroyed;
        goto case 11;
      case 11:
        if (!(((!(((_cond11) == (Destroyed)))) || (((!(((_cond01) == (Arrived)))) || (false))))))
        {

          s0 = 11;
          return RuleResult.Done;
        }
        else
        {

          goto case 2;
        }
      case 2:
        if (((!(Arrived)) && (!(Destroyed))))
        {

          goto case 0;
        }
        else
        {

          goto case 1;
        }
      case 0:
        if (!(_StarSystem.StarSystem1.Contains(this)))
        {

          goto case 3;
        }
        else
        {

          goto case 4;
        }
      case 3:
        _StarSystem.StarSystem1.Add(this);
        goto case -1;
      case 4:
        goto case -1;
      case 1:
        _StarSystem.StarSystem1.Remove(this);
        goto case -1;
      default: return RuleResult.Done;
    }
  }


  int s1 = -1;
  public RuleResult Rule1(float dt, World world)
  {
    switch (s1)
    {

      case -1:
        _cond02 = Arrived;
        _cond12 = Target;
        goto case 11;
      case 11:
        if (!(((!(((_cond12) == (_Planet)))) || (((!(((_cond02) == (Arrived)))) || (false))))))
        {

          s1 = 11;
          return RuleResult.Done;
        }
        else
        {

          goto case 2;
        }
      case 2:
        if (((Arrived) && (((Target) == (_Planet)))))
        {

          goto case 0;
        }
        else
        {

          goto case 1;
        }
      case 0:
        if (!(_Planet.Planet2.Contains(this)))
        {

          goto case 3;
        }
        else
        {

          goto case 4;
        }
      case 3:
        _Planet.Planet2.Add(this);
        goto case -1;
      case 4:
        goto case -1;
      case 1:
        _Planet.Planet2.Remove(this);
        goto case -1;
      default: return RuleResult.Done;
    }
  }


}
      