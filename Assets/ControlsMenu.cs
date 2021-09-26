using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ControlsMenu : MonoBehaviour
{
    [SerializeField] TMP_InputField sensitivity;

    [SerializeField] CameraControls cam;

    private void Start()
    {
        sensitivity.text = cam.settings.sensitivity.ToString();
    }

    public void OnChangedValue()
    {
        var con = cam.settings;
        con.sensitivity = float.Parse(sensitivity.text);
        cam.UpdateSettings(con);
    }

}
