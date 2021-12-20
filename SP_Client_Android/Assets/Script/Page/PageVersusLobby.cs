﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PageVersusLobby : MonoBehaviour {

    public GameObject objBoard;

    public Text txtMyTableNum;

    public Text[] txtTableNum;
    public Text txtReqGameCnt;
    public Text txtGameName;

    private void Start()
    {
        byte tableNum = 0;
        int GameCnt = 0;
        string GameName = "";

        if (Info.myInfo.listGameAcceptInfo.Count > 0)
        {
            UserGameAcceptInfo info = Info.myInfo.listGameAcceptInfo[Info.myInfo.listGameAcceptInfo.Count - 1];
            tableNum = info.tableNo;
            GameCnt = info.reqGameCnt;
            GameName = info.gameName;

        }
        txtMyTableNum.text = Info.TableNum.ToString();

        txtTableNum[0].text = tableNum.ToString();
        txtReqGameCnt.text = GameCnt.ToString();
        txtGameName.text = GameName.ToString();

    }

    public void ReturnHome()
    {
        SceneChanger.LoadScene("Mail", PageBase.Instance.curBoardObj());
        //REQ 보내야하넹...ㅎㅎ 이사람이 나갔으니까 상대방이 나가서 취소됬다고 애니메이션 넣기..심플하게
    }

}
