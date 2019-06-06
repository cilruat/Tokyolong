using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace JumperStepUp
{
// YOU BETTER RUN By BITBOYS STUDIO.
public class UICharacterScale : MonoBehaviour { 
	// In the Shop panel (in the UI) there is an object called "ZoomObject" this object has a collider component attached to it. 
	//If the Character models of the character selection menu collides with this collider that has a tag, this script is activated to change the character scale.

	[Header("Character Selection Models Scale")]
	public Vector3 maxScale; // Set the maximum scale that the scale method can reach.
	public Vector3 minScale; // Set the minimum scale that the scale method can reach.
	private bool scale = false; // bool variable used to cancel the model scale.
	public float scaleSpeed = 10f; // the scale speed.
	private Animator characterAnimator; // get the UI character model animator component

	void Start () {
		
		characterAnimator = this.gameObject.GetComponentInChildren<Animator> (); // Set the reference to the character animator component variable.

	}
	
	void Update () {

		if (scale) { // if the scale bool is activated.

			this.gameObject.transform.localScale = Vector3.Lerp (transform.localScale, maxScale, scaleSpeed * Time.deltaTime); // change the model scale from its original scale to the maximum scale.

				characterAnimator.SetBool ("CharacterOn", true); // enable the character animator component.
	
		} else {

			this.gameObject.transform.localScale = Vector3.Lerp (transform.localScale, minScale, scaleSpeed * Time.deltaTime); // change the model scale from its original scale to the minimum scale.

				characterAnimator.SetBool ("CharacterOn", false); // disable the character animator component.

		}

	}


	void OnTriggerEnter(Collider col) // if this object collides with the trigger object, we call the scale bool.
	{
		if (col.gameObject.tag == "ZoomUiZone") {
			scale = true;
		}
	}

		void OnTriggerExit(Collider col) // if the model leaves the collider zone, sets the scale bool to false.
	{
		if (col.gameObject.tag == "ZoomUiZone") {
			scale = false;
		}
	}
	}

}
