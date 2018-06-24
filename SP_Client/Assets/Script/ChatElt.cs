using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatElt : MonoBehaviour {

    [System.Serializable]
    public class ChatPerson
    {
        public RectTransform rtPerson;
        public RawImage imgPerson;
        public Text textCount;
        public Text textTableNo;
        public Text textTime;
        public RectTransform rtChat;
        public Text textChatMsg;
        public ContentSizeFitter fitter;
    }

    public ChatPerson[] chatPersons;

    const float paddingLeft = 22f;
    const float paddingRight = 5f;
    const float paddingTop = 5f;
    const float paddingBottom = 5f;

//	// 0: you ,  1: me
//	public Text tableNo;
//	public Text[] time;
//	public Text[] chat;
//	public GameObject objYou;
//	public GameObject objMe;

    public void SetChat(int person, int tableNo, int personCount, int time, string msg)
	{
        for (int i = 0; i < chatPersons.Length; i++)
            chatPersons[i].rtPerson.gameObject.SetActive(i == person);

        ChatPerson current = chatPersons[person];

//        current.imgPerson.sprite = 

        current.textTableNo.text = "No. <size='20'>" + tableNo.ToString() + "</size>";
        current.textCount.text = personCount.ToString() + "명";
        current.textTime.text = time.ToString();
        current.textChatMsg.text = msg;
        current.fitter.SetLayoutHorizontal();
        current.fitter.SetLayoutVertical();



	}
}
