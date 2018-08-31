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

    [Serializable]
    public class UserInfo
    {
        public int tableNum;
        public byte peopleCnt;
        public byte customerType;
        public GameInfo gameInfo;
        public List<SendMenu> menus;
        public int discount;
        public int couponCnt;
        public int tokyoLiveCnt;

        public UserInfo()
        {
            this.tableNum = -1;
            this.peopleCnt = 1;
            this.customerType = 0;
            this.gameInfo = new GameInfo();
            this.menus = new List<SendMenu>();
            this.discount = 0;
            this.couponCnt = 0;
            this.tokyoLiveCnt = 0;
        }

        public UserInfo(int tableNum)
        {
            this.tableNum = tableNum;
            this.peopleCnt = 1;
            this.customerType = 0;
            this.gameInfo = new GameInfo();
            this.menus = new List<SendMenu>();
            this.discount = 0;
            this.couponCnt = 0;
            this.tokyoLiveCnt = 0;
        }

        public UserInfo(int tableNum, byte peopleCnt, byte customerType)
        {
            this.tableNum = tableNum;
            this.peopleCnt = peopleCnt;
            this.customerType = customerType;
            this.gameInfo = new GameInfo();
            this.menus = new List<SendMenu>();
            this.discount = 0;
            this.couponCnt = 0;
            this.tokyoLiveCnt = 0;
        }

        public void SetDiscount(short sType)
        {
            int oriTotalPrice = 0;
            for (int i = 0; i < menus.Count; i++)
                oriTotalPrice += menus[i].cnt * MenuData.GetMenuPrice(menus[i].menu);

            EDiscount type = (EDiscount)sType;
            int calcDiscount = 0;

            switch (type)
            {
                case EDiscount.e1000won: calcDiscount = 1000;   break;
                case EDiscount.e5000won: calcDiscount = 5000;   break;
                case EDiscount.eHalf:
                case EDiscount.eAll:

                    int price = oriTotalPrice - discount;
                    calcDiscount = type == EDiscount.eAll ? price : (int)(price * .5f);
                    calcDiscount = Math.Min(price, calcDiscount);
                    break;
            }

            discount = Math.Min(oriTotalPrice, discount + calcDiscount);
        }

        public void SetDiscount(int inputDiscount)
        {
            int oriTotalPrice = 0;
            for (int i = 0; i < menus.Count; i++)
                oriTotalPrice += menus[i].cnt * MenuData.GetMenuPrice(menus[i].menu);

            discount = Math.Min(oriTotalPrice, discount + inputDiscount);
        }

            public bool IsAdmin()
        {
            return tableNum == 10000;
        }
    }

    public class User : IPeer
    {
        IUserState current_user_state;
        
        CUserToken token;                
        Dictionary<USER_STATE_TYPE, IUserState> user_states;

        public readonly DB db = null;

        public bool IsAdmin { get { return tableNum == 10000; } }
        public int tableNum { get { return info.tableNum; } set { info.tableNum = value; } }
        public UserInfo info = new UserInfo();

        public GameRoom battle_room { get; private set; }
        public Player player { get; private set; }

        public Frm mainFrm { get; private set; }        

        public User(CUserToken token, Frm frm)
        {            
            this.tableNum = -1;

            this.token = token;
            this.token.set_peer(this);

            // this.db = new DB(this);

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

                if (Frm.GetAdminUser() != null)
                    Frm.GetAdminUser().send(admin_msg);
            }

            CPacket msg = null;
            for (int i = 0; i < mainFrm.ListUser.Count; i++)
            {
                User user = mainFrm.ListUser[i];
                if (user.IsAdmin)
                    continue;

                msg = CPacket.create((short)PROTOCOL.LOGOUT_NOT);
                msg.push((byte)tableNum);
                user.send(msg);
            }

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
