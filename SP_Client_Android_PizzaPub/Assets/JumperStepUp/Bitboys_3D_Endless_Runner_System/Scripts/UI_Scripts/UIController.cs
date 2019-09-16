using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// YOU BETTER RUN By BITBOYS STUDIO.
public class UIController : MonoBehaviour {

	[Header("UI Animations Manager")]
	// don't touch this!!
	public Animator scorePanelAnim;
	public Animator topPanelAnim;
	public Animator playButtonAnim;
	public Animator videoAdsButtonAnim;
	public Animator shopButtonAnim;
	public Animator settingsMenuAnim;
	public Animator exitGamePanelAnim;

	[Header("UI Animated Buttons")]
	// don't touch this!!

	public GameObject settingsEgg;
	public GameObject settingsMusic;
	public GameObject settingsSfx;
	public GameObject settingsInfo;
	public GameObject settingsRate;
	public GameObject settingsHowTo;
	public GameObject spendCoinButton;


	public bool settingsMenuOpened = false; // this bool variable is used to know if the player has 1000 coins or more and set active the spend coins button.
	private LevelManager manager;
	private PauseMenu pause;

	void Start () {
		manager = FindObjectOfType<LevelManager> ();

	}
	void Update(){

		if (CoinCounter.coinCount >= 1000) { // if coins amount is bigger than 1000 coins we activate the coin/spend coins button that opens the character selection/shop.

			spendCoinButton.gameObject.SetActive (true);
		} else {

			spendCoinButton.gameObject.SetActive (false);

		}
	}


	// TOP SCREEN SCORE SMALL PANEL

	public void PanelDown(){
		
		StartCoroutine(PanelDownAnim()); 
	}

	public void PanelUp(){
		
		StartCoroutine(PanelUpAnim()); 

	}
	// TOP SCREEN PANEL (GAME OVER)

	public void GameOverPanelDown(){
		
		StartCoroutine(GameOverPanelDownAnim()); 

	}

	public void GameOverPanelUp(){

		StartCoroutine(GameOverPanelUpAnim()); 
	}
	// EXIT GAME PANEL

	public void ExitGamePanelActivate(){

		StartCoroutine(ExitPanelOnAnim()); 

	}

	public void ExitGamePanelDeactivate(){

		StartCoroutine(ExitPanelOffAnim()); 
	}


// BUTTONS//

	// PLAY BUTTON TRIGGER

	public void PlayButtonOn(){

		StartCoroutine(PlayButtonOnAnim()); 

	}

	public void PlayButtonOff(){


		StartCoroutine(PlayButtonOffAnim()); 

	}
	// SETTINGS BUTTON TRIGGER

	public void SettingButtonOpen(){

		StartCoroutine(SettingsButtonOpenAnim()); 

	}

	public void SettingButtonClose(){


		StartCoroutine(SettingsButtonCloseAnim()); 

	}

	public void SettingButtonShow(){

		StartCoroutine(SettingsMenuOn()); 

	}

	public void SettingButtonHide(){

		StartCoroutine(SettingsMenuOff()); 

	}
	// VIDEO ADS BUTTON TRIGGER

	public void AdsButtonOn(){

		StartCoroutine(AdsButtonInflateAnim()); 

	}
	// SHOP BUTTON TRIGGER

	public void ShopButtonOn(){

		StartCoroutine(ShopButtonInflateAnim()); 

	}	



	// COROUTINES, USE THEM TO AVOID CONSOLE WARNING ABOUT THE ANIMATOR HAS NOT BEEN INITIALIZED


