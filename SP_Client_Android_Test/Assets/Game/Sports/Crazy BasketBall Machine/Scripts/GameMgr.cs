using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameMgr : MonoBehaviour {

	public GameObject shooterCam;
	public GameObject centerobject;
	public GameObject shooter;
	public GameObject GoalHolder;
	public GameObject[] points;
	//the lerp scalar for moving between the current position and the previous position.
	public float lerpTime = 5;
	private bool iswitching=false;

	public int pointindex=0;
	private int oldpointindex=100;

	private int score;
	private int best;


	private float _timeRemaining = 0.0f;
	private float _timeUpdateElapsedTime = 0.0f;

	public bool canswitchpoint=false;

	public bool islastshot=false;


	public AudioClip effectScoreofShotSound;
	public AudioClip gameoverSound;
	public AudioClip bounsSound;
	public AudioClip lastshotSound;
	public AudioClip bigscoreSound;



	public GameObject gameoverPanel;

	public GameObject levelupPanel;

	public GameObject bounsshotPanel;
	public GameObject comboPanel;



	public Text time3dtext;
	public Text bestscoretext;
	public Text leveltext;
	public Text scoretext;
	public Text targettext;

	public Text comboLabel;
	public Image comboProcess;

	public int[] targetscore; //The target score of each level. If your score greater than or equal to target score, launch the bouns shot, else game is over
	public int[] targettime; //The time of each level. You must reach the target score within the giving time in each level.
	
	public bool hidefakeball=false;
	
	private int currentlevel=0;

	private bool istimeout=false;


	private int combo=0;

	private bool iscombopanelshowed=false;

	private AutoMoveAndRotate goalholderscript;




	// Use this for initialization
	void Start () {
		Application.targetFrameRate = 60;
		Time.timeScale = 1;
		istimeout=false;;


		leveltext.text = "Level: " + (currentlevel + 1).ToString ();

		targettext.text ="Target: " + targetscore [currentlevel].ToString ();
		TimeRemaining = targettime [currentlevel]; 

		best=PlayerPrefs.GetInt("Best",0);
	
		bestscoretext.text = "Best: " + best.ToString ();
		goalholderscript = GoalHolder.GetComponent<AutoMoveAndRotate> ();



	}
	
	// Update is called once per frame
	void Update () {

        if (!istimeout) {
			_timeRemaining -= Time.deltaTime; 
		
			// accumulate elapsed time 
			_timeUpdateElapsedTime += Time.deltaTime; 
		
			// has a second past? 
			if (_timeUpdateElapsedTime >= 1.0f) {
				TimeRemaining = _timeRemaining; 
			}


			if (_timeRemaining <= 0.0f) {

				istimeout=true;

				TimeRemaining=0;

				if (score >= targetscore [currentlevel])
					PlaylastBounsShot ();
				else
					Gameover ();

				if(iscombopanelshowed)
				{comboPanel.SetActive (false);
					iscombopanelshowed=false;
					combo=0;
				}
				
			}


			if(combo>0)
			{
				comboProcess.fillAmount=comboProcess.fillAmount-Time.deltaTime*0.3f;
				if(comboProcess.fillAmount<=0)
				{
					if(iscombopanelshowed)
					{comboPanel.SetActive (false);
						iscombopanelshowed=false;
					}
					combo=0;

				}
			}

		}
	}


	IEnumerator  PrepareGameoverAD (){
		
		
		gameoverPanel.SetActive (true);

		//show full ad here
		yield return new WaitForSeconds(0.5f);

		
	}

	IEnumerator  PrepareLevelUP (){
		
		
		levelupPanel.SetActive (true);

		//show full ad here
		yield return new WaitForSeconds(0.5f);

		
		
	}
	
	private void  Gameover (){
		
		hidefakeball = true;
		shooterCam.GetComponent<Shooter> ().hideFakeBasketBall ();
		AudioSource.PlayClipAtPoint(gameoverSound, gameObject.transform.position);
		StartCoroutine(PrepareGameoverAD ());
	}


	private void levelUp()
	{
		goalholderscript.starting = false;
		Time.timeScale = 1;
		shooter.GetComponent<CamFollow> ().setTarget (null);
		//AudioSource.PlayClipAtPoint(levelupSound, gameObject.transform.position);
		StartCoroutine(PrepareLevelUP ());
		
		
		
	}


	private void PlaylastBounsShot ()
	{
		pointindex = points.Length-1;
		iswitching = true;
		canswitchpoint = true;
		hidefakeball = true;
		shooterCam.GetComponent<Shooter> ().hideFakeBasketBall ();
		AudioSource.PlayClipAtPoint(bounsSound, gameObject.transform.position);
		StartCoroutine(PrepareLastShot ());
	

	}

	IEnumerator  PrepareLastShot (){
		
		
		bounsshotPanel.SetActive (true);
		yield return new WaitForSeconds(3f);

		bounsshotPanel.SetActive (false);
		hidefakeball = false;
		islastshot = true;
		shooterCam.GetComponent<Shooter> ().showFakeBasketBall ();
	}


	public void LastShotDone()
	{
		AudioSource.PlayClipAtPoint(lastshotSound, gameObject.transform.position);
		Invoke ("levelUp", 2.5f);
		Invoke ("resetTimescale", 0.8f);
	}


	void resetTimescale()
	{
	
		Time.timeScale = 1;
	}


	// Update is called once per frame
	void LateUpdate () {
		if(iswitching&&!islastshot)
			switchPoint();
		
	}

	public void addScore()
	{

		if (islastshot) {
		
			score+=10;
		
			scoretext.text=score.ToString ();
			if (score > best) {
				
				best = score;
			
				bestscoretext.text = "Best: " + best.ToString ();
				PlayerPrefs.SetInt ("Best", best);
			}
			

		
		
		
		}



		else if (_timeRemaining > 0) {
			iswitching = true;
			canswitchpoint = true;


			combo++;
			comboProcess.fillAmount = 1;

			comboLabel.text="x"+(combo-1).ToString();

			if(combo>1&&!iscombopanelshowed)
			{comboPanel.SetActive (true);

				iscombopanelshowed=true;
			}
			
			
			score+=1+(combo-1);

			scoretext.text=score.ToString ();
			if (score > best) {
		
				best = score;
			
				bestscoretext.text = "Best: " + best.ToString ();
				PlayerPrefs.SetInt ("Best", best);
			}

		


	        if(currentlevel<5)
			{pointindex = Random.Range (0, points.Length - 1);

				if (pointindex == oldpointindex) {
					
					pointindex++;
					if (pointindex >= points.Length - 3)
						pointindex = 0;
				}
				
				oldpointindex = pointindex;
			}

			else
			{
			pointindex = Random.Range (0, points.Length - 1);

			if (pointindex == oldpointindex) {
		
				pointindex++;
				if (pointindex >= points.Length - 1)
					pointindex = 0;
			}

			oldpointindex = pointindex;
			}
		}
	}





	





	public void switchPoint()
	{

	
		canswitchpoint = false;


		shooter.transform.LookAt (centerobject.transform.position);

		//lerp between the current position and the new position
		shooter.transform.position = Vector3.Lerp(shooter.transform.position,points[pointindex].transform.position,Time.smoothDeltaTime * lerpTime);


	}



	public float TimeRemaining{
		get{
			return _timeRemaining; 
		}
		set{			
			_timeRemaining = value; 
			if(_timeRemaining<=0)_timeRemaining=0;
		
		
			time3dtext.text=_timeRemaining.ToString("00:00");
			// reset the elapsed time 
			_timeUpdateElapsedTime = 0.0f; 
		}
	}


	public void StartNextLevel(){		

		Time.timeScale = 1;
		currentlevel++;
		istimeout=false;
		islastshot = false;
		pointindex = 0;
		iswitching = true;
		canswitchpoint = true;
		hidefakeball = false;

	


		switch (currentlevel) {
		
		case 0:
		case 1:
			goalholderscript.StartMove(0);
			break;

		case 2:
		case 3:
			goalholderscript.StartMove(1);
			break;
		case 4:
		case 5:
			goalholderscript.StartMove(2);
			break;
        
		case 6:
		case 7:
			goalholderscript.StartMove(3);
			break;
		case 8:
		case 9:
			goalholderscript.StartMove(4);
			break;
		
		
		case 10:
		case 11:
			goalholderscript.StartMove(5);
			break;

		case 12:
		case 13:
			goalholderscript.StartMove(6);
			break;

		case 14:
		case 15:
			goalholderscript.StartMove(7);
			break;

		case 16:
		case 17:
			goalholderscript.StartMove(8);
			break;

		case 18:
		case 19:
			goalholderscript.StartMove(9);
			break;

		case 20:
		case 21:
			goalholderscript.StartMove(10);
			break;

		case 22:
		case 23:
		default:
			goalholderscript.StartMove(11);
			break;
		}


		
		if (currentlevel >= targettime.Length)
			currentlevel = targettime.Length;
		

		
		levelupPanel.SetActive (false);
	
		
		TimeRemaining = targettime [currentlevel]; 
		targetscore [currentlevel] = targetscore [currentlevel] + score;


	
		leveltext.text ="Level: " + (currentlevel + 1).ToString ();
	
		targettext.text = "Target: " + targetscore [currentlevel].ToString ();
		TimeRemaining = targettime [currentlevel]; 


		combo = 0;
		comboPanel.SetActive (false);

		iscombopanelshowed=false;
	}

}
