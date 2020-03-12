using UnityEngine;
using UnityEngine.UI;
using RFLib;

/* Demo 2
 *  - Shows a method of popping up the high scores panel when game-over conditions arise.
 *  - Shows how to programatically add / remove event listeners to manage events
 * 
 * 
 */
namespace RFLibDemo
{

	public class RFHighScoresDemo2 : MonoBehaviour 
	{
		const string INSTRUCTIONS = "Press FIRE to crash and burn !!";
		const string HIGHSCORE_INST = "Enter Initials ! High Score !\nPress FIRE when done !!";
		const string GAMEOVER_INST = "Game Over! Press FIRE to restart !!";

		// Use a super set of states to manage behavior
		enum GAMEMODES
		{
			START,
			PLANE_CRASHING,
			GAMEOVER,
			HIGH_SCORE_COLLECT
		}
		public Transform ThePlane;		// Faux player avatar. Something to crash and in provide visual indication of "game over" state

		public Text ScoreDisplay;		// Text element for the score
		public Text Instructions;		// instructions to display

		public Transform ScorePanel;	// The panel that shows the scores; need to "activate" it when the game is over

		public RFHighScoreViewer HighScoreManager; // Reference to the RFHighScoreManager script; this allows us to perform high scores management	
													// And to add a listener to it to await events
						
		public AudioClip[] SoundClips;	// used for high score initials selection
		public AudioClip[] Crashes;		// Used for score done edit and plane crash

		AudioSource	audioSource;


		int score = 0;					// basic score value
		float scoreCountDown = 0.5f;	// Every half second, increase the score
		float crashtime = 0;			// Used in the crash rotation lerp
		float xvel = 5;					// decreasing value once the plane starts to crash; used to change x position
		float crashYVel = 5;			// increase value ince player starts to crash; use to change y position

		GAMEMODES gameMode = GAMEMODES.START;
		int updateIndex = 0;			// Simple counter to play some different sounds

		// Use this for initialization
		void Start () 
		{

			audioSource = GetComponent<AudioSource>();

		}
		
