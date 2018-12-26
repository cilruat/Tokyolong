using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuElt : MonoBehaviour 
{
    public GameObject objSelected;
    public Text textName;

    EMenuType type = EMenuType.eNone;

    public void SetMenuElt(EMenuType type)
    {
        this.type = type;

        textName.text = Info.MenuTitle(type);
    }

    public void OnSelect() { AdminTableOrderInput.Instance.OnSelectMenuElt(type); }
    public void OnSelected(bool selected) { objSelected.SetActive(selected); }
}
