using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;

public class RequestMusicInfo
{
    public int id;
	public byte tableNo;
	public string title;
	public string singer;

	public RequestMusicInfo()
	{
        this.id = -1;
		this.tableNo = 0;
		this.title = string.Empty;
		this.singer = string.Empty;
	}

	public RequestMusicInfo(int id, byte tableNo, string title, string singer)
	{
        this.id = id;
		this.tableNo = tableNo;
		this.title = title;
		this.singer = singer;
	}
}

public class UIMusicRequest : MonoBehaviour 
{
    public ScrollRect srBoard;
    public MusicElt elt;

    public GameObject objRequest;

    List<MusicElt> elts = new List<MusicElt>();

    void Awake()
    {
        if (elt.gameObject.activeSelf)
            elt.gameObject.SetActive(false);
    }

    public void SetAddMusicList(string packing)
    {
        _Clear();

        JsonData json = JsonMapper.ToObject (packing);
        for (int i = 0; i < json.Count; i++)
        {
            int id = int.Parse(json[i]["tableNo"].ToString());
            byte tableNo = byte.Parse(json[i]["tableNo"].ToString());
            string title = json[i]["title"].ToString();
			string singer = json[i]["singer"].ToString();

			MusicElt elt = CreateMusicElt ();
            elt.SetInfo (i+1, new RequestMusicInfo(id, tableNo, title, singer));
            elts.Add(elt);
		}
    }

    public void SetAddMusic(int priority, string packing)
    {
        JsonData json = JsonMapper.ToObject(packing);

        int id = int.Parse(json["tableNo"].ToString());
        byte tableNo = byte.Parse(json["tableNo"].ToString());
        string title = json["title"].ToString();
        string singer = json["singer"].ToString();

        MusicElt elt = CreateMusicElt();
        elt.SetInfo(priority, new RequestMusicInfo(id, tableNo, title, singer));
        elts.Add(elt);
    }

    MusicElt CreateMusicElt()
    {
        GameObject newObj = Instantiate (elt.gameObject) as GameObject;
        newObj.transform.SetParent (srBoard.content);
        newObj.transform.InitTransform ();
        newObj.gameObject.SetActive(true);

        MusicElt newElt = newObj.GetComponent<MusicElt> ();
        if (newElt == null)
            return null;

        return newElt;
    }

    void _Clear()
    {
        if (elts.Count > 0)
        {
            for (int i = elts.Count-1; i >= 0; i--) 
            {
                Destroy (elts[i].gameObject);
                elts.RemoveAt(i);
            }
        }
    }

    public void OnShowRequest()
    {
        objRequest.SetActive(true);
    }
}
