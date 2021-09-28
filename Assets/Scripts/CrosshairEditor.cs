using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class CrosshairEditor : MonoBehaviour
{
    [SerializeField] Transform parent;
    [SerializeField] Crosshair crosshair;
    [SerializeField] GameObject boolPrefab;
    [SerializeField] GameObject floatPrefab;
    [SerializeField] GameObject colorPrefab;
    private FieldInfo[] fields;

    private void Start()
    {
        fields = typeof(CrosshairData).GetFields();
        for (int i = 0; i < fields.Length; i++)
        {
            var fieldType = fields[i].FieldType;
            if (fieldType == typeof(float))
            {
                SpawnFloatPrefab(fields[i], i);
                continue;
            }
            if (fieldType == typeof(bool))
            {
                SpawnBoolPrefab(fields[i], i);
                continue;
            }
            if (fieldType == typeof(CrosshairColor))
            {
                SpawnColorPrefab(fields[i], i);
                continue;
            }
        }
    }

    private void SpawnBoolPrefab(FieldInfo fieldInfo, int index)
    {
        var prefab = Instantiate(boolPrefab, parent);
        prefab.GetComponent<BoolUI>().Initialize(this, fieldInfo.Name.ToUpper(), (bool)fieldInfo.GetValue(crosshair.crosshair), index);
    }

    private void SpawnColorPrefab(FieldInfo fieldInfo, int index)
    {
        var prefab = Instantiate(colorPrefab, parent);
        prefab.GetComponent<CrosshairColorUI>().Initialize(this, fieldInfo.Name.ToUpper(), (CrosshairColor)fieldInfo.GetValue(crosshair.crosshair), index);
    }

    private void SpawnFloatPrefab(FieldInfo fieldInfo, int index)
    {
        var prefab = Instantiate(floatPrefab, parent);
        prefab.GetComponent<FloatUI>().Initialize(this, fieldInfo.Name.ToUpper(), (float)fieldInfo.GetValue(crosshair.crosshair), index);
    }

    internal void SetValue(int referenceIndex, object value)
    {
        object modded = crosshair.crosshair;
        fields[referenceIndex].SetValue(modded, value);
        crosshair.crosshair = (CrosshairData)modded;
        crosshair.UpdateCrosshair();
    }
}
