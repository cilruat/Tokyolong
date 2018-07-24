using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FreeNet;

namespace SP_Server
{
    [Serializable]
    public class Unfinish
    {
        public int id = -1;
        public byte type = 0;
        public byte kind = 0;
        public byte discount = 0;

        public Unfinish(int id, byte type, byte kind, byte discount)
        {
            this.id = id;
            this.type = type;
            this.kind = kind;
            this.discount = discount;
        }
    }

    [Serializable]
    public class GameInfo
    {
        public int gameID = -1;
        public int gameCnt = 0;
        public List<Unfinish> listUnfinish = new List<Unfinish>();
    }
}
