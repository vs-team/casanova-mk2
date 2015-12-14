#pragma warning disable 162,108,618
using Casanova.Prelude;
using System.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace Game
{
  public class World : MonoBehaviour
  {
    public static int frame;
    void Update()
    {
      Update(Time.deltaTime, this);
      frame++;
    }
    public bool JustEntered = true;


    public void Start()
    {
      SelectedCubeToDestroy = (new Nothing<Cube>());
      Cubes = (

  Enumerable.Empty<Cube>()).ToList<Cube>();

    }
    public List<Cube> Cubes;
    public Option<Cube> SelectedCubeToDestroy;
    public System.Single count_down1;
    public Cube ___selected_element40;

    System.DateTime init_time = System.DateTime.Now;
    public void Update(float dt, World world)
    {
      var t = System.DateTime.Now;

      for (int x0 = 0; x0 < Cubes.Count; x0++)
      {
        Cubes[x0].Update(dt, world);
      }
      this.Rule0(dt, world);
      this.Rule1(dt, world);
      this.Rule2(dt, world);
      this.Rule3(dt, world);
      this.Rule4(dt, world);
    }





    int s0 = -1;
    public void Rule0(float dt, World world)
    {
      switch (s0)
      {

        case -1:
          if (!(UnityEngine.Input.GetKeyDown(KeyCode.P)))
          {

            s0 = -1;
            return;
          }
          else
          {

            goto case 0;
          }
        case 0:
          Cubes = new Cons<Cube>(new Cube(Color.white), (Cubes)).ToList<Cube>();
          s0 = -1;
          return;
        default: return;
      }
    }


    int s1 = -1;
    public void Rule1(float dt, World world)
    {
      if (s1 > 0 && UnityEngine.Input.GetKeyDown(KeyCode.Q)) s1 = -1;
      else if (s1 > 1 && UnityEngine.Input.GetKeyDown(KeyCode.S)) s1 = -1;

      switch (s1)
      {

        case -1:
          if (!(((UnityEngine.Input.GetKeyDown(KeyCode.Q)) || (UnityEngine.Input.GetKeyDown(KeyCode.S)))))
          {

            s1 = -1;
            return;
          }
          else
          {

            goto case 0;
          }
        case 0:
          if (UnityEngine.Input.GetKeyDown(KeyCode.Q))
          {

            goto case 2;
          }
          else
          {

            if (UnityEngine.Input.GetKeyDown(KeyCode.S))
            {

              goto case 3;
            }
            else
            {

              s1 = 0;
              return;
            }
          }
        case 2:
          UnityEngine.Debug.Log("Hello");
          Cubes = Cubes;
          s1 = -1;
          return;
        case 3:
          if (!(true))
          {

            s1 = -1;
            return;
          }
          else
          {

            goto case 7;
          }
        case 7:
          Cubes = new Cons<Cube>(new Cube(Color.white), (Cubes)).ToList<Cube>();
          s1 = 8;
          return;
        case 8:
          count_down1 = 1f;
          goto case 9;
        case 9:
          if (((count_down1) > (0f)))
          {

            count_down1 = ((count_down1) - (dt));
            s1 = 9;
            return;
          }
          else
          {

            s1 = 3;
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
          Cubes = (

        (Cubes).Select(__ContextSymbol1 => new { ___c20 = __ContextSymbol1 })
        .Where(__ContextSymbol2 => !(__ContextSymbol2.___c20.Destroyed))
        .Select(__ContextSymbol3 => __ContextSymbol3.___c20)
        .ToList<Cube>()).ToList<Cube>();
          s2 = -1;
          return;
        default: return;
      }
    }


    int s3 = -1;
    public void Rule3(float dt, World world)
    {
      switch (s3)
      {

        case -1:
          if (!(((SelectedCubeToDestroy.IsSome) && (SelectedCubeToDestroy.Value.Destroyed))))
          {

            s3 = -1;
            return;
          }
          else
          {

            goto case 0;
          }
        case 0:
          SelectedCubeToDestroy = (new Nothing<Cube>());
          s3 = -1;
          return;
        default: return;
      }
    }


    int s4 = -1;
    public void Rule4(float dt, World world)
    {
      switch (s4)
      {

        case -1:
          if (!(((UnityEngine.Input.GetKeyDown(KeyCode.R)) && (((Cubes.Count) > (0))))))
          {

            s4 = -1;
            return;
          }
          else
          {

            goto case 1;
          }
        case 1:
          ___selected_element40 = (Cubes)[UnityEngine.Random.Range(0, Cubes.Count)];
          SelectedCubeToDestroy = (new Just<Cube>(___selected_element40));
          s4 = -1;
          return;
        default: return;
      }
    }






  }
  public class Cube
  {
    public int frame;
    public bool JustEntered = true;
    private UnityEngine.Color color;
    public int ID;
    public Cube(UnityEngine.Color color)
    {
      JustEntered = false;
      frame = World.frame;
      UnityCube ___cube00;
      ___cube00 = UnityCube.Instantiate(color);
      System.Single ___dist00;
      ___dist00 = 3f;
      List<UnityEngine.Vector3> ___checkpoints00;
      ___checkpoints00 = (

  (new Cons<UnityEngine.Vector3>(new UnityEngine.Vector3((___cube00.Position.x) + (___dist00), ___cube00.Position.y, ___cube00.Position.z), (new Cons<UnityEngine.Vector3>(new UnityEngine.Vector3((___cube00.Position.x) + (___dist00), (___cube00.Position.y) + (___dist00), ___cube00.Position.z), (new Cons<UnityEngine.Vector3>(new UnityEngine.Vector3(___cube00.Position.x, (___cube00.Position.y) + (___dist00), ___cube00.Position.z), (new Cons<UnityEngine.Vector3>(___cube00.Position, (new Empty<UnityEngine.Vector3>()).ToList<UnityEngine.Vector3>())).ToList<UnityEngine.Vector3>())).ToList<UnityEngine.Vector3>())).ToList<UnityEngine.Vector3>())).ToList<UnityEngine.Vector3>()).ToList<UnityEngine.Vector3>();
      Velocity = Vector3.zero;
      UnityCube = ___cube00;
      Factor = 2f;
      Checkpoints = ___checkpoints00;

    }
    public List<UnityEngine.Vector3> Checkpoints;
    public UnityEngine.Color Color
    {
      set { UnityCube.Color = value; }
    }
    public System.Boolean Destroyed
    {
      get { return UnityCube.Destroyed; }
      set { UnityCube.Destroyed = value; }
    }
    public System.Single Factor;
    public UnityEngine.Vector3 Position
    {
      get { return UnityCube.Position; }
      set { UnityCube.Position = value; }
    }
    public UnityEngine.Vector3 Scale
    {
      get { return UnityCube.Scale; }
      set { UnityCube.Scale = value; }
    }
    public UnityCube UnityCube;
    public UnityEngine.Vector3 Velocity;
    public System.Boolean enabled
    {
      get { return UnityCube.enabled; }
      set { UnityCube.enabled = value; }
    }
    public UnityEngine.GameObject gameObject
    {
      get { return UnityCube.gameObject; }
    }
    public UnityEngine.HideFlags hideFlags
    {
      get { return UnityCube.hideFlags; }
      set { UnityCube.hideFlags = value; }
    }
    public System.Boolean isActiveAndEnabled
    {
      get { return UnityCube.isActiveAndEnabled; }
    }
    public System.String name
    {
      get { return UnityCube.name; }
      set { UnityCube.name = value; }
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
    public UnityEngine.Vector3 ___c11;
    public System.Int32 counter21;
    public UnityEngine.Vector3 ___dir010;
    public System.Single count_down2;
    public void Update(float dt, World world)
    {
      frame = World.frame;

      this.Rule0(dt, world);
      this.Rule1(dt, world);
      this.Rule2(dt, world);
    }





    int s0 = -1;
    public void Rule0(float dt, World world)
    {
      switch (s0)
      {

        case -1:
          Position = ((Position) + (((Velocity) * (dt))));
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

          counter21 = -1;
          if ((((Checkpoints).Count) == (0)))
          {

            goto case 0;
          }
          else
          {

            ___c11 = (Checkpoints)[0];
            goto case 2;
          }
        case 2:
          counter21 = ((counter21) + (1));
          if ((((((Checkpoints).Count) == (counter21))) || (((counter21) > ((Checkpoints).Count)))))
          {

            goto case 0;
          }
          else
          {

            ___c11 = (Checkpoints)[counter21];
            goto case 3;
          }
        case 3:
          ___dir010 = ((___c11) - (Position));
          Position = Position;
          Velocity = ___dir010.normalized;
          Destroyed = Destroyed;
          s1 = 7;
          return;
        case 7:
          if (!(((0f) > (UnityEngine.Vector3.Dot(___dir010, (___c11) - (Position))))))
          {

            s1 = 7;
            return;
          }
          else
          {

            goto case 6;
          }
        case 6:
          Position = ___c11;
          Velocity = Vector3.zero;
          Destroyed = Destroyed;
          s1 = 4;
          return;
        case 4:
          count_down2 = 1f;
          goto case 5;
        case 5:
          if (((count_down2) > (0f)))
          {

            count_down2 = ((count_down2) - (dt));
            s1 = 5;
            return;
          }
          else
          {

            s1 = 2;
            return;
          }
        case 0:
          Position = Position;
          Velocity = Velocity;
          Destroyed = true;
          s1 = -1;
          return;
        default: return;
      }
    }


    int s2 = -1;
    public void Rule2(float dt, World world)
    {
      switch (s2)
      {

        case -1:
          if (!(((world.SelectedCubeToDestroy.IsSome) && (((world.SelectedCubeToDestroy.Value) == (this))))))
          {

            s2 = -1;
            return;
          }
          else
          {

            goto case 1;
          }
        case 1:
          if (!(((Factor) > (0f))))
          {

            goto case 0;
          }
          else
          {

            goto case 2;
          }
        case 2:
          Scale = ((Scale) + (((Vector3.one) * (dt))));
          Factor = ((Factor) - (dt));
          Destroyed = Destroyed;
          s2 = 1;
          return;
        case 0:
          Scale = Scale;
          Factor = Factor;
          Destroyed = true;
          s2 = -1;
          return;
        default: return;
      }
    }






  }
}       