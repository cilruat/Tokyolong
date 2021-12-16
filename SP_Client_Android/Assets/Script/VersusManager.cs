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


    //셋인포 두개해야하나? 나한테 주는거 상대한테 주는거
    public void SetInfo(UserGameAcceptInfo info)
    {

        myTableNum = info.tableNo;
        tableNum = info.targettableNo;
        RPSGameCnt = info.reqGameCnt;
        RPSGameName = info.gameName;


        txtmyTableNum.text = myTableNum.ToString();
        txtTableNum.text = tableNum.ToString();
        txtGameCnt.text = RPSGameCnt.ToString();
        txtGameName.text = RPSGameName.ToString();

    }

}
