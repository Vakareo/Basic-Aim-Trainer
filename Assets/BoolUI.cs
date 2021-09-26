using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoolUI : MonoBehaviour
{
    private CrosshairEditor crosshair;
    private int referenceIndex;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] Toggle toggle;

    public void Initialize(CrosshairEditor crosshair, string name, bool value, int referenceIndex)
    {
        this.crosshair = crosshair;
        this.referenceIndex = referenceIndex;
        text.text = name;
        toggle.SetIsOnWithoutNotify(value);
    }

    public void OnValueChanged()
    {
        crosshair.SetValue(referenceIndex, toggle.isOn);
    }
}
