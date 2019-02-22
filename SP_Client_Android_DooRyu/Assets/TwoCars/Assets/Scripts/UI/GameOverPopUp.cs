//  /*********************************************************************************
//   *********************************************************************************
//   *********************************************************************************
//   * Produced by Skard Games										                 *
//   * Facebook: https://goo.gl/5YSrKw											     *
//   * Contact me: https://goo.gl/y5awt4								             *
//   * Developed by Cavit Baturalp Gürdin: https://tr.linkedin.com/in/baturalpgurdin *
//   *********************************************************************************
//   *********************************************************************************
//   *********************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace TwoCars
{
	public class GameOverPopUp : MonoBehaviour {

	    public Text score,highScore;
	    
	    void OnEnable()
	    {
	        UpdateScoreUI();
	        Managers.UI.panel.SetActive(true);
	    }

	    public void BackToMainMenu()
	    {
	        Managers.Audio.PlayUIClick();
	        Managers.UI.panel.SetActive(false);
	        Managers.Game.SetState(typeof(MenuState));
	        gameObject.SetActive(false);
	    }

	    public void UpdateScoreUI()
	    {
	        score.text = "Score: " + Managers.Score.currentScore.ToString();
	        highScore.text = "High Score: " + Managers.Score.highScore.ToString();
	    }


	}
}