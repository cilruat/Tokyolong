using System;
using System.Collections;
using System.Linq;

public class RequestMusic
{
    public int tableNo;
    public string title;
    public string singer;

    public RequestMusic()
    {
        this.tableNo = -1;
        this.title = string.Empty;
        this.singer = string.Empty;
    }

    public RequestMusic(int tableNo, string title, string singer)
    {
        this.tableNo = tableNo;
        this.title = title;
        this.singer = singer;
    }
}
