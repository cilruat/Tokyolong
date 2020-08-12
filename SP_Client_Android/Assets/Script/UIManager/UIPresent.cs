using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UIPresent : MonoBehaviour {

    public eUI uiType;

    public Text[] txtTableNum;
    public CountDown countdown;
    public GameObject objSelect;

    public void ShowPresentTable()
    {
        byte tableNum = 0;
        if (Info.myInfo.listPresentInfo.Count > 0)
        {
            UserPresentInfo info = Info.myInfo.listPresentInfo[Info.myInfo.listPresentInfo.Count - 1];
            tableNum = info.tableNo;
        }

        txtTableNum[0].text = tableNum.ToString();

        UITweenAlpha.Start(objSelect, 0f, 1f, TWParam.New(.5f).Curve(TWCurve.CurveLevel2));
    }


    public void OnClose()
    {
        countdown.Stop();
        UIManager.Instance.Hide(eUI.ePresent);
    }

    public void OnGoMailScene()
    {
        UIManager.Instance.Hide(eUI.ePresent);
        SceneChanger.LoadScene("Mail", objSelect);
    }

}
