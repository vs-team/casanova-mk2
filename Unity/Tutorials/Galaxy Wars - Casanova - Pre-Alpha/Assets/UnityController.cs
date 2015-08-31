using UnityEngine;
using System;
using System.Collections.Generic;

public interface IRTSController {
     bool LeftMouseButton{ get; }
     bool RightMouseButton { get; }
     bool RightMouseButtonUp { get; }
  bool ControlKey { get; }
  bool ShiftKey { get; }
  bool Tab { get; }

}

public class UnityController : MonoBehaviour, IRTSController
{

  /************************************MOUSE*************************************/
  public Vector3 MousePositionOnScreen
  {
    get { return Input.mousePosition; }
  }
  
  public Vector3 MousePositionInWorld
  {
    get {
      float enter;
      Plane XZ = new Plane(new Vector3(0, -1, 0), 5.499999f);
      Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      XZ.Raycast(ray, out enter);
      return ray.GetPoint(enter);

    }
  }
  public bool LeftMouseButton
  {
    get { return Input.GetMouseButtonDown(0); }
  }

  public bool RightMouseButton
  {
    get { return Input.GetMouseButtonDown(1); }
  }
  public bool RightMouseButtonUp
  {
    get { return Input.GetMouseButtonUp(1); }
  }

  public bool Tab
  {
    get
    {
      if (Input.GetKeyDown(KeyCode.Tab))
        return true;
      else
        return false;
    }
  }

  public bool MouseScrollUp
  {
    get
    {
      if (Input.GetAxis("Mouse ScrollWheel") > 0)
        return true;
      else
        return false;
    }
  }

  public bool MouseScrollDown
  {
    get
    {
      if (Input.GetAxis("Mouse ScrollWheel") < 0)
        return true;
      else
        return false;
    }
  }

  /************************************KEYBOARD*************************************/
  public bool ControlKey
  {
    get
    {
      if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        return true;
      else
        return false;
    }
  }

  public bool ShiftKey
  {
    get
    {
      if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        return true;
      else
        return false;
    }
  }

  public bool UpArrow
  {
    get
    {
      if (Input.GetKey(KeyCode.UpArrow))
        return true;
      else
        return false;
    }
  }

  public bool DownArrow
  {
    get
    {
      if (Input.GetKey(KeyCode.DownArrow))
        return true;
      else
        return false;
    }
  }

  public bool RightArrow
  {
    get
    {
      if (Input.GetKey(KeyCode.RightArrow))
        return true;
      else
        return false;
    }
  }

  public bool LeftArrow
  {
    get
    {
      if (Input.GetKey(KeyCode.LeftArrow))
        return true;
      else
        return false;
    }
  }

  public bool A
  {
    get
    {
      if (Input.GetKey(KeyCode.A))
        return true;
      else
        return false;
    }
  }

  public bool D
  {
    get
    {
      if (Input.GetKey(KeyCode.D))
        return true;
      else
        return false;
    }
  }

  public bool S
  {
    get
    {
      if (Input.GetKey(KeyCode.S))
        return true;
      else
        return false;
    }
  }

  public bool W
  {
    get
    {
      if (Input.GetKey(KeyCode.W))
        return true;
      else
        return false;
    }
  }

  public bool Alpha0
  {
    get
    {
      if (Input.GetKey(KeyCode.Alpha0))
        return true;
      else
        return false;
    }
  }

  public bool Alpha1
  {
    get
    {
      if (Input.GetKey(KeyCode.Alpha1))
        return true;
      else
        return false;
    }
  }

  public bool Alpha2
  {
    get
    {
      if (Input.GetKey(KeyCode.Alpha2))
        return true;
      else
        return false;
    }
  }

  public bool Alpha3
  {
    get
    {
      if (Input.GetKey(KeyCode.Alpha3))
        return true;
      else
        return false;
    }
  }

  public bool Alpha4
  {
    get
    {
      if (Input.GetKey(KeyCode.Alpha4))
        return true;
      else
        return false;
    }
  }

  public bool Alpha5
  {
    get
    {
      if (Input.GetKey(KeyCode.Alpha5))
        return true;
      else
        return false;
    }
  }

  public bool Alpha6
  {
    get
    {
      if (Input.GetKey(KeyCode.Alpha6))
        return true;
      else
        return false;
    }
  }

  public bool Alpha7
  {
    get
    {
      if (Input.GetKey(KeyCode.Alpha7))
        return true;
      else
        return false;
    }
  }

  public bool Alpha8
  {
    get
    {
      if (Input.GetKey(KeyCode.Alpha8))
        return true;
      else
        return false;
    }
  }

  public bool Alpha9
  {
    get
    {
      if (Input.GetKey(KeyCode.Alpha9))
        return true;
      else
        return false;
    }
  }

  public bool MinusKey
  {
    get
    {
      if (Input.GetKey(KeyCode.Minus) || Input.GetKey(KeyCode.KeypadMinus))
        return true;
      else
        return false;
    }
  }

  public bool PlusKey
  {
    get
    {
      if (Input.GetKey(KeyCode.Plus) || Input.GetKey(KeyCode.KeypadPlus))
        return true;
      else
        return false;
    }
  }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         