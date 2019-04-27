using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KnifeFlip
{

public class PlayerFlipScript : MonoBehaviour {

	public static int score; //This is the score that will be displayed.
	public AudioClip ScoreHitSound; //This is the sound that will play when you get a score. (Can also be used as a HIT sound)
	public GUIStyle ScoreStyleNormal = new GUIStyle(); //This is the GUI style of the score normal.
	public GUIStyle ScoreStyleShadow = new GUIStyle(); //This is the GUI style of the score shadow (background of the score to look better).

	public Rigidbody rigidbody; //The knifes you are using rigidbody.

	public float forceSpeed = 5f; //The force speed when flipped.
	public float torqueRotation = 20f; //The torque rotation speed.

	private float timeWhenWeStartedFlying; //Time the weapon started flipping.

	private Vector2 startSwipe; //Start swipe to flip the weapon.
	private Vector2 endSwipe; //End swipe to flip the weapon.

	bool showGUI = true; //The GUI that will be displayed 
	bool showGUI2 = false; //The GUI that will be displayed 

	public AudioClip WinGameSoundAudio; //This is the sound that will play when the game ends and you lose. It will be just an audio that will play, unlike the EndGameSoundPrefab.
	public Texture WinGameFullScreen; //The GUI that will display a big texture on the full screen when you win.
	public Texture WinGameScoreBoard; //The GUI that will display scoreboard texture on the middle of the screen when you win.

	public GameObject EndGameSoundPrefab; //This is the sound that will play when the game ends and you lose. It NEEDS TO BE A PREFAB THAT WILL BE DISABLED AT START.
	public Texture EndGameFullScreen; //The GUI that will display a big texture on the full screen when you lose.
	public Texture EndGameScoreBoard; //The GUI that will display scoreboard texture on the middle of the screen when you lose.
	public GUIStyle EndGameScoreStyle = new GUIStyle();  //The GUI text of score hat will be displayed WHEN the game ends (both win game and end game).


	void Awake() {
			score = 0; //The starting score at every scene.
	}

	void Start () {
			EndGameSoundPrefab.SetActive (false); //The end game sound that will play when the game ends, in this case it will start as false so it won't play at start.
			score = 0; //The starting score at every scene.
	}

	void Update () {

		if (!rigidbody.isKinematic)
			return;

		if (Input.GetMouseButtonDown(0)) {
			startSwipe = Camera.main.ScreenToViewportPoint(Input.mousePosition);
		}

		if (Input.GetMouseButtonUp(0)) {
			endSwipe = Camera.main.ScreenToViewportPoint(Input.mousePosition);
			Swipe();
		}
	}

	void Swipe () {
		rigidbody.isKinematic = false;

		timeWhenWeStartedFlying = Time.time;

		Vector2 swipe = endSwipe - startSwipe;

		rigidbody.AddForce(swipe * forceSpeed, ForceMode.Impulse);
		rigidbody.AddTorque(0f, 0f, -torqueRotation, ForceMode.Impulse);
	}

	void OnTriggerEnter(Collider col) {
		if (col.tag == "Win") {
			showGUI2 = true;
			GetComponent<AudioSource>().PlayOneShot(WinGameSoundAudio, 7.7F);
			rigidbody.isKinematic = true;
		}

		if (col.tag == "TargetObject") {
			score++;
			rigidbody.isKinematic = true;
			GetComponent<AudioSource>().PlayOneShot(ScoreHitSound, 7.7F);
		} else {
			Restart();
		}
	}
		

	void OnCollisionEnter() {
		float timeInAir = Time.time - timeWhenWeStartedFlying;

		if (!rigidbody.isKinematic && timeInAir >= .05f) {
			//Invoke("Restart", 0f);
			Restart();
		}
	}

	void Restart () {
		showGUI = false;
		Destroy (GameObject.FindWithTag("BackgroundMusic"));
		this.gameObject.GetComponent<SphereCollider> ().enabled = false;
	}


	void OnGUI()
	{
		if (showGUI)
		{
			GUI.Label (new Rect (7, 17, 110, 110), "<b></b> " +score, ScoreStyleNormal);
			GUI.Label (new Rect (7, 17, 110, 110), "<b></b> " +score, ScoreStyleShadow);
		} 
		else
		{
			if (showGUI2)
			{
				GUI.DrawTexture(new Rect(((Screen.width / 3) - 868f), ((Screen.height / 5) - 741.5f), 2200, 1975), WinGameFullScreen, ScaleMode.ScaleToFit, true, 0.0F);
				GUI.DrawTexture(new Rect(((Screen.width / 2) - 138f), ((Screen.height / 2) - 371.5f), 300, 695), WinGameScoreBoard, ScaleMode.ScaleToFit, true, 0.0F);
				GUI.Label (new Rect(Screen.width / 2 - 48f, ((Screen.height / 2) - 34.5f), 40, -210), "Score: "+score, EndGameScoreStyle); //This will display the current score of your game.
				if (Input.GetMouseButtonDown(0)) { //If you press the mouse button
				{
					SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
				}
			}
		} 
		else
		{
			EndGameSoundPrefab.SetActive (true); 
		{
				GUI.DrawTexture(new Rect(((Screen.width / 3) - 868f), ((Screen.height / 5) - 991.5f), 3900, 2975), EndGameFullScreen, ScaleMode.ScaleToFit, true, 0.0F); //This will display a big texture on the full screen.
				GUI.DrawTexture(new Rect(((Screen.width / 2) - 138f), ((Screen.height / 2) - 371.5f), 300, 695), EndGameScoreBoard, ScaleMode.ScaleToFit, true, 0.0F); //This will display the scoreboard texture on the screen in the middle.
				GUI.Label (new Rect(Screen.width / 2 - 48f, ((Screen.height / 2) - 34.5f), 40, -210), "Score: "+score, EndGameScoreStyle); //This will display the current score of your game.

					if (Input.GetMouseButtonDown(0)) {
		{
								SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
							}
						}
					}
				}
			}
		}
	}

}