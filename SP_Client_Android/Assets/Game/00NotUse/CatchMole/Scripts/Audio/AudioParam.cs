using UnityEngine;

[System.Serializable]
public class AudioParam {

    public EnumAudioName AudioName;
    public AudioClip AudioClip;
    public bool loop;
    [Range(0,1f)]
    public float volume;

    [HideInInspector]
    public AudioSource audioSource;
	
}
