using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UnityText : MonoBehaviour {

  public string Text 
  {
    get { return this.gameObject.GetComponent<Text>().text; }
    set { this.gameObject.GetComponent<Text>().text = value; }
  }

  private static GameObject findRootCanvas(GameObject child)
  {
    if (child.transform.parent == null && child.GetComponent<Canvas>() != null)
      return child;
    else if (child.transform.parent == null)
      throw new System.ArgumentNullException("The UI element is not the child of a Canvas");
    else
      return findRootCanvas(child.transform.parent.gameObject);
  }

  public static UnityText Instantiate(string path, string panelName, int i, int maxPlayers)
  {
    var text = GameObject.Instantiate(Resources.Load(path),Vector3.zero,Quaternion.identity) as GameObject;
    var panel = GameObject.Find(panelName).GetComponent<RectTransform>();
    text.transform.SetParent(panel.transform);
    var canvas = findRootCanvas(text.gameObject);
    const float borderOffset = 15f;
    const float textOffset = 7.5f;
    int playersPerSide = (int)((maxPlayers * 1.0f) / 2.0f + 0.5f);
    float h = text.GetComponent<RectTransform>().rect.height + textOffset;
    text.GetComponent<RectTransform>().localScale *= canvas.GetComponent<CanvasScaler>().scaleFactor;
    float xPosition;
    if (i + 1 > playersPerSide) //Go to right side of screen
    {
      xPosition =  panel.GetComponent<RectTransform>().rect.width - text.GetComponent<RectTransform>().rect.width;
    }
    else
    {
      xPosition = panel.anchoredPosition.x;
    }
    Vector2 position = new Vector2(xPosition + borderOffset, panel.anchoredPosition.y - borderOffset - h * (i % playersPerSide));
    text.GetComponent<RectTransform>().anchoredPosition = position;
    return text.GetComponent<UnityText>();
  }
}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 