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
using UnityEngine.UI;

namespace TwoCars
{
	public class SoundButton : MonoBehaviour {

	    public Sprite on, off;

	    public void TurnUpDownSound()
	    {
	        if (AudioListener.volume == 0)
	        {
	            GetComponent<Image>().sprite = on;
	            Managers.Audio.PlayUIClick();
	            AudioListener.volume = 1.0f;

	        }

	        else if (AudioListener.volume == 1.0f)
	        {
	            GetComponent<Image>().sprite = off;
	            Managers.Audio.PlayUIClick();
	            AudioListener.volume = 0f;
	        }
	    }
	}
}