using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdminTableDiscountInput : SingletonMonobehaviour<AdminTableDiscountInput>
{
    public Text textTableNo;
    public GameObject prefab;
    public RectTransform rtScroll; 
    public Text textPrice; 
    public Text textlDiscount;
    public Text textCalcPrice;

    byte tableNo = 0;
    List<TableDiscountElt> listElt = new List<TableDiscountElt>();

    int oriTablePrice = 0;
    int oriTableDiscount = 0;

    public void SetTable(byte tableNo, int tablePrice, int tableDiscount) 
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

        oriTablePrice = tablePrice;
        oriTableDiscount = tableDiscount;

        RefreshCalcPrice();
    }

    void _Clear()
    {
        for (int i = 0; i < rtScroll.childCount; i++) {
            Transform child = rtScroll.GetChild (i);
            if (child)
                Destroy (child.gameObject);
        }

        listElt.Clear ();
    }

    public int GetTotalDiscount()
    {
        int totalDiscount = 0;
        int discount = 0;
        int wonDiscount = 0;
        for (int i = 0; i < listElt.Count; i++)
        {
            EDiscount type = listElt[i].DiscountType();
            int cnt = listElt[i].GetCount();
            switch (type)
            {
                case EDiscount.e1000won:
                case EDiscount.e5000won:    wonDiscount += (Info.GetDiscountPrice(type) * cnt);         break;
                case EDiscount.eHalf:       discount += ((oriTablePrice - oriTableDiscount) / 2) * cnt; break;
                case EDiscount.eAll:        discount += (oriTablePrice - oriTableDiscount) * cnt;       break;
            }
        }

        totalDiscount = Mathf.Min(oriTablePrice, (oriTableDiscount + discount + wonDiscount));
        return totalDiscount;
    }

    public void RefreshCalcPrice()
    {
        textPrice.text = Info.MakeMoneyString (oriTablePrice);

        int discount = GetTotalDiscount ();
        textlDiscount.text = Info.MakeMoneyString (discount);
        textCalcPrice.text = Info.MakeMoneyString (oriTablePrice - discount);
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

        NetworkManager.Instance.TableDiscountInput_REQ(this.tableNo, GetTotalDiscount() - oriTableDiscount);
    }
}