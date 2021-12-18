using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;




public class VersusManager : SingletonMonobehaviour<VersusManager> {


    public Text txtTableNo;
    public Text txtPlayCnt;


    public Text txtTargetTableNo;
    public Text txtReqGameCnt;
    public Text txtGameName;



    public void SetInfo(UserGameAcceptInfo info)
    {
        txtTargetTableNo.text = info.tableNo.ToString();
        txtReqGameCnt.text = info.reqGameCnt.ToString();
        txtGameName.text = info.gameName.ToString();
    }

}
