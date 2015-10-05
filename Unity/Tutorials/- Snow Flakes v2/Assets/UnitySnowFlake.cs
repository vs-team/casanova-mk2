using UnityEngine;
using System.Collections;

public class UnitySnowFlake : MonoBehaviour
{
  public Vector3 Position
  {
    get { return this.transform.position; }
    set { this.transform.position = value; }
  }

  public Quaternion Rotation
  {
    get { return this.transform.rotation; }
    set { this.transform.rotation = value; }
  }

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

  public static UnitySnowFlake Instantiate(Vector3 position)
  {
    var snowFlake = GameObject.Instantiate(Resources.Load("SnowFlake"), position, Quaternion.LookRotation(UnityEngine.Random.insideUnitSphere)) as GameObject;

    var shader = snowFlake.GetComponent<Renderer>().material.shader;
    Material m;

    int rng = UnityEngine.Random.Range(1, 5);// 1 : 5-1
    m = Resources.Load("Materials/SnowFlake" + rng, typeof(Material)) as Material;

    snowFlake.GetComponent<Renderer>().material = m;
    snowFlake.GetComponent<Renderer>().material.shader = shader;

    return snowFlake.GetComponent<UnitySnowFlake>();
  }

}
                                                                                                                                                                      