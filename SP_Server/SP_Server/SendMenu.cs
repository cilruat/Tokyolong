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
        eDirect,
    }

    [Serializable]
    public class SendMenu
    {
        public int menu = -1;
        public int cnt = -1;

        public SendMenu()
        {
            this.menu = -1;
            this.cnt = -1;
        }

        public SendMenu(int menu, int cnt)
        {
            this.menu = menu;
            this.cnt = cnt;
        }
    }
}