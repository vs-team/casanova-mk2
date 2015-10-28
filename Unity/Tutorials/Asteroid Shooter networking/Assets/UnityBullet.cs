using UnityEngine;
using System.Collections;

public class UnityBullet : MonoBehaviour
{

  public bool destroyed;
  public bool impact;
  public Camera mainCamera;

  public static UnityBullet Instantiate(Vector3 pos)
  {
    GameObject bull = GameObject.Instantiate(Resources.Load("Bullet"), pos, Quaternion.identity) as GameObject;
    return bull.GetComponent<UnityBullet>();
  }

  void Start()
  {
    this.destroyed = false;
    mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
    impact = false;
  }

  public Vector3 Position
  {
    get { return this.gameObject.transform.position; }
    set { this.gameObject.transform.position = value; }
  }

  void OnTriggerEnter(Collider other)
  {
    if (other.gameObject.tag == "Asteroid" && !other.gameObject.GetComponent<UnityAsteroid>().Destroying)
    {
      impact = true;
    }
  }

  public bool Destroyed
  {
    get { return this.destroyed; }
    set
    {
      this.destroyed = value;
      if (destroyed)
        Destroy(gameObject);
    }
  }

  public bool Impact
  {
    get { return impact; }
    set { impact = value; }
  }

  void Update()
  {

  }

  public Vector3 ViewPortPoint
  {
    get { return this.mainCamera.WorldToViewportPoint(this.gameObject.transform.position); }
  }
}
                                                                                                                                                                                           