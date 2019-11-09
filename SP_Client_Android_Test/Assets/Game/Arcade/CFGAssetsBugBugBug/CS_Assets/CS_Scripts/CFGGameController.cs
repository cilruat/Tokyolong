#if UNITY_5_3 || UNITY_5_3_OR_NEWER
using UnityEngine.SceneManagement;
#endif

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using CoinFrenzyGame.Types;

namespace CoinFrenzyGame
{
	/// <summary>
	/// This script controls the game, starting it, following game progress, and finishing it with game over.
	/// </summary>
	public class CFGGameController:MonoBehaviour 
	{
		[Header("<Game Area>")]
		[Tooltip("The top limit of the game area")]
		public float topLimit = 6;

		[Tooltip("The bottom limit of the game area")]
		public float bottomLimit = -3;

		[Tooltip("The side limits of the game area, calculated from the edge of the screen")]
		public float sideMargin = 0.5f;
        public Rect gameAreaLimits = new Rect(-6,6,6,-2);

		// The target position the player will move towards
		internal Vector3 targetPosition = Vector3.zero;

		// The ground plain that we use to detect the mouse/tap position
		internal Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

		// Are we using the mouse now?
		internal bool usingMouse = false;

		[Header("<Player Options>")]
		[Tooltip("The player object, assigned from the project and created at the start of the game")]
		public Transform playerObject;

		[Tooltip("The starting position of the player. The player object will be spawned here")]
		public Transform startPosition;

		[Header("<Level Options>")]
		[Tooltip("A list of levels in the game, including the score needed to win the game and the limit of enemies")]
		public Level[] levels;
		
		[Tooltip("The index number of the current level we are on")]
		public int currentLevel = 0;

		// The game will continue forever after the last level
		internal bool isEndless = true;

		[Tooltip("The text that appears before the number of the level. ex: 'LEVEL' 1, 'ROUND' 1, etc")]
		public string levelNamePrefix = "LEVEL ";

		// How many enemies are left in this level, alive
		internal int enemyCount = 0;

		[Tooltip("The maximum number of enemies allowed in this level")]
		internal int enemyLimit = 3;

		[Tooltip("A list of all the enemies spawned in the game")]
		public Spawn[] enemies;
		internal Spawn[] enemyList;
		
		[Tooltip("How may seconds to wait between enemy spawns")]
		public float enemyDelay = 1;
		internal float enemyDelayCount = 0;
		
		[Tooltip("A list of all the items spawned in the game")]
		public Spawn[] items;
		internal Spawn[] itemsList;
		
		[Tooltip("How may seconds to wait between item spawns")]
		public float itemDelay = 1;
		internal float itemDelayCount = 0;

		[Tooltip("A list of powerups that can be activated")]
		public Powerup[] powerups;

		[Tooltip("The score of the game. Score is earned by collecting coins")]
		public float score = 0;
		internal float scoreCount = 0;
		
		[Tooltip("The text object that displays the score, assigned from the scene")]
		public Transform scoreText;
		internal float highScore = 0;
		internal float scoreMultiplier = 1;

		[Tooltip("The effect displayed before starting the game")]
		public Transform readyGoEffect;
		
		[Tooltip("How long to wait before starting gameplay. In this time we usually display the readyGoEffect")]
		public float startDelay = 1;

		// Is the game over?
		internal bool  isGameOver = false;

		[Tooltip("The level of the main menu that can be loaded after the game ends")]
		public string mainMenuLevelName = "CS_StartMenu";
		
		[Tooltip("The keyboard/gamepad button that will restart the game after game over")]
		public string confirmButton = "Submit";
		
		[Tooltip("The keyboard/gamepad button that pauses the game")]
		public string pauseButton = "Cancel";
		internal bool  isPaused = false;

		[Header("<User Interface>")]
		[Tooltip("Various canvases for the UI, assign them from the scene")]
		public Transform gameCanvas;
		public Transform progressCanvas;
		public Transform pauseCanvas;
		public Transform gameOverCanvas;
		public Transform levelUpCanvas;

		[Header("<Sound Options>")]
		[Tooltip("")]
		// Various sounds and their source
		public AudioClip soundLevelUp;
		public AudioClip soundGameOver;
		public string soundSourceTag = "GameController";
		internal GameObject soundSource;

		// A general use index
		internal int index = 0;

