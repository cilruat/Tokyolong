using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MailTableMenu : SingletonMonobehaviour<MailTableMenu> {

	public Text table;
	byte tableNo = 0;

    public GameObject[] objMenu;

	public void SetInfo (byte tableNo)
	{
		this.tableNo = tableNo;
		table.text = tableNo.ToString () + "번 테이블";
	}

    public void OnShow(int state)
    {
        for (int i = 0; i < objMenu.Length; i++)
        {
            if (i != state)
                continue;

            objMenu[i].SetActive(true);
            break;
        }
    }

	//정보를 먼저 가지고 와야되는건 Network매니저에서 먼저 작업하고 후처리는 씬에서 작업하고 쏘는듯

	public void OnMailSend()
	{

		//네트워큰가
		PageMail.Instance.SendMSG (tableNo);
		OnClose ();
	}








	public void OnClose() {	gameObject.SetActive (false); }

}
