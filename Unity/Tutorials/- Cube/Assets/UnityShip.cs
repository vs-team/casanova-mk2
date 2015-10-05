using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class UnityShip : MonoBehaviour
{
  public Vector3 Position
  {
    get { return gameObject.transform.position; }
    set { gameObject.transform.position = value; }
  }

  public static UnityShip Find()
  {
    return GameObject.Find("/Ship").GetComponent<UnityShip>();
  }
}
                                                                                                                      