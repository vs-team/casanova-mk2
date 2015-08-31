#pragma warning disable 162,108,618
using Casanova.Prelude;
using System.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;
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
    Velocity = new UnityEngine.Vector3(3f, 0.5f, 1f);
    UnityBall = UnityBall.Find();
    Gravity = new UnityEngine.Vector3(0f, -9.81f, 0f);
    FrictionCoefficient = 0.9f;
  }
  public System.Single FrictionCoefficient;
  public UnityEngine.Vector3 Gravity;
  public UnityEngine.Vector3 Position
  {
    get { return UnityBall.Position; }
    set { UnityBall.Position = value; }
  }
  public System.Boolean Quit
  {
    set { UnityBall.Quit = value; }
  }
  public UnityBall UnityBall;
  public UnityEngine.Vector3 Velocity;
  public UnityEngine.Animation animation
  {
    get { return UnityBall.animation; }
  }
  public UnityEngine.AudioSource audio
  {
    get { return UnityBall.audio; }
  }
  public UnityEngine.Camera camera
  {
    get { return UnityBall.camera; }
  }
  public UnityEngine.Collider collider
  {
    get { return UnityBall.collider; }
  }
  public UnityEngine.Collider2D collider2D
  {
    get { return UnityBall.collider2D; }
  }
  public UnityEngine.ConstantForce constantForce
  {
    get { return UnityBall.constantForce; }
  }
  public System.Boolean enabled
  {
    get { return UnityBall.enabled; }
    set { UnityBall.enabled = value; }
  }
  public UnityEngine.GameObject gameObject
  {
    get { return UnityBall.gameObject; }
  }
  public UnityEngine.GUIElement guiElement
  {
    get { return UnityBall.guiElement; }
  }
  public UnityEngine.GUIText guiText
  {
    get { return UnityBall.guiText; }
  }
  public UnityEngine.GUITexture guiTexture
  {
    get { return UnityBall.guiTexture; }
  }
  public UnityEngine.HideFlags hideFlags
  {
    get { return UnityBall.hideFlags; }
    set { UnityBall.hideFlags = value; }
  }
  public UnityEngine.HingeJoint hingeJoint
  {
    get { return UnityBall.hingeJoint; }
  }
  public UnityEngine.Light light
  {
    get { return UnityBall.light; }
  }
  public System.String name
  {
    get { return UnityBall.name; }
    set { UnityBall.name = value; }
  }
  public UnityEngine.ParticleEmitter particleEmitter
  {
    get { return UnityBall.particleEmitter; }
  }
  public UnityEngine.ParticleSystem particleSystem
  {
    get { return UnityBall.particleSystem; }
  }
  public UnityEngine.Renderer renderer
  {
    get { return UnityBall.renderer; }
  }
  public UnityEngine.Rigidbody rigidbody
  {
    get { return UnityBall.rigidbody; }
  }
  public UnityEngine.Rigidbody2D rigidbody2D
  {
    get { return UnityBall.rigidbody2D; }
  }
  public System.String tag
  {
    get { return UnityBall.tag; }
    set { UnityBall.tag = value; }
  }
  public UnityEngine.Transform transform
  {
    get { return UnityBall.transform; }
  }
  public System.Boolean useGUILayout
  {
    get { return UnityBall.useGUILayout; }
    set { UnityBall.useGUILayout = value; }
  }
  public System.Single count_down1;
  public void Update(float dt, World world)
  {
    this.Rule1(dt, world);

    this.Rule0(dt, world);
    this.Rule2(dt, world);
    this.Rule3(dt, world);
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
        if (!(((0f) > (Position.y))))
        {

          s0 = -1;
          return;
        }
        else
        {

          goto case 0;
        }
      case 0:
        Position = new UnityEngine.Vector3(Position.x, 0f, Position.z);
        Velocity = ((new UnityEngine.Vector3(Velocity.x, (Velocity.y) * (-1f), Velocity.z)) * (FrictionCoefficient));
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
        count_down1 = 1f;
        goto case 2;
      case 2:
        if (((count_down1) > (0f)))
        {

          count_down1 = ((count_down1) - (dt));
          s2 = 2;
          return;
        }
        else
        {

          goto case 0;
        }
      case 0:
        Velocity = ((Velocity) + (((Gravity) * (dt))));
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
        if (!(UnityEngine.Input.GetKey(KeyCode.Escape)))
        {

          s3 = -1;
          return;
        }
        else
        {

          goto case 0;
        }
      case 0:
        Quit = true;
        s3 = -1;
        return;
      default: return;
    }
  }






}
 