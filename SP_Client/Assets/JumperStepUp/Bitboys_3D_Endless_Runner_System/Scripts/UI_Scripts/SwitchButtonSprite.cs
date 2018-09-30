using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// YOU BETTER RUN By BITBOYS STUDIO.

public class SwitchButtonSprite : MonoBehaviour {
	
	[Header("Music and Sfx Image Switch")]

	public GameObject musicButton; //The pause menu music button
	public GameObject settingsMusicButton; // The settings menu music button
	public GameObject sfxButton; // The pause menu sfx button
	public GameObject settingsSfxButton; // the settings button sfx button
	public Sprite musicOnSprite; // the music "On" image
	public Sprite musicOffSprite;// the music "Off" image
	public Sprite sfxOnSprite; // the Sfx "On" sprite
	public Sprite sfxOffSprite; // The sfx "off" sprite


	void Update () {

		// we connect with the level manager to question if the music or the sfx ar actives or not, and depending of the answer we activate an sprite or another.
			
		if (LevelManager.musicActive) {

			musicButton.gameObject.GetComponent<Image> ().sprite = musicOnSprite;
			settingsMusicButton.gameObject.GetComponent<Image> ().sprite = musicOnSprite;


		}

		if (!LevelManager.musicActive) {

			musicButton.gameObject.GetComponent<Image> ().sprite = musicOffSprite;
			settingsMusicButton.gameObject.GetComponent<Image> ().sprite = musicOffSprite;


		}

		if (LevelManager.sfxActive) {

			sfxButton.gameObject.GetComponent<Image> ().sprite = sfxOnSprite;
			settingsSfxButton.gameObject.GetComponent<Image> ().sprite = sfxOnSprite;

		}

		if (!LevelManager.sfxActive) {

			sfxButton.gameObject.GetComponent<Image> ().sprite = sfxOffSprite;
			settingsSfxButton.gameObject.GetComponent<Image> ().sprite = sfxOffSprite;

		}

	}




}