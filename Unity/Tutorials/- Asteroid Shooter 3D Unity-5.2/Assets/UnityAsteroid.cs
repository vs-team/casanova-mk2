using UnityEngine;
using System.Collections;

public class UnityAsteroid : MonoBehaviour
{


  public Color Color
  {
    set { gameObject.GetComponent<Renderer>().material.color = value; }
  }

  Shader standard = Shader.Find("Standard");
  Shader transparentDiffuse = Shader.Find("Transparent/Diffuse");
  public float Transparency
  {
    get { return gameObject.GetComponent<Renderer>().material.color.a; }
    set
    {
      var color = gameObject.GetComponent<Renderer>().material.color;
      var newColor = new Color(color.r, color.g, color.b, value);
      gameObject.GetComponent<Renderer>().material.color = newColor;
      if (value >= 1.0f)
      {
        gameObject.GetComponent<Renderer>().material.shader = standard;
      }
      else
      {
        gameObject.GetComponent<Renderer>().material.shader = transparentDiffuse;
      }     
    }
  }


  public Vector3 Position
  {
    get { return gameObject.transform.position; }
    set { gameObject.transform.position = value; }
  }

  bool destroyed = false;
  public bool Destroyed
  {
    get { return destroyed; }
    set
    {
      destroyed = value;
      if (destroyed)
      {
        GameObject.Destroy(gameObject);
      }
    }
  }

  public Quaternion Rotation
  {
    get { return this.transform.rotation; }
    set { this.transform.rotation = value; }
  }

  public static UnityAsteroid Instantiate(Vector3 position)
  {
    var s = GameObject.Instantiate(Resources.Load("Asteroid"), position, Quaternion.identity) as GameObject;
    var a = s.GetComponent<UnityAsteroid>();
    a.Transparency = 0;
    return a;
  }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                