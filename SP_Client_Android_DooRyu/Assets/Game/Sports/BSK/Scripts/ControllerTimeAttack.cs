using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent (typeof (GameController))]
public class ControllerTimeAttack : MonoBehaviour {
	
	public GameObject startPanel;
	
	public Slider timeSlider;
	public float goalTimeScore = 5;
	public float timerSpeed = 5;
	public float spawnPeriod = 1;
	
	//See ControllerArcade.cs for more details about these vars
	public int bonmusRingClearCombo = 3;
	public int bonusAimCombo = 5;
	public int bonusRingThrowsLimit = 3;
	public int bonusAimThrowsLimit = 3;
	
	public Text scoreTxt;
	public Text bestScoreTxt;
	public Text plusScoreTxt;
	public Text plusDotsTxt;
	public Transformer TimeIcon;
	public BoxCollider spawnCollider;
	
	public bool debugAim;
	
	private GameObject ring;
	private Shooter shooter;
	private AudioSource thisAudio;
	
	//See ControllerArcade.cs for more details about these vars
	private int score;
	private int comboScore;
	private int comboGoals;
	private int comboClearGoals;
	private int comboGoals2BonusRing;
	private bool bonusRingActive;
	private int bonusRingThrows;
	private int comboGoals2bonusAim;
	private bool bonusAimActive;
	private int bonusAimThrows;
	private int lastRecord;
	private bool hitRecord;
	
	private bool ballThrown;
	private float ballTimer;
	private float timer;
	
	
	
	void OnEnable(){
		Ball.OnThrow += Thrown;
		Ball.OnGoal += Goal;
        Ball.OnFail += Fail;
	}
	
	void OnDisable(){
		Ball.OnThrow -= Thrown;
		Ball.OnGoal -= Goal;
        Ball.OnFail -= Fail;
	}
	
	void Awake(){
		Shooter.aimDotsNum = 60;
	}
	
	void Start(){
		ring = GameObject.Find("ring");
		shooter = GameObject.Find("Shooter").GetComponent<Shooter>();
		thisAudio = GetComponent<AudioSource>();
		ResetData();
		UpdateAimDotsNum();
		shooter.newBallPosition = GetRandomPosInCollider();
	}
	
	void ShowStartPanel(){
		startPanel.SetActive(true);
	}
	
	void HideStartPanel(){
		startPanel.SetActive(false);
	}
	
	void Update () {
		if(GameController.data.gameState == GameController.State.InGame) {
			timeSlider.value -= Time.deltaTime * timerSpeed;
			ballTimer += Time.deltaTime;
			if(ballThrown && ballTimer > spawnPeriod && timeSlider.value > 0){
				ballThrown = false;
				shooter.spawnBall();
			}
			timer += Time.deltaTime;																//Increasing timer value as usual timer
			if(timeSlider.value > 0 && timer > timeSlider.value/timeSlider.maxValue + 0.1f) {		//If timer reaches the value proportional to slider then we play "tic" sound and reset timer. So we will have it faster.
				GetComponent<AudioSource>().Play();													//Plays time sound attached to AudioSource component
				timer = 0;
			}
		}
		
		
	}
	
	void Thrown(){
		ballThrown = true;
		ballTimer = 0;
		shooter.newBallPosition = GetRandomPosInCollider();
	}
	
	void Goal(float distance, float height, bool floored, bool clear, bool special){
		comboGoals += 1;
		
		if(timeSlider.value > 0) {
			timeSlider.value += clear ? goalTimeScore*2 : goalTimeScore;
			TimeIcon.ScaleImpulse(new Vector3(1.3f,1.3f,1), 0.4f, 1);
		}
		
		if(!bonusAimActive) {
			comboGoals2bonusAim += 1;
			if(comboGoals2bonusAim == bonusAimCombo) {
				bonusAimActive = true;
				plusDotsTxt.gameObject.SetActive(true);
				thisAudio.PlayOneShot(SoundController.data.bonusOpen);
			}
		} else {
			bonusAimThrows += 1;
			if(bonusAimThrows == bonusAimThrowsLimit) {
				bonusAimThrows = comboGoals2bonusAim = 0;
				bonusAimActive = false;
			}	
		}
		
		if(bonusRingActive){
			bonusRingThrows +=1;
			if(bonusRingThrows == bonusRingThrowsLimit) {
				comboGoals2BonusRing = 0;
				StartCoroutine(ResetRing());
			}
		}
		
		if(clear) {
			comboClearGoals += 1;
			if(!bonusRingActive) {
				comboGoals2BonusRing +=1;
				if(comboGoals2BonusRing == bonmusRingClearCombo) {
					StartCoroutine(GrowRing());
				}
			}

			if(special)
				SoundController.data.playClearSpecialGoal();
			else
				SoundController.data.playClearGoal();
		} else {
			SoundController.data.playGoal();
			comboClearGoals = comboGoals2BonusRing = 0;
		}
		
		comboScore += (int)distance;
		plusScoreTxt.text = "+"+comboScore.ToString("F0");
		
		if(special) {
			int heightScore = (int)height;
			comboScore += heightScore;
			plusScoreTxt.text += "\n+"+heightScore.ToString("F0");
		}
		
		if(floored){
			int flooredScore = (int)distance*2;
			plusScoreTxt.text += "+"+flooredScore.ToString("F0");
		}
				
		plusScoreTxt.gameObject.SetActive(true);
		scoreTxt.gameObject.GetComponent<Transformer>().ScaleImpulse(new Vector3(1.3f,1.3f,1), 0.4f, 1);
		AddScore(comboScore);
		BallCompleted();
	}
	
