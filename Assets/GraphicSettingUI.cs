using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GraphicSettingUI : MonoBehaviour
{
    private GraphicSettings settings;
    private int listReference;
    private int selected;
    private string[] options;
    private string name;

    [SerializeField]
    private TMP_Dropdown optionDropdown;
    [SerializeField]
    private TextMeshProUGUI text;

    public void SetProperties(string name, string[] options, int selected, int listReference, GraphicSettings settings)
    {
        if (this.settings)
        {
            this.settings.OnLoad -= OnLoad;
        }
        this.name = name;
        this.options = options;
        this.selected = selected;
        this.listReference = listReference;
        this.settings = settings;
        text.text = name;

        SetDropdownOptions();
        SetDropdownValue();
        this.settings.OnLoad += OnLoad;
    }

    private void OnLoad()
    {
        selected = settings.GetSelected(listReference);
        SetDropdownValue();
    }

    private void OnDestroy()
    {
        settings.OnLoad -= OnLoad;
    }
    private void SetDropdownOptions()
    {
        optionDropdown.options = new List<TMP_Dropdown.OptionData>();
        for (int i = 0; i < options.Length; i++)
        {
            optionDropdown.options.Add(new TMP_Dropdown.OptionData(options[i]));
        }
    }
    public void SetDropdownValue()
    {
        optionDropdown.value = selected;
    }

    public void OnDropdownChanged()
    {
        selected = optionDropdown.value;
        settings.ApplyChange(listReference, selected);
    }

}
