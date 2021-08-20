using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UIGameInvite : MonoBehaviour {


    public eUI uiType;
    public Text[] txtTableNum;
    public Text txtGameCnt;
    public Text txtGameName;

    public CountDown countdown;
    public GameObject objSelect;

    int inviteGameCnt = 0;
    string inviteGameName = "";


    public void ShowGameInviteTable()
    {
        byte tableNum = 0;

        if (Info.myInfo.listGameInfo.Count > 0)
        {
            UserGameInfo info = Info.myInfo.listGameInfo[Info.myInfo.listGameInfo.Count - 1];
            tableNum = info.tableNo;
            inviteGameCnt = info.reqGameCnt;
        }

        txtTableNum[0].text = tableNum.ToString();
        txtGameCnt.text = inviteGameCnt.ToString();

        UITweenAlpha.Start(objSelect, 0f, 1f, TWParam.New(.5f).Curve(TWCurve.CurveLevel2));
    }

    public void OnClose()
    {
        countdown.Stop();
        UIManager.Instance.Hide(eUI.eGameInvite);
    }

    public void OnGoMailScene()
    {
        UIManager.Instance.Hide(eUI.eGameInvite);
        SceneChanger.LoadScene("Mail", objSelect);
    }


}

