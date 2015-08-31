using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;

public class UnityButton : MonoBehaviour 
{
	private bool clicked;
	
	public bool Clicked
	{
		get { return this.clicked; }
		set { this.clicked = value; }
	}
	
	public bool Disabled
	{
		get { return this.gameObject.GetComponent<Button>().interactable; }
		set { this.gameObject.GetComponent<Button>().interactable = !value; }
	}
	
	public bool Active
	{
		get { return this.gameObject.activeSelf; }
		set { this.gameObject.SetActive(value); }
	}
	
	public string ImageName
	{
		get { return this.GetComponent<Image>().sprite.name; }
		set 
		{
			string imagePath = value;
			string highlightedImagePath = imagePath + "_focused";
      string disabledImagePath = imagePath + "_disabled";
			Sprite newSprite = Resources.Load<Sprite>(imagePath) as Sprite;
			//Debug.Log(newSprite);
			this.gameObject.GetComponent<UnityEngine.UI.Image>().sprite = newSprite;
			Sprite newHighlightedSprite = Resources.Load<Sprite>(highlightedImagePath) as Sprite;
      Sprite newDisabledSprite = Resources.Load<Sprite>(disabledImagePath) as Sprite;
			//Debug.Log(newHighlightedSprite);
			SpriteState spriteState = this.gameObject.GetComponent<UnityEngine.UI.Button>().spriteState;
      spriteState.highlightedSprite = newHighlightedSprite;
      this.gameObject.GetComponent<UnityEngine.UI.Button>().spriteState = spriteState;
      spriteState.highlightedSprite = newHighlightedSprite;
      spriteState.disabledSprite = newDisabledSprite;
      this.gameObject.GetComponent<UnityEngine.UI.Button>().spriteState = spriteState;
			//Debug.Log(this.gameObject.GetComponent<UnityEngine.UI.Button>().spriteState.highlightedSprite);
		}
	}
	
	
	public void Start()
	{
		this.gameObject.GetComponent<Button>().onClick.AddListener(() => {this.clicked = true;});	
	}
	
	public static UnityButton GetButton(string name)
	{
		return GameObject.Find(name).GetComponent<UnityButton>();
	}
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               