using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Zigzag.Types;

namespace Zigzag
{
	/// <summary>
	/// This script controls the game, starting it, following game progress, and finishing it with game over.
	/// It also creates lanes with moving objects and items as the player progresses.
	/// </summary>
	public class ZIRGameController : MonoBehaviour 
	{
		// The camera that follows the player
		public Transform cameraObject;
		
		// A list of player objects
		public Transform[] playerObjects;
		public int currentPlayer = 0;
		
		// Used to record the movement delta of the player, so that the background can be animated correctly
		internal Vector3 playerPos;
		internal Vector3 playerPrevPos;
		
		// The button that turns the player when using a gamepad or keyboard
		public string turnButton = "Submit";
		
		// The ground object in the background. This object is always attached to the player while its texture is 
		// animated to give an illusion of motion. 
		public Transform groundObject;
		
		// A list of objects that are created around the road to fill the ground with details ( cactus, rock, etc )
		public Transform[] groundDetails;
		
		// The random distance offset from the road where these objects are created. 0 means they are created right at the 
		// edge of the road, and the larger the number the farther they are created from the road edge.
		public float detailsOffset = 4;
		
		// The next road section in the scene. The next road section will be created based on this one.
		public Transform nextRoadSection;
		
		// The direction the player will turn, either 0 or 90
		internal float turnDirection = 0;
        internal float[] turnDirections = { 0, 90 };
        internal int turnDirectionIndex = 0;
		
		//The current direction of the road, either 0 or 90
		internal float roadDirection = 0;
		
		// Was the previous road section a turn? If so, 
		internal bool  previousWasATurn = false;
		
		// The default size of a road 
		public int defaultRoadSize = 2;
		
		// The straight road object
		public Transform roadStraight;
		
		// The turning road object
		public Transform roadTurn;
		
		// How long a road section ( made up of several straight road objects ) should be. 0 means the road will always turn
		// while a larger number means the road section will be 3/4/5/etc long. The number is chosen randomly within the range.
		public Vector2 roadSectionLength = new Vector2(0,5);
		
		// How many sections to create before starting the game
		public int precreateRoads = 20;
		
		// A list of the objects that can be dropped on the road
		public ItemDrop[] itemDrops;
		internal Transform[] itemDropList;
		
		// The offset left and right on the section
		public float itemDropOffset = 0.5f;
		
		// The score and score text of the player
		public int score = 0;
		public Transform scoreText;
		public string scoreTextSuffix = "$";
		internal int highScore = 0;

        // How many points we need to collect in order to win the level
        public int scoreToWin = 0;
		
		// The player prefs record of the total score we have ( not high score, but total score we collected in all games which is used as money )
		public string moneyPlayerPrefs = "Money";
		
		// The overall game speed. As you level up, the game speed increases.
		public float gameSpeed = 1;
		
		//How many score the player needs to collect before leveling up
		public int levelUpEveryScore = 500;
		internal int increaseCount = 0;
		
		// How much faster the game becomes when we level up
		public float levelUpSpeedIncrease = 0.1f;
		
		// A list of messages that are displayed on level up
		public string[] levelUpMessages;
		
		// The speech bubble text object that displays messages on level up
		public Transform messageObject;
		
		// Various canvases for the UI
		public Transform gameCanvas;
		public Transform pauseCanvas;
		public Transform gameOverCanvas;
        public Transform victoryCanvas;

        // Is the game over?
        internal bool  isGameOver = false;
		
		// The level of the main menu that can be loaded after the game ends
		public string mainMenuLevelName = "MainMenu";
		
		// Various sounds
		public AudioClip soundLevelUp;
		public AudioClip soundGameOver;
        public AudioClip soundVictory;

        // The tag of the sound source
        public string soundSourceTag = "GameController";
		
		// The confirm button which restarts the game after game over
		public string confirmButton = "Submit";
		
		// The button that pauses the game. Clicking on the pause button in the UI also pauses the game
		public string pauseButton = "Cancel";
		internal bool  isPaused = false;
		
		internal int index = 0;

        void Awake()
        {
            // Activate the pause canvas early on, so it can detect info about sound volume state
            if (pauseCanvas) pauseCanvas.gameObject.SetActive(true);
        }

