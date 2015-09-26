using UnityEngine;

public class UnityEntity : MonoBehaviour
{
  public Color Color {
		get { return gameObject.GetComponent<Renderer>().material.color;}
		set { gameObject.GetComponent<Renderer>().material.color = value; }
	}

  public static UnityEntity Find()
  {
		return GameObject.Find("/Cube").GetComponent<UnityEntity>();
  }
}                                                                                                                                                                                          