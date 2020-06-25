#if UNITY_5_3_OR_NEWER
using UnityEngine.SceneManagement;
#endif

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using ShootingGallery.Types;

namespace ShootingGallery
{
	/// <summary>
	/// This script controls the game, starting it, following game progress, and finishing it with game over.
	/// </summary>
	public class SGTGameController:MonoBehaviour 
	{
		// How long to wait before starting the game. Ready?GO! time
		public float startDelay = 1;

		// The effect displayed before starting the game
		public Transform readyGoEffect;

		// How many seconds are left before game over
		public float timeLeft = 30;

		// The text object that displays the time
		public Text timeText;

		// A list of moving targets ( The duck rows )
		public Transform[] movingTargets; 

		// The speed of the moving targets
		public float movingSpeed = 2;

		// How many targets to show at once
		public int maximumTargets = 5;

		// The horizontal warea within which targets are shown. Targets outside of this area will never be shown
		public float targetShowArea = 4;

		// How long to wait before showing the targets
		public float showDelay = 3;
		internal float showDelayCount = 0;
		
		// How long to wait before hiding the targets again
		public float hideDelay = 2;
		internal float hideDelayCount = 0;

		// The left and right edges of the game area. Targets bounce off these edges.
		public Transform leftEdge;
		public Transform rightEdge;

		// The shoot button, click it or tap it to shoot
		public string shootButton = "Fire1";

		// The keyboard/gamepad button for reloading
		public string reloadButton = "Jump";

		// The bullet/shot that appears when you shoot
		public Transform shotObject;

		// The maximum number of bullets you can have
		public int ammo = 6;
		
		// The number of bullets left
		internal float ammoLeft;
		
		// The image showing how many bullets we have left
		public Image ammoBar;
		
		// The width of a single bullet in the ammo bar
		public float ammoBarWidth = 21;

		// The point at which you aim when shooting. Used for mobile and gamepad/keyboard controls
		public Transform crosshair;
		
		// How fast the crosshair moves
		public float crosshairSpeed = 15;

		// Are we using the mouse now?
		internal bool usingMouse = false;

		// The position we are aiming at now
		internal Vector3 aimPosition;

		// How many points we get when we hit a target. This bonus is multiplied by the number of targets on screen
		public int hitTargetBonus = 10;

		// Counts the current streak
		internal int streak = 1;
		
		// The bonus effect that shows how much bonus we got when we hit a target
		public Transform bonusEffect;

		// The score and score text of the player
		public int score = 0;
		public Transform scoreText;
		internal int highScore = 0;
		internal int scoreMultiplier = 1;

		// The overall game speed
		public float gameSpeed = 1;
		
		//How many points the player needs to collect before leveling up
		public Level[] levels;
		public int currentLevel = 0;

		// The game will continue forever after the last level
		public bool isEndless = false;

		// Various canvases for the UI
		public Transform gameCanvas;
		public Transform progressCanvas;
		public Transform pauseCanvas;
		public Transform gameOverCanvas;
		public Transform victoryCanvas;

		// Is the game over?
		internal bool  isGameOver = false;
		
		// The level of the main menu that can be loaded after the game ends
		public string mainMenuLevelName = "CS_StartMenu";
		
		// Various sounds and their source
		public AudioClip soundReload;
		public AudioClip soundLevelUp;
		public AudioClip soundGameOver;
		public AudioClip soundVictory;
		public string soundSourceTag = "GameController";
		internal GameObject soundSource;
		
		// The button that will restart the game after game over
		public string confirmButton = "Submit";
		
		// The button that pauses the game. Clicking on the pause button in the UI also pauses the game
		public string pauseButton = "Cancel";
		internal bool  isPaused = false;

		// A general use index
		internal int index = 0;

		//public Transform slowMotionEffect;

		void Awake()
		{
			// Activate the pause canvas early on, so it can detect info about sound volume state
			if ( pauseCanvas )    pauseCanvas.gameObject.SetActive(true);
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
			// Check if we are running on a mobile device. If so, remove the crosshair as we don't need it for taps
			if ( Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android )    
			{
				// If a crosshair is assigned, hide it
				if ( crosshair )    crosshair.gameObject.SetActive(false);
				
				crosshair = null;
			}

			//Update the score
			UpdateScore();

			// Set the ammo we have
			ammoLeft = ammo;

			// Update the ammo
			UpdateAmmo();

			//Hide the cavases
			if ( gameOverCanvas )    gameOverCanvas.gameObject.SetActive(false);
			if ( victoryCanvas )    victoryCanvas.gameObject.SetActive(false);
			if ( pauseCanvas )    pauseCanvas.gameObject.SetActive(false);

			//Get the highscore for the player
			#if UNITY_5_3_OR_NEWER
			highScore = PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "HighScore", 0);
			#else
			highScore = PlayerPrefs.GetInt(Application.loadedLevelName + "HighScore", 0);
			#endif

