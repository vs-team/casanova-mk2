using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnityShip : MonoBehaviour {

  public static UnityShip Instantiate(Vector3 startPos, Vector3 target)
  {
    var ship = GameObject.Instantiate(Resources.Load("Prefabs/Ship"), startPos, Quaternion.identity) as GameObject;
    
    var unity_ship = ship.GetComponent<UnityShip>();
    unity_ship.Rotation = target.normalized;
    
    return unity_ship;
  }

  public static List<UnityShip> FindAllShips()
  {
    List<UnityShip> ships = new List<UnityShip>();
    var searchResult = GameObject.FindGameObjectsWithTag("Ship");
    foreach (GameObject n in searchResult)
    {
      ships.Add(n.GetComponent<UnityShip>());
    }
    return ships;
  }

  public string ShipOwnerText
  {
    get { return this.transform.FindChild("ShipOwnerText").GetComponent<TextMesh>().text; }
    set { this.transform.FindChild("ShipOwnerText").GetComponent<TextMesh>().text = value; }
  }
  public Vector3 ShipOwnerTextRotation
  {
    get { return this.transform.FindChild("ShipOwnerText").rotation.eulerAngles; }
    set { this.transform.FindChild("ShipOwnerText").rotation = Quaternion.Euler(value); }
  }
    
	public Vector3 Position
  {
    get { return this.transform.position; }
    set { this.transform.position = value; }
  }

  public float Scale
  {
    get
    {
      return gameObject.transform.localScale.x;
    }
    set
    {
      gameObject.transform.localScale = new Vector3(value, value, value);
    }
  }

  public Vector3 Rotation
  {
    set { this.transform.rotation = Quaternion.LookRotation(value); }
  }
  private bool destroyed;
  public bool Destroyed
  {
    get { return destroyed; }
    set
    {
      destroyed = value;
      if (destroyed)
        GameObject.Destroy(gameObject);
    }
  }
  public bool ExplodeAndDestroyed
  {
    get { return destroyed; }
    set
    {
      destroyed = value;
      if (destroyed)
      {
        Instantiate(Resources.Load("Prefabs/ExplodeShip"), Position, Quaternion.identity);
        GameObject.Destroy(gameObject);
      }
    }
  }
  public Color MiniMapColor
  {
    set
    {
      this.transform.FindChild("Plane").gameObject.renderer.materials[0].color = value;
    }

    get { return this.transform.FindChild("Plane").gameObject.renderer.materials[0].color; }
  }
}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    