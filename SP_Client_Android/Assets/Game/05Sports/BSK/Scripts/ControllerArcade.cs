using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace BSK
{
[RequireComponent (typeof (GameController))]
public class ControllerArcade : MonoBehaviour {
	
	public GameObject startPanel;
	
	public int startBallsCount = 10;				//Balls count we start with
	public int bonusRingClearCombo = 3;				//Count of clear goals in a row to get big rinf bonus
	public int bonusAimCombo = 5;					//Count of goals in a row to get aim bonus
	public int bonusAimMinXpLevel = 5;				//Minimum xp level to be able get aim bonus
	public int bonusRingThrowsLimit = 3;			//When you get big ring bonus after this count of throws it will gone
	public int bonusAimThrowsLimit = 3;				//When you get aim bonus after this count of throws it will gone 
	public int xpScoreStep = 100;					//XP step. With help of this you can tweak the speed of getting xp level.
	
	public Text ballsCountTxt;
	public Text scoreTxt;
	public Text bestScoreTxt;
	public Text plusScoreTxt;
	public Text plusBallTxt;
	public Text plusDotsTxt;
	public Image superBallIcon;
	public GameObject superBallEffect;
	public Transformer ballIcon;
	public BoxCollider spawnCollider;
	
	public bool debugAim;
	
	private GameObject ring;
	private Shooter shooter;
	private AudioSource thisAudio;
	
	private int currentBallsCount;					//Current amount of balls left to throw
	private int score;								//Current score
	private int comboScore;							//Current amount of combo score. Increases when you have goals in a row. Resets when you fail a ball.
	private int comboGoals;							//Current quantity of usual goals got in a row. Increases when you have goals in a row. Resets when you fail a ball.
	private int comboClearGoals;					//Current quantity of clear goals got in a row. Increases when you have clear goals in a row. Resets when you fail a ball.
	private int comboGoals_bonusRing;				//Current quantity of clear goals got in a row to open ring bonus. Increases when you have clear goals in a row. Resets when you fail a ball.
	private bool bonusRingActive;					//Boolean to determine if ring bonus currently active or not
	private int bonusRingThrows;					//Current quantity of balls thrown during ring bonus active
	private int comboGoals_bonusAim;				//Current quantity of goals got in a row to open aim bonus. Increases when you have goals in a row. Resets when you fail a ball.
	private bool bonusAimActive;					//Boolean to determine if aim bonus currently active or not
	private int bonusAimThrows;						//Current quantity of balls thrown during aim bonus active or not
	private bool bonusSuperBallActive;				//Boolean to determine if superball bonus active or not
	private float superBallProgress;				//Float that keeps current superball progress value
	private int xpLevel;							//Int that defines current XP level 
	private int lastRecord;							//Int that keeps current best score 
	private bool hitRecord;							//Boolean that defines if we already hitted last best score or not
	
	
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
		ring = GameObject.Find("ring");
		shooter = GameObject.Find("Shooter").GetComponent<Shooter>();
		thisAudio = GetComponent<AudioSource>();
		currentBallsCount = startBallsCount;
		ResetData();
		shooter.newBallPosition = GetRandomPosInCollider();
	}
	
	void Update () {		
		if(bonusSuperBallActive && !shooter.isBallThrown){
			superBallEffect.transform.parent = shooter.currentBall.transform;
			superBallEffect.transform.localPosition = Vector3.zero;
			superBallEffect.SetActive(true);
		}
	}
	
	void ShowStartPanel(){
		if(PlayerPrefs.GetInt("arcadeHideHelp",0)== 0)
			startPanel.SetActive(true);
		else
			GameController.data.StartPlay();
	}
	
	public void toggleNoShowAgain(){
		PlayerPrefs.SetInt("arcadeHideHelp", PlayerPrefs.GetInt("arcadeHideHelp",0)== 1 ? 0 : 1);
	}
	
	void HideStartPanel(){
		if(startPanel.activeInHierarchy)
			startPanel.SetActive(false);
	}
	
	void Goal(float distance, float height, bool floored, bool clear, bool special){
		comboGoals += 1;
		superBallProgress += 0.01f;
		if(!bonusAimActive) {
			if(xpLevel > bonusAimMinXpLevel)
				comboGoals_bonusAim += 1;
			if(comboGoals_bonusAim == bonusAimCombo) {
				bonusAimActive = true;
				plusDotsTxt.gameObject.SetActive(true);
				thisAudio.PlayOneShot(SoundController.data.bonusOpen);
			}
		} else {
			bonusAimThrows += 1;
			if(bonusAimThrows == bonusAimThrowsLimit) {
				bonusAimThrows = comboGoals_bonusAim = 0;
				bonusAimActive = false;
			}	
		}
		
		if(bonusRingActive){
			bonusRingThrows +=1;
			if(bonusRingThrows == bonusRingThrowsLimit) {
				comboGoals_bonusRing = 0;
				StartCoroutine(ResetRing());
			}
		}
		
		if(clear) {
			currentBallsCount += 1;
			ballIcon.ScaleImpulse(new Vector3(1.3f,1.3f,1), 0.4f, 2);
			plusBallTxt.gameObject.SetActive(true);
			comboClearGoals += 1;
			
			if(!bonusRingActive) {
				comboGoals_bonusRing +=1;
				if(comboGoals_bonusRing == bonusRingClearCombo) {
					StartCoroutine(GrowRing());
				}
			}

			if(special)
				SoundController.data.playClearSpecialGoal();
			else
				SoundController.data.playClearGoal();
			superBallProgress += 0.01f;
		} else {
			SoundController.data.playGoal();
			comboClearGoals = comboGoals_bonusRing = 0;
		}
		
		comboScore += (int)distance;
		plusScoreTxt.text = "+"+comboScore.ToString("F0");
		
		if(special) {
			int heightScore = (int)height;
			comboScore += heightScore;
			plusScoreTxt.text += "\n+"+heightScore.ToString("F0");
			superBallProgress += 0.01f;
		}
		
		if(floored){
			int flooredScore = (int)distance*2;
			plusScoreTxt.text += "+"+flooredScore.ToString("F0");
			superBallProgress += 0.01f;
		}
		
		if(bonusSuperBallActive) {
			int superBallScore = (int)(height*distance*10);
			comboScore += superBallScore;
			plusScoreTxt.text += "+"+superBallScore.ToString("F0");
			superBallProgress = 0;
			bonusSuperBallActive = false;
			superBallEffect.SetActive(false);
		} else {
			if(superBallProgress >= 1) {
				bonusSuperBallActive = true;
				thisAudio.PlayOneShot(SoundController.data.bonusOpen);
				superBallIcon.gameObject.GetComponent<Transformer>().ScaleImpulse(new Vector3(1.3f,1.3f,1), 0.4f, 3);
			}
		}
		
		plusScoreTxt.gameObject.SetActive(true);
		scoreTxt.gameObject.GetComponent<Transformer>().ScaleImpulse(new Vector3(1.3f,1.3f,1), 0.4f, 1);
		AddScore(comboScore);
		BallCompleted();
	}
	
	void Fail(){
		comboGoals = comboClearGoals = comboGoals_bonusRing = comboGoals_bonusAim = comboScore = 0;
		currentBallsCount -= 1;
		
		if(bonusAimActive) {
			bonusAimThrows += 1;
			if(bonusAimThrows == bonusAimThrowsLimit) {
				bonusAimThrows = comboGoals_bonusAim = 0;
				bonusAimActive = false;
			}	
		}
		
		if(bonusRingActive) {
			bonusRingThrows += 1;
			if(bonusRingThrows == bonusRingThrowsLimit) {
				comboGoals_bonusRing = 0;
				StartCoroutine(ResetRing());
			}	
		}
		
		if(bonusSuperBallActive) {
			superBallProgress = 0;
			bonusSuperBallActive = false;
			superBallEffect.SetActive(false);
		}
		
		BallCompleted();
	}
	
	void BallCompleted(){
		xpLevel = score > 2*xpScoreStep ? score/xpScoreStep : 1;
		superBallIcon.fillAmount = superBallProgress;
		UpdateBallsCount();
		UpdateAimDotsNum();
		UpdateSpawnCollider();
		shooter.newBallPosition = GetRandomPosInCollider();
		shooter.spawnBall();
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
		if(this.score > PlayerPrefs.GetInt("arcadeBestScore",0)){
			bestScoreTxt.text = "BEST SCORE - " +this.score.ToString();
			PlayerPrefs.SetInt("arcadeBestScore",this.score);
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
	
	void UpdateBallsCount(){
		ballsCountTxt.text = currentBallsCount.ToString();
		if(currentBallsCount < 1) {
			GameController.data.Complete();
		}
	}
	
	public void UpdateAimDotsNum(){
		if(bonusAimActive || debugAim)
			Shooter.aimDotsNum = 60;
		else
			Shooter.aimDotsNum = Mathf.Clamp(31-(xpLevel/2),8,30);
	}
	
	void UpdateSpawnCollider(){
		float colLenght = Mathf.Clamp(19+xpLevel,20,35);
		spawnCollider.gameObject.transform.position = new Vector3(colLenght/2,4,0);
		Vector3 tempSize = spawnCollider.size;
		tempSize.x = colLenght;
		spawnCollider.size = tempSize;
	}
	
	public void ResetData(){
		currentBallsCount = startBallsCount;
		score = 0;
		xpLevel = 1;
		superBallIcon.fillAmount = 0;
		UpdateBallsCount();
		UpdateSpawnCollider();
		UpdateAimDotsNum();
		scoreTxt.text = this.score.ToString();
		lastRecord = PlayerPrefs.GetInt("arcadeBestScore",0);
		bestScoreTxt.text = "BEST SCORE - " +lastRecord;
	}
	
	private Vector3 GetRandomPosInCollider(){
		Vector3 center = spawnCollider.gameObject.transform.position;
		Vector3 randPos = center + new Vector3(Random.Range(-spawnCollider.bounds.size.x/2,spawnCollider.bounds.size.x/2),Random.Range(-spawnCollider.bounds.size.y/2,spawnCollider.bounds.size.y/2),0);
		return randPos;
		
	}
}
}