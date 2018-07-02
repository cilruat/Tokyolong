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

    [System.NonSerialized]public float eltHeight;

    const float paddingLeft = 30f;
    const float paddingRight = 5f;
    const float paddingTop = 5f;
    const float paddingBottom = 5f;

    const float ELT_MIN_WIDTH = 200f;
	const float ELT_MIN_HEIGHT = 30f;

    public void SetChatElt(UserInfo info, UserChat chat)
	{
        for (int i = 0; i < chatPersons.Length; i++)
            chatPersons[i].rtPerson.gameObject.SetActive(i == chat.person);

        ChatPerson current = chatPersons[chat.person];
        current.imgCustomer.texture = imgCustomer[info.customerType];
        current.imgCustomer.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, info.customerType != 2 ? 35f : 70f);

        current.textTableNo.text = "No. <size='20'>" + info.tableNo.ToString() + "</size>";
        current.textCount.text = info.peopleCnt.ToString() + "명";

        string[] times = chat.time.Split('/');
        string sendTT = times[0] == "AM" ? "오전" : "오후";
        string sendHH = times[1].ToString().StartsWith("0") ? times[1].ToString().Substring(1) : times[1].ToString();
        string sendMM = times[2].ToString();
        string sendTime = sendTT + " " + sendHH + ":" + sendMM;

        current.textTime.text = sendTime;
        current.textChatMsg.text = chat.chat;

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

        this.eltHeight = rtTableNo.rect.height + current.rtChat.rect.height;

		RectTransform rtElt = GetComponent<RectTransform> ();
		rtElt.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, eltHeight);
	}
}
