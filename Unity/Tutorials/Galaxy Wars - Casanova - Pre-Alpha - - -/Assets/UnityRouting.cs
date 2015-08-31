using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Casanova.Prelude;

public class UnityRouting : MonoBehaviour
{

  public static UnityPlanet[,] nextHop;
  public static List<UnityPlanet> planetList;
  public static List<UnityLink> linkList;


  //public static UnityRouting findRouter()
  //{

  //}

  public static UnityRouting findRouter(List<UnityPlanet> planets, List<UnityLink> links)
  {
    //List<UnityLink> links = new List<UnityLink>();
    //List<UnityPlanet> planets = new List<UnityPlanet>();

    //var tempLinks = GameObject.FindGameObjectsWithTag("Link");
    //var tempPlanets = GameObject.FindGameObjectsWithTag("Planet");

    //foreach (GameObject g in tempLinks)
    //{
    //  links.Add(g.GetComponent<UnityLink>());
    //}

    //foreach (GameObject g in tempPlanets)
    //{
    //  planets.Add(g.GetComponent<UnityPlanet>());
    //}

    float[,] distanceTable = new float[planets.Count, planets.Count];
    nextHop = new UnityPlanet[planets.Count, planets.Count];
    planetList = planets;
    linkList = links;

    for (int a = 0; a < planets.Count; a++)
    {
      for (int b = 0; b < planets.Count; b++)
      {
        distanceTable[a, b] = float.PositiveInfinity;
      }
    }

    for (int q = 0; q < planets.Count; q++)
    {
      distanceTable[q, q] = 0.0f;
      nextHop[q, q] = planets[q];
    }

    foreach (UnityLink e in links)
    {
      distanceTable[planets.IndexOf(e.startPlanet), planets.IndexOf(e.endPlanet)] = Vector3.Distance(e.startPlanet.Position, e.endPlanet.Position);
      distanceTable[planets.IndexOf(e.endPlanet), planets.IndexOf(e.startPlanet)] = Vector3.Distance(e.endPlanet.Position, e.startPlanet.Position);
      nextHop[planets.IndexOf(e.startPlanet), planets.IndexOf(e.endPlanet)] = e.endPlanet;
      nextHop[planets.IndexOf(e.endPlanet), planets.IndexOf(e.startPlanet)] = e.startPlanet;
    }

    for (int k = 0; k < planets.Count; k++)
    {
      for (int i = 0; i < planets.Count; i++)
      {
        for (int j = 0; j < planets.Count; j++)
        {
          if (distanceTable[i, j] > (distanceTable[i, k] + distanceTable[k, j]))
          {
            distanceTable[i, j] = (distanceTable[i, k] + distanceTable[k, j]);
            nextHop[i, j] = nextHop[i, k];
          }
        }
      }
    }

    string hello = "";
    string hello2 = "";

    for (int t = 0; t < planets.Count; t++)
    {
      for (int y = 0; y < planets.Count; y++)
      {
        if(y==planets.Count-1)
        {
         hello+=distanceTable[t, y].ToString()+"\n";
         hello2 += nextHop[t, y].name + "\n";
        
        }
        else
        {
          hello += distanceTable[t, y].ToString() + "\t";
          hello2 += nextHop[t, y].name + "\t";
                  
        }
         
      }
    }

    var temp = GameObject.Find("ShortestPath").GetComponent<UnityRouting>();
    return temp;

  }

  

  public static List<Tuple<UnityPlanet, UnityPlanet>> getHopTable(UnityPlanet p, List<UnityPlanet> planets)
  {
    int index = planets.IndexOf(p);

    List<Tuple<UnityPlanet, UnityPlanet>> hops = new List<Tuple<UnityPlanet, UnityPlanet>>();
    for (int i = 0; i < planets.Count; i++)
    {
      hops.Add(new Tuple<UnityPlanet, UnityPlanet>(planets[i], nextHop[index, i]));
    }
    return hops;
  }

}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            