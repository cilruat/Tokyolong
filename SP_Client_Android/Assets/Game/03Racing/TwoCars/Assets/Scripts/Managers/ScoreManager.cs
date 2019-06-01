//  /*********************************************************************************
//   *********************************************************************************
//   *********************************************************************************
//   * Produced by Skard Games										                  *
//   * Facebook: https://goo.gl/5YSrKw											      *
//   * Contact me: https://goo.gl/y5awt4								              *											
//   * Developed by Cavit Baturalp Gürdin: https://tr.linkedin.com/in/baturalpgurdin *
//   *********************************************************************************
//   *********************************************************************************
//   *********************************************************************************/

using UnityEngine;
using System.Collections;

namespace TwoCars
{
	public class ScoreManager : MonoBehaviour {

		public int currentScore=0;
		public int highScore;

	    void Awake()
	    {
	        if (Managers.Game.stats.highScore != 0)
	        {
	            highScore = Managers.Game.stats.highScore;
	        }

	        else
	        {
	            highScore = 0;
	        }
	    }

		public void OnScore(int scoreIncreaseAmount)
		{	
			currentScore += scoreIncreaseAmount;
	        //Managers.UI.inGameUI.UpdateScoreUI();
			Managers.UI.SetScore(currentScore);
	        Managers.Game.stats.totalScore += scoreIncreaseAmount;
	    }

	    public void CheckHighScore()
	    {
	        if (highScore < currentScore)
	        {
	            highScore = currentScore;
	        }
	    }

	    public void ResetScore()
	    {
	        currentScore = 0;
	        highScore = Managers.Game.stats.highScore;
	        Managers.UI.inGameUI.UpdateScoreUI();
	    }

	}
}