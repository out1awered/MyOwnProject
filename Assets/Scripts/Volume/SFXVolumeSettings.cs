using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SFXVolumeSettings : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider sfxSlider;

    void Start()
    {
        if (audioMixer == null || sfxSlider == null)
        {
            Debug.LogError("SFX AudioMixer or Slider is missing!");
            return;
        }

        // 读取上次保存的音效音量
        float savedVolume =
            PlayerPrefs.GetFloat("SFXVolume", 0.8f);

        // 恢复滑块位置
        sfxSlider.SetValueWithoutNotify(savedVolume);

        // 恢复音效音量
        ApplySFXVolume(savedVolume);

        // 自动监听滑块变化
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    public void SetSFXVolume(float value)
    {
        ApplySFXVolume(value);

        // 保存音效音量
        PlayerPrefs.SetFloat("SFXVolume", value);
    }

    void ApplySFXVolume(float value)
    {
        value = Mathf.Clamp01(value);

        float decibels;

        if (value <= 0.0001f)
        {
            decibels = -80f;
        }
        else
        {
            decibels = Mathf.Log10(value) * 20f;
        }

        audioMixer.SetFloat("SFXVolume", decibels);
    }

    void OnDestroy()
    {
        if (sfxSlider != null)
        {
            sfxSlider.onValueChanged.RemoveListener(SetSFXVolume);
        }

        PlayerPrefs.Save();
    }
}
