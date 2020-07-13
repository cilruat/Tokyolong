using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MailBoard : MonoBehaviour {

    public RectTransform rtMail;
    public MailElt mailElt;

	UserMsgInfo userMsgInfo = null;

    VirtualKeyboard keyboard = new VirtualKeyboard();

	void Update()
	{
		if (Input.GetMouseButton(0))
		{
			if (Info.IsInputFieldFocused())
				keyboard.ShowOnScreenKeyboard();
		}
	}

	public void OnEndEdit()
	{
		keyboard.HideOnScreenKeyboard();
	}


	public void SetMail(UserMsgInfo msgInfo)
    {
		this.userMsgInfo = msgInfo;

		if (this.userMsgInfo == null)
			return;


		//UserMail mail = msgInfo.strMsg;

		//UserMail mail = this.userMsgInf
		//AddMailElt(mail);

	}


}
