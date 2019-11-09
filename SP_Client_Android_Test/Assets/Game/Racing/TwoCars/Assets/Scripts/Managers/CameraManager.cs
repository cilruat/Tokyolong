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

public class CameraManager : MonoBehaviour {

	public Camera main;

    //Allows us to shake camera 
    [HideInInspector]
	public CameraShake shaker;

    void Awake()
	{
		shaker = main.gameObject.GetComponent<CameraShake> ();
    }


}
