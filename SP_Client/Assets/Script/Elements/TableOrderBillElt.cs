using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TableOrderBillElt : MonoBehaviour {

	public Text txtName;
	public Text txtVal;
	public Text txtPrice;

    EMenuDetail type;
	int value = 0;
	int price = 0;
    MenuData data;
    public void SetInfo(EMenuDetail type)
	{
        SetInfo (type, 1);
	}

    public void SetInfo(EMenuDetail type, int value)
	{
        this.type = type;
        this.data = MenuData.Get((int)type);
        if (this.data == null)
            return;

        txtName.text = data.menuName;

		this.value = value;
		txtVal.text = value.ToString ();

		_RefreshPrice ();
	}

	public void OnChangeValue(bool isAdd)
	{
		value = isAdd ? value + 1 : value - 1;
		if (value < 1)
			value = 1;

		txtVal.text = value.ToString ();
		_RefreshPrice ();
	}

	void _RefreshPrice()
	{
        price = this.data.price * value;
		txtPrice.text = Info.MakeMoneyString (price);

        AdminTableOrderInput.Instance.tableOrderBill.CalcTotalPrice();
	}

	public void OnDelete()
	{
        AdminTableOrderInput.Instance.tableOrderBill.RemoveElt(type);
	}

	public int GetCount() {	return value; }
	public int GetPrice() { return price; }
    public EMenuDetail MenuDetailType() { return type; }
    public EMenuType MenuType() { return (EMenuType)this.data.category; }
}
