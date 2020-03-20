using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputController : MonoBehaviour
{
    public static InputController Instance
    {
        get { return _instance; }
    }
    private static InputController _instance;
    [Serializable]
    public class PressEvent : UnityEvent<bool>
    {}
    public PressEvent OnPress;
    public Vector3 PressLocation
    {
        get { return Input.mousePosition;  }
    }
    void Awake()
    {

        if (_instance == null)
            _instance = this;
        else if (_instance != this)
            throw new UnityException("There cannot be more than one InputController.  The instances are " + _instance.name + " and " + name + ".");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            OnPress.Invoke(true);
        if (Input.GetMouseButtonUp(0))
            OnPress.Invoke(false);
    }
}
