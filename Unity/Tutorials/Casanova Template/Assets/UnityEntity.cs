using UnityEngine;

public class UnityEntity : MonoBehaviour
{
  public Color Color {
		get { return gameObject.renderer.material.color;}
		set { gameObject.renderer.material.color = value; }
	}

  public static UnityEntity Find()
  {
		return GameObject.Find("/Cube").GetComponent<UnityEntity>();
  }
}                                                                                                                                                                                     