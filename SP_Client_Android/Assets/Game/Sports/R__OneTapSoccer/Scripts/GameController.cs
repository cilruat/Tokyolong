using UnityEngine;
using System.Collections;

namespace OnetapSoccer
{
public class GameController : MonoBehaviour {

	/// <summary>
	/// Main game controller class.
	/// Responsible for keeping track of player shoots, goals and streaks, creating new poles, 
	/// changing game's difficulty when player passes a certain amount of goal, and updating information in the UI.
	/// </summary>

	public static int playerGoals;						//total goals by player
	public static int playerStartingBalls = 10;			//total starting balls for the player
	public static int playerCurrentBalls;				//player current available balls to shoot
	public static int playerGoalStreak;						//counter for consecutive goals 

	public static float rotationSpeedBooster = 1.0f;	//to make the game harder, we need to increase the arrow's rotation speed
														//each time player scores a goal

	public static bool gameIsFinished = false;			//global flag to finish the game
	private bool gameFinishFlag = false;				//private flag

	public GameObject[] availablePoles;					//available poles to create as obstacle

	public AudioClip startWistle;						//Audioclip
	public AudioClip endWistle;							//Audioclip

	//Reference to important game objects used inside the game
	public GameObject hudPlayerGoals;
	public GameObject hudPlayerAvailableBalls;
	public GameObject hudPlayerRoundScore;
	public GameObject hudPlayerBestScore;
	public GameObject hudTapToShootHelper;
	public GameObject ball;
	public GameObject GameFinishPlane;

	private bool isFirstRun = true;						//flag to initialize the game
	private int playerBestScore;						//fetch from playerprefs
	private GameObject AdManagerObject;


	/// <summary>
	/// Init
	/// </summary>
	void Awake () {

		//debug
		//PlayerPrefs.DeleteAll();
		
		gameIsFinished = false;
		gameFinishFlag = false;
		playerGoals = 0;
		playerGoalStreak = 0;
		playerCurrentBalls = playerStartingBalls;
		rotationSpeedBooster = 1.0f;
		playerBestScore = PlayerPrefs.GetInt("BestScore");
//		AdManagerObject = GameObject.FindGameObjectWithTag("AdManager");

		init();

		if(GameFinishPlane)
			GameFinishPlane.SetActive(false);
	}


	/// <summary>
	/// setup the game for the first run
	/// </summary>
	void init() {
		//if this is the first run, init and stop the physics for a bit
		if(isFirstRun) {
			//start the game
			isFirstRun = false;
			//play the start wistle
			playSfx(startWistle);
			//show helper text
			hudTapToShootHelper.SetActive (true);
		}
	}


	/// <summary>
	/// FSM
	/// </summary>
	void Update () {

		//if the game is running, update information on UI (hud board)
		if(!gameIsFinished)
			manageGoals();
		
		if (playerCurrentBalls <= 0)
			gameIsFinished = true;

		//increase game's difficulty by increasing shoot arrow's rotation speed
		rotationSpeedBooster = (playerGoals * 0.05f) + 1;

		//Finish the game when we have played all available balls
		if(gameIsFinished && !gameFinishFlag) {
			StartCoroutine(gameOver ());
		}

		if(Input.GetKeyUp(KeyCode.Space) || Input.GetMouseButtonUp (0)) {
			//hide helper text
			if(hudTapToShootHelper)
				hudTapToShootHelper.SetActive (false);
		}

	}


	/// <summary>
	/// This function will be called when the game is finished. (player shoot all his balls)
	/// </summary>
	IEnumerator gameOver() {

		if (gameFinishFlag)
			yield break;
		gameFinishFlag = true;

		//wait a little
		yield return new WaitForSeconds (1.0f);

		//play end wistle
		playSfx(endWistle);

		//show gamefinish plane
		GameFinishPlane.SetActive(true); 

		//Save new scores
		if (playerGoals > playerBestScore) {
			PlayerPrefs.SetInt ("BestScore", playerGoals);
			playerBestScore = playerGoals;
		}

		//update UI information
		hudPlayerRoundScore.GetComponent<TextMesh>().text = playerGoals.ToString();
		hudPlayerBestScore.GetComponent<TextMesh> ().text = playerBestScore.ToString ();

		//Optional - show an Interstitial Ad when the game is finished
		if(AdManagerObject)
			AdManagerObject.GetComponent<AdManager>().showInterstitial();
	}


	/// <summary>
	/// Create new poles for each round of shooting.
	/// As the player scores more goals, we create harder poles (narrow passage and closer to player)
	/// </summary>
	public void createPole() {

		float posX = 0;
		float posY = 0;
		int poleType = 0;

		if (playerGoals < 5) {
			
			posX = Random.Range (0, 3);
			posY = Random.Range (-1, 3);
			poleType = 0;

		} else if (playerGoals >= 5 && playerGoals < 10) {

			posX = Random.Range (-2, 1.5f);
			posY = Random.Range (0, 4);
			poleType = 1;

		} else if (playerGoals >= 10) {

			posX = Random.Range (-2.5f, 1);
			posY = Random.Range (-1, 4);
			poleType = 2;

		}
			
		//create a pole
		GameObject pole = Instantiate (availablePoles[poleType], new Vector3(posX, posY, -0.6f), Quaternion.Euler (0, 0, 0)) as GameObject;
		pole.name = "Pole";
		pole.tag = "Pole";

		//debug
		//print("New pole created!");
	}
		

	/// <summary>
	/// show goals/availableBalls on the UI
	/// </summary>
	void manageGoals() {
		hudPlayerGoals.GetComponent<TextMesh>().text = playerGoals.ToString();
		hudPlayerAvailableBalls.GetComponent<TextMesh>().text = playerCurrentBalls.ToString();
	}
		

	//*****************************************************************************
	// Play sound clips
	//*****************************************************************************
	void playSfx ( AudioClip _clip  ){
		GetComponent<AudioSource>().clip = _clip;
		if(!GetComponent<AudioSource>().isPlaying) {
			GetComponent<AudioSource>().Play();
		}
	}
}
}