using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;

/* Monobehavior meant to display high scores.  When in "collect mode", fetch player input (initials)
 * 
 * Also hooks up a "view" of the high scores data, as managed by the High Scores Manager class
 * 
 * Provides an event dispatch so other game systems may perform actions associated with new scores, player input ( change of initials ), and completion of score submission
 */ 

namespace RFLib
{
		

	public class RFHighScoreViewer : MonoBehaviour 
	{
		[Tooltip("Parent Transform where all score renderers will be placed.")]
		public Transform HighScoresContainer;		// Parent transform for high score renderers

		[Tooltip("Prefab instanced and used to render a single high score.")]
		public GameObject HighScoreRendererPrefab;	// Prefab to use to render a single line of high scores data
											        //   the prefab MUST implement the IRFHighScoreHandler interface

		[Tooltip("Default scores to display when the current list is empty.")]
		public RFHighScore[] DefaultScores;

		[Tooltip("If no scores exist, auto-set the defined defaults when scores are loaded.")]
		public bool AutosetDefaultScores = true;

		[Tooltip("Automatically load and render scores on Start")]
		public bool AutoloadScores = true;			// When Start is called, should this component automatically load and render the current high scores?
													

		[Tooltip("User selectable characters to include in user's initials.")]
		public string SelectableCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890_!$";


		[Tooltip("Filename where scores are written.")]
		public string HighscoresFile = "rf_highscores.dat";

		[Tooltip("The maximum number of high scores to maintain")]
		public int HighScoreMaxCount = 10;				


		[Tooltip("Default initials value for a high score. Also controls MAX initials count.")]
		public string NewScoreInitials	= "___";		// When a new score starts, also controls the maximum number of characters in the initials string

		[Tooltip("Input axis measured to indicate completion.")]
		public string DoneAxis			= "Fire1";


		[Serializable]
		public class RFHighScoreChangeEvent : UnityEvent<RFHighScoreEventData>{}
		[SerializeField]
		private RFHighScoreChangeEvent rfHighScoreEvent = new RFHighScoreChangeEvent();
		public RFHighScoreChangeEvent onRFHighScoreEvent	
		{		
			get { return rfHighScoreEvent; } 	
			set { rfHighScoreEvent = value; }
		}



		RFHighScoresManager  _highScoresManager;		// Instance of the high scores manager; access through the getter below

		IRFHighScoreHandler highScoreHandler;			// Reference to the High Score Handler associated with most recent high score
		RFHighScore mostRecentHighScore;				// The last high score to be added; if this is not null; then the
														// viewer is in "initials collection mode"



		RFHighScoreEventData currentHighScoreEvent;		// cached value of event data; used when a new high score is entered, passed around in event invocation


		// High Scores Manager getter
		public RFHighScoresManager HighScoresManager
		{
			get 
			{
				// Create an instance of the high score manager if one does not yet exist
				if( _highScoresManager == null )	_highScoresManager = new RFHighScoresManager();
				return _highScoresManager;
			}
		}


		void Awake()
		{
			
		}

		// Use this for initialization
		void Start () 
		{

			HighScoresManager.MaximumScoresCount = HighScoreMaxCount;
			HighScoresManager.DefaultScoreList = DefaultScores;


			if(HighScoreRendererPrefab == null || HighScoreRendererPrefab.GetComponent<IRFHighScoreHandler>() == null )
			{
				Debug.LogError( "RFHighScoreViewer::HighScoreRendererPrefab is either null or does not have a component script that implements the IRFHighScoreHandler interface" );
			}

			// Set up the high scores file to use
			HighScoresManager.HighScoreFilename = HighscoresFile;

			if( AutoloadScores )
				LoadAndDisplayScores();
				
		}


