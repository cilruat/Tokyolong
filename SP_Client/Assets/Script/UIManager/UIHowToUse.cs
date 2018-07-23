using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHowToUse : MonoBehaviour 
{
    public List<RectTransform> listObjMain;
    public List<RectTransform> listObjDesc;

    public List<float> listDescFinalY = new List<float>();

    bool skip = false;
    int idx = 0;
    Coroutine _anim = null;

    void OnEnable()
    {
        for (int i = 0; i < listObjMain.Count; i++)
        {
            listObjMain[i].gameObject.SetActive(false);
            listObjDesc[i].gameObject.SetActive(false);
        }

        skip = false;
        idx = 0;
        _anim = StartCoroutine(_Anim());
    }

    IEnumerator _Anim () 
    {
        for (int i = idx; i < listObjMain.Count; i++)
        {
            idx = i;
            UITweenAlpha.Start(listObjMain[i].gameObject, 0f, 1f, TWParam.New(skip ? .1f : 1f).Curve(TWCurve.CurveLevel4).Speed(TWSpeed.Faster));
            UITweenAlpha.Start(listObjDesc[i].gameObject, 0f, 1f, TWParam.New(skip ? .1f : 1f, skip ? .1f : 1f).Curve(TWCurve.CurveLevel4).Speed(TWSpeed.Faster));
            UITweenPosY.Start(listObjDesc[i].gameObject, listDescFinalY[i]-20f, listDescFinalY[i], TWParam.New(skip ? .1f : 1f, skip ? .1f : 1f).Curve(TWCurve.CurveLevel4).Speed(TWSpeed.Faster));

            yield return new WaitForSeconds(skip ? .25f : 2f);
        }

        _anim = null;
	}

    public void OnClose()
    {
        UIManager.Instance.Hide(eUI.eHowToUse);
    }

    public void TweenSkip()
    {
        skip = true;

        if (_anim != null)
            StopCoroutine(_anim);
        
        UITweenAlpha.Start(listObjMain[idx].gameObject, listObjMain[idx].GetComponent<CanvasGroup>().alpha , 1f, TWParam.New(.1f).Curve(TWCurve.CurveLevel4).Speed(TWSpeed.Faster));
        UITweenAlpha.Start(listObjDesc[idx].gameObject, listObjMain[idx].GetComponent<CanvasGroup>().alpha, 1f, TWParam.New(.1f).Curve(TWCurve.CurveLevel4).Speed(TWSpeed.Faster));
        UITweenPosY.Start(listObjDesc[idx].gameObject, listObjDesc[idx].anchoredPosition.y , listDescFinalY[idx], TWParam.New(.1f, .1f).Curve(TWCurve.CurveLevel4).Speed(TWSpeed.Faster));

        _anim = StartCoroutine(_Anim());
    }
}
