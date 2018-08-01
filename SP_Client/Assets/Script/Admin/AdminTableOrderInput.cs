using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdminTableOrderInput : SingletonMonobehaviour<AdminTableOrderInput>
{
    public ScrollRect srMenu;
    public MenuElt menuElt;

    public ScrollRect srMenuDetail;
    public MenuDetailElt menuDetailElt;

    public AdminTableOrderBill tableOrderBill;
    public GameObject objComplete;

    Dictionary<EMenuType, MenuElt> dictMenuElt = new Dictionary<EMenuType, MenuElt>();
    Dictionary<EMenuType, List<MenuDetailElt>> dictMenuDetailElt = new Dictionary<EMenuType, List<MenuDetailElt>>();

    EMenuType prev = EMenuType.eNone;

    byte tableNo = 0;

    void Awake() { LoadMenu(); }
    void LoadMenu()
    {
        foreach (KeyValuePair<int, List<MenuData>> pair in MenuData.dictMainMenu)
        {
            EMenuType type = (EMenuType)pair.Key;
            MenuElt newMenuElt = CreateMenuElt();
            newMenuElt.SetMenuElt(type);

            dictMenuElt.Add(type, newMenuElt);
            dictMenuDetailElt.Add(type, new List<MenuDetailElt>());
            for (int i = 0; i < pair.Value.Count; i++)
            {
                MenuDetailElt newMenuDetailElt = CreateMenuDetailElt();
                newMenuDetailElt.SetMenuElt(pair.Value[i]);
                dictMenuDetailElt[type].Add(newMenuDetailElt);
            }
        }
    }

    MenuElt CreateMenuElt()
    {
        GameObject newObj = Instantiate (menuElt.gameObject) as GameObject;
        newObj.transform.SetParent (srMenu.content);
        newObj.transform.InitTransform ();
        newObj.gameObject.SetActive(true);

        MenuElt newElt = newObj.GetComponent<MenuElt> ();
        if (newElt == null)
            return null;

        return newElt;
    }

    MenuDetailElt CreateMenuDetailElt()
    {
        GameObject newObj = Instantiate (menuDetailElt.gameObject) as GameObject;
        newObj.transform.SetParent (srMenuDetail.content);
        newObj.transform.InitTransform ();
        newObj.gameObject.SetActive(false);

        MenuDetailElt newElt = newObj.GetComponent<MenuDetailElt> ();
        if (newElt == null)
            return null;

        return newElt;
    }

    public void SetTable(byte tableNo)
    {
        this.tableNo = tableNo;
        tableOrderBill.SetTable(this.tableNo);
        OnSelectMenuElt(EMenuType.eMeal);
    }

    public void OnSelectMenuElt(EMenuType type)
    {
        if (dictMenuElt.ContainsKey(type) == false)
            return;

        if (prev != EMenuType.eNone)
        {
            dictMenuElt[prev].OnSelected(false);
            SetActiveMenuDetailElt(prev, false);
        }

        dictMenuElt[type].OnSelected(true);
        SetActiveMenuDetailElt(type, true);

        prev = type;
    }

    void SetActiveMenuDetailElt(EMenuType type, bool isActive)
    {
        List<MenuDetailElt> list = dictMenuDetailElt[type];
        for (int i = 0; i < list.Count; i++)
            list[i].gameObject.SetActive(isActive);
    }

    public void OnSelectMenuDetailElt(EMenuDetail type) { tableOrderBill.SetMenu(type); }

    [System.NonSerialized]public bool waitComplete = false;
    public void OnClose() 
    { 
        if (waitComplete)
            return;

        gameObject.SetActive (false); 
    }

    public void OnCompleteTableOrderInput()
    {
        UITweenAlpha.Start (objComplete.gameObject, 0f, 1f, TWParam.New (.4f).Curve (TWCurve.CurveLevel2));
        UITweenScale.Start (objComplete.gameObject, 1.2f, 1f, TWParam.New (.3f).Curve (TWCurve.Bounce));
        StartCoroutine(_DelayComplete());
    }

    IEnumerator _DelayComplete()
    {
        yield return new WaitForSeconds (1f);

        waitComplete = false;
        objComplete.SetActive(false);
        OnClose();
    }
}