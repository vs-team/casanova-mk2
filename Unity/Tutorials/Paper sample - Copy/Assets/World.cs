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
    void OnApplicationQuit() { file.Close(); }public static int frame;
    void Update()
    {
      Update(Time.deltaTime, this);
      frame++;
    }
    public bool JustEntered = true;

    System.IO.StreamWriter file;
    public void Start()
    {
      file = new System.IO.StreamWriter(System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "test_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second + "_" + ".csv")); System.Random ___seed00;
      ___seed00 = new System.Random(0);
      List<Elem> ___elems00;
      ___elems00 = (

  (Enumerable.Range(0, (1) + ((1000) - (0))).ToList<System.Int32>()).Select(__ContextSymbol0 => new { ___i01 = __ContextSymbol0 })
  .Select(__ContextSymbol1 => new Elem(___seed00))
  .ToList<Elem>()).ToList<Elem>();
      Seed = ___seed00;
      Elems = ___elems00;

    }
    public List<Elem> __Elems;
    public List<Elem> Elems
    {
      get { return __Elems; }
      set
      {
        __Elems = value;
        foreach (var e in value)
        {
          if (e.JustEntered)
          {
            e.JustEntered = false;
          }
        }
      }
    }
    public System.Random Seed;
    public System.Single count_down1;

    System.DateTime init_time = System.DateTime.Now;
    public void Update(float dt, World world)
    {
      var t = System.DateTime.Now; this.Rule1(dt, world);

      for (int x0 = 0; x0 < Elems.Count; x0++)
      {
        Elems[x0].Update(dt, world);
      }
      this.Rule0(dt, world);

      var t1 = System.DateTime.Now;
      file.WriteLine((t1 - t).Milliseconds + "," + (t1 - init_time).Seconds);

      if ((DateTime.Now - first_time).TotalSeconds > 60)
      { UnityEditor.EditorApplication.isPlaying = false; }
    }

    public DateTime first_time = DateTime.Now;
    public void Rule1(float dt, World world)
    {
      Elems = Elems.Where(e => !e.Counter).Select(e => e).ToList();
    }


    int s0 = -1;
    public void Rule0(float dt, World world)
    {
      switch (s0)
      {

        case -1:
          count_down1 = 5f;
          goto case 2;
        case 2:
          if (((count_down1) > (0f)))
          {

            count_down1 = ((count_down1) - (dt));
            s0 = 2;
            return;
          }
          else
          {

            goto case 0;
          }
        case 0:
          var elems = Enumerable.Range(0, 50).Select(s => new Elem(Seed));
          Elems.AddRange(elems);
          s0 = -1;
          return;
        default: return;
      }
    }







  }
  public class Elem
  {
    public int frame;
    public bool JustEntered = true;
    private System.Random Seed;
    public int ID;
    public Elem(System.Random Seed)
    {
      JustEntered = false;
      frame = World.frame;
      Velocity = Vector3.zero;
      V = -1;
      UnityCube = UnityCube.Instantiate(new UnityEngine.Vector3(((((System.Single)Seed.NextDouble())) * (500)) - (250f), ((((System.Single)Seed.NextDouble())) * (300)) - (150f), 0f));
      Counter = false;

    }
    public System.Boolean Counter;
    public System.Boolean Destroyed
    {
      get { return UnityCube.Destroyed; }
      set { UnityCube.Destroyed = value; }
    }
    public UnityEngine.Vector3 Position
    {
      get { return UnityCube.Position; }
      set { UnityCube.Position = value; }
    }
    public UnityCube UnityCube;
    public System.Int32 V;
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
    public System.Single count_down2;
    public System.Single count_down3;
    public void Update(float dt, World world)
    {
      frame = World.frame; this.Rule1(dt, world);

      this.Rule0(dt, world);
      this.Rule2(dt, world);
    }

    public void Rule1(float dt, World world)
    {
      Position = (Position) + ((Velocity) * (dt));
    }





    int s0 = -1;
    public void Rule0(float dt, World world)
    {
      switch (s0)
      {

        case -1:
          count_down2 = ((((((System.Single)world.Seed.NextDouble())) * (6))) + (8f));
          goto case 2;
        case 2:
          if (((count_down2) > (0f)))
          {

            count_down2 = ((count_down2) - (dt));
            s0 = 2;
            return;
          }
          else
          {

            goto case 0;
          }
        case 0:
          Velocity = new UnityEngine.Vector3(((((System.Single)world.Seed.NextDouble())) * (10f)) - (5f), ((((System.Single)world.Seed.NextDouble())) * (10f)) - (5f), 0f);
          V = 0;
          s0 = -1;
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
          if (!(((V) == (0))))
          {

            s2 = -1;
            return;
          }
          else
          {

            goto case 3;
          }
        case 3:
          Counter = false;
          Destroyed = false;
          s2 = 1;
          return;
        case 1:
          count_down3 = ((((((System.Single)world.Seed.NextDouble())) * (2f))) + (5f));
          goto case 2;
        case 2:
          if (((count_down3) > (0f)))
          {

            count_down3 = ((count_down3) - (dt));
            s2 = 2;
            return;
          }
          else
          {

            goto case 0;
          }
        case 0:
          Counter = true;
          Destroyed = true;
          s2 = -1;
          return;
        default: return;
      }
    }






  }
}    