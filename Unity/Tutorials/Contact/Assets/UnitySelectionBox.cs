using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;

public class UnitySelectionBox : MonoBehaviour 
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
			Sprite newSprite = Resources.Load<Sprite>(imagePath) as Sprite;
			//Debug.Log(newSprite);
			this.gameObject.GetComponent<UnityEngine.UI.Image>().sprite = newSprite;
		}
	}
	
	public void Start()
	{
		this.gameObject.GetComponent<Button>().onClick.AddListener(() => {this.clicked = true;});	
	}
	
	public static UnitySelectionBox GetBox(string name)
	{
		return GameObject.Find(name).GetComponent<UnitySelectionBox>();
	}
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     