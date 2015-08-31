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

    public static Dictionary<int, UnityCube> Entities = new Dictionary<int, UnityCube>();

    //void OnApplicationQuit() { file.Close(); }
    public static int frame;
    void Update()
    {
      //if (Network.isServer)
      //{

        Update(Time.deltaTime, this);
        frame++;
      //}
    }
    public bool JustEntered = true;

    //System.IO.StreamWriter file;
    public void Start()
    {
        //file = new System.IO.StreamWriter(System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "test_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second + "_" + ".csv")); 
      if (Network.isServer)
      {
          
        Cubes = (

        Enumerable.Empty<Cube>()).ToList<Cube>();
          //Cube = new Cube(Color.grey, Cube._GLOBAL_ID++);
        }
    }






    public void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
    {
      if (stream.isWriting)
      {
        NetworkingPrelude.SerializeBase(frame, stream);
        NetworkingPrelude.SerializeBase(count_down1, stream);

        //NetworkingPrelude.SerializeBase(Cube.ID, stream);
        //Cube.Send(stream, info);
      
        if (Cubes != null)
        {
          NetworkingPrelude.SerializeBase(Cubes.Count, stream);
          for (int i = 0; i < Cubes.Count; i++)
          {
            NetworkingPrelude.SerializeBase(Cubes[i].ID, stream);
            Cubes[i].Send(stream, info);
          }
        }
      }
      else
      {
        frame = NetworkingPrelude.IntReceiveBase(stream);
        count_down1 = NetworkingPrelude.FloatReceiveBase(stream);

        //var id = NetworkingPrelude.IntReceiveBase(stream);
        //if (Cube == null)
        //  Cube = new Cube(Color.red, id);
        //if (Cube.UnityCube != null)
        //  Cube.Receive(stream, info);
        //else
        //  Cube = null;

        //if (Cubes != null)
        //{
        var length = NetworkingPrelude.IntReceiveBase(stream);
        bool has_changed = false;
        List<Cube> tmp = new List<Cube>();

        if (length == 0) Cubes.Clear();
        for (int i = 0; i < length; i++)
        {
          var ID = NetworkingPrelude.IntReceiveBase(stream);
          var cube = Cubes.Find(c => c.ID == ID);
          if (cube == null)
          {
            //Debug.Log("ID: " + ID);
            cube = new Cube(Color.red, ID);
            if (cube.UnityCube != null)
            {
              //Debug.Log("found");
              has_changed = true;
              cube.Receive(stream, info);
              tmp.Add(cube);
            }
            else
            {
              //Debug.Log("not found");

              cube = null;
              NetworkingPrelude.IntReceiveBase(stream);
              NetworkingPrelude.BoolReceiveBase(stream);
              NetworkingPrelude.Vector3ReceiveBase(stream);
              NetworkingPrelude.BoolReceiveBase(stream);
            }
          }
          else
          {
            if (cube.UnityCube != null)
            {
              cube.Receive(stream, info);
              tmp.Add(cube);
            }
            else
            {
              NetworkingPrelude.IntReceiveBase(stream);
              NetworkingPrelude.BoolReceiveBase(stream);
              NetworkingPrelude.Vector3ReceiveBase(stream);
              NetworkingPrelude.BoolReceiveBase(stream);
            }
          }
        }
        if (has_changed)
          Cubes = tmp;
      //}
      //  else
      //    Cubes = new List<Cube>();
      }
    }


    public List<Cube> Cubes = new List<Cube>();
    //public Cube Cube;
    public System.Single count_down1;

    System.DateTime init_time = System.DateTime.Now;
    public void Update(float dt, World world)
    {
        var t = System.DateTime.Now;
        //if(Cube != null)
        //  Cube.Update(dt, this);
        if(Cubes!= null)
          for (int x0 = 0; x0 < Cubes.Count; x0++)
          {
            if(Cubes[x0]!=null)
              Cubes[x0].Update(dt, world);
          }
        this.Rule0(dt, world);

        var t1 = System.DateTime.Now;
        //file.WriteLine((t1 - t).Milliseconds + "," + (t1 - init_time).Seconds);

      //if ((DateTime.Now - first_time).TotalSeconds > 60)
      //{ UnityEditor.EditorApplication.isPlaying = false; }
    }

    public DateTime first_time = DateTime.Now;





    int s0 = -1;
    public void Rule0(float dt, World world)
    {
      switch (s0)
      {

        case -1:
          count_down1 = 2f;
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
          if(Cubes != null)
            Cubes = new Cons<Cube>(new Cube(Color.gray, Cube._GLOBAL_ID++), (Cubes)).ToList<Cube>();
          s0 = -1;
          goto case -1;

        default: return;
      }
    }







  }
  public class Cube
  {
    public int frame;
    public bool JustEntered = true;
    public static int _GLOBAL_ID;
    public int ID;
    public Cube(Color c, int id)
    {      
      JustEntered = false;
      frame = World.frame;
      this.ID = id;
      UnityCube = UnityCube.Instantiate(new UnityEngine.Vector3(UnityEngine.Random.Range(-3, 3), UnityEngine.Random.Range(3, 6), UnityEngine.Random.Range(-3, 3)), c, id);

    }

    public void Send(BitStream stream, NetworkMessageInfo info)
    {
      //NetworkingPrelude.SerializeBase(ID, stream);
      NetworkingPrelude.SerializeBase(frame, stream);
      NetworkingPrelude.SerializeBase(JustEntered, stream);
      NetworkingPrelude.SerializeBase(Position, stream);
      NetworkingPrelude.SerializeBase(Destroyed, stream);
    }

    public void Receive(BitStream stream, NetworkMessageInfo info)
    {
      //ID = NetworkingPrelude.IntReceiveBase(stream);
      frame = NetworkingPrelude.IntReceiveBase(stream);
      JustEntered = NetworkingPrelude.BoolReceiveBase(stream);
      Position = NetworkingPrelude.Vector3ReceiveBase(stream);
      Destroyed = NetworkingPrelude.BoolReceiveBase(stream);
    }



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
    public void Update(float dt, World world)
    {
      frame = World.frame;

      if (Input.GetKeyDown(KeyCode.A))
        Position = Position + new Vector3(-3, 0, 0);
      if (Input.GetKeyDown(KeyCode.D))
        Position = Position + new Vector3(3, 0, 0);
      if (Input.GetKeyDown(KeyCode.UpArrow))
        Position = Position + new Vector3(0, 3, 0);
      if (Input.GetKeyDown(KeyCode.DownArrow))
        Position = Position + new Vector3(0, -3, 0);
      if (Input.GetKeyDown(KeyCode.W))
        Position = Position + new Vector3(0, 0, 3);
      if (Input.GetKeyDown(KeyCode.S))
        Position = Position + new Vector3(0, 0, -3);
      //Debug.Log(this.ID);


    }











  }
}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                   