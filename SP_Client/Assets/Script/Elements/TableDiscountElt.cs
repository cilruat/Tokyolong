using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TableDiscountElt : MonoBehaviour {

    public EDiscount type;
	public Text txtName;

    int idx = 0;
    int discount = 0;

    void Awake()
    {
        SetInfo(type);
    }

    public void SetInfo(EDiscount type, int idx, int discount)
	{
        this.idx = idx;
        this.discount = discount;

        SetInfo(type);
	}

    public void SetInfo(EDiscount type)
    {
        this.type = type;
        switch (type)
        {
            case EDiscount.e1000won:    txtName.text = "1000원 할인";   break;
            case EDiscount.e5000won:    txtName.text = "5000원 할인";   break;
            case EDiscount.eHalf:       txtName.text = "반액 할인";     break;
            case EDiscount.eAll:        txtName.text = "전액 할인";     break;
            case EDiscount.eDirect:     txtName.text = this.discount + "원 할인";  break;
        }
    }

    public void SetIndex(int idx) { this.idx = idx; }

    public void OnRegister()
    {
        AdminTableDiscountInput.Instance.OnRegister(this.type);
    }

    public void OnDelete()
    {
        AdminTableDiscountInput.Instance.OnDelete(idx);
    }

    public EDiscount DiscountType() { return type; }
    public int Discount() { return this.discount; }
}
