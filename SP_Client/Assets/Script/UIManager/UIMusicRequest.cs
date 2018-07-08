using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;

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
            string json1 = json[i]["menu"].ToString();
            string json2 = json[i]["cnt"].ToString();
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
