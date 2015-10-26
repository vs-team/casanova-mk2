using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class UnityShip : MonoBehaviour
{
  public UnityCamera Camera;
  // Use this for initialization
  void Start()
  {
  }

  // Update is called once per frame
  void Update()

  { 
  }


  public Vector3 Position
  {
    get { return gameObject.transform.position; }
    set { gameObject.transform.position = value; }

  }

  public Quaternion Rotation
  {
    get { return gameObject.transform.rotation; }
    set { gameObject.transform.rotation = value; }

  }

  public static UnityShip Instantiate()
  {
    var s = GameObject.Find("Ship") as GameObject;
    return s.GetComponent<UnityShip>();
    //return GameObject.Find("/Ship").GetComponent<UnityShip>();
  }
}
                                                         