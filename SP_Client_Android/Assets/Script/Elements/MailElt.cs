using Photon.Pun.Demo.Cockpit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;




public class MailElt : MonoBehaviour {

	//Serializable은 인스펙터 테이블에 노출시키기 위함, 커스텀클래스는 기본적으로 노출이 안되기때문에 작성해주는것
	//[System.Serializable]
	//여기서 채팅을 제어한다

	public Text TableNo;
	public Text textMailMsg;
	public RectTransform rtElt;

	public void SetMailElt(UserMsgInfo msgInfo)
	{
		TableNo.text = string.Format("{0:D2}", msgInfo.tableNo);
		textMailMsg.text = msgInfo.strMsg;

		//MailElt는 현시점 완료된듯
	}
}
