using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Takgu
{
    /// <summary>
    /// This script controls the creation of ball gameobject, handling all canvases and the gameover screen
    /// </summary>
    public class GameController : MonoBehaviour
    {
        public GameObject GameObjectBall;
        public Vector3 spawnValues;
        public int ballCount;
        public float spawnWait;
        public float startWait;
        public float waveWait;
        public int speed;
        public int BounceSpeed;
        public int ballCountIncrease;
        float TimeSpeed = 1;
        public float TimeSpeedIncrease;
        public Text speedText;
        int speedMultiplier = 1;
        int score = 0;
        int highscore;
        public GameObject GameOverCanvas;
        public GameObject StartCanvas;
        public Text ScoreText;
        public Text HighScoreText;
        public GameObject pausecanvas;
        void Start()
        {   //start the balls
            StartCoroutine(SpawnWaves());


            //set start canvas activated
            //StartCanvas.SetActive(true);
			StartCanvas.gameObject.SetActive(true);
			//set GameOver canvas deactivated
			GameOverCanvas.gameObject.SetActive(false);
            //set puase canvas deactivated
			pausecanvas.gameObject.SetActive(false);
            //set timescale to 1
			//Time.timeScale 은 정적프로퍼터 이기 때문에 모든 씬의 게임 오
            Time.timeScale = 1;

		
            //get the value of highscore
            highscore = PlayerPrefs.GetInt("HighScore", 0);
        }

        IEnumerator SpawnWaves()
        {
            //wait of startWait(int) seconds
            yield return new WaitForSeconds(startWait);
            while (true)
            {
                for (int i = 0; i < ballCount; i++)
                {
                    //Setting spawn Position randomly between spawnValues
                    Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
                    //set rotation to the gameObjects identity
                    Quaternion spawnRotation = Quaternion.identity;
                    //create ball gameobject at spawnPosition & spawnRotation
                    Instantiate(GameObjectBall, spawnPosition, spawnRotation);
                    //wait between every ball drop
                    yield return new WaitForSeconds(spawnWait);
                }
                //wait between waves of balls 
                yield return new WaitForSeconds(waveWait);
                //increase time speed & set to time scale
				TimeSpeed += TimeSpeedIncrease;
				Time.timeScale = TimeSpeed;



                //increase speed multiplier by 1 & showing its value on speedText
                speedMultiplier++;
                speedText.text = "Speed:" + speedMultiplier.ToString() + "x";
                //LevelUp audio is played
                AudioSource audio = GetComponent<AudioSource>();
                audio.Play();
                //ballCount is increased by ballCountIncrease(int)
                ballCount += ballCountIncrease;
            }
        }

        public void Score()
        {
            //it adds score by 1 when receives message from BounceBack script
            score++;
        }
        public void GameOver()
        {
            //Pause the time
            //Time.timeScale = 0;
            //activate the gameover canvas
            GameOverCanvas.SetActive(true);
            //fading of canvas is started
            StartCoroutine(DoFade());
            //start canvas is deactivated
            StartCanvas.SetActive(false);
            //show the score value on ScoreText
            ScoreText.text = "득점은" + score.ToString();
            //if score is greater than highscore change the stored value of highscore
            if (score > highscore)
            {
                PlayerPrefs.SetInt("HighScore", score);
            }
            //show highscore value on HighScoreText
            highscore = PlayerPrefs.GetInt("HighScore", 0);
            HighScoreText.text = "최고점수는 :" + highscore.ToString();
        }
        /*public void restart()
        {
            //restart the level
            //Application.LoadLevel(0);
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        }
        public void exit()
        {
            //exit the game
			SceneManager.LoadScene("ArcadeGame");

        }*/


		public void  Restart()
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);

		}

		/// <summary>
		/// Restart the current level
		/// </summary>
		public void  MainMenu()
		{
			Time.timeScale = 1;
			SceneManager.LoadScene("ArcadeGame");
		}


        public void Pause()
        {
            //pausing the time & activating pause canvas
            Time.timeScale = 0;
            pausecanvas.SetActive(true);
        }
        public void Unpause()
        {
            //unpausing the time & deactivating pause canvas
            Time.timeScale = TimeSpeed;
            pausecanvas.SetActive(false);
        }
        IEnumerator DoFade()
        {
            //get the canvas to canavsGroup gameobject
            CanvasGroup canvasGroup = GameOverCanvas.GetComponent<CanvasGroup>();
            //set visibility to 0
            canvasGroup.alpha = 0;
            while (canvasGroup.alpha < 1)
            {
                //increse visibility by 0.05 per frame
                canvasGroup.alpha += 0.05f;
                yield return null;
            }
            //set the game object interactable
            canvasGroup.interactable = true;
            yield return null;
        }
    }
}