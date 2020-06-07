/********************************************************************
 * Game : Freaking Game
 * Scene : Common
 * Description : Util
 * History:
 *	2016/09/25	TungNguyen	First Edition
********************************************************************/
using UnityEngine;


namespace NumberTest
{
class Util
{
	/// <summary>
	/// Check internet connection
	/// </summary>
	/// <returns>true/false</returns>
	public static bool isConnect()
	{
		bool ret = false;

#if UNITY_EDITOR_WIN
		if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork || Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
		{
			ret = true;
		}
		else
		{
			ret = false;
		}
#endif
#if UNITY_IPHONE
		if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork || Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
		{
			ret = true;
		}
		else
		{
			ret = false;
		}
#endif
#if UNITY_ANDROID
		if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork || Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
		{
			ret = true;
		}
		else
		{
			ret = false;
		}
#endif

		return ret;
	}

	public static string GetLanguage()
	{
		var language = Application.systemLanguage;
		string lang;
		switch (language)
		{
			case SystemLanguage.English:
				lang = "QuizEL";
				break;

			case SystemLanguage.Vietnamese:
				lang = "QuizVN";
				break;

			default:
				lang = "QuizEL";
				break;
		}

		return lang;
	}
}
}