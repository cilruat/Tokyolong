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
        public ContentSizeFitter fitter;
    }

    public ChatPerson[] chatPersons;
    public Texture[] imgCustomer;


    const float paddingLeft = 22f;
    const float paddingRight = 5f;
    const float paddingTop = 5f;
    const float paddingBottom = 5f;

	const float ELT_MIN_HEIGHT = 30f;

//	// 0: you ,  1: me
//	public Text tableNo;
//	public Text[] time;
//	public Text[] chat;
//	public GameObject objYou;
//	public GameObject objMe;

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
        current.fitter.SetLayoutHorizontal();
		current.fitter.SetLayoutVertical ();

		RectTransform rtChatMsg = current.textChatMsg.GetComponent<RectTransform> ();
		float chatWidth = rtChatMsg.rect.width;
		float chatHeight = rtChatMsg.rect.height;

		chatWidth += paddingLeft + paddingRight;
		chatHeight += paddingTop + paddingBottom;

        chatHeight = Mathf.Max (ELT_MIN_HEIGHT, chatHeight);

		current.rtChat.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, chatWidth);
		current.rtChat.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, chatHeight);

		RectTransform rtTableNo = current.textTableNo.GetComponent<RectTransform> ();

		float eltHeight = rtTableNo.rect.height + current.rtChat.rect.height;

		RectTransform rtElt = GetComponent<RectTransform> ();
		rtElt.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, eltHeight);
	}
}
