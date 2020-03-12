using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

// High Scores renderer that uses an InputField to collect high score names

namespace RFLib
{
	public class RFHighScoreRendererInputField : MonoBehaviour, IRFHighScoreHandler 
	{
		public Text ScoreIndex;
		public Text ScoreLabel;		// Text that displays the score
		public Text InitialsLabel;	// Text that displays the Initials 
		public InputField NameField; 


		RFHighScore workingHighScore;
		RFHighScoreUpdateCallback	updateCallbackFunc;			// If not null; call it when updates happen

		#pragma warning disable 0414
		string valid_characters;
		#pragma warning restore 0414


		#region IRFHighScoreHandler interface
		virtual public void SetHighScoreData(int scoreIndex, RFHighScore rfHighScore)
		{
			if( rfHighScore == null )		return;
			workingHighScore = rfHighScore;
			InitialsLabel.text = workingHighScore.Initials;
			ScoreIndex.text = scoreIndex.ToString();
			ScoreLabel.text = workingHighScore.HighScore.ToString();
			editMode( false );
		}

		virtual public void EditHighScoreStart(string valid_chars, RFHighScoreUpdateCallback updateCallback)
		{

			valid_characters = valid_chars;
			updateCallbackFunc = updateCallback;
			editMode( true );

			EventSystem.current.SetSelectedGameObject( NameField.gameObject );
			NameField.ActivateInputField();
			NameField.text = workingHighScore.Initials;
			NameField.onValueChanged.AddListener( HandleEditorChange );
			NameField.onEndEdit.AddListener( handleEditEnd );


		}
		// HighScoreViewer indicates the player has finished editing (Pressed fire / submit / whatever )
		// So, do some cleanup
		virtual public void EditHighScoreComplete()
		{
			NameField.onValueChanged.RemoveListener( HandleEditorChange );
			NameField.onEndEdit.RemoveListener( handleEditEnd );
			updateCallbackFunc = null;
			workingHighScore.Initials = NameField.text;

			editMode( false );
			InitialsLabel.text = NameField.text;
		}

		#endregion

		void editMode(bool tf)
		{
			InitialsLabel.gameObject.SetActive( !tf );
			NameField.gameObject.SetActive( tf );
		}

		void HandleEditorChange(string newValue)
		{
			
			if( updateCallbackFunc != null )
				updateCallbackFunc( this.gameObject );
		}
		void handleEditEnd(string newValue)
		{
			if( updateCallbackFunc != null )
				updateCallbackFunc( this.gameObject, true );
		}
		// Use this for initialization
		void Start () 
		{
		
		}
		
		// Update is called once per frame
		void Update () 
		{
		
		}
	}
}