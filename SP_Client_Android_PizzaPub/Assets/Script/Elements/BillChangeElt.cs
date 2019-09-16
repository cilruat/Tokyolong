using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BillChangeElt : MonoBehaviour {

	public Text txtName;
    public GameObject objDelete;
    public GameObject objChange;
    public Text txtOriVal;
	public Text txtChangeVal;
	public Text txtPrice;

    EMenuDetail type;
    int oriValue = 0;
	int value = 0;
	int price = 0;

    MenuData data;

    public void SetInfo(EMenuDetail type, int oriVal, int value)
	{
        this.type = type;
        this.data = MenuData.Get((int)type);
        if (data == null)
            return;

        txtName.text = data.menuName;

        this.oriValue = oriVal;
        txtOriVal.text = this.oriValue.ToString();

		this.value = value;
        txtChangeVal.text = value.ToString ();

		_RefreshPrice ();
	}

    public void SetValue(int value)
    {
        this.value = Mathf.Max (value, 0);
        txtChangeVal.text = value.ToString ();

        objDelete.gameObject.SetActive(value <= 0);
        objChange.gameObject.SetActive(value > 0);

        _RefreshPrice ();
    }

	void _RefreshPrice()
	{
        price = data.price * value;
		txtPrice.text = Info.MakeMoneyString (price);
	}

    public int GetCount() {	return value - oriValue; }
	public int GetPrice() { return price; }
    public EMenuDetail MenuDetailType() { return type; }
    public EMenuType MenuType() { return (EMenuType)this.data.category; }
}
