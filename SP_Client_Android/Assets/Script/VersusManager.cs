using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;




public class VersusManager : SingletonMonobehaviour<VersusManager> {


    public Text txtmyTableNum;
    public Text txtTableNum;

    public Text txtGameCnt;
    public Text txtGameName;


    UserGameAcceptInfo info = null;



    //셋인포 두개해야하나? 나한테 주는거 상대한테 주는거
    public void SetInfo(UserGameAcceptInfo info)
    {
        this.info = info;
        txtmyTableNum.text = string.Format("{0:D2}", Info.TableNum);
        txtTableNum.text = string.Format("{0:D2}", info.targettableNo);
        txtGameCnt.text = string.Format("{0:D1}", info.reqGameCnt);
        txtGameName.text = string.Format("{0:D1}", info.gameName);
    }

}
