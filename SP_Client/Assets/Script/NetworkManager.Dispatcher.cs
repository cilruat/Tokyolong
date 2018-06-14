using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FreeNet;
using SP_Server;

public partial class NetworkManager : SingletonMonobehaviour<NetworkManager> 
{
	void on_message(CPacket msg)
	{		
		if (UIManager.Instance.IsActive (eUI.eWaiting))
			UIManager.Instance.Hide (eUI.eWaiting);

		PROTOCOL protocol_id = (PROTOCOL)msg.pop_protocol_id ();
		switch (protocol_id)
		{
		case PROTOCOL.FAILED_NOT_NUMBER:	Failed (protocol_id);		break;
		case PROTOCOL.LOGIN_ACK:			LoginACK (msg);				break;
		case PROTOCOL.ENTER_CUSTOMER_ACK:	EnterCustormerACK ();		break;
		}
	}

	void Failed(PROTOCOL id)
	{
		switch(id)
		{
		case PROTOCOL.FAILED_NOT_NUMBER:	SystemMessage.Instance.Add ("숫자로 입력해주세요");		break;
		}
	}

	void LoginACK(CPacket msg)
	{
		string pop_string = msg.pop_string ();
        if (pop_string == "admin")
        {
        }
        else
            PageLogin.Instance.OnNext();
	}

	void EnterCustormerACK()
	{
		SceneChanger.LoadScene ("Main", PageLogin.Instance.cgBoards [(int)PageLogin.Instance.curBoard].gameObject);
	}

	void OrderACK()
	{
		SystemMessage.Instance.Add ("주문이 완료되었습니다");
	}
}
