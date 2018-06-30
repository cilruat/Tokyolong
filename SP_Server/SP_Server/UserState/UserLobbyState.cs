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

                        send_msg = CPacket.create((short)PROTOCOL.ENTER_CUSTOMER_ACK);
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
                            if (other.tableNum != tableNo)
                                continue;

                            other_msg = CPacket.create((short)PROTOCOL.CHAT_NOT);
                            other_msg.push(other.customerType);
                            other_msg.push(other.tableNum);
                            other_msg.push(other.peopleCnt);                            
                            other_msg.push(makeTime);
                            other_msg.push(chat);
                            other.send(other_msg);
                            break;
                        }

                        send_msg = CPacket.create((short)PROTOCOL.CHAT_ACK);
                        send_msg.push(makeTime);
                        send_msg.push(chat);

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
