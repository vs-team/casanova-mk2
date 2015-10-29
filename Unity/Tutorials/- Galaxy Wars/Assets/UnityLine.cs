using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Casanova.Prelude;
public class UnityLine : MonoBehaviour
{

  public GameObject f, t;

  public Color c1 = Color.yellow;
  public Color c2 = Color.red;
  public int lengthOfLineRenderer = 20;

  public UnityPlanet FromPlanet {
    set
    {
      arrows[value].arrow_renderer.enabled = ActiveAutoRoute;
    }
  }

  private bool activeAutoRoute;
  public bool ActiveAutoRoute
  {
    get { return activeAutoRoute; }
    set { activeAutoRoute = value; }
  }

  public Dictionary<UnityPlanet, UnityArrow> arrows = new Dictionary<UnityPlanet, UnityArrow>();

  void Start()
  {

    var _arrows = gameObject.GetComponentsInChildren<UnityArrow>();
    arrows.Add(f.GetComponent<UnityPlanet>(), _arrows[0]);
    arrows.Add(t.GetComponent<UnityPlanet>(), _arrows[1]);

    _arrows[0].transform.position = f.transform.position + (t.transform.position - f.transform.position).normalized * 2;
    _arrows[1].transform.position = t.transform.position + (f.transform.position - t.transform.position).normalized * 2;

    _arrows[0].transform.rotation = Quaternion.LookRotation((t.transform.position - f.transform.position).normalized * -1);
    _arrows[1].transform.rotation = Quaternion.LookRotation((f.transform.position - t.transform.position).normalized * -1);

    LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
    lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
    lineRenderer.SetColors(c1, c2);
    lineRenderer.SetWidth(0.2F, 0.2F);
    lineRenderer.SetVertexCount(2);

    lineRenderer.SetPosition(0, f.transform.position);
    lineRenderer.SetPosition(1, t.transform.position);
  }

 public static List<UnityLine> FindAll()
  {

    GameObject[] lines;
    lines = GameObject.FindGameObjectsWithTag("Link");
    List<UnityLine> new_links = new List<UnityLine>();
    for (int i = 0; i < lines.Length; i++)
    {
      new_links.Add(lines[i].GetComponent<UnityLine>());
    }
    return new_links;
  }
}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          