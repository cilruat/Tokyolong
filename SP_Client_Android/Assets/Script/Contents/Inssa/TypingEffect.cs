using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypingEffect : MonoBehaviour {


	public Text tx;
	private string m_text = "왜들 그리 다운돼있어? 뭐가 문제야 say something ";







	void Start () {

		StartCoroutine (_typing ());
	}
	
	IEnumerator _typing()
	{
		while (true) {
			yield return new WaitForSeconds (4f);
			for (int i = 0; i < m_text.Length; i++) {
				tx.text = m_text.Substring (0, i);

				yield return new WaitForSeconds (0.1f);

			}
		}
	}



}
