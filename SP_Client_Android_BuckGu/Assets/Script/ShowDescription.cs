using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowDescription : MonoBehaviour {

	[System.Serializable]
	public class ShowPoint
	{
		public int showIdx = -1;
		public GameObject[] obj;
	}

	public GameObject[] desc;
	public List<ShowPoint> listPoint = new List<ShowPoint> ();

	void Awake()
	{
		for (int i = 0; i < desc.Length; i++)
			desc [i].SetActive (false);

		for (int i = 0; i < listPoint.Count; i++)
			for (int j = 0; j < listPoint [i].obj.Length; j++)
				listPoint [i].obj [j].SetActive (false);
	}

	IEnumerator Start()
	{
		if (desc == null || desc.Length == 0)
			yield break;

		yield return new WaitForSeconds (.5f);

		for (int i = 0; i < desc.Length; i++) {
			UITweenAlpha.Start (desc [i], 0f, 1f, TWParam.New (1f).Curve (TWCurve.CurveLevel2));

			ShowPoint p = listPoint.Find (o => o.showIdx - 1 == i);
			if (p != null) {
				int idx = 1;
				foreach (GameObject g in p.obj) {
					UITweenAlpha.Start (g, 0f, 1f, TWParam.New (1f, idx * .35f).Curve (TWCurve.CurveLevel2));
					++idx;
				}
			}

			yield return new WaitForSeconds (.5f);
		}
	}
}
