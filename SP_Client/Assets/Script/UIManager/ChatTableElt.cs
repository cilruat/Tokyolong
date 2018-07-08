using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatTableElt : MonoBehaviour 
{
    public UIChat uiChat;

    public Button btn;
    public Image imgSelect;
    public Text textTableNo;
    public GameObject objNew;

    byte tableNo;

	public void SetTableElt(byte tableNo)
    {
        OnNewActive(true);

		this.tableNo = tableNo;
		textTableNo.text = "No. <size='30'>" + tableNo.ToString () + "</size>";
	}

    public void OnSelect()
    {
        OnNewActive(false);
        uiChat.SelectTable(this.tableNo);
    }

    public void OnSelected(bool isSelected)
    {
        imgSelect.gameObject.SetActive(isSelected);
    }

    public void OnNewActive(bool isActive)
    {
        if (objNew.gameObject.activeSelf == isActive)
            return;

        objNew.gameObject.SetActive(isActive);
    }
}
