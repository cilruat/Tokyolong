using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public enum EAlarmType
{
	eMessage = 0,
	eHeart,
	eGift,
	eGameVSAlarm,
	eNotice,
	eTuto,
}

public class UIAlarm : MonoBehaviour 
{
	public RawImage imgAlarmCat;
	public Text textAlarm;

    public Button btn;

	public Button btnChat;
	public GameObject objAlarmChat;

	public List<Texture> listAlarmCat;

    Coroutine routine = null;
	Coroutine routineChat = null;

	public void ShowAlarm(EAlarmType type, string text, UnityAction onCallBack)
    {
		imgAlarmCat.texture = listAlarmCat [(int)type];
        textAlarm.text = text;
        btn.onClick.RemoveAllListeners();

        if(onCallBack != null)
            btn.onClick.AddListener(onCallBack);

        if (routine != null)
            StopCoroutine(routine);

        routine = StartCoroutine(_ShowAlarm());
    }

	public void ShowAlarmChat(UnityAction onCallBack)
	{
		btnChat.onClick.RemoveAllListeners ();

		if (onCallBack != null)
			btnChat.onClick.AddListener (onCallBack);

		if (routineChat != null)
			StopCoroutine (routineChat);

		routineChat = StartCoroutine (_ShowAlarmChat ());
	}

    public void HideAlarm()
    {
        if (routine != null)
        {
            StopCoroutine(routine);
            routine = null;
        }

		UITweenPosX.Start(this.gameObject, 50f, -100f, TWParam.New(.5f).Curve(TWCurve.CurveLevel4).Speed(TWSpeed.Faster));
    }

    IEnumerator _ShowAlarm()
    {
        UITween tween = UITweenPosX.Start(this.gameObject, -100, 50f, TWParam.New(.5f).Curve(TWCurve.CurveLevel4).Speed(TWSpeed.Slower));

        while (tween.IsTweening())
            yield return null;

        yield return new WaitForSeconds(2f);

        tween = UITweenPosX.Start(this.gameObject, 50f, -100f, TWParam.New(.5f).Curve(TWCurve.CurveLevel4).Speed(TWSpeed.Faster));

        while (tween.IsTweening())
            yield return null;

        routine = null;
    }

	public void HideAlarmChat()
	{
		if (routineChat != null)
		{
			StopCoroutine(routineChat);
			routineChat = null;
		}

		UITweenPosX.Start(objAlarmChat, 50f, -100f, TWParam.New(.5f).Curve(TWCurve.CurveLevel4).Speed(TWSpeed.Faster));
	}

	IEnumerator _ShowAlarmChat()
	{
		UITween tween = UITweenPosX.Start(objAlarmChat, -100, 50f, TWParam.New(.5f).Curve(TWCurve.CurveLevel4).Speed(TWSpeed.Slower));

		while (tween.IsTweening())
			yield return null;

		routineChat = null;
	}
}
