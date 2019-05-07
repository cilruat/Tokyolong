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

namespace TwoCars
{
	/// <summary>
	/// Parallax scrolling script that should be assigned to a layer
	/// </summary>
	public class ScrollScript : MonoBehaviour
	{
	    /// <summary>
	    /// Scrolling speed
	    /// </summary>
	    public float speed ;

	    /// <summary>
	    /// Moving direction
	    /// </summary>
	    private Vector2 direction = Vector2.down ;

	    public Transform background1, background2;


		void Update()
		{	
			if (Managers.UI.isStop)
				return;
			
			ScrollBg ();
		}

	    void ScrollBg()
	    {
	        // Movement
			Vector2 movement = direction * Managers.Difficulty.obstacleSpeed * 1 / Managers.Difficulty.spawnInterval;

	        movement *= Time.deltaTime;

	        background1.Translate(movement);
	        background2.Translate(movement);
	    }
	}
}