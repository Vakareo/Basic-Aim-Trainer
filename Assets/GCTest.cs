using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GCTest : MonoBehaviour
{

    public InputAction action;

    private void OnEnable()
    {
        action.Enable();
    }
    private void OnDisable()
    {
        action.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        if (action.ReadValue<float>() > 0)
        {
            GC.Collect();
        }
    }
}
