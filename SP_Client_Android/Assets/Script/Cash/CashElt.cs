using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CashElt : MonoBehaviour {

    public Text Table;
    public Text Desc;

    UserCashInfo info = null;

    public void SetInfo(UserCashInfo info)
    {
        this.info = info;
        Table.text = string.Format("{0:D2}", info.tableNo);
        Desc.text = string.Format("{0:D2}", info.reqCashItem);
    }

    public void OnDeleteAdmin()
    {
        if (!Info.isCheckScene("Admin"))
            return;

        PageAdmin.Instance.DeleteCashElt(this);
    }

    public void OnDeleteCashShop()
    {
        if (!Info.isCheckScene("CashShop"))
            return;

        PageCashShop.Instance.DeleteCashElt(this);
    }


    public byte GetTableNo() { return info.tableNo; }

}
