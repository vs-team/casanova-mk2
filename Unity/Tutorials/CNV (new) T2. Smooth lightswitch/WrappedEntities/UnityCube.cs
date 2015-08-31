using UnityEngine;
public class UnityCube : MonoBehaviour
{
  public Color Color
  {
    set { gameObject.renderer.material.color = value; }
  }

  public static UnityCube Create()
  {
    return GameObject.Find("/Cube").GetComponent<UnityCube>();
  }
}
