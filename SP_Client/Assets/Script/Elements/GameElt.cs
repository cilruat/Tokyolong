using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameElt : MonoBehaviour {

	public Text txtTitle;
	public Text txtName;

	byte tableNo = 0;
	int id = -1;
	EDiscount eDis = EDiscount.e500won;

	public void SetInfo(byte tableNo, int id, EGameType eType, int game, EDiscount eDis)
	{
		this.tableNo = tableNo;
		this.id = id;
		this.eDis = eDis;

		txtTitle.text = Info.GameTitle (eType);
		txtName.text = Info.GameName (eType, game, eDis);
	}

	public void OnConfirm(bool discount)
	{
		short sDis = discount ? (short)eDis : (short)-1;
		NetworkManager.Instance.UnfinishGameConfirm_REQ (tableNo, id, sDis);
	}

	public int GetID() { return id; }
}
