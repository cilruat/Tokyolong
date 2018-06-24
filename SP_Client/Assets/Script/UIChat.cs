using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatTable
{
    public int tableID;
    public List<string> msg;
}

public class UIChat : MonoBehaviour 
{
	public ScrollRect srTable;
	public ChatTableElt tableElt;

	public RectTransform rtChatBoard;
	public RectTransform rtChatEmpty;

	Dictionary<int, ChatTableElt> dictChatTable = new Dictionary<int, ChatTableElt>();

	public void AddTableChat(int tableNo)
	{
		ChatTableElt elt = CreateChatTableElt ();
		elt.SetTableElt (tableNo);
		dictChatTable.Add (tableNo, elt);
	}

	ChatTableElt CreateChatTableElt()
	{
		GameObject newObj = Instantiate (tableElt.gameObject) as GameObject;
		newObj.transform.SetParent (srTable.content);
		newObj.transform.InitTransform ();

		ChatTableElt newElt = newObj.GetComponent<ChatTableElt> ();

		return newElt;
	}
}
