using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace StackGameTemplate
{
	/// <summary>
	/// This script controls the game, starting it, following game progress, and finishing it with game over.
	/// </summary>
	public class STKGameController:MonoBehaviour 
	{
        [Tooltip("The camera object that moves up with the toweres stacking")]
        public Transform cameraObject;

        [Tooltip("The tower object from which all other toweres are duplicated")]
        public Transform towerObject;
        internal Transform defaulttower;
        internal Transform previoustowerObject;
        
        // The current movement direction of the tower
        internal int moveDirection = 1;

        [Tooltip("The button for dropping a tower. This is defined from the Input Manager and corresponds to the Mouse, Gamepad, Keyboard, and Touch")]
        public string dropButton = "Fire1";

        [Tooltip("The default movement speed of a tower. Notice that the motion of the tower is smoothed with consinus")]
        public float moveSpeed = 1;

        // The actual speed of the tower, calculate from moveSpeed plus moveSpeedIncrease multiplied by our current streak
        internal float currentMoveSpeed;

        [Tooltip("How much the speed increases when you get a perfectly aligned tower, and increase your streak")]
        public float moveSpeedIncrease = 0.1f;

        [Tooltip("The movement range of the cosinus motion across the center of the screen")]
        public float moveRange = 1;

        // The movement phase in the cosinus motion. This is reset every time we spawn a new tower so that it starts away from the center.
        internal float movePhase = 0;

        [Tooltip("The minimum allowed size of a slice. If the slice size is smaller than this number, the tower falls off")]
        public float minimumSliceSize = 0.1f;

        // The current streak we are on. When you get a perfect alignment with the tower below, the streak increases
        internal int streak = 0;

        [Tooltip("How many perfect drops in a row we should get before the tower expands")]
        public int streakToExpand = 7;

        [Tooltip("How much the tower expands when we get enough streak. The tower cannot be larger than 1")]
        public float expandSize = 0.1f;

        [Tooltip("The effect that appears when you drop a perfectly aligned tower")]
        public Transform perfectEffect;
        
        [Tooltip("The score of the game. Score is earned by collecting dropping toweres")]
		public int score = 0;
		
		[Tooltip("The text object that displays the score, assigned from the scene")]
		public Transform scoreText;
		internal float highScore = 0;

        [Tooltip("The object that appears at the hiegst point we reached in this level, and shows the current high score")]
        public Transform highScoreObject;

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

		[Tooltip("Various canvases for the UI, assign them from the scene")]
		public Transform gameCanvas;
		public Transform pauseCanvas;
		public Transform gameOverCanvas;

		[Tooltip("Various sounds that play in different parts of the game")]
        public AudioClip soundSlice;
        public AudioClip soundPerfect;
        public AudioClip soundExpand;
        public AudioClip soundMiss;
        public AudioClip soundGameOver;
        public string soundSourceTag = "Sound";
		internal GameObject soundSource;

		// A general use index
		internal int index = 0;

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
            currentMoveSpeed = moveSpeed;

			if ( towerObject )
			{
                defaulttower = towerObject;
                
				Transform newtowerObject = Instantiate( defaulttower, towerObject.position + Vector3.up * towerObject.GetComponentInChildren<MeshRenderer>().bounds.size.y, towerObject.rotation) as Transform;
			
				previoustowerObject = towerObject;

				towerObject = newtowerObject;

				towerObject.GetComponent<Animation>().Play();

			}

			//Update the score and enemy count
			UpdateScore();

			//Hide the game over and pause screens
			if ( gameOverCanvas )    gameOverCanvas.gameObject.SetActive(false);
			if ( pauseCanvas )    pauseCanvas.gameObject.SetActive(false);
            //PlayerPrefs.DeleteAll();
            //Get the highscore for the player
            highScore = PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "HighScore", 0);
            
            // Set the actual height of the high score object based on the number of tower pieces we need to get there
            if (highScoreObject)
            {
                highScoreObject.position = Vector3.up * (towerObject.position.y + towerObject.GetComponentInChildren<MeshRenderer>().bounds.size.y * highScore);

                highScoreObject.Find("HighScorePlane/Text").GetComponent<Text>().text = highScore.ToString();

                if (highScore == 0) highScoreObject.gameObject.SetActive(false);
            }

            //Assign the sound source for easier access
            if ( GameObject.FindGameObjectWithTag(soundSourceTag) )    soundSource = GameObject.FindGameObjectWithTag(soundSourceTag);

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

					if ( towerObject )
					{
						if ( cameraObject )    cameraObject.position = Vector3.Slerp(cameraObject.position, Vector3.up * towerObject.position.y, Time.deltaTime);

						movePhase += Time.deltaTime;

						// Move the current tower back and forth above the previous tower, based on its move direction
						if ( moveDirection == 1 )    towerObject.position = new Vector3( Mathf.Cos(movePhase * currentMoveSpeed) * moveRange, towerObject.position.y, towerObject.position.z);
						else    towerObject.position = new Vector3( towerObject.position.x, towerObject.position.y, Mathf.Cos(movePhase * currentMoveSpeed) * moveRange);
                        
						// If a player exists and the game isn't paused, we can press the dropButton to make the player jump
						if ( isPaused == false && Input.GetButtonDown(dropButton) )
                        {
                            if ( (EventSystem.current.currentSelectedGameObject != null && EventSystem.current.currentSelectedGameObject.name != "ButtonPause") || EventSystem.current.currentSelectedGameObject == null)
                            {
                                CheckSlice();
                            }
                        }
					}
				}
			}
		}

		/// <summary>
		/// Change the score
		/// </summary>
		/// <param name="changeValue">Change value</param>
		void  ChangeScore( int changeValue )
		{
			score += changeValue;

            // If we reach the high score, play the high score object animation
            if (highScoreObject && score == highScore) highScoreObject.GetComponent<Animation>().Play();

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
		}

		/// <summary>
		/// Checks if the tower is on top of the one below it, and slices accordingly.
		/// </summary>
		void CheckSlice()
		{
			// The tower is moving from top right side to bottom left side
			if ( moveDirection == 1 )
			{
				// Calculate the of the slice
				float sliceSize = previoustowerObject.localScale.x - Mathf.Abs(towerObject.position.x - previoustowerObject.position.x);

				// If the top tower is close enough to the tower below it, we have a perfect match
				if ( Mathf.Abs(towerObject.position.x - previoustowerObject.position.x) < minimumSliceSize )
				{
					// Place the top tower perfectly on the one below it
					towerObject.position = new Vector3( previoustowerObject.position.x, previoustowerObject.position.y + previoustowerObject.GetComponentInChildren<MeshRenderer>().bounds.size.y, previoustowerObject.position.z);

					// Increase the streak for the perfect match
					streak++;

                    // Set the speed based on the current streak
                    currentMoveSpeed = moveSpeed + moveSpeedIncrease * streak;

                    //If there is a source and a sound, play it from the source
                    if ( soundSource && soundPerfect )
                    {
                        soundSource.GetComponent<AudioSource>().pitch = moveSpeed + moveSpeedIncrease * streak;

                        soundSource.GetComponent<AudioSource>().PlayOneShot(soundPerfect);
                    }

                    // If we reached a high enough streak, expand the tower
                    if ( streak >= streakToExpand )
					{
						StartCoroutine(Expandtower( towerObject, new Vector3(Mathf.Clamp(towerObject.localScale.x + expandSize, 0, 1), defaulttower.localScale.y, Mathf.Clamp(towerObject.localScale.z + expandSize, 0, 1))));

                        //towerObject.localScale = new Vector3(Mathf.Clamp(towerObject.localScale.x + expandSize, 0, 1), defaulttower.localScale.y, Mathf.Clamp(towerObject.localScale.z + expandSize, 0, 1));
					}

					// Place the top tower perfectly on the one below it
					towerObject.position = new Vector3( towerObject.position.x, previoustowerObject.position.y + previoustowerObject.GetComponentInChildren<MeshRenderer>().bounds.size.y, towerObject.position.z);

					// The current top tower is now the previous bottom one
					previoustowerObject = towerObject;

                    // Create a perfect effect and set its scale to be the same as the previous tower
                    if (perfectEffect)
                    {
                        // Create a new effect at the position of the previous tower
                        Transform newEffect = Instantiate(perfectEffect, previoustowerObject.position, previoustowerObject.rotation) as Transform;

                        // Scale it to be the same as the previous tower
                        newEffect.localScale = previoustowerObject.localScale;

                        // If we are expanding, activate a special particle effect inside the perfect effect
                        if (streak >= streakToExpand) newEffect.Find("Perfect").GetComponent<ParticleSystem>().Play();
                    }

                    // Create the next tower and place it above the previous tower
                    Transform nexttowerObject = Instantiate( previoustowerObject, previoustowerObject.position + Vector3.up * towerObject.GetComponentInChildren<MeshRenderer>().bounds.size.y, previoustowerObject.rotation) as Transform;

                    // This is now the current top tower
                    towerObject = nexttowerObject;

                    // This tower
                    //towerObject.GetComponent<towerCollider>().isTrigger = false;

                    // Play the animation of the tower, lowering from the top of the screen
                    towerObject.GetComponent<Animation>().Play();
                    
					// Reset the movement phase of the tower, forcing it to start from the farthest point away from the center
					movePhase = 0;

					// Increase the score
					ChangeScore(1);
				}
				else if ( sliceSize < minimumSliceSize )    // If the slice size is smaller than the minimum allowed size, then the whole tower drops and we lose
				{
					// Allow the tower to be affectd by physics ( falling from gravity), and allow it to collide with other objects as it falls
					towerObject.GetComponent<Rigidbody>().isKinematic = false;
					towerObject.GetComponent<Collider>().isTrigger = false;

					// Clear the current tower so that we don't move it accidentally
					towerObject = null;

					// Start the game over function with a 1 second delay
					StartCoroutine(GameOver(1));

                    //If there is a source and a sound, play it from the source
                    if (soundSource && soundMiss)
                    {
                        soundSource.GetComponent<AudioSource>().pitch = 1;

                        soundSource.GetComponent<AudioSource>().PlayOneShot(soundMiss);
                    }
                }
				else // If there is no perfect match and we also didn't completely miss the mark, slice the tower based on how much of it is above the tower below it
				{
					// Create a new slice from the current tower
					Transform slicedtowerObject = Instantiate( towerObject, towerObject.position, towerObject.rotation) as Transform;

					// The slice alignment is different based on the side on which the top tower is, relative to the tower below it
					if ( towerObject.position.x > previoustowerObject.position.x )
					{
						// Set the scale of the tower and the slice so that they seem like one part was cut from the other
						slicedtowerObject.localScale = new Vector3( towerObject.localScale.x - sliceSize, slicedtowerObject.localScale.y, slicedtowerObject.localScale.z);
						towerObject.localScale = new Vector3( sliceSize, towerObject.localScale.y, towerObject.localScale.z);

						// Change the position of the tower and the slice so that they seem like one part was cut from the other
						slicedtowerObject.position = new Vector3( previoustowerObject.position.x + (previoustowerObject.localScale.x + slicedtowerObject.localScale.x) * 0.5f, slicedtowerObject.position.y, slicedtowerObject.position.z);
						towerObject.position = new Vector3( previoustowerObject.position.x + (previoustowerObject.localScale.x - towerObject.localScale.x) * 0.5f, towerObject.position.y, towerObject.position.z);
                    }
                    else // The slice is from the other side of the tower
					{
						// Set the scale of the tower and the slice so that they seem like one part was cut from the other
						slicedtowerObject.localScale = new Vector3( towerObject.localScale.x - sliceSize, slicedtowerObject.localScale.y, slicedtowerObject.localScale.z);
						towerObject.localScale = new Vector3( sliceSize, towerObject.localScale.y, towerObject.localScale.z);

						// Change the position of the tower and the slice so that they seem like one part was cut from the other
						slicedtowerObject.position = new Vector3( previoustowerObject.position.x - (previoustowerObject.localScale.x + slicedtowerObject.localScale.x) * 0.5f, slicedtowerObject.position.y, slicedtowerObject.position.z);
						towerObject.position = new Vector3( previoustowerObject.position.x + (towerObject.localScale.x - previoustowerObject.localScale.x) * 0.5f, towerObject.position.y, towerObject.position.z);
                    }

                    // Allow the sliced tower to be affectd by physics ( falling from gravity), and allow it to collide with other objects as it falls
                    slicedtowerObject.GetComponent<Rigidbody>().isKinematic = false;
					slicedtowerObject.GetComponent<Collider>().isTrigger = false;
                    
					// Throw the sliced tower away from the center
					slicedtowerObject.GetComponent<Rigidbody>().AddExplosionForce(10, towerObject.position + Vector3.up * towerObject.localScale.y * 0.5f,10);

					// The current top tower is now the previous bottom one
					previoustowerObject = towerObject;

					// Create the next tower and place it above the previous tower
					Transform nexttowerObject = Instantiate( previoustowerObject, previoustowerObject.position + Vector3.up * towerObject.GetComponentInChildren<MeshRenderer>().bounds.size.y, previoustowerObject.rotation) as Transform;

					// This is now the current top tower
					towerObject = nexttowerObject;

                    // This tower
                    //towerObject.GetComponent<towerCollider>().isTrigger = false;

                    // Play the animation of the tower, lowering from the top of the screen
                    towerObject.GetComponent<Animation>().Play();
					
					// Reset the movement phase of the tower, forcing it to start from the farthest point away from the center
					movePhase = 0;

					// Increase the score
					ChangeScore(1);

					// Reset the streak to 0
					streak = 0;

                    // Set the speed based on the current streak
                    currentMoveSpeed = moveSpeed + moveSpeedIncrease * streak;

                    //If there is a source and a sound, play it from the source
                    if (soundSource && soundSlice)
                    {
                        soundSource.GetComponent<AudioSource>().pitch = Random.Range(0.7f, 1);

                        soundSource.GetComponent<AudioSource>().PlayOneShot(soundSlice);
                    }
                }
            }
			else // The tower is moving from top left side to bottom right side
			{
				// Calculate the of the slice
				float sliceSize = previoustowerObject.localScale.z - Mathf.Abs(towerObject.position.z - previoustowerObject.position.z);
				
				// If the top tower is close enough to the tower below it, we have a perfect match
				if ( Mathf.Abs(towerObject.position.z - previoustowerObject.position.z) < minimumSliceSize )
				{
					// Place the top tower perfectly on the one below it
					towerObject.position = new Vector3( previoustowerObject.position.x, previoustowerObject.position.y + previoustowerObject.GetComponentInChildren<MeshRenderer>().bounds.size.y, previoustowerObject.position.z);

					// Increase the streak for the perfect match
					streak++;

                    // Set the speed based on the current streak
                    currentMoveSpeed = moveSpeed + moveSpeedIncrease * streak;

                    //If there is a source and a sound, play it from the source
                    if (soundSource && soundPerfect)
                    {
                        soundSource.GetComponent<AudioSource>().pitch = moveSpeed + moveSpeedIncrease * streak;

                        soundSource.GetComponent<AudioSource>().PlayOneShot(soundPerfect);
                    }

                    // If we reached a high enough streak, expand the tower
                    if ( streak >= streakToExpand )
                    {
                        StartCoroutine(Expandtower(towerObject, new Vector3(Mathf.Clamp(towerObject.localScale.x + expandSize, 0, 1), defaulttower.localScale.y, Mathf.Clamp(towerObject.localScale.z + expandSize, 0, 1))));

                       // towerObject.localScale = new Vector3(Mathf.Clamp(towerObject.localScale.x + expandSize, 0, 1), defaulttower.localScale.y, Mathf.Clamp(towerObject.localScale.z + expandSize, 0, 1));


                    }

                    // Place the top tower perfectly on the one below it
                    towerObject.position = new Vector3( towerObject.position.x, previoustowerObject.position.y + previoustowerObject.GetComponentInChildren<MeshRenderer>().bounds.size.y, towerObject.position.z);

					// The current top tower is now the previous bottom one
					previoustowerObject = towerObject;

                    // Create a perfect effect and set its scale to be the same as the previous tower
                    if (perfectEffect)
                    {
                        // Create a new effect at the position of the previous tower
                        Transform newEffect = Instantiate(perfectEffect, previoustowerObject.position, previoustowerObject.rotation) as Transform;

                        // Scale it to be the same as the previous tower
                        newEffect.localScale = previoustowerObject.localScale;

                        // If we are expanding, activate a special particle effect inside the perfect effect
                        if (streak >= streakToExpand)    newEffect.Find("Perfect").GetComponent<ParticleSystem>().Play();
                    }

                    // Create the next tower and place it above the previous tower
                    //Transform nexttowerObject = Instantiate( previoustowerObject, previoustowerObject.position + new Vector3(previoustowerObject.position.x,previoustowerObject.localScale.y,moveRange), previoustowerObject.rotation) as Transform;
                    Transform nexttowerObject = Instantiate( previoustowerObject, previoustowerObject.position + Vector3.up * towerObject.GetComponentInChildren<MeshRenderer>().bounds.size.y, previoustowerObject.rotation) as Transform;

					// This is now the current top tower
					towerObject = nexttowerObject;
                    
                    // This tower
                    //towerObject.GetComponent<towerCollider>().isTrigger = false;

                    // Play the animation of the tower, lowering from the top of the screen
                    towerObject.GetComponent<Animation>().Play();
					
					// Reset the movement phase of the tower, forcing it to start from the farthest point away from the center
					movePhase = 0;

					// Increase the score
					ChangeScore(1);
				}
				else if ( sliceSize < minimumSliceSize )    // If the slice size is smaller than the minimum allowed size, then the whole tower drops and we lose
				{
					// Allow the tower to be affectd by physics ( falling from gravity), and allow it to collide with other objects as it falls
					towerObject.GetComponent<Rigidbody>().isKinematic = false;
					towerObject.GetComponent<Collider>().isTrigger = false;
					
					// Clear the current tower so that we don't move it accidentally
					towerObject = null;
					
					// Start the game over function with a 1 second delay
					StartCoroutine(GameOver(1));

                    //If there is a source and a sound, play it from the source
                    if (soundSource && soundMiss)
                    {
                        soundSource.GetComponent<AudioSource>().pitch = 1;

                        soundSource.GetComponent<AudioSource>().PlayOneShot(soundMiss);
                    }
                }
				else // If there is no perfect match and we also didn't completely miss the mark, slice the tower based on how much of it is above the tower below it
				{
					// Create a new slice from the current tower
					Transform slicedtowerObject = Instantiate( towerObject, towerObject.position, towerObject.rotation) as Transform;
					
					// The slice alignment is different based on the side on which the top tower is, relative to the tower below it
					if ( towerObject.position.z > previoustowerObject.position.z )
					{
						// Set the scale of the tower and the slice so that they seem like one part was cut from the other
						slicedtowerObject.localScale = new Vector3( slicedtowerObject.localScale.x, slicedtowerObject.localScale.y, towerObject.localScale.z - sliceSize);
						towerObject.localScale = new Vector3( towerObject.localScale.x, towerObject.localScale.y, sliceSize);
						
						// Change the position of the tower and the slice so that they seem like one part was cut from the other
						slicedtowerObject.position = new Vector3( slicedtowerObject.position.x, slicedtowerObject.position.y, previoustowerObject.position.z + (previoustowerObject.localScale.z + slicedtowerObject.localScale.z) * 0.5f);
						towerObject.position = new Vector3( towerObject.position.x, towerObject.position.y, previoustowerObject.position.z + (previoustowerObject.localScale.z - towerObject.localScale.z) * 0.5f);
					}
					else // The slice is from the other side of the tower
					{
						// Set the scale of the tower and the slice so that they seem like one part was cut from the other
						slicedtowerObject.localScale = new Vector3( slicedtowerObject.localScale.x, slicedtowerObject.localScale.y, towerObject.localScale.z - sliceSize);
						towerObject.localScale = new Vector3( towerObject.localScale.x, towerObject.localScale.y, sliceSize);
						
						// Change the position of the tower and the slice so that they seem like one part was cut from the other
						slicedtowerObject.position = new Vector3( slicedtowerObject.position.x, slicedtowerObject.position.y, previoustowerObject.position.z - (previoustowerObject.localScale.z + slicedtowerObject.localScale.z) * 0.5f);
						towerObject.position = new Vector3( towerObject.position.x, towerObject.position.y, previoustowerObject.position.z + (towerObject.localScale.z - previoustowerObject.localScale.z) * 0.5f);
					}
					
					// Allow the sliced tower to be affectd by physics ( falling from gravity), and allow it to collide with other objects as it falls
					slicedtowerObject.GetComponent<Rigidbody>().isKinematic = false;
					slicedtowerObject.GetComponent<Collider>().isTrigger = false;
					
					// Throw the sliced tower away from the center
					slicedtowerObject.GetComponent<Rigidbody>().AddExplosionForce(10, towerObject.position + Vector3.up * towerObject.localScale.y * 0.5f,10);

					// The current top tower is now the previous bottom one
					previoustowerObject = towerObject;

                    // Create the next tower and place it above the previous tower
                    Transform nexttowerObject = Instantiate( previoustowerObject, previoustowerObject.position + Vector3.up * towerObject.GetComponentInChildren<MeshRenderer>().bounds.size.y, previoustowerObject.rotation) as Transform;
					
					// This is now the current top tower
					towerObject = nexttowerObject;
                    
                    // This tower
                    //towerObject.GetComponent<towerCollider>().isTrigger = false;

                    // Play the animation of the tower, lowering from the top of the screen
                    towerObject.GetComponent<Animation>().Play();
					
					// Reset the movement phase of the tower, forcing it to start from the farthest point away from the center
					movePhase = 0;

					// Increase the score
					ChangeScore(1);

					// Reset the streak to 0
					streak = 0;

                    // Set the speed based on the current streak
                    currentMoveSpeed = moveSpeed + moveSpeedIncrease * streak;

                    //If there is a source and a sound, play it from the source
                    if (soundSource && soundSlice)
                    {
                        soundSource.GetComponent<AudioSource>().pitch = Random.Range(0.7f, 1);

                        soundSource.GetComponent<AudioSource>().PlayOneShot(soundSlice);
                    }
                }
			}

			// Switch the direction of movement between topright-to-bottomleft and topleft-to-bottomright
			if ( moveDirection == 1 )    moveDirection = -1;
			else    moveDirection = 1;
		}

		/// <summary>
		/// Expands the tower to a certain scale and position
		/// </summary>
		/// <returns>The tower.</returns>
		IEnumerator Expandtower( Transform targetObject, Vector3 targetSize )
		{
            //If there is a source and a sound, play it from the source
            if (soundSource && soundExpand)
            {
                soundSource.GetComponent<AudioSource>().pitch = moveSpeed + moveSpeedIncrease * (streak - streakToExpand);

                soundSource.GetComponent<AudioSource>().PlayOneShot(soundExpand);
            }
            
            // Expand the tower as long as it's smaller than a scale of 1
            targetObject.localScale = Vector3.MoveTowards(targetObject.localScale, targetSize, expandSize);

            // Align the expanded tower to the one below it
            targetObject.position = Vector3.MoveTowards(targetObject.position, new Vector3(defaulttower.position.x, targetObject.position.y, defaulttower.position.z), expandSize * 0.5f);

            yield return new WaitForFixedUpdate();

        }
        
		/// <summary>
		/// Shuffles the specified Color list, and returns it
		/// </summary>
		/// <param name="colors">A list of colors</param>
		Color[] Shuffle( Color[] colors )
		{
			// Go through all the colors and shuffle them
			for ( index = 0 ; index < colors.Length ; index++ )
			{
				// Hold the text in a temporary variable
				Color tempNumber = colors[index];
				
				// Choose a random index from the text list
				int randomIndex = UnityEngine.Random.Range( index, colors.Length);
				
				// Assign a random text from the list
				colors[index] = colors[randomIndex];
				
				// Assign the temporary text to the random question we chose
				colors[randomIndex] = tempNumber;
			}
			
			return colors;
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
				gameOverCanvas.Find("Base/Texts/TextScore").GetComponent<Text>().text = "내가 올린 층수는 " + score.ToString();
				
				//Check if we got a high score
				if ( score > highScore )    
				{
					highScore = score;

                    //Register the new high score
					PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "HighScore", score);
                }

                //Write the high score text
                gameOverCanvas.Find("Base/Texts/TextHighScore").GetComponent<Text>().text = "이 빌딩의 최고 높이는 " + highScore.ToString();

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
        void Restart()
        {
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        /// <summary>
        /// Restart the current level
        /// </summary>
        void MainMenu()
        {
			SceneManager.LoadScene("ArcadeGame");
        }
    }
}