using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;

public class MusicElt : MonoBehaviour 
{
    public Text textPriority;
	public Text title;
	public Text singer;
	public Text table;

    RequestMusicInfo info = null;
    public int GetID() { return info.id; }

    public void SetInfo(string packing)
    {
        JsonData reqMusicJson = JsonMapper.ToObject(packing);
        int reqID = int.Parse(reqMusicJson["id"].ToString());
        byte reqTableNo = byte.Parse(reqMusicJson["tableNo"].ToString());
        string reqMusicTitle = reqMusicJson["title"].ToString();
        string reqMusicSinger = reqMusicJson["singer"].ToString();

        SetInfo(new RequestMusicInfo(reqID, reqTableNo, reqMusicTitle, reqMusicSinger));
    }

    public void SetInfo(RequestMusicInfo info)
    {
        this.info = info;
        table.text = string.Format("{0:D2}", info.tableNo);
        title.text = info.title;
        singer.text = info.singer;
    }

    public void SetInfo(int priority, string packing)
    {
        JsonData reqMusicJson = JsonMapper.ToObject(packing);
        int reqID = int.Parse(reqMusicJson["id"].ToString());
        byte reqTableNo = byte.Parse(reqMusicJson["tableNo"].ToString());
        string reqMusicTitle = reqMusicJson["title"].ToString();
        string reqMusicSinger = reqMusicJson["singer"].ToString();

        SetInfo(priority, new RequestMusicInfo(reqID, reqTableNo, reqMusicTitle, reqMusicSinger));
    }

    public void SetInfo(int priority, RequestMusicInfo info)
	{
        this.info = info;

        if (textPriority != null)
            textPriority.text = string.Format("{0:D2}", priority);
		
        table.text = string.Format("{0:D2}", info.tableNo);
		title.text = info.title;
		singer.text = info.singer;
	}

    public void SetPriority(int priority)
    {
        if (textPriority == null)
            return;

        textPriority.text = string.Format("{0:D2}", priority);
    }

	public void OnDelete()
	{
        NetworkManager.Instance.Request_Music_Remove_REQ(info.id);
	}
}
