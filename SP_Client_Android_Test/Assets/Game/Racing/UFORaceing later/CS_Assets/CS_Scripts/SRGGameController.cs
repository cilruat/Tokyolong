using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using SpeederRunGame.Types;

namespace SpeederRunGame
{
	/// <summary>
	/// This script controls the game, starting it, following game progress, and finishing it with game over.
	/// </summary>
	public class SRGGameController:MonoBehaviour 
	{
		[Header("<Player Options>")]
		[Tooltip("The player object, assigned from the project and created at the start of the game")]
		public Transform playerObject;

		[Tooltip("The player controller, which handles movement and rotation through the tube. It holds the player object inside it")]
		public Transform playerController;

		[Tooltip("The movement speed of the player controller")]
		public float moveSpeed = 0;

		[Tooltip("The target speed the player controller is trying to achieve")]
		public float targetSpeed = 5;
		[Tooltip("The movement speed of the player controller")]
		public float rotateSpeed = 50;

        [Tooltip("How steep should the player tilt when rotating. This is the visual effect you see when moving left/right")]
        public float tiltRange = 30;

		[Tooltip("The effect that gives an illusion of high speed. This is inside the player controller object")]
		public ParticleSystem highSpeedEffect;
		
		[Tooltip("Which speed should the player reach before showing the high speed effect")]
		public float highSpeedAt = 40;

		[Header("<Level Options>")]
		[Tooltip("The tube through which the player moves. The tube material is animated to give an illusion of movement")]
		public Transform tubeObject;
		
		// The ratio between the scale of the tube object and its tiling. This is used to calculate how fast the tile offset should move to make it look as if the player is moving in the tube.
		internal Vector2 offsetRatio;
		
		[Tooltip("The index number of the current level we are on")]
		public int currentLevel = 0;

		[Tooltip("A list of all the sections spawned in the game")]
		public Spawn[] sections;
		internal Spawn[] sectionsList;

		[Tooltip("The distance between each two wall sections")]
		public float sectionGap = 10;
		internal float currentGap;

		[Tooltip("The ratio between the current speed of the player and the distance between wall sections. This is used to control the game difficulty by making the gap smaller and smaller as we progress, but still taking into account the fact that we are moving faster and need more time to react")]
		public float gapToSpeedRatio = 0.5f;

		[Tooltip("The minimum allowed distance between two wall sections. This is to make sure there are no walls too close to each other that make it impossible to pass")]
		public float minimumGap = 10;
        
        [Tooltip("How many points we need in order to level up. When leveling up the speed and gap increase")]
		public int scoreToLevelUp = 500;
		internal int levelUpCount = 0;

		[Tooltip("How much the speed increases when we level up")]
		public float speedIncrease = 3;

		[Tooltip("The position of the first section in the tube")]
		public float sectionPosition = 20;

		[Tooltip("The score of the game. Score is earned by collecting coins")]
		public float score = 0;
		internal float scoreCount = 0;
		
		[Tooltip("The text object that displays the score, assigned from the scene")]
		public Transform scoreText;
		internal float highScore = 0;
		internal float scoreMultiplier = 1;

		[Tooltip("The distance of the player. How far it moved along the tube")]
		internal float distance = 0;
		public Transform distanceText;

		// Is the game over?
		internal bool  isGameOver = false;

		[Tooltip("The level of the main menu that can be loaded after the game ends")]
		public string mainMenuLevelName = "CS_StartMenu";
		
		[Tooltip("The keyboard/gamepad button that will restart the game after game over")]
		public string confirmButton = "Submit";
		
		[Tooltip("The keyboard/gamepad button that pauses the game")]
		public string pauseButton = "Cancel";
		internal bool  isPaused = false;

		[Header("<Control Options>")]
		[Tooltip("How sensitive the rotation is when using touch controls")]
		public float touchSensitivity = 1;
		
		[Tooltip("Should the camera rotate to follow the player movement?")]
		public bool rotateCamera = false;

		[Tooltip("Should we use a stiff input for rotating? This makes the player move immediately left/right instead of easing into the motion")]
		public bool stiffKeyboardInput = false;

