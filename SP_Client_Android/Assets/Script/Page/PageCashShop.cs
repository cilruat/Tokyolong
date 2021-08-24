using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageCashShop : SingletonMonobehaviour<PageCashShop>
{


    public GameObject objBoard;

    public Text txtPlayCnt;
    public Text txtTableNo;


    public RectTransform rtScrollCash;
    public GameObject prefabCash;
    List<CashElt> listCashelt = new List<CashElt>();

    public GameObject objPanelDontDestroy;


    public GameObject objNocoinPanel;
    public Animator NocoinAnim;



    public void Start()
    {
        txtPlayCnt.text = Info.GamePlayCnt.ToString();
        objNocoinPanel.SetActive(false);
    }


    private void Awake()
    {
        for (int i = 0; i < Info.myInfo.listCashInfo.Count; i++)
            CreateCashElt(Info.myInfo.listCashInfo[i]);

        txtTableNo.text = Info.TableNum.ToString();

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


    public void RefreshGamePlayChance()
    {
        txtPlayCnt.text = Info.GamePlayCnt.ToString();
    }

    public void NoCoin()
    {
        objNocoinPanel.SetActive(true);
        NocoinAnim.Play("Show");
    }

    public void ReturnHome()
    {
        SceneChanger.LoadScene("Main", objBoard);
    }

    public void OnGoMail()
    {
        SceneChanger.LoadScene("Mail", objBoard);

    }

    public void OnGoOrder()
    {
        SceneChanger.LoadScene("Order", objBoard);
    }

    public void CloseNoCoinPanel()
    {
        objNocoinPanel.SetActive(false);
    }

}
