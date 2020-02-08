using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
// Lotto Main System.
/// </summary>
public class LottoPicker : MonoBehaviour {
	enum STATUS{
		SELECT_BALLS,
		PLAYING,
		RESULT
	};

	private STATUS _status = STATUS.SELECT_BALLS; 
	private bool _start = false;
	private bool _wait = false;

	private Transform _left;
	private Transform _right;
	private Transform _center;
	private Transform _top;
	private Transform _middle;

	private List<GameObject> _balls = new List<GameObject>();
	private List<int> _pickedBalls = new List<int>();
	private List<int> _selectBalls;

	// Use this for initialization
	void Start() {
		_left 	= transform.Find("Left");
		_right 	= transform.Find("Right");
		_center = transform.Find("Center");
		_middle = transform.Find("Middle");
		_top 	= transform.Find("Top");

		Instantiate(Resources.Load("Prefab/Lotto/SelectBalls"));
	}

	//Play function called from Lotto Select window
	public void Play(List<int> list){
		PlayerMeta.decreaseGold(50);

		_selectBalls = list;
		GameObject[] select_go = GameObject.FindGameObjectsWithTag("LottoMyPick");
		for(int i =0;i<3;i++){
			string path = "Texture/Lotto_"+_selectBalls[i];

			select_go[i].transform.localScale = new Vector3(1,1,1);

			SpriteRenderer spr = select_go[i].GetComponent<SpriteRenderer>();
			spr.sprite = Resources.Load<Sprite>(path);
		}

		foreach(GameObject b in _balls){
			Destroy(b);
		}
		_balls.Clear();
		_pickedBalls.Clear();

		for(int i=1;i<=20;i++){
			GameObject ball = (GameObject)Instantiate(Resources.Load("Prefab/Lotto/Ball"));
			ball.transform.localPosition = new Vector3(UnityEngine.Random.Range(0,100)/1000f,0,-1);
			ball.GetComponent<LottoBall>().setType(i);

			_balls.Add(ball);
		}
		_status = STATUS.PLAYING;
	}

	//Just Timer
	IEnumerator Timer(){
		yield return new WaitForSeconds(2);
		_start = true;
		_wait = false;
	}

	// save picked ball, and 3 times picked go to result.  
	public void Picked(int type){
		GameObject ball = (GameObject)Instantiate(Resources.Load("Prefab/Lotto/PickedBall"));
		ball.transform.localPosition = new Vector3(8,-0.7f,-1);
		ball.GetComponent<LottoBall>().setType(type);

		_pickedBalls.Add(type);
		if(_pickedBalls.Count == 3){
			_status = STATUS.RESULT;
			
			GameObject result = (GameObject)Instantiate(Resources.Load("Prefab/Lotto/LottoResultWindow"));
			result.GetComponent<LottoResultWindow>().result(_selectBalls,_pickedBalls);
		}
	}

	// picker move up and down!
	float speed_Ratio = 0.5f;
	void Update () {
		if(_status == STATUS.PLAYING){
			if(_start){
				_left.localScale = new Vector3(_left.localScale.x,_left.localScale.y+0.05f*speed_Ratio,1);
				_right.localScale = new Vector3(_right.localScale.x,_right.localScale.y+0.05f*speed_Ratio,1);
				_center.localScale = new Vector3(_center.localScale.x,_center.localScale.y+0.05f*speed_Ratio,1);

				_top.localPosition = new Vector3(_top.localPosition.x,_top.localPosition.y+0.024f*speed_Ratio,-1);
				_middle.localScale = new Vector3(_middle.localScale.x,_middle.localScale.y+0.075f*0.96f*speed_Ratio,1);
				_middle.localPosition = new Vector3(_middle.localPosition.x,_middle.localPosition.y+0.0125f*0.96f*speed_Ratio,-1);

				if(_center.localScale.y > 15){
					_start = false;			
				}
			}else{
				if(_center.localScale.y <= 0){
					if(_wait == false)
						StartCoroutine("Timer");
					_wait = true;
				}else{
					_left.localScale = new Vector3(_left.localScale.x,_left.localScale.y-0.05f*speed_Ratio,1);
					_right.localScale = new Vector3(_right.localScale.x,_right.localScale.y-0.05f*speed_Ratio,1);
					_center.localScale = new Vector3(_center.localScale.x,_center.localScale.y-0.05f*speed_Ratio,1);
				
					_top.localPosition = new Vector3(_top.localPosition.x,_top.localPosition.y-0.024f*speed_Ratio,-1);
					_middle.localScale = new Vector3(_middle.localScale.x,_middle.localScale.y-0.075f*0.96f*speed_Ratio,1);
					_middle.localPosition = new Vector3(_middle.localPosition.x,_middle.localPosition.y-0.0125f*0.96f*speed_Ratio,-1);
				}
			}
		}
	}
}
