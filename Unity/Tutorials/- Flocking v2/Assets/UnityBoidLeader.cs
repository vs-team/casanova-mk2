using UnityEngine;
using System.Collections;

public class UnityBoidLeader : MonoBehaviour
{

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
  public Vector3 Scale
  {
    get {
      Debug.Log(gameObject.GetComponent<Renderer>().bounds.size);
      return gameObject.GetComponent<Renderer>().bounds.size; }
  }
  public Vector3 Right
  {
    get { return this.transform.right; }
  }
  public Vector3 Backward
  {
    get { return this.transform.forward * -1; }
  }
  public Vector3 Forward
  {
    get { return this.transform.forward; }
  }
  public Vector3 Left
  {
    get { return this.transform.right * -1; }
  }
  public Vector3 Up
  {
    get { return this.transform.up; }
  }
  public Vector3 Down
  {
    get { return this.transform.up * -1; }
  }


  public static UnityBoidLeader Instantiate(Vector3 location)
  {
    var b = GameObject.Instantiate(Resources.Load("BoidLeader"), location, Quaternion.identity) as GameObject;
    b.GetComponent<Renderer>().material.color = new Color(0,0,1,1);
    return b.GetComponent<UnityBoidLeader>();
  }

}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                