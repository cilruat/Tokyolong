using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuDetailElt : MonoBehaviour 
{
    public Text textName;
    public Text textPrice;

    MenuData data;

    public void SetMenuElt(MenuData data)
    {
        this.data = data;

        textName.text = data.menuName;
        textPrice.text = Info.MakeMoneyString (data.price);
    }

    public void OnSelect()
    {
        AdminTableOrderInput.Instance.OnSelectMenuDetailElt((EMenuDetail)data.menuID);
    }
}
