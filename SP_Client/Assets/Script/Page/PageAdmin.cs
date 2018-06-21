using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageAdmin : SingletonMonobehaviour<PageAdmin> {

	const int TABLE_NUM = 30;

	public RectTransform rtScrollTable;
	public RectTransform rtScrollOrder;
	public RectTransform rtScrollMusic;
	public GameObject prefabTable;
	public GameObject prefabOrder;
	public GameObject prefabMusic;

	List<TableElt> listTable = new List<TableElt>();
	List<OrderElt> listOrder = new List<OrderElt>();
	List<MusicElt> listMusic = new List<MusicElt>();

	void Awake()
	{
		for (int i = 0; i < TABLE_NUM; i++) {
			GameObject obj = Instantiate (prefabTable) as GameObject;
			obj.SetActive (true);

			Transform tr = obj.transform;
			tr.SetParent (rtScrollTable);
			tr.InitTransform ();

			TableElt elt = obj.GetComponent<TableElt> ();

			int tableNum = i + 1;
			elt.SetTable (tableNum);

			listTable.Add (elt);
		}
	}

	public void SetOrder(bool isOrder, string packing)
	{
		GameObject prefab = isOrder ? prefabOrder : prefabMusic;
		RectTransform rtScroll = isOrder ? rtScrollOrder : rtScrollMusic;

		GameObject obj = Instantiate (prefab) as GameObject;

		Transform tr = obj.transform;
		tr.SetParent (rtScroll);
		tr.InitTransform ();

		if (isOrder) {
			OrderElt elt = obj.GetComponent<OrderElt> ();
			elt.SetInfo (listOrder.Count, packing);
			listOrder.Add (elt);
		} else {
			MusicElt elt = obj.GetComponent<MusicElt> ();
			elt.SetInfo (listMusic.Count, packing);
			listMusic.Add (elt);
		}
	}

	public void RemoveElt(bool isOrder, int id)
	{
		int findIdx = -1;
		int length = isOrder ? listOrder.Count : listMusic.Count;
		for (int i = 0; i < length; i++) {
			int compare = isOrder ? listOrder [i].GetID () : listMusic [i].GetID ();
			if (compare != id)
				continue;

			findIdx = i;
			if (isOrder)	listOrder.RemoveAt (i);
			else			listMusic.RemoveAt (i);
			break;
		}

		if (findIdx == -1)
			return;

		RectTransform rtScroll = isOrder ? rtScrollOrder : rtScrollMusic;
		for (int i = 0; i < rtScroll.childCount; i++) {
			if (i != findIdx)
				continue;

			Transform child = rtScroll.GetChild (i);
			if (child)
				Destroy (child.gameObject);
			break;
		}
	}

	void Update()
	{
		if (Input.GetKeyDown (KeyCode.Keypad0)) {
			int rand = Random.Range (1, listTable.Count);
			listTable [rand].Login ();
		}

		if (Input.GetKeyDown (KeyCode.Keypad1)) {
			int rand = Random.Range (1, listTable.Count);
			listTable [rand].Urgency ();
		}
	}
}
