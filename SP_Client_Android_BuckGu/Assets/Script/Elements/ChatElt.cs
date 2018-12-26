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
//        public Text textCount;
        public Text textTableNo;
        public Text textTime;
        public RectTransform rtChat;
        public Text textChatMsg;
    }

    public RectTransform rtElt;
    public ChatPerson[] chatPersons;
    public Texture[] imgCustomer;

    [System.NonSerialized]public float eltHeight;

    const float ELT_MIN_HEIGHT = 60f;
    const float paddingLeft = 15f;
    const float paddingRight = 6f;
    const float paddingTop = 4f;
    const float paddingBottom = 4f;

    const float CHAT_MIN_WIDTH = 40f;
    const float CHAT_MAX_WIDTH = 200f;
	const float CHAT_MIN_HEIGHT = 30f;

    public void SetChatElt(UserChat chat)
	{
        for (int i = 0; i < chatPersons.Length; i++)
            chatPersons[i].rtPerson.gameObject.SetActive(i == (int)chat.person);

        UserInfo info = chat.info;

        ChatPerson current = chatPersons[chat.person];
        current.imgCustomer.texture = imgCustomer[info.customerType];

        current.textTableNo.text = "No. <size='20'>" + info.tableNo.ToString() + "</size>";

        string[] times = chat.time.Split('/');
        string sendTT = times[0];
        string sendHH = times[1];
        string sendMM = times[2];
        string sendTime = sendTT + " " + sendHH + ":" + sendMM;

        current.textTime.text = sendTime;
        current.textChatMsg.text = chat.chat;

		RectTransform rtChatMsg = current.textChatMsg.GetComponent<RectTransform> ();
        float chatWidth = Mathf.Max(CHAT_MIN_WIDTH, Mathf.Min(CHAT_MAX_WIDTH, current.textChatMsg.preferredWidth));
        rtChatMsg.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, chatWidth);

        float chatHeight = Mathf.Max (CHAT_MIN_HEIGHT, current.textChatMsg.preferredHeight);
        rtChatMsg.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, chatHeight);

        chatWidth += (paddingLeft + paddingRight);
        chatHeight += (paddingTop + paddingBottom);

		current.rtChat.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, chatWidth);
		current.rtChat.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, chatHeight);

		RectTransform rtTableNo = current.textTableNo.GetComponent<RectTransform> ();

        this.eltHeight = Mathf.Max(ELT_MIN_HEIGHT, rtTableNo.rect.height + chatHeight);

		rtElt.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, this.eltHeight);
	}
}
