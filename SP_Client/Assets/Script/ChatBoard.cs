using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatBoard : MonoBehaviour 
{
    public RectTransform rtSrChat;
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

    public void AddChatElt(byte person, byte customer, int tableNo, byte personCount, string time, string msg)
	{
		ChatElt elt = CreateChatElt ();
        elt.SetChatElt (person, customer, tableNo, personCount, time, msg);
		listChat.Add (elt);

        ResizeScroll();
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

    void ResizeScroll()
    {
        float totalHeight = 0f;
        for (int i = 0; i < listChat.Count; i++)
            totalHeight += listChat[i].eltHeight;

        totalHeight += 5f * (float)(Mathf.Max(0, listChat.Count - 1));

        srChat.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, totalHeight);

        float scPosY = Mathf.Max(0f, (totalHeight - rtSrChat.rect.height));
        srChat.content.anchoredPosition = new Vector2(0f, scPosY);
    }

	public void OnChatSend()
	{
		if (input.text == string.Empty)
			return;

		//REQ Send~
		//AddChatElt(myPerson, myTableNo, myPerson, currentTime, input.text);
        byte person = (byte)1;
        byte customer = 0;
        int tableNo = 5;
        byte personCount = 1;
        string tt = System.DateTime.Now.ToString("tt");
        string hh = System.DateTime.Now.ToString("hh");
        string mm = System.DateTime.Now.ToString("mm");
        string time = tt + "/" + hh + "/" + mm;

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
