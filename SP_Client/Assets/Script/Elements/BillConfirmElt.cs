using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BillConfirmElt : MonoBehaviour {

	public Text txtName;
	public Text txtVal;
	public Text txtPrice;

	EMenuDetail eType;
    int oriValue = 0;
	int value = 0;
	int price = 0;

    MenuData data;

	public void SetInfo(EMenuDetail eType, int value)
	{
		this.eType = eType;
        this.data = MenuData.Get((int)eType);

        txtName.text = data.menuName;

        this.oriValue = value;
		this.value = value;
		txtVal.text = value.ToString ();

		_RefreshPrice ();
	}

	public void OnChangeValue(bool isAdd)
	{
        value = Mathf.Max(0, isAdd ? value + 1 : value - 1);
        txtVal.text = value.ToString ();
		_RefreshPrice ();
	}

	void _RefreshPrice()
	{
        price = data.price * value;
		txtPrice.text = Info.MakeMoneyString (price);

        AdminBillConfirm.Instance.MenuChange(eType, value, oriValue);
	}

	public void OnDelete()
	{
        value = 0;
        _RefreshPrice();
	}

	public int GetCount() {	return value; }
	public int GetPrice() { return price; }
	public EMenuDetail MenuDetailType() { return eType; }
}
