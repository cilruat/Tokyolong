using System;
using System.Security.Policy;
using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements;
using UnityEngine.UI;
// 3D ENDLESS RUNNER SYSTEM By BITBOYS STUDIO.

//We recommend to do not touch nothing in this script. Please be careful.

public class AdManager : MonoBehaviour
{
	[Header("Unity Goole Video Ads Manager")]

	private GoldeEggCounter goldenEggs; // Call the egg counter
	public Button eggRewardButton; // The video button
	public GameObject EggRewardPanel; // The panel that will appear if the player end viewing the ad video.
	public GameObject ExtraEggText; // The extra egg text image
	public GameObject NonExtraEggText; // The non extra egg text image
	public GameObject videoFailedText; // The failed video image
	public GameObject rewardBackPanel; // the black transparent panel that appeas below the egg reward panel.
	public GameObject getFreeEggText; // The text showed when the cooldown is over and the player can view a ad video.
	public Text cooldownText; // the countdown text component.
	public float coolDown = 120f; // The countdown duration in seconds.
	private float remainingCooldown; // The countdown remaining time to end.
	public string zoneId = "rewardedVideo"; //Use this id to put here your Unity Ads id created by yourself.
	public bool countdownWorking = false;

	void Start() {

		eggRewardButton.onClick.AddListener(OpenReward); // If we can press the reward button we call the open reward function.

		goldenEggs = FindObjectOfType<GoldeEggCounter> (); // Use this to find the egg counter script.



	}

	void Update() {

		if (Time.time > remainingCooldown && countdownWorking == true) {

			countdownWorking = false; // Stop the countdown to avoid negative countdown.

			cooldownText.text = ""; // If internet connection fails the button does not work and the text shows nothing.


		}

		if (countdownWorking == false) {

			remainingCooldown = 0; // Nothing to count.

		}

		if (!InternetConnectionCheck.unityAdsInitialized) { // If internet conection check fail the countdown stops working.

			countdownWorking = false;

		}

		if (countdownWorking == true) {

			getFreeEggText.gameObject.SetActive (false);

			cooldownText.text = "NEXT REWARD\n IN \b" + (int)(remainingCooldown - Time.time) + " \n SECONDS"; // Here we put what we want to show while the countdown timer is working.
		}


		if (InternetConnectionCheck.unityAdsInitialized) {


			if (eggRewardButton) { // if the ad reward button is active...

				eggRewardButton.interactable = IsReady (); // we call the is ready bool to set the interactable function to true.

				if (eggRewardButton.interactable) {

					getFreeEggText.gameObject.SetActive (true);

					cooldownText.text = ""; // make the cooldown text to show nothing.


				}


			}
		}
	} 


	public void ShowVideoAd(Action<ShowResult> adCallBackAction = null, string zone = "") {

		StartCoroutine(WaitForAdEditor());

		if (string.IsNullOrEmpty(zone)) {
			zone = null;
		}

		var options = new ShowOptions();

		if (adCallBackAction == null) {
			options.resultCallback = DefaultAdCallBackHandler;
		} else {
			options.resultCallback = adCallBackAction;
		}

		if (Advertisement.IsReady(zone)) {
			Debug.Log("Showing ad for zone: " + zone);
			Advertisement.Show(zone, options);
		} else {
			Debug.LogWarning("Ad was not ready. Zone: " + zone);
		}
	}

	private void DefaultAdCallBackHandler(ShowResult result) {
		switch (result) {
		case ShowResult.Finished:
			Time.timeScale = 1f;
			break;

		case ShowResult.Failed:
			Time.timeScale = 1f;
			break;

		case ShowResult.Skipped:
			Time.timeScale = 1f;
			break;
		}
	}



	public bool IsAdWithZoneIdReady(string zoneId) {
		return Advertisement.IsReady(zoneId);
	}



	/// Used to 'pause' the game when running in the Unity editor (Unity Ads will pause the game on actual Android or iOS devices by default, this is just for the editor)

	private IEnumerator WaitForAdEditor() {
		float currentTimescale = Time.timeScale;
		Time.timeScale = 0f;
		AudioListener.pause = true;

		yield return null;

		while (Advertisement.isShowing) {
			yield return null;
		}

		AudioListener.pause = false;
		if (currentTimescale > 0f) {
			Time.timeScale = currentTimescale;
		}
		else {
			Time.timeScale = 1f;
		}
	}

	private bool IsReady() {
		if (countdownWorking == false) {
			return IsAdWithZoneIdReady(zoneId);
		}

		return false;
	}


	private void OpenReward() {
		ShowVideoAd(PresentRewardCallback, zoneId);
	}

	private void PresentRewardCallback(ShowResult showResult) {

		switch (showResult) {
		case ShowResult.Finished:


			//	Debug.Log ("Player finished watching the video ad and is being rewarded with an extra Egg.");

			goldenEggs.RewardEgg ();// Give a reward egg if watch the video.

			EggRewardPanel.gameObject.SetActive (true);
			ExtraEggText.gameObject.SetActive (true);
			rewardBackPanel.gameObject.SetActive (true);

			if (coolDown > 0f && countdownWorking == false) {
				remainingCooldown = Time.time + coolDown;
				countdownWorking = true;
			}

			break;

		case ShowResult.Skipped:

			//Debug.Log ("Player skipped watching the video ad, no reward.");


			EggRewardPanel.gameObject.SetActive (true);
			NonExtraEggText.gameObject.SetActive (true);
			rewardBackPanel.gameObject.SetActive (true);


			break;


		case ShowResult.Failed:

			EggRewardPanel.gameObject.SetActive (true);
			videoFailedText.gameObject.SetActive (true);
			rewardBackPanel.gameObject.SetActive (true);


			//Debug.Log("VIDEO FAILED, NO REWARD.");
			break;
		}
	}







}