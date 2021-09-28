using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
[RequireComponent(typeof(TextMeshProUGUI))]
public class FpsCounter : MonoBehaviour
{
    public float updateRate;
    private TextMeshProUGUI text;
    private WaitForSecondsRealtime waitTime;
    private float deltaTime;
    private int frameCount;

    private void Awake()
    {
        updateRate = Mathf.Clamp(updateRate, 1f, 165f);
        waitTime = new WaitForSecondsRealtime(1f / updateRate);
        text = GetComponent<TextMeshProUGUI>();
        text.text = "";
        StartCoroutine(UpdateFps());
    }

    private void Update()
    {
        deltaTime += Time.unscaledDeltaTime;
        frameCount++;
    }

    IEnumerator UpdateFps()
    {
        while (true)
        {
            if (deltaTime != 0)
            {

                text.text = ((int)(1f / (deltaTime / frameCount))).ToString();
                deltaTime = 0;
                frameCount = 0;
            }
            yield return waitTime;
        }
    }


}
