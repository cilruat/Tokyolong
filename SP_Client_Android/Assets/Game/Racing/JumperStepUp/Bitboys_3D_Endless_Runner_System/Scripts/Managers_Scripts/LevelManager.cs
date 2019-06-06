using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace JumperStepUp
{
// 3D ENDLESS RUNNER SYSTEM By BITBOYS STUDIO.
public class LevelManager : MonoBehaviour {

	[Header("Level Manager")]

	public GameObject playButton; // The play button object.
	public GameObject introCamera; // the intro show camera.
	public GameObject followCamera; // the in game camera.
	public GameObject infoMenuPanel; // The credits panel.
	public GameObject shopMenuPanel; // The character selection back panel.
	public GameObject characterSelectPanel; // The characters panel.
	public GameObject howToPlayPanel; // The how to play panel.

	[Header("(Tower Pieces) Only use prefabs from the Towers prefabs folder")]

	public List<GameObject> towerPiecesLevel01; // The list of towers that will be instantiated when the player plays the level 1.
	public List<GameObject> towerPiecesLevel02; // The list of towers that will be instantiated when the player plays the level 2.
	public List<GameObject> towerPiecesLevel03; // The list of towers that will be instantiated when the player plays the level 3.
	public List<GameObject> towerPiecesLevel04; // The list of towers that will be instantiated when the player plays the level 4.
	public List<GameObject> towerPiecesLevel05; // The list of towers that will be instantiated when the player plays the level 5.

	private ScreenTransition transition;
	public GameObject player; // Put here the player prefab

	public Vector3 playerStartPosition; // Set here the player initial position when the game starts.
	public Vector3 playerContinuePosition01; // Here we set the player continue position based on its last position when it dies ( PlayerLastPosition)...
	public Vector3 playerContinuePosition02; // ""
	public Vector3 playerContinuePosition03; // "" 
	public Vector3 playerContinuePosition04; // "" 
	public Vector3 playerContinuePosition05; // ""

	public Vector3 playerLastPosition; // Store the player position in realtime while game is running, and we use it to store the its position before die.


	public bool resettingAll; // We use this bool function to reset the towers position.
	public bool resetTowerRotation; // We use this bool function to reset the towers rotation to its initial rotation.
	[Header("Where we are? ")]
	public bool shopping = false; // is the character selection/Shop panel opened?
	public bool inGameOverScene = false;// we are in Game over scene?
	public bool viewingCredits = false; // we are viewing the Game Credits?
	public bool viewingHowToPlay = false; // We are viewing the how to play tutorial panel?

	[Header("Set the initial Tower Pieces positions")]

	// Set the variables to change from one level to another.

	public int towersAmount; // this counts the tower amount in scene
	public int totalTowersSpawned; // this counts the total towers spawned during the game.

	public int playerInLevel = 0; // This int variable manages in wich level the player is, it changes depending the amount of towers spawned by this script.

	public Vector3 TowerPiece01Pos; // The fixed position to spawn the tower number 01
	public Vector3 TowerPiece02Pos; // The fixed position to spawn the tower number 02
	public Vector3 TowerPiece03Pos; // The fixed position to spawn the tower number 03
	public Vector3 TowerPiece04Pos; // The fixed position to spawn the tower number 04
	public Vector3 TowerPiece05Pos; // The fixed position to spawn the tower number 05
	public Vector3 TowerPiece06Pos; // The fixed position to spawn the tower number 06
	public Vector3 TowerPiece07Pos; // The fixed position to spawn the tower number 07
	public Vector3 TowerPiece08Pos; // The fixed position to spawn the tower number 08
	public Vector3 TowerPiece09Pos; // The fixed position to spawn the tower number 09
	public Vector3 TowerPiece10Pos; // The fixed position to spawn the tower number 10
	public Vector3 TowerPiece11Pos; // The fixed position to spawn the tower number 11
	public Vector3 TowerPiece12Pos; // The fixed position to spawn the tower number 12

	private UIController uIControl; // call the ui animations script.
	private PauseMenu pause; // Call the pause menu script.
	public bool gamePlaying = false; // This bool determines if the game is running or we are in other scenes.
	public float waitSecondsToPlay = 2f; // The time to wait until the gameplay starts.
	private DistanceCounter distanceCount; // Call the distance counter script.

	[Header("Sound Stuff *(Volume Controlled from the Mixer)")]
	public AudioSource mainMusicAudioSource; // here we put the main music audio source.
	public AudioSource startMusicAudioSource; // here we put the initial music (camera show) music.
	public AudioSource uIAudioSource01; // This audio source manages the "click" button sound.
	public static bool sfxActive; // Sets if the sfx is active or inactive in the settings or pause menu.
	public static bool musicActive;// Sets if the music is active or inactive in the settings or pause menu.
	public bool pitchBending; // This bool is used to stop the music when the player dies, performing a vinyl stop effect.
	const float totalBendTime = 2f; // The music pitch bending effect durtion.
	public float currentBendTime; // how much time the effect is during.
	float startPitch = 1f;// Set the music pitch to the default 1 value.


	[Header("Only For Development")]

	public bool resetPreferences = false; // Use this to reset all the playerprefs, to use only when you are working with the unity editor.





	void Awake () {

		Application.targetFrameRate = 60; // We use this to lock the fps to 60 insted of use Vsync in the quality settings.

		Screen.sleepTimeout = SleepTimeout.NeverSleep; // Avoid that the screen turns off while we are playing the game.

		player.gameObject.GetComponent<Rigidbody>().isKinematic = true; // Sets the player object to kinematic to avoid fall when the initial towers are spawning.

		//level01 = true; // Activates the level 1 tower spawning.
		StartCoroutine (InitialTowerSpawn ()); // Spawn the initial towers

		transition = FindObjectOfType<ScreenTransition> ();
		distanceCount = FindObjectOfType<DistanceCounter> ();
		uIControl = FindObjectOfType<UIController> ();
		pause = FindObjectOfType<PauseMenu> ();

		// STARTING MUSIC
		if (!startMusicAudioSource.isPlaying) {

			startMusicAudioSource.Play (); // Play the break  music

		}

		musicActive = true; // set the music On on start.
		sfxActive = true; // set the sfx On on start.

	}


	void FixedUpdate(){



		// Change levels and minimum spawned towers to jump between levels.

		if (totalTowersSpawned <= 12) { // Force to stay in level 1 if spawned towers are less than 20

			playerInLevel = 1; //Change the int variable to 1
		}	

		if (totalTowersSpawned >= 24) { // Changes from level 1 to level 2 if the spawned towers number is bigger or equal to 20

			playerInLevel = 2;//Change the int variable to 2

		}		

		if (totalTowersSpawned >= 36) {// Changes from level 2 to level 3 if the spawned towers number is bigger or equal to 40

			playerInLevel = 3;//Change the int variable to 3

		}
		if (totalTowersSpawned >= 48) {// Changes from level 3 to level 4 if the spawned towers number is bigger or equal to 40

			playerInLevel = 4;//Change the int variable to 4

		}
		if (totalTowersSpawned >= 60) {// Changes from level 4 to level 5 if the spawned towers number is bigger or equal to 40

			playerInLevel = 5;//Change the int variable to 5

		}
	}


	void Update(){

		playerLastPosition = player.gameObject.transform.localPosition; // We store the player local position during the gameplay to know its last position before die.

		// if we are playing the game, or if the "character selection" pop up menu , or the "credits" pop up menu or the "Pause" pop up menu are open, 
		// or if we are in the game over scene, we set inactive the PLAY button.
		if (gamePlaying || shopping || pause.isPaused || inGameOverScene) { 

			playButton.gameObject.SetActive (false);

			// Get back the play button.
		}	if (!gamePlaying || !shopping || !pause.isPaused || !inGameOverScene) {

			playButton.gameObject.SetActive (true);


		}
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Use this to can set the music on and off with the setting buttons
		if (!musicActive || musicActive && pause.isPaused) {

			mainMusicAudioSource.volume = 0;
			startMusicAudioSource.volume = 0;


		} else {

			mainMusicAudioSource.volume = 1;
			startMusicAudioSource.volume = 1;


		}
		if (!sfxActive) {

			uIAudioSource01.volume = 0;


		} else {

			uIAudioSource01.volume = 1;

		}

		////////////////////////////////////////////////////////////////////PITCH BEND MUSIC EFFECT//////////////////
		// next we will make the music bends like a vinyl.
		if (gamePlaying) {// When we start playing the music starts playing

			if (!mainMusicAudioSource.isPlaying) {

				mainMusicAudioSource.Play (); // Play the main music

				mainMusicAudioSource.pitch = startPitch; // we ensure to back the pitch value to 1 after the bend effect.

				pitchBending = false; // Stops the bending effect.

			}
		}
		if (!gamePlaying) { // When player dies and if the music is playing we activate the pitch bending 

			if (mainMusicAudioSource.isPlaying && !pitchBending) {


					pitchBending = true;
				}	
			}
	
		if (pitchBending) // THE PITCH BEND EFFECT
		{
			currentBendTime += Time.deltaTime;
			mainMusicAudioSource.pitch = Mathf.Lerp (startPitch, 0, currentBendTime/totalBendTime);

			if(currentBendTime > totalBendTime) // When real time reaches the bend effect time it stops the bend effect.
			{
				pitchBending = false;
				currentBendTime = totalBendTime; // resets the current time float to 0.
				currentBendTime = 0f; 
			}
			if (currentBendTime == 0) {

				mainMusicAudioSource.Stop (); // stops the music before start playing it when the gameplay starts.

			}
		}

		///////////////////////////////////////////////////////////////////////////////////////

	// USE THIS ONLY FOR WHILE DEV YOUR GAME, THIS WILL RESET AL THE PLAYER PREFERENCES 

		if (resetPreferences) {

			PlayerPrefs.DeleteAll ();

			resetPreferences = false;
		}

	}

	/////////////////////////////////////////////////////////////////////////////////////// SHUFFLING TOWERS PREFABS LISTS /////////////////////////////////////////////////////////////////////////////////////// 

	 public void Shuffle (){  // Suffle the order of an specified list. In this case the towers objects lists.


		for (var i = 0; i < towerPiecesLevel01.Count; i ++){

			var temp = towerPiecesLevel01[i];

			var randomIndex = Random.Range(0, towerPiecesLevel01.Count);

			towerPiecesLevel01[i] = towerPiecesLevel01 [randomIndex];

			towerPiecesLevel01 [randomIndex] = temp;
		}
 	}

	/// /////////////////////////////////////////////////////////////////////////////////////////////////SPAWNING TOWERS////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public void SpawnTower(){ // Select what towers to spawn depending the level we are playing.

		if (playerInLevel == 1) {
			StartCoroutine (SpawnLevel01 ());
		}
		if (playerInLevel == 2) {
			StartCoroutine (SpawnLevel02 ());
		}
		if (playerInLevel == 3) {
			StartCoroutine (SpawnLevel03 ());
		}
		if (playerInLevel == 4) {
			StartCoroutine (SpawnLevel04 ());
		}
		if (playerInLevel == 5) {
			StartCoroutine (SpawnLevel05 ());
		}
	}

	public IEnumerator SpawnLevel01(){ // level 1 towers spawn

		GameObject tower = towerPiecesLevel01 [Random.Range (0, towerPiecesLevel01.Count)]; // First we select a random tower from the list.

		Instantiate(tower, this.transform.position,  this.transform.rotation); // We instantiate this tower at the level manager position because the game object that holds this script its located at the exact position where we want to spawn the towers prefabs.

		towersAmount = towersAmount + 1; // We add the spawned tower to the amount of towers instantiated, with this we will know if we have the necessary amount of towers to change from one level to another.

		yield return new WaitForSeconds(0.5f); // we wait mid second after made a shuffle to the towers list.

		Shuffle (); // Call the shuffle function.

			yield return null; // End the coroutine.

	}

	public IEnumerator SpawnLevel02(){ // level 2 towers spawn


		GameObject tower = towerPiecesLevel02 [Random.Range (0, towerPiecesLevel02.Count)];// First we select a random tower from the list.

		Instantiate(tower, this.transform.position,  this.transform.rotation);// We instantiate this tower at the level manager position because the game object that holds this script its located at the exact position where we want to spawn the towers prefabs.

		towersAmount = towersAmount + 1;// We add the spawned tower to the amount of towers instantiated, with this we will know if we have the necessary amount of towers to change from one level to another.

		yield return new WaitForSeconds(0.5f);// we wait mid second after made a shuffle to the towers list.

		Shuffle ();// Call the shuffle function.

		yield return null;// End the coroutine.

	}
	public IEnumerator SpawnLevel03(){ // Level 3 towers spawn


		GameObject tower = towerPiecesLevel03 [Random.Range (0, towerPiecesLevel03.Count)];// First we select a random tower from the list.

		Instantiate(tower, this.transform.position,  this.transform.rotation);// We instantiate this tower at the level manager position because the game object that holds this script its located at the exact position where we want to spawn the towers prefabs.

		towersAmount = towersAmount + 1;// We add the spawned tower to the amount of towers instantiated, with this we will know if we have the necessary amount of towers to change from one level to another.

		yield return new WaitForSeconds(0.5f);// we wait mid second after made a shuffle to the towers list.

		Shuffle ();// Call the shuffle function.

		yield return null;// End the coroutine.

	}
	public IEnumerator SpawnLevel04(){ // Level 4 towers spawn


		GameObject tower = towerPiecesLevel03 [Random.Range (0, towerPiecesLevel04.Count)];// First we select a random tower from the list.

		Instantiate(tower, this.transform.position,  this.transform.rotation);// We instantiate this tower at the level manager position because the game object that holds this script its located at the exact position where we want to spawn the towers prefabs.

		towersAmount = towersAmount + 1;// We add the spawned tower to the amount of towers instantiated, with this we will know if we have the necessary amount of towers to change from one level to another.

		yield return new WaitForSeconds(0.5f);// we wait mid second after made a shuffle to the towers list.

		Shuffle ();// Call the shuffle function.

		yield return null;// End the coroutine.

	}
	public IEnumerator SpawnLevel05(){ // Level 5 towers spawn


		GameObject tower = towerPiecesLevel03 [Random.Range (0, towerPiecesLevel05.Count)];// First we select a random tower from the list.

		Instantiate(tower, this.transform.position,  this.transform.rotation);// We instantiate this tower at the level manager position because the game object that holds this script its located at the exact position where we want to spawn the towers prefabs.

		towersAmount = towersAmount + 1;// We add the spawned tower to the amount of towers instantiated, with this we will know if we have the necessary amount of towers to change from one level to another.

		yield return new WaitForSeconds(0.5f);// we wait mid second after made a shuffle to the towers list.

		Shuffle ();// Call the shuffle function.

		yield return null;// End the coroutine.

	}
	/////////////////////////////////////////////////////////////////////////////////////////////RESET AND CONTINUE THE GAME////////////////////////////////////////////////////////////////////////////////////////	

	public void Reset(){ // This function is called from the reset button on the game over scene. Restarts the scene to the its initial state but making a shuffle to the towers list to avoid that player always play the same scene start.

		StartCoroutine(ResetGame()); 

		System.GC.Collect (); // Call garbage collection to clean the garbage generated by the instantiated and destroyed towers.
	}

	public void Continue(){

		StartCoroutine(ContinueGame());  // this function is called from the continue button on the game over scene, but instead of destroy all the towers an respawn all again, call the tower movement script and resets the towers rotation to its initial rotation without change the tower position.

		System.GC.Collect ();// // Call garbage collection to clean the garbage generated by the instantiated and destroyed towers.

	}

	public IEnumerator ContinueGame() // This coroutine store all the steps that the game makes when the player press the continue button on the game over scene/zone.
	{

		transition.TransitionsShuffle (); // First we make shuffle to the transition images that we have pre made, they are located on the hierarchy, under the UICanvas object.
	
		yield return new WaitForSeconds(0.5f); // wait for mid second before call the transition (fill the image from 0 to 1)

		transition.GoFade (); // fill the transition image script, we make it fill from o to 1.

		yield return new WaitForSeconds(0.75f); // Wait again, this time a little from to leave the transition to end and after wait we call the back image fade transition.

		transition.FadeinBlack (); // fade the back transition image.

		// Now we make a very fast call to the tower prefabs, we reset its rotation and after mid second we call again the bool to set it to false.
		yield return new WaitForSeconds(0.5f);
		resetTowerRotation = true;
		yield return new WaitForSeconds(0.5f);
		resetTowerRotation = false;
		yield return new WaitForSeconds(0.5f);

		player.gameObject.SetActive (true); // Now that the towers are rotated to its initial rotation we activate the player (the player was deactivated when dies).

		// Select the continue position based on the dead position

		//  CONTINUE POSITION 01

		if (playerLastPosition.y <= playerStartPosition.y) { // if the player last position before die is less than the original start position, we set the restart position the initial position, thus preventing the player from appearing outside the playing area.

			player.gameObject.transform.localPosition = playerStartPosition; 
		} 
		//  CONTINUE POSITION 01

		if (playerLastPosition.y >= playerContinuePosition01.y) { // if the player last position before die is bigger than the continue position that we have writed in the script (from the editor) we set the restart position to the player continue position01.

			player.gameObject.transform.localPosition = playerContinuePosition02;
		} 
		//  CONTINUE POSITION 02
		if (playerLastPosition.y >= playerContinuePosition02.y) {

			player.gameObject.transform.localPosition = playerContinuePosition03;

		}
		//  CONTINUE POSITION 03
		if (playerLastPosition.y >= playerContinuePosition03.y) {

			player.gameObject.transform.localPosition = playerContinuePosition04;

		}
		if (playerLastPosition.y >= playerContinuePosition04.y) {

			player.gameObject.transform.localPosition = playerContinuePosition05;

		}
	
		yield return new WaitForSeconds(0.5f);


		player.gameObject.GetComponent<Rigidbody>().isKinematic = false; // After reseting the player position we set the iskinematic function to false, this way we will be able to handle it again when the game starts.


		yield return new WaitForSeconds(0.5f);


		transition.FadeOutBlack (); // Call the back transition image and make it transparent.


		yield return new WaitForSeconds(0.5f);


		inGameOverScene = false; // We say to the script that we are no longer in the game over scene and that we go on to the main scene


		transition.BackFade (); // Call the frontal transition to change the image fill from 1 to 0.


		playButton.gameObject.SetActive (true); // Activates the play button object.


		yield return new WaitForSeconds(0.5f);


		uIControl.PanelDown (); // Call the UI controller script and activate the in game up panel animation to go down.


		uIControl.SettingButtonShow (); // Call the UI controller script and activate the settings button.


		uIControl.PlayButtonOn (); // Call the UI controller script to trigger the play button animation.


		playButton.GetComponent<Image> ().raycastTarget = true; // enable the touch raycast on the play button. (this makes that we will interact with the button).


		yield return null; // end the routine.


	}



	public IEnumerator ResetGame()// This coroutine store all the steps that the game makes when the player press the reset button on the game over scene/zone.
	{

		transition.TransitionsShuffle ();// First we make shuffle to the transition images that we have pre made, they are located on the hierarchy, under the UICanvas object.

		yield return new WaitForSeconds(0.5f);// wait for mid second before call the transition (fill the image from 0 to 1)

		transition.GoFade ();// fill the transition image script, we make it fill from o to 1.

		yield return new WaitForSeconds(0.75f);// Wait again, this time a little from to leave the transition to end and after wait we call the back image fade transition.

		transition.FadeinBlack ();// fade the back transition image.

		yield return new WaitForSeconds(0.5f);

		totalTowersSpawned = 0; // Reset the total towers respawned to 0, with this we loose all the progress that we have made in the last play.

		playerInLevel = 1; // Get back the game level to the first level.

		distanceCount.distance = 0; // Reset the distance counter to 0.

		// Insted of rotate the towers to its inital rotation, we destroy all the scene towers and after this we make a respawn of all the level 1 towers.
		yield return new WaitForSeconds(0.5f);
		resettingAll = true;
        yield return new WaitForSeconds(0.5f);
		resettingAll = false;
		yield return new WaitForSeconds(0.5f);

		RespawnTowers (); // Respawn the towers.

		yield return new WaitForSeconds(0.5f);

		player.transform.localPosition = playerStartPosition; // Directly we put the player on its normal start position.

		player.gameObject.SetActive (true); // we enable the player object.

		yield return new WaitForSeconds(0.5f);

		player.gameObject.GetComponent<Rigidbody>().isKinematic = false; // After reseting the player position we set the iskinematic function to false, this way we will be able to handle it again when the game starts.

		yield return new WaitForSeconds(0.5f);

		transition.FadeOutBlack (); // Call the back transition image and make it transparent.

		yield return new WaitForSeconds(0.5f);

		inGameOverScene = false; // We say to the script that we are no longer in the game over scene and that we go on to the main scene

		transition.BackFade (); // Call the frontal transition to change the image fill from 1 to 0.

		playButton.gameObject.SetActive (true);  // Activates the play button object.

		yield return new WaitForSeconds(0.5f);

		uIControl.PanelDown ();  // Call the UI controller script and activate the in game up panel animation to go down.

		uIControl.SettingButtonShow (); // Call the UI controller script and activate the settings button.

		uIControl.PlayButtonOn (); // Call the UI controller script to trigger the play button animation.

		playButton.GetComponent<Image> ().raycastTarget = true;  // enable the touch raycast on the play button. (this makes that we will interact with the button).


		yield return null; // end the routine.


	}

	/// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	// // // // // // // // // // // // // // // // // NOT USED BUT USEFUL // // // // // // // // // // // // // // // // // // // // // // // // // //

	// The next function finally it has not been used in the game, but it can be very useful to call it whenever you spawn a tower and want to erase it from the tower object list so that it is not done not used again.
	void SubstractFromList (){ // This call substract the last used tower piece from the objects list.

		int lastObject;
		lastObject = towerPiecesLevel01.Count - 1;
		removerArray(lastObject);
	}

	// // // // // // // // // // // // // // // // // 

	void removerArray (int position) { // This function removes the last tower instantiated regardless of its position.

		towerPiecesLevel01.RemoveAt (position);

	}
	// // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // //

	void RespawnTowers (){ // ReSpawn the start game tower pieces when player dies. Also we use this to spawn the initial towers when the game starts.

		Shuffle (); // Make shuffle to the towers list.

		GameObject tower01 = towerPiecesLevel01 [0];  // Get the first tower of the list.
			
		Instantiate(tower01, TowerPiece01Pos,  this.transform.rotation); // Instantiates the tower at the first tower position (we set this position in the script but from the editor) WE RECOMMEND TO DO NOT TOUCH THIS.

		GameObject tower02 = towerPiecesLevel01 [1]; 

		Instantiate(tower02, TowerPiece02Pos,  this.transform.rotation);

		GameObject tower03 = towerPiecesLevel01 [2]; 

		Instantiate(tower03, TowerPiece03Pos,  this.transform.rotation);

		GameObject tower04 = towerPiecesLevel01 [3]; 

		Instantiate(tower04, TowerPiece04Pos,  this.transform.rotation);

		GameObject tower05 = towerPiecesLevel01 [4]; 

		Instantiate(tower05, TowerPiece05Pos,  this.transform.rotation);

		GameObject tower06 = towerPiecesLevel01 [5]; 

		Instantiate(tower06, TowerPiece06Pos,  this.transform.rotation);

		GameObject tower07 = towerPiecesLevel01 [6]; 

		Instantiate(tower07, TowerPiece07Pos,  this.transform.rotation);

		GameObject tower08 = towerPiecesLevel01 [7]; 

		Instantiate(tower08, TowerPiece08Pos,  this.transform.rotation);

		GameObject tower09 = towerPiecesLevel01 [8]; 

		Instantiate(tower09, TowerPiece09Pos,  this.transform.rotation);

		GameObject tower10 = towerPiecesLevel01 [9]; 

		Instantiate(tower10, TowerPiece10Pos,  this.transform.rotation);

		GameObject tower11 = towerPiecesLevel01 [10]; 

		Instantiate(tower11, TowerPiece11Pos,  this.transform.rotation);

		GameObject tower12 = towerPiecesLevel01 [11]; 

		Instantiate(tower12, TowerPiece12Pos,  this.transform.rotation);

		towersAmount = towersAmount + 12; // As we have just launched 12 towers, we add these towers to the total number of towers we have launched.

		totalTowersSpawned = 12; // Set the total towers spawned to count if got to change from one level to another.

	}


	public void GameReady (){ // this function is called from the LOGO IMAGE, as a button touch. It makes that the intro show camera movement stops and activate the main camera.

		introCamera.gameObject.SetActive (false);
		followCamera.gameObject.SetActive (true);
	}

	public void GameStart (){ // this funcion is called by the pause menu script, it starts when the the countdown was finished.

		playButton.GetComponent<Image> ().raycastTarget = false; // disables the touch raycast to avoid touches.
		player.gameObject.GetComponent<Rigidbody>().isKinematic = false;// disables the kinematic body of the player to can start running.
		gamePlaying = true; // Set the gameplaying bool to true so it indicates that we are playing the game.
		distanceCount.meterIncresing = true; // we call the distance counter script and tell that the meter counter should start working.

	}

	/// /////////////////////////////////////MUSIC AND SFX SWITCH (ON - OFF) //////////////////////////////////////////////

	public void MusicSwitch(){ // Call this function from the music button in the settings menu or in the pause menu to make the music be active or inactive.

		musicActive = !musicActive;

	}

	public void SfxSwitch (){ // Call this function from the sfx button in the settings menu or in the pause menu to make thesfx be active or inactive.

		sfxActive = !sfxActive;

	}
		
	////////////////////////////////////// MENU PANELS, SET ACTIVE, OPEN - CLOSE, AND ANIMATION TRIGGERS //////////////////////////////////////////////////////////

	public void ShopOpen(){ // This function is called from the settings menu, the character select button open the SHOP/CHARACTER SELECTION menu.


		StartCoroutine(OpenShop());  // Starts the routine. See further down.

	}

	public void ShopClosed(){ // This function is called from the SHOP/CHARACTER SELECTION menu close button (X).


		StartCoroutine(CloseShop()); // Starts the routine. See further down.

	}
	public void CreditsOpen(){ // This function is called from the settings menu, the credis button (i) open the credits panel.


		StartCoroutine(OpenCredits()); // Starts the routine. See further down.

	}

	public void CreditsClosed(){ // This function is called from the CREDITS PANEL menu close button (X).


		StartCoroutine(CloseCredits()); // Starts the routine. See further down.

	}
	public void HowToPlayOpen(){// This function is called from the settings menu, the how to play button (?) open the how to play panel.


		StartCoroutine(OpenHowToPlay()); // Starts the routine. See further down.

	}

	public void HowToPlayClosed(){ // This function is called from the HOW TO PLAY PANEL menu, is triggered when the player touches the how to play panel and closes the menu.


		StartCoroutine(CloseHowToPlay()); // Starts the routine. See further down.

	}

	public IEnumerator OpenHowToPlay()
	{
		howToPlayPanel.gameObject.SetActive (true); // enable the panel.

		yield return new WaitForSeconds(0.25f);

		viewingHowToPlay = true; // Set this bool to true to know if we are viewing the how to play panel, we use this function to hide some buttons and avoid the player to can touch them.

	}
	public IEnumerator CloseHowToPlay()
	{


		viewingHowToPlay = false; // Set this bool to false to reactivate the buttons.

		yield return new WaitForSeconds(0.25f);

		howToPlayPanel.gameObject.SetActive (false); // disable the panel to avoid bad performance (camera rendering)

	}

	public IEnumerator OpenCredits()
	{
		infoMenuPanel.gameObject.SetActive (true); // enable the panel to can trigger the appear animation.

		yield return new WaitForSeconds(0.25f);

		infoMenuPanel.gameObject.GetComponent<Animator> ().SetTrigger ("PanelOn"); // Call the panel Animator component an triggers the Animation that makes appear on the scene.

		viewingCredits = true; // Set this bool to true to know if we are viewing the how to play panel, we use this function to hide some buttons and avoid the player to can touch them.

	}
	public IEnumerator CloseCredits()
	{

		infoMenuPanel.gameObject.GetComponent<Animator> ().SetTrigger ("PanelOff"); // Call the panel animator component to trigger the dissapear animation.

		viewingCredits = false; // Set this bool to false to reactivate the buttons.

		yield return new WaitForSeconds(0.25f);

		infoMenuPanel.gameObject.SetActive (false); // disables the panel to avoid be rendered by the camera and bad performance.

	}

	public IEnumerator OpenShop()
	{
		shopMenuPanel.gameObject.SetActive (true); // enable the panel to can trigger the appear animation.

		yield return new WaitForSeconds(0.25f);

		characterSelectPanel.gameObject.SetActive (true); // enable the panel to can trigger the appear animation.

		shopMenuPanel.gameObject.GetComponent<Animator> ().SetTrigger ("PanelOn");// Call the panel Animator component an triggers the Animation that makes appear on the scene.


		shopping = true;// Set this bool to true to know if we are viewing the how to play panel, we use this function to hide some buttons and avoid the player to can touch them.

	}
	public IEnumerator CloseShop()
	{
		characterSelectPanel.gameObject.SetActive (false); // disables the panel to avoid be rendered by the camera and bad performance.

		shopMenuPanel.gameObject.GetComponent<Animator> ().SetTrigger ("PanelOff");// Call the panel animator component to trigger the dissapear animation.

		shopping = false; // Set this bool to false to reactivate the buttons.

		yield return new WaitForSeconds(0.25f);

		shopMenuPanel.gameObject.SetActive (false); // disables the panel to avoid be rendered by the camera and bad performance.

	}
	/// ///////////////////////////////////////////////////////////////////////////////////////////////////


	// THIS FUNCTION IS VERY IMPORTANT AND IS CALLED WHEN SCENE STARTS, IT PERFORMS THE INITIAL TOWER SPAWN.
	public IEnumerator InitialTowerSpawn()
	{
		RespawnTowers ();

		yield return null;

	}
	}
}


	

