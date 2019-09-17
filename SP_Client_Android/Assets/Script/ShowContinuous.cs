using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowContinuous : MonoBehaviour {

	public Transform parent;

	List<GameObject> listObj = new List<GameObject> ();

	void Awake()
	{
		for (int i = 0; i < parent.childCount; i++)
			listObj.Add (parent.GetChild (i).gameObject);
	}

	IEnumerator Start()
	{
		while (true) {
			for (int i = 0; i < parent.childCount; i++) {
				GameObject obj = parent.GetChild (i).gameObject;
				UITweenAlpha.Start (obj, 0f, 1f, TWParam.New (1f).Curve (TWCurve.CurveLevel2));
				yield return new WaitForSeconds (.025f);

				UITweenAlpha.Start (obj, 1f, 0f, TWParam.New (1f, .05f).Curve (TWCurve.CurveLevel2));
			}

			yield return new WaitForSeconds (1f);
		}
	}
}