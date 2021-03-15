using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class PopupWebView {

	#if UNITY_IPHONE
	[DllImport("__Internal")]
	public static extern void _HelloFromUnity();
	[DllImport("__Internal")]
	public static extern void _FullWebView(string url);	
	[DllImport("__Internal")]
	public static extern void _CustomWebView(string url,bool isFull,float width,float height);
	#endif

	/// <summary>
	/// Display Full Screen Webview
	/// </summary>
	/// <param name="url">Url</param>
	public static void FullWebView(string url)
	{
		#if UNITY_EDITOR
			
		#elif UNITY_ANDROID 
		AndroidJavaClass unity = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
		AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject> ("currentActivity");
		AndroidJavaClass pluginclass = new AndroidJavaClass ("com.johanfayt.PopupWebview");		
		pluginclass.CallStatic ("PopUpWebView",currentActivity,url,true,0,0);
		#elif UNITY_IPHONE
			
		_FullWebView (url);

		#endif

	}

	/// <summary>
	/// Display Custom Screen Webview
	/// </summary>
	/// <param name="url">URL</param>
	/// <param name="isFull">If set to <c>true</c> is full.</param>
	/// <param name="width">Width</param>
	/// <param name="height">Height</param>
	public static void CustomWebView(string url,bool isFull,int width,int height)
	{
		#if UNITY_EDITOR
			
		#elif UNITY_ANDROID
		AndroidJavaClass unity = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
		AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject> ("currentActivity");
		AndroidJavaClass pluginclass = new AndroidJavaClass ("com.johanfayt.PopupWebview");		
		pluginclass.CallStatic ("PopUpWebView",currentActivity,url,isFull,width,height);
		#elif UNITY_IPHONE

		_CustomWebView (url, isFull, width, height);

		#endif
	}




}
