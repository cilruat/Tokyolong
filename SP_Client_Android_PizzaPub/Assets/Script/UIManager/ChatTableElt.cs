using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatTableElt : MonoBehaviour 
{
    public Image imgSelect;
    public Text textTableNo;
    public GameObject objNew;

    byte tableNo;

    UIChat owner;

    public void SetTableElt(byte tableNo, UIChat owner)
    {
        OnNewActive(true);

		this.tableNo = tableNo;
        this.owner = owner;

		textTableNo.text = "No. <size='30'>" + tableNo.ToString () + "</size>";
	}

    public void OnSelect()
    {
        OnNewActive(false);
		this.owner.SelectTable(this.tableNo);
    }

    public void OnSelected(bool isSelected)
    {
        imgSelect.gameObject.SetActive(isSelected);
    }

    public void OnNewActive(bool isActive)
    {
        objNew.gameObject.SetActive(isActive);
    }

    public void Remove()
    {
        this.owner.RemoveChat(tableNo);
    }
}
