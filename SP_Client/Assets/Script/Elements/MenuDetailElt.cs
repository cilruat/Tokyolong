using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuDetailElt : MonoBehaviour 
{
    public Text textName;
    public Text textPrice;

    EMenuDetail type = EMenuDetail.eNagasaki;

    public void SetMenuElt(EMenuDetail type)
    {
        this.type = type;

        textName.text = Info.MenuName (type);
        textPrice.text = Info.MakeMoneyString (Info.MenuPrice (type));
    }

    public void OnSelect()
    {
        AdminTableOrderInput.Instance.OnSelectMenuDetailElt(type);
    }
}
