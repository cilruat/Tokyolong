using UnityEngine;
using System.Collections;

public class PinBall_Shooter : MonoBehaviour {

	public GameObject _pinball;
	// Use this for initialization
	void Start () {
	}

	void GameStart(int ballnum){
		StartCoroutine(Shoot(ballnum));		
	}

	IEnumerator Shoot(int count){
		for(int i =0;i<count; i++){
			int forceX = UnityEngine.Random.Range(-0,-200);
			int forceY = UnityEngine.Random.Range(50,100);

			GameObject ball = (GameObject)Instantiate(_pinball);
			ball.transform.localPosition = this.transform.localPosition;
			ball.GetComponent<Rigidbody2D>().AddForce(new Vector2(forceX,forceY));
			yield return new WaitForSeconds(0.2f);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
