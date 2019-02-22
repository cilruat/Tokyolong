﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FreeNet;
using SP_Server;
using FlappyBirdStyle;

public partial class NetworkManager : SingletonMonobehaviour<NetworkManager> 
{
	void on_message(CPacket msg)
	{		
		if (UIManager.Instance.IsActive (eUI.eWaiting))
			UIManager.Instance.Hide (eUI.eWaiting);

		PROTOCOL protocol_id = (PROTOCOL)msg.pop_protocol_id ();
        #if UNITY_EDITOR
        Debug.Log(protocol_id.ToString());
        #endif

		switch (protocol_id)
		{
		case PROTOCOL.FAILED:
		case PROTOCOL.FAILED_NOT_NUMBER:			Failed (protocol_id);			break;
		case PROTOCOL.LOGIN_ACK:					LoginACK (msg);					break;
		case PROTOCOL.LOGIN_NOT:					LoginNOT (msg);					break;
        case PROTOCOL.LOGOUT_ACK:					LogoutACK (msg);				break;
		case PROTOCOL.LOGOUT_NOT:					LogoutNOT (msg);				break;
		case PROTOCOL.ENTER_CUSTOMER_ACK:			EnterCustormerACK (msg);		break;
		case PROTOCOL.ENTER_CUSTOMER_NOT:			EnterCustormerNOT (msg);		break;
		case PROTOCOL.WAITER_CALL_ACK:				WaiterCallACK ();				break;
		case PROTOCOL.WAITER_CALL_NOT:				WaiterCallNOT (msg);			break;
		case PROTOCOL.ORDER_ACK:					OrderACK (msg);					break;
		case PROTOCOL.ORDER_NOT:					OrderNOT (msg);					break;
        case PROTOCOL.CHAT_ACK:             		ChatACK(msg);               	break;
        case PROTOCOL.CHAT_NOT:             		ChatNOT(msg);               	break;
        case PROTOCOL.ORDER_DETAIL_ACK:     		OrderDetailACK(msg);        	break;
		case PROTOCOL.GAME_DISCOUNT_ACK:			GameDiscountACK (msg);			break;
		case PROTOCOL.REQUEST_MUSIC_ACK:			RequestMusicACK (msg);			break;
		case PROTOCOL.REQUEST_MUSIC_NOT:			RequestMusicNOT (msg); 			break;
		case PROTOCOL.REQUEST_MUSIC_LIST_ACK: 		RequestMusicListACK (msg);    	break;
        case PROTOCOL.REQUEST_MUSIC_REMOVE_ACK: 	RequestMusicRemoveACK(msg); 	break;
        case PROTOCOL.REQUEST_MUSIC_REMOVE_NOT: 	RequestMusicRemoveNOT(msg); 	break;
        case PROTOCOL.ORDER_CONFIRM_ACK:        	OrderConfirmACK(msg);       	break;
        case PROTOCOL.ORDER_CONFIRM_NOT:        	OrderConfirmNOT(msg);       	break;
        case PROTOCOL.TABLE_ORDER_CONFIRM_ACK:  	TableOrderConfirmACK(msg);  	break;
        case PROTOCOL.TABLE_ORDER_INPUT_ACK:    	TableOrderInputACK();    		break;
        case PROTOCOL.TABLE_ORDER_INPUT_NOT:    	TableOrderInputNOT(msg);    	break;
		case PROTOCOL.SLOT_START_ACK:				SlotStartACK (msg);				break;
        case PROTOCOL.TABLE_DISCOUNT_INPUT_ACK:     TableDiscountInputACK(msg);     break;
        case PROTOCOL.TABLE_DISCOUNT_INPUT_NOT:     TableDiscountInputNOT(msg);     break;
        case PROTOCOL.GET_RANDOM_DISCOUNT_PROB_ACK: GetDiscountProb_ACK(msg);   	break;
        case PROTOCOL.SET_RANDOM_DISCOUNT_PROB_ACK: SetDiscountProb_ACK(msg);   	break;
        case PROTOCOL.TABLE_PRICE_CONFIRM_ACK:   	TablePriceConfirm_ACK(msg);    	break;
		case PROTOCOL.SURPRISE_ACK:					Surprise_ACK (msg);				break;
		case PROTOCOL.GAME_COUNT_INPUT_ACK:			GameCountInputACK ();			break;
		case PROTOCOL.GAME_COUNT_INPUT_NOT:			GameCountInputNOT (msg);		break;
		case PROTOCOL.TABLE_MOVE_ACK:				TableMoveACK (msg);				break;
		case PROTOCOL.TABLE_MOVE_NOT:				TableMoveNOT ();				break;
		}

		isSending = false;
	}

