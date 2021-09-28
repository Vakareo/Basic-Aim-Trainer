using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResSettingUI : MonoBehaviour
{
    public bool isFullscreen = true;
    public TMP_Dropdown dropdown;
    public Toggle toggle;

    private void Awake()
    {
        PopulateDropdown();
        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            if (Screen.resolutions[i].height == Screen.currentResolution.height && Screen.resolutions[i].width == Screen.currentResolution.width && Screen.resolutions[i].refreshRate == Screen.currentResolution.refreshRate)
            {
                dropdown.value = i;
                break;
            }
        }
        toggle.isOn = isFullscreen;
    }
    private void PopulateDropdown()
    {
        dropdown.options = new List<TMP_Dropdown.OptionData>();
        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            var res = Screen.resolutions[i];
            dropdown.options.Add(new TMP_Dropdown.OptionData($"{res.width}x{res.height}@{res.refreshRate}"));
        }
        GC.Collect();
    }
    public void OnFullscreenChanged()
    {
        isFullscreen = toggle.isOn;
    }
    public void OnDropdownChanged()
    {
        var res = Screen.resolutions[dropdown.value];
        Screen.SetResolution(res.width, res.height, isFullscreen, res.refreshRate);
    }
}
