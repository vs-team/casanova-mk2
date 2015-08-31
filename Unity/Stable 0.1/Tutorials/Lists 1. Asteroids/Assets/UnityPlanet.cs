using UnityEngine;

public class UnityPlanet : MonoBehaviour
{

	private bool destroyed;
	
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
	
	
	
	public static UnityPlanet Instantiate(Vector3 pos, float mass)
	{
		int asteroidVariant = Random.Range (1,6);
		var planet = GameObject.Instantiate(Resources.Load("Asteroid" + asteroidVariant.ToString()), pos, Quaternion.identity) as GameObject;
		planet.transform.rigidbody.mass = mass;
		var scale = planet.transform.localScale;
		var scaleFactor = mass / 10000.0f;
		planet.transform.localScale = new Vector3(scale.x + scaleFactor, scale.y + scaleFactor, scale.z + scaleFactor);
		return planet.GetComponent<UnityPlanet>();
	}
}
                                                                                                                                                    