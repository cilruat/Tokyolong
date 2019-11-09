using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace JumperStepUp
{
// 3D ENDLESS RUNNER SYSTEM By BITBOYS STUDIO.
public class ScreenTransition : MonoBehaviour {

	[Header("Camera Screen Transitions")]

	public Image[] panel; // The transition images list (UI Sprite) We use a list to change random between various transition types.
	public float fillSpeed = 50f; // The image fill transition speed.
	public bool fadeIn = false; // fade in is over?
	public bool fadeOut = false; // fade out is over?
	public Image backPanel; // The back panel that appears when the normal transition image fills the screen.


	void Update () {


		// We make shuffle in the transition images list and always use the firts image from the list ( [0] )

		if(fadeIn){ // Fade random in front panel

			if(panel[0].fillAmount <1f){ // if the fill amount is less than 1, that is to say, the image has not been completely filled, make the fade in.

				panel[0].fillAmount = (panel[0].fillAmount + 0.1f * fillSpeed * Time.smoothDeltaTime);


			}
		}
		if (fadeOut) {// Fade out random front panel


			if (panel[0].fillAmount > 0f) { // if the image is totally unfilled we fill it!



				panel[0].fillAmount = (panel[0].fillAmount - 0.1f * fillSpeed * Time.smoothDeltaTime);

			}
		}
			

		if(panel[0].fillAmount == 1f){ // Make sure the panel is full filled and set fade in bool  to false.

			fadeIn = false;
		}

			
		if(panel[0].fillAmount == 0f){ // Make sure the panel is not filled and set fade out bool to false.

			fadeOut = false;

		}


	}

	public void GoFade(){

		StartCoroutine(FadeInTexture());  // This method is called from the level manager script when we restart o continue the game after die.

	}

	public IEnumerator FadeInTexture()
	{
		panel[0].gameObject.SetActive (true);

		yield return new WaitForSeconds(0.25f);

		fadeIn = true;

	}

	public void BackFade (){

		StartCoroutine(FadeOutTexture()); 

	}

	public IEnumerator FadeOutTexture()
	{
		fadeOut = true;

		yield return new WaitForSeconds(panel[0].fillAmount);

		panel[0].gameObject.SetActive (false);

	}

	public void TransitionsShuffle (){ // Make shuffle to the transition images list before use them. 
		
		for (var i = 0; i < panel.Length; i++) {

			var temp = panel [i];

			var randomIndex = Random.Range (0, panel.Length);

			panel [i] = panel [randomIndex];

			panel [randomIndex] = temp;
		}
	}


	public void FadeinBlack (){

		StartCoroutine(BackPanelOn()); 
	}

	public void FadeOutBlack (){

		StartCoroutine(BackPanelOff()); 


	}

	public IEnumerator BackPanelOn() // When this is called, the back panel image appears fading in behind the transition image.
	{
		backPanel.gameObject.SetActive (true);

		yield return new WaitForSeconds(0.1f);

		backPanel.gameObject.GetComponent<FadeTexture> ().FadeIn();

	}

	public IEnumerator BackPanelOff() // When this is called, the back panel image dissappears fading in behind the transition image.
	{

		backPanel.gameObject.GetComponent<FadeTexture> ().FadeOut();

		yield return new WaitForSeconds(2f);

		backPanel.gameObject.SetActive (false);

	}
	}
}
