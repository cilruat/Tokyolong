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
        #if UNITY_EDITOR
        Debug.Log(protocol_id.ToString());
        #endif

		switch (protocol_id)
		{
		case PROTOCOL.FAILED_NOT_NUMBER:	Failed (protocol_id);		break;
		case PROTOCOL.LOGIN_ACK:			LoginACK (msg);				break;
		case PROTOCOL.LOGIN_NOT:			LoginNOT (msg);				break;
        case PROTOCOL.LOGOUT_ACK:			LogoutACK (msg);			break;
		case PROTOCOL.LOGOUT_NOT:			LogoutNOT (msg);			break;
		case PROTOCOL.ENTER_CUSTOMER_ACK:	EnterCustormerACK (msg);	break;
		case PROTOCOL.ENTER_CUSTOMER_NOT:	EnterCustormerNOT (msg);	break;
		case PROTOCOL.WAITER_CALL_ACK:		WaiterCallACK ();			break;
		case PROTOCOL.WAITER_CALL_NOT:		WaiterCallNOT (msg);		break;
		case PROTOCOL.ORDER_ACK:			OrderACK (msg);				break;
		case PROTOCOL.ORDER_NOT:			OrderNOT (msg);				break;
        case PROTOCOL.CHAT_ACK:             ChatACK(msg);               break;
        case PROTOCOL.CHAT_NOT:             ChatNOT(msg);               break;
        case PROTOCOL.ORDER_DETAIL_ACK:     OrderDetailACK(msg);        break;
		case PROTOCOL.GAME_DISCOUNT_ACK:	GameDiscountACK (msg);		break;
		case PROTOCOL.GAME_DISCOUNT_NOT:	GameDiscountNOT (msg);		break;
		case PROTOCOL.REQUEST_MUSIC_ACK:	RequestMusicACK (msg);		break;
		case PROTOCOL.REQUEST_MUSIC_NOT:	RequestMusicNOT (msg); 		break;
		case PROTOCOL.REQUEST_MUSIC_LIST_ACK: RequestMusicListACK (msg);    break;
        case PROTOCOL.REQUEST_MUSIC_REMOVE_ACK: RequestMusicRemoveACK(msg); break;
        case PROTOCOL.REQUEST_MUSIC_REMOVE_NOT: RequestMusicRemoveNOT(msg); break;
        case PROTOCOL.ORDER_CONFIRM_ACK:        OrderConfirmACK(msg);       break;
        case PROTOCOL.ORDER_CONFIRM_NOT:        OrderConfirmNOT(msg);       break;
        case PROTOCOL.TABLE_ORDER_CONFIRM_ACK:  TableOrderConfirmACK(msg);  break;
        case PROTOCOL.TABLE_ORDER_INPUT_ACK:    TableOrderInputACK(msg);    break;
        case PROTOCOL.TABLE_ORDER_INPUT_NOT:    TableOrderInputNOT(msg);    break;
		case PROTOCOL.SLOT_START_ACK:			SlotStartACK (msg);			break;
		case PROTOCOL.REPORT_OFFLINE_GAME_ACK:	ReportOfflineGameACK ();	break;
		case PROTOCOL.UNFINISH_GAME_LIST_ACK:	UnfinishGameListACK(msg);	break;
		case PROTOCOL.UNFINISH_GAME_CONFIRM_ACK:	UnfinishGameConfirmNOT (msg);	break;
		case PROTOCOL.UNFINISH_GAME_CONFIRM_NOT:	UnfinishGameConfirmNOT (msg);	break;
        case PROTOCOL.TABLE_DISCOUNT_INPUT_ACK:     TableDiscountInputACK(msg);     break;
        case PROTOCOL.TABLE_DISCOUNT_INPUT_NOT:     TableDiscountInputNOT(msg);     break;
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
            Info.adminTablePacking = msg.pop_string ();
            Info.adminOrderPacking = msg.pop_string ();
            Info.adminMusicPacking = msg.pop_string ();
            SceneChanger.LoadScene("Admin", PageBase.Instance.curBoardObj());
        }
		else {
            Info.TableNum = byte.Parse (pop_string);
			Info.GamePlayCnt = (byte)msg.pop_int32 ();

			int existUser = msg.pop_int32 ();
			if (existUser == 1) {
				Info.PersonCnt = msg.pop_byte ();
				Info.ECustomer = (ECustomerType)msg.pop_byte ();

				string packing = msg.pop_string ();
				Info.SetLoginedOtherUser (packing);

				SceneChanger.LoadScene ("Main", PageBase.Instance.curBoardObj ());
			}
			else
				PageLogin.Instance.OnNext ();
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
		Info.GamePlayCnt = msg.pop_byte ();
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
        UserChat newChat = new UserChat(1, time, chat);

        Info.AddUserChatInfo(otherTableNo, newChat, false);
        GameObject obj = UIManager.Instance.GetCurUI();
        UIChat uiChat = obj.GetComponent<UIChat>();
        uiChat.chatBoard.AddChatElt(Info.myInfo, newChat);
    }

    void ChatNOT(CPacket msg)
    {
        byte otherTableNo = msg.pop_byte ();
        string time = msg.pop_string();
        string chat = msg.pop_string();
        UserChat newChat = new UserChat(0, time, chat);

        bool isActive = UIManager.Instance.IsActive(eUI.eChat);
        Info.AddUserChatInfo(otherTableNo, newChat, !isActive);

        if (UIManager.Instance.IsActive(eUI.eChat))
        {
            GameObject obj = UIManager.Instance.GetCurUI();
            UIChat uiChat = obj.GetComponent<UIChat>();
            uiChat.chatBoard.AddChatElt(Info.GetUser(otherTableNo), newChat);
        }
        else
            UIManager.Instance.ShowChatAlarm();        
    }

    void OrderDetailACK(CPacket msg)
    {
        string menuPacking = msg.pop_string();
        string discountPacking = msg.pop_string();
        if (UIManager.Instance.IsActive(eUI.eBillDetail))
            return;
        
        GameObject obj = UIManager.Instance.Show(eUI.eBillDetail);
        UIBillDetail uiBillDetail = obj.GetComponent<UIBillDetail>();
        uiBillDetail.SetBill(menuPacking, discountPacking);
    }

	void GameDiscountACK(CPacket msg)
	{
		Info.GameDiscountWon = -1;

		if (Info.isCheckScene ("TokyoLive"))
			PageTokyoLive.Instance.ReturnHome ();
		else if (Info.isCheckScene ("PicturePuzzle"))
			PagePicturePuzzle.Instance.ReturnHome ();
	}

	void GameDiscountNOT(CPacket msg)
	{
        byte type = msg.pop_byte();
        int orderId = msg.pop_int32();
        byte tableNo = msg.pop_byte ();
        string packing = msg.pop_string ();

        RequestOrder reqOrder = new RequestOrder(type, orderId, tableNo, packing);

        PageAdmin.Instance.SetOrder (reqOrder);
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
        int priority = msg.pop_int32();
        string packing = msg.pop_string ();
        GameObject obj = UIManager.Instance.GetUI (eUI.eMusicRequest);
        UIRequestMusic uiReqMusic = obj.GetComponent<UIRequestMusic> ();
        uiReqMusic.SetAddMusic (priority, packing);
    }

    void RequestMusicNOT(CPacket msg)
    {
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
        if (UIManager.Instance.IsActive(eUI.eMusicRequest) == false)
            return;

        int removeReqMusicID = msg.pop_int32();
        GameObject obj = UIManager.Instance.GetUI(eUI.eMusicRequest);
        UIRequestMusic uiReqMusic = obj.GetComponent<UIRequestMusic> ();
        uiReqMusic.RemoveRequestMusic(removeReqMusicID);
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
        switch (type)
        {
            case ERequestOrderType.eOrder:      UIManager.Instance.ShowOrderAlarm();    break;
            case ERequestOrderType.eDiscount:   UIManager.Instance.ShowDiscountAlarm(); break;
        }
    }

    void TableOrderConfirmACK(CPacket msg)
    {
        byte tableNo = msg.pop_byte();
        string orderPacking = msg.pop_string();
        string discountPacking = msg.pop_string();
        AdminTableMenu.Instance.OnClose();
        PageAdmin.Instance.ShowBillConfirm (tableNo, orderPacking, discountPacking);
    }

    void TableOrderInputACK(CPacket msg)
    {
        if(PageAdmin.Instance.objTableOrderInput.activeSelf)
            AdminTableOrderInput.Instance.OnCompleteTableOrderInput();

        if(PageAdmin.Instance.objBillConfirm.activeSelf)
            AdminBillConfirm.Instance.OnCompleteTableOrderInput();
    }

    void TableOrderInputNOT(CPacket msg)
    {
        Info.GamePlayCnt = msg.pop_byte();
        if (Info.isCheckScene("Main"))
            ((PageMain)PageBase.Instance).RefreshGamePlay();

        UIManager.Instance.ShowOrderAlarm();
    }

	void SlotStartACK(CPacket msg)
	{
		Info.GamePlayCnt = msg.pop_byte ();

		if (Info.GamePlayCnt < 0)
			Info.GamePlayCnt = 0;

		((PageGame)PageBase.Instance).FinishStart ();
	}

	void ReportOfflineGameACK()
	{
		NetworkManager.Instance.UnfinishGamelist_REQ (Info.TableNum);
	}

	void UnfinishGameListACK(CPacket msg)
	{
		string packing = msg.pop_string ();
		byte tableNo = msg.pop_byte ();

		if (Info.isCheckScene ("Admin"))
			PageAdmin.Instance.ShowUnfinishGameList (packing, tableNo);
		else
			((PageGame)PageBase.Instance).RefreshUnfinishList (packing);
	}

	void UnfinishGameConfirmNOT(CPacket msg)
	{
		int id = msg.pop_int32 ();
        if (Info.isCheckScene("Admin"))
            PageAdmin.Instance.RemoveUnfinishGame(id);
        else
        {
            UIManager.Instance.ShowDiscountAlarm();
            if(Info.isCheckScene("Game"))
                ((PageGame)PageBase.Instance).RemoveUnfinishGame(id);
        }
	}

    void TableDiscountInputACK(CPacket msg)
    {
        AdminTableDiscountInput.Instance.OnClose();
    }

    void TableDiscountInputNOT(CPacket msg)
    {
        UIManager.Instance.ShowDiscountAlarm();
    }
}
