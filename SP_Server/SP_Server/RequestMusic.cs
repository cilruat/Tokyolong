using System;
using System.Collections;
using System.Linq;

public class RequestMusicInfo
{
    public int id;
    public int tableNo;
    public string title;
    public string singer;

    public RequestMusicInfo()
    {
        this.id = -1;
        this.tableNo = -1;
        this.title = string.Empty;
        this.singer = string.Empty;
    }

    public RequestMusicInfo(int id, int tableNo, string title, string singer)
    {
        this.id = id;
        this.tableNo = tableNo;
        this.title = title;
        this.singer = singer;
    }
}
