using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdminTableDiscountInput : SingletonMonobehaviour<AdminTableDiscountInput>
{
    public Text textTableNo;
    public GameObject prefab;
    public RectTransform rtScroll; 
    public Text totalDiscount;

    byte tableNo = 0;
    List<TableDiscountElt> listElt = new List<TableDiscountElt>();

    public void SetTable(byte tableNo) 
    {
        this.tableNo = tableNo;
        textTableNo.text = tableNo.ToString() +"번 테이블";
        _Clear();

        int discountCnt = System.Enum.GetValues(typeof(EDiscount)).Length;
        for (int i = 0; i < discountCnt; i++)
        {
            GameObject obj = Instantiate (prefab) as GameObject;
            obj.SetActive (true);

            Transform tr = obj.transform;
            tr.SetParent (rtScroll);
            tr.InitTransform ();

            TableDiscountElt elt = obj.GetComponent<TableDiscountElt> ();
            elt.SetInfo ((EDiscount)i);
            listElt.Add (elt);
        }
    }

    void _Clear()
    {
        for (int i = 0; i < rtScroll.childCount; i++) {
            Transform child = rtScroll.GetChild (i);
            if (child)
                Destroy (child.gameObject);
        }

        listElt.Clear ();

        CalcTotalPrice ();
    }

    public void CalcTotalPrice()
    {
        int total = 0;
        for (int i = 0; i < listElt.Count; i++)
            total += listElt [i].GetDiscount ();

        totalDiscount.text = Info.MakeMoneyString (total);
    }

    public void OnClose() { gameObject.SetActive (false); }

    public void FinishDiscount()
    {
        if (tableNo == (byte)0)
        {
            SystemMessage.Instance.Add ("테이블 정보가 잘못되었습니다.");
            return;
        }

        if (listElt.Count == 0) 
        {
            SystemMessage.Instance.Add ("입력할 할인 내역이 없습니다");
            return;
        }

        int discount1000Won = 0;
        for (int i = 0; i < listElt.Count; i++)
        {
            EDiscount discountType = listElt[i].DiscountType();

            switch (discountType)
            {
                case EDiscount.e1000won:    discount1000Won += listElt[i].GetCount();   break;
            }
        }

        int price1000 = Info.GetDiscountPrice(EDiscount.e1000won);
        discount1000Won *= price1000;
        NetworkManager.Instance.TableDiscountInput_REQ (this.tableNo, discount1000Won);
    }
}