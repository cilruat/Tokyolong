using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MailElt : MonoBehaviour {
    
	public Text TableNo;
	public Text strMsg;
	public Text Desc;

    UserMsgInfo info;

	public void SetInfo(UserMsgInfo info)
	{
        this.info = info;
		TableNo.text = string.Format("{0:D2}", info.tableNo);
        strMsg.text = info.strMsg;
    }

    public void OpenMsg()
    {
        if (!Info.isCheckScene("Mail"))
            return;

        PageMail.Instance.showMsg.No.text = info.tableNo.ToString();
        PageMail.Instance.showMsg.Msg.text = info.strMsg;
        PageMail.Instance.showMsg.obj.SetActive(true);
    }

    public void OnClose()
    {
        if (!Info.isCheckScene("Mail"))
            return;

        PageMail.Instance.showMsg.obj.SetActive(false);
    }

    public void OnDelete()
    {
        if (!Info.isCheckScene("Mail"))
            return;

        PageMail.Instance.DeleteMailElt(this);
    }

    public byte GetTableNo() { return info.tableNo; }
}
