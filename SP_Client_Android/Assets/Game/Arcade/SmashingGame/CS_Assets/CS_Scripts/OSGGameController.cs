using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using ObjectSmashingGame.Types;

namespace ObjectSmashingGame
{
	/// <summary>
	/// This script controls the game, starting it, following game progress, and finishing it with game over or victory.
	/// </summary>
	public class OSGGameController : MonoBehaviour 
	{
        internal Camera cameraObject;
        internal Transform cameraHolder;

        [Tooltip("The number of lanes in the level")]
        public int laneCount = 4;

        [Tooltip("The gap space between each two lanes")]
        public float laneGap = 1;

        [Tooltip("How gap between each two rows of objects")]
        public float spawnGap = 1.6f;
        internal float spawnGapCount = 0;

        [Tooltip("The length of a lane. Objects are spawned at the start of a lane, and removed when they reach the end of the lane")]
        public float laneLength = 8;

        [Tooltip("The good object which you can break and earn coins from")]
        public OSGSmashObject goodObject;

        [Tooltip("The bad object which you lose a life if you touch")]
        public OSGSmashObject badObject;

        [Tooltip("A list of levels, each with its own target score, target limit, and time bonus")]
        public Level[] levels;

        [Tooltip("The current level we are on. We must reach the target score in order to go to the next level")]
        public int currentLevel = 0;

        [Tooltip("If you set this to true the game will continue forever after the last level in the list. Otherwise you will get the victory screen after the last level")]
        public bool isEndless = false;

        // The number of rows in this level. This is defined for each level in the scene
        internal int rowsInLevel = 0;
        internal bool isSpawning = true;
        internal int objectsLeft = 0;

        // The speed of the game in this level. This is defined for each level in the scene
        internal float moveSpeed = 1;

        // The maximum number of objects in a row. This is defined for each level in the scene
        internal int maxObjectsInRow = 1;

        // The chance for an object to be bad. This is defined for each level in the scene
        internal float badObjectChance = 0.1f;

        //[Tooltip("The final object that spawns, which should be bigger and slower than the rest, but give more score. You can leave this empty if you don't want a big object in this level")]
        //internal Transform bigObject;

        [Tooltip("How long to wait before starting the game. Ready?GO! time")]
        public float startDelay = 1;

        [Tooltip("The effect displayed before starting the game")]
        public Transform readyGoEffect;
        
        [Tooltip("The attack button, click it or tap it to smash objects")]
        public string attackButton = "Fire1";

        [Tooltip("The effect created when hitting an object")]
        public Transform hitEffect;

        [Tooltip("The bonus effect that shows how much bonus we got when we hit a target")]
        public Transform bonusEffect;

        [Tooltip("The score of the player")]
        public int score = 0;

        [Tooltip("The lives object which displays the current lives of the player")]
        public Transform livesObject;

        [Tooltip("The number of lives the player has. If lives reach 0, it's game over")]
        public int lives = 3;

        [Tooltip("A delay to prevent the player from losing many lives together. If you lose a life, you will not lose another life for some time")]
        public float loseLifeDelay = 1;
        internal float loseLifeDelayCount;

        [Tooltip("The score text object which displays the current score of the player")]
        public Transform scoreText;
		internal int highScore = 0;
		internal int scoreMultiplier = 1;

        [Tooltip("A list of messages that are displayed on level up")]
        public string[] levelUpMessages = { "AWESOME!", "GREAT!", "SMASHING!", "SUPER!", "FANTASTIC!" };

        // Various canvases for the UI
        public Transform gameCanvas;
		public Transform progressCanvas;
        public Transform levelUpCanvas;
        public Transform pauseCanvas;
		public Transform gameOverCanvas;
		public Transform victoryCanvas;

		// Is the game over?
		internal bool  isGameOver = false;
		
		// The level of the main menu that can be loaded after the game ends
		public string mainMenuLevelName = "CS_StartMenu";
		
		// Various sounds and their source
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
            if (cameraObject == null)
            {
                cameraObject = Camera.main;

                if (cameraObject.transform.root) cameraHolder = cameraObject.transform.root;
            }
            
            // Disable multitouch so that we don't tap two answers at the same time ( prevents multi-answer cheating, thanks to Miguel Paolino for catching this bug )
            Input.multiTouchEnabled = true;

            //Update the progress in the level
            //InvokeRepeating("UpdateProgress", 0, 0.5f);

            UpdateProgress();

            ChangeScore(0);

            // Update the number of lives we have
            ChangeLives(0);

            loseLifeDelayCount = 0;

            // Check what level we are on
            UpdateLevel();

            //Hide the cavases
            if ( gameOverCanvas )    gameOverCanvas.gameObject.SetActive(false);
			if ( victoryCanvas )    victoryCanvas.gameObject.SetActive(false);
			if ( pauseCanvas )    pauseCanvas.gameObject.SetActive(false);

			//Get the highscore for the player

			highScore = PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "HighScore", 0);

			//Assign the sound source for easier access
			if ( GameObject.FindGameObjectWithTag(soundSourceTag) )    soundSource = GameObject.FindGameObjectWithTag(soundSourceTag);

