using UnityEngine;

public class UnityBall : MonoBehaviour 
{
	private bool quit;
	
	public bool Quit
	{
		set { this.quit = true; Application.Quit(); }
	}
	
	public Vector3 Position
	{
		get { return gameObject.transform.position; }
		set { gameObject.transform.position = value; }
	}
	
	public static UnityBall Find()
	{
		return GameObject.Find("/Ball").GetComponent<UnityBall>();
	}
}
                                                                     