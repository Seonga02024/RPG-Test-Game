using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : MonoBehaviour, PanelController
{
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private Slider masterSlider;

    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;

    public void OnPanel(bool isActive)
    {
        mainPanel.SetActive(isActive);
    }

    public void ChangeMasterSound(){
        GameSoundManager.Instance.SetMasterAudioMixer(masterSlider.value);
    }

    public void ChangeBGMSound(){
        GameSoundManager.Instance.SetBGMAudioMixer(bgmSlider.value);
    }

    public void ChangeSFXSound(){
        GameSoundManager.Instance.SetSFXAudioMixer(sfxSlider.value);
    }

    public void ClickRestartGameBtn(){
        GameManager.Instance.GameOver();
        OnPanel(false);
    }

    public void ClickQuitGameBtn(){
        // save data
        SaveLoadManager.Instance.SaveData();
        #if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
		#else
			Application.Quit();
		#endif
    }

    public void SettingBGMAndSFXSlider(float masterSound, float bgmSound, float sfxSound){
        masterSlider.value = masterSound;
        bgmSlider.value = bgmSound;
        sfxSlider.value = sfxSound;
    }
}
