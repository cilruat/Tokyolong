using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatBoard : MonoBehaviour 
{
	public ScrollRect srChat;
	public ChatElt chatElt;	

	public InputField input;

	List<ChatElt> listChat = new List<ChatElt>();

	public void AddChatElt(int person, int tableNo, int personCount, int time, string msg)
	{
		ChatElt elt = CreateChatElt ();
		elt.SetChatElt (person, tableNo, personCount, time, msg);
		listChat.Add (elt);
	}

	ChatElt CreateChatElt()
	{
		GameObject newObj = Instantiate (chatElt.gameObject) as GameObject;
		newObj.transform.SetParent (srChat.content);
		newObj.transform.InitTransform ();

		ChatElt newElt = newObj.GetComponent<ChatElt> ();

		return newElt;
	}

	public void OnChatSend()
	{
		if (input.text == string.Empty)
			return;

		//REQ Send~
		//AddChatElt(myPerson, myTableNo, myPerson, currentTime, input.text);

		input.text = string.Empty;
	}
}
