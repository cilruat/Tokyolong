using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// 3D ENDLESS RUNNER SYSTEM By BITBOYS STUDIO.
public class PauseMenu : MonoBehaviour {

	[Header("Pause Menu Manager and Countdown Manager")]

	public bool isPaused; // Used to store if we have paused the game or not.
	public GameObject pauseMenuPanel; // The pause menu panel object.
	public GameObject quitPopUpPanel; // The Quit game pop up panel that appears when the plyer wants to close the game.
	private CharController player; // Call the character controller script.
	private CamShake shake; // Call the camera shake effect script.
	private LevelManager manager; // Call the level manager script.
	private UIController uIControl; // Usted to connect with the UI controller script.
	public GameObject number3; // The countdown number 3 image.
	public GameObject number2;// The countdown number 2 image
	public GameObject number1;// The countdown number 1 image
	public GameObject go;// The countdown GO! image
	public float secondsBettweenNumbers = 0.5f; // The time that we want to wait before show the next number in the countdown function.
	public GameObject pauseButton; // The pause button UI object.

	[Header("Sound Stuff *(Volume Controlled from the Mixer)")]
	public AudioSource countdownAudioSource; // The audio source that controls the countdown sound
	public AudioClip countdownSfx; // The countdown audio clip.


	void Awake () {
		
		// We need to set a reference to the variables that we have write to call some scripts that we need to use in this script.
		player = FindObjectOfType<CharController> (); 
		shake = FindObjectOfType<CamShake> ();	
		manager = FindObjectOfType<LevelManager> ();
		uIControl = FindObjectOfType<UIController> ();

	}

	void Update(){


		// Use this to set the sfx on and off with the setting buttons
	
		if (!LevelManager.sfxActive) {

			countdownAudioSource.volume = 0;
		} else {

			countdownAudioSource.volume = 1;
		}
	}

	public void QuitPopUp(){ // This function is called from the quit button in the pause menu.

		StartCoroutine(QuitGamePopUp()); 

	}

	public IEnumerator QuitGamePopUp()
	{
		quitPopUpPanel.gameObject.SetActive (true); // First set the panel active.

		yield return new WaitForSecondsRealtime(0.1f); 

		uIControl.ExitGamePanelActivate (); // Call the script responsible of trigger the menu panels animations.
		pauseMenuPanel.gameObject.GetComponent<Animator> ().SetTrigger ("PanelOff"); // Trigger the pause menu animation that close the panel.
		pauseMenuPanel.gameObject.GetComponent<FadeTexture> ().FadeOut (); // make a little fade to the pause menu panel while the animation is triggered.
		uIControl.PanelDown ();// Call the in game up panel to trigger the animation that makes them appear in the scene, with this, we make the player see their game progress.
		Time.timeScale = 0f;// Set the time scale to 0, this pauses the game, and makes that the objects stops moving.

		yield return null;


	}
	public void QuitPopUpClose(){ // Used for close the quit pop up, called from the (X) button

		StartCoroutine(QuitGamePopUpClose()); 
	}

	public IEnumerator QuitGamePopUpClose()
	{

		uIControl.ExitGamePanelDeactivate (); // Trigger the close animation.

		Pause (); // Get back the pause menu.

		yield return new WaitForSecondsRealtime(0.25f); 

		quitPopUpPanel.gameObject.SetActive (false); // Set the quit panel to false, so it will be deactivated to avoid bad performance.


		yield return null;

	}

	public void  Resume() // this function is called from the resume button in the pause menu panel, it close the pause menu and set the time scale to 1.
	{
		if (manager.gamePlaying) {
			
			isPaused = false;

			Time.timeScale = 1f;
			player.enabled = true; // Enable the player again.


		}
	}
	public void Pause(){ // This function is called from the pause button located in the in game up panel.

		StartCoroutine(PauseGame()); 

}
	public IEnumerator PauseGame() // this routine enables the pause menu panel, stop the cam shake effect if is working in this moment and set the time scale to 0, so any object can move while its working.
	{
		pauseMenuPanel.gameObject.SetActive (true); // enable the pause menu panel object.
		shake.ShakeCamera (0f, 0f);
		Time.timeScale = 0f;
		player.enabled = false; // disable the player to avoid jump sounds or run sounds when the game is paused.

		yield return new WaitForSecondsRealtime(0.1f); // wait a little to enable call other functions.

		isPaused = true; // set the paused bool to true.

		pauseMenuPanel.gameObject.GetComponent<Animator> ().SetTrigger ("PanelOn"); // trigger the pause panel appear animationn.
		pauseMenuPanel.gameObject.GetComponent<FadeTexture> ().FadeIn (); // makes a little fade in effect while the appear animation is triggered.
		uIControl.PanelUp (); // Hide the in game up panel, calling the ui controller script.

	}

	public void  Unpause() // This function is called to close the pause menu. It is triggered by the resume button.
	{
		StartCoroutine(UnpauseGame()); // Starts the routine. See further down.

	}
		

