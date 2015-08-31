using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnityShip : MonoBehaviour
{
  public Vector3 Position
  {
    get { return gameObject.transform.position; }
    set { gameObject.transform.position = value; }
  }
  private Vector3 velocity;
  public Vector3 Velocity
  {
    get { return velocity; }
    set
    {
      velocity = new Vector3(value.x, value.y, value.z);
      velocity = Vector3.Normalize(velocity) * 2; //* 0.1f;

      transform.rotation = Quaternion.LookRotation(velocity);

    }
  }

  Transform trail;
  public float Scale
  {
    get
    {
      return gameObject.transform.localScale.x;
    }
    set
    {
      gameObject.transform.localScale = new Vector3(value, value, value);
      trail.gameObject.SetActive(value >= 0.5f);
    }
  }

  public float alpha, dist;
  public Vector3 avoidance;
  public string closestPlanetName;
  bool destroyed = false;
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

  void Start()
  {
    trail = gameObject.transform.FindChild("Trail");
    Scale = 0;
  }
  public List<Collider> PlanetsAndStars;

  public static UnityShip Instantiate(Vector3 start, Vector3 end, Vector3 init_velocity)
  {

    var lo = GameObject.Instantiate(Resources.Load("Ship"), Vector3.zero, Quaternion.identity) as GameObject;
    UnityShip ship = lo.gameObject.GetComponent<UnityShip>();

    var planets_and_stars = new List<Collider>();
    foreach (var p in UnityPlanet.FindAllPlanets())
    {
      planets_and_stars.Add(new Collider() { Position = p.Position, Radius = 2.0f, gameObject = p.gameObject });
    }

    var objPlanets = GameObject.FindGameObjectsWithTag("Star");
    Debug.Log(objPlanets);
    foreach (var obj in objPlanets)
    {
      planets_and_stars.Add(new Collider() { Position = obj.transform.position, Radius = 4.0f, gameObject = obj.gameObject });
    }

    ship.PlanetsAndStars = planets_and_stars;

    ship.Velocity = init_velocity;
    ship.Position = start;
    return ship;
  }
}

public struct Collider
{
  public Vector3 Position;
  public float Radius;
  public GameObject gameObject;
  public static bool operator ==(Collider c1, Collider c2)
  {
    return c1.Equals(c2);
  }
  public static bool operator !=(Collider c1, Collider c2)
  {
    return !c1.Equals(c2);
  }
}
                                                                                                                             