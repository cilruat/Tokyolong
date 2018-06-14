using System;
using System.Collections;
using System.Collections.Generic;
using FreeNet;

namespace SP_Server
{
    public class Player
    {
        public delegate void SendFn(CPacket msg);

        IPeer owner;

        public byte player_index { get; private set; }        

        public Player(User user, byte player_index)
        {
            this.owner = user;
            this.player_index = player_index;            
        }        

        public void send(CPacket msg)
        {
            this.owner.send(msg);
        }        

        public void removed()
        {
            ((User)this.owner).change_state(UserState.USER_STATE_TYPE.LOBBY);
        }

        public void disconnect()
        {
            this.owner.disconnect();
        }
    }
}
