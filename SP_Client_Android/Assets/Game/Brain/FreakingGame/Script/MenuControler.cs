/********************************************************************
 * Game : Freaking Game
 * Scene : Menu Game
 * Description : Menu Controler
 * History:
 *	2016/09/25	TungNguyen	First Edition
********************************************************************/
using UnityEngine;
using System.Collections;

namespace Freaking
{
public class MenuControler : MonoBehaviour
{

	#region Declare
	SoundManager _soundManager;
	/// <summary>
	/// Reference to the sound manager
	/// </summary>
	public SoundManager soundManager
	{
		get
		{
			if (_soundManager == null)
				_soundManager = FindObjectOfType<SoundManager>();

			return _soundManager;
		}
	}

	#endregion

	#region init
	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}
	#endregion init

	#region Method
	public void BtnMath()
	{
		cMgrCommon.GamePlay = 0;
		Play_Event();
	}

	public void BtnWord()
	{
		cMgrCommon.GamePlay = 1;
		Play_Event();
	}

	private void Play_Event()
	{
		soundManager.PlaySoundPush();
		StartCoroutine(WaitToEvent(1));
	}

	public void Rate_Event()
	{
		soundManager.PlaySoundPush();
		StartCoroutine(WaitToEvent(2));
	}

	public void More_Event()
	{
		soundManager.PlaySoundPush();
		StartCoroutine(WaitToEvent(3));
	}

	public void Exit_Event()
	{
		soundManager.PlaySoundPush();
		StartCoroutine(WaitToEvent(4));
	}

	/// <summary>
	/// Open Url to rate game
	/// </summary>
	private void Rate()
	{
#if UNITY_IPHONE
		   Application.OpenURL(cMgrCommon.urlGameIphone);
#endif
#if UNITY_ANDROID
		Application.OpenURL(cMgrCommon.urlGameAndroid);
#endif
	}

	/// <summary>
	/// Link to appstore, playstore
	/// </summary>
	private void More()
	{
#if UNITY_IPHONE
    Application.OpenURL("");
#endif
#if UNITY_ANDROID
		Application.OpenURL("");	
#endif
	}

	private void Exit()
	{
		Application.Quit();
	}

    /// <summary>
    /// Wait 1.5s to next event
    /// </summary>
    /// <param name="index"></param>
    /// <returns>none</returns>
    IEnumerator WaitToEvent(int index)
	{
		yield return new WaitForSeconds(1.5f);
		switch (index)
		{
			case 1:
				//SceneManager.LoadScene("MainGame"); //Play
                Application.LoadLevel("MainGame");
                break;

			case 2:
				this.Rate();
				break;

			case 3:
				this.More();
				break;

			case 4:
				this.Exit();
				break;

			default:
				break;
		}
	}
	#endregion Method
}
}