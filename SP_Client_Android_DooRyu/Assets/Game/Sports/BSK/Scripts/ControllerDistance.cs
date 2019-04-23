using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent (typeof (GameController))]
public class ControllerDistance : MonoBehaviour {
	
	public GameObject startPanel;
	
	public int startDistance = 1;
	public int distanceStep = 1;
	public Text distanceTxt;
	public Text bestDistanceTxt;
	public Text plusDistanceTxt;
	
	public bool debugAim;
	
	private Shooter shooter;
	
	private int distance;
	private int lastRecord;
	private bool hitRecord;
	private Vector3 newBallPos;
	
	
	
	void OnEnable(){
		Ball.OnGoal += Goal;
        Ball.OnFail += Fail;
	}
	
	void OnDisable(){
		Ball.OnGoal -= Goal;
        Ball.OnFail -= Fail;
	}
	
	void Awake(){
		Shooter.aimDotsNum = 60;
	}
	
	void Start(){
		shooter = GameObject.Find("Shooter").GetComponent<Shooter>();
		ResetData();
		UpdateAimDotsNum();
		shooter.newBallPosition = newBallPos;
	}
	
	void ShowStartPanel(){
		if(PlayerPrefs.GetInt("distanceHideHelp",0)== 0)
			startPanel.SetActive(true);
		else
			GameController.data.StartPlay();
	}
	
	public void toggleNoShowAgain(){
		PlayerPrefs.SetInt("distanceHideHelp", PlayerPrefs.GetInt("distanceHideHelp",0)== 1 ? 0 : 1);
	}
	
	void HideStartPanel(){
		if(startPanel.activeInHierarchy)
			startPanel.SetActive(false);
	}
	
	void Goal(float distance, float height, bool floored, bool clear, bool special){
		if(clear) {
			if(special || floored) {
				UpdateDistance(distanceStep*4);
				SoundController.data.playClearSpecialGoal();
			} else
				UpdateDistance(distanceStep*2);
			SoundController.data.playClearGoal();
				
		} else {
			if(special || floored) {
				UpdateDistance(distanceStep*2);
				SoundController.data.playClearSpecialGoal();
			}	
			else
				UpdateDistance(distanceStep);
			SoundController.data.playGoal();
		}
	}
	
	void Fail(){
		UpdateDistance(-distanceStep);
	}

	public void UpdateDistance(int deltaDistance) {
		distance += deltaDistance;
		distanceTxt.text = distance.ToString()+"m";
		if(deltaDistance > 0) {
			plusDistanceTxt.text = "+"+deltaDistance+"m";
			plusDistanceTxt.color = Color.yellow;
		} else {
			plusDistanceTxt.text = deltaDistance.ToString()+"m";
			plusDistanceTxt.color = Color.red;
		}
		
		plusDistanceTxt.gameObject.SetActive(true);
		float newXpos = distance * 2.5f;
		newBallPos = new Vector3(newXpos, Random.Range(3.0f,7.0f), 0);
		shooter.newBallPosition = newBallPos;
		if(distance >= 1)
			shooter.spawnBall();
		else
			GameController.data.Complete();
		
		if(distance > PlayerPrefs.GetInt("distanceBestScore",0)){
			bestDistanceTxt.text = "RECORD - " +distance.ToString()+"m";
			PlayerPrefs.SetInt("distanceBestScore",distance);
			if(lastRecord > 0 && !hitRecord) {
				HitNewRecord();
			}
		}
		UpdateAimDotsNum();
	}
	
	public void HitNewRecord(){
		bestDistanceTxt.color = Color.yellow;
		bestDistanceTxt.gameObject.GetComponent<Transformer>().ScaleImpulse(new Vector3(1.3f,1.3f,1), 0.4f, 1);
		SoundController.data.playNewRecord();
		hitRecord = true;
	}
	
	public void UpdateAimDotsNum(){
		if(debugAim)
			Shooter.aimDotsNum = 60;
		else
			Shooter.aimDotsNum = distance+7;
	}
	
	public void ResetData(){
		distance = startDistance;
		if(startDistance > 1)
			newBallPos = new Vector3(startDistance*2, Random.Range(3.0f,7.0f), 0);
		else
			newBallPos = new Vector3(0, Random.Range(3.0f,7.0f), 0);
		distanceTxt.text = this.distance.ToString()+"m";
		lastRecord = PlayerPrefs.GetInt("distanceBestScore",1);
		bestDistanceTxt.text = "RECORD - " +lastRecord+"m";
	}
}
