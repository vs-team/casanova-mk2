using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ComboList : MonoBehaviour 
{
	private List<GameObject> buttons = new List<GameObject>();
	private GameObject rootCanvas;
	private float buttonDistance = 1.5f;
	private GameObject currentSelection;
	private int currentSelectionIndex;
	
	public int CurrentSelectionIndex
	{
		get { return this.currentSelectionIndex; }
	}

	public float ButtonDistance
	{
		get { return this.buttonDistance; }
	}
	
	public GameObject RootCanvas
	{
		get { return this.rootCanvas; }
	}

  public List<GameObject> Buttons
  {
    get { return this.buttons; }
  }

  public static void Reset(ComboList box)
  {
    box.Reset();
  }

  public string SelectionName
  {
    get 
    {
      if (this.currentSelection != null)
        return this.currentSelection.gameObject.transform.FindChild("Text").GetComponent<Text>().text;
      else
        return "";
    }
  }

  private void Reset()
  {
    for (int i = 0; i < this.buttons.Count; i++)
    {
      ComboList.RemoveButton(this, this.buttons[i].transform.FindChild("Text").GetComponent<Text>().text);
      i--;
    }
    this.currentSelectionIndex = 0;
  }

  public static void printComboList(ComboList list)
  {
    if (list != null)
      Debug.Log(list.comboListToString());
  }

  private string comboListToString()
  {
    string s = "";
    s = "BUTTONS:\n";
    foreach (var button in this.buttons)
    {
      s += button.ToString() + " ";
    }
    s += "\n";
    s += "CURRENT SELECTION:\n" + (currentSelection != null ? this.currentSelection.ToString() : "null") + "\n";
    s += "CURRENT SELECTION INDEX:\n" + this.currentSelectionIndex.ToString();
    return s;
  }

  
	
	public static void AddButton(ComboList box, string text)
	{
		//Debug.Log("Adding button...");
		GameObject panel = box.transform.FindChild("Panel").gameObject;
		List<GameObject> children = new List<GameObject>(box.transform.FindChild("Panel").childCount);
		for (int i = 0; i < box.transform.FindChild("Panel").childCount; i++)
			children.Add(panel.transform.GetChild(i).gameObject);
		if (children.Exists(child => child.transform.FindChild("Text").GetComponent<Text>().text == text)) return;
		GameObject newButton = GameObject.Instantiate(box.transform.FindChild("ComboButton").gameObject) as GameObject;
		newButton.SetActive(true);
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
	}
	
	public static void RemoveButton(ComboList box, string text)
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
	
	public void comboButtonPressed(GameObject button)
	{
		this.currentSelection = button;
		button.transform.FindChild("Background").gameObject.SetActive(true);
		foreach (GameObject element in this.buttons)
		{
			if (element != button)
				element.transform.FindChild("Background").gameObject.SetActive(false);
		}
		this.currentSelectionIndex = this.buttons.FindIndex(element => element == button);
	}
	
	private GameObject findRootCanvas(GameObject child)
	{
		if (child.transform.parent == null && child.GetComponent<Canvas>() != null)
			return child;
		else if (child.transform.parent == null)
			throw new System.ArgumentNullException("The Combo list is not the child of a Canvas");
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
	
	public static ComboList Find(string name)
	{
		return GameObject.Find(name).GetComponent<ComboList>();
	}
	
	public static ComboList Find(string name, List<string> elements)
	{
		ComboList box = GameObject.Find(name).GetComponent<ComboList>();
		foreach (string element in elements)
			ComboList.AddButton(box, element);
		return box;
	}
	
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     