using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Scripting;

[Preserve]
public class GraphicSettings : MonoBehaviour
{
    public GameObject settingPrefab;
    public Transform settingParent;
    private List<BaseGraphicSetting> settings;
    public Action OnLoad;
    public GameObject screenResPrefab;

    private void Awake()
    {
        CreateListOfSettings();
        SpawnUIObjects();
        GC.Collect();
    }

    private void CreateListOfSettings()
    {
        settings = new List<BaseGraphicSetting>()
        {
            new BaseGraphicSetting(nameof(QualitySettings.vSyncCount), new List<(string, int)>(){
                ("OFF", 0),
                ("ON", 1)
            }),
            new BaseGraphicSetting(nameof(QualitySettings.maxQueuedFrames), new List<(string, int)>(){
                ("0",0),
                ("1",1),
                ("2",2),
                ("3",3),
                ("4",4),
            }),
            new BaseGraphicSetting(nameof(QualitySettings.shadows)),
            new BaseGraphicSetting(nameof(QualitySettings.shadowResolution)),
            new BaseGraphicSetting(nameof(QualitySettings.anisotropicFiltering)),
            new BaseGraphicSetting(nameof(QualitySettings.antiAliasing), new List<(string, int)>(){
                ("NONE", 0),
                ("2XAA", 2),
                ("4XAA", 4),
                ("8XAA", 8),
            }),
        };
        Load();
    }

    private void SpawnUIObjects()
    {
        Instantiate(screenResPrefab, settingParent);
        for (int i = 0; i < settings.Count; i++)
        {
            var obj = Instantiate(settingPrefab, settingParent);
            var gSet = obj.GetComponent<GraphicSettingUI>();
            var optnameList = new List<string>();
            foreach (var item in settings[i].options)
            {
                optnameList.Add(item.Item1);
            }
            gSet.SetProperties(settings[i].propertyName, optnameList.ToArray(), settings[i].selected, i, this);
        }
    }
    internal void ApplyChange(int listReference, int selected)
    {
        settings[listReference].SetValue(selected);
        Save();
    }

    [ContextMenu("load")]
    public void Load()
    {
        var json = "";
        if (File.Exists(Path.Combine(Application.persistentDataPath, "graphics.json")))
        {
            json = File.ReadAllText(Path.Combine(Application.persistentDataPath, "graphics.json"));
        }
        else
        {
            json = File.ReadAllText(Path.Combine(Application.dataPath, "defaultgraphics.json"));
        }

        var state = JsonUtility.FromJson<GraphicSettingState>(json);
        for (int i = 0; i < state.value.Length; i++)
        {
            settings[i].selected = state.value[i];
        }
        OnLoad?.Invoke();
    }
    public int GetSelected(int index)
    {
        return settings[index].selected;
    }

    public void Save()
    {
        var state = new GraphicSettingState();
        var values = new List<int>();
        for (int i = 0; i < settings.Count; i++)
        {
            values.Add(settings[i].selected);
        }
        state.value = values.ToArray();
        var json = JsonUtility.ToJson(state);
        File.WriteAllText(Path.Combine(Application.persistentDataPath, "graphics.json"), json);
    }

    [ContextMenu("Save Default")]
    public void SaveDefault()
    {
        var state = new GraphicSettingState();
        var values = new List<int>();
        for (int i = 0; i < settings.Count; i++)
        {
            values.Add(settings[i].selected);
        }
        state.value = values.ToArray();
        var json = JsonUtility.ToJson(state);
        File.WriteAllText(Path.Combine(Application.dataPath, "defaultgraphics.json"), json);
    }

}

[Preserve]
[System.Serializable]
public struct GraphicSettingState
{
    public int[] value;
}

