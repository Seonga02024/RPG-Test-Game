using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio; 

public enum BGM
{
    Main = 0
}

public enum SFX
{
    Die = 0,
    Lose = 1,
    Win = 2,
    LevelUp = 3,
    BasicAttack = 4,
    SkillAttack_Knight = 5,
    SkillAttack_Archer = 6,
    SkillAttack_Priest = 7,
    SkillAttack_Thief = 8,
    SelectUI = 9
}

public class GameSoundManager : SingleTon<GameSoundManager>
{
    [Header("[Object Setting]")]
    [SerializeField] private AudioMixer audioMixer;
    private AudioSource audioSource;

    [Header("[BGM]")]
    [SerializeField] private AudioClip[] bgmSources;

    [Header("[SFX]")]
    [SerializeField] private AudioSource[] sfxSource;

    private Coroutine fadeOutCoroutine;
    private bool isPause = false;
    public bool IsPause { get { return isPause; } }
    private BGM bgmCur = BGM.Main;
    private float audioVolume;
    private float audioVolumeCur = 0;

    /////////////////////////////////////////////////////////////////////////////////
    ///

    public void SfxPlay(SFX sfx)
    {
        sfxSource[(int)sfx].Play();
    }

    public void BgmPlay()
    {
        if (!audioSource.isPlaying)
        {
            if (isPause)
            {
                isPause = false;
                audioSource.UnPause();
            }
            else
            {
                audioSource.clip = bgmSources[(int)bgmCur];
                audioSource.Play();
            }
        }
        else
        {
            if (fadeOutCoroutine != null)
            {
                StopCoroutine(fadeOutCoroutine);
            }
            fadeOutCoroutine = StartCoroutine(FadeOut());
        }
    }

    public void BgmPause()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Pause();
            isPause = true;
        }
    }

    public void BgmSet(BGM index)
    {
        bgmCur = index;
    }

    // 0 ~ 1
    public void SetMasterAudioMixer(float value)
    {
        if (value < 0.0001)
        {
            audioMixer.SetFloat("Master", -80);
        }
        else
        {
            audioMixer.SetFloat("Master", Mathf.Log10(value) * 20);
        }
        SaveLoadManager.Instance.SetMasterSound(value);
    }

    // 0 ~ 1
    public void SetBGMAudioMixer(float value)
    {
        if (value < 0.0001)
        {
            audioMixer.SetFloat("BGM", -80);
        }
        else
        {
            audioMixer.SetFloat("BGM", Mathf.Log10(value) * 20);
        }
        SaveLoadManager.Instance.SetBgmSound(value);
    }

    // 0 ~ 1
    public void SetSFXAudioMixer(float value)
    {
        if (value < 0.0001)
        {
            audioMixer.SetFloat("SFX", -80);
        }
        else
        {
            audioMixer.SetFloat("SFX", Mathf.Log10(value) * 20);
        }
        SaveLoadManager.Instance.SetSfxSound(value);
    }

    /////////////////////////////////////////////////////////////////////////////////
    ///

    private void Start(){
        float masterSound = SaveLoadManager.Instance.GetMasterSound();
        float sfxSound = SaveLoadManager.Instance.GetSfxSound();
        float bgmSound = SaveLoadManager.Instance.GetBgmSound();
        SettingSounds(masterSound, bgmSound, sfxSound);

        audioSource = GetComponent<AudioSource>();
        audioVolume = audioSource.volume;
        BgmPlay();
    }

    private void SettingSounds(float masterSound, float bgmSound, float sfxSound){
        SetMasterAudioMixer(masterSound);
        SetSFXAudioMixer(sfxSound);
        SetBGMAudioMixer(bgmSound);
        SystemCanvas.Instance.GetSettingPanel().SettingBGMAndSFXSlider(masterSound, bgmSound, sfxSound);
    }

    // 1sec
    private IEnumerator FadeOut()
    {
        audioVolumeCur = audioVolume;
        float minIntervalTime = Time.deltaTime;
        while (true)
        {
            if (audioVolumeCur > 0)
            {
                audioVolumeCur -= audioVolume * minIntervalTime;
                audioSource.volume = audioVolumeCur;
                yield return new WaitForSeconds(minIntervalTime);
            }
            else
            {
                break;
            }
        }
        audioSource.clip = bgmSources[(int)bgmCur];
        audioSource.Play();
        audioSource.volume = audioVolume;
    }
}
