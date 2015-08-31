using UnityEngine;
using System.Collections;

public class UnityMiniMap : MonoBehaviour {
  public float CameraSize
  {
    get { return this.gameObject.camera.orthographicSize; }
    set { this.gameObject.camera.orthographicSize = value; }
  }
  public Vector3 CameraPosition
  {
    get { return this.gameObject.transform.position; }
    set { this.gameObject.transform.position = value; }
  }

  public bool Hide
  {
    get { return this.gameObject.activeSelf; }
    set { this.gameObject.SetActive(value); }
  }

  public static UnityMiniMap CreateMiniMapCamera()
  {
    return GameObject.Find("/MiniMapCamera").GetComponent<UnityMiniMap>();
  }
}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            