	void Failed(PROTOCOL id)
	{
		switch(id)
		{
		case PROTOCOL.FAILED:				SystemMessage.Instance.Add ("동작을 실패했어요");		break;
		case PROTOCOL.FAILED_NOT_NUMBER:	SystemMessage.Instance.Add ("숫자로 입력해주세요");		break;
		}
	}

	void LoginACK(CPacket msg)
	{
		string pop_string = msg.pop_string ();
        if (pop_string == "admin")
        {
            Info.adminTablePacking = msg.pop_string ();
            Info.adminOrderPacking = msg.pop_string ();
            Info.adminMusicPacking = msg.pop_string ();
            SceneChanger.LoadScene("Admin", PageBase.Instance.curBoardObj());
        }
		else {
            Info.TableNum = byte.Parse (pop_string);
            int gameCnt = msg.pop_int32 ();
            Info.AddGameCount(gameCnt, true);
			        
			Info.surpriseCnt = msg.pop_int32 ();
			Info.waitSurprise = false;

			int existUser = msg.pop_int32 ();
			if (existUser == 1) {
				Info.PersonCnt = msg.pop_byte ();
				Info.ECustomer = (ECustomerType)msg.pop_byte ();

				string packing = msg.pop_string ();
				Info.SetLoginedOtherUser (packing);

				SceneChanger.LoadScene ("Main", PageBase.Instance.curBoardObj ());
			} else
				((PageLogin)PageBase.Instance).SuccessLogin ();
		}
	}

	void LoginNOT(CPacket msg)
	{
		byte tableNo = msg.pop_byte ();
		PageAdmin.Instance.SetLogin ((int)tableNo);
	}

    //Admin In 
    void LogoutACK(CPacket msg)
    {
        byte tableNo = msg.pop_byte ();
        PageAdmin.Instance.SetLogout ((int)tableNo);
    }

    //Other Users 
	void LogoutNOT(CPacket msg)
	{
		byte tableNo = msg.pop_byte ();

        if (tableNo == Info.TableNum)
        {
            Info.Init();
            UIManager.Instance.Hide_All();
            NetworkManager.Instance.disconnect();
            SceneChanger.LoadScene ("Login", PageBase.Instance.curBoardObj ());
        }
        else
            Info.SetLogoutOtherUser(tableNo);
	}

	void EnterCustormerACK(CPacket msg)
	{
		Info.PersonCnt = msg.pop_byte ();
		Info.ECustomer = (ECustomerType)msg.pop_byte ();

		string packing = msg.pop_string ();
        Info.SetLoginedOtherUser (packing);

		SceneChanger.LoadScene ("Main", PageBase.Instance.curBoardObj ());
	}

