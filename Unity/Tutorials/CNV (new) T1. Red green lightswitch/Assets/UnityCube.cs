using UnityEngine;
using System.Collections.Generic;
public class UnityCube : MonoBehaviour
{
  public Color Color
  {
		get {return gameObject.renderer.material.color;}
    set { gameObject.renderer.material.color = value; }
  }

  public List<int> lst;

  public static UnityCube Find()
  {
	  var c = GameObject.Find("/Cube").GetComponent<UnityCube>();
    c.lst = new List<int>();
    c.lst.Add(1);
    c.lst.Add(2);
    return c;
  }
}
                                                                                                                                                                                                                                                                                                                                                                                                             