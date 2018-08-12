
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeNet;

namespace SP_Server.RoomState
{    
    class GameRoomPlayState : IState
    {
        GameRoom room;       

        public GameRoomPlayState(GameRoom room)
        {
            this.room = room;            
        }

        void IState.on_enter()
        {
        }

        void IState.on_exit()
        {
        }
    }
}
