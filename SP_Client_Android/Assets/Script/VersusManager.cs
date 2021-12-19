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


    int inviteGameCnt;
    string inviteGameName;
    byte tableNum = 0;



    //public GameObject prefabGame;


    //Elt를 만드는 방식
    /*

public void SetGame(UserGameAcceptInfo info)
{
    CreateGameAcceptElt(info);
}

void CreateGameAcceptElt(UserGameAcceptInfo info)
{
    GameObject obj = Instantiate(prefabGame) as GameObject;
    obj.SetActive(true);

    Transform tr = obj.transform;
    tr.InitTransform();

    VersusElt elt = obj.GetComponent<VersusElt>();
    elt.SetInfo(info);

}
*/

    public void ShowGame()
    {

        if (Info.myInfo.listGameAcceptInfo.Count > 0)
        {
            UserGameAcceptInfo info = Info.myInfo.listGameAcceptInfo[Info.myInfo.listGameAcceptInfo.Count - 1];
            tableNum = info.tableNo;
            inviteGameCnt = info.reqGameCnt;
            inviteGameName = info.gameName;
        }

        txtTableNo.text = tableNum.ToString();
        txtPlayCnt.text = inviteGameCnt.ToString();
        txtGameName.text = inviteGameName.ToString();

        //UITweenAlpha.Start(objSelect, 0f, 1f, TWParam.New(.5f).Curve(TWCurve.CurveLevel2));
    }





}
