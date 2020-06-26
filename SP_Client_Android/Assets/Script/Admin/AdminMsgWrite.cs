using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdminMsgWrite : SingletonMonobehaviour<AdminMsgWrite>
{
    public Text table;
    public Text Msg;

    public InputField input;

    byte tableNo = 0;
    string strMsg;


    public void SetInfo(byte tableNo)
    {
        this.tableNo = tableNo;
        table.text = tableNo.ToString() + "번 테이블";
    }


    public void OnConfirm()
    {
        //UIChat 참조 OnChatSend()
        //맞는거같은디
        if (input.text == string.Empty)
            return;

        string strMsg = input.text;
        NetworkManager.Instance.Message_Send_REQ(tableNo, strMsg);
        input.text = string.Empty;
    }

    public void OnClose()
    {
        gameObject.SetActive(false);
    }

    public void InputComplete()
    {
        SystemMessage.Instance.Add(this.tableNo.ToString() + "번 테이블에 쪽지보냄~어드민에서작업~");
        this.tableNo = 0;
        OnClose();
    }
}
