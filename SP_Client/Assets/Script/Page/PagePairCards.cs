using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PagePairCards : MonoBehaviour {

	const int LIMIT_TIME = 20;
	const int NORMAL_MODE_CARD_COUNT = 18;
	const int HARD_MODE_CARD_COUNT = 24;

	public Text txtCountDown;
	public CountDown countDown;
	public Image imgTime;
	public GridLayoutGroup grid;
	public GameObject objCard;
	public GameObject objBoard;
	public List<Texture> listCards = new List<Texture> ();
	public List<CardElt> listElt = new List<CardElt> ();

	void Awake()
	{
		txtCountDown.text = LIMIT_TIME.ToString ();
		_SetCards ();
	}

	void _SetCards()
	{
		int mode = Random.Range (0, 2);
		mode = 0;
		int cnt = mode == 0 ? NORMAL_MODE_CARD_COUNT : HARD_MODE_CARD_COUNT;

		int prev_pairNum = 0;
		int pairNum = 0;
		Texture tex = null;
		for (int i = 0; i < cnt; i++) {
			GameObject obj = Instantiate (objCard) as GameObject;
			obj.name = "Card" + i.ToString ();
			obj.SetActive (true);

			RectTransform rt = (RectTransform)obj.transform;
			rt.SetParent (grid.transform);
			rt.InitTransform ();

			if (i % 2 == 0) {
				++pairNum;
				tex = _GetRandCardImg ();
			}

			CardElt elt = rt.GetComponent<CardElt> ();
			elt.SetIdx (i, pairNum);
			elt.SetImg (tex);

			prev_pairNum = pairNum;

			listElt.Add (elt);
		}


		_MixCards ();
	}

	Texture _GetRandCardImg()
	{
		Texture tex = null;
		System.Random random = new System.Random ();
		for (int i = 0; i < listCards.Count; i++) {
			int rand = random.Next (listCards.Count);
			tex = listCards [rand];
			listCards.RemoveAt (rand);
			break;
		}

		return tex;
	}

	void _MixCards()
	{
		System.Random random = new System.Random ();
		for (int i = 0; i < listElt.Count; i++) {
			int rand = random.Next (listElt.Count);
			listElt[i].transform.SetSiblingIndex (rand);
		}
	}		

	void Update()
	{		
		float elapsed = countDown.GetElapsed ();
		float fill = (LIMIT_TIME - elapsed) / (float)LIMIT_TIME;
		imgTime.fillAmount = fill;
	}

	List<KeyValuePair<int,int>> listChecks = new List<KeyValuePair<int, int>>();
	public void CheckPair(int idx, int pairNum)
	{
		for (int i = 0; i < listElt.Count; i++) {
			if (listElt [i].idx == idx && listElt [i].objImg.activeSelf)
				return;
		}
			
		for (int i = 0; i < listChecks.Count; i++) {
			if (listChecks [i].Key == idx)
				return;
		}

		listChecks.Add (new KeyValuePair<int, int> (idx, pairNum));

		for (int i = 0; i < listChecks.Count / 2; i++) {
			int check = i * 2;
			KeyValuePair<int,int> first = listChecks [check];
			KeyValuePair<int,int> second = listChecks [check + 1];

			if (first.Value == second.Value)
				_CheckAllPair ();
			else
				StartCoroutine (_DelayedHide (first.Key, second.Key));
		}
	}

	void _CheckAllPair()
	{
	}

	IEnumerator _DelayedHide(int firstIdx, int secondIdx)
	{
		yield return new WaitForSeconds (1f);

		listElt [firstIdx].Hide ();
		listElt [secondIdx].Hide ();

		for (int i = 0; i < listChecks.Count; i++) {
			if (listChecks [i].Key == firstIdx ||
			    listChecks [i].Key == secondIdx) {
				listChecks.RemoveAt (i);
				--i;
			}
		}
	}

	public void ReturnHome()
	{
		SceneChanger.LoadScene ("Game", objBoard);
	}
}
