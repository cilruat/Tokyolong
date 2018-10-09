using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGTSoundManager : MonoBehaviour {

    public static CGTSoundManager instance = null;

    public AudioSource efxSource;
    public AudioSource musicSource;

    public CGTSpriteToggle[] Buttons;

    internal float currentState = 1.0f;
    internal bool mute = false;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        //GameObject.DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        UpdateVolume();
    }

    public void UpdateVolume()
    {
        efxSource.volume = PlayerPrefs.GetFloat("SOUND_ON", 1.0f);
        musicSource.volume = PlayerPrefs.GetFloat("MUSIC_ON", 0.3f);

        mute = System.Convert.ToBoolean(PlayerPrefs.GetInt("MUTE_ALL", 0));
        SetSoundOnOff(mute);
    }

    public void PlaySound(AudioClip clip)
    {
        efxSource.clip = clip;
        efxSource.Play();
    }

    public void PlaySound2(AudioClip clip)
    {
        efxSource.clip = clip;
        efxSource.pitch = Random.Range(0.9f, 1.1f);
        efxSource.Play();
    }

    public void ToggleSoundOnOff()
    {
        SetSoundOnOff(!mute);
    }

    public void SetSoundOnOff(bool off)
    {
        mute = off;
        PlayerPrefs.SetInt("MUTE_ALL", mute ? 1 : 0);

        efxSource.mute = off;
        musicSource.mute = off;

        foreach (CGTSpriteToggle buttonToggle in Buttons)
            buttonToggle.SetToggleValue(System.Convert.ToInt32(!mute));
    }

    public bool GetSoundEfxMute()
    {
        return !efxSource.mute;

    }
}
