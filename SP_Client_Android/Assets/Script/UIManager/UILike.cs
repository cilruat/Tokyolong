using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UILike : MonoBehaviour {

    public eUI uiType;

    public Text[] txtTableNum;
    public CountDown countdown;
    public GameObject objSelect;

    public void ShowLikeTable()
    {
        byte tableNum = 0;
        if (Info.myInfo.listLikeInfo.Count > 0)
        {
            UserLikeInfo info = Info.myInfo.listLikeInfo[Info.myInfo.listLikeInfo.Count - 1];
            tableNum = info.tableNo;
        }

        txtTableNum[0].text = tableNum.ToString();
        //txtTableNum[1].text = tableNum.ToString();

        UITweenAlpha.Start(objSelect, 0f, 1f, TWParam.New(.5f).Curve(TWCurve.CurveLevel2));
        //objSelect.SetActive(true);
    }


    public void OnClose()
    {
        countdown.Stop();
        UIManager.Instance.Hide(eUI.eLike);
    }

    public void OnGoMailScene()
    {
        UIManager.Instance.Hide(eUI.eLike);
        SceneChanger.LoadScene("Mail", objSelect);
        //SceneManager.LoadScene("Mail");
    }

}