	void Fail(){
		comboGoals = comboClearGoals = comboGoals2BonusRing = comboGoals2bonusAim = comboScore = 0;
		
		if(bonusAimActive) {
			bonusAimThrows += 1;
			if(bonusAimThrows == bonusAimThrowsLimit) {
				bonusAimThrows = comboGoals2bonusAim = 0;
				bonusAimActive = false;
			}	
		}
		
		if(bonusRingActive) {
			bonusRingThrows += 1;
			if(bonusRingThrows == bonusRingThrowsLimit) {
				comboGoals2BonusRing = 0;
				StartCoroutine(ResetRing());
			}	
		}
		
		BallCompleted();
	}
	
	void BallCompleted(){
		if(timeSlider.value == 0 && GameController.data.gameState == GameController.State.InGame && ballThrown)
			GameController.data.Complete();
		UpdateAimDotsNum();
	}
		
	IEnumerator GrowRing(){
		yield return new WaitForSeconds(0.5f);
		bonusRingActive = true;
		ring.SetActive(false);
		ring.transform.localPosition = new Vector3(1.9f,9.51f,0);
		ring.transform.localScale = new Vector3(2,2,2);
		ring.SetActive(true);
		thisAudio.PlayOneShot(SoundController.data.bonusOpen);
	}
	
	IEnumerator ResetRing(){
		yield return new WaitForSeconds(0.5f);
		bonusRingActive = false;
		ring.SetActive(false);
		ring.transform.localPosition = new Vector3(1.2f,9.51f,0);
		ring.transform.localScale = new Vector3(1,1,1);
		ring.SetActive(true);
		bonusRingThrows = 0;
	}
	
	public void AddScore(int score) {
		this.score += score;
		scoreTxt.text = this.score.ToString();
		if(this.score > PlayerPrefs.GetInt("timeAttackBestScore",0)){
			bestScoreTxt.text = "BEST SCORE - " +this.score.ToString();
			PlayerPrefs.SetInt("timeAttackBestScore",this.score);
			if(lastRecord > 0 && !hitRecord) {
				HitNewRecord();
			}
		}
		
	}
	
	public void HitNewRecord(){
		bestScoreTxt.color = Color.yellow;
		bestScoreTxt.gameObject.GetComponent<Transformer>().ScaleImpulse(new Vector3(1.3f,1.3f,1), 0.4f, 1);
		SoundController.data.playNewRecord();
		hitRecord = true;
	}
	
	public void UpdateAimDotsNum(){
		if(bonusAimActive || debugAim)
			Shooter.aimDotsNum = 60;
		else
			Shooter.aimDotsNum = 15;
	}
	
	public void ResetData(){
		score = 0;
		timeSlider.value = timeSlider.maxValue;
		scoreTxt.text = this.score.ToString();
		lastRecord = PlayerPrefs.GetInt("timeAttackBestScore",0);
		bestScoreTxt.text = "BEST SCORE - " +lastRecord;
	}
	
	private Vector3 GetRandomPosInCollider(){
		Vector3 center = spawnCollider.gameObject.transform.position;
		Vector3 randPos = center + new Vector3(Random.Range(-spawnCollider.bounds.size.x/2,spawnCollider.bounds.size.x/2),Random.Range(-spawnCollider.bounds.size.y/2,spawnCollider.bounds.size.y/2),0);
		return randPos;
		
	}
}
