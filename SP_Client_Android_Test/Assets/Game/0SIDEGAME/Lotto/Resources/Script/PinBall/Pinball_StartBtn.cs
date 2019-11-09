using UnityEngine;
using System.Collections;

/// <summary>
// Pachinko Start Button
/// </summary>
public class Pinball_StartBtn : MonoBehaviour {
	int[] _boxesNum = new int[5]{0,0,0,0,0}; 

	//is now playing
	bool isPlay(){
		GameObject[] balls = GameObject.FindGameObjectsWithTag("PinBall");
		return balls.Length > 0;
	}

	//clear box information 
	void ClearBoxes(){
		for(int i=0;i<_boxesNum.Length;i++){
			_boxesNum[i] = 0;
		}
	}

	// processing Goal in
	void GoalInBall(int box){
		_boxesNum[box-1]++;
		if(GameObject.FindGameObjectsWithTag("PinBall").Length <= 1){
			PinBall_ResultWIndow window = (PinBall_ResultWIndow)((GameObject)Instantiate(Resources.Load("prefab/PinBall/PinBallResultWindow"))).GetComponent<PinBall_ResultWIndow>();
			int give = window.result(_boxesNum);

			PlayerMeta.incraseGold(give);

		}
	}

	// Play Pachinko!
	void OnMouseClicked(){
		if(isPlay())return ;
		ClearBoxes();

		string toggle = "";
		GameObject[] btns = GameObject.FindGameObjectsWithTag("BetBtn");
		for(int i =0;i<btns.Length;i++){
			if(btns[i].GetComponent<Pinball_BetBtn>().toggle){
				toggle = btns[i].transform.name;
			}
		}

		if(toggle.Length == 0){
			AlertWindow window = (AlertWindow)((GameObject)Instantiate(Resources.Load("prefab/Alert"))).GetComponent<AlertWindow>();
			window.title.text = "Pin&Ball";
			window.text.text = "You need selecting bet coin";
		}else{
			int num = 0;
			if(toggle == "Bet1"){
				num = 1;
			}else if(toggle == "Bet10"){
				num = 10;
			}else if(toggle == "Bet50"){
				num = 50;
			}

			if(PlayerMeta.GetGold() >= num){
				PlayerMeta.decreaseGold(num);

				transform.localScale = new Vector3(1.4f,1.4f,1);
				GameObject shooter = GameObject.Find("Shooter");
				shooter.SendMessage("GameStart",num);
			}else{
				AlertWindow window = (AlertWindow)((GameObject)Instantiate(Resources.Load("prefab/Alert"))).GetComponent<AlertWindow>();
				window.title.text = "Pin&Ball";
				window.text.text = "You need more gold";
			}
		}
	}

	void OnMouseRelease(){
		transform.localScale = new Vector3(1.5f,1.5f,1);
	}

	void OnMouseOver(){
		transform.localScale = new Vector3(1.6f,1.6f,1);
	}

	void OnMouseOut(){
		transform.localScale = new Vector3(1.5f,1.5f,1);
	}
}
