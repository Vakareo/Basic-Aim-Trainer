using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

public class Menu : MonoBehaviour
{
    [SerializeField] GameObject eventSystem;
    [SerializeField] GameObject GraphicsMenu;
    [SerializeField] GameObject ControlsMenu;
    [SerializeField] GameObject CrosshairMenu;
    private void Awake()
    {
        DontDestroyOnLoad(this);
        DontDestroyOnLoad(eventSystem);

    }
    public void SwitchToCrosshair()
    {
        DisableAllMenus();
        CrosshairMenu.SetActive(true);
    }
    public void SwitchToGraphics()
    {
        DisableAllMenus();
        GraphicsMenu.SetActive(true);
    }
    public void SwitchToControls()
    {
        DisableAllMenus();
        ControlsMenu.SetActive(true);
    }

    private void DisableAllMenus()
    {
        GraphicsMenu.SetActive(false);
        ControlsMenu.SetActive(false);
        CrosshairMenu.SetActive(false);
    }
    private void EnableAllMenus()
    {
        GraphicsMenu.SetActive(true);
        ControlsMenu.SetActive(true);
        CrosshairMenu.SetActive(true);
    }

}