		// Update is called once per frame
		void Update () 
		{
			// If we're monkeying in the editor, update the HighScoresManager with value changes
			// Also redisplay the list if it shrunk (will be culled in the high scores manager)
			#if UNITY_EDITOR
				int oldvalue = HighScoresManager.MaximumScoresCount;
				HighScoresManager.MaximumScoresCount = HighScoreMaxCount;
				if(HighScoreMaxCount < oldvalue)
					displayHighScores();
			#endif

			// When the most recent high score is not null, then we're in "collect initials" mode
			if( mostRecentHighScore != null && highScoreHandler != null )
			{
				// Input selection is DONE!
				if( Input.GetAxis( DoneAxis ) != 0 )
				{
					Input.ResetInputAxes();
					endHighScoreEdit();

				}
			}

		}
		void endHighScoreEdit()
		{
			highScoreHandler.EditHighScoreComplete( );
			// All done - Better save those scores.
			HighScoresManager.SaveHighScores(HighscoresFile);

			// Let other systems know editing is DONE!
			currentHighScoreEvent.EventType = RFHighScoreEventType.RFHIGH_SCORE_EDIT_DONE;
			onRFHighScoreEvent.Invoke( currentHighScoreEvent );
			// reset some values
			currentHighScoreEvent = null;
			mostRecentHighScore = null;
			highScoreHandler = null;
		}

		// Iterate through the high scores list; create renderers, pass data
		// for renderers to do their thing
		void displayHighScores()
		{
			
			// The container and the renderer prefab both need to be hooked up!
			if( HighScoresContainer == null || HighScoreRendererPrefab == null )
			{
				return;
			}

			List<RFHighScore> scores = HighScoresManager.HighScoresList;

			// Destroy current high scores children
			RFUtils.DestroyTransformChildren( HighScoresContainer );


			for( int cnt = 0; cnt < scores.Count; cnt++ )
			{
				GameObject renderer = Instantiate( HighScoreRendererPrefab ) as GameObject;
				IRFHighScoreHandler scoreHandler = renderer.GetComponent<IRFHighScoreHandler>();
				if( scoreHandler != null)
				{
					renderer.name = HighScoreRendererPrefab.name + "_" + (cnt+1).ToString();
					renderer.transform.SetParent( HighScoresContainer, false );
					RFHighScore highScore = scores[ cnt ];
					scoreHandler.SetHighScoreData( cnt + 1, highScore );
					renderer.SetActive( true );

					//  Check to see if the high score is a recent one added
					//  If so, there is a little house keeping to happen
					if( mostRecentHighScore == highScore && mostRecentHighScore != null )
					{
						scoreHandler.EditHighScoreStart( SelectableCharacters, HandleScoreEditUpdate );
						if( currentHighScoreEvent != null )
							currentHighScoreEvent.HighScoreRenderer = renderer;

						highScoreHandler = scoreHandler;
					}
				}
			}

		}
		// High Score Edit Update callback used by Renderers
		// of importance: rendererSubmit allows the renderer the ability to end rendering vs. using the
		// input axis management.
		public void HandleScoreEditUpdate(GameObject highScoreRenderer, bool rendererSubmit=false)
		{
			if( currentHighScoreEvent != null && !rendererSubmit )
			{
				currentHighScoreEvent.EventType = RFHighScoreEventType.RFHIGH_SCORE_EDIT_UPDATE;
				onRFHighScoreEvent.Invoke( currentHighScoreEvent );
			}
			else if( rendererSubmit )
			{
				endHighScoreEdit();
			}

		}

		// Clear All Scores in the manager and in the display
		public void ClearScores()
		{
			HighScoresManager.ClearAllScores();
			HighScoresManager.SaveHighScores( HighscoresFile );
			displayHighScores();
		}

		// Add a score to the list; if it is a high score go into "collect initials" mode
		public bool AddScore(int score)
		{
			RFHighScore scoreData = HighScoresManager.AddNewScore( score, NewScoreInitials );
			// If it was a high score, reset the display..
			if( scoreData != null )
			{

				mostRecentHighScore = scoreData;
				currentHighScoreEvent = new RFHighScoreEventData();
				currentHighScoreEvent.HighScoreData = scoreData;

				displayHighScores();
				// Let other systems know a new high score was just submitted
				rfHighScoreEvent.Invoke( currentHighScoreEvent );

			}

			return scoreData != null;
		}

		// Load the scores up and generate renderers
		public void LoadAndDisplayScores()
		{
			HighScoresManager.LoadHighScores(HighscoresFile, AutosetDefaultScores);
			displayHighScores();

		}
		// Convenience save function
		// Automatically passes the HighScoreFile to the scores manager save function
		public void SaveScores()
		{
			HighScoresManager.SaveHighScores(HighscoresFile);
		}
	}
}
