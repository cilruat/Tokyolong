using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using FreeNet;

namespace SP_Server
{
    using UserState;

    public class UserInfo
    {
        public int tableNum;
        public byte peopleCnt;
        public byte customerType;

        public UserInfo()
        {
            this.tableNum = -1;
            this.peopleCnt = 0;
            this.customerType = 0;
        }

        public UserInfo(byte tableNum, byte peopleCnt, byte customerType)
        {
            this.tableNum = tableNum;
            this.peopleCnt = peopleCnt;
            this.customerType = customerType;
        }
    }

    public class User : IPeer
    {
        IUserState current_user_state;
        
        CUserToken token;                
        Dictionary<USER_STATE_TYPE, IUserState> user_states;
        
        public bool IsAdmin { get { return tableNum == 10000; } }
        public int tableNum { get { return info.tableNum; } set { info.tableNum = value; } }
        public byte peopleCnt { get { return info.peopleCnt; } set { info.peopleCnt = value; } }
        public byte customerType { get { return info.customerType; } set { info.customerType = value; } }
        public UserInfo info = new UserInfo();

        public GameRoom battle_room { get; private set; }
        public Player player { get; private set; }

        public Frm mainFrm { get; private set; }        

        public User(CUserToken token, Frm frm)
        {            
            this.tableNum = -1;

            this.token = token;
            this.token.set_peer(this);

            this.mainFrm = frm;

            this.user_states = new Dictionary<USER_STATE_TYPE, IUserState>();
            this.user_states.Add(USER_STATE_TYPE.LOBBY, new UserLobbyState(this));
            this.user_states.Add(USER_STATE_TYPE.PLAY, new UserPlayState(this));
            change_state(USER_STATE_TYPE.LOBBY);
        }

        public void Init()
        {
            this.info = new UserInfo();
        }

        public void change_state(USER_STATE_TYPE state)
        {
            this.current_user_state = this.user_states[state];
        }

        void IPeer.disconnect()
        {
            this.token.ban();
        }

        void IPeer.on_message(CPacket msg)
        {
            this.current_user_state.on_message(msg);
        }

        void IPeer.on_removed()
        {
            StackFrame stackFrame = new StackFrame(1, true);

            try
            {
                this.mainFrm.BeginInvoke(this.mainFrm.WriteLogInstance,
                    new object[] { "[table Num : " + tableNum.ToString() + "] client Disconnected!!",
                        stackFrame.GetMethod().Name, stackFrame.GetFileName(),
                        stackFrame.GetFileLineNumber().ToString() });               
            }
            catch (System.Exception e)
            {
                this.mainFrm.BeginInvoke(this.mainFrm.WriteLogInstance,
                    new object[] { "TCPServer Log Error : " + e.ToString(),
                        stackFrame.GetMethod().Name, stackFrame.GetFileName(),
                        stackFrame.GetFileLineNumber().ToString() });                
            }                        

            // Admin Send packet
            if(IsAdmin == false)
            {
                CPacket admin_msg = CPacket.create((short)PROTOCOL.LOGOUT_ACK);
                admin_msg.push((byte)tableNum);
                Frm.GetAdminUser().send(admin_msg);
            }

            mainFrm.RemoveUserData(this.tableNum);
            mainFrm.remove_user(this);

            if (this.battle_room != null)
                this.battle_room.on_player_removed(this.player);
        }

        public void send(CPacket msg)
        {
            msg.record_size();

            // 소켓 버퍼로 보내기 전에 복사해 놓음.
            byte[] clone = new byte[msg.position];
            Array.Copy(msg.buffer, clone, msg.position);

            this.token.send(new ArraySegment<byte>(clone, 0, msg.position));
        }
    }
}