			//Assign the sound source for easier access
			if ( GameObject.FindGameObjectWithTag(soundSourceTag) )    soundSource = GameObject.FindGameObjectWithTag(soundSourceTag);

			// Reset the spawn delay
			showDelayCount = 0;

			// Check what level we are on
			UpdateLevel();

			// Move the targets from one side of the screen to the other, and then reset them
			foreach ( Transform movingTarget in movingTargets )
			{
				movingTarget.SendMessage("HideTarget");
			}

			// Create the ready?GO! effect
			if ( readyGoEffect )    Instantiate( readyGoEffect );
		}

		/// <summary>
		/// Update is called every frame, if the MonoBehaviour is enabled.
		/// </summary>
		void  Update()
		{
			// Delay the start of the game
			if ( startDelay > 0 )
			{
				startDelay -= Time.deltaTime;
			}
			else
			{
				// Move the targets from one side of the screen to the other, and then reset them
				foreach ( Transform movingTarget in movingTargets )
				{
					// Check the direction of movement
					if ( movingTarget.localScale.x > 0 )
					{
						// Move to the right
						movingTarget.position = new Vector3( movingTarget.position.x + movingSpeed * Time.deltaTime, movingTarget.position.y, movingTarget.position.z);

						// When the target reaches the right edge, reset it to the left edge
						if ( movingTarget.position.x > rightEdge.position.x )    movingTarget.position = new Vector3( leftEdge.position.x, movingTarget.position.y, movingTarget.position.z);
					}
					else
					{
						// Move to the left
						movingTarget.position = new Vector3( movingTarget.position.x - movingSpeed * Time.deltaTime, movingTarget.position.y, movingTarget.position.z);

						// When the target reaches the right edge, reset it to the left edge
						if ( movingTarget.position.x < leftEdge.position.x )    movingTarget.position = new Vector3( rightEdge.position.x, movingTarget.position.y, movingTarget.position.z);
					}
				}

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
					// If we press the reload button, reload!
					if ( Input.GetButtonDown(reloadButton) )
					{
						Reload();
					}

					// Count down the time until game over
					if ( timeLeft > 0 )
					{
						// Count down the time
						timeLeft -= Time.deltaTime;

						// Update the timer
						UpdateTime();
					}

					// Keyboard and Gamepad controls
					if ( crosshair )
					{
						// If we move the mouse in any direction, then mouse controls take effect
						if ( Input.GetAxisRaw("Mouse X") != 0 || Input.GetAxisRaw("Mouse Y") != 0 || Input.GetMouseButtonDown(0) || Input.touchCount > 0 )    usingMouse = true;

						// We are using the mouse, hide the crosshair
						if ( usingMouse == true )
						{
							// Calculate the mouse/tap position
							aimPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
							
							// Make sure it's 2D
							aimPosition.z = 0;
						}

						// If we press gamepad or keyboard arrows, then mouse controls are turned off
						if ( Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0 )    
						{
							usingMouse = false;
						}

						// Move the crosshair based on gamepad/keyboard directions
						aimPosition += new Vector3( Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), aimPosition.z) * crosshairSpeed * Time.deltaTime;
					
						// Limit the position of the crosshair to the edges of the screen
						// Limit to the left screen edge
						if ( aimPosition.x < Camera.main.ScreenToWorldPoint(Vector3.zero).x )    aimPosition = new Vector3( Camera.main.ScreenToWorldPoint(Vector3.zero).x, aimPosition.y, aimPosition.z);
						
						// Limit to the right screen edge
						if ( aimPosition.x > Camera.main.ScreenToWorldPoint(Vector3.right * Screen.width).x )    aimPosition = new Vector3( Camera.main.ScreenToWorldPoint(Vector3.right * Screen.width).x, aimPosition.y, aimPosition.z);
						
						// Limit to the bottom screen edge
						if ( aimPosition.y < Camera.main.ScreenToWorldPoint(Vector3.zero).y )    aimPosition = new Vector3( aimPosition.x, Camera.main.ScreenToWorldPoint(Vector3.zero).y, aimPosition.z);
						
						// Limit to the top screen edge
						if ( aimPosition.y > Camera.main.ScreenToWorldPoint(Vector3.up * Screen.height).y )    aimPosition = new Vector3( aimPosition.x, Camera.main.ScreenToWorldPoint(Vector3.up * Screen.height).y, aimPosition.z);

						// Place the crosshair at the position of the mouse/tap, with an added offset
						crosshair.position = aimPosition;

						// If we press the shoot button, SHOOT!
						if ( usingMouse == false && Input.GetButtonDown(shootButton) )    Shoot();
					}

					// Count down to the next target spawn
					if ( showDelayCount > 0 )    showDelayCount -= Time.deltaTime;
					else 
					{
						// Reset the spawn delay count
						showDelayCount = showDelay;

						ShowTargets(maximumTargets);
					}

					//Toggle pause/unpause in the game
					if ( Input.GetButtonDown(pauseButton) )
					{
						if ( isPaused == true )    Unpause();
						else    Pause();
					}
				}
			}
		}

		/// <summary>
		/// Updates the timer text, and checks if time is up
		/// </summary>
		void UpdateTime()
		{
			// Update the timer text
			if ( timeText )    
			{
				timeText.text = timeLeft.ToString("00");
			}

			// Time's up!
			if ( timeLeft <= 0 )    
			{
				StartCoroutine(GameOver(0));
			}
		}

		/// <summary>
		/// Shows a random batch of targets. Due to the random nature of selection, some targets may be selected more than once making the total number of targets to appear smaller.
		/// </summary>
		/// <param name="targetCount">The maximum number of target that will appear</param>
		void ShowTargets( int targetCount )
		{
			// Limit the number of tries when showing targets, so we don't get stuck in an infinite loop
			int maximumTries = targetCount * 5;

			// Show several targets that are within the game area
			while ( targetCount > 0 && maximumTries > 0 )
			{
				maximumTries--;

				// Choose a random target
				int randomTarget = Mathf.FloorToInt(Random.Range(0, movingTargets.Length));

				// If the chosen target is hidden, and is within the game area, show it!
				if ( Mathf.Abs(movingTargets[randomTarget].position.x) < targetShowArea )
				{
					targetCount--;
					
					// Show a random targets from the list of moving targets
					movingTargets[randomTarget].SendMessage("ShowTarget", hideDelay);
				}
			}

			// Reset the streak when showing a batch of new targets
			streak = 1;
		}

		/// <summary>
		/// Give a bonus when the target is hit. The bonus is multiplied by the number of targets on screen
		/// </summary>
		/// <param name="hitSource">The target that was hit</param>
		void HitBonus( Transform hitSource )
		{
			// If we have a bonus effect
			if ( bonusEffect )
			{
				// Create a new bonus effect at the hitSource position
				Transform newBonusEffect = Instantiate(bonusEffect, hitSource.position, Quaternion.identity) as Transform;

				// Display the bonus value multiplied by a streak
				newBonusEffect.Find("Text").GetComponent<Text>().text = "+" + (hitTargetBonus * streak).ToString();

				// Rotate the bonus text slightly
				newBonusEffect.eulerAngles = Vector3.forward * Random.Range(-10,10);
			}

			// Increase the hit streak
			streak++;

			// Add the bonus to the score
			ChangeScore(hitTargetBonus * streak);
		}

		/// <summary>
		/// Change the score
		/// </summary>
		/// <param name="changeValue">Change value</param>
		void  ChangeScore( int changeValue )
		{
			score += changeValue;

			//Update the score
			UpdateScore();
		}
		
		/// <summary>
		/// Updates the score value and checks if we got to the next level
		/// </summary>
		void  UpdateScore()
		{
			//Update the score text
			if ( scoreText )    scoreText.GetComponent<Text>().text = score.ToString();

			// If we reached the required number of points, level up!
			if ( score >= levels[currentLevel].scoreToNextLevel )
			{
				if ( currentLevel < levels.Length - 1 )    LevelUp();
				else    if ( isEndless == false )    StartCoroutine(Victory(0));
			}

			// Update the progress bar to show how far we are from the next level
			if ( progressCanvas )
			{
				if ( currentLevel == 0 )    progressCanvas.GetComponent<Image>().fillAmount = score * 1.0f/levels[currentLevel].scoreToNextLevel * 1.0f;
				else    progressCanvas.GetComponent<Image>().fillAmount = (score - levels[currentLevel - 1].scoreToNextLevel) * 1.0f/(levels[currentLevel].scoreToNextLevel - levels[currentLevel - 1].scoreToNextLevel) * 1.0f;
			}
		}

		/// <summary>
		/// Set the score multiplier ( Get double score for hitting and destroying targets )
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

			// Set the maximum number of targets
			maximumTargets = levels[currentLevel].maximumTargets;

			// Update the game speed
			movingSpeed = levels[currentLevel].movingSpeed;

			// Give time bonus for winning the level
			timeLeft += levels[currentLevel].timeBonus;

			// Update the timer
			UpdateTime();

			// Set the number of bullets the player has this level
			ammo = levels[currentLevel].ammo;

			// Update the ammo
			UpdateAmmo();
		}

		/// <summary>
		/// Shows the effect associated with leveling up ( a sound and text bubble )
		/// </summary>
		void  LevelUpEffect ()
		{
			// Show the time bonus effect when we level up
			if ( bonusEffect )
			{
				// Create a new bonus effect at the hitSource position
				Transform newBonusEffect = Instantiate(bonusEffect) as Transform;

				newBonusEffect.position = new Vector3( Camera.main.ScreenToWorldPoint(timeText.transform.position).x, Camera.main.ScreenToWorldPoint(timeText.transform.position).y - 2, 0);

				// Display the bonus value multiplied by a streak
				newBonusEffect.Find("Text").GetComponent<Text>().text = "EXTRA TIME!\n+" + levels[currentLevel].timeBonus.ToString();
			}

			//If there is a source and a sound, play it from the source
			if ( soundSource && soundLevelUp )    
			{
				soundSource.GetComponent<AudioSource>().pitch = 1;

				soundSource.GetComponent<AudioSource>().PlayOneShot(soundLevelUp);
			}
		}

		/// <summary>
		/// Pause the game
		/// </summary>
		public void  Pause()
		{
			isPaused = true;
			
			//Set timescale to 0, preventing anything from moving
			Time.timeScale = 0;
			
			//Show the pause screen and hide the game screen
			if ( pauseCanvas )    pauseCanvas.gameObject.SetActive(true);
			if ( gameCanvas )    gameCanvas.gameObject.SetActive(false);
		}

        /// <summary>
        /// Resume the game
        /// </summary>
        public void Unpause()
		{
			isPaused = false;
			
			//Set timescale back to the current game speed
			Time.timeScale = gameSpeed;
			
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
			if ( pauseCanvas )    Destroy(pauseCanvas.gameObject);
			if ( gameCanvas )    Destroy(gameCanvas.gameObject);
			
			//Show the game over screen
			if ( gameOverCanvas )    
			{
				//Show the game over screen
				gameOverCanvas.gameObject.SetActive(true);
				
				//Write the score text
				gameOverCanvas.Find("TextScore").GetComponent<Text>().text = "내 점수는 " + score.ToString();
				
				//Check if we got a high score
				if ( score > highScore )    
				{
					highScore = score;
					
					//Register the new high score
					#if UNITY_5_3_OR_NEWER
					PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "HighScore", score);
					#else
					PlayerPrefs.SetInt(Application.loadedLevelName + "HighScore", score);
					#endif
				}
				
				//Write the high sscore text
				gameOverCanvas.Find("TextHighScore").GetComponent<Text>().text = "최고 점수는 " + highScore.ToString();

				//If there is a source and a sound, play it from the source
				if ( soundSource && soundGameOver )    
				{
					soundSource.GetComponent<AudioSource>().pitch = 1;
					
					soundSource.GetComponent<AudioSource>().PlayOneShot(soundGameOver);
				}
			}
		}

		/// <summary>
		/// Runs the victory event and shows the victory screen
		/// </summary>
		IEnumerator Victory(float delay)
		{
			isGameOver = true;
			
			yield return new WaitForSeconds(delay);
			
			//Remove the pause and game screens
			if ( pauseCanvas )    Destroy(pauseCanvas.gameObject);
			if ( gameCanvas )    Destroy(gameCanvas.gameObject);
			
			//Show the game over screen
			if ( victoryCanvas )    
			{
				//Show the game over screen
				victoryCanvas.gameObject.SetActive(true);
				
				//Write the score text
				victoryCanvas.Find("TextScore").GetComponent<Text>().text = "점수는 " + score.ToString();
				
				//Check if we got a high score
				if ( score > highScore )    
				{
					highScore = score;

					//Register the new high score
					#if UNITY_5_3_OR_NEWER
					PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "HighScore", score);
					#else
					PlayerPrefs.SetInt(Application.loadedLevelName + "HighScore", score);
					#endif
				}
				
				//Write the high sscore text
				victoryCanvas.Find("TextHighScore").GetComponent<Text>().text = "최고 점수는 " + highScore.ToString();
				
				//If there is a source and a sound, play it from the source
				if ( soundSource && soundVictory )    
				{
					soundSource.GetComponent<AudioSource>().pitch = 1;
					
					soundSource.GetComponent<AudioSource>().PlayOneShot(soundVictory);
				}
			}
		}
		
		/// <summary>
		/// Restart the current level
		/// </summary>
		void  Restart()
		{
			#if UNITY_5_3_OR_NEWER
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
			#if UNITY_5_3_OR_NEWER
			SceneManager.LoadScene(mainMenuLevelName);
			#else
			Application.LoadLevel(mainMenuLevelName);
			#endif
		}

		/// <summary>
		/// Updates the ammo we have
		/// </summary>
		public void UpdateAmmo()
		{
			// Only update if the ammor bar has been assigned
			if ( ammoBar )    
			{
				// Update the ammo left by scaling the width of the AmmoFull bar
				ammoBar.rectTransform.sizeDelta = new Vector2( ammoLeft * ammoBarWidth, ammoBar.rectTransform.sizeDelta.y);

				// Set the AmmoEmpty width based on the number of bullets we have multiplied by the width of a single bullet in the bar
				ammoBar.transform.Find("Empty").GetComponent<Image>().rectTransform.sizeDelta = new Vector2( ammo * ammoBarWidth, ammoBar.rectTransform.sizeDelta.y);
			}	
		}

		/// <summary>
		/// Shoots!
		/// </summary>
		public void Shoot()
		{
			if ( startDelay <= 0 && ammoLeft > 0 && shotObject && Time.deltaTime > 0 )
			{
				// Create a new shot at the position of the mouse/tap
				Transform newShot = Instantiate( shotObject ) as Transform;

				// If we are using the mouse, make sure we are aiming at the mouse position
				if ( crosshair == false )    aimPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				
				// Make sure it's 2D
				aimPosition.z = 0;
				
				// Place the shot at the position of the click, and spread it randomly around the center
				newShot.transform.position = aimPosition;

				// Reduce from ammo
				ammoLeft--;

				// Update the ammo we have left
				UpdateAmmo();
			}
		}

		/// <summary>
		/// Reloads the ammo
		/// </summary>
		public void Reload()
		{
			// Refill the ammo
			ammoLeft = ammo;

			// Update the ammo we have left
			UpdateAmmo();

			//If there is a source and a sound, play it from the source
			if ( soundSource && soundReload )    
			{
				soundSource.GetComponent<AudioSource>().pitch = 1;

				soundSource.GetComponent<AudioSource>().PlayOneShot(soundReload);
			}
		}
