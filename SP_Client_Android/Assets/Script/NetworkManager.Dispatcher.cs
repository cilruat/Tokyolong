using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FreeNet;
using SP_Server;
using FlappyBirdStyle;
using LitJson;

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
		case PROTOCOL.FAILED_NOT_NUMBER:
        case PROTOCOL.FAILED_ALREADY_SEND_LIKE:     Failed (protocol_id);			break;
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
		case PROTOCOL.GAME_COUNT_INPUT_ACK:			GameCountInputACK ();			break;
		case PROTOCOL.GAME_COUNT_INPUT_NOT:			GameCountInputNOT (msg);		break;
		case PROTOCOL.TABLE_MOVE_ACK:				TableMoveACK (msg);				break;
		case PROTOCOL.TABLE_MOVE_NOT:				TableMoveNOT ();				break;
		case PROTOCOL.OWNER_GAME_NOT:				OwnerGameNOT (msg);				break;
        case PROTOCOL.MSG_SEND_ACK:                 MsgSendACK(msg);                break;
        case PROTOCOL.MSG_SEND_NOT:                 MsgSendNOT(msg);                break;
        case PROTOCOL.LKE_SEND_ACK:                 LkeSendACK(msg);                break;
        case PROTOCOL.LKE_SEND_NOT:                 LkeSendNOT(msg);                break;
        case PROTOCOL.PRESENT_SEND_ACK:             PresentSendACK(msg);            break;
        case PROTOCOL.PRESENT_SEND_NOT:             PresentSendNOT(msg);            break;
        case PROTOCOL.PLZ_SEND_ACK:                 PleaseSendACK(msg);            break;
        case PROTOCOL.PLZ_SEND_NOT:                 PleaseSendNOT(msg);            break;
        case PROTOCOL.CASH_SEND_ACK:                CashSendACK(msg);              break;
        case PROTOCOL.CASH_SEND_NOT:                CashSendNOT(msg);              break;
        case PROTOCOL.GAME_VERSUS_INVITE_ACK:       GameVersusInviteACK(msg); break;
        case PROTOCOL.GAME_VERSUS_INVITE_NOT:       GameVersusInviteNOT(msg); break;
        case PROTOCOL.GAME_REFUSE_ACK:              GameRefuseACK(msg); break;
        case PROTOCOL.GAME_REFUSE_NOT:              GameRefuseNOT(msg); break;
        case PROTOCOL.GAME_ACCEPT_ACK:              GameAcceptACK(msg); break;
        case PROTOCOL.GAME_ACCEPT_NOT:              GameAcceptNOT(msg); break;


        }

        isSending = false;
	}

	void Failed(PROTOCOL id)
	{
		switch(id)
		{
		case PROTOCOL.FAILED:				    SystemMessage.Instance.Add ("동작을 실패했어요");		break;
		case PROTOCOL.FAILED_NOT_NUMBER:	    SystemMessage.Instance.Add ("숫자로 입력해주세요");		break;
        case PROTOCOL.FAILED_ALREADY_SEND_LIKE: SystemMessage.Instance.Add ("한 테이블에 한번만 보낼 수 있습니다");    break;
		}
	}

	void LoginACK(CPacket msg)
	{
		string pop_string = msg.pop_string ();
        Info.adminTablePacking = msg.pop_string();
        if (pop_string == "admin")
        {
            Info.adminOrderPacking = msg.pop_string ();
            Info.adminMusicPacking = msg.pop_string ();
            SceneChanger.LoadScene("Admin", PageBase.Instance.curBoardObj());
        }
		else {
            Info.TableNum = byte.Parse (pop_string);
            int gameCnt = msg.pop_int32 ();
            Info.AddGameCount(gameCnt, true);

            //추가

            int existUser = msg.pop_int32 ();
			if (existUser == 1) {
				Info.PersonCnt = msg.pop_byte ();
				Info.ECustomer = (ECustomerType)msg.pop_byte ();

				string packing = msg.pop_string ();
				Info.SetLoginedOtherUser (packing);

                SceneChanger.LoadScene ("Order", PageBase.Instance.curBoardObj ());
			} else
				((PageLogin)PageBase.Instance).SuccessLogin ();

            Info.listLoginTable.Clear();
            if (string.IsNullOrEmpty(Info.adminTablePacking) == false)
            {
                JsonData tableJson = JsonMapper.ToObject(Info.adminTablePacking);
                for (int i = 0; i < tableJson.Count; i++)
                    Info.listLoginTable.Add(int.Parse(tableJson[i].ToString()));
            }
        }
	}

	void LoginNOT(CPacket msg)
	{
		byte tableNo = msg.pop_byte ();
        //PageAdmin.Instance.SetLogin((int)tableNo);

        //if (tableNo == Info.AdminTableNum)

        int findTableIdx = Info.listLoginTable.FindIndex(x => x == (int)tableNo);
        if (findTableIdx == -1)
            Info.listLoginTable.Add((int)tableNo);

        if (Info.isCheckScene("Admin"))
        {
            PageAdmin.Instance.SetLogin((int)tableNo);
            Debug.Log("어드민");

        }
        if (Info.isCheckScene("Mail"))
        {
            PageMail.Instance.SetLogin((int)tableNo);
            Debug.Log("메일");
        }
        else
        {
            Debug.Log("메인화면");
        }        
    }

    //Admin In 
    void LogoutACK(CPacket msg)
    {
        byte tableNo = msg.pop_byte ();
        //PageAdmin.Instance.SetLogout((int)tableNo);        

        //if (tableNo == Info.AdminTableNum)
        if (Info.isCheckScene("Admin"))
            PageAdmin.Instance.SetLogout((int)tableNo);
        
    }

    //Other Users 
    void LogoutNOT(CPacket msg)
	{
		byte tableNo = msg.pop_byte ();

        int findTableIdx = Info.listLoginTable.FindIndex(x => x == (int)tableNo);
        if (findTableIdx != -1)
            Info.listLoginTable.RemoveAt(findTableIdx);

        if (tableNo == Info.TableNum)
        {
            Info.Init();
            UIManager.Instance.Hide_All();
            NetworkManager.Instance.disconnect();
            SceneChanger.LoadScene ("Login", PageBase.Instance.curBoardObj ());
        }
        else
        { 
            Info.SetLogoutOtherUser(tableNo);
            if (Info.isCheckScene("Mail"))
                PageMail.Instance.SetLogout((int)tableNo);
        }
    }

    void EnterCustormerACK(CPacket msg)
	{
		Info.PersonCnt = msg.pop_byte ();
		Info.ECustomer = (ECustomerType)msg.pop_byte ();

		string packing = msg.pop_string ();
        Info.SetLoginedOtherUser (packing);

		SceneChanger.LoadScene ("Order", PageBase.Instance.curBoardObj ());
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

		Info.firstOrder = System.Convert.ToBoolean(msg.pop_byte());
		Info.firstOrder = false;

		if (Info.isCheckScene ("Order"))
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
		} else if (UIManager.Instance.IsActive (eUI.eBillDetail) == false) {
			GameObject obj = UIManager.Instance.Show (eUI.eBillDetail);
			UIBillDetail uiBillDetail = obj.GetComponent<UIBillDetail> ();
			uiBillDetail.SetBill (menuPacking, discountPrice);
		}         
    }

	void GameDiscountACK(CPacket msg)
	{
		GameObject obj = UIManager.Instance.Show (eUI.eDiscountAni);
		UIDiscountAnimation ui = obj.GetComponent<UIDiscountAnimation> ();
		ui.SendREQ ();
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
		switch (type) {
		case ERequestOrderType.eOrder:
			UIManager.Instance.ShowOrderAlarm ();
			break;
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

	void GameCountInputACK()
	{
        if (Info.isCheckScene("Admin"))
            AdminTableGameCountInput.Instance.InputComplete ();
	}

	void GameCountInputNOT(CPacket msg)
	{
		int cnt = msg.pop_int32 ();
        Info.AddGameCount(cnt);

        if (Info.isCheckScene("Main"))
        {
            if (cnt < 0)
                ((PageMain)PageBase.Instance).RefreshGamePlay();
            else
                ((PageMain)PageBase.Instance).StartFlyChance();
        }

        if (Info.isCheckScene("Game"))
            ((PageGame)PageBase.Instance).RefreshPlayCnt();
        else if (Info.isCheckScene("Lotto"))
            PlayerMeta.RefreshGold(cnt);
        else if (Info.isCheckScene("JJangGameBbo"))
            JjangGameBbo.Instance.ShowCoin();
        else if (Info.isCheckScene("SpinWheel"))
            GameBench.UIManager.Instance.RefreshCoins();
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

	void OwnerGameNOT(CPacket msg)
	{
		byte ownerIdx = msg.pop_byte ();

		eUI ownerUI = eUI.eOwnerGame;
		switch (ownerIdx) {
		case 0:		ownerUI = eUI.eOwnerGame;	break;
		case 1:		ownerUI = eUI.eOwnerQuiz;	break;
		case 2:		ownerUI = eUI.eOwnerTrick;	break;
		}

		Info.OwnerUI = ownerUI;
		Info.CheckOwnerEvt = true;
	}

    void MsgSendACK(CPacket msg)
    {
        byte tableNo = msg.pop_byte();
        SystemMessage.Instance.Add(tableNo.ToString() + "번 테이블에 성공적으로 [쪽지]를 보냈습니다 ꈍ◡ꈍ");

    }


    void MsgSendNOT(CPacket msg)
    {
        byte tableNo = msg.pop_byte(); //서버꺼
        string strMsg = msg.pop_string(); //서버꺼 패킷과 구조를 일치

        UserMsgInfo msginfo = new UserMsgInfo(); //클래스를 쓰도록하고
        msginfo.tableNo = tableNo;
        msginfo.strMsg = strMsg;

        Info.myInfo.listMsgInfo.Add(msginfo);

        if (Info.isCheckScene("Mail"))
            PageMail.Instance.SetMail(msginfo);
        
        UIManager.Instance.ShowMsg();
    }

    void LkeSendACK(CPacket msg)
    {
        byte tableNo = msg.pop_byte();
        SystemMessage.Instance.Add(tableNo.ToString() + "번 테이블에 좋아요 ღ'ᴗ'ღ 했어요~♥");
        //UIMANAGER 끄기
        if (Info.isCheckScene("Mail"))
        {
            PageMail.Instance.RefreshGamePlayChance();
        }

    }

    void LkeSendNOT(CPacket msg)
    {
        byte tableNo = msg.pop_byte();
        int gameCount = msg.pop_int32();


        UserLikeInfo likeinfo = new UserLikeInfo(); //클래스를 쓰도록하고
        likeinfo.tableNo = tableNo;

        Info.myInfo.listLikeInfo.Add(likeinfo);

        if (Info.isCheckScene("Mail"))
            PageMail.Instance.SetLike(likeinfo);

        if (gameCount < 0)
        {
            Info.AddGameCount(gameCount);
            if (Info.isCheckScene("Main"))
                ((PageMain)PageBase.Instance).RefreshGamePlay();
        }
        else
        {
            Info.AddOrderCount(gameCount);
            if (Info.isCheckScene("Main"))
                ((PageMain)PageBase.Instance).StartFlyChance();
        }

        if (Info.isCheckScene("Game"))
            ((PageGame)PageBase.Instance).RefreshPlayCnt();

        if (Info.isCheckScene("Mail"))
        {
            PageMail.Instance.RefreshGamePlayChance();
        }



        UIManager.Instance.ShowLike();

    }


    void PresentSendACK(CPacket msg)
    {
        byte tableNo = msg.pop_byte();
        int game_cnt = msg.pop_int32();
        Info.AddGameCount(game_cnt, true);

        if (Info.isCheckScene("Mail"))
        {
            PageMail.Instance.RefreshGamePlayChance();
        }

    }

    void  PresentSendNOT(CPacket msg)
    {
        byte tableNo = msg.pop_byte();
        int gameCnt = msg.pop_int32();

        UserPresentInfo presentInfo = new UserPresentInfo(); //새로운걸 만들어야겟군, 클래스 쓰도록한다

        presentInfo.tableNo = tableNo;
        presentInfo.presentCount = gameCnt;

        Info.myInfo.listPresentInfo.Add(presentInfo);

        if (Info.isCheckScene("Mail"))
            PageMail.Instance.SetPresent(presentInfo);


        //gameCnt == 요청값, GamePlayCnt == 보유값
        //if(Info.GamePlayCnt >= gameCnt)
        //{
        if (gameCnt < 0)
            {
                Info.AddGameCount(gameCnt);
                if (Info.isCheckScene("Main"))
                    ((PageMain)PageBase.Instance).RefreshGamePlay();
            }
            else
            {
                Info.AddOrderCount(gameCnt);
                if (Info.isCheckScene("Main"))
                    ((PageMain)PageBase.Instance).StartFlyChance();
            }

            if (Info.isCheckScene("Game"))
                ((PageGame)PageBase.Instance).RefreshPlayCnt();

        if (Info.isCheckScene("Mail"))
        {
            PageMail.Instance.RefreshGamePlayChance();
        }



        UIManager.Instance.ShowPresent();


    }

    //UIMANAGER 서 활동할 내용 작업

    void PleaseSendACK(CPacket msg)
    {
        byte tableNo = msg.pop_byte();
        SystemMessage.Instance.Add(tableNo.ToString() + "번에게 조르기ƪ(•ε•)∫ ☆완료★ ");
        //UIMANAGER 끄기
    }

    void PleaseSendNOT(CPacket msg)
    {
        byte tableNo = msg.pop_byte(); //서버꺼
        int plzCount = msg.pop_int32(); //서버꺼 패킷과 구조를 일치

        UserPlzInfo plzInfo = new UserPlzInfo(); //새로운걸 만들어야겟군, 클래스 쓰도록한다

        plzInfo.tableNo = tableNo;
        plzInfo.plzCount = plzCount;

        Info.myInfo.listPlzInfo.Add(plzInfo);

        if (Info.isCheckScene("Mail"))
            PageMail.Instance.SetPlz(plzInfo);



        UIManager.Instance.ShowPlz();

    }

    void CashSendACK(CPacket msg)
    {
        int game_cnt = msg.pop_int32();
        Info.AddGameCount(game_cnt, true);
        PageCashShop.Instance.RefreshGamePlayChance();
        SystemMessage.Instance.Add("코인을 사용해 주문을 넣었습니다");


    }

    void CashSendNOT(CPacket msg)
    {
        byte tableNo = msg.pop_byte();
        string title = msg.pop_string();
        //int game_cnt = msg.pop_int32();



        if (Info.isCheckScene("Admin"))
        {
            Debug.Log(tableNo.ToString() + "캐쉬를 보내는 번호");
            Debug.Log(Info.TableNum.ToString() + "내가 접속한 번호");

            UserCashInfo cashInfo = new UserCashInfo();
            cashInfo.tableNo = tableNo;
            cashInfo.reqCashItem = title;
            PageAdmin.Instance.SetCash(cashInfo);
        }

        if (Info.isCheckScene("CashShop") && tableNo == Info.TableNum)
        {
            Debug.Log(tableNo.ToString() + "캐쉬를 보내는 번호");
            Debug.Log(Info.TableNum.ToString() + "내가 접속한 번호");

            UserCashInfo cashInfo = new UserCashInfo();
            cashInfo.tableNo = tableNo;
            cashInfo.reqCashItem = title;
            Info.myInfo.listCashInfo.Add(cashInfo);
            PageCashShop.Instance.SetCash(cashInfo);
        }

    }


    void GameVersusInviteACK(CPacket msg)
    {
        byte tableNo = msg.pop_byte();
        SystemMessage.Instance.Add(tableNo.ToString() + "번에게 초대를 보냈습니다");
    }

    void GameVersusInviteNOT(CPacket msg)
    {
        byte tableNo = msg.pop_byte();
        int reqGameCnt = msg.pop_int32();
        string gameName = msg.pop_string();


        
        UserGameInfo gameInfo = new UserGameInfo();

        gameInfo.tableNo = tableNo;
        gameInfo.reqGameCnt = reqGameCnt;
        gameInfo.gameName = gameName;

        
        Info.myInfo.listGameInfo.Add(gameInfo);

        if (Info.isCheckScene("Mail"))
            PageMail.Instance.SetGame(gameInfo);
        
        

        UIManager.Instance.ShowGameInvite();

    }


    void GameRefuseACK(CPacket msg)
    {
        byte tableNo = msg.pop_byte();
        SystemMessage.Instance.Add(tableNo.ToString() + "번에게 미안하다고했어요");
    }

    void GameRefuseNOT(CPacket msg)
    {
        byte tableNo = msg.pop_byte();

        UserRefuseInfo refuseInfo = new UserRefuseInfo();

        refuseInfo.tableNo = tableNo;
        Info.myInfo.listRefuseInfo.Add(refuseInfo);



        UIManager.Instance.ShoweGameRefuse();
    }

    //예외처리해야될게 여러사람에게서 계속 게임신청이 들어온다면? 불값 넣기

    void GameAcceptACK(CPacket msg)
    {
        byte tableNo = msg.pop_byte();
        SystemMessage.Instance.Add(tableNo.ToString() + "수락누르면 나한텐 이게뜸");

    }

    void GameAcceptNOT(CPacket msg)
    {
        byte tableNo = msg.pop_byte();
        int reqGameCnt = msg.pop_int32();
        string gameName = msg.pop_string();



        UserGameAcceptInfo gameInfo = new UserGameAcceptInfo();


        gameInfo.tableNo = tableNo;
        gameInfo.reqGameCnt = reqGameCnt;
        gameInfo.gameName = gameName;

        Info.myInfo.listGameAcceptInfo.Add(gameInfo);

        //인포에 정보를 담았으니깐 괜찮지 않냐?



        if (gameName == "가위바위보")
        {
            SceneChanger.LoadScene("VersusLobby", PageBase.Instance.curBoardObj());

            VersusManager Versus = GetComponent<VersusManager>();
            Versus.ShowGame();

        }
        else
            Debug.Log("안된다");
    }




}


