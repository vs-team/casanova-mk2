using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UnityPanel : MonoBehaviour 
{

  public float Alpha
  {
    get { return this.gameObject.GetComponent<Image>().color.a; }
    set 
    {
      Color currentColor = this.gameObject.GetComponent<Image>().color;
      Color newColor = new Color(currentColor.r, currentColor.g, currentColor.b, value);
      this.gameObject.GetComponent<Image>().color = newColor;
    }
  }

	public static UnityPanel GetPanel(string name)
  {
    return GameObject.Find(name).GetComponent<UnityPanel>();
  }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         