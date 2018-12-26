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
using System.Collections;

namespace TwoCars
{
	public class RestartButton : MonoBehaviour {

	    public void OnClickRestartButton()
	    {
	        Managers.Game.SetState(typeof(GamePlayState));
	        Managers.UI.inGameUI.gameOverPopUp.SetActive(false);
	        Managers.Spawner.ClearObstacles();       
	    }
	}	
}