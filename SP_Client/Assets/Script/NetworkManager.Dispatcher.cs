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
		case PROTOCOL.ENTER_CUSTOMER_ACK:	EnterCustormerACK (msg);	break;
		case PROTOCOL.ENTER_CUSTOMER_NOT:	EnterCustormerNOT (msg);	break;
		case PROTOCOL.WAITER_CALL_ACK:		WaiterCallACK ();			break;
		case PROTOCOL.WAITER_CALL_NOT:		WaiterCallNOT (msg);		break;
		case PROTOCOL.ORDER_ACK:			OrderACK ();				break;
		case PROTOCOL.ORDER_NOT:			OrderNOT (msg);				break;
        case PROTOCOL.CHAT_ACK:             ChatACK(msg);               break;
        case PROTOCOL.CHAT_NOT:             ChatNOT(msg);               break;
        case PROTOCOL.ORDER_DETAIL_ACK:     OrderDetailACK(msg);        break;
		case PROTOCOL.GAME_DISCOUNT_ACK:	GameDiscountACK (msg);		break;
		case PROTOCOL.GAME_DISCOUNT_NOT:	GameDiscountNOT (msg);		break;
		case PROTOCOL.REQUEST_MUSIC_ACK:	RequestMusicACK (msg);		break;
		case PROTOCOL.REQUEST_MUSIC_NOT:	RequestMusicNOT (msg); 		break;
		case PROTOCOL.REQUEST_MUSIC_LIST_ACK: RequestMusicListACK (msg); break;
        case PROTOCOL.REQUEST_MUSIC_REMOVE_ACK: RequestMusicRemoveACK(msg); break;
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
            Info.TableNum = byte.Parse (pop_string);
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

        Info.SetLogoutOtherUser(tableNo);

		if (SceneManager.GetActiveScene ().name == "Admin")
			PageAdmin.Instance.SetLogout ((int)tableNo);
		else
			SceneChanger.LoadScene ("Login", PageBase.Instance.curBoardObj ());
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
        string packingMenu = msg.pop_string();
        string packingCnt = msg.pop_string();
        if (UIManager.Instance.IsActive(eUI.eBillDetail))
            return;
        
        GameObject obj = UIManager.Instance.Show(eUI.eBillDetail);
        UIBillDetail uiBillDetail = obj.GetComponent<UIBillDetail>();
        uiBillDetail.SetBill(packingMenu, packingCnt);
    }

	void GameDiscountACK(CPacket msg)
	{
		Info.GameDiscountWon = -1;
		PageTokyoLive.Instance.ReturnHome ();
	}

	void GameDiscountNOT(CPacket msg)
	{
		byte tableNo = msg.pop_byte ();
		short discount = msg.pop_int16 ();
		PageAdmin.Instance.SetOrder (tableNo, discount);
	}

	void RequestMusicListACK(CPacket msg)
	{
		string packing = msg.pop_string ();
		GameObject obj = UIManager.Instance.Show (eUI.eMusicRequest);
		UIMusicRequest mr = obj.GetComponent<UIMusicRequest> ();
        mr.SetAddMusicList (packing);
	}

    void RequestMusicACK(CPacket msg)
    {
        int priority = msg.pop_int32();
        string packing = msg.pop_string ();
        if (UIManager.Instance.IsActive (eUI.eMusicRequest)) 
        {
            GameObject obj = UIManager.Instance.GetUI (eUI.eMusicRequest);
            UIMusicRequest mr = obj.GetComponent<UIMusicRequest> ();
            mr.SetAddMusic (priority, packing);
        }
    }

    void RequestMusicNOT(CPacket msg)
    {
        string packing = msg.pop_string();
        PageAdmin.Instance.SetRequestMusic(packing);
    }

    void RequestMusicRemoveACK(CPacket msg)
    {
        int removeReqMusicID = msg.pop_int32();
        PageAdmin.Instance.RemoveRequestMusic(removeReqMusicID);
    }
}
