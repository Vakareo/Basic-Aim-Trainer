using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Scripting;

public class MenuToggle : MonoBehaviour
{
    public GameObject menu;
    public Controls controls;
    private InputAction toggle;
    private void Awake()
    {
        controls = new Controls();
#if !UNITY_EDITOR
        GarbageCollector.GCMode = GarbageCollector.Mode.Manual;
#endif
    }

    private void OnEnable()
    {
        toggle = controls.Global.MenuToggle;
        toggle.Enable();
        toggle.performed += ToggleMenu;
        ToggleMenu();
        ToggleMenu();
    }

    private void ToggleMenu(InputAction.CallbackContext obj)
    {
        ToggleMenu();
    }

    private void ToggleMenu()
    {
        var value = !menu.activeSelf;
        menu.SetActive(value);
        SetCursorActive(value);
        if (value)
        {
            GC.Collect();
        }
    }

    private void SetCursorActive(bool value)
    {
        if (value)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;
        }
    }

    private void OnDisable()
    {
        toggle.performed -= ToggleMenu;
        toggle.Disable();
    }
}
