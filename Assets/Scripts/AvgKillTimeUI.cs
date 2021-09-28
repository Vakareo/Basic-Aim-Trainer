using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class AvgKillTimeUI : MonoBehaviour
{
    private TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        ReturnCenterAimTrainer.OnUpdatedAvg += UpdateText;
    }

    private void UpdateText(int value)
    {
        text.text = value.ToString();
    }

    private void OnDisable()
    {
        ReturnCenterAimTrainer.OnUpdatedAvg -= UpdateText;
    }
}
