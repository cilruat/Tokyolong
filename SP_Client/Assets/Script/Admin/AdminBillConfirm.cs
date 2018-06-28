using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdminBillConfirm : SingletonMonobehaviour<AdminBillConfirm> {

	public Text table;
	public Bill bill;

	byte tableNo = 0;

	public void SetInfo(byte tableNo, List<KeyValuePair<EMenuDetail,int>> list)
	{
		this.tableNo = tableNo;
		table.text = tableNo.ToString () + "번 테이블";
		bill.CopyBill (list);
	}

	public void OnLogout()
	{
		NetworkManager.Instance.Logout_REQ(tableNo);
	}

	public void OnClose() {	gameObject.SetActive (false); }
}
