using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FloatUI : MonoBehaviour
{
    private CrosshairEditor crosshair;
    private int referenceIndex;

    [SerializeField] TMP_InputField inputField;
    [SerializeField] TextMeshProUGUI text;

    public void Initialize(CrosshairEditor crosshair, string name, float value, int referenceIndex)
    {
        this.crosshair = crosshair;
        this.referenceIndex = referenceIndex;
        text.text = name;
        inputField.text = value.ToString();
    }

    public void OnFloatChanged()
    {
        crosshair.SetValue(referenceIndex, float.Parse(inputField.text));
    }
}
