using UnityEngine;
using System.Collections;

public class UnityFleet : MonoBehaviour {

  TextMesh textMesh;

  public string Info { get { return textMesh.text; } set { textMesh.text = value; } }

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

  public Vector3 Position {
    set { transform.position = value; }
    get { return this.transform.position; } }


  public static UnityFleet Instantiate(Vector3 p, Vector3 to) {
    var object_fleet = GameObject.Instantiate(Resources.Load("Fleet"), p, Quaternion.LookRotation((to-p).normalized)) as GameObject;
    var text_mesh = object_fleet.GetComponentInChildren<TextMesh>();
    text_mesh.transform.rotation = Quaternion.LookRotation(Vector3.down);
    var fleet = object_fleet.GetComponent<UnityFleet>();
    fleet.textMesh = text_mesh;
    return fleet;

  }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          