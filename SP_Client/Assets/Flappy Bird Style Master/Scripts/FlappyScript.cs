using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace FlappyBirdStyle
{
	/// <summary>
	/// Spritesheet for Flappy Bird found here: http://www.spriters-resource.com/mobile_phone/flappybird/sheet/59537/
	/// Audio for Flappy Bird found here: https://www.sounds-resource.com/mobile/flappybird/sound/5309/
	/// </summary>
	public class FlappyScript : MonoBehaviour
	{	
		const float MAX_SPEED = 5f;
		int finishLimitTime = 0;

		public static FlappyScript instance;

	    public AudioClip FlyAudioClip, DeathAudioClip, ScoredAudioClip;
	    public Sprite GetReadySprite;
	    public float RotateUpSpeed = 1, RotateDownSpeed = 1;
	    public GameObject IntroGUI, DeathGUI;
	    public Collider2D restartButtonGameCollider;
	    public float VelocityPerJump = 3;
	    public float XSpeed = 1;

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
			if (instance == null)
				instance = this;
			else if (instance != null)
				Destroy (gameObject);

			GameStateManager.GameState = GameState.Intro;
		}

	    // Use this for initialization
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

			BoostOnYAxis();
			GameStateManager.GameState = GameState.Playing;

			finishLimitTime = Info.FLAPPY_BIRD_LIMIT_TIME;
			txtTime.text = Info.practiceGame ? "∞" : finishLimitTime.ToString ();

			limitTime.transform.parent.gameObject.SetActive (true);

			if (Info.practiceGame == false)
				limitTime.Set (finishLimitTime, () => StartCoroutine (_SuccessEndGame ()));
		}

		IEnumerator _SuccessEndGame ()
		{
			GameStateManager.GameState = GameState.Dead;
			GetComponent<Rigidbody2D> ().gravityScale = 0f;
			limitTime.Stop ();

			// show sendserver obj
			ShiningGraphic.Start (imgVictory);
			objVictory.SetActive (true);
			yield return new WaitForSeconds (4f);

			GetComponent<SpriteRenderer> ().enabled = false;
			UITweenAlpha.Start (objVictory, 1f, 0f, TWParam.New (.5f).Curve (TWCurve.CurveLevel2));
			UITweenAlpha.Start (objSendServer, 0f, 1f, TWParam.New (.5f).Curve (TWCurve.CurveLevel2));

			yield return new WaitForSeconds (1f);
			UITweenAlpha.Start (objSendServer, 1f, 0f, TWParam.New (.5f).Curve (TWCurve.CurveLevel2));

			if (Info.TableNum == 0)
				ReturnHome ();
			else
				Info.ShowResult ();
		}

		public void ReturnHome()
		{
			SceneChanger.LoadScene ("Main", objBoard);
		}

	    FlappyYAxisTravelState flappyYAxisTravelState;

	    enum FlappyYAxisTravelState
	    {
	        GoingUp, GoingDown
	    }

	    Vector3 birdRotation = Vector3.zero;
	    // Update is called once per frame
	    void Update()
	    {
	        //handle back key in Windows Phone
	        /*if (Input.GetKeyDown(KeyCode.Escape))
	            Application.Quit();*/

			if (isStart == false)
				return;

	        if (GameStateManager.GameState == GameState.Intro)
	        {
	            MoveBirdOnXAxis();
	            if (WasTouchedOrClicked())
	            {
	                BoostOnYAxis();
	                GameStateManager.GameState = GameState.Playing;
	                IntroGUI.SetActive(false);
	                ScoreManagerScript.Score = 0;
	            }
	        }

	        else if (GameStateManager.GameState == GameState.Playing)
	        {
	            MoveBirdOnXAxis();
	            if (WasTouchedOrClicked())
	            {
	                BoostOnYAxis();
	            }

				if (Info.practiceGame)
					return;

				float elapsed = limitTime.GetElapsed();
				float fill = (finishLimitTime - elapsed) / (float)finishLimitTime;
				imgTime.fillAmount = fill;
	        }

	        else if (GameStateManager.GameState == GameState.Dead)
	        {
	            /*Vector2 contactPoint = Vector2.zero;

	            if (Input.touchCount > 0)
	                contactPoint = Input.touches[0].position;
	            if (Input.GetMouseButtonDown(0))
	                contactPoint = Input.mousePosition;

	            //check if user wants to restart the game
	            if (restartButtonGameCollider == Physics2D.OverlapPoint
	                (Camera.main.ScreenToWorldPoint(contactPoint)))
	            {
	                GameStateManager.GameState = GameState.Intro;
	                Application.LoadLevel(Application.loadedLevelName);
	            }*/
	        }

	    }


	    void FixedUpdate()
	    {
	        //just jump up and down on intro screen
	        if (GameStateManager.GameState == GameState.Intro)
	        {
	            if (GetComponent<Rigidbody2D>().velocity.y < -1) //when the speed drops, give a boost
	                GetComponent<Rigidbody2D>().AddForce(new Vector2(0, GetComponent<Rigidbody2D>().mass * 5500 * Time.deltaTime)); //lots of play and stop 
	                                                        //and play and stop etc to find this value, feel free to modify
	        }
	        else if (GameStateManager.GameState == GameState.Playing || GameStateManager.GameState == GameState.Dead)
	        {
	            FixFlappyRotation();
	        }
	    }

	    bool WasTouchedOrClicked()
	    {
			if (Input.GetKeyDown (KeyCode.LeftArrow) ||
			    Input.GetKeyDown (KeyCode.RightArrow) ||
			    Input.GetKeyDown (KeyCode.UpArrow) ||
			    Input.GetMouseButtonDown (0))
				return true;
	        else
	            return false;
	    }

	    void MoveBirdOnXAxis()
	    {
			if (GameStateManager.GameState == GameState.Playing) {
				XSpeed += (Time.deltaTime * .1f);
				if (XSpeed >= MAX_SPEED)
					XSpeed = MAX_SPEED;
			}

	        transform.position += new Vector3(Time.deltaTime * XSpeed, 0, 0);
	    }

	    void BoostOnYAxis()
	    {
	        GetComponent<Rigidbody2D>().velocity = new Vector2(0, VelocityPerJump);
	        GetComponent<AudioSource>().PlayOneShot(FlyAudioClip);
	    }



	    /// <summary>
	    /// when the flappy goes up, it'll rotate up to 45 degrees. when it falls, rotation will be -90 degrees min
	    /// </summary>
	    private void FixFlappyRotation()
	    {
	        if (GetComponent<Rigidbody2D>().velocity.y > 0) flappyYAxisTravelState = FlappyYAxisTravelState.GoingUp;
	        else flappyYAxisTravelState = FlappyYAxisTravelState.GoingDown;

	        float degreesToAdd = 0;

	        switch (flappyYAxisTravelState)
	        {
	            case FlappyYAxisTravelState.GoingUp:
	                degreesToAdd = 6 * RotateUpSpeed;
	                break;
	            case FlappyYAxisTravelState.GoingDown:
	                degreesToAdd = -3 * RotateDownSpeed;
	                break;
	            default:
	                break;
	        }
	        //solution with negative eulerAngles found here: http://answers.unity3d.com/questions/445191/negative-eular-angles.html

	        //clamp the values so that -90<rotation<45 *always*
	        birdRotation = new Vector3(0, 0, Mathf.Clamp(birdRotation.z + degreesToAdd, -90, 45));
	        transform.eulerAngles = birdRotation;
	    }

	    /// <summary>
	    /// check for collision with pipes
	    /// </summary>
	    /// <param name="col"></param>
	    void OnTriggerEnter2D(Collider2D col)
	    {
	        if (GameStateManager.GameState == GameState.Playing)
	        {
	            if (col.gameObject.tag == "Pipeblank") //pipeblank is an empty gameobject with a collider between the two pipes
	            {
	                GetComponent<AudioSource>().PlayOneShot(ScoredAudioClip);
	                ScoreManagerScript.Score++;
	            }
	            else if (col.gameObject.tag == "Pipe")
	            {
	                FlappyDies();
	            }
	        }
	    }

	    void OnCollisionEnter2D(Collision2D col)
	    {
	        if (GameStateManager.GameState == GameState.Playing)
	        {
	            if (col.gameObject.tag == "Floor")
	            {
	                FlappyDies();
	            }
	        }
	    }

	    void FlappyDies()
	    {
	        GameStateManager.GameState = GameState.Dead;
	        DeathGUI.SetActive(true);
			limitTime.Stop ();
	        GetComponent<AudioSource>().PlayOneShot(DeathAudioClip);
	    }

	}
}