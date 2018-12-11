using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdminTableDiscountInput : SingletonMonobehaviour<AdminTableDiscountInput>
{
    public Text textTableNo;
    public GameObject eltPrefab;
    public RectTransform rtScroll;
    public GridLayoutGroup layout;
    public InputField inputField;
    public Text textPrice; 
    public Text textlDiscount;
    public Text textCalcPrice;
    public Text textTotalDiscount;

    List<TableDiscountElt> listElt = new List<TableDiscountElt>();

    byte tableNo = 0;
    int oriTablePrice = 0;
    int oriTableDiscount = 0;

    public void SetTable(byte tableNo, int tablePrice, int tableDiscount) 
    {
        this.tableNo = tableNo;
        textTableNo.text = tableNo.ToString() +"번 테이블";
        _Clear();

        oriTablePrice = tablePrice;
        oriTableDiscount = tableDiscount;

        RefreshCalcPrice();
    }

    void _Clear()
    {
        for (int i = 0; i < rtScroll.childCount; i++) 
        {
            Transform child = rtScroll.GetChild (i);
            if (child)
                Destroy (child.gameObject);
        }

        listElt.Clear ();
    }

    public void OnRegister(EDiscount type)
    {
        if (type == EDiscount.eAll)
        {
            bool isAdd = false;
            for (int i = 0; i < listElt.Count; i++)
            {
                if (listElt[i].DiscountType() == type)
                {
                    isAdd = true;
                    break;
                }
            }

            if (isAdd)
            {
                SystemMessage.Instance.Add("원 단위가 아닌 할인 유형은 한번에 한번 가능합니다");
                return;
            }
        }

        GameObject obj = CreateDiscountElt();

        TableDiscountElt elt = obj.GetComponent<TableDiscountElt>();

        int discount = GetDiscount(type);

        elt.SetInfo(type, listElt.Count, discount);
        listElt.Add(elt);

        RefreshCalcPrice();
    }

    public void OnDirectRegister()
    {
        if (inputField.text.Length <= 0)
        {
            SystemMessage.Instance.Add("할인 금액이 입력되지 않았습니다");
            inputField.text = string.Empty;
            return;
        }

        int discount = 0;
        if (int.TryParse(inputField.text, out discount) == false)
        {
            SystemMessage.Instance.Add("할인 금액 입력에 오류가 발생 하였습니다.\n재입력 하여 주십시오");
            inputField.text = string.Empty;
            return;
        }

        inputField.text = string.Empty;

        GameObject obj = CreateDiscountElt();
        TableDiscountElt elt = obj.GetComponent <TableDiscountElt>();
        elt.SetInfo(EDiscount.eDirect, listElt.Count, discount);
        listElt.Add(elt);

        RefreshCalcPrice();
    }

    GameObject CreateDiscountElt()
    {
        GameObject obj = Instantiate(eltPrefab) as GameObject;
        obj.SetActive(true);

        RectTransform rt = obj.GetComponent<RectTransform>();
        rt.SetParent(rtScroll);
        rt.InitTransform();

        return obj;
    }

    public void OnDelete(int idx)
    {
        Destroy(listElt[idx].gameObject);
        listElt.RemoveAt(idx);

        for (int i = idx; i < listElt.Count; i++)
            listElt[i].SetIndex(i);

        RefreshCalcPrice();
    }

    public int GetDiscount(EDiscount type)
    {
        int discount = 0;
        switch (type)
        {
		case EDiscount.e500won:
        case EDiscount.e1000won:
		case EDiscount.e2000won:
        case EDiscount.e5000won:    discount = (Info.GetDiscountPrice(type));		break;
        case EDiscount.eAll:        discount = (oriTablePrice - oriTableDiscount);	break;
        }

        return discount;
    }

    public int GetTotalDiscount()
    {
        int totalDiscount = 0;
        for (int i = 0; i < listElt.Count; i++)
            totalDiscount += listElt[i].Discount();

        textTotalDiscount.text = "- " + Info.MakeMoneyString(totalDiscount);
        totalDiscount = Mathf.Min(oriTablePrice, (oriTableDiscount + totalDiscount));

        return totalDiscount;
    }

    public void RefreshCalcPrice()
    {
        textPrice.text = Info.MakeMoneyString (oriTablePrice);

        int discount = GetTotalDiscount ();
        textlDiscount.text = "- " + Info.MakeMoneyString(discount);
        textCalcPrice.text = Info.MakeMoneyString (oriTablePrice - discount);

        ResizeScroll();
    }

    void ResizeScroll()
    {
        float height = layout.cellSize.y * (float)listElt.Count;
        height += layout.spacing.y * (float)Mathf.Max((listElt.Count - 1), 0);
        height += layout.padding.top + layout.padding.bottom;

        if (height != rtScroll.rect.height)
        {
            rtScroll.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
            float parentHeight = rtScroll.parent.GetComponent<RectTransform>().rect.height;
            rtScroll.anchoredPosition = new Vector2(0f, Mathf.Max((height - parentHeight), 0f));
        }
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