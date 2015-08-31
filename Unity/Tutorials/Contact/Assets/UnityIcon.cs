using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UnityIcon : MonoBehaviour 
{

	private bool destroyed;
	
	public string Text
	{
		get 
		{ 
			return this.gameObject.transform.FindChild("Text").GetComponent<Text>().text;
		}
		set 
		{ 
			var text = this.gameObject.transform.FindChild("Text").GetComponent<Text>();
			text.text = value;
		}
	}
	
	public float Width
	{
		get { return this.gameObject.GetComponent<RectTransform>().rect.width; }
	}
	
	public float Height
	{
		get { return this.gameObject.GetComponent<RectTransform>().rect.height; }
	}

  public Color Color
  {
    get { return this.gameObject.renderer.material.color; }
    set { this.gameObject.renderer.material.color = value; }
  }
	
	public Vector2 Origin
	{
		get
		{
			RectTransform rectTransform = this.gameObject.GetComponent<RectTransform>();
			Vector2 pos = new Vector2(rectTransform.anchoredPosition.x,rectTransform.anchoredPosition.y);
			return pos;
		}
		set 
		{ 
			RectTransform rectTransform = this.gameObject.GetComponent<RectTransform>();
			Vector2 pos = new Vector2(value.x,value.y);
			//Debug.Log (pos);
			rectTransform.anchoredPosition = pos;
		}
	}
	
	public static void Destroy(UnityIcon icon)
	{
		GameObject.Destroy(icon.gameObject);
	}
	
	public string ImageName
	{
		get { return this.GetComponent<Image>().sprite.name; }
		set 
		{
			string imagePath = value;
			string highlightedImagePath = imagePath + "_focused";
			Sprite newSprite = Resources.Load<Sprite>(imagePath) as Sprite;
			//Debug.Log(newSprite);
			this.gameObject.GetComponent<UnityEngine.UI.Image>().sprite = newSprite;
			//Debug.Log(newHighlightedSprite);
		}
	}
	
	public bool Active
	{
		get { return this.gameObject.activeSelf; }
		set
		{
			this.gameObject.SetActive(value);
		}
	}

	public static UnityIcon GetIcon(string name)
	{
		return GameObject.Find(name).GetComponent<UnityIcon>();
	}
	
	public static UnityIcon Instantiate(string name, string spriteName, string parentPath, Vector3 pos)
	{
		GameObject newIcon = GameObject.Instantiate(Resources.Load(name),pos,Quaternion.identity) as GameObject;
		GameObject parent = GameObject.Find(parentPath);
		newIcon.transform.SetParent(parent.transform,false);
		newIcon.transform.parent = parent.transform;
		UnityIcon newIconScript = newIcon.GetComponent<UnityIcon>();
		newIconScript.ImageName = spriteName;
		return newIconScript;
	}
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           