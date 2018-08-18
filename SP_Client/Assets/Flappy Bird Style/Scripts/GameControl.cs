using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour 
{
	const int EASY_LIMIT_TIME = 20;
	const int HARD_LIMIT_TIME = 40;
	const float MAX_SPEED = 7f;
	int finishLimitTime = 0;
	
	public static GameControl instance;			//A reference to our game control script so we can access it statically.
	public Text scoreText;						//A reference to the UI text component that displays the player's score.

	private int score = 0;						//The player's score.
	public bool gameOver = false;				//Is the game over?
	public float scrollSpeed = -3f;

	public Bird bird;
	public bool isStart = false;

	public GameObject objStart;
	public Text txtTime;
	public Image imgTime;
	public CountDown limitTime;
	public RawImage imgVictory;
	public GameObject objVictory;
	public GameObject objSendServer;
	public GameObject objGameOver;
	public GameObject objReady;
	public GameObject objGo;
	public GameObject objBoard;

	void Awake()
	{
		//If we don't currently have a game control...
		if (instance == null)
			//...set this one to be it...
			instance = this;
		//...otherwise...
		else if(instance != this)
			//...destroy this one because it is a duplicate.
			Destroy (gameObject);
	}

	void Update()
	{
		if (GameControl.instance && GameControl.instance.isStart == false)
			return;
		
		scrollSpeed -= (Time.deltaTime * .1f);

		float elapsed = limitTime.GetElapsed();
		float fill = (finishLimitTime - elapsed) / (float)finishLimitTime;
		imgTime.fillAmount = fill;

		//If the game is over and the player has pressed some input...
		/*if (gameOver && Input.GetMouseButtonDown(0)) 
		{
			//...reload the current scene.
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}*/
	}		

	public void OnStart()
	{
		StartCoroutine (_Start ());
	}

	IEnumerator _Start()
	{		
		UITweenAlpha.Start (objStart, 1f, 0f, TWParam.New (.5f).Curve (TWCurve.CurveLevel2).DisableOnFinish ());
		yield return new WaitForSeconds (.5f);

		UITweenAlpha.Start(objReady, 0f, 1f, TWParam.New(.5f).Curve(TWCurve.CurveLevel2));

		yield return new WaitForSeconds (1f);
		UITweenAlpha.Start(objReady, 1f, 0f, TWParam.New(.5f).Curve(TWCurve.CurveLevel2));

		yield return new WaitForSeconds (.25f);
		UITweenAlpha.Start(objGo, 0f, 1f, TWParam.New(.5f).Curve(TWCurve.CurveLevel2));

		yield return new WaitForSeconds (1f);
		UITweenAlpha.Start(objGo, 1f, 0f, TWParam.New(.5f).Curve(TWCurve.CurveLevel2));

		yield return new WaitForSeconds (.3f);

		isStart = true;
		bird.SetGravity (.66f);

		finishLimitTime = Info.GameDiscountWon == (short)EDiscount.e1000won ? EASY_LIMIT_TIME : HARD_LIMIT_TIME;
		txtTime.text = finishLimitTime.ToString ();

		limitTime.transform.parent.gameObject.SetActive (true);
		limitTime.Set (finishLimitTime, () => StartCoroutine (_SuccessEndGame ()));
	}

	IEnumerator _SuccessEndGame ()
	{
		// show sendserver obj
		gameOver = true;
		bird.SetGravity (0f);
		ShiningGraphic.Start (imgVictory);
		objVictory.SetActive (true);
		yield return new WaitForSeconds (4f);

		UITweenAlpha.Start (objVictory, 1f, 0f, TWParam.New (.5f).Curve (TWCurve.CurveLevel2));
		UITweenAlpha.Start (objSendServer, 0f, 1f, TWParam.New (.5f).Curve (TWCurve.CurveLevel2));

		yield return new WaitForSeconds (1f);

		if (Info.TableNum == 0)
			ReturnHome ();
		else
			NetworkManager.Instance.Game_Discount_REQ (Info.GameDiscountWon);
	}

	public void BirdScored()
	{
		//The bird can't score if the game is over.
		if (gameOver)	
			return;
		//If the game is not over, increase the score...
		score++;
		//...and adjust the score text.
		scoreText.text = "Score: " + score.ToString();
	}

	public void BirdDied()
	{
		//Activate the game over text.
		objGameOver.SetActive (true);
		//Set the game to be over.
		gameOver = true;

		limitTime.Stop ();
	}

	public void ReturnHome()
	{
		SceneChanger.LoadScene ("Main", objBoard);
	}
}
