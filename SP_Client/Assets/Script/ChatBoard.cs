using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatBoard : MonoBehaviour 
{
	public ScrollRect srChat;
	public ChatElt chatElt;	

	public InputField input;
    public RectTransform rtInput;

	List<ChatElt> listChat = new List<ChatElt>();

    public void RemoveAllChat()
    {
        for(int i = listChat.Count-1; i >= 0; i--)
        {
            Destroy(listChat[i].gameObject);
            listChat.RemoveAt(i);
        }
    }

    public void AddChatElt(byte person, int customer, int tableNo, byte personCount, int time, string msg)
	{
		ChatElt elt = CreateChatElt ();
        elt.SetChatElt (person, customer, tableNo, personCount, time, msg);
		listChat.Add (elt);
	}

	ChatElt CreateChatElt()
	{
		GameObject newObj = Instantiate (chatElt.gameObject) as GameObject;
		newObj.transform.SetParent (srChat.content);
		newObj.transform.InitTransform ();
        newObj.gameObject.SetActive(true);

		ChatElt newElt = newObj.GetComponent<ChatElt> ();

		return newElt;
	}

	public void OnChatSend()
	{
		if (input.text == string.Empty)
			return;

		//REQ Send~
		//AddChatElt(myPerson, myTableNo, myPerson, currentTime, input.text);
        byte person = (byte)1;
        int customer = 1;
        int tableNo = 5;
        byte personCount = 1;
        int time = 0;
        string msg = input.text;

        AddChatElt(person, customer, tableNo, personCount, time, msg);
        input.text = string.Empty;
	}

    const char LINE_CUTTER = '\n';
    const int MAX_LINE = 3;
    const float INPUT_MIN_HEIGHT = 30f;
    float textPaddingTop = 3f;
    float textPAddingBottom = 3f;

    public void OnValueChanged()
    {
        string[] lines = input.text.Split(LINE_CUTTER);
        if (lines.Length > MAX_LINE)
            return;

        float totalHeight = input.preferredHeight + textPaddingTop + textPAddingBottom;
        rtInput.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Mathf.Max(INPUT_MIN_HEIGHT, totalHeight));
    }
}
