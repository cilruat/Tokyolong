using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;




public class VersusManager : SingletonMonobehaviour<VersusManager> {


    public Text txtmyTableNum;
    public Text txtTableNum;

    public Text txtGameCnt;
    public Text txtGameName;


    int RPSGameCnt;
    string RPSGameName;
    byte tableNum = 0;
    byte myTableNum = 0;

    UserGameAcceptInfo info;

    public bool isLobby = false;
    public bool isReady = false;


    private void Awake()
    {
        //txtmyTableNum.text = Info.TableNum.ToString(); // 내 테이블 정보

        txtmyTableNum.text = info.tableNo.ToString();
        txtTableNum.text = info.targettableNo.ToString();
        txtGameCnt.text = info.reqGameCnt.ToString();
        txtGameName.text = info.gameName.ToString();
    }


    //셋인포 두개해야하나? 나한테 주는거 상대한테 주는거
    public void SetInfo(UserGameAcceptInfo info)
    {
        this.info = info;
        myTableNum = info.tableNo;
        tableNum = info.targettableNo;
        RPSGameCnt = info.reqGameCnt;
        RPSGameName = info.gameName;
    }

}
