using UnityEngine;
using System.Collections;

//This script changes camera size position each new throw and turns to camera extra modes
public class AdaptiveCamera : MonoBehaviour {
	
	public Color[] randomColors;
	public Transform leftSidePivot;
	private static float currentLerpTime;
	private float resizeTime = 1;
	private static bool needResize;
	private static float fromSize, toSize, fromX, toX;
	public enum Mode {Off, UnderBoard, FollowBall, RandomChoice}
	public Mode CamExtraMode;
	public static bool extraMode;
	private static Camera thisCamera;
	private Shooter shooter;
	private Transform basket;
	private int randomInt;
	private Vector3 lastPos;
	private Quaternion lastRot;
	private static bool colorChanged;
	
	void Start () {
		thisCamera = GetComponent<Camera>();
		shooter = GameObject.Find("Shooter").GetComponent<Shooter>();
		basket = GameObject.Find("ring").GetComponent<Transform>();
		extraMode = false;
	}
	
	
	void Update () {
		if(!GameController.data.isPlaying) {
			if(extraMode && GameController.data.gameState != GameController.State.Paused)
				GoToNormal();
			return;
		}
			
		if(shooter.currentBall.GetComponent<Ball>().special && shooter.currentBall.GetComponent<Ball>().clear && CamExtraMode != Mode.Off && shooter.currentBall != null && !extraMode) {
			extraMode = true;
			lastPos = transform.position;
			lastRot = transform.rotation;
			thisCamera.orthographic = false;
			GameController.data.switchMaxHeightUI();
			switch(CamExtraMode){
				case Mode.UnderBoard:
					thisCamera.fieldOfView = 95;
				break;
				case Mode.FollowBall:
					thisCamera.fieldOfView = 45;
				break;
				case Mode.RandomChoice:
					randomInt = Random.Range(0,2);
				break;
			}
		}
		
		if(!extraMode) {
			if(needResize)
				UpdateCamSize();
			Vector3 delta = thisCamera.WorldToViewportPoint(leftSidePivot.position);
			transform.position = new Vector3(transform.position.x+(delta.x*20),transform.position.y,transform.position.z);
		} else {
			switch(CamExtraMode){
				case Mode.UnderBoard:
					UpdateUnderBoardCam();
				break;
				case Mode.FollowBall:
					UpdateFollowBallCam();
				break;
				case Mode.RandomChoice:
					if(randomInt == 1)
						UpdateUnderBoardCam();
					else
						UpdateFollowBallCam();
				break;
			}
		}
	}
	
	void UpdateUnderBoardCam(){
		if(shooter.currentBall.transform.position.y > basket.position.y-0.5f && !shooter.currentBall.GetComponent<Ball>().failed) {
			transform.position = new Vector3(-6.5f,1.4f,-2.3f);
			transform.LookAt(shooter.currentBall.transform);
		} else {
			GoToNormal();
		}
	}
	
	void UpdateFollowBallCam(){
		if(shooter.currentBall.transform.position.y > basket.position.y-0.5f && !shooter.currentBall.GetComponent<Ball>().failed) {
			transform.position = new Vector3(shooter.currentBall.transform.position.x+2, shooter.currentBall.transform.position.y*1.1f,0);
			if(transform.position.x > basket.transform.position.x)
				transform.LookAt(basket);
			SoundController.data.playBallInWind();
		} else {
			GoToNormal();
		}
	}
	
	void GoToNormal(){
		SoundController.data.Stop();
		extraMode = false;
		thisCamera.orthographic = true;
		transform.position = lastPos;
		transform.rotation = lastRot;
		GameController.data.switchMaxHeightUI();
	}
	
	public static void ResizeCam(float newSize){
		newSize = Mathf.Clamp(newSize, 4, 50);
		fromSize = thisCamera.orthographicSize;
		toSize = newSize;
		needResize = true;
		colorChanged = false;
		currentLerpTime = 0;
	}
	
	public static void ResizeCam(Vector3 ballPos){
		Vector3 viewPos = thisCamera.WorldToViewportPoint(ballPos);
		ResizeCam(thisCamera.orthographicSize*(viewPos.x * 1.05f));
		
	}
	
	void UpdateCamSize(){
		currentLerpTime += Time.deltaTime;
		if(currentLerpTime > resizeTime)
			currentLerpTime = resizeTime;
		
		float t = currentLerpTime/resizeTime;
		t = Mathf.Sin(t * Mathf.PI * 0.5f);
		thisCamera.orthographicSize = Mathf.Lerp(fromSize, toSize, t);
		if(t >= 1)
			needResize = false;
		if(!colorChanged){
			Color newCol = randomColors[Random.Range(0, randomColors.Length)];
			Camera.main.backgroundColor = newCol;
			colorChanged = true;
		}
	}
}
