using UnityEngine;
using System.Collections.Generic;

public class UnityPlanet : MonoBehaviour {

  public GameObject initial_owner;

  public string InitialOwnerName { get { if (initial_owner != null) return initial_owner.GetComponent<UnityPlayer>().Name; else return ""; } }

  TextMesh textMesh;

  public string Info { get { return textMesh.text; } set { textMesh.text = value; } }

  public Vector3 Position { get { return this.transform.position; } }

  public bool IsHit
  {
    get
    {

      Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      RaycastHit hit;

      if(this.name.ToLower() == "p5")
      {
        Debug.Log("HI");
      }
      if (Physics.Raycast(ray, out hit))
      {
        Debug.Log("HI2");
        if (this.gameObject.Equals(hit.collider.gameObject))
          return true;
        else
          return false;
      }
      else
        return false;
    }
  }

  MeshRenderer selection_light;

  public bool selected;
  public bool Selected
  {
    get { return selected; }
    set
    {
      selected = value;
      if (value)
      {
        selection_light.enabled = true;
      }
      else
      {
        selection_light.enabled = false;
      }
    }
  }
  public bool rightSelected;
  public bool RightSelected
  {
    get { return rightSelected; }
    set
    {
      rightSelected = value;
      if (value)
      {
        selection_light.enabled = true;
      }
      else
      {
        selection_light.enabled = false;
      }
    }
  }

  public static List<UnityPlanet> FindAll() { 

    GameObject[] planets;
    planets = GameObject.FindGameObjectsWithTag("Planet");
    List<UnityPlanet> new_planets = new List<UnityPlanet>();
    for (int i = 0; i < planets.Length; i++)
    {
      var p = planets[i].GetComponent<UnityPlanet>();
      var text_mesh = p.gameObject.GetComponentInChildren<TextMesh>();
      p.textMesh =  text_mesh;
      foreach (var item in p.gameObject.GetComponentsInChildren<MeshRenderer>())
      {
        if(item.gameObject.GetComponentInChildren<UnitySelection>() != null)
          p.selection_light = item;

      }
      new_planets.Add(p);
    }
    return new_planets;
  }

}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                