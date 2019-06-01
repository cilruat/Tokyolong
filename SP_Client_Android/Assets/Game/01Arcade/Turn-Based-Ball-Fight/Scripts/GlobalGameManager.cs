using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
namespace Turnbase
{
public class GlobalGameManager : MonoBehaviour {

	/// <summary>
	/// This is the main game manager class. It's responsible to calculate rounds,
	/// assign turns to player or AI, keeping track of time and score, and
	/// showing required information on screen.
	/// </summary>
		
	public GameObject playerBall;				//playerBall prefab
	public GameObject opponentBall;				//opponentBall prefab
	
	public static int level;					//We have 4 level in this kit. each level contains 4 sets.
												//after players beat 4 sets in a level, they advance a level
												//and they start the new level with one ball less.
	public static int round;					//internal counter to assign turn to player and AI
	public static int setCoef;					//score modifier for each set
	public static int playerRemainingBall;		//player's remaining balls
	public static int opponentRemainingBall;	//opponent's remaining balls
	public static bool playersTurn;				//flag to check if this is player's turn
	public static bool opponentsTurn;			//flag to check if this is opponent's turn
	public static bool gameIsFinished;			//global flag for game finish state
	private bool endgameFlag = false;			//private flag to run the finish sequence just once
	public static float cooldownDelay = 1.0f;	//delay required before gameManager change the turns
	public static int score;					//player's score
	public static string whosTurn;				//current turn holder in string. useful if you want to extend the kit.


	//reference to UI elements
	public 	GameObject uiScore;					
	public GameObject uiLevel;
	public GameObject uiStatus;
	public GameObject GameFinishPlane;
	public GameObject uiHint;


	//reference to scene buttons
	public GameObject uiBtnNext;
	public GameObject uiBtnQuit;
	public GameObject uiBtnAgain;


	//audio clips
	public AudioClip winSfx;
	public AudioClip loseSfx;


	public static int progress;					//we have a total of 16 sets in this kit. when player starts
												//the game for the first time, the progress will be set on 0.
												//by beating each set, they get an increase on the progress
												//and see thir progress on screen (the yellow dots and the cup)
												//Range: 0 ~ 15
	public GameObject[] progressIndicators;		//reference to all yellow dots which shows player's progress
	public Material[] statusMat;				//two different material for setting indicators on or off


	private int playerBalls;					//total number of player balls
	private int opponentBalls;					//total number of opponent balls
	private int playerLine;						//player balls line on the board (we have 4 lines in this game)
	private int opponentLine;					//opponent balls line on the board


	/// <summary>
	/// Init
	/// </summary>
	void Awake() {	

		round = 1;
		gameIsFinished = false;
		endgameFlag = false;
		score = 0;

		//set unwanted UI elements off
		GameFinishPlane.SetActive(false);
		uiHint.SetActive(false);
		uiBtnAgain.SetActive(false);

		initLevel();
		initProgress();
	}


	/// <summary>
	/// Checks player progress and previous status, and creates the new level (on the fly)
	/// </summary>
	void initLevel() {

		//if this is the first time we want to play, load the level with no saved value
		if(PlayerPrefs.GetInt("wasPlaying") == 0) {
			
			//init the vars
			level = 1;
			progress = 0;
			playerBalls = 8;
			opponentBalls = 8;
			playerLine = 1;
			opponentLine = 1;
			
			//save player settings in playerprefs
			PlayerPrefs.SetInt("playerBalls", playerBalls);
			PlayerPrefs.SetInt("opponentBalls", opponentBalls);
			PlayerPrefs.SetInt("playerLine", playerLine);
			PlayerPrefs.SetInt("opponentLine", opponentLine);
			PlayerPrefs.SetInt("level", 1);
			PlayerPrefs.SetInt("score", 0);
			PlayerPrefs.SetInt("progress", 0);
			
			//setup the level
			createLevel(playerBalls, playerLine, opponentBalls, opponentLine);
			PlayerPrefs.SetInt("wasPlaying", 1);
		
		} else {
		
			//if we are in the middle of game, load our settings and progress from playerprefs 
			playerBalls = PlayerPrefs.GetInt("playerBalls");
			opponentBalls = PlayerPrefs.GetInt("opponentBalls");
			playerLine = PlayerPrefs.GetInt("playerLine");
			opponentLine = PlayerPrefs.GetInt("opponentLine");
			level = PlayerPrefs.GetInt("level");		
			score = PlayerPrefs.GetInt("score");
			progress = PlayerPrefs.GetInt("progress");
			
			//if previous set has been won
			if(PlayerPrefs.GetString("lastStatus") == "setWon") {
				PlayerPrefs.SetInt("playerBalls", playerBalls - 1);
				PlayerPrefs.SetInt("opponentBalls", opponentBalls);
				PlayerPrefs.SetInt("playerLine", playerLine + 1);
				PlayerPrefs.SetInt("opponentLine", opponentLine);
			}
				
			//if previous set has been lost
			if(PlayerPrefs.GetString("lastStatus") == "setLost") {
				PlayerPrefs.SetInt("playerBalls", playerBalls);
				PlayerPrefs.SetInt("opponentBalls", opponentBalls - 1);
				PlayerPrefs.SetInt("playerLine", playerLine);
				PlayerPrefs.SetInt("opponentLine", opponentLine + 1);
			}
			
			//if previous set has been won, and we also won 4 sets, we must advance a Level
			//we set every element to their starting position, but we give one less ball to the player
			if(PlayerPrefs.GetString("lastStatus") == "levelWon") {
				PlayerPrefs.SetInt("playerBalls", 8 - level + 1);	//level starts from 1, not 0.
				PlayerPrefs.SetInt("opponentBalls", 8);
				PlayerPrefs.SetInt("playerLine", 1);
				PlayerPrefs.SetInt("opponentLine", 1);
			}

			//this is very important to save the progress settings again to avoid having unsaved 
			//variable in the above conditions. 
			playerBalls = PlayerPrefs.GetInt("playerBalls");
			opponentBalls = PlayerPrefs.GetInt("opponentBalls");
			playerLine = PlayerPrefs.GetInt("playerLine");
			opponentLine = PlayerPrefs.GetInt("opponentLine");

			createLevel(playerBalls, playerLine, opponentBalls, opponentLine);
		}
	}


