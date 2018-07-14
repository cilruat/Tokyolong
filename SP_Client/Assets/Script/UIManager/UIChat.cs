using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIChat : MonoBehaviour 
{
	public ScrollRect srTable;
	public ChatTableElt tableElt;

    public ChatBoard chatBoard;
	public RectTransform rtChatEmpty;

    public InputField input;
    public RectTransform rtInput;

    Dictionary<byte, ChatTableElt> dictChatTable = new Dictionary<byte, ChatTableElt>();

    byte selectTableNo = byte.MaxValue;

    public void ShowChatTable()
    {
        foreach (KeyValuePair<byte, UserChatInfo> pair in Info.dictUserChatInfo)
        {
            bool isCreate = dictChatTable.ContainsKey(pair.Key) == false;
            if (isCreate)   
                AddTableChat(pair.Key);
            else 
                dictChatTable[pair.Key].OnNewActive(pair.Value.isNew);

            dictChatTable[pair.Key].OnSelected(false);
        }

        if (chatBoard.gameObject.activeSelf)
            chatBoard.gameObject.SetActive(false);

        if (rtChatEmpty.gameObject.activeSelf == false)
            rtChatEmpty.gameObject.SetActive(true);
    }

    public void SelectTable(byte tableNo)
    {
        if (dictChatTable.ContainsKey(selectTableNo))
            dictChatTable[selectTableNo].OnSelected(false);

        if (chatBoard.gameObject.activeSelf == false)
            chatBoard.gameObject.SetActive(true);

        if (rtChatEmpty.gameObject.activeSelf)
            rtChatEmpty.gameObject.SetActive(false);

        selectTableNo = tableNo;
        dictChatTable[selectTableNo].OnSelected(true);
        chatBoard.SetChat(Info.GetUserChat(tableNo));
    }

    void AddTableChat(byte tableNo)
	{
		ChatTableElt elt = CreateChatTableElt ();
		elt.SetTableElt (tableNo);
		dictChatTable.Add (tableNo, elt);

        Info.AddUserChatInfo(tableNo, null, false);
	}

    public void SelectAddTableChat(byte tableNo)
    {
        ShowChatTable();

        if (Info.GetUserChat(tableNo) == null)
            AddTableChat(tableNo);

        SelectTable(tableNo);
    }

	ChatTableElt CreateChatTableElt()
	{
		GameObject newObj = Instantiate (tableElt.gameObject) as GameObject;
		newObj.transform.SetParent (srTable.content);
		newObj.transform.InitTransform ();
        newObj.gameObject.SetActive(true);

		ChatTableElt newElt = newObj.GetComponent<ChatTableElt> ();

		return newElt;
	}

    public void RemoveChatTableElt(byte tableNo)
    {
        if (dictChatTable.ContainsKey(tableNo) == false)
            return;

        Destroy(dictChatTable[tableNo].gameObject);
        dictChatTable.Remove(tableNo);
    }

    public void OnChatSend()
    {
        if (input.text == string.Empty)
            return;

        string chat = input.text;
        NetworkManager.Instance.Chat_REQ(selectTableNo, chat);
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
