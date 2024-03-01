using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    private int audioClickCount = 0;
    public GameObject audioButton;
    private int optClickCount = 0;

    [SerializeField] Slider volumeSlider;
    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume",1);
        }
        else
        {
            Load();
        }
    }

    public void ChangeVolume()
    {
        AudioListener.volume = volumeSlider.value;
        Save();
    }

    private void Load()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
    }

    private void Save()
    {
        PlayerPrefs.SetFloat("musicVolume",volumeSlider.value);
    }

    public void ShowVolumeSlider()
    {
        if (audioClickCount == 0)
        {
            volumeSlider.gameObject.SetActive(true);
            audioClickCount++;
        }
        else
        {
            volumeSlider.gameObject.SetActive(false);
            audioClickCount--;
        }
    }

    public void ShowAudioButton()
    {
        if(optClickCount == 0)
        {
            audioButton.SetActive(true);
            optClickCount++;
        }
        else
        {
            audioButton.SetActive(false);
            optClickCount--;
            volumeSlider.gameObject.SetActive(false);
        }
    }
}