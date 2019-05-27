using UnityEngine;
using System.Collections;

/// <summary>
// Slot Cylinder
/// </summary>
public class SlotCylinder : MonoBehaviour {
	float _rotPower=0;
	float _toAngle=0;
	bool _match = false;

	GameObject target;

	//Play Rotating!
	public void Play(GameObject btn,float waitSec){
		target = btn;
		StartCoroutine(Shoot(waitSec));		
	}

	//Time schedule
	// 1 step just wait.
	// 2 step rotating powerfully.
	// 3 step lose power.
	// 4 step select images.
	IEnumerator Shoot(float sec){
		int select_slot = UnityEngine.Random.Range(0,8);
		yield return new WaitForSeconds(sec);

		_match = false;
		_rotPower = 1000f;

		yield return new WaitForSeconds(0.6f);

		float st = UnityEngine.Random.Range(0,300)/10000f;
		while(_rotPower >= 100f){
			_rotPower *= 0.96f - st;
			yield return new WaitForSeconds(0.1f);
		}

		_toAngle = select_slot * 45f;
		_match = true;
	}

	// Update is called once per frame
	void Update () {
		if (_match){
			float angle = Quaternion.Angle(transform.rotation, Quaternion.Euler(_toAngle, 0, 90));
			if(angle < 5){
				transform.rotation = Quaternion.Euler(_toAngle, 0, 90);
				_rotPower = 0;
				_match = false;

				finished();
			}
		}
		if(_rotPower > 0){
			transform.Rotate(Vector3.up * Time.deltaTime * _rotPower);
		}
	}

	// get image index with rotate information.
	int getIndex(){
		int x = (int)transform.eulerAngles.x;
		int y = (int)transform.eulerAngles.y;
		int z = (int)transform.eulerAngles.z;

		if(x == 45 && y ==  0 && z == 90 ) return 0; // banana;
		if(x ==315 && y ==180 && z ==270 ) return 1; // waterm;
		if(x == 45 && y ==180 && z ==270 ) return 2; // cherry;
		if(x ==270 && y ==90  && z ==  0 ) return 3; // straw;
		if(x ==  0 && y ==180 && z ==270 ) return 4; // orange;
		if(x == 90 && y ==270 && z ==  0 ) return 5; // bar;
		if(x ==315 && y ==  0 && z == 90 ) return 6; // 7;
		if(x ==  0 && y ==  0 && z == 90 ) return 7; // 777;
		return -1;
	}

	// rotating finish!
	void finished(){
		target.SendMessage("SlotFinished",getIndex());
	}
}