		// The direction in which we are rotating. Limited between -1 and 1 ( left to right )
		internal float rotateDirection = 0;
		
		// We are using touch controls now
		internal bool usingTouchControls = false;
		
		// We are using gamepad/keyboard controls now
		internal bool usingGamepad = false;

		[Tooltip("If on mobile, should we use tilt controls instead of touch controls?")]
		public bool tiltControls = false;

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
			if ( playerController )
			{
				// Create the player object
				playerObject = Instantiate( playerObject) as Transform;

				// Set the player controller start position as the parent of the player
				playerObject.SetParent(playerController.Find("StartPosition"));

				// Place the player at the position of the start point
				playerObject.localPosition = Vector3.zero;
			}
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
			// Check if we are not running on a mobile device. If so, remove any mobile specific controls
			if ( Application.platform != RuntimePlatform.IPhonePlayer && Application.platform != RuntimePlatform.Android )    
			{
				tiltControls = false;
			}

			//Update the score
			UpdateScore();

			//Hide the game over and pause screens
			if ( gameOverCanvas )    gameOverCanvas.gameObject.SetActive(false);
			if ( pauseCanvas )    pauseCanvas.gameObject.SetActive(false);

			//Get the highscore for the player
			highScore = PlayerPrefs.GetFloat(SceneManager.GetActiveScene().name + "HighScore", 0);

//CALCULATING section CHANCES
			// Calculate the chances for the objects to spawn
			int totalsections = 0;
			int totalsectionsIndex = 0;
			
			// Calculate the total number of sections with their chances
			for( index = 0; index < sections.Length; index++)
			{
				totalsections += sections[index].spawnChance;
			}
			
			// Create a new list of the objects that can be dropped
			sectionsList = new Spawn[totalsections];
			
			// Go through the list again and fill out each type of drop based on its drop chance
			for( index = 0; index < sections.Length; index++)
			{
				int sectionChanceCount = 0;
				
				while( sectionChanceCount < sections[index].spawnChance )
				{
					sectionsList[totalsectionsIndex] = sections[index];
					
					sectionChanceCount++;
					
					totalsectionsIndex++;
				}
			}

			//Assign the sound source for easier access
			if ( GameObject.FindGameObjectWithTag(soundSourceTag) )    soundSource = GameObject.FindGameObjectWithTag(soundSourceTag);

			// Check what level we are on
			UpdateLevel();

			// Start the level up effect
			LevelUpEffect();

			// Calculate the ratio between the water object's scale and its tiling. This is used to set the offset speed of the texture
			if ( tubeObject )    offsetRatio = new Vector2( -tubeObject.GetComponent<Renderer>().material.mainTextureScale.x/tubeObject.localScale.x, -tubeObject.GetComponent<Renderer>().material.mainTextureScale.y/tubeObject.localScale.z);

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