	public IEnumerator UnpauseGame()
	{

		pauseMenuPanel.gameObject.GetComponent<Animator> ().SetTrigger ("PanelOff"); // First we use the pause menu panel Animator component to perform the "dissapear" animation.
		pauseMenuPanel.gameObject.GetComponent<FadeTexture> ().FadeOut (); // Also we make a little fade out effect calling the fadetexture script attached to the object.
		uIControl.PanelDown ();// we call the ui control script to trigger the animation that makes the in game up panel go down.

		countdownAudioSource.PlayOneShot (countdownSfx, 1f); // Play the first countdown sound

		number3.gameObject.SetActive (true); // Activates the number 3 image.

		yield return new WaitForSecondsRealtime(secondsBettweenNumbers);  // wait until we want to set active the next number.

		number3.gameObject.SetActive (false); //disable the number 3 image
		number2.gameObject.SetActive (true); // and now we activate the number 2 image.

		countdownAudioSource.PlayOneShot (countdownSfx, 1f); // Also we call the countdown sound audio source to play the countdown sound again while we activate the number 2 image.


		yield return new WaitForSecondsRealtime(secondsBettweenNumbers);// wait until we want to set active the next number.

		number2.gameObject.SetActive (false);//disable the number 2 image
		number1.gameObject.SetActive (true);// and now we activate the number 1 image.
		countdownAudioSource.PlayOneShot (countdownSfx, 1f);// Call the countdown sound audio source to play the countdown sound again while we activate the number 1 image.

	
		yield return new WaitForSecondsRealtime(secondsBettweenNumbers);// wait until we want to set active the next image.

		number1.gameObject.SetActive (false); // disable the number 1 image.

		countdownAudioSource.PlayOneShot (countdownSfx, 1f); // play the sound again.


		go.gameObject.SetActive (true); // and show the "GO" image.

		Resume ();// At this moment we call the resume function, this makes the game to be unpaused.

		pauseMenuPanel.gameObject.SetActive (false); // Disables the pause menu object to avoid bad performance.

		yield return new WaitForSecondsRealtime(secondsBettweenNumbers); // wait until we want to set active the next image.


		go.gameObject.GetComponent<FadeTexture> ().FadeOut (); // Call the fade texture script attached to the go image and perform a little fade effect.

		yield return new WaitForSecondsRealtime(1f); // Wait a second before disable the "GO" image and also we set back the image alpha to 1 calling the fade in function of the fadetexture script already attached to the go image.

		go.gameObject.SetActive (false);

		go.gameObject.GetComponent<FadeTexture> ().FadeIn ();


		yield return null;


	}

	public void StartCountdown(){ // This function is called from the play button, this shows the countdown before player starts running in the game.

		StartCoroutine(Countdown()); 
		uIControl.SettingButtonHide (); // Hide the Settings menu pivot object with an amimation.
		uIControl.SettingButtonClose (); // close al the settings menu buttons with animation.
	}

	public IEnumerator Countdown()
	{
		if (manager.startMusicAudioSource.isPlaying) {

			manager.startMusicAudioSource.Stop (); // Stop the break music.

		}
		countdownAudioSource.PlayOneShot (countdownSfx, 1f);// Play the first countdown sound


		number3.gameObject.SetActive (true);// Activates the number 3 image.

		yield return new WaitForSecondsRealtime(secondsBettweenNumbers); // wait until we want to set active the next number.


		number3.gameObject.SetActive (false);//disable the number 3 image
		number2.gameObject.SetActive (true);// and now we activate the number 2 image.

		countdownAudioSource.PlayOneShot (countdownSfx, 1f);// Also we call the countdown sound audio source to play the countdown sound again while we activate the number 2 image.


		yield return new WaitForSecondsRealtime(secondsBettweenNumbers);// wait until we want to set active the next number.


		number2.gameObject.SetActive (false);//disable the number 2 image
		number1.gameObject.SetActive (true);;// and now we activate the number 1 image.
		countdownAudioSource.PlayOneShot (countdownSfx, 1f);// Call the countdown sound audio source to play the countdown sound again while we activate the number 1 image.


		yield return new WaitForSecondsRealtime(secondsBettweenNumbers);// wait until we want to set active the next image.

		number1.gameObject.SetActive (false);// disable the number 1 image.

		countdownAudioSource.PlayOneShot (countdownSfx, 1f);// play the sound again.


		go.gameObject.SetActive (true);// and show the "GO" image.

		pauseButton.GetComponent<Image> ().raycastTarget = true; // Enable the pause button when game starts.

		pauseButton.GetComponent<FadeTexture> ().FadeIn();

		manager.GameStart (); //Send message to the Level manager script to start running.

		yield return new WaitForSecondsRealtime(secondsBettweenNumbers); // Wait again


		go.gameObject.GetComponent<FadeTexture> ().FadeOut (); // Call the fade texture script attached to the go image and perform a little fade effect.

		yield return new WaitForSecondsRealtime(1f);// Wait a second before disable the "GO" image and also we set back the image alpha to 1 calling the fade in function of the fadetexture script already attached to the go image.


		go.gameObject.SetActive (false);
		go.gameObject.GetComponent<FadeTexture> ().FadeIn ();


		yield return null;


	}

}