		void Awake()
		{
			// Activate the pause canvas early on, so it can detect info about sound volume state
			if ( pauseCanvas )    pauseCanvas.gameObject.SetActive(true);

			// Create the player object and place it at the start position
			if ( playerObject )
			{
				// If we have a start position assigned, place the player at it. Otherwise place the player at its own default position ( as it appears when dragged from the project to the scene hierarchy )
				if ( startPosition )
				{
					playerObject = Instantiate( playerObject, startPosition.position, startPosition.rotation) as Transform;
				}
				else
				{
					playerObject = Instantiate( playerObject) as Transform;
				}
			}

			// Set the target postion to the start position
			targetPosition = startPosition.position;

			playerObject.SendMessage("SetTargetPosition", targetPosition);
		}

		/// <summary>
		/// Start is only called once in the lifetime of the behaviour.
		/// The difference between Awake and Start is that Start is only called if the script instance is enabled.
		/// This allows you to delay any initialization code, until it is really needed.
		/// Awake is always called before any Start functions.
		/// This allows you to order initialization of scripts
		/// </summary>
		void Start()
		{
			// Set the game area limits from all sides, based on topLimit, bottomLimit, and sideMargin
			//gameAreaLimits = new Rect( Camera.main.ScreenToWorldPoint(Vector3.zero).x + sideMargin, topLimit, Camera.main.ScreenToWorldPoint(Vector3.one * Camera.main.pixelWidth).x - sideMargin, bottomLimit);

			//Update the score and enemy count
			UpdateScore();

			//Hide the game over and pause screens
			if ( gameOverCanvas )    gameOverCanvas.gameObject.SetActive(false);
			if ( pauseCanvas )    pauseCanvas.gameObject.SetActive(false);

			//Get the highscore for the player
			#if UNITY_5_3 || UNITY_5_3_OR_NEWER
			highScore = PlayerPrefs.GetFloat(SceneManager.GetActiveScene().name + "HighScore", 0);
			#else
			highScore = PlayerPrefs.GetFloat(Application.loadedLevelName + "HighScore", 0);
			#endif

//CALCULATING ITEM CHANCES
			// Calculate the chances for the objects to spawn
			int totalItems = 0;
			int totalItemsIndex = 0;
			
			// Calculate the total number of items with their chances
			for( index = 0; index < items.Length; index++)
			{
				totalItems += items[index].spawnChance;
			}
			
			// Create a new list of the objects that can be dropped
			itemsList = new Spawn[totalItems];
			
			// Go through the list again and fill out each type of drop based on its drop chance
			for( index = 0; index < items.Length; index++)
			{
				int itemChanceCount = 0;
				
				while( itemChanceCount < items[index].spawnChance )
				{
					itemsList[totalItemsIndex] = items[index];
					
					itemChanceCount++;
					
					totalItemsIndex++;
				}
			}

//CALCULATING SPAWN CHANCES
			// Calculate the chances for the objects to spawn
			int totalEnemies = 0;
			int totalEnemiesIndex = 0;
			
			// Calculate the total number of enemies with their chances
			for( index = 0; index < enemies.Length; index++)
			{
				totalEnemies += enemies[index].spawnChance;
			}
			
			// Create a new list of the objects that can be dropped
			enemyList = new Spawn[totalEnemies];
			
			// Go through the list again and fill out each type of drop based on its drop chance
			for( index = 0; index < enemies.Length; index++)
			{
				int enemyChanceCount = 0;
				
				while( enemyChanceCount < enemies[index].spawnChance )
				{
					enemyList[totalEnemiesIndex] = enemies[index];
					
					enemyChanceCount++;
					
					totalEnemiesIndex++;
				}
			}

			//Go through all the powerups and reset their timers
			for ( index = 0 ; index < powerups.Length ; index++ )
			{
				//Set the maximum duration of the powerup
				powerups[index].durationMax = powerups[index].duration;
				
				//Reset the duration counter
				powerups[index].duration = 0;
				
				//Deactivate the icon of the powerup
				powerups[index].icon.gameObject.SetActive(false);
			}

			//Assign the sound source for easier access
			if ( GameObject.FindGameObjectWithTag(soundSourceTag) )    soundSource = GameObject.FindGameObjectWithTag(soundSourceTag);

			// Check what level we are on
			UpdateLevel();

			// Create the ready?GO! effect
			if ( readyGoEffect )    Instantiate( readyGoEffect );

		}

