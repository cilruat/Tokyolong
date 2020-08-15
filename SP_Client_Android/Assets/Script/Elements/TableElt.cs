using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;

public class TableElt : MonoBehaviour {

	public Text tableNum;
	public GameObject objCover;
	public GameObject objUrgency;

	bool isLogin = false;
	bool isUrgency = false;
	byte tableNo = 0;
    bool isLike = false;
	UITween tweenUrgency;

	List<short> listDiscount = new List<short>();

	public void SetTable(int num)
	{
		this.tableNo = (byte)num;
		tableNum.text = num.ToString ();
	}

	public void SetOrder(short discount)
	{
		listDiscount.Add (discount);
	}
		
	public void Login()
	{
		isLogin = true;
		objCover.SetActive (true);
	}

    public void Like()
    {
        isLike = true;
    }

	public void Logout()
	{		
		isLogin = false;
		objCover.SetActive (false);
        isLike = false;
		StopUrgency ();
	}

	public void Urgency()
	{
		if (isLogin == false || isUrgency)
			return;

		isUrgency = true;

		if (tweenUrgency == null)
			tweenUrgency = UITweenAlpha.Start (objUrgency, .25f, 1f, TWParam.New (.8f).Curve (TWCurve.Linear).Loop (TWLoop.PingPong));

		objCover.SetActive (false);
	}

	public void StopUrgency()
	{
		if (isUrgency == false)
			return;

		isUrgency = false;

		if (tweenUrgency != null) {
			tweenUrgency.StopTween ();
			tweenUrgency = null;
		}

		objUrgency.SetActive (false);
		objCover.SetActive (true);
	}

	public void OnTableMenu()
	{
		if (isLogin == false)
			return;

		PageAdmin.Instance.ShowTableMenu (tableNo);
	}

    public void OnMailMenu()
    {
#if !UNITY_EDITOR
        if (isLogin == false)
        {
            SystemMessage.Instance.Add("비어있는 테이블입니다.");
            return;
        }

        if(Info.TableNum == this.tableNo)
        {
            SystemMessage.Instance.Add("내 테이블입니다.");
            return;
        }
        
#endif

        if (PageMail.Instance.objTableMenu == null)
            return;

        MailTableMenu mail = PageMail.Instance.objTableMenu.GetComponent<MailTableMenu>();
        if (mail != null)
            mail.SetInfo(tableNo);

        PageMail.Instance.objTableMenu.SetActive(true);

    }

    public bool IsLogin() { return isLogin; }
	public byte GetTableNo() { return this.tableNo; }
	public bool GetUrgency() { return isUrgency; }
    public bool IsLike() { return isLike; }

}
