using UnityEngine;
using System.Collections;

public class UnityMinimap : MonoBehaviour 
{

  public Vector3 Position
  {
    get { return this.gameObject.transform.position; }
    set { this.gameObject.transform.position = value; }
  }

  public static UnityMinimap CreateMinimap()
  {
    return GameObject.Find("Minimap").GetComponent<UnityMinimap>();
  }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 