using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HumorVideoControll : MonoBehaviour {


	public MediaPlayerCtrl scrMedia;
	public GameObject objTitle;
	public GameObject objPlay;
	public GameObject objPause;
	public GameObject objVideo;

	bool isPlay = false;
	bool first = false;

	void Awake()
	{
		if (scrMedia != null) {
			string file = "Movie";
			switch (SceneManager.GetActiveScene ().name) {
			case "PicturePuzzle":			file += "PicturePuzzle";	break;
			case "PairCards":				file += "PairCards";		break;
			case "FlappyBirdMasterMain":	file += "FlyBird";			break;
			case "CrashCatStart":			file += "CrashCat";			break;
			case "TouchNumber":				file += "TouchNumber";		break;
			case "FindDiffPicture":			file += "FindDiffPicture";	break;
			}

			scrMedia.m_strFileName = file + ".mp4";
		}

		objPlay.SetActive (true);
		objPause.SetActive (false);

		Click ();
	}

	public void Click()
	{
		if (scrMedia == null)
			return;

		if (isPlay) {
			scrMedia.Pause ();
			objPause.SetActive (true);
		} else {
			scrMedia.Play ();
			objPause.SetActive (false);
		}

		isPlay = !isPlay;

		if (first == false)
			StartCoroutine (_Delay ());
	}

	IEnumerator _Delay()
	{
		first = true;
		yield return new WaitForSeconds (.5f);

		if (objTitle.activeSelf)	objTitle.SetActive (false);
		if (objPlay.activeSelf)		objPlay.SetActive (false);

		UITweenAlpha.Start (objVideo, 1f, TWParam.New (.5f).Curve (TWCurve.CurveLevel2));
	}
}
