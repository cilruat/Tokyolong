using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using LitJson;

public class AdminBillConfirmChange : MonoBehaviour {

    public GameObject prefab;
    public RectTransform rtScroll; 
    public GridLayoutGroup layout;
    public Text totalPrice;

    List<BillChangeElt> listElt = new List<BillChangeElt>();

    byte tableNo = 0;
    public void SetTable(byte tableNo) 
    {
        this.tableNo = tableNo; 
        _Clear();
    }

    public void SetMenu(EMenuDetail eType, int value, int oriValue)
    {
        int findIdx = -1;
        for (int i = 0; i < listElt.Count; i++) {
            if (listElt [i].MenuDetailType () != eType)
                continue;

            findIdx = i;
            break;
        }

        if (findIdx != -1) 
        {
            listElt [findIdx].SetValue (value);
        } 
        else 
        {
            GameObject obj = Instantiate (prefab) as GameObject;
            obj.SetActive (true);

            Transform tr = obj.transform;
            tr.SetParent (rtScroll);
            tr.InitTransform ();

            BillChangeElt elt = obj.GetComponent<BillChangeElt> ();
            elt.SetInfo (eType, oriValue, value);
            listElt.Add (elt);
        }       

        CalcTotalPrice ();
        ResizeScroll();
    }

    public void CalcTotalPrice()
    {
        int total = 0;
        for (int i = 0; i < listElt.Count; i++)
            total += listElt [i].GetPrice ();

        totalPrice.text = Info.MakeMoneyString (total);
    }

    public void RemoveElt(EMenuDetail eType)
    {
        int findIdx = -1;
        for (int i = 0; i < listElt.Count; i++) {
            if (listElt [i].MenuDetailType () != eType)
                continue;

            findIdx = i;
            listElt.RemoveAt (i);
            break;
        }

        if (findIdx == -1)
            return;

        for (int i = 0; i < rtScroll.childCount; i++) {
            if (i != findIdx)
                continue;

            Transform child = rtScroll.GetChild (i);
            if (child)
                Destroy (child.gameObject);
            break;
        }

        CalcTotalPrice ();
        ResizeScroll();
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
        ResizeScroll();
    }

    public void FinishOrder()
    {
        if (tableNo == (byte)0)
        {
            SystemMessage.Instance.Add ("테이블 정보가 잘못되었습니다.");
            return;
        }

        if (listElt.Count == 0) 
        {
            SystemMessage.Instance.Add ("주문내역이 없습니다");
            return;
        }

        if (AdminBillConfirm.Instance.waitComplete)
            return;

        int orderCnt = 0;
        List<SendMenu> list = new List<SendMenu> ();
        for (int i = 0; i < listElt.Count; i++) {
            int menu = (int)listElt [i].MenuDetailType ();
            int cnt = listElt [i].GetCount ();
            SendMenu send = new SendMenu (menu, cnt);
            list.Add (send);

            if (listElt[i].MenuType() != EMenuType.eDrink)
                orderCnt += cnt;
        }

        JsonData json = JsonMapper.ToJson (list);
        NetworkManager.Instance.Table_Order_Input_REQ (this.tableNo, json.ToString (), orderCnt);
        AdminBillConfirm.Instance.waitComplete = true;
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
}
