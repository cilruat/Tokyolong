using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIMail : MonoBehaviour {

    public eUI uiType;

    public Text[] txtTableNum;
    public Text txtDesc;
    public CountDown countdown;
    public GameObject objSelect;
    public GameObject objContent;


    byte tableNum = 0;

    public GameObject objChatPanel;
    public Text Msg;
    public InputField input;

    string desc = "";

    public void ShowMsgTable()
    {
        if (Info.myInfo.listMsgInfo.Count > 0)
        {
            UserMsgInfo info = Info.myInfo.listMsgInfo[Info.myInfo.listMsgInfo.Count - 1];
            tableNum = info.tableNo;
            desc = info.strMsg;
        }

        txtTableNum[0].text = tableNum.ToString();
        txtTableNum[1].text = tableNum.ToString();

        /*
        CanvasGroup cgSelect = objSelect.GetComponent<CanvasGroup>();
        if (cgSelect != null)
            cgSelect.alpha = 1f;

        CanvasGroup cgCongent = objSelect.GetComponent<CanvasGroup>();
        if (cgCongent != null)
            cgCongent.alpha = 0f;*/

        UITweenAlpha.Start(objSelect, 0f, 1f, TWParam.New(.5f).Curve(TWCurve.CurveLevel2));
        UITweenAlpha.Start(objContent, 1f, 0f, TWParam.New(.5f, .5f).Curve(TWCurve.CurveLevel2));

        objSelect.SetActive(true);
        objContent.SetActive(false);
    }    

    public void OnConfirm()
    {
        txtDesc.text = desc;

        objSelect.SetActive(false);
        objContent.SetActive(true);
    }

    public void OnClose()
    {
        countdown.Stop();
        UIManager.Instance.Hide(eUI.eMail);
    }

    public void OnGoMailScene()
    {
        UIManager.Instance.Hide(eUI.eMail);
        SceneChanger.LoadScene("Mail", objContent);
    }

    public void OnCloseContent()
    {
        UIManager.Instance.Hide(eUI.eMail);
    }

    public void OnOpenChatPanel()
    {
        objChatPanel.SetActive(true);
    }

    public void OnSendReply()
    {
        if (input.text == string.Empty)
            return;

        string strMsg = input.text;
        NetworkManager.Instance.Message_Send_REQ(tableNum, strMsg);
        input.text = string.Empty;
        OnCloseReply();
    }

    public void OnCloseReply()
    {
        objChatPanel.SetActive(false);
    }



}
