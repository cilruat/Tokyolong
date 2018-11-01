using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageFindDiffPicture : SingletonMonobehaviour<PageFindDiffPicture> {

	public GameObject objStart;
	public Text txtTime;
	public Image imgTime;
	public CountDown limitTime;
	public RawImage imgVictory;
	public GameObject objVictory;
	public GameObject objSendServer;
	public GameObject objGameOver;
	public GameObject objReady;
	public GameObject objGo;
	public GameObject objBoard;
	public GameObject objQuit;
	public GameObject objHide;

	public Transform trFive;

	int finish = 0;
	List<GameObject> listObjL = new List<GameObject> ();
	List<GameObject> listObjR = new List<GameObject> ();

	void Awake()
	{
		// test
		Info.GameDiscountWon = 0;

		Transform tr = null;
		switch (Info.GameDiscountWon) {
		case (short)EDiscount.e1000won:		tr = trFive;	finish = 5;		break;
		}
			
		int rand = Random.Range (0, tr.childCount);
		Transform findPicture = null;

		for (int i = 0; i < tr.childCount; i++) {
			if (i != rand)
				continue;

			findPicture = tr.GetChild (i);
			break;
		}

		_SetAnswer (true, findPicture);
		_SetAnswer (false, findPicture);
	}

	void _SetAnswer(bool isLeft, Transform parent)
	{
		string str = isLeft ? "Left" : "Right";
		Transform trLeft = parent.Find (str);
		for (int i = 0; i < finish; i++) {
			int num = i + 1;
			Transform child = trLeft.Find (num.ToString ());
			GameObject obj = child.Find ("Circle").gameObject;
			obj.SetActive (false);

			Button btn = child.GetComponent<Button> ();
			btn.onClick.RemoveAllListeners ();

			int idx = i;
			btn.onClick.AddListener (() => _OnFind (idx));

			if (isLeft)
				listObjL.Add (obj);
			else
				listObjR.Add (obj);
		}
	}

	void _OnFind(int idx)
	{
		for (int i = 0; i < listObjL.Count; i++) {
			if (i != idx)
				continue;
			
			listObjL [i].SetActive (true);
			listObjR [i].SetActive (true);
			break;
		}
	}
}