		/// <summary>
		/// Update is called every frame, if the MonoBehaviour is enabled.
		/// </summary>
		void  Update()
		{
			// Make the score count up to its current value
			if ( scoreCount < score )
			{
				// Count up to the courrent value
				scoreCount = Mathf.Lerp( scoreCount, score, Time.deltaTime * 10);
				
				// Update the score text
				UpdateScore();
			}

			// Delay the start of the game
			if ( startDelay > 0 )
			{
				startDelay -= Time.deltaTime;
			}
			else
			{
				//If the game is over, listen for the Restart and MainMenu buttons
				if ( isGameOver == true )
				{
					//The jump button restarts the game
					if ( Input.GetButtonDown(confirmButton) )
					{
						Restart();
					}
					
					//The pause button goes to the main menu
					if ( Input.GetButtonDown(pauseButton) )
					{
						MainMenu();
					}
				}
				else
				{
					//Toggle pause/unpause in the game
					if ( Input.GetButtonDown(pauseButton) )
					{
						if ( isPaused == true )    Unpause();
						else    Pause(true);
					}

					// If we move the mouse in any direction, then mouse controls take effect
					if ( Input.GetAxisRaw("Mouse X") != 0 || Input.GetAxisRaw("Mouse Y") != 0 || Input.GetMouseButtonDown(0) || Input.touchCount > 0 )    usingMouse = true;

					// If we press gamepad or keyboard arrows, then mouse controls are turned off
					if ( Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0 )    usingMouse = false;

					// Using the mouse for movement
					if ( usingMouse == true )
					{
						// Get the pointer position on the ground when we click/tap
						if ( Input.GetButton("Fire1") && !EventSystem.current.IsPointerOverGameObject() )
						{
							// Cast a ray from the mouse position to the world
							Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
									
							float rayDistance;

							// Find the point on the ground which the mouse points to
							if ( groundPlane.Raycast(ray, out rayDistance) )    targetPosition = ray.GetPoint(rayDistance);
						}
					}
					else if ( playerObject && isPaused == false )
					{
						// Moving right/left
						if ( Input.GetAxisRaw("Horizontal") > 0 )    targetPosition = new Vector3( playerObject.position.x + 0.2f, targetPosition.y, targetPosition.z);
						if ( Input.GetAxisRaw("Horizontal") < 0 )    targetPosition = new Vector3( playerObject.position.x - 0.2f, targetPosition.y, targetPosition.z);

						// Moving up/down
						if ( Input.GetAxisRaw("Vertical") > 0 )    targetPosition = new Vector3( targetPosition.x, targetPosition.y, playerObject.position.z + 0.2f);
						if ( Input.GetAxisRaw("Vertical") < 0 )    targetPosition = new Vector3( targetPosition.x, targetPosition.y, playerObject.position.z - 0.2f);
					}

				// Limit the position of the targetPosition to the edges of the screen
					// Limit movement to the left
					if ( targetPosition.x < gameAreaLimits.x )    targetPosition = new Vector3( gameAreaLimits.x, targetPosition.y, targetPosition.z);

					// Limit movement to the bottom
					if ( targetPosition.z < gameAreaLimits.height )    targetPosition = new Vector3( targetPosition.x, targetPosition.y, gameAreaLimits.height );

					// Limit movement to the right
					if ( targetPosition.x > gameAreaLimits.width )    targetPosition = new Vector3( gameAreaLimits.width, targetPosition.y, targetPosition.z);
					
					// Limit movement to the top
					if ( targetPosition.z > gameAreaLimits.y )    targetPosition = new Vector3( targetPosition.x, targetPosition.y, gameAreaLimits.y);

					// If a player exists, move it to the target position
					if ( playerObject )
					{
						playerObject.SendMessage("SetTargetPosition", targetPosition);
					}

					// Spanwing items
					if ( itemDelayCount > 0 )    itemDelayCount -= Time.deltaTime;
					else 
					{
						// Reset the item delay count
					    itemDelayCount = itemDelay;

						// Spawn an item within the game area
						SpawnObject(itemsList);
					}

					// Spawning enemies
					if ( enemyCount < enemyLimit )
					{
						// Count down to the next object spawn
						if ( enemyDelayCount > 0 )    enemyDelayCount -= Time.deltaTime;
						else 
						{
							// Reset the spawn delay count
							enemyDelayCount = enemyDelay;

							// Spawn an enemy within the game area, and set its movement limits within that area
							SpawnObject(enemyList).SendMessage("SetGameAreaLimits", gameAreaLimits);

							enemyCount++;
						}
					}
				}
			}
		}

