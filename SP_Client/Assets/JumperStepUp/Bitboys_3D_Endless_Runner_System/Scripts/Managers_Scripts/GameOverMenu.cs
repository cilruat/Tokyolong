using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
// 3D ENDLESS RUNNER SYSTEM By BITBOYS STUDIO.
public class GameOverMenu : MonoBehaviour {


	[Header("Game Over Manager")]
	public GameObject gameOverPanel; // The game over panel image
	private CamShake shake; // Call the camera shake script
	private DistanceCounter distanceCount; // call the distance counter script.
	private GoldeEggCounter goldenEgg; // call the egg counter script.
	public Button continueButton; // the continue game button object
	public GameObject resetButton; // the reset button object
	public GameObject gameOverLogo; // the game over logo image
	public GameObject pauseButton; // the pause button object
	public GameObject videoAdsButton; // the video ads button
	public GameObject buyButton; // the google shop button
	private UIController uIControl; // call the ui animations script.
	public static bool newBest = false; // sets the new best bool to false to can be called after by the distance counter script and can spawn some particles if we reach a new best.
	public GameObject newBestEffects; // the new best particle effects.
	public float fireworksDuration = 10f; // set the duration of the new best particle effects.
	private LevelManager manager; // call the level manager script.
	[Header("Sound Stuff *(Volume Controlled from the Mixer)")]
	public AudioSource deadAudioSource;
	public AudioClip deadSfx;
	public AudioSource scoreMusicAudioSource;
	public AudioSource startMusicAudioSource;
	public AudioSource newBestAudioSource;
	public AudioClip newBestSfx;
	public AudioSource menuWooshAudioSource;
	public AudioClip menuWooshSfx;

	[Header("Continue Button Timer")]
	float timeLeft = 10f;
	public bool continueTimer = false;
	public Text countdownText;
	public GameObject timeOutSprite;
	public GameObject continueSprite;


	void Start () {

		shake = FindObjectOfType<CamShake> ();	
		manager = FindObjectOfType<LevelManager> ();
		distanceCount = FindObjectOfType<DistanceCounter> ();
		goldenEgg = FindObjectOfType<GoldeEggCounter> ();
		uIControl = FindObjectOfType<UIController> ();

	}

	void Update () {

		if (continueTimer) {

			timeLeft -= Time.deltaTime;
			countdownText.text = "" + Mathf.Round (timeLeft);
			continueSprite.gameObject.SetActive (true);
			timeOutSprite.gameObject.SetActive (false);

			if (timeLeft < 0) {
				
				countdownText.text = "";
				continueTimer = false;
				continueSprite.gameObject.SetActive (false);
				timeOutSprite.gameObject.SetActive (true);

			}
		} 

		// if the "character selection" pop up menu , or the "credits" pop up menu or the "how to play" pop up menu are open, we set inactive the reset button and the game over image logo.
		if (manager.shopping || manager.viewingCredits || manager.viewingHowToPlay) {

			resetButton.gameObject.SetActive (false);
			gameOverLogo.gameObject.SetActive (false);

		}
		// get back the reset button and the game over logo if the above bools are false.
		if (!manager.shopping && manager.inGameOverScene && !manager.viewingCredits && !manager.viewingHowToPlay) {

			resetButton.gameObject.SetActive (true);
			gameOverLogo.gameObject.SetActive (true);

		}

		// Use this to can set the music on and off with the setting buttons
		if (!LevelManager.sfxActive) {

			deadAudioSource.volume = 0;
			newBestAudioSource.volume = 0;
			menuWooshAudioSource.volume = 0;

		} else {

			deadAudioSource.volume = 1;
			newBestAudioSource.volume = 1;
			menuWooshAudioSource.volume = 1;

		}

		if (!LevelManager.musicActive) {

			scoreMusicAudioSource.volume = 0;
			startMusicAudioSource.volume = 0;
		} else {

			scoreMusicAudioSource.volume = 1;
			startMusicAudioSource.volume = 1;


		}

		/////////////////////////////////////// /////////////////////////////////////// /////////////////////////////////////// ///////////////////////////////////////

		if (!goldenEgg.canContinue || continueTimer == false) { // if we can't continue because we have 0 golde eggs, we set the continue button to not interactable.

			continueButton.interactable = false;


		} else {

			continueButton.interactable = true;

		}
	}

	  /////////////////////////////////////// /////////////////////////////////////// /////////////////////////////////////// ///////////////////////////////////////


	public void ContinueGame(){ // We can call this function from the continue button only if we have more that 1 golde egg. This function is called by the continue button(Set in the editor)


		StartCoroutine (Continue ()); 

	}