	public IEnumerator PanelDownAnim(){

		yield return new WaitUntil(() => scorePanelAnim.isInitialized);

		scorePanelAnim.SetTrigger ("Panel_Down");

	}
	public IEnumerator PanelUpAnim(){

		yield return new WaitUntil(() => scorePanelAnim.isInitialized);

		scorePanelAnim.SetTrigger ("Panel_Up");

	}
	public IEnumerator GameOverPanelDownAnim(){

		yield return new WaitUntil(() => topPanelAnim.isInitialized);

		topPanelAnim.SetTrigger ("Panel_Down");

	}
	public IEnumerator GameOverPanelUpAnim(){

		yield return new WaitUntil(() => topPanelAnim.isInitialized);

		topPanelAnim.SetTrigger ("Panel_Up");

	}
	public IEnumerator ExitPanelOnAnim(){

		yield return new WaitUntil(() => exitGamePanelAnim.isInitialized);

		exitGamePanelAnim.SetTrigger ("PanelOn");

	}
	public IEnumerator ExitPanelOffAnim(){

		yield return new WaitUntil(() => exitGamePanelAnim.isInitialized);

		exitGamePanelAnim.SetTrigger ("PanelOff");

	}

	public IEnumerator PlayButtonOnAnim(){

		if (!manager.gamePlaying) {

			yield return new WaitUntil (() => playButtonAnim.isInitialized);

			playButtonAnim.SetTrigger ("ButtonOn");
		}
	}
	public IEnumerator PlayButtonOffAnim(){

		yield return new WaitUntil(() => playButtonAnim.isInitialized);

		playButtonAnim.SetTrigger ("ButtonOff");

	}

