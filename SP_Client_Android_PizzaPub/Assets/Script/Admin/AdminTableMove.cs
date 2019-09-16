using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdminTableMove : SingletonMonobehaviour<AdminTableMove> {

	public Text table;
	public Text moveNo;
	public GameObject objMsgBox;
	public Text msg;

	byte tableNo = 0;
	int inputMoveNo = 0;

	void _init()
	{
		inputMoveNo = 0;
		moveNo.text = "0번";
		objMsgBox.SetActive (false);
		msg.text = "";
	}

	public void SetInfo(byte tableNo)
	{
		this.tableNo = tableNo;
		table.text = tableNo.ToString () + "번 테이블 - 자리 이동";
		_init ();
	}

	public void OnPressedNum(int num)
	{
		inputMoveNo = inputMoveNo == 0 ? num : (inputMoveNo * 10) + num;
		if (inputMoveNo > PageAdmin.TABLE_NUM) {
			inputMoveNo = PageAdmin.TABLE_NUM;
			SystemMessage.Instance.Add ("현재 테이블은 " + PageAdmin.TABLE_NUM.ToString () + "번까지 있어용!");
		}

		moveNo.text = inputMoveNo.ToString () + "번";
	}

	public void OnInit()
	{
		_init ();
	}

	public void OnShowMsgBox(bool isShow)
	{
		objMsgBox.SetActive (isShow);

		if(isShow)
			msg.text = "다시 확인부탁드려요~\n" + 
				this.tableNo.ToString () + "번 테이블 => <color=#ffff00>" + 
				inputMoveNo.ToString () + "번 테이블</color>";
	}

	public void OnMove()
	{
		if (inputMoveNo == this.tableNo) {
			SystemMessage.Instance.Add ("동일한 테이블로는 이동을 못하지요~");
			return;
		} else if (PageAdmin.Instance.CheckTableLogin (inputMoveNo) == false) {
			SystemMessage.Instance.Add (inputMoveNo.ToString() + "번 테이블은 접속되어 있지않아요~");
			return;
		}

		NetworkManager.Instance.TableMove_REQ (this.tableNo, inputMoveNo);
	}

	public void OnClose()
	{
		gameObject.SetActive (false);
	}

	public void MoveComplete()
	{
		string desc = this.tableNo.ToString () + "번 테이블에서 " + inputMoveNo.ToString () + "번 테이블로 자리 이동되었어요~";
		SystemMessage.Instance.Add (desc);

		this.tableNo = 0;
		_init ();
		OnClose ();
	}
}
