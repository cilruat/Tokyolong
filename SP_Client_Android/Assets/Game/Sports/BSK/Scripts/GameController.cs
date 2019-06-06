using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace BSK
{

public class GameController : MonoBehaviour {

	public static GameController data;
	
	public GameObject completePanel;
	public GameObject pausePanel;
	public Text maxHeightTxt;
	private Shooter shooter;
	public bool isPlaying;
	public enum State {InGame, Paused, Complete, StartUp}
	public State gameState;

	
	void Awake () {
		data = this;
		Time.timeScale = 1.5f;
		shooter = GameObject.Find("Shooter").GetComponent<Shooter>();
		BroadcastMessage("ShowStartPanel");
		gameState = State.StartUp;
		AudioListener.volume = PlayerPrefs.GetInt("sound", 1);
	}
	
	void Start(){
		shooter.inverseAim = PlayerPrefs.GetInt("inverseAim", 0) == 1 ? true : false;
	}
	
	void Update () {
		if(AdaptiveCamera.extraMode) {
			maxHeightTxt.text = (shooter.currentBall.GetComponent<Ball>().maxHeight).ToString("F2")+" M";
		}
		
		if (Input.GetKeyDown("escape"))
			togglePause();
		if (Input.GetKey(KeyCode.R))
            Application.LoadLevel(Application.loadedLevel);
	}
	
	public void StartPlay(){
		isPlaying = true;
		gameState = State.InGame;
		shooter.spawnBall();
		BroadcastMessage("HideStartPanel");
		AdaptiveCamera.extraMode = false;
	}
	
	public void Complete(){
		completePanel.SetActive(true);
		isPlaying = false;
		gameState = State.Complete;
		SoundController.data.playGameOver();
	}
	
	public void Restart(){
		Application.LoadLevel(Application.loadedLevel);
	}
	
	public void togglePause(){
		if(gameState == State.StartUp || gameState == State.Complete)
			return;
		isPlaying = !isPlaying;
		pausePanel.SetActive(!isPlaying);
		Time.timeScale = isPlaying ? 1.5f : 0;
		gameState = isPlaying ? State.InGame : State.Paused;
	}
	
	public void loadMenu(){
		Time.timeScale = 1.5f;
		Application.LoadLevel("Menu");
	}
	
	public void switchAim(){
		shooter.inverseAim =! shooter.inverseAim;
	}
	
	public void switchMaxHeightUI(){
		maxHeightTxt.gameObject.transform.parent.gameObject.SetActive(AdaptiveCamera.extraMode);
	}
	
	public void ClearPlayerPrefs(){
		PlayerPrefs.DeleteAll();
		Application.LoadLevel(Application.loadedLevel);
	}
	}
}
