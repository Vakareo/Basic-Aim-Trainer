using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class TestStarter : MonoBehaviour
{
    public InputAction inputOne;
    public InputAction inputTwo;
    public UnityEvent eventOne;
    public UnityEvent eventTwo;


    private void OnEnable()
    {
        inputOne.Enable();
        inputTwo.Enable();
        inputOne.performed += InvokeEventOne;
        inputTwo.performed += InvokeEventTwo;
    }
    private void InvokeEventOne(InputAction.CallbackContext obj)
    {
        eventOne?.Invoke();
    }

    private void InvokeEventTwo(InputAction.CallbackContext obj)
    {
        eventTwo?.Invoke();
    }


    private void OnDisable()
    {
        inputOne.Disable();
        inputTwo.Disable();
        inputOne.performed -= InvokeEventOne;
        inputTwo.performed -= InvokeEventTwo;
    }
}

