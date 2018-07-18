using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FreeNet;

namespace SP_Server
{
    public class Unfinish
    {
        int id = -1;
        byte type = 0;
        byte kind = 0;
        byte discount = 0;

        public Unfinish(int id, byte type, byte kind, byte discount)
        {
            this.id = id;
            this.type = type;
            this.kind = kind;
            this.discount = discount;
        }
    }

    public class GameInfo
    {
        public byte gameCnt = 0;
        public List<Unfinish> listUnfinish = new List<Unfinish>();
    }
}
