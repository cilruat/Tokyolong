using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class VersusElt : MonoBehaviour {


    public Text txtmyTableNum;
    public Text txtTableNum;

    public Text txtGameCnt;
    public Text txtGameName;

    UserGameAcceptInfo info = null;


    public void SetInfo(UserGameAcceptInfo info)
    {
        this.info = info;

        Debug.Log(info.tableNo);
        Debug.Log(info.reqGameCnt);
        Debug.Log(info.gameName);

        //정보는 다 들어왓는데..변환이 안되는데요?

        txtmyTableNum.text = string.Format("{0:D2}", Info.TableNum);
        txtTableNum.text = string.Format("{0:D2}", info.tableNo);
        txtGameCnt.text = string.Format("{0:D2}", info.reqGameCnt);
        txtGameName.text = string.Format("{0:D2}", info.gameName);



    }

    public byte GetTableNo() { return info.tableNo; }




}
