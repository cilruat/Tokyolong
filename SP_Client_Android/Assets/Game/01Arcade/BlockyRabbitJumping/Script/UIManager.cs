using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace BlockJumpRabbit
{

public class UIManager : MonoBehaviour {

    public Text textScoreUI;
    public GameObject homeUI;
    public GameObject gamePlayUI;
    public GameObject gameoverUI;
	public GameObject loadingUI;
    public GameObject pauseUI;
	public TextureRenderConvert effectScreen;
	public Animator animUI;
    public GameObject tutorialUI;
  
    public Text currentScoreText;
    public Text bestScoreText;

    private GameController gameController;

    void Awake()
    {
        gameController = GetComponent<GameController>();
    }

    public void ChangeScoreText(int score)
    {
        textScoreUI.text = score.ToString();
    }

    public void CallGameStart()
    {
        animUI.SetTrigger("gameplay");
        StartCoroutine(StartGame());

        if (gameController.firstPlay)
            tutorialUI.SetActive(true);
    }

    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(1.0f);
        gameController.ChangeStateGame(StateGame.GamePlay);
        gamePlayUI.SetActive(true);
        yield return new WaitForSeconds(3.0f);
        homeUI.SetActive(false);
    }

    public void CallGameOVer()
    {      
        gameoverUI.SetActive(true);

        currentScoreText.text = gameController.currentScore.ToString();
        bestScoreText.text = gameController.highestScore.ToString();

        gamePlayUI.SetActive(false);
    }

    public void CallGameRestart()
    {
        gameController.RestartGame();
        gamePlayUI.SetActive(true);
        gameoverUI.SetActive(false);
    }

	public void CallHome() {
		effectScreen.SaveTexture ();
	}

    // Call pause button
    public void CallPause()
    {
        Time.timeScale = 0;
        pauseUI.SetActive(true);
    }

    // Call resume
    public void Resume()
    {
        Time.timeScale = 1;
        pauseUI.SetActive(false);
    }

    // Call tutorial
    public void CallTutorial()
    {
        gameController.firstPlay = false;
        tutorialUI.gameObject.SetActive(true);
    }

	// Call rate
	public void CallRate(){
		Application.OpenURL ("https://www.facebook.com/stommedia/");	
	}
	}
}