		/// <summary>
		/// Creates a new enemy at the end of a random lane 
		/// </summary>
		public Transform SpawnObject( Spawn[] currentSpawnList )
		{
			// Create a new random target from the target list
			Transform newSpawn = Instantiate( currentSpawnList[Mathf.FloorToInt(Random.Range(0,currentSpawnList.Length))].spawnObject ) as Transform;
			
			// Place the target at a random position along the throw height
			newSpawn.position = new Vector3( Random.Range( gameAreaLimits.x, gameAreaLimits.width), 0, Random.Range( gameAreaLimits.y, gameAreaLimits.height));

			return newSpawn;
		}

		/// <summary>
		/// Change the score
		/// </summary>
		/// <param name="changeValue">Change value</param>
		void  ChangeScore( int changeValue )
		{
			score += changeValue * scoreMultiplier;

			//Update the score
			UpdateScore();
		}
		
		/// <summary>
		/// Updates the score value and checks if we got to the next level
		/// </summary>
		void  UpdateScore()
		{
			//Update the score text
			if ( scoreText )    scoreText.GetComponent<Text>().text = Mathf.CeilToInt(scoreCount).ToString();

			//Update the score text
			//if ( scoreText )    scoreText.GetComponent<Text>().text = score.ToString();

			// If we reached the required number of points, level up!
			if ( score >= levels[currentLevel].scoreToNextLevel )
			{
				if ( currentLevel < levels.Length - 1 )    LevelUp();
				//else    if ( isEndless == false )    StartCoroutine(Victory(0));
			}
			
			// Update the progress bar to show how far we are from the next level
			if ( progressCanvas )
			{
				if ( currentLevel == 0 )    progressCanvas.GetComponent<Image>().fillAmount = scoreCount * 1.0f/levels[currentLevel].scoreToNextLevel * 1.0f;
				else    progressCanvas.GetComponent<Image>().fillAmount = (scoreCount - levels[currentLevel - 1].scoreToNextLevel) * 1.0f/(levels[currentLevel].scoreToNextLevel - levels[currentLevel - 1].scoreToNextLevel) * 1.0f;
			}
		}

		/// <summary>
		/// Levels up, and increases the difficulty of the game
		/// </summary>
		void  LevelUp()
		{
			currentLevel++;

			// Reset the enemy delay count
			enemyDelayCount = enemyDelay;

			// Update the level attributes
			UpdateLevel();

			//Run the level up effect, displaying a sound
			LevelUpEffect();
		}

		/// <summary>
		/// Updates the level and sets some values like maximum targets, throw angle, and level text
		/// </summary>
		void UpdateLevel()
		{
			// Display the current level we are on
			if ( progressCanvas )    progressCanvas.Find("Text").GetComponent<Text>().text = (currentLevel + 1).ToString();

			// Set the enemy count based on the current level
			enemyLimit = levels[currentLevel].enemyLimit;
		}

		/// <summary>
		/// Shows the effect associated with leveling up ( a sound and text bubble )
		/// </summary>
		void LevelUpEffect()
		{
			// If a level up effect exists, update it and play its animation
			if ( levelUpCanvas )
			{
				// Update the text of the level
				levelUpCanvas.Find("Text").GetComponent<Text>().text = levelNamePrefix + (currentLevel+1).ToString();

				// Play the level up animation
				if ( levelUpCanvas.GetComponent<Animation>() )    levelUpCanvas.GetComponent<Animation>().Play();
			}

			//If there is a source and a sound, play it from the source
			if ( soundSource && soundLevelUp )    
			{
				soundSource.GetComponent<AudioSource>().pitch = 1;

				soundSource.GetComponent<AudioSource>().PlayOneShot(soundLevelUp);
			}
		}

