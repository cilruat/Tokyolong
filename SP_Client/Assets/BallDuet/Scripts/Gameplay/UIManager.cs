using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using OnefallGames;
using UnityEngine.UI;

namespace BallDuet
{
	public class UIManager : MonoBehaviour {

	    public static UIManager Instance { private set; get; }

	    //Gameplay UI
	    [SerializeField] private GameObject gameplayUI;
	    [SerializeField] private Text scoreTxt;


	    //Revive UI
	    [SerializeField] private GameObject reviveUI;
	    [SerializeField] private Image reviveCoverImg;

	    //GameOver UI
	    [SerializeField] private GameObject gameOverUI;
	    [SerializeField] private Text bestScoreTxt;
	    [SerializeField] private Text currentScoreTxt;
	    [SerializeField] private GameObject playBtn;
	    [SerializeField] private GameObject restartBtn;
	    [SerializeField] private GameObject soundOnBtn;
	    [SerializeField] private GameObject soundOffBtn;
	    [SerializeField] private GameObject musicOnBtn;
	    [SerializeField] private GameObject musicOffBtn;

	    //References
	    [SerializeField] private AnimationClip servicesBtns_Show;
	    [SerializeField] private AnimationClip servicesBtns_Hide;
	    [SerializeField] private Animator servicesAnim;

	    private void OnEnable()
	    {
	        GameManager.GameStateChanged += GameManager_GameStateChanged;
	    }


	    private void OnDisable()
	    {
	        GameManager.GameStateChanged -= GameManager_GameStateChanged;
	    }

	    private void GameManager_GameStateChanged(GameState obj)
	    {
	        if (obj == GameState.GameOver)
	        {
	            StartCoroutine(ShowGameOverUI(0.5f));
	        }
	        else if (obj == GameState.Playing)
	        {
	            gameplayUI.SetActive(true);
	            gameOverUI.SetActive(false);
	            reviveUI.SetActive(false);
	        }
	        else if (obj == GameState.Revive)
	        {
	            StartCoroutine(ShowReviveUI(0.5f));
	        }
	        else if (obj == GameState.Pause)
	        {
	            StartCoroutine(ShowPauseUI(0.5f));
	        }
	    }

	    void Awake()
	    {
	        if (Instance == null)
	        {
	            Instance = this;
	        }
	        else
	        {
	            DestroyImmediate(Instance.gameObject);
	            Instance = this;
	        }
	    }

	    void OnDestroy()
	    {
	        if (Instance == this)
	        {
	            Instance = null;
	        }
	    }




	    // Use this for initialization
	    void Start () {

	        if (!GameManager.isRestart) //This is the first load
	        {
	            gameplayUI.SetActive(false);
	            reviveUI.SetActive(false);
	            gameOverUI.SetActive(true);

	            restartBtn.SetActive(false);
	            playBtn.SetActive(true);
	        }

	        servicesAnim.Play(servicesBtns_Show.name);
	    }
		
		// Update is called once per frame
		void Update () {

	        UpdateMusicButtons();
	        UpdateMuteButtons();

	        scoreTxt.text = "SCORE: " + ScoreManager.Instance.Score.ToString();
	        bestScoreTxt.text = ScoreManager.Instance.BestScore.ToString();
	        currentScoreTxt.text = ScoreManager.Instance.Score.ToString();

	    }


	    ////////////////////////////Publish functions
	    public void PlayButtonSound()
	    {
	        SoundManager.Instance.PlaySound(SoundManager.Instance.button);
	    }

	    public void PauseBtn()
	    {
	        GameManager.Instance.PauseGame();
	    }

	    public void PlayBtn()
	    {
	        StartCoroutine(HandlePlayBtn());
	    }
	    public void RestartBtn()
	    {
	        StartCoroutine(HandleRestartBtn());
	    }

	    public void NativeShareBtn()
	    {
	        ShareManager.Instance.NativeShare();
	    }
	    public void FBShareBtn()
	    {
	        ShareManager.Instance.FacebookShare();
	    }


	    public void ToggleSound()
	    {
	        SoundManager.Instance.ToggleMute();
	    }

	    public void ToggleMusic()
	    {
	        SoundManager.Instance.ToggleMusic();
	    }

	    public void RateAppBtn()
	    {
	        Application.OpenURL(ShareManager.Instance.AppUrl);
	    }

	    public void ReviveBtn()
	    {
	        reviveUI.SetActive(false);
	        AdManager.Instance.ShowRewardedVideoAd();
	    }

	    public void SkipBtn()
	    {
	        reviveUI.SetActive(false);
	        GameManager.Instance.GameOver();
	    }



	    /////////////////////////////Private functions
	    private void UpdateMuteButtons()
	    {
	        if (SoundManager.Instance.IsMuted())
	        {
	            soundOnBtn.gameObject.SetActive(false);
	            soundOffBtn.gameObject.SetActive(true);
	        }
	        else
	        {
	            soundOnBtn.gameObject.SetActive(true);
	            soundOffBtn.gameObject.SetActive(false);
	        }
	    }


	    private void UpdateMusicButtons()
	    {
	        if (SoundManager.Instance.IsMusicOff())
	        {
	            musicOffBtn.gameObject.SetActive(true);
	            musicOnBtn.gameObject.SetActive(false);
	        }
	        else
	        {
	            musicOffBtn.gameObject.SetActive(false);
	            musicOnBtn.gameObject.SetActive(true);
	        }
	    }


	    private IEnumerator ShowPauseUI(float delay)
	    {
	        yield return new WaitForSeconds(delay);
	        gameOverUI.SetActive(true);
	        restartBtn.SetActive(false);
	        servicesAnim.Play(servicesBtns_Show.name);
	    }
	    private IEnumerator ShowReviveUI(float delay)
	    {
	        yield return new WaitForSeconds(delay);

	        reviveUI.SetActive(true);
	        StartCoroutine(ReviveCountDown());
	    }

	    private IEnumerator ShowGameOverUI(float delay)
	    {
	        yield return new WaitForSeconds(delay);

	        gameplayUI.SetActive(false);
	        gameOverUI.SetActive(true);
	        bestScoreTxt.gameObject.SetActive(true);
	        playBtn.SetActive(false);
	        restartBtn.SetActive(true);
	        servicesAnim.Play(servicesBtns_Show.name);
	    }

	    private IEnumerator ReviveCountDown()
	    {
	        float t = 0;
	        while (t < GameManager.Instance.ReviveWaitTime)
	        {
	            if (!reviveUI.activeInHierarchy)
	                yield break;
	            t += Time.deltaTime;
	            float factor = t / GameManager.Instance.ReviveWaitTime;
	            reviveCoverImg.fillAmount = Mathf.Lerp(1, 0, factor);
	            yield return null;
	        }
	        reviveUI.SetActive(false);
	        GameManager.Instance.GameOver();
	    }

	    private IEnumerator HandlePlayBtn()
	    {
	        servicesAnim.Play(servicesBtns_Hide.name);
	        yield return new WaitForSeconds(servicesBtns_Hide.length);
	        GameManager.Instance.PlayingGame();
	    }
	    private IEnumerator HandleRestartBtn()
	    {
	        servicesAnim.Play(servicesBtns_Hide.name);
	        yield return new WaitForSeconds(servicesBtns_Hide.length);

	        GameManager.Instance.LoadScene(SceneManager.GetActiveScene().name, 0);
	    }
	}
}