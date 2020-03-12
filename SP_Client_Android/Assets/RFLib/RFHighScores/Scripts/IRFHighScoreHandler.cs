using UnityEngine;
using System.Collections;

namespace RFLib
{

	// Update Callback delegate definition;
	// HighScoreRenderer: The renderer the called the update
	// rendererSubmit:  The RENDERER itself is forcing a submit request (high score manager will go through editing stop)
	public delegate void RFHighScoreUpdateCallback(GameObject highScoreRenderer, bool rendererSubmit=false);

	/// <summary>
	/// Interface for RF High Score renderers.  Score Renderers server a couple purposes
	///  * Display Initials / Name and the Score
	///  * Act as an editing mechanism to capture name / initials
	///  * Indicate the entry number / list index; signifies leaderboard position
	/// Interface methods are utilized by the RFHighScoreViewer script to communicate with renderers.
	/// </summary>
	public interface IRFHighScoreHandler
	{
		/// <summary>
		/// Sets the high score data.
		/// </summary>
		/// <param name="scoreIndex">Score index.</param>
		/// <param name="rfHighScore">RfHighScore data</param>
		void SetHighScoreData(int scoreIndex, RFHighScore rfHighScore);

		/// <summary>
		/// Tell the renderer to enter High Score Edit Mode. "Nice" editors will make calls to the update callback
		/// as the player presses keys/updates data.  updateCallback is set by RFHighScoreViewer.
		/// </summary>
		/// <param name="valid_chars">Valid selectable characters that may be entered in a name, initals, etc</param>
		/// <param name="updateCallback">Callback set by the RFHighScoreViewer to furhter dispatch change events to listeners</param>
		void EditHighScoreStart(string valid_chars, RFHighScoreUpdateCallback updateCallback);

		/// <summary>
		/// High Score Name Editing Complete
		/// Called by the high scores manager when a user has triggered the "end of editing" mechanism OR as a response
		/// to the renderer passing TRUE to the rendererSubmit parameter in RFHighScoreUpdateCallback
		/// </summary>
		void EditHighScoreComplete();

	}
}