using UnityEngine;
using System.Collections.Generic;
using System;
using System.Threading;

public class UnityPlanet : MonoBehaviour 
{
  public DisplayDepth DP;
  public Vector2 LP;
  public int minX;
  public int maxX;
  public int minZ;
  public int maxZ;
  public float deltaX;
  public float deltaZ;

	public bool ClickedOver
	{
		get 
		{
			RaycastHit hit;
			var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit, 100))
			{
				if (this.gameObject.name.Equals(hit.collider.gameObject.name))
				{
					//Debug.Log(this.gameObject.name + " selected");
					return true;
				}
				else
					return false;
			}
			else {return false;}
		}
	}

  GameObject selection;
	public bool Selected
	{
    get { return selection.activeSelf; }
    set { selection.SetActive(value); }
  }


  public void Start()
  {
    selection = transform.FindChild("Selection").gameObject;
    Selected = false;
    try
    {
      DP = GameObject.Find("DepthImagePlane").GetComponent("DisplayDepth") as DisplayDepth;
    }
    catch (Exception e)
    {
      Debug.Log(e.Message);
    }
    minX = -40;
    maxX = 18;
    minZ = -15;
    maxZ = 18;
    deltaX = maxX - minX;
    deltaZ = maxZ - minZ;
    LP = normalize();

  }
  public void Update()
  {
     PositionY = DP.getValue((int)(this.LP.x + (this.LP.y * 320))) * 5;
  }

    public Vector2 normalize()
  {
    float newX = (Position.x - minX) / deltaX;
    float newZ = (Position.z - minZ) / deltaZ;

    return new Vector2(newX, newZ);

  }
  


	public static void Print(string message)
	{
		Debug.Log (message);
	}

  public float PositionY
  {
    get { return gameObject.transform.position.y; }
    set { gameObject.transform.position = new Vector3(Position.x, value, Position.z);}
  }

	public Vector3 Position
	{
		get { return gameObject.transform.position; }
		set { gameObject.transform.position = value; }
	}
	
	public int Ships = 0;
	
	public static List<UnityPlanet> FindAllPlanets()
	{
		var objPlanets = GameObject.FindGameObjectsWithTag("Planet");
		var planets = new List<UnityPlanet>();
		foreach (var obj in objPlanets) 
		{
			var component = obj.GetComponent<UnityPlanet> ();
			planets.Add (component);
		}
		//Debug.Log (planets.Count.ToString());
		return planets;
	}
  

}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            