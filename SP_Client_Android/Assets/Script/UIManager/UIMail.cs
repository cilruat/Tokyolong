﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIMail : MonoBehaviour {

    public eUI uiType;

    public CountDown countdown;
    public GameObject objSelect;
    public GameObject objContent;


    public ScrollRect srBoard;
    public MailElt mailElt;


    public void ShowMsgTable()
    {

        /*
        CanvasGroup cgSelect = objSelect.GetComponent<CanvasGroup>();
        if (cgSelect != null)
            cgSelect.alpha = 1f;

        CanvasGroup cgCongent = objSelect.GetComponent<CanvasGroup>();
        if (cgCongent != null)
            cgCongent.alpha = 0f;*/

        UITweenAlpha.Start(objSelect, 0f, 1f, TWParam.New(.5f).Curve(TWCurve.CurveLevel2));
        UITweenAlpha.Start(objContent, 1f, 0f, TWParam.New(.5f, .5f).Curve(TWCurve.CurveLevel2));

        objSelect.SetActive(true);
        objContent.SetActive(false);
    }

    //열때 모양을 결정하주는거랑 시간
    void _ShowContent()
    {
        UITweenAlpha.Start(objSelect, 1f, 0f, TWParam.New(.5f).Curve(TWCurve.CurveLevel2));
        UITweenAlpha.Start(objContent, 0f, 1f, TWParam.New(.5f, .5f).Curve(TWCurve.CurveLevel2));
    }



    public void AddMailElt(UserMsgInfo userMsgInfo)
    {
        MailElt elt = CreateMailElt();
        elt.SetMailElt(userMsgInfo);
    }

    MailElt CreateMailElt()
    {
        GameObject newObj = Instantiate(mailElt.gameObject) as GameObject;
        newObj.transform.SetParent(srBoard.content);
        newObj.transform.InitTransform();
        MailElt newElt = newObj.GetComponent<MailElt>();

        if (newElt == null)
            return null;

        return newElt;
    }


    public void OnShowContent()
    {
        countdown.Stop();
        _ShowContent();
    }

    public void OnClose()
    {
        countdown.Stop();
        UIManager.Instance.Hide(eUI.eMail);
    }

    public void OnGoMailScene()
    {
        SystemMessage.Instance.Add("다음주에 쪽지기능이 나와요~ 지금은 테스트중입니다");
        //SceneChanger.LoadScene("Mail", gameObject);
    }

    public void OnCloseContent()
    {
        UIManager.Instance.Hide(eUI.eMail);
    }

    //코루틴 들어가고 그런거 없이 Content에 내용만 쪽지에 맞게 적용만한다면 완료되는거 같은데 그다음에 리스트에 넣어주고 ++ 해주고
    //쪽지의 내용을 이제 여기서 뿌려주어야하는데 Distpath에서 어떻게 연결하까
}
