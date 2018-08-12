using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using FreeNet;
using LitJson;

namespace SP_Server.UserState
{
    class UserLobbyState : IUserState
    {
        User owner;

        public UserLobbyState(User owner)
        {
            this.owner = owner;
        }

        void IUserState.on_message(FreeNet.CPacket msg)
        {
            StackFrame stackFrame = new StackFrame(1, true);

            try
            {
                PROTOCOL protocol = (PROTOCOL)msg.pop_protocol_id();

                owner.mainFrm.BeginInvoke(owner.mainFrm.WriteLogInstance,
                        new object[] { "[table Num : " + owner.tableNum.ToString() + "] protocol id " + protocol,
                        stackFrame.GetMethod().Name, stackFrame.GetFileName(),
                        stackFrame.GetFileLineNumber().ToString() });

                CPacket send_msg = null;
                CPacket other_msg = null;
                byte tableNo = 0;
                switch (protocol)
                {
                    case PROTOCOL.LOGIN_REQ:
                        bool existUser = false;
                        int tableNum = 0;

                        string tableNoStr = msg.pop_string();
                        if (tableNoStr == "admin")
                        {
                            tableNum = 10000;
                            Frm.SetAdminUser(owner);
                        }
                        else
                        {
                            if (int.TryParse(tableNoStr, out tableNum) == false)
                            {
                                send_msg = CPacket.create((short)PROTOCOL.FAILED_NOT_NUMBER);
                                break;
                            }

                            tableNum = int.Parse(tableNoStr);

                            UserInfo getUserInfo = null;
                            if (owner.mainFrm.AddUserInfo(tableNum, ref getUserInfo) == false)
                            {
                                owner.info = getUserInfo;
                                existUser = true;
                            }                                

                            // Admin Send packet
                            if(Frm.GetAdminUser() != null)
                            {
                                other_msg = CPacket.create((short)PROTOCOL.LOGIN_NOT);
                                other_msg.push(tableNum);
                                Frm.GetAdminUser().send(other_msg);
                            }                            
                        }

                        owner.tableNum = tableNum;                        

                        send_msg = CPacket.create((short)PROTOCOL.LOGIN_ACK);
                        send_msg.push(tableNoStr);

                        if (tableNoStr == "admin")
                        {
                            List<int> listUserTableNo = new List<int>();

                            foreach (UserInfo e in owner.mainFrm.dictUserInfo.Values)
                            {
                                UserInfo userInfo = e;
                                if (userInfo.IsAdmin())
                                    continue;

                                listUserTableNo.Add(userInfo.tableNum);
                            }

                            List<RequestOrder> listReqOrder = owner.mainFrm.listRequestOrder;
                            List<RequestMusicInfo> listReqMusic = owner.mainFrm.listReqMusicInfo;

                            send_msg.push((JsonMapper.ToJson(listUserTableNo)).ToString());
                            send_msg.push((JsonMapper.ToJson(listReqOrder)).ToString());
                            send_msg.push((JsonMapper.ToJson(listReqMusic)).ToString());
                        }
                        else
                        {                            
                            send_msg.push(owner.mainFrm.GetGameCount(tableNum));
                            send_msg.push(existUser ? 1 : 0);

                            // 유저 정보가 있는경우 추가 패킷
                            if (existUser)
                            {
                                send_msg.push(owner.info.peopleCnt);
                                send_msg.push(owner.info.customerType);

                                List<UserInfo> list = new List<UserInfo>();
                                list.Add(owner.info);
                                // 접속된 유저들에게 현재 접속 유저 정보 전송
                                for (int i = 0; i < owner.mainFrm.ListUser.Count; i++)
                                {
                                    User user = owner.mainFrm.ListUser[i];
                                    if (user.tableNum == 10000 ||
                                        user.tableNum <= 0 ||
                                        user.info == null)
                                        continue;

                                    list.Add(user.info);

                                    if (user.info.tableNum == owner.info.tableNum)
                                        continue;

                                    other_msg = CPacket.create((short)PROTOCOL.ENTER_CUSTOMER_NOT);
                                    JsonData loginUser = JsonMapper.ToJson(owner.info);
                                    other_msg.push(loginUser.ToString());
                                    user.send(other_msg);
                                }

                                JsonData json = JsonMapper.ToJson(list);
                                send_msg.push(json.ToString());
                            }                            
                        }                           
                        break;
                    case PROTOCOL.LOGOUT_REQ:
                        tableNo = msg.pop_byte();

                        // 해당 테이블 로그인 화면으로 보내기
                        for (int i = 0; i < owner.mainFrm.ListUser.Count; i++)
                        {
                            User user = owner.mainFrm.ListUser[i];
                            if (user.IsAdmin)
                                continue;

                            other_msg = CPacket.create((short)PROTOCOL.LOGOUT_NOT);
                            other_msg.push(tableNo);
                            user.send(other_msg);
                        }
                        
                        owner.mainFrm.RemoveUserData((int)tableNo);

                        send_msg = CPacket.create((short)PROTOCOL.LOGOUT_ACK);
                        send_msg.push(tableNo);

                        break;
                    case PROTOCOL.ENTER_CUSTOMER_REQ:
                        tableNo = msg.pop_byte();
                        byte peopleCnt = msg.pop_byte();
                        byte customerType = msg.pop_byte();

                        // 유저 리스트에 정보 입력하기                        
                        owner.info = new UserInfo(owner.tableNum, peopleCnt, customerType);
                        owner.mainFrm.SetUserInfo(tableNo, owner.info);

                        List<UserInfo> listUserInfo = new List<UserInfo>();
                        listUserInfo.Add(owner.info);
                        // 접속된 유저들에게 현재 접속 유저 정보 전송
                        for (int i = 0; i < owner.mainFrm.ListUser.Count; i++)
                        {
                            User user = owner.mainFrm.ListUser[i];
                            if (user.tableNum == 10000 || 
                                user.tableNum <= 0 || 
                                user.info == null)
                                continue;

                            listUserInfo.Add(user.info);

                            if (user.info.tableNum == owner.info.tableNum)
                                continue;

                            other_msg = CPacket.create((short)PROTOCOL.ENTER_CUSTOMER_NOT);
                            JsonData loginUser = JsonMapper.ToJson(owner.info);
                            other_msg.push(loginUser.ToString());
                            user.send(other_msg);
                        }

                        send_msg = CPacket.create((short)PROTOCOL.ENTER_CUSTOMER_ACK);
                        send_msg.push(peopleCnt);
                        send_msg.push(customerType);

                        JsonData listUerJson = JsonMapper.ToJson(listUserInfo);
                        send_msg.push(listUerJson.ToString());
                        break;
                    case PROTOCOL.WAITER_CALL_REQ:
                        tableNo = msg.pop_byte();

                        // Admin Send packet
                        if (Frm.GetAdminUser() != null)
                        {
                            other_msg = CPacket.create((short)PROTOCOL.WAITER_CALL_NOT);
                            other_msg.push(tableNo);
                            Frm.GetAdminUser().send(other_msg);
                        }

                        send_msg = CPacket.create((short)PROTOCOL.WAITER_CALL_ACK);                        
                        break;
                    case PROTOCOL.ORDER_REQ:
                        tableNo = msg.pop_byte();
                        string order = msg.pop_string();
                        int orderCnt = msg.pop_int32();

                        owner.info.gameInfo.gameCnt += orderCnt;
                        owner.mainFrm.RefreshGameCount(tableNo, owner.info.gameInfo.gameCnt);

                        ++owner.mainFrm.orderID;

                        RequestOrder reqOrder = new RequestOrder((byte)ERequestOrerType.eOrder, owner.mainFrm.orderID, tableNo, order);
                        owner.mainFrm.SetRequestOrder(reqOrder);

                        // Admin Send packet
                        if (Frm.GetAdminUser() != null)
                        {
                            other_msg = CPacket.create((short)PROTOCOL.ORDER_NOT);
                            other_msg.push(reqOrder.type);
                            other_msg.push(reqOrder.id);
                            other_msg.push((byte)reqOrder.tableNo);
                            other_msg.push(reqOrder.packing);

                            Frm.GetAdminUser().send(other_msg);
                        }

                        send_msg = CPacket.create((short)PROTOCOL.ORDER_ACK);
                        send_msg.push(orderCnt);
                        break;
                    case PROTOCOL.CHAT_REQ:
                        tableNo = msg.pop_byte();
                        byte otherTableNo = msg.pop_byte();
                        string chat = msg.pop_string();

                        string tt = DateTime.Now.ToString("tt");
                        string hh = DateTime.Now.ToString("hh");
                        string mm = DateTime.Now.ToString("mm");
                        string makeTime = tt + "/" + hh + "/" + mm;

                        // 상대방 유저에게 채팅 보내기
                        for (int i = 0; i < owner.mainFrm.ListUser.Count; i++)
                        {
                            User other = owner.mainFrm.ListUser[i];
                            if (other.tableNum != otherTableNo)
                                continue;

                            other_msg = CPacket.create((short)PROTOCOL.CHAT_NOT);
                            other_msg.push(tableNo);                            
                            other_msg.push(makeTime);
                            other_msg.push(chat);
                            other.send(other_msg);
                            break;
                        }

                        send_msg = CPacket.create((short)PROTOCOL.CHAT_ACK);
                        send_msg.push(otherTableNo);
                        send_msg.push(makeTime);
                        send_msg.push(chat);

                        break;
                    case PROTOCOL.ORDER_DETAIL_REQ:
                        tableNo = msg.pop_byte();

                        List<SendMenu> listSendMenu = owner.mainFrm.GetOrder((int)tableNo);
                        JsonData listSendMenuJson = JsonMapper.ToJson(listSendMenu);

                        List<short> listDiscount = owner.mainFrm.GetDiscount((int)tableNo);
                        JsonData listDiscountJson = JsonMapper.ToJson(listDiscount);

                        send_msg = CPacket.create((short)PROTOCOL.ORDER_DETAIL_ACK);
                        send_msg.push(listSendMenuJson.ToString());
                        send_msg.push(listDiscountJson.ToString());
                        break;
                    case PROTOCOL.GAME_DISCOUNT_REQ:

                        tableNo = msg.pop_byte();
                        short discount = msg.pop_int16();

                        ++owner.mainFrm.orderID;
                        RequestOrder reqOrderDiscount = new RequestOrder((byte)ERequestOrerType.eDiscount, owner.mainFrm.orderID, tableNo, Convert.ToString(discount));
                        owner.mainFrm.SetRequestOrder(reqOrderDiscount);
                        // Admin Send packet
                        if (Frm.GetAdminUser() != null)
                        {
                            other_msg = CPacket.create((short)PROTOCOL.GAME_DISCOUNT_NOT);

                            other_msg.push(reqOrderDiscount.type);
                            other_msg.push(reqOrderDiscount.id);
                            other_msg.push((byte)reqOrderDiscount.tableNo);
                            other_msg.push(reqOrderDiscount.packing);

                            Frm.GetAdminUser().send(other_msg);
                        }

                        send_msg = CPacket.create((short)PROTOCOL.GAME_DISCOUNT_ACK);

                        break;
                    case PROTOCOL.REQUEST_MUSIC_LIST_REQ:

                        JsonData listRequestMusicJson = JsonMapper.ToJson(owner.mainFrm.listReqMusicInfo);

                        send_msg = CPacket.create((short)PROTOCOL.REQUEST_MUSIC_LIST_ACK);
                        send_msg.push(listRequestMusicJson.ToString());

                        break;
                    case PROTOCOL.REQUEST_MUSIC_REQ:

                        byte reqTableNo = msg.pop_byte();
                        string reqTitle = msg.pop_string();
                        string reqSinger = msg.pop_string();

                        RequestMusicInfo reqMusicInfo = owner.mainFrm.AddRequestMusic(reqTableNo, reqTitle, reqSinger);
                        JsonData reqMusicJson = JsonMapper.ToJson(reqMusicInfo);

                        // 관리자에게 전달
                        if (Frm.GetAdminUser() != null)
                        {
                            other_msg = CPacket.create((short)PROTOCOL.REQUEST_MUSIC_NOT);
                            other_msg.push(reqMusicJson.ToString());
                            Frm.GetAdminUser().send(other_msg);
                        }

                        send_msg = CPacket.create((short)PROTOCOL.REQUEST_MUSIC_ACK);
                        send_msg.push(owner.mainFrm.musicID);
                        send_msg.push(reqMusicJson.ToString());

                        break;
                    case PROTOCOL.REQUEST_MUSIC_REMOVE_REQ:

                        int removeReqMusicID = msg.pop_int32();
                        owner.mainFrm.RemoveRequestMusicInfo(removeReqMusicID);

                        for (int i = 0; i < owner.mainFrm.ListUser.Count; i++)
                        {
                            User other = owner.mainFrm.ListUser[i];
                            if (other.tableNum == 10000 || other.tableNum <= 0 || other.info == null)
                                continue;

                            other_msg = CPacket.create((short)PROTOCOL.REQUEST_MUSIC_REMOVE_NOT);
                            other_msg.push(removeReqMusicID);
                            other.send(other_msg);
                            break;
                        }

                        send_msg = CPacket.create((short)PROTOCOL.REQUEST_MUSIC_REMOVE_ACK);
                        send_msg.push(removeReqMusicID);

                        break;
                    case PROTOCOL.ORDER_CONFIRM_REQ:
                        byte reqType = msg.pop_byte();
                        int reqOrderId = msg.pop_int32();
                        byte reqOrderTableNo = msg.pop_byte();
                        string reqOrderPacking = msg.pop_string();

                        switch ((ERequestOrerType)reqType)
                        {
                            case ERequestOrerType.eOrder:       owner.mainFrm.SetOrder((int)reqOrderTableNo, reqOrderPacking);      break;
                            case ERequestOrerType.eDiscount:    owner.mainFrm.SetDiscount((int)reqOrderTableNo, reqOrderPacking);   break;
                        }

                        for (int i = 0; i < owner.mainFrm.ListUser.Count; i++)
                        {
                            User other = owner.mainFrm.ListUser[i];
                            if (other.tableNum != reqOrderTableNo)
                                continue;

                            other_msg = CPacket.create((short)PROTOCOL.ORDER_CONFIRM_NOT);
                            other_msg.push(reqType);
                            other.send(other_msg);
                            break;
                        }

                        owner.mainFrm.RemoveRequestOrder(reqOrderId);

                        send_msg = CPacket.create((short)PROTOCOL.ORDER_CONFIRM_ACK);
                        send_msg.push(reqOrderId);
                        break;
                    case PROTOCOL.TABLE_ORDER_CONFIRM_REQ:
                        tableNo = msg.pop_byte();
                        List<SendMenu> listTableOrder = owner.mainFrm.GetOrder((int)tableNo);
                        JsonData tableOrderJson = JsonMapper.ToJson(listTableOrder);

                        List<short> listTableDiscount = owner.mainFrm.GetDiscount((int)tableNo);
                        JsonData tableDiscountJson = JsonMapper.ToJson(listTableDiscount);

                        send_msg = CPacket.create((short)PROTOCOL.TABLE_ORDER_CONFIRM_ACK);
                        send_msg.push(tableNo);
                        send_msg.push(tableOrderJson.ToString());
                        send_msg.push(tableDiscountJson.ToString());
                        break;
                    case PROTOCOL.TABLE_ORDER_INPUT_REQ:
                        tableNo = msg.pop_byte();
                        string inputTableOrderPacking = msg.pop_string();
                        int tableInputOrderCnt = msg.pop_int32();

                        owner.info.gameInfo.gameCnt += tableInputOrderCnt;
                        owner.mainFrm.RefreshGameCount(tableNo, owner.info.gameInfo.gameCnt);

                        JsonData inputOrder = JsonMapper.ToObject(inputTableOrderPacking);
                        for (int i = 0; i < inputOrder.Count; i++)
                        {
                            int reqSendMenu = int.Parse(inputOrder[i]["menu"].ToString());
                            int reqSendCnt = int.Parse(inputOrder[i]["cnt"].ToString());

                            owner.mainFrm.SetOrder((int)tableNo, new SendMenu(reqSendMenu, reqSendCnt));
                        }

                        for (int i = 0; i < owner.mainFrm.ListUser.Count; i++)
                        {
                            User inputTargetUser = owner.mainFrm.ListUser[i];
                            if (inputTargetUser.tableNum != (int)tableNo)
                                continue;

                            other_msg = CPacket.create((short)PROTOCOL.TABLE_ORDER_INPUT_NOT);
                            other_msg.push(tableInputOrderCnt);
                            inputTargetUser.send(other_msg);
                            break;
                        }

                        send_msg = CPacket.create((short)PROTOCOL.TABLE_ORDER_INPUT_ACK);
                        break;
                    case PROTOCOL.SLOT_START_REQ:
                        --owner.info.gameInfo.gameCnt;
                        owner.mainFrm.RefreshGameCount(tableNo, owner.info.gameInfo.gameCnt);

                        short ranDiscountIdx = owner.mainFrm.GetRandomDiscountIndex();

                        send_msg = CPacket.create((short)PROTOCOL.SLOT_START_ACK);                        
                        send_msg.push(owner.info.gameInfo.gameCnt);
                        send_msg.push(ranDiscountIdx);
                        break;
                    case PROTOCOL.REPORT_OFFLINE_GAME_REQ:
                        tableNo = msg.pop_byte();
                        byte gameType = msg.pop_byte();
                        byte gameKind = msg.pop_byte();
                        byte gameDiscount = msg.pop_byte();

                        ++owner.info.gameInfo.gameID;
                        Unfinish unfinish = new Unfinish(owner.info.gameInfo.gameID, gameType, gameKind, gameDiscount);
                        owner.info.gameInfo.listUnfinish.Add(unfinish);
                        owner.mainFrm.SetUnfinishGame(tableNo, unfinish);

                        send_msg = CPacket.create((short)PROTOCOL.REPORT_OFFLINE_GAME_ACK);
                        break;
                    case PROTOCOL.UNFINISH_GAME_LIST_REQ:
                        tableNo = msg.pop_byte();

                        List<Unfinish> listUnfinish = owner.mainFrm.GetUnfinishList(tableNo);
                        JsonData listUnfinishJson = JsonMapper.ToJson(listUnfinish);

                        send_msg = CPacket.create((short)PROTOCOL.UNFINISH_GAME_LIST_ACK);
                        send_msg.push(listUnfinishJson.ToString());
                        send_msg.push(tableNo);
                        break;
                    case PROTOCOL.UNFINISH_GAME_CONFIRM_REQ:
                        tableNo = msg.pop_byte();
                        int id = msg.pop_int32();
                        short sDis = msg.pop_int16();

                        if (sDis > -1)
                            owner.mainFrm.SetDiscount(tableNo, sDis);

                        owner.mainFrm.RemoveUnfinishGame(tableNo, id);

                        for (int i = 0; i < owner.mainFrm.ListUser.Count; i++)
                        {
                            User other = owner.mainFrm.ListUser[i];
                            if (other.tableNum != tableNo)
                                continue;

                            other_msg = CPacket.create((short)PROTOCOL.UNFINISH_GAME_CONFIRM_NOT);
                            other_msg.push(id);
                            other.send(other_msg);
                            break;
                        }

                        send_msg = CPacket.create((short)PROTOCOL.UNFINISH_GAME_CONFIRM_ACK);
                        send_msg.push(id);
                        break;
                    case PROTOCOL.TABLE_DISCOUNT_INPUT_REQ:
                        tableNo = msg.pop_byte();
                        int discount500Cnt = msg.pop_int32();
                        int discount1000Cnt = msg.pop_int32();

                        for (int i = 0; i < discount500Cnt; i++)
                            owner.mainFrm.SetDiscount((int)tableNo, (short)0);

                        for (int i = 0; i < discount1000Cnt; i++)
                            owner.mainFrm.SetDiscount((int)tableNo, (short)1);


                        for (int i = 0; i < owner.mainFrm.ListUser.Count; i++)
                        {
                            User other = owner.mainFrm.ListUser[i];
                            if (other.tableNum != tableNo)
                                continue;

                            other_msg = CPacket.create((short)PROTOCOL.TABLE_DISCOUNT_INPUT_NOT);
                            other.send(other_msg);
                            break;
                        }

                        send_msg = CPacket.create((short)PROTOCOL.TABLE_DISCOUNT_INPUT_ACK);
                        break;
                    default:
                        break;
                }

                send(send_msg);
            }
            catch (Exception e)
            {
                owner.mainFrm.BeginInvoke(owner.mainFrm.WriteLogInstance,
                        new object[] { e.ToString(),
                        stackFrame.GetMethod().Name, stackFrame.GetFileName(),
                        stackFrame.GetFileLineNumber().ToString() });
            }

            //owner.db.Close();
        }

        void send(CPacket msg)
        {
            PROTOCOL protocol = (PROTOCOL)msg.protocol_id;

            StackFrame stackFrame = new StackFrame(1, true);
            owner.mainFrm.BeginInvoke(owner.mainFrm.WriteLogInstance,
                        new object[] { "[table Num : " + owner.tableNum.ToString() + "] protocol id " + protocol,
                        stackFrame.GetMethod().Name, stackFrame.GetFileName(),
                        stackFrame.GetFileLineNumber().ToString() });

            owner.send(msg);
        }


    }
}
