using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIGameWaiting : MonoBehaviour {

    public eUI uiType;

    public Text txtTableNum;

    public GameObject objSelect;


    public Animator anim;

    public void ShowWaiting()
    {
        byte tableNum = 0;

        if (Info.myInfo.listGameAcceptInfo.Count > 0)
        {
            UserGameAcceptInfo info = Info.myInfo.listGameAcceptInfo[Info.myInfo.listGameAcceptInfo.Count - 1];
            tableNum = info.tableNo;
        }

        txtTableNum.text = tableNum.ToString();

        //UITweenAlpha.Start(objSelect, 0f, 1f, TWParam.New(.5f).Curve(TWCurve.CurveLevel2));
        anim.Play("VersusWaiting");
        StartCoroutine(_DestroyShadow());
    }



    IEnumerator _DestroyShadow()
    {
        yield return new WaitForSeconds(4f);
        UIManager.Instance.Hide(eUI.eGameWaiting);
        VersusManager.Instance.LoadPage();
    }



}