		/// <summary>
		/// Activates a power up from a list of available power ups
		/// </summary>
		/// <param name="setValue">The index numebr of the powerup to activate</param>
		IEnumerator ActivatePowerup( int powerupIndex )
		{
			//If there is already a similar powerup running, refill its duration timer
			if ( powerups[powerupIndex].duration > 0 )
			{
				//Refil the duration of the powerup to maximum
				powerups[powerupIndex].duration = powerups[powerupIndex].durationMax;
			}
			else //Otherwise, activate the power up functions
			{
				//Activate the powerup icon
				if ( powerups[powerupIndex].icon )    powerups[powerupIndex].icon.gameObject.SetActive(true);

				//Run up to two start functions from the gamecontroller
				if ( powerups[powerupIndex].startFunction != string.Empty )    SendMessage(powerups[powerupIndex].startFunction, powerups[powerupIndex].startParamater);
				
				//Fill the duration timer to maximum
				powerups[powerupIndex].duration = powerups[powerupIndex].durationMax;
				
				//Count down the duration of the powerup
				while ( powerups[powerupIndex].duration > 0 )
				{
					yield return new WaitForSeconds(Time.deltaTime);
					
					powerups[powerupIndex].duration -= Time.deltaTime;
					
					//Animate the powerup timer graphic using fill amount
					if ( powerups[powerupIndex].icon )    powerups[powerupIndex].icon.GetComponent<Image>().fillAmount = powerups[powerupIndex].duration/powerups[powerupIndex].durationMax;
				}
				
				//Run up to two end functions from the gamecontroller
				if ( powerups[powerupIndex].endFunction != string.Empty )    SendMessage(powerups[powerupIndex].endFunction, powerups[powerupIndex].endParamater);
				
				//Deactivate the powerup icon
				if ( powerups[powerupIndex].icon )    powerups[powerupIndex].icon.gameObject.SetActive(false);
			}
		}

		/// <summary>
		/// Sends a SetSpeedMultiplier command to the player, which makes it either faster or slower
		/// </summary>
		void SetSpeedMultiplier( float setValue )
		{
			if ( playerObject )    playerObject.SendMessage("SetSpeedMultiplier", setValue);
		}
		
		/// <summary>
		/// Set the score multiplier
		/// </summary>
		void SetScoreMultiplier( int setValue )
		{
			// Set the score multiplier
			scoreMultiplier = setValue;
		}

		/// <summary>
		/// Sends a rescale command to the player
		/// </summary>
		void RescalePlayer( float targetScale )
		{
			if ( playerObject )    playerObject.SendMessage("Rescale", targetScale);
		}

		/// <summary>
		/// Sends a ToggleImmortal command to the player, which makes it either unkillable or killable
		/// </summary>
		void ToggleImmortal()
		{
			if ( playerObject )    playerObject.SendMessage("ToggleImmortal");
		}

		/// <summary>
		/// Sends a ToggleKiller command to the player, which makes it either able to kill enemies, or not
		/// </summary>
		void ToggleKiller()
		{
			if ( playerObject )    playerObject.SendMessage("ToggleKiller");
		}

		/// <summary>
		/// Sends a SetGiant command to the player. This makes the player larger, immortal, and can kill enemies
		/// </summary>
		///GIANT DOESN'T WORK CORRECTLY YET. IT WILL BE ADDED IN ANOTHER UPDATE
		void SetGiant( float targetScale )
		{
			RescalePlayer(targetScale);

			ToggleImmortal();

			ToggleKiller();
		}




		/// <summary>
		/// Shuffles the specified text list, and returns it
		/// </summary>
		/// <param name="texts">A list of texts</param>
		float[] Shuffle( float[] positions )
		{
			// Go through all the positions and shuffle them
			for ( index = 0 ; index < positions.Length ; index++ )
			{
				// Hold the text in a temporary variable
				float tempNumber = positions[index];
				
				// Choose a random index from the text list
				int randomIndex = UnityEngine.Random.Range( index, positions.Length);
				
				// Assign a random text from the list
				positions[index] = positions[randomIndex];
				
				// Assign the temporary text to the random question we chose
				positions[randomIndex] = tempNumber;
			}
			
			return positions;
		}

		/// <summary>
		/// Pause the game, and shows the pause menu
		/// </summary>
		/// <param name="showMenu">If set to <c>true</c> show menu.</param>
		public void  Pause( bool showMenu )
		{
			isPaused = true;
			
			//Set timescale to 0, preventing anything from moving
			Time.timeScale = 0;
			
			//Show the pause screen and hide the game screen
			if ( showMenu == true )
			{
				if ( pauseCanvas )    pauseCanvas.gameObject.SetActive(true);
				if ( gameCanvas )    gameCanvas.gameObject.SetActive(false);
			}
		}
		
