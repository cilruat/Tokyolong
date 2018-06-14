using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeNet;

namespace SP_Server.RoomState
{    
    public interface IRoomState
    {
        void on_receive(PROTOCOL protocol, Player owner, CPacket msg);
    }
}
