using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FreeNet;
using SP_Server;

public partial class RobotNetwork : MonoBehaviour
{
	void on_message(CPacket msg)
	{
		waiting = false;
		float timeLength = Time.time - timeToReq;
		accRecvTime += (timeLength - accRecvTime) * .05f;

		PROTOCOL protocol_id = (PROTOCOL)msg.pop_protocol_id ();
        #if UNITY_EDITOR
        Debug.Log(protocol_id.ToString());
        #endif

		switch (protocol_id)
		{
		case PROTOCOL.FAILED_NOT_NUMBER:	Failed (protocol_id);		break;
		case PROTOCOL.LOGIN_ACK:			LoginACK (msg);				break;
		case PROTOCOL.ENTER_CUSTOMER_ACK:	EnterCustormerACK (msg);	break;
		case PROTOCOL.WAITER_CALL_ACK:		WaiterCallACK ();			break;
		case PROTOCOL.ORDER_ACK:			OrderACK (msg);				break;
        case PROTOCOL.CHAT_ACK:             ChatACK(msg);               break;
        case PROTOCOL.ORDER_DETAIL_ACK:     OrderDetailACK(msg);        break;
		case PROTOCOL.GAME_DISCOUNT_ACK:	GameDiscountACK (msg);		break;
		case PROTOCOL.REQUEST_MUSIC_ACK:	RequestMusicACK (msg);		break;
		case PROTOCOL.REQUEST_MUSIC_LIST_ACK: RequestMusicListACK (msg);    break;
		case PROTOCOL.SLOT_START_ACK:			SlotStartACK (msg);			break;
		case PROTOCOL.REPORT_OFFLINE_GAME_ACK:	ReportOfflineGameACK ();	break;
		case PROTOCOL.UNFINISH_GAME_LIST_ACK:	UnfinishGameListACK(msg);	break;
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
		numLogined++;
	}			  

	void EnterCustormerACK(CPacket msg)
	{		
	}

	void WaiterCallACK()
	{
	}

	void OrderACK(CPacket msg)
	{
		Info.listGamePlayCnt_Robot[idRobot] = msg.pop_byte ();
	}

    void ChatACK(CPacket msg)
    {        
    }

    void OrderDetailACK(CPacket msg)
    {       
    }

	void GameDiscountACK(CPacket msg)
	{		
	}

	void RequestMusicListACK(CPacket msg)
	{		
	}

    void RequestMusicACK(CPacket msg)
    {       
    }

	void SlotStartACK(CPacket msg)
	{
		Info.listGamePlayCnt_Robot[idRobot] = msg.pop_byte ();

		if (Info.listGamePlayCnt_Robot[idRobot] < 0)
			Info.listGamePlayCnt_Robot[idRobot] = 0;
	}

	void ReportOfflineGameACK()
	{
	}

	void UnfinishGameListACK(CPacket msg)
	{
	}		
}
