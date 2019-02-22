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
    public Text textExtraGameCnt;

    int peopleCount = 0;
    int totalPrice = 0;

	public void SetBill(string meuPacking, int discountPrice)
    {
        peopleCount = (int)Info.myInfo.peopleCnt;

        List<KeyValuePair<EMenuDetail, int>> listOrder = new List<KeyValuePair<EMenuDetail, int>>(); 
        JsonData json = JsonMapper.ToObject (meuPacking);

        for (int i = 0; i < json.Count; i++) 
        {
            int menu = int.Parse(json[i]["menu"].ToString());
            int cnt = int.Parse(json[i]["cnt"].ToString());

            EMenuDetail eType = (EMenuDetail)menu;
            listOrder.Add(new KeyValuePair<EMenuDetail, int>(eType, cnt));
        }

		bill.CopyBill(listOrder, discountPrice);

        textpeopleCnt.text = peopleCount.ToString();

        totalPrice = bill.BillTotalPrice;
        textTotalPrice.text = Info.MakeMoneyString (totalPrice);

        int dutchPayPrice = totalPrice / peopleCount;
        textDutchPayPrice.text = Info.MakeMoneyString (dutchPayPrice);

        textExtraGameCnt.text = "남은 할인 찬스 : <color='#70ad47'><size=23>" + Info.GamePlayCnt.ToString() + "</size></color>";
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
