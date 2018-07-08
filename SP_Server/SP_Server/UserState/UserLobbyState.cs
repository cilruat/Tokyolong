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
                            other_msg = CPacket.create((short)PROTOCOL.LOGIN_NOT);
                            other_msg.push(tableNum);                            
                            Frm.GetAdminUser().send(other_msg);
                        }                            

                        owner.tableNum = tableNum;                        

                        send_msg = CPacket.create((short)PROTOCOL.LOGIN_ACK);
                        send_msg.push(pop_string);    

                        break;
                    case PROTOCOL.LOGOUT_REQ:
                        tableNo = msg.pop_byte();

                        // 해당 테이블 로그인 화면으로 보내기
                        for (int i = 0; i < owner.mainFrm.ListUser.Count; i++)
                        {
                            User user = owner.mainFrm.ListUser[i];
                            if (user.tableNum != tableNo)
                                continue;

                            user.ClearOrder();

                            other_msg = CPacket.create((short)PROTOCOL.LOGOUT_NOT);
                            other_msg.push(tableNo);
                            user.send(other_msg);
                            break;
                        }

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
                            if (user.tableNum == 10000 || user.tableNum <= 0 ||user.info == null)
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
                        other_msg = CPacket.create((short)PROTOCOL.WAITER_CALL_NOT);
                        other_msg.push(tableNo);                        
                        Frm.GetAdminUser().send(other_msg);

                        send_msg = CPacket.create((short)PROTOCOL.WAITER_CALL_ACK);                        
                        break;
                    case PROTOCOL.ORDER_REQ:
                        tableNo = msg.pop_byte();
                        string order = msg.pop_string();

                        // 주문 정보에 입력
                        JsonData json = JsonMapper.ToObject(order);
                        for (int i = 0; i < json.Count; i++)
                        {
                            string json1 = json[i]["menu"].ToString();
                            string json2 = json[i]["cnt"].ToString();

                            int menu = int.Parse(json1);
                            int cnt = int.Parse(json2);

                            owner.SetOrder(menu, cnt);
                        }                        

                        // Admin Send packet
                        other_msg = CPacket.create((short)PROTOCOL.ORDER_NOT);
                        other_msg.push(tableNo);
                        other_msg.push(order);
                        Frm.GetAdminUser().send(other_msg);

                        send_msg = CPacket.create((short)PROTOCOL.ORDER_ACK);                        
                        break;
                    case PROTOCOL.REQUEST_MUSIC_REQ:
                        string requestMusicPacking = msg.pop_string();

                        JsonData reqMusicJson = JsonMapper.ToObject(requestMusicPacking);
                        byte reqTableNo = byte.Parse(reqMusicJson["tableNo"].ToString());
                        string reqMusicTitle = reqMusicJson["title"].ToString();
                        string reqMusicSinger = reqMusicJson["singer"].ToString();
                        
                        owner.mainFrm.listRequestMusic.Add(new RequestMusic(reqTableNo, reqMusicTitle, reqMusicSinger));

                        other_msg = CPacket.create((short)PROTOCOL.REQUEST_MUSIC_NOT);
                        other_msg.push(requestMusicPacking);
                        Frm.GetAdminUser().send(other_msg);

                        send_msg = CPacket.create((short)PROTOCOL.REQUEST_MUSIC_ACK);
                        send_msg.push(requestMusicPacking);

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
                        send_msg = CPacket.create((short)PROTOCOL.ORDER_DETAIL_ACK);
                        List<int> menus = new List<int>(owner.orderTable.Keys);
                        List<int> cnts = new List<int>(owner.orderTable.Values);

                        JsonData orderMenus = JsonMapper.ToJson(menus);
                        JsonData orderCnt = JsonMapper.ToJson(cnts);

                        send_msg.push(orderMenus.ToString());
                        send_msg.push(orderCnt.ToString());
                        break;
                    case PROTOCOL.GAME_DISCOUNT_REQ:
                        tableNo = msg.pop_byte();
                        short discount = msg.pop_int16();

                        // Admin Send packet
                        other_msg = CPacket.create((short)PROTOCOL.GAME_DISCOUNT_NOT);
                        other_msg.push(tableNo);
                        other_msg.push(discount);
                        Frm.GetAdminUser().send(other_msg);

                        send_msg = CPacket.create((short)PROTOCOL.GAME_DISCOUNT_ACK);
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
