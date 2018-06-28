using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class CountDown : MonoBehaviour {

	Text count;
	int sec = 0;
	UnityAction callback = null;

	void Awake()
	{
		count = GetComponent<Text> ();
	}

	public void Set(int sec, UnityAction callback)
	{
		this.sec = sec;
		this.callback = callback;
		StartCoroutine (_CountDown ());
	}

	IEnumerator _CountDown()
	{
		count.text = sec.ToString ();

		float timeToStart = Time.timeSinceLevelLoad;
		while (true) {
			float elapsed = Time.timeSinceLevelLoad - timeToStart;
			int remain = Mathf.CeilToInt (Mathf.Max (0f, sec - elapsed));

			string str = remain.ToString ();
			if (count.text != str)
				count.text = str;

			if (remain <= 0)
				break;

			yield return null;
		}

		count.text = "";

		if (callback != null) {
			callback ();
			callback = null;
		}
	}
}
