using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// YOU BETTER RUN By BITBOYS STUDIO.
public class RateButton : MonoBehaviour {


	public void RateUs()
	{
		#if UNITY_ANDROID
		Application.OpenURL("market://details?id=com.Bitboys.YouBetterRun"); // Put here your google play game URL to redirect the player to the google play store.
		#elif UNITY_IPHONE
		Application.OpenURL("itms-apps://itunes.apple.com/app/idYOUR_ID");
		#endif
	}

}
