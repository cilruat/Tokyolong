using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatElt : MonoBehaviour {

    [System.Serializable]
    public class ChatPerson
    {
        public RectTransform rtPerson;
        public RawImage imgCustomer;
        public Text textCount;
        public Text textTableNo;
        public Text textTime;
        public RectTransform rtChat;
        public Text textChatMsg;
    }

    public ChatPerson[] chatPersons;
    public Texture[] imgCustomer;

    const float paddingLeft = 22f;
    const float paddingRight = 5f;
    const float paddingTop = 5f;
    const float paddingBottom = 5f;

    const float ELT_MIN_WIDTH = 200f;
	const float ELT_MIN_HEIGHT = 30f;

    public void SetChatElt(byte person, int customer, int tableNo, byte personCount, int time, string msg)
	{
        for (int i = 0; i < chatPersons.Length; i++)
            chatPersons[i].rtPerson.gameObject.SetActive(i == person);

        ChatPerson current = chatPersons[person];
        current.imgCustomer.texture = imgCustomer[customer];

        current.textTableNo.text = "No. <size='20'>" + tableNo.ToString() + "</size>";
        current.textCount.text = personCount.ToString() + "명";
        current.textTime.text = time.ToString();
        current.textChatMsg.text = msg;

		RectTransform rtChatMsg = current.textChatMsg.GetComponent<RectTransform> ();
        float chatWidth = Mathf.Min(ELT_MIN_WIDTH, current.textChatMsg.preferredWidth);
        rtChatMsg.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, chatWidth);

        float chatHeight = Mathf.Max (ELT_MIN_HEIGHT, current.textChatMsg.preferredHeight);
        rtChatMsg.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, chatHeight);

        chatWidth += paddingLeft + paddingRight;
        chatHeight += paddingTop + paddingBottom;

		current.rtChat.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, chatWidth);
		current.rtChat.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, chatHeight);

		RectTransform rtTableNo = current.textTableNo.GetComponent<RectTransform> ();

		float eltHeight = rtTableNo.rect.height + current.rtChat.rect.height;

		RectTransform rtElt = GetComponent<RectTransform> ();
		rtElt.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, eltHeight);
	}
}
