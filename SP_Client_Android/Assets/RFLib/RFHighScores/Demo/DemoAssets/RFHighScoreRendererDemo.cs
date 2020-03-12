using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using RFLib;

/* Renderer for a high score, initials and an "index" into the score location in the leaderboard/scores list
 * 
 * Extends RFHighSCoreRenderer for convenience
 * 
 */ 
namespace RFLibDemo
{
	public class RFHighScoreRendererDemo : RFHighScoreRenderer
	{
		public Text ScoreIndex;		// Text display of leaderboard / score index
		int workingScoreIndex;


		// Override the Set High Score data so this component display more information (the Score Index)
		override public void SetHighScoreData(int scoreIndex, RFHighScore rfHighScore)
		{
			if( rfHighScore == null )		return;
			workingScoreIndex = scoreIndex;
			base.SetHighScoreData( scoreIndex, rfHighScore );
		}

		// DisplaySCore needs to take into account the Index data too!
		override protected void displayScore()
		{
			if( ScoreIndex != null )
			{
				string ndx = workingScoreIndex.ToString();
				// If in edit mode, change everyone's color!

				if( isEditMode )
				{
					ndx =  newHighScoreHTMLColor+ ndx + "</color>";

				}

				ScoreIndex.text =  ndx;

				base.displayScore();

			}

		}
		




	}
}