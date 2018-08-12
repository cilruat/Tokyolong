using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FreeNet;

namespace SP_Server
{
    public enum EDiscount
    {
        e1000won,
        e5000won,
        eHalf,
        eAll,
    }

    [Serializable]
    public class SendMenu
    {
        public int menu = -1;
        public int cnt = -1;
        public int halfCnt = -1;
        public int allCnt = -1;

        public SendMenu()
        {
            this.menu = -1;
            this.cnt = -1;
            this.halfCnt = -1;
            this.allCnt = -1;
        }

        public SendMenu(int menu, int cnt)
        {
            this.menu = menu;
            this.cnt = cnt;
        }

        public void SetHalfDiscount()
        {
            this.halfCnt = this.cnt;
        }

        public void SetAllDiscount()
        {
            this.allCnt = this.cnt;
            this.halfCnt = -1;
        }
    }
}