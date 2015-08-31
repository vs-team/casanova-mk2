using UnityEngine;

public class CasanovaBehavior : MonoBehaviour
{

  private bool _destroyed;
  public bool Destroyed
  {
    get { return _destroyed; }
    set { _destroyed = value; }
  }


  private Vector3 _position = new Vector3();
  public Vector3 Position
  {
    get { return _position; }
    set { _position = value; }
  }


  void HandlePreUpdate()
  {
    _position = gameObject.transform.position;
  }

  void LateUpdate()
  {
    if (_destroyed) { GameObject.Destroy(gameObject); }
    gameObject.transform.position = _position;
    
  }

}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                      