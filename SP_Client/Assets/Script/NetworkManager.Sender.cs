using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FreeNet;
using SP_Server;

public partial class NetworkManager : SingletonMonobehaviour<NetworkManager> 
{
	void send(CPacket msg)
	{
		if (UIManager.Instance.IsActive (eUI.eWaiting) == false)
			UIManager.Instance.Show (eUI.eWaiting);

        #if UNITY_EDITOR
        Debug.Log(((PROTOCOL)msg.protocol_id).ToString());
        #endif

		this.sending_queue.Enqueue(msg);
	}

	public void Login_REQ(string table_no)
	{
		CPacket msg = CPacket.create ((short)PROTOCOL.LOGIN_REQ);
		msg.push (table_no);

		send (msg);
	}

	public void Logout_REQ(byte table_no)
	{
		CPacket msg = CPacket.create ((short)PROTOCOL.LOGOUT_REQ);
		msg.push (table_no);

		send (msg);
	}

	public void EnterCostomer_REQ(byte howMany, byte type)
	{
        CPacket msg = CPacket.create((short)PROTOCOL.ENTER_CUSTOMER_REQ);
		msg.push (Info.TableNum);
        msg.push(howMany);
        msg.push(type);

        send(msg);
	}

	public void WaiterCall_REQ ()
	{
		CPacket msg = CPacket.create((short)PROTOCOL.WAITER_CALL_REQ);
		msg.push (Info.TableNum);

		send (msg);
	}

    public void Order_REQ(string order)
	{
		CPacket msg = CPacket.create((short)PROTOCOL.ORDER_REQ);
        msg.push (Info.TableNum);
		msg.push (order);

		send (msg);
	}

    public void Chat_REQ(byte otherTableNo, string chat)
    {
        CPacket msg = CPacket.create((short)PROTOCOL.CHAT_REQ);
        msg.push(Info.TableNum);
        msg.push(otherTableNo);
        msg.push(chat);

        send(msg);
    }

    public void Order_Detail_REQ()
    {
        CPacket msg = CPacket.create((short)PROTOCOL.ORDER_DETAIL_REQ);
        msg.push(Info.TableNum);

        send(msg);
    }

	public void Game_Discount_REQ(short discount)
	{
		CPacket msg = CPacket.create((short)PROTOCOL.GAME_DISCOUNT_REQ);
		msg.push (Info.TableNum);
		msg.push (discount);
		send (msg);
	}

	public void Request_Music_List_REQ()
	{
		CPacket msg = CPacket.create ((short)PROTOCOL.REQUEST_MUSIC_LIST_REQ);
		send (msg);
	}

    public void Request_Music_REQ(string title, string singer)
    {
        CPacket msg = CPacket.create((short)PROTOCOL.REQUEST_MUSIC_REQ);
        msg.push(Info.TableNum);
        msg.push(title);
        msg.push(singer);

        send(msg);
    }

    public void Request_Music_Remove_REQ(int removeMusicID)
    {
        CPacket msg = CPacket.create((short)PROTOCOL.REQUEST_MUSIC_REMOVE_REQ);
        msg.push(removeMusicID);

        send(msg);
    }

    public void Order_Confirm_REQ(byte type, int id, byte tableNo, string packing)
    {
        CPacket msg = CPacket.create((short)PROTOCOL.ORDER_CONFIRM_REQ);
        msg.push(type);
        msg.push(id);
        msg.push(tableNo);
        msg.push(packing);

        send(msg);
    }

    public void Table_Order_Confirm_REQ(byte tableNo)
    {
        CPacket msg = CPacket.create((short)PROTOCOL.TABLE_ORDER_CONFIRM_REQ);
        msg.push(tableNo);

        send(msg);
    }

    public void Table_Order_Input_REQ(byte tableNo, string packing)
    {
        CPacket msg = CPacket.create((short)PROTOCOL.TABLE_ORDER_INPUT_REQ);
        msg.push(tableNo);
        msg.push(packing);

        send(msg);
    }

	public void SlotStart_REQ()
	{
		CPacket msg = CPacket.create((short)PROTOCOL.SLOT_START_REQ);
		send (msg);
	}

	public void ReportOfflineGame_REQ(bool isJackpot, byte gameType, byte gameKind, byte discount)
	{
		byte jackpot = (byte)(isJackpot ? 1 : 0);

		CPacket msg = CPacket.create((short)PROTOCOL.REPORT_OFFLINE_GAME_REQ);
		msg.push (Info.TableNum);
		msg.push (jackpot);
		msg.push (gameType);
		msg.push (gameKind);
		msg.push (discount);

		send (msg);
	}

	public void UnfinishGamelist_REQ()
	{
		CPacket msg = CPacket.create((short)PROTOCOL.UNFINISH_GAME_LIST_REQ);
		send (msg);
	}
}
