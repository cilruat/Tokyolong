using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatTableElt : MonoBehaviour 
{
    public Button btn;
    public Image imgSelect;
    public Text textTableNo;
    public GameObject objNew;

	int tableNo = -1;

	public void SetTableElt(int tableNo)
    {
        if (objNew.activeSelf == false)
            objNew.gameObject.SetActive(true);

		this.tableNo = tableNo;
		textTableNo.text = "No. <size='30'>" + tableNo.ToString () + "</size>";
	}

    public void OnSelect()
    {
        if (objNew.activeSelf)
            objNew.gameObject.SetActive(false);
    }
}
