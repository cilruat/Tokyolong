using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Emoji2
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance;

        [System.Serializable]
        public class Sound
        {
            public AudioClip clip;
            [HideInInspector]
            public int simultaneousPlayCount = 0;
        }

        [Header("Max number allowed of same sounds playing together")]
        public int maxSimultaneousSounds = 7;

        public Sound button;
        public Sound jump;
        public Sound gameOver;

        public delegate void OnMuteStatusChanged(bool isMuted);

        public static event OnMuteStatusChanged MuteStatusChanged;

        private AudioSource audioSource;
        private const string MUTE_PREF_KEY = "MutePreference";
        private const int MUTED = 1;
        private const int UN_MUTED = -1;

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
            // Get audio source component
            audioSource = GetComponent<AudioSource>();

            // Set mute based on the valued stored in PlayerPrefs
            SetMute(IsMuted());
        }

        /// <summary>
        /// Plays the given sound with option to progressively scale down volume of multiple copies of same sound playing at
        /// the same time to eliminate the issue that sound amplitude adds up and becomes too loud.
        /// </summary>
        /// <param name="sound">Sound.</param>
        /// <param name="autoScaleVolume">If set to <c>true</c> auto scale down volume of same sounds played together.</param>
        /// <param name="maxVolumeScale">Max volume scale before scaling down.</param>
        public void PlaySound(Sound sound, bool autoScaleVolume = true, float maxVolumeScale = 1f)
        {
            StartCoroutine(CRPlaySound(sound, autoScaleVolume, maxVolumeScale));	
        }

        IEnumerator CRPlaySound(Sound sound, bool autoScaleVolume = true, float maxVolumeScale = 1f)
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
                        
            audioSource.PlayOneShot(sound.clip, vol);

            // Wait til the sound almost finishes playing then reduce play count
            float delay = sound.clip.length * 0.7f;

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
            audioSource.clip = music.clip;
            audioSource.loop = loop;
            audioSource.Play();
        }

        /// <summary>
        /// Pauses the music.
        /// </summary>
        public void PauseMusic()
        {
            audioSource.Pause();
        }

        /// <summary>
        /// Resumes the music.
        /// </summary>
        public void ResumeMusic()
        {
            audioSource.UnPause();
        }

        /// <summary>
        /// Stop music.
        /// </summary>
        public void Stop()
        {
            audioSource.Stop();
        }

        /// <summary>
        /// Determines whether sound is muted.
        /// </summary>
        /// <returns><c>true</c> if sound is muted; otherwise, <c>false</c>.</returns>
        public bool IsMuted()
        {
            return (PlayerPrefs.GetInt(MUTE_PREF_KEY, UN_MUTED) == MUTED);
        }

        /// <summary>
        /// Toggles the mute status.
        /// </summary>
        public void ToggleMute()
        {
            // Toggle current mute status
            bool mute = !IsMuted();

            if (mute)
            {
                // Muted
                PlayerPrefs.SetInt(MUTE_PREF_KEY, MUTED);

                if (MuteStatusChanged != null)
                {
                    MuteStatusChanged(true);
                }
            }
            else
            {
                // Un-muted
                PlayerPrefs.SetInt(MUTE_PREF_KEY, UN_MUTED);

                if (MuteStatusChanged != null)
                {
                    MuteStatusChanged(false);
                }
            }

            SetMute(mute);
        }

        void SetMute(bool isMuted)
        {
            audioSource.mute = isMuted;
        }
    }
}
