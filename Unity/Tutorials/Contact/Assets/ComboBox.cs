using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ComboBox : MonoBehaviour 
{
	private List<GameObject> buttons = new List<GameObject>();
	private GameObject rootCanvas;
	private float buttonDistance = 1.5f;
	
	public float ButtonDistance
	{
		get { return this.buttonDistance; }
	}
	
	public GameObject RootCanvas
	{
		get { return this.rootCanvas; }
	}
	
	public string SelectionName
	{
		get { return this.gameObject.transform.FindChild("ComboButton").FindChild("Text").GetComponent<Text>().text; }
	}
	
	public static void AddButton(ComboBox box, string text)
	{
		//Debug.Log("Adding button...");
		GameObject panel = box.transform.FindChild("Panel").gameObject;
		List<GameObject> children = new List<GameObject>(box.transform.FindChild("Panel").childCount);
		for (int i = 0; i < box.transform.FindChild("Panel").childCount; i++)
			children.Add(panel.transform.GetChild(i).gameObject);
		if (children.Exists(child => child.transform.FindChild("Text").GetComponent<Text>().text == text)) return;
		GameObject newButton = GameObject.Instantiate(box.transform.FindChild("ComboButton").gameObject) as GameObject;
		GameObject parent = box.transform.parent.gameObject;
		float canvasScaleFactor = box.RootCanvas.GetComponent<Canvas>().scaleFactor;
		newButton.transform.SetParent(box.gameObject.transform.FindChild("Panel"));	
		newButton.GetComponent<Button>().onClick.RemoveAllListeners();
		newButton.GetComponent<Button>().onClick.AddListener(() => box.comboButtonPressed(newButton));
		newButton.transform.FindChild("Text").GetComponent<Text>().text = text;
		RectTransform buttonTransform = newButton.GetComponent<RectTransform>();
		RectTransform panelTransform = box.gameObject.transform.FindChild("Panel").gameObject.GetComponent<RectTransform>();
		Rect buttonRect = buttonTransform.rect;
		Rect panelRect = panelTransform.rect;
		//float distance = box.ButtonDistance / canvasScaleFactor;
		//Debug.Log(distance);
		float offset = buttonRect.height * (box.buttons.Count); //+ distance;
		Vector2 panelPos = panelTransform.anchoredPosition;
		//Debug.Log(panelPos);
		Vector2 buttonPos = new Vector2(panelTransform.anchoredPosition.x, panelTransform.anchoredPosition.y - offset);
		//Debug.Log (buttonPos);
		buttonTransform.anchoredPosition = buttonPos;
		float newHeigth = buttonRect.height * (box.buttons.Count + 1.0f);// + distance;
		panelTransform.sizeDelta = new Vector2(panelRect.width, newHeigth);
		newButton.transform.localScale = newButton.transform.localScale * canvasScaleFactor;
		box.buttons.Add(newButton);
		if (box.buttons.Count == 1)
			box.transform.FindChild("ComboButton").FindChild("Text").gameObject.GetComponent<Text>().text = text;
	}
	
	public static void RemoveButton(ComboBox box, string text)
	{
		Transform panelTransform = box.transform.FindChild("Panel");
		List<GameObject> children = new List<GameObject>(panelTransform.childCount);
		GameObject header = box.transform.FindChild("ComboButton").gameObject;
		//float canvasScaleFactor = box.RootCanvas.GetComponent<Canvas>().scaleFactor;
		for (int i = 0; i < panelTransform.childCount; i++)
			children.Add(panelTransform.GetChild(i).gameObject);
		if (!children.Exists(child => child.transform.FindChild("Text").GetComponent<Text>().text == text)) return;
		GameObject removingObject = 
			children.Find(button => button.transform.FindChild("Text").GetComponent<Text>().text.Equals(text));
		if (children.Count <= 1) throw new System.ArgumentException("The combo box must have more than 1 element to allow removing");
		if (text == (header.transform.FindChild("Text").gameObject.GetComponent<Text>().text))
		{
			GameObject validChild = 
				children.Find(child => child.transform.FindChild("Text").gameObject.GetComponent<Text>().text != text);
			header.transform.FindChild("Text").gameObject.GetComponent<Text>().text = 
				validChild.transform.FindChild("Text").gameObject.GetComponent<Text>().text;
		}
		RectTransform buttonTransform = removingObject.GetComponent<RectTransform>();
		RectTransform panelRectTransform = box.gameObject.transform.FindChild("Panel").gameObject.GetComponent<RectTransform>();
		Rect panelRect = panelRectTransform.rect;
		Rect buttonRect = buttonTransform.rect;
		float buttonHeight = buttonRect.height;
		children.Clear();
		box.buttons.Remove(removingObject);
		GameObject.DestroyImmediate(removingObject);
		//float distance = box.ButtonDistance / canvasScaleFactor;
		for (int i = 0; i < panelTransform.childCount; i++)
			children.Add(panelTransform.GetChild(i).gameObject);
		for (int i = 0; i < children.Count; i++)
		{
			float offset = buttonHeight * i; //+ distance;
			Vector2 buttonPos = new Vector2(panelRectTransform.anchoredPosition.x, panelRectTransform.anchoredPosition.y - offset);
			RectTransform childTransform = panelTransform.GetChild(i).gameObject.GetComponent<RectTransform>();
			childTransform.anchoredPosition = buttonPos;	
		}
		float newHeigth = buttonRect.height * children.Count; //+ distance;
		panelRectTransform.sizeDelta = new Vector2(panelRectTransform.rect.width, newHeigth);
	}
	
	public void titlePressed()
	{
		if (this.buttons.Count <= 1) return;
		this.transform.FindChild("Panel").gameObject.SetActive(true);
		this.transform.FindChild("ComboButton").gameObject.SetActive(false);
		this.transform.FindChild("Scrollbar").gameObject.SetActive(true);
	}
	
	public void comboButtonPressed(GameObject button)
	{
		GameObject comboTitle = this.transform.FindChild("ComboButton").FindChild("Text").gameObject;
		comboTitle.GetComponent<Text>().text = button.transform.FindChild("Text").GetComponent<Text>().text;
		this.transform.FindChild("Panel").gameObject.SetActive(false);
		this.transform.FindChild("ComboButton").gameObject.SetActive(true);
		this.transform.FindChild("Scrollbar").gameObject.SetActive(false);
	}
	
	System.Collections.IEnumerator Test()
	{
		yield return new WaitForSeconds(0.5f);	
	}
	
	private GameObject findRootCanvas(GameObject child)
	{
		if (child.transform.parent == null && child.GetComponent<Canvas>() != null)
			return child;
		else if (child.transform.parent == null)
			throw new System.ArgumentNullException("The Combo box is not the child of a Canvas");
		else
			return findRootCanvas(child.transform.parent.gameObject);
	}

	
	void Start() 
	{
		this.rootCanvas = findRootCanvas(this.gameObject);
		//Debug.Log(rootCanvas);
		/*GameObject buttonTemplate = this.gameObject.transform.FindChild("ComboButton").gameObject;
		ComboBox.AddButton(this, buttonTemplate.transform.FindChild("Text").GetComponent<Text>().text);*/
	}
	
	public static ComboBox Find(string name)
	{
		return GameObject.Find(name).GetComponent<ComboBox>();
	}
	
	public static ComboBox Find(string name, List<string> elements)
	{
		ComboBox box = GameObject.Find(name).GetComponent<ComboBox>();
		foreach (string element in elements)
			ComboBox.AddButton(box, element);
		return box;
	}
	
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       