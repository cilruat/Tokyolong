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
		case PROTOCOL.LOGIN_NOT:			LoginNOT (msg);				break;
		case PROTOCOL.LOGOUT_ACK:			LogoutNOT (msg);			break;
		case PROTOCOL.LOGOUT_NOT:			LogoutNOT (msg);			break;
		case PROTOCOL.ENTER_CUSTOMER_ACK:	EnterCustormerACK ();		break;
		case PROTOCOL.WAITER_CALL_ACK:		WaiterCallACK ();			break;
		case PROTOCOL.WAITER_CALL_NOT:		WaiterCallNOT (msg);			break;
		case PROTOCOL.ORDER_ACK:			OrderACK ();				break;
		case PROTOCOL.ORDER_NOT:			OrderNOT (msg);				break;
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
			SceneChanger.LoadScene ("Admin", PageBase.Instance.curBoardObj ());
		else {
			byte no = byte.Parse (pop_string);
			Info.SetTableNum (no);
			PageLogin.Instance.OnNext ();
		}
	}

	void LoginNOT(CPacket msg)
	{
		byte tableNo = msg.pop_byte ();
		PageAdmin.Instance.SetLogin ((int)tableNo);
	}

	void LogoutNOT(CPacket msg)
	{
		byte tableNo = msg.pop_byte ();

		if (SceneManager.GetActiveScene ().name == "Admin")
			PageAdmin.Instance.SetLogout ((int)tableNo);
		else
			SceneChanger.LoadScene ("Login", PageBase.Instance.curBoardObj ());
	}

	void EnterCustormerACK()
	{
		SceneChanger.LoadScene ("Main", PageBase.Instance.curBoardObj ());
	}

	void WaiterCallACK()
	{
		SystemMessage.Instance.Add ("직원을 호출하였습니다");
	}

	void WaiterCallNOT(CPacket msg)
	{
		byte tableNo = msg.pop_byte ();
		PageAdmin.Instance.Urgency ((int)tableNo);
	}

	void OrderACK()
	{
		((PageOrder)PageBase.Instance).bill.CompleteOrder ();
	}

	void OrderNOT(CPacket msg)
	{
		byte tableNo = msg.pop_byte ();
		string order = msg.pop_string ();
		PageAdmin.Instance.SetOrder (true, tableNo, order);
	}
}
