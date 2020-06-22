using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdminMsgWrite : SingletonMonobehaviour<AdminMsgWrite>
{
    public Text table;
    public Text Msg;


    byte tableNo = 0;
    string strMsg;


    public void SetInfo(byte tableNo)
    {
        this.tableNo = tableNo;
        table.text = tableNo.ToString() + "번 테이블";
    }


    public void OnConfirm()
    {
        NetworkManager.Instance.Message_Send_REQ(tableNo, strMsg);
    }

    public void OnClose()
    {
        gameObject.SetActive(false);
    }

    public void InputComplete()
    {
        SystemMessage.Instance.Add(this.tableNo.ToString() + "번 테이블에 쪽지보냄~");
        this.tableNo = 0;
        OnClose();
    }
}
