using UnityEngine;
using System.Collections;

public class UnityPause : MonoBehaviour
{


  void Start()
  {
  }

  public string Texture
  {
    set
    {
      if (renderer.material.mainTexture.name != value)
      {
        var _texture = Resources.Load(value) as Texture;
        renderer.material.mainTexture = _texture;
      }
    }
    get { return renderer.material.mainTexture.name; }
  }
  private Texture new_texture;

  void Update()
  {
  }

  public Transform pauseButton;

  public bool IsPausePressed
  {
    get
    {
      RaycastHit hit;
      var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      return Physics.Raycast(ray, out hit, 100) && pauseButton.gameObject.name.Equals(hit.collider.gameObject.name);
    }
  }
  public static UnityPause Find()
  {

    UnityPause unityPause = GameObject.Find("/PauseButton").GetComponent<UnityPause>();
    IEnumerable childs = unityPause.transform;
    foreach (Transform child in childs)
    {
      if (child.name == "Button")
      {
        unityPause.pauseButton = child;
        Debug.Log("Found button!");
      }
    }
    return unityPause;
  }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          