//FOR A FUTURE UPDATE
		/// <summary>
		/// Slows the game down to 0.5 speed for a few seconds
		/// </summary>
		/// <param name="duration">Duration of slowmotion effect</param>
//		IEnumerator SlowMotion(float duration)
//		{
//			Transform newEffect = null;
//
//			if ( slowMotionEffect )    
//			{
//				// Create a slow motion effect
//				newEffect = Instantiate( slowMotionEffect, Vector3.zero, Quaternion.identity) as Transform;
//
//				// Animate the effect
//				if ( newEffect.animation )
//				{
//					newEffect.animation[newEffect.animation.clip.name].speed = 1; 
//					newEffect.animation.Play(newEffect.animation.clip.name);
//				}
//			}
//
//			// Set the game speed to half
//			gameSpeed *= 0.5f;
//
//			// Set the timescale accordingly
//			Time.timeScale = gameSpeed;
//
//			// This makes sure the game runs smoothly even in slowmotion. Otherwise you will get clunky physics
//			Time.fixedDeltaTime = Time.timeScale * 0.02f;
//
//			// Wait for some time
//			yield return new WaitForSeconds(duration);
//
//			// Reverse the slowmotion animation
//			if ( slowMotionEffect && newEffect.animation )    
//			{
//				newEffect.animation[newEffect.animation.clip.name].speed = -1; 
//				newEffect.animation[newEffect.animation.clip.name].time = newEffect.animation[newEffect.animation.clip.name].length; 
//				newEffect.animation.Play(newEffect.animation.clip.name);
//			}
//
//			// Reset speed back to normal
//			gameSpeed *= 2;
//
//			// Set the timescale accordingly
//			Time.timeScale = gameSpeed;
//
//			Time.fixedDeltaTime = 0.02f;
//		}

		/// <summary>
		/// Raises the draw gizmos event.
		/// </summary>
		void OnDrawGizmos()
		{
			Gizmos.color = Color.red;

			// Draw the left limit of the area in which targets can be shown
			Gizmos.DrawLine( new Vector3( targetShowArea, -5, 0), new Vector3( targetShowArea, 5, 0) );

			// Draw the right limit of the area in which targets can be shown
			Gizmos.DrawLine( new Vector3( -targetShowArea, -5, 0), new Vector3( -targetShowArea, 5, 0) );
		}
	}
}