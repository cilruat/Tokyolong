using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatTable
{
    public int tableID;
}

public class UIChat : MonoBehaviour 
{
	public ScrollRect srTable;
	public ChatTableElt tableElt;

    public ChatBoard chatBoard;
	public RectTransform rtChatEmpty;

	Dictionary<int, ChatTableElt> dictChatTable = new Dictionary<int, ChatTableElt>();

	public void AddTableChat(int tableNo)
	{
        if (dictChatTable.ContainsKey(tableNo))
            return;

		ChatTableElt elt = CreateChatTableElt ();
		elt.SetTableElt (tableNo);
		dictChatTable.Add (tableNo, elt);
	}

	ChatTableElt CreateChatTableElt()
	{
		GameObject newObj = Instantiate (tableElt.gameObject) as GameObject;
		newObj.transform.SetParent (srTable.content);
		newObj.transform.InitTransform ();
        newObj.gameObject.SetActive(true);

		ChatTableElt newElt = newObj.GetComponent<ChatTableElt> ();

		return newElt;
	}

    public void HideChat()
    {
        UIManager.Instance.Hide(eUI.eChat);
    }
}
