using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flight : MonoBehaviour {

	public Rigidbody2D rigid;
	public PageAvoidBullets parent;

	bool isMove = false;
	Vector2 move;
	RectTransform rt;

	void Awake() { rt = (RectTransform)transform; }

	void Update()
	{
		#if UNITY_EDITOR
		_KeyInput ();
		#endif

		if (isMove) {
			float posX = Mathf.Clamp (rt.anchoredPosition.x, 
				             parent.rtBox.rect.xMin + 25f, parent.rtBox.rect.xMax - 25f);
			rt.anchoredPosition = new Vector2 (posX, rt.anchoredPosition.y);
		}

		rigid.velocity = move * parent.SPEED_FLIGHT;
	}

	void _KeyInput()
	{
		float x = Input.GetAxisRaw ("Horizontal");
		move = new Vector2 (x, 0).normalized;
		isMove = x != 0;
	}

	public void Move(float x)
	{
		isMove = x != 0;
		move = new Vector2 (x, 0).normalized;
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		_DestroyEff ();
		Destroy (gameObject);

		parent.FailEndGame ();
	}

	void _DestroyEff()
	{
		
	}
}
