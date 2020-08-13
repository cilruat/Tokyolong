using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MailElt : MonoBehaviour {


	public Text[] TableNo;
	public Text strMsg;
	public Text Desc;
    public GameObject objMsg;


    UserMsgInfo info = null;

	public void SetInfo(UserMsgInfo info)
	{
        this.info = info;
		TableNo[0].text = string.Format("{0:D2}", info.tableNo);
        TableNo[1].text = string.Format("{0:D2}", info.tableNo);
        strMsg.text = info.strMsg;
	}

    public void OpenMsg()
    {
        objMsg.SetActive(true);
    }

    public void OnClose()
    {
        objMsg.SetActive(false);
    }

    public void OnDelete()
    {
        Destroy(gameObject);
    }

}
