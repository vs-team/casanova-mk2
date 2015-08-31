using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace WrappedEntities
{
  public class WrappedShip
  {
    GameObject ship;
    public Vector3 Position { get { return ship.transform.position; } set { ship.transform.position = value; } }

    public bool Destroyed { set { if (value) UnityEngine.Object.Destroy(ship); } }

    static public WrappedShip Create(Vector3 position)
    {
      var ship = UnityEngine.Object.Instantiate(Resources.Load("Ship"), position, Quaternion.identity) as GameObject;
      return new WrappedShip() { ship = ship };
    }
  }

  public class WrappedLink
  {
    UnityLink Link;
    public GameObject BeginningGameObject { get; set; }
    public GameObject EndGameObject { get; set; }

    const float selectedWidth = 2.0f, unselectedWidth = 0.5f;
    float startWidth = unselectedWidth, endWidth = unselectedWidth;
    public bool StartToEndEnabled
    {
      set
      {
        if (value)
          startWidth = selectedWidth;
        else
          startWidth = unselectedWidth;
        Link.LineRenderer.SetWidth(startWidth, endWidth);
      }
    }

    public bool EndToStartEnabled
    {
      set
      {
        if (value)
          endWidth = selectedWidth;
        else
          endWidth = unselectedWidth;
        Link.LineRenderer.SetWidth(startWidth, endWidth);
      }
    }

    public static WrappedLink Create(GameObject game_object)
    {
      var link = game_object.GetComponent<MonoBehaviour>() as UnityLink;
      var ret = new WrappedLink();
      ret.Link = link;
      ret.BeginningGameObject = GameObject.Find(link.Beginning);
      ret.EndGameObject = GameObject.Find(link.End);
      return ret;
    }
  }

  public class WrappedPlanet
  {
    UnityPlanet planet;
    public bool LeftDown { get { return planet.LeftDown; } set { planet.LeftDown = value; } }
    public bool RightDown { get { return planet.RightDown; } set { planet.RightDown = value; } }
    public string OwnerName { set { planet.OwnerText.text = value; } }
    public string NumArmies { set { planet.NumArmiesText.text = value; } }

    public static WrappedPlanet Create(GameObject game_object)
    {
      var planet = game_object.GetComponent<MonoBehaviour>() as UnityPlanet;
      var ret = new WrappedPlanet() { planet = planet };
      return ret;
    }
  }

}
