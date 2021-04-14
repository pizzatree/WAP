using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private string     parameter    = "SFXVolume";
    [SerializeField] private float      min          = -60, max = 5;
    [SerializeField] private float      defaultValue = 0.11f;

    private Slider slider;

    private void Start()
    {
        slider       = GetComponentInChildren<Slider>();

        slider.minValue = min;
        slider.maxValue = max;
        
        slider.value = PlayerPrefs.GetFloat(parameter, defaultValue);
        ChangeSliderValue(slider.value);
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat(parameter, slider.value);
    }

    public void ChangeSliderValue(float value)
    {
        mixer.SetFloat(parameter, value);
    }
}