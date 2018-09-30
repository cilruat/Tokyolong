using UnityEngine;
using System.Collections;
using System.Collections.Generic;
// YOU BETTER RUN By BITBOYS STUDIO.

[RequireComponent (typeof (Animator))]
[RequireComponent (typeof (Rigidbody))]

public class CharController : MonoBehaviour { // Please be carefull editing this script to avoid bad behaviors.

	[Header("Character Control")]

	[Header("Jump and Grounded Settings")]

	public float rayGroundedLength = 1.1f; // Set here the distance between the raycast ray and the ground.
	public bool isGrounded = false; // The bool function that indicates if the player its touching the ground.
	public float jumpHeight = 5.1f; // The force amount applied to the jump function.
	public float doubleJumpHeight = 3f;// The force amount applied to the double jump function.
	public float gravity = 20f; // the amount of gravity applied to the player body

	[Header("Used to know the player states")]

	public bool isJumping= false; // the bool used to know if the player its performing a jump.
	public bool isDoubleJumping= false; // the bool used to know if the player its performing a double jump.
	public bool falling = false; // the bool variable used to know if the player its falling down.
	public bool goingUp = false; // the variable used to know if the player its going down.
	public bool isRunning = false; // the bool variable used to know if the player its running or not.
	public bool loosingCoins = false; // Used to know if the player is loosing coins after collide with a bad coin object.


	// Call some scripts to connect with the character controller script.
	private LevelManager manager; // use this variable to call the level manager script.
	private CamShake shake; // Call the shake camera effect script.
	private UiCoinSpawner uiCoin;
	private GameOverMenu endGameMenu;


	private Rigidbody rb; // The character Rigidbody component.
	private Animator anim; // The character Animator component

	[Header("The Characters materials")]

	public Material []normalMaterial;
	public Material [] damagedMaterial;
	public GameObject crazyCharacterHat;
	public GameObject alienCharacterAntenna;
	public GameObject devilCharacterHorns;
	public GameObject robotCharacterAntenna;
	public GameObject mariachiCharacterHat;
	public GameObject swagCharacterHat;
	public GameObject clownAccessories;
	public GameObject swimCharacterFloater;


	[Header("Character model to change the material at runtime")]

	public GameObject characterModel; // The player object.

	[Header("Die Effects (Particles)")]

	public GameObject dieParticles; // The particles that we want to spawn when character dies.

	[Header("Run Effects (Particles)")]

	public GameObject runParticles; // The particles used when the character is running (dust)

	[Header("Sound Stuff *(Volume Controlled from the Mixer)")]
	public AudioSource coinAudioSource;
	public AudioClip coinSfx;
	public AudioSource bigCoinAudioSource;
	public AudioClip bigCoinSfx;
	public AudioSource looseCoinAudioSource;
	public AudioClip looseCoinSfx;
	public AudioSource jumpAudioSource;
	public AudioClip jumpSfx;
	public AudioSource runAudioSource;
	public AudioClip runSfx;


	void Start(){

		rb = GetComponent<Rigidbody> ();
		anim = this.GetComponent<Animator> ();
		shake = FindObjectOfType<CamShake> ();
		manager = FindObjectOfType<LevelManager> ();
		uiCoin = FindObjectOfType<UiCoinSpawner> ();
		endGameMenu = FindObjectOfType<GameOverMenu> ();
	}
	
	void FixedUpdate(){

		
	checkGrounded (); // We ensure to check if the player is grounded at any time.


	GetComponent<Rigidbody>().AddForce(new Vector3 (0, -gravity * GetComponent<Rigidbody>().mass, 0)); // Aplly force down to the Rigidbody component, to set a more fast jump movement.

	}