	/// <summary>
	/// turn the yellow dots on/off to show player's progress
	/// </summary>
	void initProgress() {
		for(int i = 0; i < 16; i++) {
			if(progress > i)
				progressIndicators[i].GetComponent<Renderer>().material = statusMat[1];
			else
				progressIndicators[i].GetComponent<Renderer>().material = statusMat[0];
		}
	}


	void Start() {
		roundTurnManager();	//assign turns to player or AI
	}
		

	/// <summary>
	/// player balls (1~8) - the smaller, the harder the game
	/// player starting line (1~4) - the larger, the harder 
	/// opponent balls (1~8) - the larger, the harder the game
	/// opponent starting line (1~4) - the larger, the harder the game
	/// </summary>
	void createLevel(int _playerUnits, int _playerLine, int _opponentUnits, int _opponentLine) {

		//set level score multiplier
		setCoef = (10 * (8 - _playerUnits)) + (10 * level);
		
		//create player units
		for(int i = 0; i < _playerUnits; i++) {

			//space between player balls
			float playerOffsetX = (8 - _playerUnits) * 1.1f;

			//position of each player balls on the board. this formula checks player balls count and 
			//position them accordingly, se they always get positioned on the center of the board.
			Vector3 playerPos = new Vector3( (-7.7f + (i * 2.18f)) + playerOffsetX, 1, -7.7f + ((_playerLine - 1) * 2.2f));

			//create player balls
			GameObject playerBallInGame = Instantiate(playerBall, playerPos, Quaternion.Euler(0,0,0)) as GameObject;

			//Rename them to something developer friendly
			playerBallInGame.name = "PlayerBall-" + (i + 1).ToString();
		}
		
		//create opponent units
		for(int j = 0; j < _opponentUnits; j++) {
			float opponentOffsetX = (8 - _opponentUnits) * 1.1f;
			Vector3 opponentPos = new Vector3( (-7.7f + (j * 2.18f)) + opponentOffsetX, 1, 7.8f - ((_opponentLine - 1) * 2.2f));
			GameObject OpponentBallInGame = Instantiate(opponentBall, opponentPos, Quaternion.Euler(0,0,0)) as GameObject;
			OpponentBallInGame.name = "OpponentBall-" + (j + 1).ToString();
		}
	}


	/// <summary>
	/// FSM
	/// </summary>
	void Update() {

		//check if game is finished every frame!
		if(gameIsFinished) {
			if(!endgameFlag)
				endGame();
		}

		//show player's score and level on screen
		uiScore.GetComponent<TextMesh>().text = score.ToString();
		uiLevel.GetComponent<TextMesh>().text = level.ToString();

		//debug restart - just for developers - should not be used in a real game
		if(Input.GetKey(KeyCode.R)) {
			SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
		}

		//debug restart with a reset function that totally deletes player's progress
		if(Input.GetKey(KeyCode.T)) {
			PlayerPrefs.DeleteAll();
			SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
		}
	}


	/// <summary>
	/// Assign turns to player and AI.
	/// </summary>
	void roundTurnManager() {

		if(gameIsFinished)
			return;

		//if round carry is odd, its players turn, otherwise its opponent's turn
		int carry;
		carry = round % 2;

		if(carry == 1) {
			playersTurn = true;
			opponentsTurn = false;
			PlayerController.canShoot = true;
			OpponentAI.canShoot = false;
			whosTurn = "Player's Turn...";
		} else {
			playersTurn = false;
			opponentsTurn = true;
			PlayerController.canShoot = false;
			OpponentAI.canShoot = true;
			whosTurn = "Opponent's Turn...";
		}
	}
		

