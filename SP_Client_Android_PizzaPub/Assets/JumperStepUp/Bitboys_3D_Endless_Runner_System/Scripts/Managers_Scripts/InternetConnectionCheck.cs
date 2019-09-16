using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 3D ENDLESS RUNNER SYSTEM By BITBOYS STUDIO.
public class InternetConnectionCheck : MonoBehaviour { // This script will check for internet connection to avoid Unity Ads problems.

	public static bool unityAdsInitialized = false;//can be used for informing the user about the status
	public bool checkingInternet = false;
	private LevelManager manager;

		void Awake () 
	{
		manager = FindObjectOfType<LevelManager> ();
	}

	void Update(){

		if (manager.inGameOverScene && checkingInternet == false) {


			StartCoroutine("checkInternetConnection");

			checkingInternet = true;

		}

		if (!manager.inGameOverScene) {

			checkingInternet = false;
		}
	}

	public IEnumerator checkInternetConnection()
	{
		float timeCheck = 2.0f;//will check google.com every two seconds
		float t1;
		float t2;
		while(!unityAdsInitialized)
		{
			WWW www = new WWW("http://google.com");
			t1 = Time.fixedTime;
			yield return www;
			if (www.error == null)//if no error
			{
				#if UNITY_ANDROID
				//Advertisement.Initialize(androidGameID); // initialize Unity Ads.
				#elif UNITY_IOS
				#endif

				unityAdsInitialized = true;

				break;//will end the coroutine
			}
			t2 = Time.fixedTime - t1;
			if(t2 < timeCheck)
				yield return new WaitForSeconds(timeCheck - t2);
		}
	} 


}