		// Update is called once per frame
		void Update ()
		{
			// Game has started; move the player avatar and update the score!
			if( gameMode == GAMEMODES.START )
			{
				// 	Score Increment if necessary
				scoreCountDown -= Time.deltaTime;
				if( scoreCountDown < 0 )
				{
					score += Random.Range( 10, 25 );
					ScoreDisplay.text = score.ToString();
					// Timer reset!
					scoreCountDown = 0.5f;
				}

				// Generic plane movement
				Vector3 pos = ThePlane.position;
				pos.x += Time.deltaTime * xvel;

				// Give the plan a little bobble
				Quaternion q = ThePlane.localRotation;
				q.eulerAngles = new Vector3( 0, 0, Mathf.Sin( Time.time * 10 ) * 10 );
				ThePlane.localRotation = q;
				// warp to beginning of screen...
				if( pos.x > 10 )
				{
					pos.x = -10;
					pos.y = Random.Range( -2.0f, 5.5f );
				}
				ThePlane.position = pos;

				// DOH! plane Crashed...player pressed FIRE!!
				if( Input.GetAxis( "Fire1" ) != 0 )
				{
					gameMode = GAMEMODES.PLANE_CRASHING;
					Instructions.text = "";
				}
			} 

			// Animate the plane into the ground...
			else if( gameMode == GAMEMODES.PLANE_CRASHING )
			{
				// Lerp a rotation downward over a stretched amount of "time"
				Vector3 newueler = Vector3.Lerp( Vector3.zero, new Vector3( 0, 0, -90 ), (crashtime ) );
				Quaternion q = ThePlane.localRotation;
				q.eulerAngles = newueler;
				ThePlane.localRotation = q;
				crashtime += Time.deltaTime*1.5f;
				if( crashtime > 1.0f )	crashtime = 1.0f;

				// Generic plane movement
				Vector3 pos = ThePlane.position;

				// Move it in a decreasing x dir...
				pos.x += Time.deltaTime * xvel;
				xvel = xvel * 0.98f;
				// And.. move it downward with an increasing speed.
				pos.y -= Time.deltaTime * crashYVel*1.05f;

				// Warp if necessary
				if( pos.x > 10 )
					pos.x = -10;

				// If the plane has fallen to our "ground", then... game over! Get the score
				if( pos.y < -4.25f )
				{
					pos.y = -4.25f;
					playCrash( 1 );
					ThePlane.GetComponent<Animator>().StopPlayback();

					// default game over mode set
					gameMode = GAMEMODES.GAMEOVER; 

					ScorePanel.gameObject.SetActive( true );

					// Player Achieved a high score;  So, add an even listener to the high scores manager
					if( HighScoreManager.AddScore( this.score ) == true )
					{
						HighScoreManager.onRFHighScoreEvent.AddListener( HandleRFHighScoreEvent ); // Add the listener	
					
						// Set the game stat to "Collecting the high score"
						gameMode = GAMEMODES.HIGH_SCORE_COLLECT;
						Instructions.text = HIGHSCORE_INST;
					}

				}
				ThePlane.position = pos;
			}
			else if( gameMode == GAMEMODES.GAMEOVER )
			{
				if(Instructions.text != GAMEOVER_INST)
					Instructions.text = GAMEOVER_INST;
				
				if(Input.GetAxis("Fire1") != 0)
				{
					Input.ResetInputAxes();
					restart();
				}	
					
					
			}

		}
		// Restart everything; Resets all values of interest back to starting vals
		void restart()
		{
			Instructions.text = INSTRUCTIONS;
			ScorePanel.gameObject.SetActive( false );
			score = 0;
			scoreCountDown = 0.5f;	// Every half second, increase the score
			crashtime = 0;
			xvel = 5;
			crashYVel = 5;
			ThePlane.position = new Vector3( 0, 0, 0 );

			gameMode = GAMEMODES.START;

		}

		// The following event handler is connected via the RFHighScoreEvent dispatch found in RFHighScoreViewer
		// Added as an event listener when an actual HIGH SCORE was mode available
		public void HandleRFHighScoreEvent(RFHighScoreEventData highScoreEvent)
		{
			// Don't do anything unless we're actually in "Game Over" mode
			if( gameMode != GAMEMODES.HIGH_SCORE_COLLECT )
				return;
			
			switch( highScoreEvent.EventType )
			{
				// Player is all done editing their score;  Remove the event listener and restart
				case RFHighScoreEventType.RFHIGH_SCORE_EDIT_DONE:
					playCrash( 0 );
					HighScoreManager.onRFHighScoreEvent.RemoveListener( HandleRFHighScoreEvent ); 
					// Reset the game mode to standard game over!
					gameMode = GAMEMODES.GAMEOVER;
					break;

				// Player got a new high score; however, in this demo, the even listener is added AFTER we figure out if 
				// a high score was achieved. (That means this event type won't get handled)
				case RFHighScoreEventType.RFHIGH_SCORE_NEW:
					Debug.Log( "Player has a new high score! Draw their attention to it and let them know then can edit their initials!" );
					break;

				case RFHighScoreEventType.RFHIGH_SCORE_EDIT_UPDATE:
					playSelectorSound( updateIndex % 3 );
					updateIndex++;
					break;


			}
		}
		// Simple method used to play one of our crash sounds
		void playCrash(int which)
		{
			if( Crashes != null && which < Crashes.Length && audioSource != null)
			{
				audioSource.clip = Crashes[ which ];
				audioSource.Play();
			}
		}
		// Method to play on of the sound clips (used in high scores initials selection)
		void playSelectorSound(int which)
		{
			if( SoundClips != null && which < SoundClips.Length && audioSource != null)
			{
				audioSource.clip = SoundClips[ which ];
				audioSource.Play();
			}
		}

	}
}