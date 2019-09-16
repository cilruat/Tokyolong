using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// YOU BETTER RUN By BITBOYS STUDIO.
public class EggUnlockButton : MonoBehaviour {// this script needs to be attached to the character unlock button in the character selection menu.

	[Header("Character Unlock Button")]


	private ScrollRectSnap charMenu;// Call the scroll rect menu script.
	private CharacterSelection charSelection; // Call the character selection script

	void Start () {

		charMenu = FindObjectOfType<ScrollRectSnap> ();// Put a reference to locate the variables created to connect with this scripts.
		charSelection = FindObjectOfType<CharacterSelection> ();

	}
		
	public void UnlockCharacter(){


		//UNLOCK CHARACTER 02
		if (charMenu.overCharacter02 && !charSelection.ownCharacter02 && CoinCounter.coinCount >= 1000) { // if the UI character is located at the center of the screen and the player has the amount of coins necessary to unlock the new custome...
			charSelection.UnlockCharacter02 (); // call the character selection script to unlock the character.
			//Debug.Log ("Character02 Unlocked and Selected");

			charSelection.SelectCharacter02 ();// Also at the same time that we unlock the character, automatically we select it.
		}
		//UNLOCK CHARACTER 03
		if (charMenu.overCharacter03 && !charSelection.ownCharacter03 && CoinCounter.coinCount >= 1000) {
			charSelection.UnlockCharacter03 ();
			//Debug.Log ("Character03 Unlocked and Selected");


			charSelection.SelectCharacter03 ();
		}
		//UNLOCK CHARACTER 04
		if (charMenu.overCharacter04 && !charSelection.ownCharacter04 && CoinCounter.coinCount >= 1000) {
			charSelection.UnlockCharacter04 ();
			//Debug.Log ("Character04 Unlocked and Selected");


			charSelection.SelectCharacter04 ();
		}
		//UNLOCK CHARACTER 05
		if (charMenu.overCharacter05 && !charSelection.ownCharacter05 && CoinCounter.coinCount >= 1000) {
			charSelection.UnlockCharacter05 ();
			//Debug.Log ("Character05 Unlocked and Selected");


			charSelection.SelectCharacter05 ();
		}
		//UNLOCK CHARACTER 06
		if (charMenu.overCharacter06 && !charSelection.ownCharacter06 && CoinCounter.coinCount >= 1000) {
			charSelection.UnlockCharacter06 ();
			//Debug.Log ("Character06 Unlocked and Selected");


			charSelection.SelectCharacter06 ();
		}
		//UNLOCK CHARACTER 07
		if (charMenu.overCharacter07 && !charSelection.ownCharacter07 && CoinCounter.coinCount >= 1000) {
			charSelection.UnlockCharacter07 ();
			//Debug.Log ("Character07 Unlocked and Selected");


			charSelection.SelectCharacter07 ();
		}
		//UNLOCK CHARACTER 08
		if (charMenu.overCharacter08 && !charSelection.ownCharacter08 && CoinCounter.coinCount >= 1000) {
			charSelection.UnlockCharacter08 ();
			//Debug.Log ("Character08 Unlocked and Selected");


			charSelection.SelectCharacter08 ();
		}
		//UNLOCK CHARACTER 09
		if (charMenu.overCharacter09 && !charSelection.ownCharacter09 && CoinCounter.coinCount >= 1000) {
			charSelection.UnlockCharacter09 ();
			//Debug.Log ("Character09 Unlocked and Selected");


			charSelection.SelectCharacter09 ();
		}
		//UNLOCK CHARACTER 10
		if (charMenu.overCharacter10 && !charSelection.ownCharacter10 && CoinCounter.coinCount >= 1000) {
			charSelection.UnlockCharacter10 ();
			//Debug.Log ("Character10 Unlocked and Selected");


			charSelection.SelectCharacter10 ();
		}
		//UNLOCK CHARACTER 11
		if (charMenu.overCharacter11 && !charSelection.ownCharacter11 && CoinCounter.coinCount >= 1000) {
			charSelection.UnlockCharacter11 ();
			//Debug.Log ("Character11 Unlocked and Selected");


			charSelection.SelectCharacter11 ();
		}
		//UNLOCK CHARACTER 12
		if (charMenu.overCharacter12 && !charSelection.ownCharacter12 && CoinCounter.coinCount >= 1000) {
			charSelection.UnlockCharacter12 ();
			//Debug.Log ("Character12 Unlocked and Selected");


			charSelection.SelectCharacter12 ();
		}
		//UNLOCK CHARACTER 13
		if (charMenu.overCharacter13 && !charSelection.ownCharacter13 && CoinCounter.coinCount >= 1000) {
			charSelection.UnlockCharacter13 ();
			//Debug.Log ("Character13 Unlocked and Selected");


			charSelection.SelectCharacter13 ();
		}
	}
}
