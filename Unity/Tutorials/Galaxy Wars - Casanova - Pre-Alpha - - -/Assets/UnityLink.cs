using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class UnityLink : MonoBehaviour {

  public UnityPlanet startPlanet;
  public UnityPlanet endPlanet;
  public bool UnitySourcesReversed;
  public bool SSLink;

  private bool autohop;

  bool sourceActive, destActive;

  public bool UnityAutoHopActive
  {
    get { return autohop; }
    set {
      LineRenderer lineRenderer = this.GetComponent<LineRenderer>();


      if (!UnitySourcesReversed)
        sourceActive = value;
      else
        destActive = value;

      var source_value = 0.2f;
      var dest_value = 0.2f;
      if (sourceActive) source_value = 1.0f;
      if (destActive) dest_value = 1.0f;

      lineRenderer.SetWidth(source_value, dest_value);        
      
      autohop = value; }
  }
  
  
  public static List<UnityLink> FindLinks()
  {
    var linklist = new List<UnityLink>();
    var links = GameObject.FindGameObjectsWithTag("Link");
    foreach (GameObject l in links)
    {
      linklist.Add(l.GetComponent<UnityLink>());
    }
    return linklist;
  }

  void Start()
  {
    var lr = this.GetComponent<LineRenderer>() as LineRenderer;
    lr.SetPosition(0, startPlanet.Position);
    lr.SetPosition(1, endPlanet.Position);
    this.transform.name = "Link S: " + startPlanet.transform.name + " E: " + endPlanet.transform.name;
    
  }

  public Vector3 Position
  {
    get{return this.transform.position;}
    set{ this.transform.position =  value;}
  }

}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        