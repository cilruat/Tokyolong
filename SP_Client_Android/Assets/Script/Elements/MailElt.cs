using Photon.Pun.Demo.Cockpit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;




public class MailElt : MonoBehaviour {

	//Serializable은 인스펙터 테이블에 노출시키기 위함, 커스텀클래스는 기본적으로 노출이 안되기때문에 작성해주는것
	//[System.Serializable]
	//여기서 채팅을 제어한다

	public Text textTableNo;
	public Text textMailMsg;

	public void SetMailElt(UserMail strMsg)
    {
		UserMsgInfo info = strMsg.info;

		textTableNo.text = "No. <size='20'>" + info.tableNo.ToString() + "</size>";
		textMailMsg.text = strMsg.mail;
		RectTransform rtMailMsg = textMailMsg.GetComponent<RectTransform>();
		RectTransform rtTableNo = textTableNo.GetComponent<RectTransform>();

    }


}
