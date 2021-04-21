using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SensitivityAdjustment : MonoBehaviour
{
    private TMP_InputField inputField;

    private void Start()
    {
        inputField                 = GetComponentInChildren<TMP_InputField>();
        
        Preferences.AimSensitivity = PlayerPrefs.GetFloat("AimSensitivity", 150);
        inputField.text            = Preferences.AimSensitivity.ToString();
    }
    
    public void ChangeSensitivity(string value)
    {
        var parsed = float.Parse(value);
        Preferences.AimSensitivity = parsed;
        PlayerPrefs.SetFloat("AimSensitivity", parsed);
    }
}
