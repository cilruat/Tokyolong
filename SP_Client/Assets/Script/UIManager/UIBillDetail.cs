using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;

public class UIBillDetail : MonoBehaviour 
{
    public Bill bill;
    public Text textpeopleCnt;
    public Text textTotalPrice;
    public Text textDutchPayPrice;

    int peopleCount = 0;
    int totalPrice = 0;

    public void SetBill(string packingMenu, string packingCnt)
    {
        peopleCount = (int)Info.myInfo.peopleCnt;

        List<KeyValuePair<EMenuDetail, int>> listOrder = new List<KeyValuePair<EMenuDetail, int>>(); 
        JsonData jsonMenu = JsonMapper.ToObject (packingMenu);
        JsonData jsonCnt = JsonMapper.ToObject (packingCnt);

        for (int i = 0; i < jsonMenu.Count; i++) {
            string json1 = jsonMenu[i].ToString();
            string json2 = jsonCnt[i].ToString();

            EMenuDetail eType = (EMenuDetail)int.Parse (json1);
            int cnt = int.Parse (json2);
            listOrder.Add(new KeyValuePair<EMenuDetail, int>(eType, cnt));
        }

        bill.CopyBill(listOrder);

        textpeopleCnt.text = peopleCount.ToString();

        totalPrice = bill.BillTotalPrice;
        textTotalPrice.text = Info.MakeMoneyString (totalPrice);

        int dutchPayPrice = totalPrice / peopleCount;
        textDutchPayPrice.text = Info.MakeMoneyString (dutchPayPrice);
    }

    public void OnDutchPayPerson(bool up)
    {
        if (peopleCount == 1 && up == false)
        {
            SystemMessage.Instance.Add("손님분들.. \n그래도 한 분은 비용을 지불해야 되지 않겠습니까..");
            return;
        }

        peopleCount = Mathf.Max(1, up ? ++peopleCount : --peopleCount);
        textpeopleCnt.text = peopleCount.ToString();

        int dutchPayPrice = totalPrice / peopleCount;
        textDutchPayPrice.text = Info.MakeMoneyString (dutchPayPrice);
    }
}
