using UnityEngine;
using System.Collections;

/// <summary>
// Lotto Ball.
// Ball has physics component.
/// </summary>
public class LottoBall : MonoBehaviour {
	private int _type=0;

	// set ball number!
	public void setType(int type){
		_type = type;
		string path = "Texture/Lotto_"+type;
		Debug.Log(Resources.Load(path));
		GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(path);
	}
	
	// Update is called once per frame
	void Update () {
		if(GetComponent<Rigidbody2D>().velocity.magnitude > 10){
			GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity.normalized*5;
		}

		if(transform.localPosition.y > 6){
			LottoPicker picker = FindObjectOfType(typeof(LottoPicker)) as LottoPicker;
			picker.Picked(_type);

			Destroy(gameObject);
		}
	}

	// if ball collid air zone, adding force!
	void OnCollisionEnter2D(Collision2D collision) {
		if(collision.gameObject.name == "AirOn"){
			Vector3 dir = new Vector3(UnityEngine.Random.Range(0,100)/100f-50f,UnityEngine.Random.Range(0,100)/100f,0);
			dir = dir.normalized * 2000;
			GetComponent<Rigidbody2D>().AddForce(dir);
			if(GetComponent<Rigidbody2D>().velocity.magnitude > 10){
				GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity.normalized*5;
			}
		}
	}

}
