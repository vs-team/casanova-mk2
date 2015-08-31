using UnityEngine;

public class UnityPlanet : MonoBehaviour
{
	private bool selected;
	private Color ownerColor;
  private Color targetingPlayerColor;
	private Vector3 ownerRelPos;
  private Vector3 targetingPlayerRelPos;
	private Quaternion ownerStartingRotation;
  private Quaternion targetingPlayerStartingRotation;
	
	public bool Selected
	{
		get 
		{ 
			return this.selected; 
		}
		set 
		{
			this.gameObject.transform.FindChild("Selection").gameObject.SetActive(value);
			this.selected = value; 
		}
	}
	
	public Color OwnerColor
	{
		get { return ownerColor; }
		set
		{
			Color newColor = value;
			//Debug.Log(value);
			this.gameObject.transform.FindChild("Owner").gameObject.renderer.material.color = newColor;
			ownerColor = newColor; 
		}
	}

  public Color TargetingPlayerColor
  {
    get { return targetingPlayerColor; }
    set
    {
      Color newColor = value;
      //Debug.Log(value);
      this.gameObject.transform.FindChild("TargetingPlayer").gameObject.renderer.material.color = newColor;
      targetingPlayerColor = newColor;
    }
  }
	
	public bool ClickedOver
	{
		get 
		{
			RaycastHit hit;
			var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit, 100))
			{
				if (this.gameObject.Equals(hit.collider.gameObject))
				{
					return true;
				}
				else
					return false;
			}
			else {return false;}
		}
	}
	
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
	
	public static void Revolute(UnityPlanet planet, Vector3 origin, float angle)
	{
		planet.gameObject.transform.RotateAround(origin,Vector3.up,angle);
	}
	
	void Update()
	{
    var owner = this.gameObject.transform.FindChild("Owner");
    var targetingPlayer = this.gameObject.transform.FindChild("TargetingPlayer");
    owner.transform.rotation = this.ownerStartingRotation;
    targetingPlayer.transform.rotation = this.targetingPlayerStartingRotation;
		Vector3 parentPos = this.gameObject.transform.position;
		Vector3 scale = this.gameObject.transform.localScale;
		owner.transform.position = 
			new Vector3(parentPos.x + this.ownerRelPos.x * scale.x,
						parentPos.y + this.ownerRelPos.y * scale.y,
						parentPos.z + this.ownerRelPos.z * scale.z);
    targetingPlayer.transform.position =
      new Vector3(parentPos.x + this.targetingPlayerRelPos.x * scale.x,
            parentPos.y + this.targetingPlayerRelPos.y * scale.y,
            parentPos.z + this.targetingPlayerRelPos.z * scale.z);
	}
	
	public void Start()
	{
		this.selected = false;
		this.ownerStartingRotation = this.gameObject.transform.FindChild("Owner").transform.rotation;
    this.targetingPlayerStartingRotation = this.gameObject.transform.FindChild("TargetingPlayer").transform.rotation;
    this.ownerRelPos = this.gameObject.transform.FindChild("Owner").transform.localPosition;
    this.targetingPlayerRelPos = this.gameObject.transform.FindChild("TargetingPlayer").transform.localPosition;
	}	
	
	public static UnityPlanet Instantiate(Vector3 pos)
	{
		int planetVariant = Random.Range (1,5);
		var planet = GameObject.Instantiate(Resources.Load("Planet" + planetVariant.ToString()), pos, Quaternion.identity) as GameObject;
		var r = Random.Range (0.75f,4.0f);
		var randomScale = new Vector3(r,r,r);
		planet.transform.localScale = randomScale;
		return planet.GetComponent<UnityPlanet>();
	}

  public static UnityPlanet Find(string path)
  {
    return GameObject.Find(path).GetComponent<UnityPlanet>();
  }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     