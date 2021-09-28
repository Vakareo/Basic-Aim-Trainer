using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Scripting;

[Preserve]
public class BaseGraphicSetting
{
    public List<(string, int)> options;
    public int selected;
    public string propertyName;
    readonly PropertyInfo property;
    public BaseGraphicSetting(string propertyName)
    {
        this.propertyName = propertyName;
        property = typeof(QualitySettings).GetProperty(propertyName);
        var t = property.PropertyType;
        var names = Enum.GetNames(t);
        var vals = Enum.GetValues(t);
        options = new List<(string, int)>();
        var index = 0;
        foreach (var item in vals)
        {
            options.Add((names[index], (int)item));
            index++;
        }
    }
    public BaseGraphicSetting(string propertyName, List<(string, int)> options)
    {
        this.propertyName = propertyName;
        property = typeof(QualitySettings).GetProperty(propertyName);
        this.options = options;
    }

    public void SetValue(int value)
    {
        selected = value;
        property.SetValue(null, options[value].Item2);
    }
}

