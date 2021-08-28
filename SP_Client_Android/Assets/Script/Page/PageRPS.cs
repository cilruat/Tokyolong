using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageRPS : SingletonMonobehaviour<PageRPS>  {


    public GameObject objBoard;



    //public Text txtPlayCnt;
    public Text txtTableNo;
    public Text txtOtherTableNo;
    public Text txtGameCnt;
    public Text txtGameName;

    //public GameObject objSelect;

    public GameObject objReadyPlayer_1, objReadyPlayer_2;
    public GameObject objReadyBtn, objReadyFinBtn;

    int RPSGameCnt;
    string RPSGameName;
    byte tableNum = 0;

    UserGameInfo info;



    public void SetInfo(UserGameInfo info)
    {
        this.info = info;
        tableNum = info.tableNo;
        RPSGameCnt = info.reqGameCnt;
        RPSGameName = info.gameName;


        Debug.Log(info.tableNo);
        Debug.Log(info.reqGameCnt);
        Debug.Log(info.gameName);

    }



    public void ShowRPSInfo()
    {

        txtTableNo.text = info.tableNo.ToString();
        txtGameCnt.text = info.reqGameCnt.ToString();
        txtGameName.text = info.gameName.ToString();

        //UITweenAlpha.Start(objSelect, 0f, 1f, TWParam.New(.5f).Curve(TWCurve.CurveLevel2));
    }



    public void ReturnHome()
    {

        //REQ 보내야하넹...ㅎㅎ 이사람이 나갔으니까 상대방이 나가서 취소됬다고 애니메이션 넣기..심플하게
    }






}
