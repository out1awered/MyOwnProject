using System.Collections;
using System.Collections.Generic;


using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MusicVolumeSettings : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider musicSlider;

    void Start()
    {
        if (audioMixer == null || musicSlider == null)
        {
            Debug.LogError("AudioMixer or MusicSlider is not assigned!");
            return;
        }

        // 读取之前保存的音量
        float savedVolume =
            PlayerPrefs.GetFloat("MusicVolume", 0.8f);

        // 恢复滑块位置
        musicSlider.SetValueWithoutNotify(savedVolume);

        // 恢复音乐音量
        ApplyMusicVolume(savedVolume);

        // 自动监听滑块变化
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
    }

    public void SetMusicVolume(float value)
    {
        ApplyMusicVolume(value);

        // 保存音量
        PlayerPrefs.SetFloat("MusicVolume", value);
    }

    void ApplyMusicVolume(float value)
    {
        float decibels;

        if (value <= 0.0001f)
        {
            decibels = -80f;
        }
        else
        {
            decibels = Mathf.Log10(value) * 20f;
        }

        audioMixer.SetFloat("MusicVolume", decibels);
    }

    void OnDestroy()
    {
        if (musicSlider != null)
        {
            musicSlider.onValueChanged.RemoveListener(SetMusicVolume);
        }

        PlayerPrefs.Save();
    }
}
