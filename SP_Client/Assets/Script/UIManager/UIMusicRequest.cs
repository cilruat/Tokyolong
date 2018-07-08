using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;

public class RequestMusicInfo
{
	public byte tableNo;
	public string title;
	public string singer;

	public RequestMusicInfo()
	{
		this.tableNo = 0;
		this.title = string.Empty;
		this.singer = string.Empty;
	}

	public RequestMusicInfo(byte tableNo, string title, string singer)
	{
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

    void Awake()
    {
        if (elt.gameObject.activeSelf)
            elt.gameObject.SetActive(true);
    }

    public void SetMusicList(string packing)
    {
        JsonData json = JsonMapper.ToObject (packing);
        for (int i = 0; i < json.Count; i++)
        {
            string json1 = json[i]["tableNo"].ToString();
            string json2 = json[i]["title"].ToString();
			string json3 = json[i]["singer"].ToString();

			byte tableNo = byte.Parse (json1);

			MusicElt elt = CreateMusicElt ();
			elt.SetInfo (i+1, new RequestMusicInfo(tableNo, json2, json3));
		}
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

    public void OnSelectRequest()
    {
        objRequest.SetActive(true);


    }
}
