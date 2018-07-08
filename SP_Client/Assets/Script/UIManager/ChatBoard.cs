using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatBoard : MonoBehaviour 
{
    public RectTransform rtSrChat;
	public ScrollRect srChat;
	public ChatElt chatElt;	

	List<ChatElt> listChat = new List<ChatElt>();

    UserChatInfo userChatInfo = null;

    public void RemoveAllChat()
    {
        for(int i = listChat.Count-1; i >= 0; i--)
        {
            Destroy(listChat[i].gameObject);
            listChat.RemoveAt(i);
        }
    }

    public void SetChat(UserChatInfo chatInfo)
    {
        RemoveAllChat();
        this.userChatInfo = chatInfo;

        if (this.userChatInfo == null)
            return;

        for (int i = 0; i < this.userChatInfo.listChat.Count; i++)
        {
            UserChat chat = this.userChatInfo.listChat[i];
            AddChatElt(chatInfo.info, chat);
        }
    }

    public void AddChatElt(UserInfo userInfo, UserChat chat)
	{
		ChatElt elt = CreateChatElt ();
        elt.SetChatElt (userInfo, chat);
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
}
