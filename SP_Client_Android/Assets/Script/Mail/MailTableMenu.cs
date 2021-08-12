using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MailTableMenu : SingletonMonobehaviour<MailTableMenu> {

	public Text table;
	byte tableNo = 0;

    public GameObject[] objMenu;
    public Animator startShowanim;


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
            startShowanim.Play("Show");
            break;
        }

        // 0: 쪽지 1: 좋아요 2: 선물하기 3:조르기
        switch (state) {
            case 0:
                MailMsgWrite write = objMenu[state].GetComponent<MailMsgWrite>();
                if (write)
                    write.SetInfo(tableNo);
                break;
            case 1:
                MailLike like = objMenu[state].GetComponent<MailLike>();
                if (like)
                    like.SetInfo(tableNo);
                break;
            case 2:
                MailPresent present = objMenu[state].GetComponent<MailPresent>();
                if (present)
                    present.SetInfo(tableNo);
                break;
            case 3:
                MailPlz plz = objMenu[state].GetComponent<MailPlz>();
                if (plz)
                    plz.SetInfo(tableNo);
                break;

                //여기에 1vs1 대전 케이스를 추가해주어야한다
        }
    }

	public void OnClose() {	gameObject.SetActive (false); }
}
