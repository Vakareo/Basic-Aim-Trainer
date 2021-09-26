using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CrosshairColorUI : MonoBehaviour
{
    private CrosshairEditor crosshair;
    private CrosshairColor color;
    private int referenceIndex;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] Slider red;
    [SerializeField] Slider green;
    [SerializeField] Slider blue;
    [SerializeField] Slider alpha;



    public void Initialize(CrosshairEditor crosshair, string name, CrosshairColor color, int referenceIndex)
    {
        this.color = color;
        this.crosshair = crosshair;
        this.referenceIndex = referenceIndex;
        red.SetValueWithoutNotify(color.red);
        green.SetValueWithoutNotify(color.green);
        blue.SetValueWithoutNotify(color.blue);
        alpha.SetValueWithoutNotify(color.alpha);
        text.text = name;
    }

    public void OnBlueChanged()
    {
        color.blue = (byte)blue.value;
        crosshair.SetValue(referenceIndex, color);
    }
    public void OnRedChanged()
    {
        color.red = (byte)red.value;
        crosshair.SetValue(referenceIndex, color);
    }
    public void OnGreenChanged()
    {
        color.green = (byte)green.value;
        crosshair.SetValue(referenceIndex, color);
    }
    public void OnAlphaChanged()
    {
        color.alpha = (byte)alpha.value;
        crosshair.SetValue(referenceIndex, color);
    }

}
