using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace JumperStepUp
{

// 3D ENDLESS RUNNER SYSTEM By BITBOYS STUDIO.
public class CoinCounter : MonoBehaviour {


	[Header("Coin Counter Manager")]

	public static int coinCount ; // this variable manages the coins amount and is set to static to can call it easily from other scripts.
	public Text scorePanelCoins; // The in game panel coin counter text component.
	public Text gameOverPanelCoins;// The game over top panel coin counter text component.


	void Awake () 
	{


		// We use PlayerPrefs to save the amount of coins that the player have.
		coinCount = PlayerPrefs.GetInt("PlayerCurrentCoins"); // this option will make that the lives that we collect are stored in the project preferences or in the device wich we play.

	}



	void Update () 
	{

		scorePanelCoins.text = "" + coinCount; // the text component displays the amount of lives that the player has.
		gameOverPanelCoins.text = "" + coinCount; // the text component displays the amount of lives that the player has.


	}

 
	public void AddCoins ()
	{
		coinCount++; 

		PlayerPrefs.SetInt ("PlayerCurrentCoins", coinCount); // stores the current coins in the player prefs.

	}
	public void AddTenCoins ()
	{
		coinCount = coinCount + 10; 

		PlayerPrefs.SetInt ("PlayerCurrentCoins", coinCount); // stores the current coins in the player prefs.

	}
	public void AddCoinsTest ()
	{
		coinCount = coinCount + 1000; 

		PlayerPrefs.SetInt ("PlayerCurrentCoins", coinCount); // stores the current coins in the player prefs.

	}

	public void QuitCoins ()
	{

		if(coinCount >= 10){
		coinCount = coinCount - 10; 

		PlayerPrefs.SetInt ("PlayerCurrentCoins", coinCount); // stores the current coins in the player prefs.
		}

		if(coinCount <= 10){
			coinCount = 0; 

			PlayerPrefs.SetInt ("PlayerCurrentCoins", coinCount); // stores the current coins in the player prefs.
		}
	}
	public void ResetCoins()
	{
		coinCount = 0;
		PlayerPrefs.SetInt ("PlayerCurrentCoins", coinCount); // stores the current coins in the player prefs.

	}

	public void SpendCoins()
	{
		coinCount--; 
		PlayerPrefs.SetInt ("PlayerCurrentCoins", coinCount);// stores the current lives in the player prefs.
	}

	public void BuyCharacter()
	{

		if (coinCount >= 1000) {
			coinCount = coinCount - 1000; 
			PlayerPrefs.SetInt ("PlayerCurrentCoins", coinCount);// stores the current lives in the player prefs.
		}
	}
	}
}