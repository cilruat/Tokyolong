using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SystemMessage : SingletonMonobehaviour<SystemMessage> {

	public GameObject msgPrefab;
	int msgCount = 0;

	public void Add (string desc)
	{
		gameObject.SetActive (true);
		StartCoroutine (ShowMsg (desc));
	}

	const float SHOW_HEIGHT = .13F;
	IEnumerator ShowMsg (string msg)
	{
		msgCount++;

		GameObject newMsg = GameObject.Instantiate(msgPrefab, transform, true) as GameObject;
		newMsg.name = msgPrefab.name;
		newMsg.SetActive(true);

		RectTransform rt = (RectTransform)newMsg.transform;
		Vector3 change_pos = Vector3.zero;
		rt.anchoredPosition = new Vector2 (change_pos.x, change_pos.y - 270f);

		Text textMsg = newMsg.GetComponentInChildren<Text>();
		textMsg.text = msg;

		rt.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, textMsg.preferredWidth + 20f);
		rt.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, textMsg.preferredHeight + 10f);

		UITweenAlpha.Start(newMsg, 0f, 1f, TWParam.New(.2f).Curve(TWCurve.Back));
		UITweenScale.Start(newMsg, .5f, 1f, TWParam.New(.2f).Curve(TWCurve.Back));

		float showTime = .2f + Mathf.Max (.8f, msg.Length * .04f);
		yield return new WaitForSecondsRealtime(showTime);

		UITweenPosY.Start(newMsg, rt.anchoredPosition.y + 40f, TWParam.New(.6f).Curve(TWCurve.CurveLevel2).Speed(TWSpeed.Faster));
		UITweenAlpha.Start(newMsg, 0f, TWParam.New(.6f).Curve(TWCurve.CurveLevel2).Speed(TWSpeed.Faster).DestroyOnFinish()).AddCallback(TweenCompleteMsg);
	}

	void TweenCompleteMsg ()
	{
		msgCount--;
		CheckHide();
	}

	void CheckHide ()
	{
		if (msgCount <= 0)
			gameObject.SetActive(false);
	}
}
