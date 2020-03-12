using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/* 
 * Example, default renderer.  May be used as a base to create more advanced and interesting renderer behaviors
 * Renderer for a high score and Initials.  Displays initials and high score;
 * If the score is a NEW high score, then the player is allowed to enter their initials.
 * Also, The "selected, working" initial/letter is displayed in NewScoreColor as is the score text in the high score 
 * 
 * Player uses up/down (vertical axis change) to select a character 
 * Player uses left/right (horizontal axis change) to change the position of the caret (selected, working letter/initial)
 * Player uses fire( fire1 axis change) to end editting - this is managed / forced by the RFHighScoreViewer class
 * 
 * Implements the IRFHighScoreHandler, as required by the high score viewer
 * 
 */ 
namespace RFLib
{
	public class RFHighScoreRenderer : MonoBehaviour, IRFHighScoreHandler 
	{
		public Text ScoreLabel;		// Text that displays the score
		public Text InitialsLabel;	// Text that displays the Initials

		[Tooltip("Delay between input update checks. Effects the selection speed of Initials.")]
		public float InputDelay			 = 0.4f;			// Wait time between input up/down/left/right handling

		[Tooltip("Input axis measured to move selection left or right.")]
		public string MoveCaretAxis 	= "Horizontal";

		[Tooltip("Input axis measured to scroll up or down through the selectable characters.")]
		public string RotateCharsAxis	= "Vertical";

		[Tooltip("Color to display new, high score text.")]
		public Color32 NewScoreColor    = new Color32(128,128,128,255);


		protected string newHighScoreHTMLColor;		// New High Score Color value - set on Awake
		protected RFHighScore workingHighScore;		// High score passed to this renderer
		protected string selectableCharacters 	= "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890_!$";  // Default selectable characters; updated during EditStart
		protected bool isEditMode				= false; // editMode Flag -- true if the renderered data is a high score



		string workingValue				= "";				// The string of characters to tweak

		int currentCaretIndex		 	= 0;	 			// location of caret in string (indicates which character is being edited)
		int currentSelectedCharIndex 	= 0;				// Index to the selected character to select from SelectCharacters list

		float currentInputDelay 	 	= 0.1f;				// time.delta time countdown mechanism so that input key presses/axis reads don't go too fast

		RFHighScoreUpdateCallback	updateCallbackFunc;			// If not null; call it when updates happen

		#region IRFHighScoreHandler Implementation
		// IRFHighScoreHandler methods marked as virtual so this class is easily extendable.  If extending with custom
		// classes, don't forget to call these base methods when overriden!

		virtual public void SetHighScoreData(int scoreIndex, RFHighScore rfHighScore)
		{
			if( rfHighScore == null )		return;

			workingHighScore = rfHighScore;
			workingValue = rfHighScore.Initials;
			UpdateColorHighScoreColor();

		}

		virtual public void EditHighScoreStart(string valid_chars, RFHighScoreUpdateCallback updateCallback)
		{
			updateCallbackFunc = updateCallback;
			 
			if( !string.IsNullOrEmpty( valid_chars ) )
				selectableCharacters = valid_chars;
			isEditMode = true;

			displayScore();

		}
		// HighScoreViewer indicates the player has finished editing (Pressed fire / submit / whatever )
		// So, do some cleanup
		virtual public void EditHighScoreComplete()
		{
			updateCallbackFunc = null;
			isEditMode = false;
			displayScore();
		}

		#endregion

		public void UpdateColorHighScoreColor()
		{
			// Cache the color string.
			newHighScoreHTMLColor = string.Format("<color='#{0}'>", RFUtils.ColorToHex( NewScoreColor ));
			displayScore();

		}

		void Awake()
		{
			

		}

		// Use this for initialization
		void Start () 
		{

		}



		// Update the score and initials labels
		// If in Edit Mode; change the color of the currently selected initial
		virtual protected void displayScore()
		{
			if( ScoreLabel != null && InitialsLabel != null && workingHighScore != null)
			{
				string letters = workingHighScore.Initials;
				string scoreVal = workingHighScore.HighScore.ToString();

				if( isEditMode )
				{
					

					letters = letters.Insert( currentCaretIndex + 1, "</color>" );
					letters = letters.Insert( currentCaretIndex, newHighScoreHTMLColor );
					scoreVal = newHighScoreHTMLColor + scoreVal + "</color>";

				}

				ScoreLabel.text = scoreVal;
				InitialsLabel.text = letters;
			}

		}
		
		// Update is called once per frame
		void Update () 
		{
			if( isEditMode )
			{
				currentInputDelay -= Time.deltaTime;
				// Enough time has gone by to manage some input
				if( currentInputDelay < 0 )
				{	

					// Move Index Right
					if( Input.GetAxis( MoveCaretAxis ) > 0 )
					{
						updateCaretIndex( 1 );	
					}
					// Move Index Left
					else if( Input.GetAxis( MoveCaretAxis ) < 0 )
					{
						updateCaretIndex( -1 );	
					}
					// Scroll current initials character forward through Selected Characters
					else if( Input.GetAxis( RotateCharsAxis ) > 0 )
					{
						updateInitialValue( 1 );
					}
					// Scroll current initials character backwards through Selected Characters
					else if( Input.GetAxis( RotateCharsAxis ) < 0 )
					{
						updateInitialValue( -1 );
					}
				}
			}
		}




		// Update the current initial we want to change
		void updateCaretIndex(int direction)
		{
			int maxInitialsCount 		 = workingValue.Length;			
			if( workingHighScore != null)
			{
				// Update the current Initials reference index; based on the Initial; set the currentSelectedCharIndex
				currentCaretIndex += direction;
				if( currentCaretIndex < 0 )
					currentCaretIndex = maxInitialsCount - 1;
				else if( currentCaretIndex >= maxInitialsCount )
					currentCaretIndex = 0;

				string initial = workingHighScore.GetInitialAt(currentCaretIndex);

				currentSelectedCharIndex = selectableCharacters.IndexOf(initial);

				displayScore();

				if( currentSelectedCharIndex < 0 )		currentSelectedCharIndex = 0;
			}
			if( updateCallbackFunc != null )		updateCallbackFunc( this.gameObject );
			currentInputDelay = InputDelay; // Reset the input delay

		}

		// Update the initial itself, based on the selected character from SelectCharacters
		void updateInitialValue(int direction)
		{
			if( workingHighScore != null)
			{
				// Update the current selected position
				currentSelectedCharIndex += direction;

				// "scroll" through possible selections
				if( currentSelectedCharIndex < 0 )	
					currentSelectedCharIndex = selectableCharacters.Length - 1;
				if( currentSelectedCharIndex >= selectableCharacters.Length )
					currentSelectedCharIndex = 0;


				string currentCharacter = selectableCharacters.Substring( currentSelectedCharIndex, 1 );

				workingHighScore.SetInitialAt(currentCaretIndex, currentCharacter);
				displayScore();
			}

			if( updateCallbackFunc != null )		updateCallbackFunc( this.gameObject );
			currentInputDelay = InputDelay; // Reset the input delay


		}






	}
}