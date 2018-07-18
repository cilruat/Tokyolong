using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameElt : MonoBehaviour {

	public Text txtTitle;
	public Text txtName;

	int id = -1;

	public void SetInfo(int id, EGameType eType, int game, EDiscount eDis)
	{
		this.id = id;

		txtTitle.text = Info.GameTitle (eType);
		txtName.text = Info.GameName (eType, game, eDis);
	}

	public int GetID() { return id; }
}
