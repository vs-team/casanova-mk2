using UnityEngine;
using System.Collections;

public class UnitySelectionRectangle : MonoBehaviour {

  public Vector3 Position
  {
    get { return this.transform.position; }
    set { this.transform.position = value; }
  }
  public Vector3 Scale
  {
    get { return this.transform.localScale; }
    set { this.transform.localScale = value; }
  }
  public void Start()
  {
    Renderer r = this.gameObject.GetComponent<Renderer>();
    r.material.color = new Color(1, 0, 0, 0.32f);
  }
  private bool destroyed;
  public bool Destroyed
  {
    get { return destroyed; }
    set
    {
      destroyed = value;
      if (destroyed)
        GameObject.Destroy(gameObject);
    }
  }
  
  public static UnitySelectionRectangle Instantiate(Vector3 Start)
  {
    var plane = GameObject.Instantiate(Resources.Load("Prefabs/SelectionRect"), Start, Quaternion.identity) as GameObject;
    plane.transform.localScale = Vector3.zero;
    return plane.GetComponent<UnitySelectionRectangle>();
  }
  

  /*
     public bool OnMouseOver
    {
      get
      {
        RaycastHit hit;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 100))
        {
          if (this.gameObject.name.Equals(hit.collider.gameObject.name))
          {
            return true;
          }
          else
            return false;
        }
        else { return false; }
      }
    }
 
   */
}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            