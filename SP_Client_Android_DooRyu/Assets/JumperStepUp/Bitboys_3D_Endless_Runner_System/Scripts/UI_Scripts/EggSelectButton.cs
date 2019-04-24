using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace JumperStepUp
{
// YOU BETTER RUN By BITBOYS STUDIO.
public class EggSelectButton : MonoBehaviour { // this script needs to be attached to the character select button in the character selection menu.

	[Header("Character Selection Button")]


	private ScrollRectSnap charMenu; // Call the scroll rect menu script.
	private CharacterSelection charSelection; // Call the character selection script

	void Start () {

		charMenu = FindObjectOfType<ScrollRectSnap> (); // Put a reference to locate the variables created to connect with this scripts.
		charSelection = FindObjectOfType<CharacterSelection> ();

	}

	public void SelectCharacter(){

		//SELECT CHARACTER 01
		if (charMenu.overCharacter01 && charSelection.ownCharacter01) { // if the UI character is located at the center of the screen and we have already unlocked it, we can selected.

			charSelection.SelectCharacter01 (); // Call the character selection script to select one character or another.
			//Debug.Log ("Character01 Selected");

		}
		//SELECT CHARACTER 02
		if (charMenu.overCharacter02 && charSelection.ownCharacter02) {

			charSelection.SelectCharacter02 ();
			//Debug.Log ("Character02 Selected");

		}
		//SELECT CHARACTER 03
		if (charMenu.overCharacter03 && charSelection.ownCharacter03) {

			charSelection.SelectCharacter03 ();
			//Debug.Log ("Character03 Selected");

		}
		//SELECT CHARACTER 04
		if (charMenu.overCharacter04 && charSelection.ownCharacter04) {

			charSelection.SelectCharacter04 ();
			//Debug.Log ("Character04 Selected");

		}
		//SELECT CHARACTER 05
		if (charMenu.overCharacter05 && charSelection.ownCharacter05) {

			charSelection.SelectCharacter05 ();
			//Debug.Log ("Character05 Selected");

		}
		//SELECT CHARACTER 06
		if (charMenu.overCharacter06 && charSelection.ownCharacter06) {

			charSelection.SelectCharacter06 ();
			//Debug.Log ("Character06 Selected");

		}
		//SELECT CHARACTER 07
		if (charMenu.overCharacter07 && charSelection.ownCharacter07) {

			charSelection.SelectCharacter07 ();
			//Debug.Log ("Character07 Selected");

		}
		//SELECT CHARACTER 08
		if (charMenu.overCharacter08 && charSelection.ownCharacter08) {

			charSelection.SelectCharacter08 ();
			//Debug.Log ("Character08 Selected");

		}
		//SELECT CHARACTER 09
		if (charMenu.overCharacter09 && charSelection.ownCharacter09) {

			charSelection.SelectCharacter09 ();
			//Debug.Log ("Character09 Selected");

		}
		//SELECT CHARACTER 10
		if (charMenu.overCharacter10 && charSelection.ownCharacter10) {

			charSelection.SelectCharacter10 ();
			//Debug.Log ("Character10 Selected");

		}
		//SELECT CHARACTER 11
		if (charMenu.overCharacter11 && charSelection.ownCharacter11) {

			charSelection.SelectCharacter11 ();
			//Debug.Log ("Character11 Selected");

		}
		//SELECT CHARACTER 12
		if (charMenu.overCharacter12 && charSelection.ownCharacter12) {

			charSelection.SelectCharacter12 ();
			//Debug.Log ("Character12 Selected");

		}
		//SELECT CHARACTER 13
		if (charMenu.overCharacter13 && charSelection.ownCharacter13) {

			charSelection.SelectCharacter13 ();
			//Debug.Log ("Character13 Selected");

		}
	}
}
}