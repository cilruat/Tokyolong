using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIPlz : MonoBehaviour {

    public eUI uiType;
    public Text[] txtTableNum;
    public Text txtPlzCount;
    public CountDown countdown;
    public GameObject objSelect;
    int plzCount = 0;

    public void ShowPlzTable()
    {
        byte tableNum = 0;

        if (Info.myInfo.listPlzInfo.Count > 0)
        {
            UserPlzInfo info = Info.myInfo.listPlzInfo[Info.myInfo.listPlzInfo.Count - 1];
            tableNum = info.tableNo;
            plzCount = info.plzCount;
        }

        txtTableNum[0].text = tableNum.ToString();
        txtPlzCount.text = plzCount.ToString();


        UITweenAlpha.Start(objSelect, 0f, 1f, TWParam.New(.5f).Curve(TWCurve.CurveLevel2));
    }

    public void OnClose()
    {
        countdown.Stop();
        UIManager.Instance.Hide(eUI.ePlease);
    }

    public void OnGoMailScene()
    {
        UIManager.Instance.Hide(eUI.ePlease);
        SceneChanger.LoadScene("Mail", objSelect);
    }


}
