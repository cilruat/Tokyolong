using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BillElt : MonoBehaviour {

	public Text txtName;
	public Text txtVal;
	public Text txtPrice;

	EMenuDetail eType;
	int value = 0;
	int price = 0;

    MenuData menu;

	public void SetInfo(EMenuDetail eType)
	{
		SetInfo (eType, 1);
	}

	public void SetInfo(EMenuDetail eType, int value)
	{
		this.eType = eType;
        this.menu = MenuData.Get((int)eType);

        txtName.text = this.menu.menuName;

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
        price = menu.price * value;
		txtPrice.text = Info.MakeMoneyString (price);

        if (Info.isCheckScene("Admin") == false)
        {
            if (PageBase.Instance.GetType() == typeof(PageOrder))
                ((PageOrder)PageBase.Instance).bill.CalcTotalPrice();
        }
	}

	public void OnDelete()
	{
		((PageOrder)PageBase.Instance).bill.RemoveElt (eType);
	}

	public int GetCount() {	return value; }
	public int GetPrice() { return price; }
	public EMenuDetail MenuDetailType() { return eType; }
    public EMenuType MenuType() { return (EMenuType)this.menu.category; }
}