			//If the game is over, listen for the Restart and MainMenu buttons
			if ( isGameOver == true )
			{
				if ( highSpeedEffect.isPlaying == true )    highSpeedEffect.Stop();

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
				//Keep track of the distance
				distance = playerController.position.z;
				
				//Update the distance text
				if ( distanceText )    distanceText.GetComponent<Text>().text = Mathf.CeilToInt(distance ).ToString();

				//Toggle pause/unpause in the game
				if ( Input.GetButtonDown(pauseButton) )
				{
					if ( isPaused == true )    Unpause();
					else    Pause(true);
				}

				if ( isPaused == false )
				{
					// The high speed effect that appears when the player is moving very fast
					if ( highSpeedEffect )
					{
						// Appear when we are moving fast
						if ( moveSpeed > highSpeedAt )
						{
							// Place the high speed effect at the center of the screen
							highSpeedEffect.transform.eulerAngles = new Vector3( highSpeedEffect.transform.eulerAngles.x, highSpeedEffect.transform.eulerAngles.y, 0);

							// Play the particle effect
							if ( highSpeedEffect.isPlaying == false )    highSpeedEffect.Play();
						}
						else if ( highSpeedEffect.isPlaying == true )
						{
							// Stop the particle effect
							highSpeedEffect.Stop();
						}
					}

					// If tilt controls are on, calculate the device's tilt direction
					if( tiltControls == true )
					{
						rotateDirection = Input.acceleration.x * 80 * touchSensitivity * Time.deltaTime;
					}
					else if ( Input.touchCount > 0 ) // If we detect a touch on the screen, we are using touch controls
				    {
						// If we touch the right side of the screen move right, and if we touch the left half, move left.
						if ( Input.GetTouch(0).position.x > Screen.width * 0.5 )    
						{
							if ( rotateDirection < 0 )     rotateDirection += Time.deltaTime * 10 * touchSensitivity;
							else    rotateDirection += Time.deltaTime * 3;
						}
						else if ( Input.GetTouch(0).position.x < Screen.width * 0.5 ) 
						{
							if ( rotateDirection > 0 )     rotateDirection -= Time.deltaTime * 10 * touchSensitivity;
							else    rotateDirection -= Time.deltaTime * 3;
						}
					}
					else
					{
						// If there is no touch input, slow down to a stop
						rotateDirection *= 0.7f;
					}

					// If we detect gamepad/keybaord input, we are using gamepad/keyaboard controls
					if ( Input.GetAxis("Horizontal") != 0 || Input.GetAxisRaw("Horizontal") != 0 )    usingGamepad = true;

					// Using gamepad
					if ( usingGamepad == true )
					{	
						// Rotate left/right based on gamepad/keyaboard input
						if ( stiffKeyboardInput == true )    rotateDirection = Input.GetAxisRaw("Horizontal");
						else    rotateDirection = Input.GetAxis("Horizontal");
					}

					// Limit the rotation direction between -1 and 1 ( left to right )
					rotateDirection = Mathf.Clamp( rotateDirection, -1, 1);

					// Rotate the player controller based on the rotation direction and the rotation speed
					playerController.eulerAngles += Vector3.forward * Time.deltaTime * rotateSpeed *  rotateDirection;

					// Change the moevement speed to match the target speed of this level
					moveSpeed = Mathf.Lerp( moveSpeed, targetSpeed, Time.deltaTime);

					// Move the player controller forward based on move speed
					playerController.Translate(Vector3.forward * Time.deltaTime * moveSpeed, Space.Self);

					// Keep the tube at the position of the player
					tubeObject.position = new Vector3( tubeObject.position.x, tubeObject.position.y, playerController.position.z);

					// Animate the material of the tube to give an illusion of movement through it
					tubeObject.GetComponent<Renderer>().material.mainTextureOffset = new Vector2( tubeObject.GetComponent<Renderer>().material.mainTextureOffset.x, offsetRatio.y * tubeObject.position.z * 0.12f);

					// Rotate the camera along with the player rotation
					if ( rotateCamera == false )    Camera.main.transform.eulerAngles = Vector3.forward;

					// Tilt the player when rotating, gives a nice motion effect
					if ( playerObject )    playerObject.localEulerAngles = Vector3.forward * -rotateDirection * tiltRange;
				}
			}

			//If the camera moved forward enough, create another lane
			if ( sectionsList.Length > 0 && sectionPosition - playerController.position.z < 500 )
			{ 
				CreateSection(sectionsList);
			}
		}

		/// <summary>
		/// Creates a new enemy at the end of a random lane 
		/// </summary>
		public Transform CreateSection( Spawn[] currentSpawnList )
		{
			// Create a new random target from the target list
			Transform newSpawn = Instantiate( currentSpawnList[Mathf.FloorToInt(Random.Range(0,currentSpawnList.Length))].spawnObject ) as Transform;
			
			// Place the target at a random position along the throw height
			newSpawn.position = Vector3.forward * sectionPosition;

			// Calculate the next section position
			sectionPosition += currentGap;

			return newSpawn;
		}

        /// <summary>
        /// Activates a shiled on the player, so it doesn't get hurt when hitting an obstacle
        /// </summary>
        /// <param name="duration"></param>
        public void ActivateShield( float duration )
        {
            // Activate the shield on the player
            if (playerObject) playerObject.SendMessage("Shield", duration);
        }

