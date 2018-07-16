using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameElt : MonoBehaviour {

	public Text txtTitle;
	public Text txtName;

	EGameType eType = EGameType.eWinWaiter;
	int game = -1;
	EDiscount eDis = EDiscount.e500won;


	public void SetInfo(EGameType eType, int game, EDiscount eDis)
	{
		this.eType = eType;
		this.game = game;
		this.eDis = eDis;

		txtTitle.text = Info.GameTitle (eType);
		txtName.text = Info.GameName (eType, game, eDis);
	}

	public EGameType GameType() { return eType; }
	public int Game() {	return game; }
	public EDiscount Discount() { return eDis; }
}
