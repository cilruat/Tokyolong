using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UIAlarm : MonoBehaviour 
{
    public Text textAlarm;
    public Button btn;

    Coroutine routine = null;

    public void ShowAlarm(string text, UnityAction onCallBack)
    {
        textAlarm.text = text;
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(onCallBack);

        if (routine != null)
            StopCoroutine(routine);

        routine = StartCoroutine(_ShowAlarm());
    }

    public void HideAlarm()
    {
        if (routine != null)
        {
            StopCoroutine(routine);
            routine = null;
        }

        UITweenPosY.Start(this.gameObject, 60f, -100f, TWParam.New(.5f).Curve(TWCurve.CurveLevel4).Speed(TWSpeed.Faster));
    }

    IEnumerator _ShowAlarm()
    {
        UITween tween = UITweenPosY.Start(this.gameObject, -80f, 60f, TWParam.New(.5f).Curve(TWCurve.CurveLevel4).Speed(TWSpeed.Slower));

        while (tween.IsTweening())
            yield return null;

        yield return new WaitForSeconds(2f);

        tween = UITweenPosY.Start(this.gameObject, 60f, -100f, TWParam.New(.5f).Curve(TWCurve.CurveLevel4).Speed(TWSpeed.Faster));

        while (tween.IsTweening())
            yield return null;

        routine = null;
    }
}
