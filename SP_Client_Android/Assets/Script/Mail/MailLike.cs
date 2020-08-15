using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MailLike : SingletonMonobehaviour<MailLike>
{
    public Text table;

    byte tableNo = 0;
    int gameCount = 1;


    public void SetInfo(byte tableNo)
    {
        this.tableNo = tableNo;
        table.text = tableNo.ToString() + "번 테이블";

    }

    public void OnConfirm()
    {

            NetworkManager.Instance.Like_Send_REQ(tableNo, gameCount);
            OnClose();
    }

    public void OnClose()
    {
        gameObject.SetActive(false);
    }

}
