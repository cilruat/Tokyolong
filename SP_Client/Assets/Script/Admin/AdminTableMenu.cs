using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdminTableMenu : SingletonMonobehaviour<AdminTableMenu> {

	public Text table;

	byte tableNo = 0;
	List<KeyValuePair<EMenuDetail,int>> list = new List<KeyValuePair<EMenuDetail, int>> ();

	public void SetInfo (byte tableNo, List<KeyValuePair<EMenuDetail,int>> list)
	{
		this.list.Clear ();

		this.list = list;
		this.tableNo = tableNo;
		table.text = tableNo.ToString () + "번 테이블";
	}

	public void OnCallConfirm()
	{
		PageAdmin.Instance.StopUrgency (tableNo);
		OnClose ();
	}

	public void OnBillConfirm()
	{
		PageAdmin.Instance.ShowBillConfirm (tableNo, list);
		OnClose ();
	}

	public void OnClose() {	gameObject.SetActive (false); }
}