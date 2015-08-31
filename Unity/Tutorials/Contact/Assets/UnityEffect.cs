using UnityEngine;
using System.Collections.Generic;

public class UnityEffect : MonoBehaviour 
{
  private bool destroyed = false;

  public Vector3 Position
  {
    get { return this.gameObject.transform.position; }
    set { this.gameObject.transform.position = value; }
  }

  public Quaternion Rotation
  {
    get { return this.gameObject.transform.rotation; }
    set { this.gameObject.transform.rotation = value; }
  }

  public Vector3 Scale
  {
    get { return this.gameObject.transform.localScale; }
    set { this.gameObject.transform.localScale = value; }
  }

  public string MaterialName
  {
    get { return this.gameObject.GetComponent<MeshRenderer>().materials[0].name; }
    set 
    {
      Material mat = Resources.Load(value) as Material;
      //Debug.Log(mat);
      this.gameObject.GetComponent<MeshRenderer>().material = mat;
    }
  }

  public float Alpha
  {
    get { return this.gameObject.renderer.material.color.a; }
    set
    {
      Color matColor = this.gameObject.renderer.material.color;
      Color c = new Color(matColor.r, matColor.g, matColor.b, value);
      this.gameObject.renderer.material.color = c;
    }
  }

  public bool Destroyed
  {
    get { return destroyed; }
    set
    {
      destroyed = value;
      if (destroyed)
      {
        Destroy(this.gameObject);
      }
    }
  }

  public Vector3 Forward
  {
    get { return this.gameObject.transform.forward; }
  }

  public Vector3 Down
  {
    get { return -this.gameObject.transform.up; }
  }

  public static UnityEffect Instantiate(string materialName, Vector3 position, Quaternion rotation, Vector3 scale)
  {
    var effect = GameObject.Instantiate(Resources.Load("2DEffect"), position, Quaternion.identity) as GameObject;
    effect.GetComponent<UnityEffect>().MaterialName = materialName;
    //Debug.Log(effect.GetComponent<UnityEffect>().MaterialName);
    effect.transform.localScale = scale;
    effect.transform.rotation = rotation;
    return effect.GetComponent<UnityEffect>();
  }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            