	void EnterCustormerNOT(CPacket msg)
	{
		string packing = msg.pop_string ();
        Info.AddOtherLoginUser (packing);
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

	void OrderACK(CPacket msg)
	{
        int orderCnt = msg.pop_int32();
        Info.AddOrderCount(orderCnt);
        ((PageOrder)PageBase.Instance).bill.CompleteOrder ();
	}

	void OrderNOT(CPacket msg)
	{
        byte type = msg.pop_byte();
        int orderId = msg.pop_int32();
		byte tableNo = msg.pop_byte ();
		string packing = msg.pop_string ();

        RequestOrder reqOrder = new RequestOrder(type, orderId, tableNo, packing);

        PageAdmin.Instance.SetOrder (reqOrder);
	}

    void ChatACK(CPacket msg)
    {              
        byte otherTableNo = msg.pop_byte();
        string time = msg.pop_string();
        string chat = msg.pop_string();
        UserChat newChat = new UserChat(Info.GetUser(Info.TableNum), (byte)1, time, chat);

        Info.AddUserChatInfo(otherTableNo, newChat, false);
        GameObject obj = UIManager.Instance.GetCurUI();
        UIChat uiChat = obj.GetComponent<UIChat>();
        uiChat.AddChat(otherTableNo, newChat);
    }

    void ChatNOT(CPacket msg)
    {
        byte otherTableNo = msg.pop_byte ();
        string time = msg.pop_string();
        string chat = msg.pop_string();

        UserChat newChat = new UserChat(Info.GetUser(otherTableNo), (byte)0, time, chat);

        bool isActive = UIManager.Instance.IsActive(eUI.eChat);
        Info.AddUserChatInfo(otherTableNo, newChat, !isActive);

        if (UIManager.Instance.IsActive(eUI.eChat))
        {
            GameObject obj = UIManager.Instance.GetUI(eUI.eChat);
            UIChat uiChat = obj.GetComponent<UIChat>();
            uiChat.AddChat(otherTableNo, newChat);
        }
        else
            UIManager.Instance.ShowChatAlarm();        
    }

    void OrderDetailACK(CPacket msg)
    {
        string menuPacking = msg.pop_string();
		int discountPrice = msg.pop_int32();

		if (UIManager.Instance.IsActive (eUI.eDiscountAni)) {
			GameObject obj = UIManager.Instance.GetCurUI ();
			UIDiscountAnimation uiResult = obj.GetComponent<UIDiscountAnimation> ();
			uiResult.SetInfo (menuPacking, discountPrice);
		} else if (UIManager.Instance.IsActive (eUI.eSurpriseResult)) {
			GameObject obj = UIManager.Instance.GetCurUI ();
			UISurpriseResult uiResult = obj.GetComponent<UISurpriseResult> ();
			uiResult.uiDiscount.SetInfo (menuPacking, discountPrice);
		} else if (UIManager.Instance.IsActive (eUI.eBillDetail) == false) {
			GameObject obj = UIManager.Instance.Show (eUI.eBillDetail);
			UIBillDetail uiBillDetail = obj.GetComponent<UIBillDetail> ();
			uiBillDetail.SetBill (menuPacking, discountPrice);
		}         
    }

	void GameDiscountACK(CPacket msg)
	{
		if (Info.SURPRISE_STEP > -1) {
			GameObject obj = UIManager.Instance.Show (eUI.eSurpriseResult);
			UISurpriseResult uiResult = obj.GetComponent<UISurpriseResult> ();
			uiResult.uiDiscount.SendREQ ();
		} else {
			GameObject obj = UIManager.Instance.Show (eUI.eDiscountAni);
			UIDiscountAnimation ui = obj.GetComponent<UIDiscountAnimation> ();
			ui.SendREQ ();
		}
	}

	void RequestMusicListACK(CPacket msg)
	{
		string packing = msg.pop_string ();
		GameObject obj = UIManager.Instance.Show (eUI.eMusicRequest);
        UIRequestMusic uiReqMusic = obj.GetComponent<UIRequestMusic> ();
        uiReqMusic.SetAddMusicList (packing);
	}

    void RequestMusicACK(CPacket msg)
    {
        bool isAdd = System.Convert.ToBoolean(msg.pop_byte());
        if (isAdd == false)
        {
            SystemMessage.Instance.Add("죄송합니다. 현재 신청곡이 많아 지연되고 있습니다.");
            return;
        }

        string packing = msg.pop_string ();

		if(UIManager.Instance.IsActive (eUI.eMusicRequest))
        {
            GameObject obj = UIManager.Instance.GetUI (eUI.eMusicRequest);
            UIRequestMusic uiReqMusic = obj.GetComponent<UIRequestMusic> ();
            uiReqMusic.SetAddMusic (packing);
        }
    }

    void RequestMusicNOT(CPacket msg)
    {
        bool isAdd = System.Convert.ToBoolean(msg.pop_byte());
        if (isAdd == false)
            return;

        string packing = msg.pop_string();
        PageAdmin.Instance.SetRequestMusic(packing);
    }

    void RequestMusicRemoveACK(CPacket msg)
    {
        int removeReqMusicID = msg.pop_int32();
        PageAdmin.Instance.RemoveElt(false, removeReqMusicID);
    }

    void RequestMusicRemoveNOT(CPacket msg)
    {
        int removeReqMusicID = msg.pop_int32();

		if(UIManager.Instance.IsActive (eUI.eMusicRequest))
        {
            GameObject obj = UIManager.Instance.GetUI(eUI.eMusicRequest);
            UIRequestMusic uiReqMusic = obj.GetComponent<UIRequestMusic> ();
            uiReqMusic.RemoveRequestMusic(removeReqMusicID);
        }
    }

    void OrderConfirmACK(CPacket msg)
    {
        int removeId = msg.pop_int32();
        PageAdmin.Instance.RemoveElt(true, removeId);
        AdminOrderDetail.Instance.OnClose();
    }

    void OrderConfirmNOT(CPacket msg)
    {
        ERequestOrderType type = (ERequestOrderType)msg.pop_byte();
		Info.surpriseCnt = msg.pop_int32 ();

        switch (type)
        {
            case ERequestOrderType.eOrder:      UIManager.Instance.ShowOrderAlarm();    break;
        }
    }

    void TableOrderConfirmACK(CPacket msg)
    {
        byte tableNo = msg.pop_byte();
        string orderPacking = msg.pop_string();
		int discount = msg.pop_int32();
        int extraGameCnt = msg.pop_int32();
        AdminTableMenu.Instance.OnClose();
        PageAdmin.Instance.ShowBillConfirm (tableNo, orderPacking, discount, extraGameCnt);
    }

    void TableOrderInputACK()
    {
        if(PageAdmin.Instance.objTableOrderInput.activeSelf)
            AdminTableOrderInput.Instance.OnCompleteTableOrderInput();

        if(PageAdmin.Instance.objBillConfirm.activeSelf)
            AdminBillConfirm.Instance.OnCompleteTableOrderInput();
    }

    void TableOrderInputNOT(CPacket msg)
    {
		int cnt = msg.pop_int32 ();
		if (cnt < 0) {
            Info.AddGameCount(cnt);
			if (Info.isCheckScene ("Main"))
				((PageMain)PageBase.Instance).RefreshGamePlay ();			
		} else {
            Info.AddOrderCount(cnt);
			if (Info.isCheckScene ("Main"))
				((PageMain)PageBase.Instance).StartFlyChance ();			
		}

		if (Info.isCheckScene ("Game"))
			((PageGame)PageBase.Instance).RefreshPlayCnt ();

        UIManager.Instance.ShowOrderAlarm();
    }

	void SlotStartACK(CPacket msg)
	{        
        int gameCnt = msg.pop_int32();
        Info.AddGameCount(gameCnt, true);
        short discountType = msg.pop_int16();

        ((PageGame)PageBase.Instance).FinishStart (discountType);
	}

    void TableDiscountInputACK(CPacket msg)
    {
        AdminTableDiscountInput.Instance.OnClose();
    }

    void TableDiscountInputNOT(CPacket msg)
    {
        UIManager.Instance.ShowDiscountAlarm();
    }

    void GetDiscountProb_ACK(CPacket msg)
    {
        List<float> listProb = new List<float>();

        for (int i = 0; i < 5; i++)
            listProb.Add(msg.pop_float());

        PageAdmin.Instance.ShowSettingDiscountProb(listProb);
    }

    void SetDiscountProb_ACK(CPacket msg)
    {
        SystemMessage.Instance.Add("설정이 완료 되었습니다.");
    }

    void TablePriceConfirm_ACK(CPacket msg)
    {
        byte tableNo = msg.pop_byte();
        int tablePrice = msg.pop_int32();
        int tableDiscount = msg.pop_int32();

        PageAdmin.Instance.ShowTableDiscountInput(tableNo, tablePrice, tableDiscount);
    }		

	void Surprise_ACK(CPacket msg)
	{
		Info.surpriseCnt = msg.pop_int32 ();

		GameObject obj = UIManager.Instance.Show (eUI.eSurpriseStart);
		UISurpriseStart uiSurprise = obj.GetComponent<UISurpriseStart>();
		if(uiSurprise)
			uiSurprise.PrevSet ();
	}

	void GameCountInputACK()
	{
		AdminTableGameCountInput.Instance.InputComplete ();
	}

	void GameCountInputNOT(CPacket msg)
	{
		int cnt = msg.pop_int32 ();
		if (cnt < 0) {
			Info.AddGameCount(cnt);
			if (Info.isCheckScene ("Main"))
				((PageMain)PageBase.Instance).RefreshGamePlay ();			
		} else {
			Info.AddOrderCount(cnt);
			if (Info.isCheckScene ("Main"))
				((PageMain)PageBase.Instance).StartFlyChance ();			
		}

		if (Info.isCheckScene ("Game"))
			((PageGame)PageBase.Instance).RefreshPlayCnt ();
	}

	void TableMoveACK(CPacket msg)
	{
		byte tableNo = msg.pop_byte ();
		AdminTableMove.Instance.MoveComplete ();
		NetworkManager.Instance.Logout_REQ (tableNo);
	}

	void TableMoveNOT()
	{
		NetworkManager.Instance.disconnect ();
		SceneChanger.LoadScene ("Login", null);
	}
}