using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TableDiscountElt : MonoBehaviour {

	public Text txtName;
	public Text txtVal;
	public Text txtDiscount;

    EDiscount type;
	int value = 0;
	int discount = 0;

    public void SetInfo(EDiscount type)
	{
        this.type = type;

        switch (type)
        {
            case EDiscount.e1000won:    txtName.text = "1000원 할인";  break;
        }
	}

    public void OnChangeValue(bool isAdd)
    {
        value = Mathf.Max(0, isAdd ? value + 1 : value - 1);

        txtVal.text = value.ToString();
        _RefreshPrice();
    }

	void _RefreshPrice()
	{
        discount = Info.GetDiscountPrice (type) * value;
        txtDiscount.text = Info.MakeMoneyString (discount);

        AdminTableDiscountInput.Instance.CalcTotalPrice();
	}

	public int GetCount() {	return value; }
    public int GetDiscount() { return discount; }
    public EDiscount DiscountType() { return type; }
}
