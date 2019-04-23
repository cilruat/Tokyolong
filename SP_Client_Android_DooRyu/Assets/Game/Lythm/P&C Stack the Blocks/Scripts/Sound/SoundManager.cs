using UnityEngine;
using PnCCasualGameKit;

namespace PnCCasualGameKit
{
/// <summary>
/// Extended from MISoundManger which provides the basic fucntionalies for managing game sounds.
/// Implements other game specific sound methods
/// </summary>
public class SoundManager : PnCSoundManger {

    public static SoundManager m_instance;

    /// <summary>Singleton Instance </summary>
    public static SoundManager Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<SoundManager>();
            }
            return m_instance;
        }
    }

    //Game's sound state
    private bool isSoundOn = true;

    void Start()
    {
        //get the last stored sound setting
        isSoundOn = PlayerData.Instance.soundSetting;
        applySoundSetting();
    }

    /// <summary>
    /// Toggles the sound on off and saves to storage
    /// </summary>
    public void toggleSoundOnOff()
    {
        playSound(AudioClips.UI);
        isSoundOn = !isSoundOn;
        applySoundSetting();
        PlayerData.Instance.soundSetting = isSoundOn;
        PlayerData.Instance.SaveData();
    }

    /// <summary>
    /// Applies the sound setting.
    /// </summary>
    void applySoundSetting()
    {
        AudioListener.volume = isSoundOn ? 1 : 0;
        UIManager.Instance.ToggleSoundSprite(isSoundOn);
    }
}
}
