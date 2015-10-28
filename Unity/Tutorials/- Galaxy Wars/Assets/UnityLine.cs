using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnityLine : MonoBehaviour
{

  public GameObject f, t;

  public Color c1 = Color.yellow;
  public Color c2 = Color.red;
  public int lengthOfLineRenderer = 20;

  void Start()
  {
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