using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MailMsgWrite : SingletonMonobehaviour<MailMsgWrite> {

    public Text table;
    public Text Msg;
    public InputField input;

    byte tableNo = 0;
    //string strMsg;

    public void SetInfo(byte tableNo)
    {
        this.tableNo = tableNo;
        table.text = tableNo.ToString() + "번 테이블";

    }

    public void OnConfirm()
    {
        if (input.text == string.Empty)
            return;

        string strMsg = input.text;
        NetworkManager.Instance.Message_Send_REQ(tableNo, strMsg);
        //보내고나면 비운다
        input.text = string.Empty;
        //메세지 화면을 끈다 추가할것
        OnClose();
    }

    public void OnClose()
    {
        gameObject.SetActive(false);
    }

    public void InputComplete()
    {
        SystemMessage.Instance.Add(this.tableNo.ToString() + "번 테이블에 쪽지보냈어요 이건 사람들끼리임");
        this.tableNo = 0;
        OnClose();
    }
}
