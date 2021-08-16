using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageCashShop : SingletonMonobehaviour<PageCashShop>
{


    public GameObject objBoard;
    public Text txtPlayCnt;

    public RectTransform rtScrollCash;
    public GameObject prefabCash;
    List<CashElt> listCashelt = new List<CashElt>();

    public GameObject objPanelDontDestroy;



    private void Awake()
    {
        for (int i = 0; i < Info.myInfo.listCashInfo.Count; i++)
            CreateCashElt(Info.myInfo.listCashInfo[i]);
    }

    public void SetCash(UserCashInfo info)
    {
        CreateCashElt(info);

    }

    void CreateCashElt(UserCashInfo info)
    {
        GameObject obj = Instantiate(prefabCash) as GameObject;
        obj.SetActive(true);

        Transform tr = obj.transform;
        tr.SetParent(rtScrollCash);
        tr.InitTransform();

        CashElt elt = obj.GetComponent<CashElt>();
        elt.SetInfo(info);

        listCashelt.Add(elt);

    }

    public void DeleteCashElt(CashElt elt)
    {
        listCashelt.Remove(elt);
        Destroy(elt.gameObject);
    }



    public void ReturnHome()
    {
        SceneChanger.LoadScene("Main", objBoard);

    }


}
