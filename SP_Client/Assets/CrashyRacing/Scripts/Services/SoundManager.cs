using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CrashRacing
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance { get; private set; }

        [Header("Max number allowed of same sounds playing together")]
        public int maxSimultaneousSounds = 7;

        // List of sounds used in this game
        public Sound background;
        public Sound button;
        public Sound coin;
        public Sound gameOver;
        public Sound tick;
        public Sound rewarded;
        public Sound unlock;
        public Sound hitCar;
        public Sound hitGold;

        public delegate void MusicStatusChangedHandler(bool isOn);
        public static event MusicStatusChangedHandler MusicStatusChanged;

        public delegate void SoundStatusChangedHandler(bool isOn);
        public static event SoundStatusChangedHandler SoundStatusChanged;

        enum PlayingState
        {
            Playing,
            Paused,
            Stopped
        }

        public AudioSource bgmSource;
        public AudioSource sfxSource;
        [Tooltip(
            "Audio source to play special sound like 'unlock', 'reward', etc.\n" +
            "Sound played with this source will overwhelms others, including background music (ducking effect)\n" +
            "Call PlaySound() with isSpecialSound set to 'true'")]
        public AudioSource specialSfxSource;

        private PlayingState musicState = PlayingState.Stopped;
        private const string MUTE_PREF_KEY = "MutePreference";
        private const int MUTED = 1;
        private const int UN_MUTED = 0;
        private const string MUSIC_PREF_KEY = "MusicPreference";
        private const int MUSIC_OFF = 0;
        private const int MUSIC_ON = 1;
        private const string SOUND_PREF_KEY = "SoundPreference";
        private const int SOUND_OFF = 0;
        private const int SOUND_ON = 1;

        void Awake()
        {
            if (Instance)
            {
                DestroyImmediate(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        void Start()
        {
            SetMusicOn(!IsMusicOff());
            SetSoundOn(!IsSoundOff());
        }

        /// <summary>
        /// Plays the given sound with option to progressively scale down volume of multiple copies of same sound playing at
        /// the same time to eliminate the issue that sound amplitude adds up and becomes too loud.
        /// </summary>
        /// <param name="sound">Sound.</param>
        /// <param name="isSpecialSound">Set to true if using ducking effect (additional setup needed)</param>
        /// <param name="autoScaleVolume">If set to <c>true</c> auto scale down volume of same sounds played together.</param>
        /// <param name="maxVolumeScale">Max volume scale before scaling down.</param>
        public void PlaySound(Sound sound, bool isSpecialSound = false, bool autoScaleVolume = true, float maxVolumeScale = 1f)
        {
            StartCoroutine(CRPlaySound(sound, isSpecialSound, autoScaleVolume, maxVolumeScale));
        }

        IEnumerator CRPlaySound(Sound sound, bool isSpecialSound = false, bool autoScaleVolume = true, float maxVolumeScale = 1f)
        {
            if (sound.simultaneousPlayCount >= maxSimultaneousSounds)
            {
                yield break;
            }

            sound.simultaneousPlayCount++;

            float vol = maxVolumeScale;

            // Scale down volume of same sound played subsequently
            if (autoScaleVolume && sound.simultaneousPlayCount > 0)
            {
                vol = vol / (float)(sound.simultaneousPlayCount);
            }

            AudioSource src = null;
            if (isSpecialSound)
                src = specialSfxSource;
            if (src == null)
                src = sfxSource;

            src.PlayOneShot(sound.clip, vol);

            float delay = 1;
            // Wait til the sound almost finishes playing then reduce play count
            if(sound.clip)
                delay = sound.clip.length * 0.7f;

            yield return new WaitForSeconds(delay);

            sound.simultaneousPlayCount--;
        }

        /// <summary>
        /// Plays the given music.
        /// </summary>
        /// <param name="music">Music.</param>
        /// <param name="loop">If set to <c>true</c> loop.</param>
        public void PlayMusic(Sound music, bool loop = true)
        {
            bgmSource.clip = music.clip;
            bgmSource.loop = loop;
            bgmSource.Play();
            musicState = PlayingState.Playing;
        }

        /// <summary>
        /// Pauses the music.
        /// </summary>
        public void PauseMusic()
        {
            if (musicState == PlayingState.Playing)
            {
                bgmSource.Pause();
                musicState = PlayingState.Paused;
            }
        }

        /// <summary>
        /// Resumes the music.
        /// </summary>
        public void ResumeMusic()
        {
            if (musicState == PlayingState.Paused)
            {
                bgmSource.UnPause();
                musicState = PlayingState.Playing;
            }
        }

        /// <summary>
        /// Stop music.
        /// </summary>
        public void StopMusic()
        {
            bgmSource.Stop();
            musicState = PlayingState.Stopped;
        }

        public bool IsMusicOff()
        {
            return PlayerPrefs.GetInt(MUSIC_PREF_KEY, MUSIC_ON) == MUSIC_OFF;
        }

        public void SetMusicOn(bool isOn)
        {
            int lastStatus = PlayerPrefs.GetInt(MUSIC_PREF_KEY, MUSIC_ON);
            int status = isOn ? 1 : 0;

            PlayerPrefs.SetInt(MUSIC_PREF_KEY, status);
            bgmSource.mute = !isOn;

            if (lastStatus != status)
            {
                if (MusicStatusChanged != null)
                    MusicStatusChanged(isOn);
            }
        }

        /// <summary>
        /// Toggles the mute status.
        /// </summary>
        public void ToggleMusic()
        {
            if (IsMusicOff())
            {
                // Turn music ON
                SetMusicOn(true);
                if (musicState == PlayingState.Paused)
                {
                    ResumeMusic();
                }
            }
            else
            {
                // Turn music OFF
                SetMusicOn(false);
                if (musicState == PlayingState.Playing)
                {
                    PauseMusic();
                }
            }
        }

        public bool IsSoundOff()
        {
            return PlayerPrefs.GetInt(SOUND_PREF_KEY, SOUND_ON) == SOUND_OFF;
        }

        public void SetSoundOn(bool isOn)
        {
            int lastStatus = PlayerPrefs.GetInt(SOUND_PREF_KEY, SOUND_ON);
            int status = isOn ? 1 : 0;

            PlayerPrefs.SetInt(SOUND_PREF_KEY, status);
            sfxSource.mute = !isOn;
            if (specialSfxSource)
                specialSfxSource.mute = !isOn;

            if (lastStatus != status)
            {
                if (SoundStatusChanged != null)
                    SoundStatusChanged(isOn);
            }
        }

        public void ToggleSound()
        {
            if (IsSoundOff())
            {
                SetSoundOn(true);
            }
            else
            {
                SetSoundOn(false);
            }
        }
    }
}
