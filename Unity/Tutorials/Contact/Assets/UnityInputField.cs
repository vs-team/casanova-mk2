using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UnityInputField : MonoBehaviour 
{
	
	private int maxValue = int.MaxValue;
  private int minValue = int.MinValue;
	
	public int MaxValue
	{
		get { return this.maxValue; }
		set { this.maxValue = value; }
	}

  public int MinValue
  {
    get { return this.minValue; }
    set { this.minValue = value; }
  }
	
	void Update()
	{
    if (this.gameObject.transform.FindChild("Placeholder").GetComponent<Text>().text == "" && this.Text == "")
    {
      this.gameObject.GetComponent<InputField>().text = minValue.ToString();
    }
    else if (System.Convert.ToInt32(this.Text) < minValue && !this.gameObject.GetComponent<InputField>().isFocused)
    {
      this.gameObject.GetComponent<InputField>().text = minValue.ToString();
    }
    else if (System.Convert.ToInt32(this.Text) > maxValue && !this.gameObject.GetComponent<InputField>().isFocused)
		{
			//Debug.Log("Setting max value to " + this.maxValue.ToString());
			this.gameObject.GetComponent<InputField>().text = maxValue.ToString();
		}
	}
	
	public string Text
	{
		get 
		{
			GameObject textObject = this.gameObject.transform.FindChild("Text").gameObject;
			string text = textObject.GetComponent<Text>().text;
			if (text == "")
				return this.gameObject.transform.FindChild("Placeholder").gameObject.GetComponent<Text>().text;
			else
				return text;
		}
    set { this.gameObject.transform.FindChild("Text").gameObject.GetComponent<Text>().text = value; }
	}

	
	public static UnityInputField Find(string name)
	{
		return GameObject.Find(name).GetComponent<UnityInputField>();
	}
}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 