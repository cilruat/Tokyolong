using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StartGame : MonoBehaviour {

    public float moveSpeed;

    public static StartGame startGameScript;

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

	bool isStart = false;
	int finishLimitTime = 0;

    float timeToStartGame;
    bool moveMask;
    bool timerFlag;
    bool moveUpMask;
    BoxCollider2D boxCollider;
    float maxPosY = 17f; // the maximum possible position of the mask
    float minPosY = -6f; // the minimum possible position of the mask
    float maskPosY;

    // Use this for initialization
    void Start() {
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
        gameObject.transform.position = new Vector2(gameObject.transform.position.x, maxPosY);
        maskPosY = gameObject.transform.position.y;
        startGameScript = gameObject.GetComponent<StartGame>();
    }

    // Update is called once per frame
    void Update() {
        StartMoveMask(); // The method processes the movement of the mask and is responsible for starting the game
        TimeToStartGame(); // The timer counts down the time before the game starts

		if (isStart == false)
			return;

		if (Info.practiceGame)
			return;

		float elapsed = limitTime.GetElapsed();
		float fill = (finishLimitTime - elapsed) / (float)finishLimitTime;
		imgTime.fillAmount = fill;
    }

    void StartMoveMask() {
        maskPosY = Mathf.Clamp(maskPosY, minPosY, maxPosY);
        if (moveMask) {
            if (maskPosY > minPosY && !moveUpMask) { // Move the mask down
                maskPosY -= Time.deltaTime * moveSpeed;
                if (maskPosY <= minPosY) {
                    moveUpMask = true;
                    moveMask = false;
                }
            }
            if (maskPosY < maxPosY && moveUpMask) { // Move the mask up
                maskPosY += Time.deltaTime * moveSpeed;
                if (maskPosY >= maxPosY) {
                    // Returning data to its original position, when the mask reaches its maximum height                    
                    moveUpMask = false;
                    moveMask = false;
                    //Score.scoreScript.SetMaxScore();
                    AudioManager.audioManager.PlayAudio(EnumAudioName.MAIN_MUSIC, false);
                }
            }
            gameObject.transform.position = new Vector2(gameObject.transform.position.x, maskPosY);
        }
    }

    public void HitMe(bool flag) { // Method for processing the pressure on the mask
        if (flag) {
            AudioManager.audioManager.PlayAudio(EnumAudioName.BUTTON_CLICK_MUSIC, true);
            HealPoint.healPointScript.ChangeHealPoint(4);            
            moveMask = true;
            timerFlag = true;
            timeToStartGame = 1.5f;
        }
    }

    public void LoseTheGame() {
        moveMask = true;

		limitTime.Stop ();
		objGameOver.SetActive (true);
    }


    void TimeToStartGame() { // time to the beginning of the game
        if (timerFlag) {
            if (timeToStartGame > 0) {
                timeToStartGame -= Time.deltaTime;
            }
            else {
                HealPoint.GameOver = false; // The variable is responsible for the start and end of the game
                AudioManager.audioManager.PlayAudio(EnumAudioName.MAIN_MUSIC, true);
                timerFlag = false;
            }
        }
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

		yield return new WaitForSeconds (.3f);
		HitMe (true);

		yield return new WaitForSeconds (.7f);
		UITweenAlpha.Start(objReady, 1f, 0f, TWParam.New(.5f).Curve(TWCurve.CurveLevel2));

		yield return new WaitForSeconds (.25f);
		UITweenAlpha.Start(objGo, 0f, 1f, TWParam.New(.5f).Curve(TWCurve.CurveLevel2));

		yield return new WaitForSeconds (1f);
		UITweenAlpha.Start(objGo, 1f, 0f, TWParam.New(.5f).Curve(TWCurve.CurveLevel2));

		yield return new WaitForSeconds (.3f);

		isStart = true;

		finishLimitTime = Info.CATCH_MOLE_LIMIT_TIME;
		txtTime.text = Info.practiceGame ? "∞" : finishLimitTime.ToString ();

		limitTime.transform.parent.gameObject.SetActive (true);

		if (Info.practiceGame == false)
			limitTime.Set (finishLimitTime, () => StartCoroutine (_SuccessEndGame ()));
	}

	IEnumerator _SuccessEndGame ()
	{		
		// show sendserver obj
		moveMask = true;
		limitTime.Stop ();
		HealPoint.GameOver = true;
		PlaceManager.placeManagerScript.StopGame();

		ShiningGraphic.Start (imgVictory);
		objVictory.SetActive (true);
		yield return new WaitForSeconds (4f);

		GetComponent<SpriteRenderer> ().enabled = false;
		UITweenAlpha.Start (objVictory, 1f, 0f, TWParam.New (.5f).Curve (TWCurve.CurveLevel2));
		UITweenAlpha.Start (objSendServer, 0f, 1f, TWParam.New (.5f).Curve (TWCurve.CurveLevel2));

		yield return new WaitForSeconds (1f);

		if (Info.TableNum == 0)
			ReturnHome ();
		else
			NetworkManager.Instance.Game_Discount_REQ (Info.GameDiscountWon);
	}

	public void ReturnHome()
	{		
		SceneChanger.LoadScene ("Main", objBoard);
	}
}
