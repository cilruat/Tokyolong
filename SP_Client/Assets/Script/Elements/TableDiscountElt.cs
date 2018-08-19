using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TableDiscountElt : MonoBehaviour {

	public Text txtName;
	public Text txtVal;

    EDiscount type;
	int value = 0;

    public void SetInfo(EDiscount type)
	{
        this.type = type;

        switch (type)
        {
            case EDiscount.e1000won:    txtName.text = "1000원 할인";   break;
            case EDiscount.e5000won:    txtName.text = "5000원 할인";   break;
            case EDiscount.eHalf:       txtName.text = "반액 할인";     break;
            case EDiscount.eAll:        txtName.text = "전액 할인";     break;
        }
	}

    public void OnChangeValue(bool isAdd)
    {
        value = Mathf.Max(0, isAdd ? value + 1 : value - 1);
        if (value > 1 && (type == EDiscount.eHalf || type == EDiscount.eAll))
        {
            value = 1;
            return;
        }

        txtVal.text = value.ToString();
        AdminTableDiscountInput.Instance.RefreshCalcPrice();
    }

	public int GetCount() {	return value; }
    public EDiscount DiscountType() { return type; }
}
