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

    public ChatBoard chatBoard;
	public RectTransform rtChatEmpty;

	Dictionary<int, ChatTableElt> dictChatTable = new Dictionary<int, ChatTableElt>();

    void OnEnable()
    {
        chatBoard.RemoveAllChat();
        byte person = (byte)0;
        int customer = 2;
        int tableNo = 10;
        byte personCount = 2;
        int time = 0;
        string msg = "안녕하세요 \n 더 이상 네버 고잉 다운 입니다. ㅎㅎㅎㅎ";

        chatBoard.AddChatElt(person, customer, tableNo, personCount, time, msg);
    }

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

    public void HideChat()
    {
        UIManager.Instance.Hide(eUI.eChat);
    }
}
