using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;

public class MusicElt : MonoBehaviour 
{
    public Text priority;
	public Text title;
	public Text singer;
	public Text table;

	int id = -1;

	public void SetInfo(int id, RequestMusicInfo info)
	{
		this.id = id;
		if (priority != null)
			priority.text = string.Format("{00}", id.ToString());
		
		table.text = string.Format("{00}", info.tableNo.ToString());
		title.text = info.title;
		singer.text = info.singer;
	}

	public void SetInfo(int id, string packing)
	{
		this.id = id;

        if (priority != null)
            priority.text = string.Format("{00}", id.ToString());

		JsonData reqMusicJson = JsonMapper.ToObject(packing);
		byte reqTableNo = byte.Parse(reqMusicJson["tableNo"].ToString());
		string reqMusicTitle = reqMusicJson["title"].ToString();
		string reqMusicSinger = reqMusicJson["singer"].ToString();

		table.text = string.Format("{00}", reqTableNo.ToString());
		title.text = reqMusicTitle;
		singer.text = reqMusicSinger;
	}

	public void OnDelete()
	{
		PageAdmin.Instance.RemoveElt (false, id);
	}

	public int GetID() { return id; }
}
