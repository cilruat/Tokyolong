using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class RequestCashInfo
{
    public int id;
    public int tableNo;
    public string title;

    public RequestCashInfo()
    {
        this.id = -1;
        this.tableNo = -1;
        this.title = string.Empty;
    }

    public RequestCashInfo(int id, int tableNo, string title)
    {
        this.id = id;
        this.tableNo = tableNo;
        this.title = title;
    }
}