	/// <summary>
	/// This function will be called by other controller. It checks game condition after each shoot
	/// and decides if them should continue or get finished.
	/// </summary>
	public IEnumerator managePostShoot( string _shootBy) {

		print("Shoot event received! By: " + _shootBy);

		//avoid using small values here, to prevent null reference bugs
		yield return new WaitForSeconds(cooldownDelay);	
			
		//get remaining balls of each side
		opponentRemainingBall = GameObject.FindGameObjectsWithTag("Opponent").Length;
		playerRemainingBall = GameObject.FindGameObjectsWithTag("Player").Length;
		
		print("Player Units: " + playerRemainingBall + " --- Opponent Units: " + opponentRemainingBall);

		//check if game is finished?
		if(opponentRemainingBall == 0) {
			finishGame(1);
			yield break;
		} else if(playerRemainingBall == 0) {
			finishGame(0);
			yield break;
		}

		//change the turn
		switch(_shootBy) {
			case "Player":
				round = 2;
				break;
			
			case "Opponent":
				round = 1;
				break;
		}
		
		roundTurnManager();
	}


	/// <summary>
	/// Finish game routine.
	/// if this a normal set win, we show the regular GameFinishPlane object with "Quit" and "Next" buttons.
	/// but if this is a gameOver (by losing or by beating all 16 sets), we have to just active "Again" button.
	/// </summary>
	void finishGame(int _state) {

		print("Game is finished");

		switch(_state) {
			case 0:
				playSfx(loseSfx);
				
				//if player has lost 4 sets, the game is over!
				//when player loses a set, opponent starts with one less ball, and also an advance in line to ease the situation for player.
				if(opponentLine == 4) {
					print("Game Over...");
					gameIsFinished = true;
					return;
				}
				
				print("You lost the set!!");
				nextLevel(playerBalls, playerLine, opponentBalls, opponentLine, "setLost");
				break;
				
			case 1:
				print("You won the set!");
				playSfx(winSfx);
				PlayerPrefs.SetInt("progress", PlayerPrefs.GetInt("progress") + 1);
				nextLevel(playerBalls, playerLine, opponentBalls, opponentLine, "setWon");
				break;
		}
	}


	/// <summary>
	/// create next set based on information from the current set.
	/// </summary>
	void nextLevel(int _pBalls, int _pLine, int _oBalls, int _oLine, string _setStatus) {

		PlayerPrefs.SetInt("playerBalls", _pBalls);
		PlayerPrefs.SetInt("opponentBalls", _oBalls);
		PlayerPrefs.SetInt("playerLine", _pLine);
		PlayerPrefs.SetInt("opponentLine", _oLine);	
		PlayerPrefs.SetInt("score", score);	
		
		GameFinishPlane.SetActive(true);	//show the default GameFinishPlane with "Next" and "Quit" buttons
		
		//if player has beat 4 set, move to the next level
		if(_pLine == 4 && _setStatus == "setWon") {
		
			//check final gameWon status
			if(level == 4) {
				gameIsFinished = true;
				return;
			}
			
			PlayerPrefs.SetString("lastStatus", "levelWon");
			PlayerPrefs.SetInt("level", PlayerPrefs.GetInt("level") + 1);
			uiHint.SetActive(true);
			uiStatus.GetComponent<TextMesh>().text = "Level Won";
			
		} else {
			PlayerPrefs.SetString("lastStatus", _setStatus);
			uiStatus.GetComponent<TextMesh>().text = _setStatus;
		}
	}


	/// <summary>
	/// Show gameFinishPlane on screen with required buttons.
	/// </summary>
	void endGame() {

		endgameFlag = true;
		GameFinishPlane.SetActive(true);
		uiHint.SetActive(false);
		uiBtnAgain.SetActive(true);
		uiBtnQuit.SetActive(false);
		uiBtnNext.SetActive(false);

		//if player has won all 16 sets, game is finished!
		if(PlayerPrefs.GetString("lastStatus") == "setWon" || PlayerPrefs.GetString("lastStatus") == "levelWon")
			uiStatus.GetComponent<TextMesh>().text = "You are the Champion !";	
		else
			uiStatus.GetComponent<TextMesh>().text = "Game Over";
	}


	/// <summary>
	/// Play sfx
	/// </summary>
	/// <param name="_sfx">Audioclip</param>
	void playSfx ( AudioClip _sfx  ){
		GetComponent<AudioSource>().clip = _sfx;
		if(!GetComponent<AudioSource>().isPlaying)
			GetComponent<AudioSource>().Play();
	}
	}
}