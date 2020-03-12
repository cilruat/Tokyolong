using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;

using RFLib;

namespace RFLibDemo
{
	
	public class RFHighScoresDemo : MonoBehaviour 
	{
		
		public Transform HighScoresContainer;

		public RFHighScoreViewer RFHighScores;
		public InputField ScoreInput;


		// Use this for initialization
		void Start () {
		
		}
		
		// Update is called once per frame
		void Update () 
		{
		}

		// Scene buttons call this method on click
		// Hooked up to each button in the scene (hookup via unity's event system)
		public void HandleButtons(string request)
		{
			if( RFHighScores == null )
			{
				Debug.Log( "High Scores viewer is not hooked up?!" );
				return;
			}

			if( request == "show_scores" )
			{
				Debug.Log( "Show the scores!" );
				HighScoresContainer.gameObject.SetActive( true );
			} 
			else if( request == "hide_scores" )
			{
				Debug.Log( "HIde the scores!" );
				HighScoresContainer.gameObject.SetActive( false );
			} 
			else if( request == "add_score" )
			{
				string score = ScoreInput.text;
				if( string.IsNullOrEmpty( score ) )
				{
					Debug.Log( "Not score entered! Enter a score first." );
					return;
				}

				int scoreValue = Convert.ToInt32( score );
				RFHighScores.AddScore( scoreValue );

				Debug.Log( "Add a new score! " + ScoreInput.text );

			} 
			else if( request == "clear_score" )
			{
				RFHighScores.ClearScores();
				Debug.Log( "Clear all scores!" );
			} 

			else if( request == "load_scores" )
			{
				RFHighScores.LoadAndDisplayScores();
			}
			else if( request == "save_scores" )
			{
				RFHighScores.SaveScores();
			}

			// Deselect the button; 
			if( EventSystem.current.currentSelectedGameObject )
			{
				EventSystem.current.SetSelectedGameObject( null );
			}

		}

		// The following event handler is connected via the RFHighScoreEvent dispatch found in RFHighScoreViewer
		public void HandleRFHighScoreEvent(RFHighScoreEventData highScoreEvent)
		{

			switch( highScoreEvent.EventType )
			{
				case RFHighScoreEventType.RFHIGH_SCORE_EDIT_DONE:
					Debug.Log( "Player is done editing! Play big sounds and make lots of visual fx!" );
					break;
				case RFHighScoreEventType.RFHIGH_SCORE_NEW:
					Debug.Log( "Player has a new high score! Draw their attention to it and let them know then can edit their initials!" );
					break;
				case RFHighScoreEventType.RFHIGH_SCORE_EDIT_UPDATE:
					Debug.Log( "Player is editing their initials/Name!!" );
					break;



			}
		}
	}
}
