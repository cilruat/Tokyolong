using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class UILog : MonoBehaviour {

    public eUI uiType;

    public RectTransform rtScrollMail;
    public GameObject prefabMail;
    public RectTransform rtScrollLike;
    public GameObject prefabLike;
    public RectTransform rtScrollPresent;
    public GameObject prefabPresent;
    public RectTransform rtScrollPlz;
    public GameObject prefabPlz;

    List<MailElt> listMailelt = new List<MailElt>();
    List<LikeElt> listLikeelt = new List<LikeElt>();
    List<PresentElt> listPresentelt = new List<PresentElt>();
    List<PlzElt> listPlzelt = new List<PlzElt>();

    public void SetMail(UserMsgInfo info)
    {
        MailElt elt = CreateMailElt();
        elt.SetInfo(info);
    }

    MailElt CreateMailElt()
    {
        GameObject obj = Instantiate(prefabMail) as GameObject;
        obj.SetActive(true);

        Transform tr = obj.transform;
        tr.SetParent(rtScrollMail);
        tr.InitTransform();

        MailElt elt = obj.GetComponent<MailElt>();
        listMailelt.Add(elt);

        return elt;

    }



    public void SetLike(UserLikeInfo info)
    {
        LikeElt elt = CreateLikeElt();
        elt.SetInfo(info);
    }

    LikeElt CreateLikeElt()
    {
        GameObject obj = Instantiate(prefabLike) as GameObject;
        obj.SetActive(true);

        Transform tr = obj.transform;
        tr.SetParent(rtScrollLike);
        tr.InitTransform();

        LikeElt elt = obj.GetComponent<LikeElt>();
        listLikeelt.Add(elt);

        return elt;

    }

    public void SetPresent(UserPresentInfo info)
    {
        PresentElt elt = CreatePresentElt();
        elt.SetInfo(info);
    }

    PresentElt CreatePresentElt()
    {
        GameObject obj = Instantiate(prefabPresent) as GameObject;
        obj.SetActive(true);

        Transform tr = obj.transform;
        tr.SetParent(rtScrollPresent);
        tr.InitTransform();

        PresentElt elt = obj.GetComponent<PresentElt>();
        listPresentelt.Add(elt);

        return elt;

    }

    public void SetPlz(UserPlzInfo info)
    {
        PlzElt elt = CreatePlzElt();
        elt.SetInfo(info);
    }

    PlzElt CreatePlzElt()
    {
        GameObject obj = Instantiate(prefabPlz) as GameObject;
        obj.SetActive(true);

        Transform tr = obj.transform;
        tr.SetParent(rtScrollPlz);
        tr.InitTransform();

        PlzElt elt = obj.GetComponent<PlzElt>();
        listPlzelt.Add(elt);

        return elt;

    }

    public void OnClose()
    {
        UIManager.Instance.Hide(eUI.eLog);
    }



}
