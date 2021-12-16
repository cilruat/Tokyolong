using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UIGameInvite : MonoBehaviour {

    //UIManager에서 뜨는 화면 1vs1 게임은 여기서 최종처리

    public eUI uiType;
    public Text[] txtTableNum;
    public Text txtGameCnt;
    public Text txtGameName;

    public CountDown countdown;
    public GameObject objSelect;

    int inviteGameCnt;
    string inviteGameName;

    byte tableNum = 0;

    public void ShowGameInviteTable()
    {

        if (Info.myInfo.listGameInfo.Count > 0)
        {
            UserGameInfo info = Info.myInfo.listGameInfo[Info.myInfo.listGameInfo.Count - 1];
            tableNum = info.tableNo;
            inviteGameCnt = info.reqGameCnt;
            inviteGameName = info.gameName;

            //여기서 info는 
        }

        txtTableNum[0].text = tableNum.ToString();
        txtGameCnt.text = inviteGameCnt.ToString();
        txtGameName.text = inviteGameName.ToString();

        UITweenAlpha.Start(objSelect, 0f, 1f, TWParam.New(.5f).Curve(TWCurve.CurveLevel2));
    }

    public void OnClose()
    {
        countdown.Stop();
        UIManager.Instance.Hide(eUI.eGameInvite);
        NetworkManager.Instance.Game_Refuse_REQ(tableNum);

    }

    public void OnAcceptGame()
    {
        UIManager.Instance.Hide(eUI.eGameInvite);
        //씬체인지는 ACK에서 NOT에서 시켜지는지 확인할것
        NetworkManager.Instance.Game_Accept_REQ(Info.TableNum, tableNum, inviteGameCnt, inviteGameName);
    }


}

