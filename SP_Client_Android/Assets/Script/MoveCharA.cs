using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCharA : MonoBehaviour {
	public RectTransform character;

	void Start()//스타트 코루틴
	{
		StartCoroutine(Move());
	}

	IEnumerator Move()
	{
		character.anchoredPosition = new Vector2(0f, 30f);//rt 로고를 정의해주기, 시작하는 위치설정 

		while (true)
		{
			float posX = character.anchoredPosition.x;//변수들을 정의
			float elapsed = Time.deltaTime *10f;//초당움직이는 프레임, 프레임은 유니티내부에서 설정 가능
			float moveX = Mathf.MoveTowards(posX, 150f, elapsed);//목표방향,계속 쏴주기

			character.anchoredPosition = new Vector2(moveX, 30f);

			if (character.anchoredPosition.x >= 150f)//목표지점까지가면끝입니다
				break;

			yield return null;
		}

		Debug.Log("End Move");
		StartCoroutine(Move());
	}
}