	void Update(){

		//Falling
		if (rb.velocity.y < -0.1) { // if the Rigidbody component velocity is less than -0.1 we call the falling bool and set it to true to know that the character is falling down.
			falling = true;

		} 

		if (rb.velocity.y > -0.1) { // if the Rigidbody component velocity is bigger than -0.1 we call the falling bool and set it to false to know that the character is not falling down.
			falling = false;

		}

		// Going Up
		if (rb.velocity.y > 0.1) {// if the Rigidbody component velocity is bigger than 0.1 we call the going up bool and set it to true to know that the character is going up.
			goingUp = true;
		} 

		if (rb.velocity.y < 0.1) {// if the Rigidbody component velocity is less than 0.1 we call the going up bool and set it to false to know that the character is not going up.

			goingUp = false;

		}

		////////////////////////////////////////PLAYER STATES //////////////////////////////////////////////////////////////////////////////////////////////////////////////
	 
		// Player movement check

		if(manager.gamePlaying){ // If we are playing the game...

			if (isGrounded) { // If player is touching the ground.

				isJumping = false; // We are not jumping
				isDoubleJumping = false; // We are not air jumping
				falling = false; // We are not falling

			} 

			if (falling && !isJumping && !isDoubleJumping) { // If player is falling and its not jumping or air jumping we trigger the falling animation.

				anim.SetTrigger ("Falling");

			}

			if (isGrounded && !isJumping && !isDoubleJumping && !falling) { // if character it's touching the ground and it's not jumping and not air jumping and not falling ...it is Running!

				anim.SetBool ("Running", true); // Trigger the running animation only if its touching the ground.
				isRunning = true; // set the running bool to true
				runParticles.gameObject.SetActive (true); // Activates the running particles.

				if (!runAudioSource.isPlaying) {
					runAudioSource.PlayOneShot (runSfx,1); // If the running audio source is not working we call it to play the running sound.
				}
			}

			if (!isGrounded || isJumping || isDoubleJumping || falling) {  // If the player is not grounded and its jumping or air jumping or falling we stop the running functions.


				anim.SetBool ("Running", false); // Call the character animation component and set the running animation to false.
				isRunning = false; // Set the running bool to false.
				runParticles.gameObject.SetActive (false); // disable the running particles object.
				runAudioSource.Stop (); // stop playing the running sound.


			}
			///////////////////////////////////////////////////////////////////MOBILE TOUCH CONTROLLER ///////////////////////////////////////
			 
			//CHARACTER TOUCH CONTROL (EDITOR AND MOBILE DEVICES)

			if (isGrounded && Input.GetMouseButtonDown (0)) { // Only if the player is touching the ground we can call the Jump void function and perform a Jump when touch the screen.

				Jump (); // Call the Jump function

				isJumping = true; // Set the jumping bool to true to know that the character is jumping.

				anim.SetTrigger ("Jump"); // Call the character animator component to trigger the jump animation.

			}

			if (falling && !isDoubleJumping && Input.GetMouseButtonDown (0)) { // If the character is falling and we touch the screen we can perform an Air Jump.

				DoubleJump (); // Call the Air jump function

				isDoubleJumping = true; // Set the double jump bool state to true.

				anim.SetTrigger ("DoubleJump"); // Call the character animator component to trigger the air jump animation.

			}
			////////////////////////////////////////////////////////////////EDITOR CONTROLLER ///////////////////////////SAME AS ABOVE BUT IN THIS CASE USING BUTTONS OR KEYS WHILE PLAY IN THE UNITY EDITOR.//////////////
 
			//CHARACTER TOUCH CONTROL (EDITOR ONLY)

			#if UNITY_EDITOR

			if (isGrounded && Input.GetButtonDown("Jump")) { // Only if the player is touching the ground we can call the Jump void function and perform a Jump when press the Jump button or the Space key.


				Jump (); // Call the Jump function

				isJumping = true;// Set the jumping bool to true to know that the character is jumping.

				anim.SetTrigger ("Jump");// Call the character animator component to trigger the jump animation.

			}

			if (falling && !isDoubleJumping && Input.GetButtonDown("Jump")) { // If the character is falling and we press the jump button we can perform an Air Jump.


				DoubleJump ();// Call the Air jump function

				isDoubleJumping = true; // Set the double jump bool state to true.

				anim.SetTrigger ("DoubleJump"); // Call the character animator component to trigger the air jump animation.

			}

				#endif

		}
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		// Use this to can set the music on and off with the setting buttons
		if (!LevelManager.sfxActive) {

			coinAudioSource.volume = 0;
			bigCoinAudioSource.volume = 0;
			looseCoinAudioSource.volume = 0;
			jumpAudioSource.volume = 0;
			runAudioSource.volume = 0;


		} else {

			coinAudioSource.volume = 1;
			bigCoinAudioSource.volume = 1;
			looseCoinAudioSource.volume = 1;
			jumpAudioSource.volume = 1;
			runAudioSource.volume = 1;

		}


		/////////////////////////////////////// 


		if (!manager.gamePlaying) { // If the player is not running the gameplay scene we set to false all the Player States and stop the sound. This is triggerd when the player dies.

			isRunning = false;
			isJumping = false;
			isDoubleJumping = false;
			falling = false;
			goingUp = false;
			anim.SetBool ("Running", false);
			runParticles.gameObject.SetActive (false);
			runAudioSource.Stop ();

		}
		// Change materials when select each character.

		if (CharacterSelection.characterSelected == 1) { // If we have selected the character 1, we chage the character materials with the character 01 materials.

			if (!loosingCoins) { // We know if we are loosing coins when the character collides with a bad coin. So will change the main material by another with a "Damaged" Face for a small amount of time.
				characterModel.gameObject.GetComponent<Renderer> ().material = normalMaterial [0];
			} else {

				characterModel.gameObject.GetComponent<Renderer> ().material = damagedMaterial[0]; // If is not loosing coins the main material is set back to the normal material.
			}
		}

		if (CharacterSelection.characterSelected == 2) {

			if (!loosingCoins) {
				characterModel.gameObject.GetComponent<Renderer> ().material = normalMaterial [1];
			} else {

				characterModel.gameObject.GetComponent<Renderer> ().material = damagedMaterial[1];
			}

			alienCharacterAntenna.gameObject.SetActive (true); // Activate the alien character antenna when selects the alien character.
		}else{
			alienCharacterAntenna.gameObject.SetActive (false); // Set inactive the antenna object.
		}

		if (CharacterSelection.characterSelected == 3) {

			if (!loosingCoins) {
				characterModel.gameObject.GetComponent<Renderer> ().material = normalMaterial [2];
			} else {

				characterModel.gameObject.GetComponent<Renderer> ().material = damagedMaterial[2];
			}
		}
		if (CharacterSelection.characterSelected == 4) {

			if (!loosingCoins) {
				characterModel.gameObject.GetComponent<Renderer> ().material = normalMaterial [3];
			} else {

				characterModel.gameObject.GetComponent<Renderer> ().material = damagedMaterial[3];
			}

			crazyCharacterHat.gameObject.SetActive (true); // Activate the crazy character hat when selects the alien character.
		}else{
			crazyCharacterHat.gameObject.SetActive (false); // Set inactive the object.
		}


		if (CharacterSelection.characterSelected == 5) {

			if (!loosingCoins) {
				characterModel.gameObject.GetComponent<Renderer> ().material = normalMaterial [4];
			} else {

				characterModel.gameObject.GetComponent<Renderer> ().material = damagedMaterial[4];
			}
			robotCharacterAntenna.gameObject.SetActive (true); // Activate the robot character antenna.
		}else{
			robotCharacterAntenna.gameObject.SetActive (false); // Set inactive the object.
		}


		if (CharacterSelection.characterSelected == 6) {

			if (!loosingCoins) {
				characterModel.gameObject.GetComponent<Renderer> ().material = normalMaterial [5];
			} else {

				characterModel.gameObject.GetComponent<Renderer> ().material = damagedMaterial[5];
			}

			devilCharacterHorns.gameObject.SetActive (true); // Activate the devil character horns.
		}else{
			devilCharacterHorns.gameObject.SetActive (false); // Set inactive the object.
		}

		if (CharacterSelection.characterSelected == 7) {

			if (!loosingCoins) {
				characterModel.gameObject.GetComponent<Renderer> ().material = normalMaterial [6];
			} else {

				characterModel.gameObject.GetComponent<Renderer> ().material = damagedMaterial[6];
			}
		}
		if (CharacterSelection.characterSelected == 8) {

			if (!loosingCoins) {
				characterModel.gameObject.GetComponent<Renderer> ().material = normalMaterial [7];
			} else {

				characterModel.gameObject.GetComponent<Renderer> ().material = damagedMaterial[7];
			}
		}
		if (CharacterSelection.characterSelected == 9) {

			if (!loosingCoins) {
				characterModel.gameObject.GetComponent<Renderer> ().material = normalMaterial [8];
			} else {

				characterModel.gameObject.GetComponent<Renderer> ().material = damagedMaterial[8];
			}
		}
		if (CharacterSelection.characterSelected == 10) {

			if (!loosingCoins) {
				characterModel.gameObject.GetComponent<Renderer> ().material = normalMaterial [9];
			} else {

				characterModel.gameObject.GetComponent<Renderer> ().material = damagedMaterial[9];
			}

			mariachiCharacterHat.gameObject.SetActive (true); // Activate the Mariachi character hat.
		}else{
			mariachiCharacterHat.gameObject.SetActive (false); // Set inactive the object.
		}

		if (CharacterSelection.characterSelected == 11) {

			if (!loosingCoins) {
				characterModel.gameObject.GetComponent<Renderer> ().material = normalMaterial [10];
			} else {

				characterModel.gameObject.GetComponent<Renderer> ().material = damagedMaterial[10];
			}

			swagCharacterHat.gameObject.SetActive (true); // Activate the Mariachi character hat.
		}else{
			swagCharacterHat.gameObject.SetActive (false); // Set inactive the object.
		}
		if (CharacterSelection.characterSelected == 12) {

			if (!loosingCoins) {
				characterModel.gameObject.GetComponent<Renderer> ().material = normalMaterial [11];
			} else {

				characterModel.gameObject.GetComponent<Renderer> ().material = damagedMaterial[11];
			}

			clownAccessories.gameObject.SetActive (true); // Activate the Mariachi character hat.
		}else{
			clownAccessories.gameObject.SetActive (false); // Set inactive the object.
		}
		if (CharacterSelection.characterSelected == 13) {

			if (!loosingCoins) {
				characterModel.gameObject.GetComponent<Renderer> ().material = normalMaterial [12];
			} else {

				characterModel.gameObject.GetComponent<Renderer> ().material = damagedMaterial[12];
			}

			swimCharacterFloater.gameObject.SetActive (true); // Activate the Mariachi character hat.
		}else{
			swimCharacterFloater.gameObject.SetActive (false); // Set inactive the object.
		}

	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


	public void Jump (){ 


		Vector3 velocity =rb.velocity; 

		GetComponent<Rigidbody> ().velocity = new Vector3 (velocity.x, Mathf.Sqrt (2 * jumpHeight * gravity), velocity.z);

		jumpAudioSource.PlayOneShot (jumpSfx, 1f); // Play the jump sound

	}

	public void DoubleJump (){


		Vector3 velocity = rb.velocity;

		GetComponent<Rigidbody> ().velocity = new Vector3 (velocity.x, Mathf.Sqrt (2 * doubleJumpHeight * gravity), velocity.z);

		jumpAudioSource.PlayOneShot (jumpSfx, 1f); //Play the air jump sound

	}

	public void GameOver(){ // this function is called when the character collides with an enemy. See further.

		manager.gamePlaying = false; // Set the gameplaying bool to false.
		this.gameObject.GetComponent<Rigidbody>().isKinematic = true; // Set the kinematic option from the Rigidbody component to true.
		this.gameObject.SetActive (false);// Disable the character object.
		endGameMenu.GameOver (); // Call the game over script to indicate that the game has stopped.
		shake.ShakeCamera (0.35f, 0.3f); // This will shake the camera when the character collides with an enemy.
		loosingCoins = false;// if the character is loosing coins at the same moment that it dies, we set the loosing coins to false to avoid bad behaviors.

	}

	IEnumerator LooseCoins(){ // This coroutine is started when the player collides with a bad coin object. See further.

		loosingCoins = true; // Set the loose coins bool to true 

		yield return new WaitForSeconds (0.5F); // wait a little

		loosingCoins = false; // and get back the bool to false.

	}

	void checkGrounded() { // This function manages the character grounded raycast.
		/* ==============
         * REMEMBER
         * ==============
         * If you change the size of the prefab, you may have
         * to change the length of the ray to ensure it hits
         * the ground.
         * 
         * All obstacles/walls/floors must have rigidbodies
         * attached to them. If not, Unity physics may get
         * confused and the player can jump really high
         * when in a corner between 2 walls for example.
         */

		RaycastHit hit;
		Ray ray = new Ray(transform.position, -transform.up);
		Debug.DrawRay(ray.origin, ray.direction * rayGroundedLength);
		// if there is something directly below the player
		if (Physics.Raycast (ray, out hit, rayGroundedLength)) {
			isGrounded = true;

		} else {

			isGrounded = false;

		}

	}
////////////////////////////////////////////////////////// CHARACTER COLLISIONS MANAGEMENT/////////////////////////////////

	// Character Collisions management

	void OnTriggerEnter(Collider col) // if the character collides with a Coin
	{
		if (col.gameObject.tag == "Coin")
		{
			coinAudioSource.PlayOneShot (coinSfx, 1f); // Play the coin pick up sound

			uiCoin.SpawnFakeCoin (); // Call the object that instantiates fake coins that will fly to the UI.

		}

		if (col.gameObject.tag == "BigCoin")// if the character collides with a Big Coin
		{
			bigCoinAudioSource.PlayOneShot (bigCoinSfx, 1f);// Play the coin pick up sound

			uiCoin.SpawnTenFakeCoin ();// Call the object that instantiates 10 fake coins that will fly to the UI.

		}

		if (col.gameObject.tag == "Enemy" && manager.gamePlaying == true) // if character collides with an enemy and the game is in playing state
		{
			GameOver(); // Call the function that stops the game
			Instantiate(dieParticles, transform.position, Quaternion.identity); // Instantiate the broken egg prefab and some particles
			runParticles.gameObject.SetActive (false); // ensure to stop the running particles
			runAudioSource.Stop (); // ensure to stop the running audio effect.
			Debug.Log ("Enemy collision");

		}

		if (col.gameObject.tag == "BadCoin"&& manager.gamePlaying == true) // if character collides with a bad coin and the game is in playing state
		{
			shake.ShakeCamera (0.35f, 0.3f); // Call the camera shake effect script and perform a little shake effect.
			StartCoroutine(LooseCoins()); // Starts the coroutine that instantiates some particles.
			looseCoinAudioSource.PlayOneShot (looseCoinSfx, 1f);// Plays the loose coins audio sfx.

		}

}

}

