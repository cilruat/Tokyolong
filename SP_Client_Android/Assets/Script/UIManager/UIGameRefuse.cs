using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIGameRefuse : MonoBehaviour {

    public eUI uiType;
    public Text[] txtTableNum;
    public GameObject objSelect;

    public Animator anim;

    public void ShowGameRefuse()
    {
        byte tableNum = 0;

        if (Info.myInfo.listGameInfo.Count > 0)
        {
            UserRefuseInfo info = Info.myInfo.listRefuseInfo[Info.myInfo.listRefuseInfo.Count - 1];
            tableNum = info.tableNo;
        }

        txtTableNum[0].text = tableNum.ToString();

        //UITweenAlpha.Start(objSelect, 0f, 1f, TWParam.New(.5f).Curve(TWCurve.CurveLevel2));
        anim.Play("GameRefuse");

    }

}
