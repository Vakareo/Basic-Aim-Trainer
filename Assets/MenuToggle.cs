using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuToggle : MonoBehaviour
{
    public GameObject menu;
    public Controls controls;
    private InputAction toggle;
    private void Awake()
    {
        controls = new Controls();
    }

    private void OnEnable()
    {
        toggle = controls.Global.MenuToggle;
        toggle.Enable();
        toggle.performed += ToggleMenu;
    }

    private void ToggleMenu(InputAction.CallbackContext obj)
    {
        menu.SetActive(!menu.activeSelf);
    }

    private void OnDisable()
    {
        toggle.performed -= ToggleMenu;
        toggle.Disable();
    }
}