		/// <summary>
		/// Resume the game
		/// </summary>
		public void  Unpause()
		{
			isPaused = false;
			
			//Set timescale back to the current game speed
			Time.timeScale = 1;
			
			//Hide the pause screen and show the game screen
			if ( pauseCanvas )    pauseCanvas.gameObject.SetActive(false);
			if ( gameCanvas )    gameCanvas.gameObject.SetActive(true);
		}

		/// <summary>
		/// Runs the game over event and shows the game over screen
		/// </summary>
		IEnumerator GameOver(float delay)
		{
			isGameOver = true;

			//Go through all the powerups and nullify their timers, making them end
			for ( index = 0 ; index < powerups.Length ; index++ )
			{
				//Set the duration of the powerup to 0
				powerups[index].duration = 0;
			}

			yield return new WaitForSeconds(delay);
			
			//Remove the pause and game screens
			if ( pauseCanvas )    Destroy(pauseCanvas.gameObject);
			if ( gameCanvas )    Destroy(gameCanvas.gameObject);
			
			//Show the game over screen
			if ( gameOverCanvas )    
			{
				//Show the game over screen
				gameOverCanvas.gameObject.SetActive(true);
				
				//Write the score text
				gameOverCanvas.Find("Base/Texts/TextScore").GetComponent<Text>().text = "SCORE " + score.ToString();
				
				//Check if we got a high score
				if ( score > highScore )    
				{
					highScore = score;
					
					//Register the new high score
					#if UNITY_5_3 || UNITY_5_3_OR_NEWER
					PlayerPrefs.SetFloat(SceneManager.GetActiveScene().name + "HighScore", score);
					#else
					PlayerPrefs.SetFloat(Application.loadedLevelName + "HighScore", score);
					#endif
				}
				
				//Write the high sscore text
				gameOverCanvas.Find("Base/Texts/TextHighScore").GetComponent<Text>().text = "HIGH SCORE " + highScore.ToString();

				//If there is a source and a sound, play it from the source
				if ( soundSource && soundGameOver )    
				{
					soundSource.GetComponent<AudioSource>().pitch = 1;
					
					soundSource.GetComponent<AudioSource>().PlayOneShot(soundGameOver);
				}
			}
		}
		
		/// <summary>
		/// Restart the current level
		/// </summary>
		void  Restart()
		{
			#if UNITY_5_3 || UNITY_5_3_OR_NEWER
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
			#else
			Application.LoadLevel(Application.loadedLevelName);
			#endif
		}
		
		/// <summary>
		/// Restart the current level
		/// </summary>
		void  MainMenu()
		{
			#if UNITY_5_3 || UNITY_5_3_OR_NEWER
			SceneManager.LoadScene(mainMenuLevelName);
			#else
			Application.LoadLevel(mainMenuLevelName);
			#endif
		}

		void OnDrawGizmos()
		{
			Gizmos.color = Color.green;

			// Set the game area limits from all sides, based on topLimit, bottomLimit, and sideMargin
			//gameAreaLimits = new Rect( Camera.main.ScreenToWorldPoint(Vector3.zero).x + sideMargin, topLimit, Camera.main.ScreenToWorldPoint(Vector3.one * Camera.main.pixelWidth).x - sideMargin, bottomLimit);

			// Top line
			Gizmos.DrawLine( new Vector3(gameAreaLimits.x, 0, gameAreaLimits.y), new Vector3(gameAreaLimits.width, 0, gameAreaLimits.y));

			// Right line
			Gizmos.DrawLine( new Vector3(gameAreaLimits.width, 0, gameAreaLimits.y), new Vector3(gameAreaLimits.width, 0, gameAreaLimits.height));

			// Bottom line
			Gizmos.DrawLine( new Vector3(gameAreaLimits.width, 0, gameAreaLimits.height), new Vector3(gameAreaLimits.x, 0, gameAreaLimits.height));

			// Left line
			Gizmos.DrawLine( new Vector3(gameAreaLimits.x, 0, gameAreaLimits.height), new Vector3(gameAreaLimits.x, 0, gameAreaLimits.y));

		}
	}
}