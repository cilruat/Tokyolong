using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FreeNet;
using SP_Server;

public partial class RobotNetwork : MonoBehaviour
{
	void send(CPacket msg)
	{
        #if UNITY_EDITOR
        Debug.Log(((PROTOCOL)msg.protocol_id).ToString());
        #endif

		this.sending_queue.Enqueue(msg);
	}

	public void Login_REQ()
	{
		CPacket msg = CPacket.create ((short)PROTOCOL.LOGIN_REQ);
		msg.push (idRobot.ToString ());

		send (msg);
	}

	public void Logout_REQ()
	{
		CPacket msg = CPacket.create ((short)PROTOCOL.LOGOUT_REQ);
		msg.push ((byte)idRobot);

		send (msg);
	}

	public void EnterCostomer_REQ()
	{
        CPacket msg = CPacket.create((short)PROTOCOL.ENTER_CUSTOMER_REQ);
		msg.push ((byte)idRobot);
		msg.push((byte)Random.Range(1,8));
		msg.push((byte)Random.Range(0, 3));

        send(msg);
	}

	public void WaiterCall_REQ ()
	{
		CPacket msg = CPacket.create((short)PROTOCOL.WAITER_CALL_REQ);
		msg.push (Info.TableNum);

		send (msg);
	}

    public void Order_REQ()
	{
		CPacket msg = CPacket.create((short)PROTOCOL.ORDER_REQ);
		msg.push ((byte)idRobot);
		msg.push ("[{\\\"menu\\\":3,\\\"cnt\\\":1},{\\\"menu\\\":6,\\\"cnt\\\":1}]");

		send (msg);
	}

    public void Chat_REQ()
    {
        CPacket msg = CPacket.create((short)PROTOCOL.CHAT_REQ);
		msg.push ((byte)idRobot);
		msg.push((byte)Random.Range(0, 21));
        msg.push("Hello!");

        send(msg);
    }

    public void Order_Detail_REQ()
    {
        CPacket msg = CPacket.create((short)PROTOCOL.ORDER_DETAIL_REQ);
		msg.push ((byte)idRobot);

        send(msg);
    }

	public void Game_Discount_REQ()
	{
		CPacket msg = CPacket.create((short)PROTOCOL.GAME_DISCOUNT_REQ);
		msg.push ((byte)idRobot);
		msg.push ((short)Random.Range(0, 2));
		send (msg);
	}

	public void Request_Music_List_REQ()
	{
		CPacket msg = CPacket.create ((short)PROTOCOL.REQUEST_MUSIC_LIST_REQ);
		send (msg);
	}

    public void Request_Music_REQ()
    {
        CPacket msg = CPacket.create((short)PROTOCOL.REQUEST_MUSIC_REQ);
        msg.push(Info.TableNum);
        msg.push("눈의꽃");
        msg.push("박효신");

        send(msg);
    }
		   
	public void SlotStart_REQ()
	{
		if (Info.listGamePlayCnt_Robot [idRobot] <= 0) {
			waiting = false;
			return;
		}

		CPacket msg = CPacket.create((short)PROTOCOL.SLOT_START_REQ);
		send (msg);
	}

	public void ReportOfflineGame_REQ()
	{
		CPacket msg = CPacket.create((short)PROTOCOL.REPORT_OFFLINE_GAME_REQ);
		msg.push ((byte)idRobot);

		byte discount = (byte)Random.Range(0,2);
		byte gameType = (byte)Random.Range(0, 4);
		byte gameKind = 0;
		switch (gameType) {
		case 0:		
			gameKind = (byte)Random.Range (0, 4);	
			break;
		case 1:		
		case 2:		
			gameType = 0;
			gameKind = (byte)Random.Range (0, 4);
			break;
		case 3:
			gameKind = (byte)Random.Range (0, 5);
			break;
		}

		msg.push (gameType);
		msg.push (gameKind);
		msg.push (discount);

		send (msg);
	}

	public void UnfinishGamelist_REQ()
	{
		CPacket msg = CPacket.create((short)PROTOCOL.UNFINISH_GAME_LIST_REQ);
		msg.push ((byte)idRobot);

		send (msg);
	}		
}
