using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MailLike : SingletonMonobehaviour<MailLike>
{
    public Text table;

    byte tableNo = 0;
    int gameCount = 1;


    List<TableElt> listTable = new List<TableElt>();


    public void SetInfo(byte tableNo)
    {
        this.tableNo = tableNo;
        table.text = tableNo.ToString() + "번 테이블";

    }

    //리스트에서해야되는거같은디
    public bool CheckTableLike()
    {
        for (int i = 0; i < listTable.Count; i++)
        {
            if (listTable[i].IsLike() == false)
                continue;

            if (listTable[i].IsLike())
                return true;

        }
        return false;
    }

    //좋아요중복
    public void OnConfirm()
    {
        //if(조건문, info값 ? TableElt값?)

            NetworkManager.Instance.Like_Send_REQ(tableNo, gameCount);
            OnClose();

        //SystemMessage.Instance.Add("중복이다");
    }

    public void OnClose()
    {
        gameObject.SetActive(false);
    }

}
