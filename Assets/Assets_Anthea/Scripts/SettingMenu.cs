using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SettingMenu : MonoBehaviour
{
    public AudioMixer audioMixer; 
    public Slider musicSlider;    
    public Slider sfxSlider;      

    private void Start()
    {
        
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.75f);

        
        SetMusicVolume(musicSlider.value);
        SetSFXVolume(sfxSlider.value);
    }

    
    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20); 
        PlayerPrefs.SetFloat("MusicVolume", volume); 
    }

    
    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20); // 将线性值转换为对数
        PlayerPrefs.SetFloat("SFXVolume", volume); // 保存音量设置
    }

    
    public void BackToMainMenu()
    {
        SceneManager.LoadScene("StartMenu"); // 假设主菜单场景名为 "MainMenu"
    }
}
