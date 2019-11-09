﻿using System.Collections;
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

public class UIRequestMusic : MonoBehaviour 
{
    public ScrollRect srBoard;
    public MusicElt elt;

    public MusicRequestor requestor;

    List<MusicElt> elts = new List<MusicElt>();

    void Awake()
    {
        if (elt.gameObject.activeSelf)
            elt.gameObject.SetActive(false);

        if (requestor.gameObject.activeSelf)
            requestor.gameObject.SetActive(false);
    }

    void OnEanble()
    {
        if (requestor.gameObject.activeSelf)
            requestor.gameObject.SetActive(false);
    }

    public void SetAddMusicList(string packing)
    {
        _Clear();

        JsonData json = JsonMapper.ToObject (packing);
        for (int i = 0; i < json.Count; i++)
        {
            int id = int.Parse(json[i]["id"].ToString());
            byte tableNo = byte.Parse(json[i]["tableNo"].ToString());
            string title = json[i]["title"].ToString();
			string singer = json[i]["singer"].ToString();

			MusicElt elt = CreateMusicElt ();
            elt.SetInfo (i+1, new RequestMusicInfo(id, tableNo, title, singer));
            elts.Add(elt);
		}
    }

    public void SetAddMusic(string packing)
    {
        MusicElt elt = CreateMusicElt();
        elts.Add(elt);
        elt.SetInfo(elts.Count, packing);
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

    public void RemoveRequestMusic(int removeID)
    {
        int removeIdx = -1;
        for (int i = 0; i < elts.Count; i++)
        {
            if (removeID != elts[i].GetID())
                continue;

            removeIdx = i;
            break;
        }

        if (removeIdx == -1)
            return;

        Destroy(elts[removeIdx].gameObject);
        elts.RemoveAt(removeIdx);

        for (int i = removeIdx; i < elts.Count; i++)
            elts[i].SetPriority(i+1);
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
        requestor.gameObject.SetActive(true);
    }
}