			// Reset the spawn delay
			spawnGapCount = 0;
			
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

                //if (startDelay <= 0) isSpawning = true;
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
                    if (loseLifeDelayCount > 0) loseLifeDelayCount -= Time.deltaTime;

                    // If we press the attack button, Attack!
                    if (!EventSystem.current.IsPointerOverGameObject() )
                    {
                        // Mouse attack
                        if (Input.GetButtonDown(attackButton))
                        {
                            Attack(Input.mousePosition);
                        }

                        

                        // Touch (mobile) attack
                        if ( Input.touchCount > 0 )
                        {
                            for (int i = 0; i < Input.touchCount; i++)
                            {
                                if (Input.GetTouch(i).phase == TouchPhase.Began )
                                {
                                    Attack(Input.GetTouch(i).position);
                                }
                            }
                        }
                    }

                    // Count down to the next target spawn
                    if ( isSpawning == true )
                    {
                        if (spawnGapCount > 0) spawnGapCount -= moveSpeed * Time.deltaTime;
                        else
                        {
                            // Reset the spawn delay count
                            spawnGapCount = spawnGap;

                            // Spawn a new row of objects
                            CreateRow();
                        }
                    }

					//Toggle pause/unpause in the game
					if ( Input.GetButtonDown(pauseButton) )
					{
						if ( isPaused == true )    Unpause();
						else    Pause(true);
					}
				}
			}
		}

        public void Attack( Vector3 attackPosition )
        {
            Ray ray = Camera.main.ScreenPointToRay(attackPosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hitEffect) Instantiate(hitEffect, hit.point, Quaternion.identity);

                hit.collider.SendMessage("HitObject", hit.point);
            }

            //foreach (Touch touch in Input.touches)
            //{
            //    //if ( touch.phase == TouchPhase.Began )
            //    //{
            //        if (Physics.Raycast(Camera.main.ScreenPointToRay(touch.position), out hit, 100))
            //        {
            //            if (hitEffect) Instantiate(hitEffect, hit.point, Quaternion.identity);

            //            hit.collider.SendMessage("HitObject", hit.point);
            //        }
            //    //}
            //}

            //Touch myTouch = Input.GetTouch(0);

            //Touch[] myTouches = Input.touches;


        }

        /// <summary>
        /// Spawns a new row of objects, some of which may be good or bad objects
        /// </summary>
        public void CreateRow()
        {
            int objectsInRow = Random.Range(1, maxObjectsInRow);

            // Draws a white line showing a lane from start to end
            for ( index = 0; index < laneCount; index++ )
            {
                if (Random.value < badObjectChance)
                {
                    if (badObject) Instantiate(badObject, new Vector3(index * laneGap, 0, 0), Quaternion.identity);

                    objectsLeft++;
                }
                else if ( Random.value < 0.5f )
                {
                    if ( goodObject )    Instantiate(goodObject, new Vector3(index * laneGap, 0, 0), Quaternion.identity);

                    objectsInRow--;

                    objectsLeft++;
                }

                if (objectsInRow <= 0) break;
            }

            rowsInLevel++;

            //UpdateProgress();
        }
        
        /// <summary>
        /// Changes the lives of the player. If lives reach 0, it's game over
        /// </summary>
        /// <param name="changeValue"></param>
        public void ChangeLives(int changeValue)
        {
            if ( loseLifeDelayCount <= 0 )
            {
                // Chnage health value
                lives += changeValue;

                if (lives <= 0)
                {
                    // Health reached 0, so the target should die
                    StartCoroutine(GameOver(0.5f));
                }

                if (livesObject)
                {
                    livesObject.Find("Text").GetComponent<Text>().text = lives.ToString();

                    if (livesObject.GetComponent<Animation>()) livesObject.GetComponent<Animation>().Play();
                }

                loseLifeDelayCount = loseLifeDelay;

                if ( changeValue < 0 )
                {
                    if (cameraHolder) cameraHolder.GetComponent<Animation>().Play();
                }
            }
            
        }

        /// <summary>
        /// Change the score and update it
        /// </summary>
        /// <param name="changeValue">Change value</param>
        public void  ChangeScore( int changeValue )
		{
			score += changeValue;

            //Update the score text
            if ( scoreText )    scoreText.GetComponent<Text>().text = score.ToString();
        }
		
		/// <summary>
		/// Updates the rows value and checks if we got to the next level
		/// </summary>
		public void  UpdateProgress()
		{
            // If we reached the required number of points, level up!
			if ( rowsInLevel >= levels[currentLevel].rowsInLevel )
			{
                isSpawning = false;

                // All small objects have been removed
                if ( objectsLeft <= 0 )
                {
                   // if ( levels[currentLevel].bigObject != null && bigObject == null )
                    //{
                    //    SpawnBigObject();
                    //}
                    //else
                   // {
                        if (currentLevel < levels.Length - 1) LevelUp();
                        else if (isEndless == false) StartCoroutine(Victory(1));
                   // }
                }
			}

			// Update the progress bar to show how far we are from the next level
			if ( progressCanvas )
			{
				if ( currentLevel == 0 )    progressCanvas.GetComponent<Image>().fillAmount = rowsInLevel * 1.0f/levels[currentLevel].rowsInLevel * 1.0f;
				else    progressCanvas.GetComponent<Image>().fillAmount = (rowsInLevel - levels[currentLevel - 1].rowsInLevel) * 1.0f/(levels[currentLevel].rowsInLevel - levels[currentLevel - 1].rowsInLevel) * 1.0f;
			}
		}


        public void SpawnBigObject()
        {
            //bigObject = Instantiate(levels[currentLevel].bigObject, new Vector3((laneGap * (laneCount-1)) * 0.5f, 0, 0), Quaternion.identity) as Transform;

            //levels[currentLevel].bigObject = null;

            //objectsLeft++;

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

			//Run the level up effect
			LevelUpEffect();
		}

		/// <summary>
		/// Updates the level and sets some values like maximum targets, throw angle, and level text
		/// </summary>
		void UpdateLevel()
		{
            //bigObject = null;

            isSpawning = true;

            // Display the current level we are on
            if ( progressCanvas )    progressCanvas.Find("Text").GetComponent<Text>().text = (currentLevel + 1).ToString();

            // Set the number of rows in this level
            rowsInLevel = 0;

            // Set the speed of rows of objects in this level
            moveSpeed = levels[currentLevel].moveSpeed;

            // Set the maximum number of objects in a row for this level. Example: If you set it to 4, there will be between 0 and 4 objects in a row.
            maxObjectsInRow = levels[currentLevel].maxObjectsInRow;

            // Set the chance for a bad object to appear. A bad object can only appear instead of an empty slot in a row of objects.
            badObjectChance = levels[currentLevel].badObjectChance;
        }

		/// <summary>
		/// Shows the effect associated with leveling up ( a sound and text bubble )
		/// </summary>
		void  LevelUpEffect()
		{
            // If a level up effect exists, update it and play its animation
            if (levelUpCanvas)
            {
                //Display a random level up message
                if (levelUpMessages.Length > 0) levelUpCanvas.Find("Text").GetComponent<Text>().text = levelUpMessages[Mathf.FloorToInt(Random.Range(0, levelUpMessages.Length))];
                //levelUpCanvas.Find("Text").GetComponent<Text>().text = levels[currentLevel].levelName;

                // Play the level up animation
                if (levelUpCanvas.GetComponent<Animation>()) levelUpCanvas.GetComponent<Animation>().Play();
            }

            //If there is a source and a sound, play it from the source
            if ( soundSource && soundLevelUp )    
			{
				soundSource.GetComponent<AudioSource>().pitch = 1;

				soundSource.GetComponent<AudioSource>().PlayOneShot(soundLevelUp);
			}
		}

        /// <summary>
        /// Pause the game, and shows the pause menu
        /// </summary>
        /// <param name="showMenu">If set to <c>true</c> show menu.</param>
        public void Pause(bool showMenu)
        {
            isPaused = true;

            //Set timescale to 0, preventing anything from moving
            Time.timeScale = 0;

            //Show the pause screen and hide the game screen
            if (showMenu == true)
            {
                if (pauseCanvas) pauseCanvas.gameObject.SetActive(true);
                if (gameCanvas) gameCanvas.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Resume the game
        /// </summary>
        public void Unpause()
        {
            isPaused = false;

            //Set timescale back to the current game speed
            Time.timeScale = 1;

            //Hide the pause screen and show the game screen
            if (pauseCanvas) pauseCanvas.gameObject.SetActive(false);
            if (gameCanvas) gameCanvas.gameObject.SetActive(true);
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
				gameOverCanvas.Find("Base/TextScore").GetComponent<Text>().text = "제 점수는요 " + score.ToString();
				
				//Check if we got a high score
				if ( score > highScore )    
				{
					highScore = score;
					
					//Register the new high score
					PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "HighScore", score);
				}
				
				//Write the high sscore text
				gameOverCanvas.Find("Base/TextHighScore").GetComponent<Text>().text = "최고점은요 " + highScore.ToString();

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
				victoryCanvas.Find("Base/TextScore").GetComponent<Text>().text = "최고점이네요!!! " + score.ToString();
				
				//Check if we got a high score
				if ( score > highScore )    
				{
					highScore = score;

					//Register the new high score
		
					PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "HighScore", score);
				}
				
				//Write the high sscore text
				victoryCanvas.Find("Base/TextHighScore").GetComponent<Text>().text = "당신의 점수가 최고! " + highScore.ToString();
				
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
            // Draws a white line showing a lane from start to end
            for (index = 0; index < laneCount; index++)
            {
                Gizmos.DrawLine(new Vector3(index * laneGap, 0, 0), new Vector3(index * laneGap, 0, laneLength));
            }

            float laneLengthTemp = 0;

            while ( laneLengthTemp < laneLength)
            {
                Gizmos.DrawLine(new Vector3(0, 0, laneLengthTemp), new Vector3((laneCount-1) * laneGap, 0, laneLengthTemp));

                laneLengthTemp += spawnGap;
            }
        }
    }
}