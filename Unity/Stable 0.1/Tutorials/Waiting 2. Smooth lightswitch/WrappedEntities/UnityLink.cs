using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class UnityLink : MonoBehaviour
{
  public string Beginning;
  public string End;
  public LineRenderer LineRenderer;

  // Use this for initialization
  void Start()
  {
    LineRenderer = this.GetComponent<LineRenderer>();
    LineRenderer.SetVertexCount(2);
    var p1 = GameObject.Find(Beginning);
    var p2 = GameObject.Find(End);
    LineRenderer.SetPosition(0, p1.transform.position);
    LineRenderer.SetPosition(1, p2.transform.position);
  }
}
