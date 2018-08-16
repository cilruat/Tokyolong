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
        case PROTOCOL.REPORT_OFFLINE_GAME_ACK:	ReportOfflineGameACK (msg);	break;
		case PROTOCOL.UNFINISH_GAME_LIST_ACK:	UnfinishGameListACK(msg);	break;
        case PROTOCOL.TABLE_DISCOUNT_INPUT_ACK:     TableDiscountInputACK (msg);   break;
        case PROTOCOL.GET_RANDOM_DISCOUNT_PROB_ACK: GetDiscountProbACK (msg);      break;
        case PROTOCOL.SET_RANDOM_DISCOUNT_PROB_ACK: SetDiscountProbACK (msg);      break;
        case PROTOCOL.COUPON_ACK:                   CouponACK (msg);               break;
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
        byte tableNo = msg.pop_byte ();
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
        Info.listOrderCnt_Robot[idRobot] = msg.pop_int32 ();
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
        string packing = msg.pop_string ();
	}

    void RequestMusicACK(CPacket msg)
    {       
        int priority = msg.pop_int32();
        string packing = msg.pop_string ();
    }

	void SlotStartACK(CPacket msg)
	{
        Info.listGameCnt_Robot[idRobot] = msg.pop_byte ();
        short discountType = msg.pop_int16();

        if (Info.listGameCnt_Robot[idRobot] < 0)
            Info.listGameCnt_Robot[idRobot] = 0;
	}

    void ReportOfflineGameACK(CPacket msg)
	{
	}

	void UnfinishGameListACK(CPacket msg)
	{
	}		

    public void TableDiscountInputACK(CPacket msg)
    {
    }

    public void GetDiscountProbACK(CPacket msg)
    {
        List<float> listProb = new List<float>();
        for (int i = 0; i < 4; i++)
            listProb.Add(msg.pop_float());
    }

    public void SetDiscountProbACK(CPacket msg)
    {
    }

    public void CouponACK(CPacket msg)
    {
        int couponCnt = msg.pop_int32();
    }
}