	public IEnumerator Continue(){
		

		if (goldenEgg.canContinue && continueButton.interactable == true) {

			manager.Continue (); // Call the Level manager to start the continue function.

			// Plays the menu woosh sound effect
			if (!menuWooshAudioSource.isPlaying) {

				menuWooshAudioSource.PlayOneShot(menuWooshSfx,1f); 
			}
			// Stop the score scene music
			if (scoreMusicAudioSource.isPlaying) {

				scoreMusicAudioSource.Stop (); // Play the break  music
			}
			// Plays the start music
			if (!startMusicAudioSource.isPlaying) {

				startMusicAudioSource.Play (); // Play the break  music

			}


			gameOverPanel.gameObject.GetComponent<FadeTexture> ().FadeOut (); // fade out the game over panel image.

			gameOverPanel.gameObject.GetComponent<Animator> ().SetTrigger ("PanelOff"); // trigger the animation that makes the game over panel disappear.

			uIControl.GameOverPanelUp (); // Call the uicontrol script to trigger the animation that makes the game over upper panel go up.

			uIControl.SettingButtonHide (); // Hide the Settings menu pivot object with an amimation.
			uIControl.SettingButtonClose (); // close al the settings menu buttons with animation.

			goldenEgg.SpendEggs (); // Call the spend eggs function of the golden egg counter script, with this we sustract 1 egg from the total amount of eggs.

			newBestEffects.gameObject.SetActive (false); // Set inactive the new best particle effects.

			yield return new WaitForSeconds (0.5f);

			gameOverPanel.gameObject.SetActive (false); // disable the game over panel to avoid bad performance.

		}
	}



	public void ResetGame(){


		StartCoroutine (Reset ()); 
		continueTimer = false;

	}

	public IEnumerator Reset(){

		manager.Reset ();

		// Plays the menu woosh sound effect
		if (!menuWooshAudioSource.isPlaying) {

			menuWooshAudioSource.PlayOneShot(menuWooshSfx,1f); 
		}
		// Stop the score scene music
		if (scoreMusicAudioSource.isPlaying) {

			scoreMusicAudioSource.Stop (); // Play the break  music
		}
		// Plays the start music
		if (!startMusicAudioSource.isPlaying) {

			startMusicAudioSource.Play (); // Play the break  music

		}

		gameOverPanel.gameObject.GetComponent<FadeTexture> ().FadeOut ();

		gameOverPanel.gameObject.GetComponent<Animator> ().SetTrigger ("PanelOff");

		uIControl.GameOverPanelUp ();

		uIControl.SettingButtonHide (); // Hide the Settings menu pivot object with an amimation.
		uIControl.SettingButtonClose (); // close al the settings menu buttons with animation.



		newBestEffects.gameObject.SetActive (false);


		yield return new WaitForSeconds (0.5f);

		gameOverPanel.gameObject.SetActive (false); // disable the game over panel to avoid bad performance.

	}

	public void GameOver(){ // this function is called when the player collides with an enemy and we want to stop the game.

		StartCoroutine (StopGame ()); 
		deadAudioSource.PlayOneShot (deadSfx, 1f); // play the dead sound.
		pauseButton.GetComponent<Image> ().raycastTarget = false; // Disable the pause button to avoid touches when game is over.
		pauseButton.GetComponent<FadeTexture> ().FadeOut(); // make the pause button invisible calling its fade script.

	}

	public IEnumerator StopGame(){

		gameOverPanel.gameObject.SetActive (true); // first enable the game over panel to show animations.

		shake.ShakeCamera (0f, 0f);

		distanceCount.meterIncresing = false; // Stops the meter distance counter


		yield return new WaitForSeconds (1.5f); // we use this to make the next function to wait before start.

		uIControl.PanelUp ();

		uIControl.SettingButtonShow ();

		manager.inGameOverScene = true;


		yield return new WaitForSeconds (0.5f);


		if (!scoreMusicAudioSource.isPlaying) {

			scoreMusicAudioSource.Play (); // Play the break  music

		}

		if (!menuWooshAudioSource.isPlaying) {

			menuWooshAudioSource.PlayOneShot(menuWooshSfx,1f); 

		}

		gameOverPanel.gameObject.GetComponent<Animator> ().SetTrigger ("PanelOn");


		gameOverPanel.gameObject.GetComponent<FadeTexture> ().FadeIn ();


		uIControl.GameOverPanelDown ();


		continueButton.gameObject.SetActive (true);


		continueTimer = true;

		timeLeft = 10;


		if (newBest) {

			newBestEffects.gameObject.SetActive (true);
			newBestAudioSource.PlayOneShot (newBestSfx, 1f);
			newBest = false;
		}
			

		pauseButton.GetComponent<Image> ().raycastTarget = true; // enable the pause button raycast touch when is outside the screen



		yield return new WaitForSeconds (fireworksDuration);
	

		newBestEffects.gameObject.SetActive (false);


		yield return null;

	}

	public void AnimateVideoAndBuyButtons (){ // We use this function to call the vide ads and shop button animators and make them to trigger its animation.


		if (!goldenEgg.canContinue) {

			uIControl.AdsButtonOn ();	
			uIControl.ShopButtonOn ();	

		}
	}



}