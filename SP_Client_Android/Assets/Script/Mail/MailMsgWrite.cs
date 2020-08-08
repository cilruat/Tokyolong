﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MailMsgWrite : SingletonMonobehaviour<MailMsgWrite> {

    public Text table;
    public Text Msg;
    public InputField input;

    byte tableNo = 0;

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
        input.text = string.Empty;
        OnClose();
    }

    public void OnClose()
    {
        gameObject.SetActive(false);
    }
}
