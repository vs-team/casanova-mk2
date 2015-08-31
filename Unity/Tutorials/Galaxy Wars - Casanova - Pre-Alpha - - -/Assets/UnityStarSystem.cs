using UnityEngine;
using System.Collections;
using Casanova.Prelude;
using System.Collections.Generic;

public class UnityStarSystem : MonoBehaviour
{

  public List<UnityPlanet> Planets_2 { get; private set; }

      
  public static List<UnityStarSystem> FindAllStarSystems()
  {
    List<UnityStarSystem> starsystems = new List<UnityStarSystem>();
    var searchResult = GameObject.FindGameObjectsWithTag("StarSystem"); // get the starsystems
    foreach (GameObject n in searchResult) // get the planets
    {
      var ss = n.GetComponent<UnityStarSystem>();
      ss.Planets_2 = new List<UnityPlanet>();
      foreach (Transform child in ss.transform)
      {
        var elem = child.GetComponent<UnityPlanet>();
        if(elem!=null)
          ss.Planets_2.Add(elem);
      }

      starsystems.Add(ss);
    }
    return starsystems;
  }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               