	public IEnumerator AdsButtonInflateAnim(){

		yield return new WaitUntil(() => videoAdsButtonAnim.isInitialized);

		videoAdsButtonAnim.SetTrigger ("Inflate");

	}
	public IEnumerator ShopButtonInflateAnim(){

		yield return new WaitUntil(() => shopButtonAnim.isInitialized);

		shopButtonAnim.SetTrigger ("Inflate");

	}
	public IEnumerator SettingsButtonOpenAnim(){

		if (!settingsMenuOpened) {



			settingsEgg.gameObject.SetActive (true);
			settingsMusic.gameObject.SetActive (true);
			settingsSfx.gameObject.SetActive (true);
			settingsInfo.gameObject.SetActive (true);
			settingsRate.gameObject.SetActive (true);
			settingsHowTo.gameObject.SetActive (true);

			yield return new WaitForSeconds(0.25f);


			yield return new WaitUntil (() => settingsEgg.gameObject.GetComponent<Animator>().isInitialized);
			yield return new WaitUntil (() => settingsMusic.gameObject.GetComponent<Animator>().isInitialized);
			yield return new WaitUntil (() => settingsSfx.gameObject.GetComponent<Animator>().isInitialized);
			yield return new WaitUntil (() => settingsInfo.gameObject.GetComponent<Animator>().isInitialized);
			yield return new WaitUntil (() => settingsRate.gameObject.GetComponent<Animator>().isInitialized);
			yield return new WaitUntil (() => settingsHowTo.gameObject.GetComponent<Animator>().isInitialized);


			settingsEgg.gameObject.GetComponent<Animator>().SetTrigger ("ButtonOn");
			settingsMusic.gameObject.GetComponent<Animator>().SetTrigger ("ButtonOn");
			settingsSfx.gameObject.GetComponent<Animator>().SetTrigger ("ButtonOn");
			settingsInfo.gameObject.GetComponent<Animator>().SetTrigger ("ButtonOn");
			settingsRate.gameObject.GetComponent<Animator>().SetTrigger ("ButtonOn");
			settingsHowTo.gameObject.GetComponent<Animator>().SetTrigger ("ButtonOn");



			settingsEgg.gameObject.GetComponent<Image> ().raycastTarget = true; // enable the raycast target
			settingsMusic.gameObject.GetComponent<Image> ().raycastTarget = true; 
			settingsSfx.gameObject.GetComponent<Image> ().raycastTarget = true; 
			settingsInfo.gameObject.GetComponent<Image> ().raycastTarget = true;
			settingsRate.gameObject.GetComponent<Image> ().raycastTarget = true;
			settingsHowTo.gameObject.GetComponent<Image> ().raycastTarget = true;


			settingsEgg.gameObject.GetComponent<FadeTexture> ().FadeIn (); // enable the raycast target
			settingsMusic.gameObject.GetComponent<FadeTexture> ().FadeIn (); // enable the raycast target
			settingsSfx.gameObject.GetComponent<FadeTexture> ().FadeIn (); // enable the raycast target
			settingsInfo.gameObject.GetComponent<FadeTexture> ().FadeIn (); // enable the raycast target
			settingsRate.gameObject.GetComponent<FadeTexture> ().FadeIn (); // enable the raycast target
			settingsHowTo.gameObject.GetComponent<FadeTexture> ().FadeIn (); // enable the raycast target


			settingsMenuOpened = true;

		}
	}
	public IEnumerator SettingsButtonCloseAnim(){

		if (settingsMenuOpened) {


			

			yield return new WaitUntil (() => settingsEgg.gameObject.GetComponent<Animator>().isInitialized);
			yield return new WaitUntil (() => settingsMusic.gameObject.GetComponent<Animator>().isInitialized);
			yield return new WaitUntil (() => settingsSfx.gameObject.GetComponent<Animator>().isInitialized);
			yield return new WaitUntil (() => settingsInfo.gameObject.GetComponent<Animator>().isInitialized);
			yield return new WaitUntil (() => settingsRate.gameObject.GetComponent<Animator>().isInitialized);
			yield return new WaitUntil (() => settingsHowTo.gameObject.GetComponent<Animator>().isInitialized);


			settingsEgg.gameObject.GetComponent<Animator>().SetTrigger ("ButtonOff");
			settingsMusic.gameObject.GetComponent<Animator>().SetTrigger ("ButtonOff");
			settingsSfx.gameObject.GetComponent<Animator>().SetTrigger ("ButtonOff");
			settingsInfo.gameObject.GetComponent<Animator>().SetTrigger ("ButtonOff");
			settingsRate.gameObject.GetComponent<Animator>().SetTrigger ("ButtonOff");
			settingsHowTo.gameObject.GetComponent<Animator>().SetTrigger ("ButtonOff");


			settingsEgg.gameObject.GetComponent<Image> ().raycastTarget = false; // Disable the pause button to avoid touches when game is over.
			settingsMusic.gameObject.GetComponent<Image> ().raycastTarget = false; // Disable the pause button to avoid touches when game is over.
			settingsSfx.gameObject.GetComponent<Image> ().raycastTarget = false; // Disable the pause button to avoid touches when game is over.
			settingsInfo.gameObject.GetComponent<Image> ().raycastTarget = false; // Disable the pause button to avoid touches when game is over.
			settingsRate.gameObject.GetComponent<Image> ().raycastTarget = false; // Disable the pause button to avoid touches when game is over.
			settingsHowTo.gameObject.GetComponent<Image> ().raycastTarget = false; // Disable the pause button to avoid touches when game is over.

			settingsEgg.gameObject.GetComponent<FadeTexture> ().FadeOut (); // enable the raycast target
			settingsMusic.gameObject.GetComponent<FadeTexture> ().FadeOut (); // enable the raycast target
			settingsSfx.gameObject.GetComponent<FadeTexture> ().FadeOut (); // enable the raycast target
			settingsInfo.gameObject.GetComponent<FadeTexture> ().FadeOut (); // enable the raycast target
			settingsRate.gameObject.GetComponent<FadeTexture> ().FadeOut (); // enable the raycast target
			settingsHowTo.gameObject.GetComponent<FadeTexture> ().FadeOut (); // enable the raycast target

			yield return new WaitForSeconds(0.5f);

			settingsEgg.gameObject.SetActive (false);
			settingsMusic.gameObject.SetActive (false);
			settingsSfx.gameObject.SetActive (false);
			settingsInfo.gameObject.SetActive (false);
			settingsRate.gameObject.SetActive (false);
			settingsHowTo.gameObject.SetActive (false);


			settingsMenuOpened = false;


	}

}

	public IEnumerator SettingsMenuOn(){

		yield return new WaitUntil(() => settingsMenuAnim.isInitialized);

		settingsMenuAnim.SetTrigger ("MenuOn");

	}

	public IEnumerator SettingsMenuOff(){

		yield return new WaitUntil(() => settingsMenuAnim.isInitialized);

		settingsMenuAnim.SetTrigger ("MenuOff");

	}


}
