using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BlockJumpRabbit
{
public class SoundController : MonoBehaviour {

	public AudioSource backgroundAudio;
	public AudioSource gameOverAudio;

	public bool isSoundOn{ get; private set;}

	private AudioSource[] audios;

	void Awake(){
		// Find all audio in game
		audios = FindObjectsOfType<AudioSource> ();

		// First setting sound when get local data
		GetSoundSetting ();

		if (isSoundOn)
			SoundOn ();
		else
			SoundOff ();
	}

	// Method to sound on
	public void SoundOn(){
		isSoundOn = true;
		foreach(AudioSource audio in audios) {
			audio.mute = false;
		}
		SaveSoundSetting (isSoundOn);
	}

	// Method to sound off
	public void SoundOff(){
		isSoundOn = false;
		foreach(AudioSource audio in audios) {
			audio.mute = true;
		}
		SaveSoundSetting (isSoundOn);
	}

	#region Sound Event
	public void SoundBackgroundOn(){
		backgroundAudio.Play ();
	}

	public void SoundBackgroundOff(){
		backgroundAudio.Stop ();
	}

	public void SoundGameOverOn(){
		gameOverAudio.Play ();
	}

	public void SoundGameOverOff(){
		gameOverAudio.Stop ();
	}

	#endregion

	#region Setting Sound to persistent data
	void GetSoundSetting(){
		if (PlayerPrefs.HasKey ("isSound"))
			isSoundOn = (PlayerPrefs.GetInt ("isSound") == 1) ? true : false;
		else{
			PlayerPrefs.SetInt ("isSound", 1);
			isSoundOn = true;
		}
	}

	void SaveSoundSetting(bool isSoundOn){

		if (isSoundOn)
			PlayerPrefs.SetInt ("isSound", 1);
		else
			PlayerPrefs.SetInt ("isSound", 0);
	}
	#endregion
}
}