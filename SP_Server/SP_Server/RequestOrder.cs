using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FreeNet;

namespace SP_Server
{
    public enum ERequestOrerType : byte
    {
        eNone = 0,
        eOrder,
        eDiscount,
    }

    [Serializable]
    public class RequestOrder
    {
        public byte type;
        public int id;
        public int tableNo;
        public string packing;

        public RequestOrder()
        {
            this.type = 0;
            this.id = -1;
            this.tableNo = -1;
            this.packing = string.Empty;
        }

        public RequestOrder(byte type, int id, int tableNo, string packing)
        {
            this.type = type;
            this.id = id;
            this.tableNo = tableNo;
            this.packing = packing;
        }
    }
}