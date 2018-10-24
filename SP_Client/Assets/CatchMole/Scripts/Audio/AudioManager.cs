using System.Collections.Generic;
using UnityEngine;


public class AudioManager : MonoBehaviour {

    public List<AudioParam> allAudio; // list with parameters for each sound on the stage
    public static AudioManager audioManager;

    private void Awake() 
    {
        audioManager = gameObject.GetComponent<AudioManager>();

        foreach (AudioParam audioParam in allAudio) {
            audioParam.audioSource = gameObject.AddComponent<AudioSource>();
            audioParam.audioSource.clip = audioParam.AudioClip;
            audioParam.audioSource.volume = audioParam.volume;
            audioParam.audioSource.loop = audioParam.loop;
        }
    }


    public void PlayAudio(EnumAudioName name, bool play) { // This method is called to play the sound. It takes the name of the sound and the type of boolean
        int count = 0;
        for (int i = 0; i < allAudio.Count; i++) {
            if (allAudio[i].AudioName == name) {
                if (play) {
                    allAudio[i].audioSource.Play();
                    break;
                } else {
                    allAudio[i].audioSource.Stop();
                    break;
                }
            }
            else {
                count++;
                if (count == allAudio.Count) {
                    return;
                }
            }
        }
    }
}
