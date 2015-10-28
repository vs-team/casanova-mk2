using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UnityText : MonoBehaviour
{

  public static UnityText Find(string name, bool visible)
  {
    UnityText text = GameObject.Find(name).GetComponent<UnityText>();
    text.Alpha = visible ? 1.0f : 0.0f;
    return text;
  }

  public string Text
  {
    get { return this.gameObject.GetComponent<Text>().text; }
    set { this.gameObject.GetComponent<Text>().text = value;  }
  }

  public float Alpha
  {
    get { return this.gameObject.GetComponent<Text>().color.a; }
    set
    {
      Text text = this.gameObject.GetComponent<Text>();
      Color oldColor = text.color;
      Color newColor = new Color(oldColor.r, oldColor.g, oldColor.b, value);
      text.color = newColor;
    }
  }
}
                                                                                                                                                                                                                                                                                                                                 