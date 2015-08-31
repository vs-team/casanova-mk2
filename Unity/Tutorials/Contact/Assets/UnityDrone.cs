using UnityEngine;
using System.Collections;
using System.Linq;

public class UnityDrone : MonoBehaviour 
{
  private Color ownerColor;
  private Vector3 ownerRelPos;
  private Quaternion ownerStartingRotation;

  public bool Visible
  {
    get
    {
      Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();
      foreach (Renderer renderer in renderers)
      {
        if (!renderer.enabled)
          return false;
      }
      return true;
    }
    set
    {
      Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();
      foreach (Renderer r in renderers)
      {
        r.enabled = value;
      }
    }
  }

  public Color OwnerColor
  {
    get { return ownerColor; }
    set
    {
      Color newColor = value;
      //Debug.Log(value);
      this.gameObject.transform.FindChild("Owner").gameObject.renderer.material.color = newColor;
      ownerColor = newColor;
    }
  }

  public Vector3 Position
  {
    get { return gameObject.transform.position; }
    set { gameObject.transform.position = value; }
  }

  public Quaternion Rotation
  {
    get { return gameObject.transform.rotation; }
    set { gameObject.transform.rotation = value; }
  }

  public Vector3 Forward
  {
    get { return this.gameObject.transform.forward; }
  }

  public static void Revolute(UnityPlanet planet, Vector3 origin, float angle)
  {
    planet.gameObject.transform.RotateAround(origin, Vector3.up, angle);
  }

  void Update()
  {
    this.gameObject.transform.FindChild("Owner").transform.rotation = this.ownerStartingRotation;
    Vector3 parentPos = this.gameObject.transform.position;
    Vector3 scale = this.gameObject.transform.localScale;
    this.gameObject.transform.FindChild("Owner").transform.position =
      new Vector3(parentPos.x + this.ownerRelPos.x * scale.x,
            parentPos.y + this.ownerRelPos.y * scale.y,
            parentPos.z + this.ownerRelPos.z * scale.z);
  }

  public void Start()
  {
    this.ownerStartingRotation = this.gameObject.transform.FindChild("Owner").transform.rotation;
    this.ownerRelPos = this.gameObject.transform.FindChild("Owner").transform.localPosition;
  }

  public static UnityDrone Instantiate(Vector3 pos)
  {
    var drone = GameObject.Instantiate(Resources.Load("Drone"), pos, Quaternion.identity) as GameObject;
    return drone.GetComponent<UnityDrone>();
  }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               