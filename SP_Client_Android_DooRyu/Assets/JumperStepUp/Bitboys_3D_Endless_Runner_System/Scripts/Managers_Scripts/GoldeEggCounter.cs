using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace JumperStepUp
{
// 3D ENDLESS RUNNER SYSTEM By BITBOYS STUDIO.
public class GoldeEggCounter : MonoBehaviour {


	[Header("Golde Egg (Continue Items) Counter")]

	private int eggCount; // Used to store the amount of continue game items that we already have.
	public Text goldenEggtext; // The golden egg conunter text component that we view in the gameplay scene.
	public Text goldenEggtextGameOver; // The golden egg conunter text component that we view in the game over scene.
	public bool canContinue; // sets true or false if the player can continue.

	void Start ()
	{
		// Save the egg amount in playerprefs(device)
		eggCount = PlayerPrefs.GetInt ("PlayerGoldenEggs"); // this option will save the player egg amount in the player preferences(device)

		// We call this to make sure that when the player runs the game by the first time, it has 2 golde eggs for free, but only if is the first time it plays the game.
		if (PlayerPrefs.GetInt("FirstPlay", 1) == 1)
		{
			//Debug.Log("First Time Playing");

			//Set first time opening to false
			PlayerPrefs.SetInt("FirstPlay", 0);

			eggCount = 2; 

			PlayerPrefs.SetInt ("PlayerGoldenEggs", eggCount); // stores the current coins in the player prefs.
		}
		else
		{
		//	Debug.Log("NOT First Time Playing");
		}

	}


	void Update () 
	{

		goldenEggtext.text = "" + eggCount; // the text component displays the amount of lives that the player has on game over screen.

		goldenEggtextGameOver.text = "" + eggCount; // the text component displays the amount of lives that the player has.

	
		if (eggCount >= 1) { // if the player has one egg or more..

			canContinue = true;
		} else {  // else if does not has eggs.

			canContinue = false;
		}		

	}
	public void RewardEgg(){ // We call this function from the admanager script, to give a free egg if the player views a video ad.

		eggCount= eggCount +1; 

		PlayerPrefs.SetInt ("PlayerGoldenEggs", eggCount); // stores the current coins in the player prefs.


	}


	public void AddEggs () // This function is only used to add eggs to the egg counter under the development.
	{
		eggCount++; 

		PlayerPrefs.SetInt ("PlayerGoldenEggs", eggCount); // stores the current coins in the player prefs.

	}
	public void BuyEggs () // We call this function from the iap manager, if the player buy eggs with real money
	{
		eggCount= eggCount +10; 

		PlayerPrefs.SetInt ("PlayerGoldenEggs", eggCount); // stores the current coins in the player prefs.

	}



	public void SpendEggs() // This function is called to substract eggs everytime the player touches the continue button in the game over scene.
	{
		eggCount--; 
		PlayerPrefs.SetInt ("PlayerGoldenEggs", eggCount);// stores the current lives in the player prefs.
	}

	}
}