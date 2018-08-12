using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	
	///***********************************************************************
	/// Main GameController Class.
	/// It supports two different modes: "Escape" & "Survival"
	/// in escape mode, user must escape through various mazes.
	/// in survival mode, user must avoid enemyballs reaching the bottom of the screen.
	/// 
	/// This class clones the maze and enemyball objects in the game.
	/// It also manages the difficulty steep of the game, by increasing the movement speed of the elements.
	///***********************************************************************0

	static int gameMode = 0; 				//escape by default
											//index[0] = escape
											//index[1] = survival

	//Difficulty variables
	public static float moveSpeed; 			//Global speed of moving items (mazes)
	public static float cloneInterval; 		//clone maze and enenmyball every N seconds
	
	//leveling vars
	public static int currentLevel = 1;		//Start from easy settings (1 = very easy ---> 10 = very hard)	
	private float levelJump = 10.0f; 		//increase the level every N seconds

	private Vector3 startPoint;				//starting point of the clones object
	private float levelPassedTime;			//passed time since we started the game
	private float levelStartTime;			//time of starting the game
	
	//Gamevver state
	public static bool gameOver;			//Gameover plane object
	private bool gameOverFlag;				//Run the gameover sequence just once
	
	//AudioClips
	public AudioClip levelAdvanceSfx;
	public AudioClip gameoverSfx;
	
	//maze & enemyball creation flag
	private bool createMaze;				//can we clone a new maze?
	private bool createEnemyBall;			//can we clone a new enemyball?

	//maze types
	public GameObject[] maze;				//Array of all available mazes
	public GameObject enemyBall;			//reference to the only enemyball object
	
	//Game finish variables
	public GameObject gameOverPlane;		//reference to gameover plane (activates when we hit a maze)
	public GameObject mainBackground;		//reference to the main background object (to modify its color)
	public GameObject player;				//Reference to main ball object	
	
	
	///***********************************************************************
	/// Init everything here
	///***********************************************************************
	void Awake() {

		gameOverPlane.SetActive(false);				//hide the gameover plane
		mainBackground.GetComponent<Renderer>().material.color = new Color(1, 1, 1);	//set the background color to default
		gameMode = PlayerPrefs.GetInt("GameMode");	//check game mode

		createMaze = true;			//allow maze creation
		createEnemyBall = true;		//allow enemyball creation

		currentLevel = 1;				
		levelPassedTime = 0;
		levelStartTime = 0;
		moveSpeed = 1.2f;
		cloneInterval = 1.0f;
		gameOver = false;
		gameOverFlag = false;
	}
	
	
	///***********************************************************************
	/// Main FSM
	///***********************************************************************
	void Update() {

		//If we have lost the set
		if(gameOver) {

			if(!gameOverFlag) {
				gameOverFlag = true;
				playSfx(gameoverSfx);
				//show gameover menu
				processGameover();
			}

			return;
		}

		//Escape or Survival modes ?
		if(gameMode == 0) {
			//if we are allowed to spawn a maze
			if(createMaze) 
				cloneMaze(); 
		} else {
			//if we are allowed to spawn enemyBall
			if(createEnemyBall) 
				cloneEnemyBall();
		}
		
		//if the game is not yet finished, make it harder and harder by increasing the objects movement speed
		modifyLevelDifficulty();
		
	}
	
	///***********************************************************************
	/// Clone Maze item based on a simple chance factor
	///***********************************************************************
	void cloneMaze() {
		createMaze = false;
		startPoint = new Vector3( Random.Range(-1.0f, 1.0f) , 0.5f, 7);
		Instantiate(maze[Random.Range(0, maze.Length)], startPoint, Quaternion.Euler( new Vector3(0, 0, 0)));	
		StartCoroutine(reactiveMazeCreation());
	}


	///***********************************************************************
	/// Clone a new enemyball object and let them have random sizes
	///***********************************************************************
	void cloneEnemyBall() {
		createEnemyBall = false;
		startPoint = new Vector3( Random.Range(-3.2f, 3.2f) , 0.5f, 7);
		GameObject eb = Instantiate(enemyBall, startPoint, Quaternion.Euler( new Vector3(0, 0, 0))) as GameObject;	
		eb.name = "enemyBall";
		float scaleModifier = Random.Range(-0.4f, 0.1f);
		eb.transform.localScale = new Vector3(eb.transform.localScale.x + scaleModifier,
		                                      eb.transform.localScale.y,
		                                      eb.transform.localScale.z + scaleModifier);
		StartCoroutine(reactiveEnemyBallCreation());
	}


	//enable this controller to be able to clone maze objects again
	IEnumerator reactiveMazeCreation() {
		yield return new WaitForSeconds(cloneInterval);
		createMaze = true;
	}


	//enable this controller to be able to clone enemyball objects again
	IEnumerator reactiveEnemyBallCreation() {
		yield return new WaitForSeconds(0.35f);
		createEnemyBall = true;
	}
	
	
	///***********************************************************************
	/// Here can increase gameSpeed and decrease itemCloneInterval values to 
	/// make the game harder.
	///***********************************************************************
	void modifyLevelDifficulty() {

		levelPassedTime = Time.timeSinceLevelLoad;
		if(levelPassedTime > levelStartTime + levelJump) {

			//increase level difficulty (but limit it to a maximum level of 10)
			if(currentLevel < 10) {

				currentLevel += 1;

				//let the player know what happened to him/her
				playSfx(levelAdvanceSfx);

				//increase difficulty by increasing movement speed
				moveSpeed += 0.6f;

				//clone items faster
				cloneInterval -= 0.18f; //very important!!!
				print ("cloneInterval: " + cloneInterval);
				if(cloneInterval < 0.3f) cloneInterval = 0.3f;

				levelStartTime += levelJump;

				//Background color correction (fade to red)
				float colorCorrection = currentLevel / 10.0f;
				//print("colorCorrection: " + colorCorrection);
				mainBackground.GetComponent<Renderer>().material.color = new Color(1, 
								                                                   1 - colorCorrection, 
								                                                   1 - colorCorrection);
			}
		}
	}
	
	
	///***********************************************************************
	/// Game Over routine
	///***********************************************************************
	void processGameover() {
		gameOverPlane.SetActive(true);
	}


	///***********************************************************************
	/// Play audioclips
	///***********************************************************************
	void playSfx(AudioClip _sfx) {
		GetComponent<AudioSource>().clip = _sfx;
		if(!GetComponent<AudioSource>().isPlaying)
			GetComponent<AudioSource>().Play();
	}	

}