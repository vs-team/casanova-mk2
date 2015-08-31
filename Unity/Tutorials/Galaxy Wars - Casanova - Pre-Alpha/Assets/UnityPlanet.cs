using UnityEngine;
using System.Collections;
using Casanova.Prelude;
using System.Collections.Generic;

public class UnityPlanet : MonoBehaviour
{



  public bool IsFrontier;

  bool selected = false;

  public enum PlanetType { Asteroid, MineralPlanet, Earth };

  public PlanetType Type;

  public bool IsStartingPlanet;

  public int CommanderIndex = -1;

  public static List<UnityPlanet> FindAllPlanets(int x)
  {

    List<UnityPlanet> planets = new List<UnityPlanet>();
    var searchResult = GameObject.FindGameObjectsWithTag("Planet");
    int Counter = x - 1;
    foreach (GameObject n in searchResult)
    {
      var pl = n.GetComponent<UnityPlanet>();
      planets.Add(pl);
    }
    for (int i = planets.Count - 1; i >= 0; i--)
    {
      var idx = Random.Range(0, i);
      var tmp = planets[i];
      planets[i] = planets[idx];
      planets[idx] = tmp;
    }

    foreach (var p in planets)
    {
      if (p.IsStartingPlanet && Counter >= 0)
      {
        p.CommanderIndex = Counter;
        Counter--;
      }
    }
    return planets;
  }

  public static List<UnityPlanet> FindAllTextObjects()
  {
    List<UnityPlanet> planets = new List<UnityPlanet>();
    var searchResult = GameObject.FindGameObjectsWithTag("Planet");
    foreach (GameObject n in searchResult)
    {
      planets.Add(n.GetComponent<UnityPlanet>());
    }
    return planets;
  }

  public string Name
  {
    get { return this.gameObject.name; }
    set {  this.gameObject.name = value; }
  }

  public Vector3 Position
  {
    get { return this.transform.position; }
    set { this.transform.position = value; }
  }

  public bool ActionLight
  {
    get { return this.transform.FindChild("Selection").active; }
    set { this.transform.FindChild("Selection").active = value; }
  }
  public Color MiniMapColor
  {
    set
    {
      this.transform.FindChild("Plane").gameObject.renderer.materials[0].color = value;
    }

    get { return this.transform.FindChild("Plane").gameObject.renderer.materials[0].color; }
  }

  public bool OnMouseOver
  {
    get
    {
      RaycastHit hit;
      var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      if (Physics.Raycast(ray, out hit, 100))
      {
        if (this.gameObject.name.Equals(hit.collider.gameObject.name))
        {
          return true;
        }
        else
          return false;
      }
      else { return false; }
    }
  }

  public Color LightColor
  {
    get { return this.transform.FindChild("Selection").GetComponent<Light>().color; }
    set { this.transform.FindChild("Selection").GetComponent<Light>().color = value; }
  }
}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            