        /// <summary>
        /// Start is only called once in the lifetime of the behaviour.
        /// The difference between Awake and Start is that Start is only called if the script instance is enabled.
        /// This allows you to delay any initialization code, until it is really needed.
        /// Awake is always called before any Start functions.
        /// This allows you to order initialization of scripts
        /// </summary>
        void  Start ()
		{
			//Update the score without adding to it
			ChangeScore(0);
			
			//Hide the canvases
			if ( gameOverCanvas )    gameOverCanvas.gameObject.SetActive(false);
            if ( victoryCanvas)    victoryCanvas.gameObject.SetActive(false);
            if (pauseCanvas) pauseCanvas.gameObject.SetActive(false);

            // Get the highscore for the player
            highScore = PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "_HighScore", 0);

            //Calculate the chances for the objects to drop
            int totalDrops = 0;
			int totalDropsIndex = 0;
			
			//Calculate the total number of drops with their chances
			for ( index = 0 ; index < itemDrops.Length ; index++ )
			{
				totalDrops += itemDrops[index].dropChance;
			}
			
			//Create a new list of the objects that can be dropped
			itemDropList = new Transform[totalDrops];
			
			//Go through the list again and fill out each type of drop based on its drop chance
			for ( index = 0 ; index < itemDrops.Length ; index++ )
			{
				int dropChanceCount = 0;
				
				while ( dropChanceCount < itemDrops[index].dropChance )
				{
					itemDropList[totalDropsIndex] = itemDrops[index].droppedObject;
					
					dropChanceCount++;
					
					totalDropsIndex++;
				}
			}
			
			//Get the currently selected player from PlayerPrefs
			currentPlayer = PlayerPrefs.GetInt("CurrentPlayer", currentPlayer);
			
			//Set the current player object
			SetPlayer(currentPlayer);
			
			playerPos = playerPrevPos = playerObjects[currentPlayer].position;
			
			//If the player object is not already assigned, Assign it from the "Player" tag
			if ( cameraObject == null )    cameraObject = GameObject.FindGameObjectWithTag("MainCamera").transform;
			
			//Create a few sections at the start of the game
			createSection(precreateRoads);

            //Pause the game at the start
            //Pause();

            LevelUpEffect();
        }

