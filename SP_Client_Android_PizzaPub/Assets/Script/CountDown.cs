using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class CountDown : MonoBehaviour {

	public bool isShowZero = false;

	Text count;
	int sec = 0;
    float elapsed = 0f;
	UnityAction callback = null;

	void Awake()
	{
		count = GetComponent<Text> ();
	}

	public void Set(int sec, UnityAction callback = null)
	{
		this.sec = sec;
		this.callback = callback;

		StopAllCoroutines ();
		StartCoroutine (_CountDown ());
	}

	IEnumerator _CountDown()
	{
		count.text = sec.ToString ();

		float timeToStart = Time.timeSinceLevelLoad;
		while (true) {
			elapsed = Time.timeSinceLevelLoad - timeToStart;
			int remain = Mathf.CeilToInt (Mathf.Max (0f, sec - elapsed));

			string str = remain.ToString ();
			if (count.text != str)
				count.text = str;

			if (remain <= 0)
				break;

			yield return null;
		}

		if (isShowZero == false)
			count.text = "";

		if (callback != null) {
			callback ();
			callback = null;
		}
	}

	public void Stop() { StopAllCoroutines (); }
    public float GetElapsed() { return elapsed; }
}
