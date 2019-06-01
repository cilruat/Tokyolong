using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace PnCCasualGameKit
{
    /// <summary>
    /// Manages sound for the game.
    /// How to Use:
    /// After assigning audio clips, Use the "Generate Enum" button in the inspector to create an enum for audio clips.
    /// Use this enum to play a clip : playsound(AudioClips.clipname). Overloaded methods for directly using the index and name also available 
    /// </summary>
    public class PnCSoundManger : MonoBehaviour
    {
        /// <summary>List of game audios </summary>
        public List<Audio> audios;

        /// <summary>The audio source </summary>
       AudioSource audioSource;

        /// <summary>name and audioclip dictionary for easy access </summary>
        Dictionary<string, AudioClip> soundDictionary = new Dictionary<string, AudioClip>();

        void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            //Create a dictionary of audioName and clip.
            foreach (Audio item in audios)
            {
                soundDictionary.Add(item.audioName, item.clip);
            }
        }

        /// <summary>
        /// Overloaded method to play a clip
        /// param as enum
        /// </summary>
        public void playSound(AudioClips clipName)
        {
            PlaySound(clipName.ToString());
        }

        /// <summary>
        /// Overloaded method to play a clip
        /// param as string.
        /// </summary>
        public void PlaySound(string clipName)
        {
            AudioClip clip = null;
            if (soundDictionary.TryGetValue(clipName, out clip))
            {
                audioSource.clip = clip;
                audioSource.Play();
            }
            else
            {
                Debug.LogError("audio clip does not exist");
            }
        }

        /// <summary>
        /// Overloaded method to play.
        /// param as index
        /// </summary>
        public void playSound(int index)
        {
            audioSource.clip = audios[index].clip;
            audioSource.Play();
        }
    }


    [System.Serializable]
    public class Audio
    {
        public string audioName;
        public AudioClip clip;
    }
}

