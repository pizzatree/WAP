using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private string     parameter = "SFXVolume";

    private Slider slider;

    private void Start()
    {
        slider       = GetComponentInChildren<Slider>();

        slider.minValue = -60;
        slider.maxValue = 5;
        
        slider.value = PlayerPrefs.GetFloat(parameter, 0.11f);
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