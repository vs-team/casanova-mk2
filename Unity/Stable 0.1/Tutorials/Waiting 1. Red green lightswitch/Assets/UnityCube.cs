using UnityEngine;

public class UnityCube : MonoBehaviour
{
  public Color Color
  {
		get {return gameObject.renderer.material.color;}
    set { gameObject.renderer.material.color = value; }
  }

  public static UnityCube Find()
  {
	return GameObject.Find("/Cube").GetComponent<UnityCube>();
  }
}
                                                                                                                                                                                                                   