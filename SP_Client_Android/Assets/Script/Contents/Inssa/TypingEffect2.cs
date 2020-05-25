using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypingEffect2 : MonoBehaviour {

	public Text tx;
	private string m_text = "아무 게임이나 일단 틀어! 아무거나 신나는 걸로~ 아무렇게나 춤춰 ";







	void Start () {

		StartCoroutine (_typing ());
	}

	IEnumerator _typing()
	{
		while (true) {
			yield return new WaitForSeconds (6f);
			for (int i = 0; i < m_text.Length; i++) {
				tx.text = m_text.Substring (0, i);

				yield return new WaitForSeconds (0.1f);

			}
		}
	}

}
