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
                        int tableNum = 0;
                        string pop_string = msg.pop_string();
                        if (pop_string == "admin")
                        {
                            tableNum = 10000;
                            Frm.SetAdminUser(owner);
                        }
                        else
                        {
                            if (int.TryParse(pop_string, out tableNum) == false)
                            {
                                send_msg = CPacket.create((short)PROTOCOL.FAILED_NOT_NUMBER);
                                break;
                            }

                            tableNum = int.Parse(pop_string);

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
                        send_msg.push(pop_string);
                        if (pop_string == "admin")
                        {
                            List<int> listUserTableNo = new List<int>();

                            for (int i = 0; i < owner.mainFrm.listUserInfo.Count; i++)
                            {
                                UserInfo userInfo = owner.mainFrm.listUserInfo[i];
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
                        owner.peopleCnt = peopleCnt;
                        owner.customerType = customerType;

                        owner.info = new UserInfo((byte)owner.tableNum, peopleCnt, customerType);

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

                        ++owner.gameInfo.gameCnt;
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
                        send_msg.push(owner.gameInfo.gameCnt);
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
                        byte targetTableNo = msg.pop_byte();
                        string inputTableOrderPacking = msg.pop_string();
                        JsonData inputOrder = JsonMapper.ToObject(inputTableOrderPacking);
                        for (int i = 0; i < inputOrder.Count; i++)
                        {
                            int reqSendMenu = int.Parse(inputOrder[i]["menu"].ToString());
                            int reqSendCnt = int.Parse(inputOrder[i]["cnt"].ToString());

                            owner.mainFrm.SetOrder((int)targetTableNo, new SendMenu(reqSendMenu, reqSendCnt));
                        }

                        for (int i = 0; i < owner.mainFrm.ListUser.Count; i++)
                        {
                            User inputTargetUser = owner.mainFrm.ListUser[i];
                            if (inputTargetUser.tableNum != (int)targetTableNo)
                                continue;

                            other_msg = CPacket.create((short)PROTOCOL.TABLE_ORDER_INPUT_NOT);
                            inputTargetUser.send(other_msg);
                            break;
                        }

                        send_msg = CPacket.create((short)PROTOCOL.TABLE_ORDER_INPUT_ACK);
                        break;
                    case PROTOCOL.SLOT_START_REQ:
                        --owner.gameInfo.gameCnt;

                        send_msg = CPacket.create((short)PROTOCOL.SLOT_START_ACK);                        
                        send_msg.push(owner.gameInfo.gameCnt);
                        break;
                    case PROTOCOL.REPORT_OFFLINE_GAME_REQ:
                        tableNo = msg.pop_byte();
                        byte byJackpot = msg.pop_byte();
                        byte gameType = msg.pop_byte();
                        byte gameKind = msg.pop_byte();
                        byte gameDiscount = msg.pop_byte();

                        ++owner.unfinishGameID;
                        Unfinish unfinish = new Unfinish(owner.unfinishGameID, gameType, gameKind, gameDiscount);
                        owner.gameInfo.listUnfinish.Add(unfinish);

                        // Admin Send packet
                        if (byJackpot == 1 && Frm.GetAdminUser() != null)
                        {
                            other_msg = CPacket.create((short)PROTOCOL.REPORT_OFFLINE_GAME_NOT);
                            other_msg.push(tableNo);                            
                            Frm.GetAdminUser().send(other_msg);
                        }

                        send_msg = CPacket.create((short)PROTOCOL.REPORT_OFFLINE_GAME_ACK);
                        break;
                    case PROTOCOL.UNFINISH_GAME_LIST_REQ:
                        JsonData listUnfinishJson = JsonMapper.ToJson(owner.gameInfo.listUnfinish);

                        send_msg = CPacket.create((short)PROTOCOL.UNFINISH_GAME_LIST_ACK);
                        send_msg.push(listUnfinishJson.ToString());
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