		/// <summary>
		/// Update is called every frame, if the MonoBehaviour is enabled.
		/// </summary>
		void  Update()
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
				if ( isPaused == true )
				{
					//If we press ESC or SPACE (by default), unpause the game
					if ( Input.GetButtonDown(pauseButton) )    Unpause();
					if ( Input.GetButtonDown(confirmButton) )    Unpause();
				}
				else
				{
					//If we press the turn button, turn the player
					if ( Input.GetButtonDown(turnButton) )    TurnPlayer();
					
					//If we press ESC ( by default ), pause the game
					if ( Input.GetButtonDown(pauseButton) )    Pause();
				}
			}
			
			//If we have a camera object and a ground object, make the ground object follow the position of the camera and
			//at the same time animate its material using texture offset to give an illusion of movement to the whole scene.
			if ( cameraObject && groundObject )
			{
				//Set the position of the ground at the position of the camera
				groundObject.position = new Vector3( cameraObject.position.x, groundObject.position.y, cameraObject.position.z);
				
				//Record the player's position
				playerPos = cameraObject.position;
				
				//Move the texture offset of the ground object based on the player positon
				groundObject.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(-0.4f * playerPos.x, -0.4f * playerPos.z);
				
				playerPrevPos = playerPos;
			}
		}

		/// <summary>
		/// Fixed Update is called after Update, if the MonoBehaviour is enabled.
		/// </summary>
		void LateUpdate()
		{
			//Make the camera chase the player in all directions
			if ( playerObjects[currentPlayer] )    
			{
				//If the camera moved forward enough, create another section
				if ( Vector3.Distance( playerObjects[currentPlayer].position, nextRoadSection.position ) < 20 )
				{ 
					createSection(1);
				}
				
				//Make the camera object follow the player object
				if ( cameraObject )
				{
					cameraObject.position = new Vector3( playerObjects[currentPlayer].position.x, cameraObject.position.y, playerObjects[currentPlayer].position.z);
				}
				
				//Make the message object follow the player
				if ( messageObject )
				{
					messageObject.position = playerObjects[currentPlayer].position;
				}
			}
		}
		
		/// <summary>
		/// Creates a section, sometimes reversing the paths of the moving objects in it
		/// </summary>
		/// <param name="sectionCount">How many road sections to create</param>
		void  createSection (  int sectionCount   )
		{
			//Create a few sections at the start of the game
			while ( sectionCount > 0 )
			{
				sectionCount--;
				
				//Check if we should have a turn or a straight road section. After a straight section there is *always* a turn.
				if ( previousWasATurn == false && roadTurn )
				{
					//Create a new turn road section
					Transform newTurnSection = Instantiate(roadTurn) as Transform;
					
					//If the last road turn direction is 90, turn left to 0 degress. So in the end we have a zig zag of turning roads
					if ( roadDirection == 90 )
					{
						roadDirection = 0;
						
						newTurnSection.position = nextRoadSection.position + new Vector3(defaultRoadSize,0,0);

						newTurnSection.eulerAngles = new Vector3( newTurnSection.eulerAngles.x, 180, newTurnSection.eulerAngles.z);
					}
					else
					{
						roadDirection = 90;
						
						newTurnSection.position = nextRoadSection.position + new Vector3(0,0,defaultRoadSize);
					}
					
					//Set this section as the next road section
					nextRoadSection = newTurnSection;
					
					previousWasATurn = true;
				}
				else if ( roadStraight ) //Otherwise, create a straight road section
				{
					//Set the length of the straight road
					int roadSectionLengthTemp = Mathf.RoundToInt(Random.Range(roadSectionLength.x, roadSectionLength.y));
					
					//Decide on which side of the road the items are created
					float itemOffset = Random.Range(-itemDropOffset, itemDropOffset);
					
					//Create the sections of the straight road
					for ( index = 0; index < roadSectionLengthTemp; index++)
					{
						//Create a new straight road section
						Transform newStraightSection= Instantiate(roadStraight) as Transform;
						
						//Align the next road section based on the rotation of the previous road direction
						if ( roadDirection == 0 )
						{
							newStraightSection.position = nextRoadSection.position + new Vector3(0,0,defaultRoadSize);
						}
						else
						{
							newStraightSection.position = nextRoadSection.position + new Vector3(defaultRoadSize,0,0);
							
							newStraightSection.eulerAngles = new Vector3( newStraightSection.eulerAngles.x, 90, newStraightSection.eulerAngles.z);
						}
						
						//Set this section as the next road section
						nextRoadSection = newStraightSection; 
						
						//Create an item in this road section
						Transform newItem = Instantiate( itemDropList[Mathf.FloorToInt(Random.Range(0, itemDropList.Length))] ) as Transform;
						
						newItem.position = newStraightSection.position;
						
						//newItem.position.z += itemOffset;
						newItem.Translate( newStraightSection.right * itemOffset, Space.Self);
						
                        if (index > 0 && index < roadSectionLengthTemp - 1)
                        {
                            //Create a ground detail object ( eock ex ,  bush, cactus, etc)
                            Transform newGroundObject = Instantiate(groundDetails[Mathf.FloorToInt(Random.Range(0, groundDetails.Length))]) as Transform;

                            newGroundObject.position = new Vector3(newStraightSection.position.x, newStraightSection.position.y + groundObject.position.y, newStraightSection.position.z);

                            float minimumOffset = defaultRoadSize + newGroundObject.GetComponentInChildren<MeshRenderer>().bounds.size.x;

                            //newGroundObject.Translate( newStraightSection.right * Random.Range( minimumOffset, minimumOffset + detailsOffset ), Space.Self);

                            //print(newGroundObject.name);

                            //Offset the object from the edge of the road, either to the right of the road or to the left of it, based on the direction of the road itself
                            if ( roadDirection == 90 )
                            {
                                newGroundObject.Translate( newStraightSection.right * Random.Range( minimumOffset, minimumOffset + detailsOffset ), Space.World);

                            }
                            else if ( roadDirection == 0 )
                            {
                                newGroundObject.Translate( newStraightSection.right * Random.Range( -minimumOffset, -minimumOffset - detailsOffset ), Space.World);

                            }

                            newGroundObject.Rotate(Vector3.up * Random.Range(0, 360), Space.World);

                        }
                    }
					
					previousWasATurn = false;
				}
				
			}
			
		}
		
		/// <summary>
		/// Changes the score of the player
		/// </summary>
		/// <param name="changeValue">Increase/Decrease value</param>
		void  ChangeScore (  int changeValue   )
		{
			//Change the score
			score += changeValue;
			
			//Update the score text
			if ( scoreText )    scoreText.GetComponent<Text>().text = score.ToString() + scoreTextSuffix;

            // Animate the score object
            if (scoreText.GetComponent<Animation>()) scoreText.GetComponent<Animation>().Play();

            //Increase the counter to the next level
            increaseCount += changeValue;

            // If we collect enough score, we win!
            if (scoreToWin > 0 && score >= scoreToWin)
            {
                Victory();

                return;
            }

            //If we reached the required score, level up!
            if ( increaseCount >= levelUpEveryScore )
			{
				increaseCount -= levelUpEveryScore;
				
				LevelUp();
			}
           
            
		}
		
		/// <summary>
		/// Levels up, and increases the difficulty of the game
		/// </summary>
		void  LevelUp ()
		{
			//Increase game speed
			gameSpeed += levelUpSpeedIncrease;
			
			//Set the timescale to game speed
			Time.timeScale = gameSpeed;
			
			//Run the level up effect, displaying a random message and sound
			LevelUpEffect();
		}
		
		/// <summary>
		/// Shows the effect associated with leveling up ( a sound and text bubble )
		/// </summary>
		void  LevelUpEffect ()
		{
			//Display one of the messages in the text bubble, and animate it
			if ( messageObject )
			{
				//If there's an animation, play it
				if ( messageObject.GetComponent<Animation>() )    messageObject.GetComponent<Animation>().Play();
				
				//Display a random message
				if ( levelUpMessages.Length > 0 )    messageObject.Find("Base/Text").GetComponent<Text>().text = levelUpMessages[Mathf.FloorToInt(Random.Range(0, levelUpMessages.Length))];
			}
			
			//If there is a source and a sound, play it from the source
			if ( soundSourceTag != string.Empty && soundLevelUp )    GameObject.FindGameObjectWithTag(soundSourceTag).GetComponent<AudioSource>().PlayOneShot(soundLevelUp);
		}
		
		/// <summary>
		/// Pauses the game
		/// </summary>
		public void  Pause ()
		{
			isPaused = true;
			
			//Set timescale to 0, preventing anything from moving
			Time.timeScale = 0;
			
			//Show the pause screen and hide the game screen
			if ( pauseCanvas )    pauseCanvas.gameObject.SetActive(true);
			if ( gameCanvas )    gameCanvas.gameObject.SetActive(false);
		}

        /// <summary>
        /// Unpauses the game
        /// </summary>
        public void Unpause ()
		{
			isPaused = false;
			
			//Set timescale back to the current game speed
			Time.timeScale = gameSpeed;
			
			//Hide the pause screen and show the game screen
			if ( pauseCanvas )    pauseCanvas.gameObject.SetActive(false);
			if ( gameCanvas )    gameCanvas.gameObject.SetActive(true);
		}
		
		/// <summary>
		/// Handles when the game is over.
		/// </summary>
		/// <returns>Yields for a period of time to allow execution to continue then continues through the game over text/gui display</returns>
		/// <param name="delay">The delay of the yield in seconds</param>
		IEnumerator GameOver(float delay)
		{
			yield return new WaitForSeconds(delay);
			
			isGameOver = true;
			
			//Remove the pause and game screens
			if ( pauseCanvas )    Destroy(pauseCanvas.gameObject);
			if ( gameCanvas )    Destroy(gameCanvas.gameObject);
			
			//Get the number of money we have
			int totalMoney = PlayerPrefs.GetInt( moneyPlayerPrefs, 0);
			
			//Add to the number of money we collected in this game
			totalMoney += score;
			
			//Record the number of money we have
			PlayerPrefs.SetInt( moneyPlayerPrefs, totalMoney);
			
			//Show the game over screen
			if ( gameOverCanvas )    
			{
				//Show the game over screen
				gameOverCanvas.gameObject.SetActive(true);
				
				//Write the score text
				gameOverCanvas.Find("Base/TextScore").GetComponent<Text>().text = "SCORE " + score.ToString();
				
				//Check if we got a high score
				if ( score > highScore )    
				{
					highScore = score;

                    // Register the new high score
                    PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "_HighScore", score);
                }

                //Write the high sscore text
                gameOverCanvas.Find("Base/TextHighScore").GetComponent<Text>().text = "HIGH SCORE " + highScore.ToString();
			}

            //If there is a source and a sound, play it from the source
            if ( soundSourceTag != string.Empty && soundGameOver ) GameObject.FindGameObjectWithTag(soundSourceTag).GetComponent<AudioSource>().PlayOneShot(soundGameOver);
        }

        /// <summary>
		/// Handles when the game is won.
		/// </summary>
		void Victory()
        {
            isGameOver = true;

			if ( playerObjects[currentPlayer] )    playerObjects[currentPlayer].SendMessage("Victory");

            //Remove the pause and game screens
            if (pauseCanvas) Destroy(pauseCanvas.gameObject);
            if (gameCanvas) Destroy(gameCanvas.gameObject);

            //Get the number of money we have
            int totalMoney = PlayerPrefs.GetInt(moneyPlayerPrefs, 0);

            //Add to the number of money we collected in this game
            totalMoney += score;

            //Record the number of money we have
            PlayerPrefs.SetInt(moneyPlayerPrefs, totalMoney);

            //Show the victory screen
            if (victoryCanvas)
            {
                //Show the game over screen
                victoryCanvas.gameObject.SetActive(true);

                //Write the score text
                victoryCanvas.Find("Base/TextScore").GetComponent<Text>().text = "SCORE " + score.ToString();

                //Check if we got a high score
                if (score > highScore)
                {
                    highScore = score;

                    // Register the new high score
                    PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "_HighScore", score);
                }

                //Write the high score text
                victoryCanvas.Find("Base/TextHighScore").GetComponent<Text>().text = "HIGH SCORE " + highScore.ToString();
            }

            //If there is a source and a sound, play it from the source
            if (soundSourceTag != string.Empty && soundVictory) GameObject.FindGameObjectWithTag(soundSourceTag).GetComponent<AudioSource>().PlayOneShot(soundVictory);
        }

        /// <summary>
		/// Reloads the current loaded level.
		/// </summary>
		void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        /// <summary>
        /// Loads and returns the user/player to the main menu.
        /// </summary>
        void MainMenu()
        {
            SceneManager.LoadScene(mainMenuLevelName);
        }

        /// <summary>
        /// Activates the selected player, while deactivating all the others
        /// </summary>
        /// <param name="playerNumber">The index of the player</param>
        void  SetPlayer (  int playerNumber   )
		{
			//Go through all the players, and hide each one except the current player
			for(index = 0; index < playerObjects.Length; index++)
			{
				if ( index != playerNumber )    playerObjects[index].gameObject.SetActive(false);
				else    playerObjects[index].gameObject.SetActive(true);
			}
		}

		/// <summary>
		/// Sends a turn command to the current player
		/// </summary>
		public void  TurnPlayer ()
		{
			if ( isPaused == false && playerObjects[currentPlayer] )    
			{
                // Use the turn directions from the list
                if (turnDirections.Length > 0)
                {
                    // Go to the next available turn direction, or start over from the first direction
                    if (turnDirectionIndex < turnDirections.Length - 1) turnDirectionIndex++;
                    else turnDirectionIndex = 0;

                    // Set the direction
                    turnDirection = turnDirections[turnDirectionIndex];
                }
                else // Or use the default 0 to 90 to 0 turns
                {
                    if ( turnDirection == 0 )    turnDirection = 90;
                    else turnDirection = 0;
                }

                // Make the player turn
                playerObjects[currentPlayer].SendMessage("SetTurn", turnDirection);
			}
		}

		/// <summary>
		/// Draws the position of the next lane in the editor.
		/// </summary>
		void  OnDrawGizmos ()
		{
			Gizmos.color = Color.red;
			
			if ( nextRoadSection )    
			{
				//Draw lines to show where the next road section will be created based on the default road size
				Gizmos.DrawLine( nextRoadSection.position + new Vector3(0.5f * defaultRoadSize, 0, 0.5f * defaultRoadSize), nextRoadSection.position + new Vector3(0.5f * defaultRoadSize, 0, 1.5f * defaultRoadSize));
				Gizmos.DrawLine( nextRoadSection.position + new Vector3(-0.5f * defaultRoadSize, 0, 0.5f * defaultRoadSize), nextRoadSection.position + new Vector3(-0.5f * defaultRoadSize, 0, 1.5f * defaultRoadSize));
			}
		}
	}
}