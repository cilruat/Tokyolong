using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class PageGame : PageBase {

	public RectTransform rtScroll;
	public ScrollRect scrollRect;
	public GameObject objPrefab;

	void _RefreshList()
	{
		for (int i = 0; i < rtScroll.childCount; i++) {
			Transform child = rtScroll.GetChild (i);
			if (child)
				Destroy (child.gameObject);
		}

		for (int i = 0; i < Info.listUnfinishGame.Count; i++) {
			GameElt elt = Info.listUnfinishGame [i];
			_SetUnfinishGame (elt.GameType (), elt.Game (), elt.Discount (), false);
		}

		scrollRect.horizontalNormalizedPosition = 0f;
	}

	void _SetUnfinishGame(EGameType eType, int game, EDiscount eDis, bool addList = true)
	{
		GameObject obj = Instantiate (objPrefab) as GameObject;
		obj.SetActive (true);

		Transform tr = obj.transform;
		tr.SetParent (rtScroll);
		tr.InitTransform ();

		GameElt elt = obj.GetComponent<GameElt> ();
		elt.SetInfo (eType, game, eDis);

		if (addList)
			Info.listUnfinishGame.Add (elt);
	}

	public void RemoveUnfinish()
	{
	}
}