        /// <summary>
        /// Activates a shiled on the player, so it doesn't get hurt when hitting an obstacle
        /// </summary>
        /// <param name="duration"></param>
        public void ActivateSlowmotion(float duration)
        {
            // Activate the shield on the player
            if (playerObject) playerObject.SendMessage("Slowmotion", duration);
        }

        /// <summary>
        /// Change the score
        /// </summary>
        /// <param name="changeValue">Change value</param>
        void  ChangeScore( int changeValue )
		{
			score += changeValue;

			//Increase the counter to the next level
			levelUpCount += changeValue;

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

			//If we reached the required score, level up!
			if ( levelUpCount >= scoreToLevelUp )
			{
				levelUpCount -= scoreToLevelUp;
				
				LevelUp();
			}

			// Update the progress bar to show how far we are from the next level
			if ( progressCanvas )
			{
				progressCanvas.GetComponent<Image>().fillAmount = levelUpCount * 1.0f/scoreToLevelUp * 1.0f;
			}
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
		/// Levels up, and increases the difficulty of the game
		/// </summary>
		void  LevelUp()
		{
			currentLevel++;

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

			// Set the speed of the player in this level
			//targetSpeed = levels[currentLevel].targetSpeed;
			targetSpeed += speedIncrease;

			// Set the distance between each two sections in this level
			//sectionGap = levels[currentLevel].sectionGap;
			currentGap = sectionGap * targetSpeed * gapToSpeedRatio;

			// Make sure the gap is never smaller than the minimum
			if ( currentGap < minimumGap )    currentGap = minimumGap;
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
				//levelUpCanvas.Find("Text").GetComponent<Text>().text = levelNamePrefix + (currentLevel+1).ToString();

				// Play the level up animation
				if ( levelUpCanvas.GetComponent<Animation>() )    levelUpCanvas.GetComponent<Animation>().Play();

				if ( playerObject )    playerObject.GetComponent<Animator>().SetTrigger("SpeedUp");


			}

			//If there is a source and a sound, play it from the source
			if ( soundSource && soundLevelUp )    
			{
				soundSource.GetComponent<AudioSource>().pitch = 1;

				soundSource.GetComponent<AudioSource>().PlayOneShot(soundLevelUp);
			}
		}


		public void Continue()
		{
			// Create the player object and place it at the start position
			if ( playerController )
			{
				playerController.Translate( Vector3.forward * -currentGap * 0.8f, Space.World);

				// Enable the player object
				playerObject.gameObject.SetActive(true);

				isGameOver = false;

				//Enable the pause and game screens
				if ( gameCanvas )    gameCanvas.gameObject.SetActive(true);

				//Hide the game over screen
				gameOverCanvas.gameObject.SetActive(false);

				playerObject.SendMessage("Live");
			}
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
		public void Pause( bool showMenu )
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

			yield return new WaitForSeconds(delay);
			
			//Remove the pause and game screens
			if ( pauseCanvas )    pauseCanvas.gameObject.SetActive(false);
			if ( gameCanvas )    gameCanvas.gameObject.SetActive(false);
			
			//Show the game over screen
			if ( gameOverCanvas )    
			{
				//Show the game over screen
				gameOverCanvas.gameObject.SetActive(true);
				
				//Write the score text
				gameOverCanvas.Find("TextScore").GetComponent<Text>().text = "SCORE " + score.ToString();
				
				//Check if we got a high score
				if ( score > highScore )    
				{
					highScore = score;
					
					//Register the new high score
					PlayerPrefs.SetFloat(SceneManager.GetActiveScene().name + "HighScore", score);
				}
				
				//Write the high sscore text
				gameOverCanvas.Find("TextHighScore").GetComponent<Text>().text = "HIGH SCORE " + highScore.ToString();

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
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
		
		/// <summary>
		/// Restart the current level
		/// </summary>
		void  MainMenu()
		{
			SceneManager.LoadScene(mainMenuLevelName);
		}

		void OnDrawGizmos()
		{
			Gizmos.color = Color.red;

			Gizmos.DrawSphere (Vector3.forward * sectionPosition, 1);
		}
	}
}