using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FreeNet;
using SP_Server;

public partial class NetworkManager : SingletonMonobehaviour<NetworkManager> 
{
	public static bool isSending = false;
	void send(CPacket msg)
	{
		if (UIManager.Instance.IsActive (eUI.eWaiting) == false)			
			UIManager.Instance.Show (eUI.eWaiting);

        #if UNITY_EDITOR
        Debug.Log(((PROTOCOL)msg.protocol_id).ToString());
        #endif

		isSending = true;
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

    public void Order_REQ(string order, int orderCnt)
	{
		CPacket msg = CPacket.create((short)PROTOCOL.ORDER_REQ);
        msg.push (Info.TableNum);
		msg.push (order);
        msg.push(orderCnt);

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

    public void Table_Order_Input_REQ(byte tableNo, string packing, int orderCnt)
    {
        CPacket msg = CPacket.create((short)PROTOCOL.TABLE_ORDER_INPUT_REQ);
        msg.push(tableNo);
        msg.push(packing);
        msg.push(orderCnt);

        send(msg);
    }

	public void SlotStart_REQ()
	{
		CPacket msg = CPacket.create((short)PROTOCOL.SLOT_START_REQ);
        msg.push(Info.TableNum);
		send (msg);
	}

    public void TableDiscountInput_REQ(byte tableNo, int discount)
    {
        CPacket msg = CPacket.create((short)PROTOCOL.TABLE_DISCOUNT_INPUT_REQ);
        msg.push (tableNo);
        msg.push (discount);

        send(msg);
    }

    public void GetDiscountProb_REQ()
    {
        CPacket msg = CPacket.create((short)PROTOCOL.GET_RANDOM_DISCOUNT_PROB_REQ);
        send(msg);
    }

    public void SetDiscountProb_REQ(List<float> list)
    {
        CPacket msg = CPacket.create((short)PROTOCOL.SET_RANDOM_DISCOUNT_PROB_REQ);
		for (int i = 0; i < list.Count; i++)
			msg.push (list [i]);
		
        send(msg);
    }

    public void TablePriceConfirm_REQ(byte tableNo)
    {
        CPacket msg = CPacket.create((short)PROTOCOL.TABLE_PRICE_CONFIRM_REQ);
        msg.push(tableNo);

        send(msg);
    }

	public void GameCountInput_REQ(byte tableNo, int gameCnt)
	{
		CPacket msg = CPacket.create((short)PROTOCOL.GAME_COUNT_INPUT_REQ);
		msg.push(tableNo);
		msg.push(gameCnt);

		send(msg);
	}

	public void TableMove_REQ(byte tableNo, int moveTableNo)
	{
		CPacket msg = CPacket.create((short)PROTOCOL.TABLE_MOVE_REQ);
		msg.push(tableNo);
		msg.push(moveTableNo);

		send(msg);
	}

    public void Message_Send_REQ(byte targetTableNo, string strMsg)
    {
        CPacket msg = CPacket.create((short)PROTOCOL.MSG_SEND_REQ);
        msg.push(Info.TableNum);
        msg.push(targetTableNo);
        msg.push(strMsg);

        send(msg);
    }

    public void Like_Send_REQ(byte targetTableNo, int gameCount)
    {
        CPacket msg = CPacket.create((short)PROTOCOL.LKE_SEND_REQ);
        msg.push(Info.TableNum);
        msg.push(targetTableNo);
        msg.push(gameCount);

        send(msg);

    }

    public void Prensent_Send_REQ(byte targetTableNo, int gameCnt)
    {
        CPacket msg = CPacket.create((short)PROTOCOL.PRESENT_SEND_REQ);
        msg.push(Info.TableNum);
        msg.push(targetTableNo);
        msg.push(gameCnt);

        send(msg);

    }

    public void Please_Send_REQ(byte targetTableNo, int plzCount)
    {
        CPacket msg = CPacket.create((short)PROTOCOL.PLZ_SEND_REQ);
        msg.push(Info.TableNum);
        msg.push(targetTableNo);
        msg.push(plzCount);

        send(msg);

    }


    public void Cash_Send_REQ(string title, int gameCnt)
    {
        CPacket msg = CPacket.create((short)PROTOCOL.CASH_SEND_REQ);
        msg.push(Info.TableNum);
        msg.push(title);
        msg.push(gameCnt);

        send(msg);

    }

    public void Game_Versus_Invite_REQ(byte targetTableNo, int reqGameCnt, string gameName)
    {
        CPacket msg = CPacket.create((short)PROTOCOL.GAME_VERSUS_INVITE_REQ);
        msg.push(Info.TableNum);
        msg.push(targetTableNo);
        msg.push(reqGameCnt);
        msg.push(gameName);

        send(msg);

    }

    public void Game_Refuse_REQ(byte targetTableNo)
    {
        CPacket msg = CPacket.create((short)PROTOCOL.GAME_REFUSE_REQ);
        msg.push(Info.TableNum);
        msg.push(targetTableNo);
        send(msg);

    }

    public void Game_Accept_REQ(byte targetTableNo, int reqGameCnt, string gameName)
    {
        CPacket msg = CPacket.create((short)PROTOCOL.GAME_ACCEPT_REQ);
        msg.push(Info.TableNum);
        msg.push(targetTableNo);
        msg.push(reqGameCnt);
        msg.push(gameName);
        send(msg);

    }

    public void Game_Cancel_REQ(byte targetTableNo)
    {
        CPacket msg = CPacket.create((short)PROTOCOL.GAME_CANCEL_REQ);
        msg.push(Info.TableNum);
        msg.push(targetTableNo);
        send(msg);

    }

    public void Game_Ready_REQ(byte targetTableNo)
    {
        CPacket msg = CPacket.create((short)PROTOCOL.GAME_READY_REQ);
        msg.push(Info.TableNum);
        msg.push(targetTableNo);
        send(msg);

    }


    public void Versus_Rock_REQ(byte targetTableNo)
    {
        CPacket msg = CPacket.create((short)PROTOCOL.VERSUS_ROCK_REQ);
        msg.push(Info.TableNum);
        msg.push(targetTableNo);
        send(msg);

    }

    public void Versus_Paper_REQ(byte targetTableNo)
    {
        CPacket msg = CPacket.create((short)PROTOCOL.VERSUS_PAPER_REQ);
        msg.push(Info.TableNum);
        msg.push(targetTableNo);
        send(msg);

    }

    public void Versus_Scissor_REQ(byte targetTableNo)
    {
        CPacket msg = CPacket.create((short)PROTOCOL.VERSUS_SCISSOR_REQ);
        msg.push(Info.TableNum);
        msg.push(targetTableNo);
        send(msg);

    }

    public void Versus_Win_REQ(byte targetTableNo)
    {
        CPacket msg = CPacket.create((short)PROTOCOL.VERSUS_WIN_REQ);
        msg.push(Info.TableNum);
        msg.push(targetTableNo);
        send(msg);

    }

    public void Versus_Lose_REQ(byte targetTableNo)
    {
        CPacket msg = CPacket.create((short)PROTOCOL.VERSUS_LOSE_REQ);
        msg.push(Info.TableNum);
        msg.push(targetTableNo);
        send(msg);

    }


    public void Versus_Draw_REQ(byte targetTableNo)
    {
        CPacket msg = CPacket.create((short)PROTOCOL.VERSUS_DRAW_REQ);
        msg.push(Info.TableNum);
        msg.push(targetTableNo);
        send(msg);

    }


    // need GameCnt
    public void Versus_Victory_REQ(byte targetTableNo)
    {
        CPacket msg = CPacket.create((short)PROTOCOL.VERSUS_VICTORY_REQ);
        msg.push(Info.TableNum);
        msg.push(targetTableNo);
        send(msg);

    }

    public void Versus_GameOver_REQ(byte targetTableNo)
    {
        CPacket msg = CPacket.create((short)PROTOCOL.VERSUS_GAMEOVER_REQ);
        msg.push(Info.TableNum);
        msg.push(targetTableNo);
        send(msg);

    }























}
