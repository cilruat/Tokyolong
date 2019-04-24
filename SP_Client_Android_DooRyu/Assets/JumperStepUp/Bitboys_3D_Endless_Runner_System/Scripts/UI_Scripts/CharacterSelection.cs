using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace JumperStepUp
{
// YOU BETTER RUN By BITBOYS STUDIO.
public class CharacterSelection : MonoBehaviour {

	[Header("Character Selection Manager")]

	public static int characterSelected; // used to store what character custome are selected. // if you want to add new character you will need to add another int variable to this list.
	public static int hasChar01; // Used to set if a character has been already unlocked.
	public static int hasChar02;
	public static int hasChar03;
	public static int hasChar04;
	public static int hasChar05;
	public static int hasChar06;
	public static int hasChar07;
	public static int hasChar08;
	public static int hasChar09;
	public static int hasChar10;
	public static int hasChar11;
	public static int hasChar12;
	public static int hasChar13;

    public GameObject[] unlockPanels;// The little panels that appears in front of the characters when they are not unclocked.

	public bool ownCharacter01; // Used to store if we have unlocked a character or not.  // if you want to add new character you will need to add another bool variable to this list.
	public bool ownCharacter02 = false;
	public bool ownCharacter03 = false;
	public bool ownCharacter04 = false;
	public bool ownCharacter05 = false;
	public bool ownCharacter06 = false;
	public bool ownCharacter07 = false;
	public bool ownCharacter08 = false;
	public bool ownCharacter09 = false;
	public bool ownCharacter10 = false;
	public bool ownCharacter11 = false;
	public bool ownCharacter12 = false;
	public bool ownCharacter13 = false;


	private CoinCounter coins; // Call the coin counter script.
	private ScrollRectSnap scrollMenu;// call the scroll rect menu script

	[Header("Character Selection Menu models")] // if you want to add new character you will need to add another object to this list.
	public GameObject menuCharacter01;
	public GameObject menuCharacter02;
	public GameObject menuCharacter03;
	public GameObject menuCharacter04;
	public GameObject menuCharacter05;
	public GameObject menuCharacter06;
	public GameObject menuCharacter07;
	public GameObject menuCharacter08;
	public GameObject menuCharacter09;
	public GameObject menuCharacter10;
	public GameObject menuCharacter11;
	public GameObject menuCharacter12;
	public GameObject menuCharacter13;



	[Header("Character Selection Menu models Accessories")]
	public GameObject crazyCharacterHat;
	public GameObject alienCharacterAntenna;
	public GameObject devilCharacterHorns;
	public GameObject robotCharacterAntenna;
	public GameObject mariachiCharacterHat;
	public GameObject swagCharacterHat;
	public GameObject clownAccessories;
	public GameObject swimCharacterFloater;

	[Header("Sound Stuff *(Volume Controlled from the Mixer)")]
	public AudioSource celebrationAudioSource;
	public AudioClip celebrationSfx;
	public GameObject celebrationFx;
	public Transform celebrationFxPoint; // the point to instantiate the unlock celebration particles.

	void Start ()
	{
		coins = FindObjectOfType<CoinCounter> ();
		scrollMenu = FindObjectOfType<ScrollRectSnap> ();

		characterSelected = PlayerPrefs.GetInt ("CharacterSelect"); // this option will save the selected player in the player preferences(device)


		if (PlayerPrefs.GetInt("FirstPlayCharacter", 1) == 1) // this store if is the first time that the player plays the game.
		{

			//Set first time opening to false
			PlayerPrefs.SetInt("FirstPlayCharacter", 0);

			characterSelected = 1;  // Auto select the first character for the first time.

			hasChar01 = 1; 

			PlayerPrefs.SetInt ("HasCharacter01", hasChar01); // Save that we already have the character 01

			hasChar02 = 0; // Sets to false that have unlocked the other characters.
			hasChar03 = 0; 
			hasChar04 = 0; 
			hasChar05 = 0; 
			hasChar06 = 0; 
			hasChar07 = 0; 
			hasChar08 = 0; 
			hasChar09 = 0; 
			hasChar10 = 0; 
			hasChar11 = 0; 
			hasChar12 = 0; 
			hasChar13 = 0; 


			PlayerPrefs.SetInt ("CharacterSelect", characterSelected); // stores the current coins in the player prefs.
		}
		else
		{
			//Debug.Log("Not First Time playing");

		}
		///////////////////////////////////////////////////////////////////

		hasChar01 = PlayerPrefs.GetInt ("HasCharacter01"); // this option will save tif we already have a character or not and will save it in the player preferences(device)

		hasChar02 = PlayerPrefs.GetInt ("HasCharacter02"); 

		hasChar03 = PlayerPrefs.GetInt ("HasCharacter03"); 

		hasChar04 = PlayerPrefs.GetInt ("HasCharacter04"); 

		hasChar05 = PlayerPrefs.GetInt ("HasCharacter05");

		hasChar06 = PlayerPrefs.GetInt ("HasCharacter06"); 

		hasChar07 = PlayerPrefs.GetInt ("HasCharacter07"); 

		hasChar08 = PlayerPrefs.GetInt ("HasCharacter08"); 

		hasChar09 = PlayerPrefs.GetInt ("HasCharacter09"); 

		hasChar10 = PlayerPrefs.GetInt ("HasCharacter10"); 

		hasChar11 = PlayerPrefs.GetInt ("HasCharacter11"); 

		hasChar12 = PlayerPrefs.GetInt ("HasCharacter12"); 

		hasChar13 = PlayerPrefs.GetInt ("HasCharacter13"); 


	}
	
	// Update is called once per frame
	void Update () {

		if (!LevelManager.sfxActive) { // call the level manager to know if the sfx is activated or not.

			celebrationAudioSource.volume = 0;

		} else {

			celebrationAudioSource.volume = 1;

		}
		////////////////////////////////////CHARACTERS UNLOCK////////////////////////////////////////////////////////////////////////

		// Character 01 it's unlocked since game begins.
		if (hasChar01 == 1) {

			ownCharacter01 = true;
		} else {
			ownCharacter01 = false;
		}
		//Unlock character 02 or continue locked.
		if (hasChar02 == 1) { //if we have unlocked the character the int variable is set to 1.

			ownCharacter02 = true; // turn to bool owncharacter variable.
			unlockPanels[0].gameObject.SetActive (false); // Disable the unlock panel image.
			menuCharacter02.gameObject.GetComponentInChildren<Renderer>().material.SetColor ("_Color", Color.white); // change the character model material to white.
			alienCharacterAntenna.gameObject.GetComponent<Renderer>().material.SetColor ("_Color", Color.white); // chage the character accessories 3d model material to white.


		} else { // if we don't have unlocked the character.

			ownCharacter02 = false; // set its bool to false.
			unlockPanels[0].gameObject.SetActive (true); // The panel mantains active.
			menuCharacter02.gameObject.GetComponentInChildren<Renderer>().material.SetColor ("_Color", Color.black); // The character model material is set to black.
			menuCharacter02.gameObject.GetComponentInChildren<Animator> ().enabled = false; // the animation is not triggered.
			alienCharacterAntenna.gameObject.GetComponent<Renderer>().material.SetColor ("_Color", Color.black); // Also set the accessories material to black.


		}
		//Unlock character 03 or continue locked.

		if (hasChar03 == 1) {

			ownCharacter03 = true;
			unlockPanels[1].gameObject.SetActive (false);
			menuCharacter03.gameObject.GetComponentInChildren<Renderer>().material.SetColor ("_Color", Color.white);

		} else {

			ownCharacter03 = false;
			unlockPanels[1].gameObject.SetActive (true);
			menuCharacter03.gameObject.GetComponentInChildren<Renderer>().material.SetColor ("_Color", Color.black);
			menuCharacter03.gameObject.GetComponentInChildren<Animator> ().enabled = false;

		}
		//Unlock character 04 or continue locked.

		if (hasChar04 == 1) {

			ownCharacter04 = true;
			unlockPanels[2].gameObject.SetActive (false);
			menuCharacter04.gameObject.GetComponentInChildren<Renderer>().material.SetColor ("_Color", Color.white);
			crazyCharacterHat.gameObject.GetComponent<Renderer>().material.SetColor ("_Color", Color.white);


		} else {

			ownCharacter04 = false;
			unlockPanels[2].gameObject.SetActive (true);
			menuCharacter04.gameObject.GetComponentInChildren<Renderer>().material.SetColor ("_Color", Color.black);
			menuCharacter04.gameObject.GetComponentInChildren<Animator> ().enabled = false;
			crazyCharacterHat.gameObject.GetComponent<Renderer>().material.SetColor ("_Color", Color.black);


		}
		//Unlock character 05 or continue locked.

		if (hasChar05 == 1) {

			ownCharacter05 = true;
			unlockPanels[3].gameObject.SetActive (false);
			menuCharacter05.gameObject.GetComponentInChildren<Renderer>().material.SetColor ("_Color", Color.white);
			robotCharacterAntenna.gameObject.GetComponent<Renderer>().material.SetColor ("_Color", Color.white);


		} else {

			ownCharacter05 = false;
			unlockPanels[3].gameObject.SetActive (true);
			menuCharacter05.gameObject.GetComponentInChildren<Renderer>().material.SetColor ("_Color", Color.black);
			menuCharacter05.gameObject.GetComponentInChildren<Animator> ().enabled = false;
			robotCharacterAntenna.gameObject.GetComponent<Renderer>().material.SetColor ("_Color", Color.black);


		}
		//Unlock character 06 or continue locked.

		if (hasChar06 == 1) {

			ownCharacter06 = true;
			unlockPanels[4].gameObject.SetActive (false);
			menuCharacter06.gameObject.GetComponentInChildren<Renderer>().material.SetColor ("_Color", Color.white);
			devilCharacterHorns.gameObject.GetComponent<Renderer>().material.SetColor ("_Color", Color.white);


		} else {

			ownCharacter06 = false;
			unlockPanels[4].gameObject.SetActive (true);
			menuCharacter06.gameObject.GetComponentInChildren<Renderer>().material.SetColor ("_Color", Color.black);
			menuCharacter06.gameObject.GetComponentInChildren<Animator> ().enabled = false;
			devilCharacterHorns.gameObject.GetComponent<Renderer>().material.SetColor ("_Color", Color.black);


		}
		//Unlock character 07 or continue locked.

		if (hasChar07 == 1) {

			ownCharacter07 = true;
			unlockPanels[5].gameObject.SetActive (false);
			menuCharacter07.gameObject.GetComponentInChildren<Renderer>().material.SetColor ("_Color", Color.white);

		} else {

			ownCharacter07 = false;
			unlockPanels[5].gameObject.SetActive (true);
			menuCharacter07.gameObject.GetComponentInChildren<Renderer>().material.SetColor ("_Color", Color.black);
			menuCharacter07.gameObject.GetComponentInChildren<Animator> ().enabled = false;

		}
		//Unlock character 08 or continue locked.

		if (hasChar08 == 1) {

			ownCharacter08 = true;
			unlockPanels[6].gameObject.SetActive (false);
			menuCharacter08.gameObject.GetComponentInChildren<Renderer>().material.SetColor ("_Color", Color.white);

		} else {

			ownCharacter08 = false;
			unlockPanels[6].gameObject.SetActive (true);
			menuCharacter08.gameObject.GetComponentInChildren<Renderer>().material.SetColor ("_Color", Color.black);
			menuCharacter08.gameObject.GetComponentInChildren<Animator> ().enabled = false;

		}

		//Unlock character 09 or continue locked.

		if (hasChar09 == 1) {

			ownCharacter09 = true;
			unlockPanels[7].gameObject.SetActive (false);
			menuCharacter09.gameObject.GetComponentInChildren<Renderer>().material.SetColor ("_Color", Color.white);

		} else {

			ownCharacter09 = false;
			unlockPanels[7].gameObject.SetActive (true);
			menuCharacter09.gameObject.GetComponentInChildren<Renderer>().material.SetColor ("_Color", Color.black);
			menuCharacter09.gameObject.GetComponentInChildren<Animator> ().enabled = false;

		}
		//Unlock character 10 or continue locked.

		if (hasChar10 == 1) {

			ownCharacter10 = true;
			unlockPanels[8].gameObject.SetActive (false);
			menuCharacter10.gameObject.GetComponentInChildren<Renderer>().material.SetColor ("_Color", Color.white);
			mariachiCharacterHat.gameObject.GetComponent<Renderer>().material.SetColor ("_Color", Color.white);


		} else {

			ownCharacter10 = false;
			unlockPanels[8].gameObject.SetActive (true);
			menuCharacter10.gameObject.GetComponentInChildren<Renderer>().material.SetColor ("_Color", Color.black);
			menuCharacter10.gameObject.GetComponentInChildren<Animator> ().enabled = false;
			mariachiCharacterHat.gameObject.GetComponent<Renderer>().material.SetColor ("_Color", Color.black);


		}
		//Unlock character 11 or continue locked.

		if (hasChar11 == 1) {

			ownCharacter11 = true;
			unlockPanels[9].gameObject.SetActive (false);
			menuCharacter11.gameObject.GetComponentInChildren<Renderer>().material.SetColor ("_Color", Color.white);
			swagCharacterHat.gameObject.GetComponent<Renderer>().material.SetColor ("_Color", Color.white);


		} else {

			ownCharacter11 = false;
			unlockPanels[9].gameObject.SetActive (true);
			menuCharacter11.gameObject.GetComponentInChildren<Renderer>().material.SetColor ("_Color", Color.black);
			menuCharacter11.gameObject.GetComponentInChildren<Animator> ().enabled = false;
			swagCharacterHat.gameObject.GetComponent<Renderer>().material.SetColor ("_Color", Color.black);


		}
		//Unlock character 12 or continue locked.

		if (hasChar12 == 1) {

			ownCharacter12 = true;
			unlockPanels[10].gameObject.SetActive (false);
			menuCharacter12.gameObject.GetComponentInChildren<Renderer>().material.SetColor ("_Color", Color.white);
			clownAccessories.gameObject.GetComponent<Renderer>().material.SetColor ("_Color", Color.white);


		} else {

			ownCharacter12 = false;
			unlockPanels[10].gameObject.SetActive (true);
			menuCharacter12.gameObject.GetComponentInChildren<Renderer>().material.SetColor ("_Color", Color.black);
			menuCharacter12.gameObject.GetComponentInChildren<Animator> ().enabled = false;
			clownAccessories.gameObject.GetComponent<Renderer>().material.SetColor ("_Color", Color.black);


		}
		//Unlock character 13 or continue locked.

		if (hasChar13 == 1) {

			ownCharacter13 = true;
			unlockPanels[11].gameObject.SetActive (false);
			menuCharacter13.gameObject.GetComponentInChildren<Renderer>().material.SetColor ("_Color", Color.white);
			swimCharacterFloater.gameObject.GetComponent<Renderer>().material.SetColor ("_Color", Color.white);


		} else {

			ownCharacter13 = false;
			unlockPanels[11].gameObject.SetActive (true);
			menuCharacter13.gameObject.GetComponentInChildren<Renderer>().material.SetColor ("_Color", Color.black);
			menuCharacter13.gameObject.GetComponentInChildren<Animator> ().enabled = false;
			swimCharacterFloater.gameObject.GetComponent<Renderer>().material.SetColor ("_Color", Color.black);


		}

		/////////////////////////////////////////////////////////////DISABLE ANIMATION IF CHARACTER IS NOT CENTERED ON SCREEN///////////////////////////////
		 
		//CHARACTER01 ANIMATOR
		if (scrollMenu.overCharacter01) {
			menuCharacter01.gameObject.GetComponentInChildren<Animator> ().enabled = true; // This ensure that the animator does not play any anomation if the player is not centerd in the center of the screen in the character selection panel.
		} else {
			menuCharacter01.gameObject.GetComponentInChildren<Animator> ().enabled = false;
		}
		//CHARACTER02 ANIMATOR
		if (scrollMenu.overCharacter02) {
			menuCharacter02.gameObject.GetComponentInChildren<Animator> ().enabled = true;
		} else {
			menuCharacter02.gameObject.GetComponentInChildren<Animator> ().enabled = false;
		}
		//CHARACTER03 ANIMATOR
		if (scrollMenu.overCharacter03) {
			menuCharacter03.gameObject.GetComponentInChildren<Animator> ().enabled = true;
		} else {
			menuCharacter03.gameObject.GetComponentInChildren<Animator> ().enabled = false;
		}
		//CHARACTER04 ANIMATOR
		if (scrollMenu.overCharacter04) {
			menuCharacter04.gameObject.GetComponentInChildren<Animator> ().enabled = true;
		} else {
			menuCharacter04.gameObject.GetComponentInChildren<Animator> ().enabled = false;
		}
		//CHARACTER05 ANIMATOR
		if (scrollMenu.overCharacter05) {
			menuCharacter05.gameObject.GetComponentInChildren<Animator> ().enabled = true;
		} else {
			menuCharacter05.gameObject.GetComponentInChildren<Animator> ().enabled = false;
		}
		//CHARACTER06 ANIMATOR
		if (scrollMenu.overCharacter06) {
			menuCharacter06.gameObject.GetComponentInChildren<Animator> ().enabled = true;
		} else {
			menuCharacter06.gameObject.GetComponentInChildren<Animator> ().enabled = false;
		}
		//CHARACTER07 ANIMATOR
		if (scrollMenu.overCharacter07) {
			menuCharacter07.gameObject.GetComponentInChildren<Animator> ().enabled = true;
		} else {
			menuCharacter07.gameObject.GetComponentInChildren<Animator> ().enabled = false;
		}
		//CHARACTER08 ANIMATOR
		if (scrollMenu.overCharacter08) {
			menuCharacter08.gameObject.GetComponentInChildren<Animator> ().enabled = true;
		} else {
			menuCharacter08.gameObject.GetComponentInChildren<Animator> ().enabled = false;
		}
		//CHARACTER09 ANIMATOR
		if (scrollMenu.overCharacter09) {
			menuCharacter09.gameObject.GetComponentInChildren<Animator> ().enabled = true;
		} else {
			menuCharacter09.gameObject.GetComponentInChildren<Animator> ().enabled = false;
		}
		//CHARACTER10 ANIMATOR
		if (scrollMenu.overCharacter10) {
			menuCharacter10.gameObject.GetComponentInChildren<Animator> ().enabled = true;
		} else {
			menuCharacter10.gameObject.GetComponentInChildren<Animator> ().enabled = false;
		}
		//CHARACTER11 ANIMATOR
		if (scrollMenu.overCharacter11) {
			menuCharacter11.gameObject.GetComponentInChildren<Animator> ().enabled = true;
		} else {
			menuCharacter11.gameObject.GetComponentInChildren<Animator> ().enabled = false;
		}
		//CHARACTER12 ANIMATOR
		if (scrollMenu.overCharacter12) {
			menuCharacter12.gameObject.GetComponentInChildren<Animator> ().enabled = true;
		} else {
			menuCharacter12.gameObject.GetComponentInChildren<Animator> ().enabled = false;
		}
		//CHARACTER13 ANIMATOR
		if (scrollMenu.overCharacter13) {
			menuCharacter13.gameObject.GetComponentInChildren<Animator> ().enabled = true;
		} else {
			menuCharacter13.gameObject.GetComponentInChildren<Animator> ().enabled = false;
		}

	}

	// UNLOCK CHARACTERS (CALLED FROM BUTTON)//

	public void UnlockCharacter02(){

		if (hasChar02 == 0) { // if we have not yet unlocked the character

			if (CoinCounter.coinCount >= 1000) { // if the player coins amount is equal or bigger thant the character price

				coins.BuyCharacter (); // we call the coin counter script an subtract the coins.

				hasChar02 = 1;  // change the int variable to 1, this makes that we can't buy the character two times.

				CharacterUnlockCelebration (); // call the function that spawn some particles

				PlayerPrefs.SetInt ("HasCharacter02", hasChar02); // Save that we have unlocked the character to avoid loose the progress when the player closes the game on their device.

			}
		}
	}
	public void UnlockCharacter03(){

		if (hasChar03 == 0) {

			if (CoinCounter.coinCount >= 1000) {

				coins.BuyCharacter ();

				hasChar03 = 1; 

				CharacterUnlockCelebration ();


				PlayerPrefs.SetInt ("HasCharacter03", hasChar03);

			}
		}
	}
	public void UnlockCharacter04(){

		if (hasChar04 == 0) {

			if (CoinCounter.coinCount >= 1000) {

				coins.BuyCharacter ();

				hasChar04 = 1; 

				CharacterUnlockCelebration ();


				PlayerPrefs.SetInt ("HasCharacter04", hasChar04);

			}
		}
	}
	public void UnlockCharacter05(){

		if (hasChar05 == 0) {

			if (CoinCounter.coinCount >= 1000) {

				coins.BuyCharacter ();

				hasChar05 = 1; 

				CharacterUnlockCelebration ();


				PlayerPrefs.SetInt ("HasCharacter05", hasChar05);

			}
		}
	}
	public void UnlockCharacter06(){

		if (hasChar06 == 0) {

			if (CoinCounter.coinCount >= 1000) {

				coins.BuyCharacter ();

				hasChar06 = 1; 

				CharacterUnlockCelebration ();


				PlayerPrefs.SetInt ("HasCharacter06", hasChar06);

			}
		}
	}
	public void UnlockCharacter07(){

		if (hasChar07 == 0) {

			if (CoinCounter.coinCount >= 1000) {

				coins.BuyCharacter ();

				hasChar07 = 1; 

				CharacterUnlockCelebration ();


				PlayerPrefs.SetInt ("HasCharacter07", hasChar07);

			}
		}
	}
	public void UnlockCharacter08(){

		if (hasChar08 == 0) {

			if (CoinCounter.coinCount >= 1000) {

				coins.BuyCharacter ();

				hasChar08 = 1; 

				CharacterUnlockCelebration ();


				PlayerPrefs.SetInt ("HasCharacter08", hasChar08);

			}
		}
	}
	public void UnlockCharacter09(){

		if (hasChar09 == 0) {

			if (CoinCounter.coinCount >= 1000) {

				coins.BuyCharacter ();

				hasChar09 = 1; 

				CharacterUnlockCelebration ();


				PlayerPrefs.SetInt ("HasCharacter09", hasChar09);

			}
		}
	}
	public void UnlockCharacter10(){

		if (hasChar10 == 0) {

			if (CoinCounter.coinCount >= 1000) {

				coins.BuyCharacter ();

				hasChar10 = 1; 

				CharacterUnlockCelebration ();


				PlayerPrefs.SetInt ("HasCharacter10", hasChar10);

			}
		}
	}
	public void UnlockCharacter11(){

		if (hasChar11 == 0) {

			if (CoinCounter.coinCount >= 1000) {

				coins.BuyCharacter ();

				hasChar11 = 1; 

				CharacterUnlockCelebration ();


				PlayerPrefs.SetInt ("HasCharacter11", hasChar11);

			}
		}
	}
	public void UnlockCharacter12(){

		if (hasChar12 == 0) {

			if (CoinCounter.coinCount >= 1000) {

				coins.BuyCharacter ();

				hasChar12 = 1; 

				CharacterUnlockCelebration ();


				PlayerPrefs.SetInt ("HasCharacter12", hasChar12);

			}
		}
	}
	public void UnlockCharacter13(){

		if (hasChar13 == 0) {

			if (CoinCounter.coinCount >= 1000) {

				coins.BuyCharacter ();

				hasChar13 = 1; 

				CharacterUnlockCelebration ();


				PlayerPrefs.SetInt ("HasCharacter13", hasChar13);

			}
		}
	}

 /////////////////////////////////////////////////////////////////////////////////


	// SELECT THE CHARACTER
	public void SelectCharacter01 () // when we select a character from the character selection menu we save it in the player preferences so this way we ensure that when the game is opened again the player don't have to select again the character.
	{
		if (hasChar01 == 1) {
			
			characterSelected = 1; // Change  the character int variable to the selected character.

			PlayerPrefs.SetInt ("CharacterSelect", characterSelected); // save the selection in playerprefs.

		}
	}

	public void SelectCharacter02 ()
	{
		if (hasChar02 == 1) {
			
			characterSelected = 2; 

			PlayerPrefs.SetInt ("CharacterSelect", characterSelected); 
		}
	}
	public void SelectCharacter03 ()
	{
		if (hasChar03 == 1) {
			
			characterSelected = 3; 

			PlayerPrefs.SetInt ("CharacterSelect", characterSelected); 
		}
	}
	public void SelectCharacter04 ()
	{
		if (hasChar04 == 1) {
			
			characterSelected = 4; 

			PlayerPrefs.SetInt ("CharacterSelect", characterSelected); 
		}
	}
	public void SelectCharacter05 ()
	{
		if (hasChar05 == 1) {
			
			characterSelected = 5; 

			PlayerPrefs.SetInt ("CharacterSelect", characterSelected); 
		}
	}
	public void SelectCharacter06 ()
	{
		if (hasChar06 == 1) {
			
			characterSelected = 6; 

			PlayerPrefs.SetInt ("CharacterSelect", characterSelected); 
		}
	}
	public void SelectCharacter07 ()
	{
		if (hasChar07 == 1) {
			
			characterSelected = 7; 

			PlayerPrefs.SetInt ("CharacterSelect", characterSelected); 
		}
	}
	public void SelectCharacter08 ()
	{
		if (hasChar08 == 1) {
			
			characterSelected = 8; 

			PlayerPrefs.SetInt ("CharacterSelect", characterSelected); 
		}
	}
	public void SelectCharacter09 ()
	{
		if (hasChar09 == 1) {

			characterSelected = 9; 

			PlayerPrefs.SetInt ("CharacterSelect", characterSelected); 
		}
	}
	public void SelectCharacter10 ()
	{
		if (hasChar10 == 1) {

			characterSelected = 10; 

			PlayerPrefs.SetInt ("CharacterSelect", characterSelected); 
		}
	}
	public void SelectCharacter11 ()
	{
		if (hasChar11 == 1) {

			characterSelected = 11; 

			PlayerPrefs.SetInt ("CharacterSelect", characterSelected); 
		}
	}
	public void SelectCharacter12 ()
	{
		if (hasChar12 == 1) {

			characterSelected = 12; 

			PlayerPrefs.SetInt ("CharacterSelect", characterSelected); 
		}
	}
	public void SelectCharacter13 ()
	{
		if (hasChar13 == 1) {

			characterSelected = 13; 

			PlayerPrefs.SetInt ("CharacterSelect", characterSelected); 
		}
	}

	public void CharacterUnlockCelebration(){ // this function is called every time that the player unlock a new character.

		celebrationAudioSource.PlayOneShot (celebrationSfx, 1f); //Play the unlock celebrating sound.
	
		Instantiate(celebrationFx, celebrationFxPoint.transform.position, celebrationFxPoint.transform.rotation); // Spawn some particles at the celebration effects point.

}
		
}
}