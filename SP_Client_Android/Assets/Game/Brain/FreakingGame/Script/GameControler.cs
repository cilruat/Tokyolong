/********************************************************************
 * Game : Freaking Game
 * Scene : Main Game
 * Description : Game Controler
 * History:
 *	2016/09/25	TungNguyen	First Edition
********************************************************************/
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Freaking
{
public class GameControler : MonoBehaviour
{
    #region Declare
    SoundManager _soundManager;
    /// <summary>
    /// Reference to the sound manager
    /// </summary>
    public SoundManager soundManager
    {
        get
        {
            if (_soundManager == null)
                _soundManager = FindObjectOfType<SoundManager>();

            return _soundManager;
        }
    }

    public Sprite soundOn;
    public Sprite soundOff;

    public int MaxWordQuestion;
	public int MaxMathQuestion;

	Sprite[] sprites;

    public Image imageLeft;
    public Image imageRight;

    string imgLeft;
    string imgRight;

    public Text suggest;

    TextAsset txtAsset;

    public GameObject panelGameOver;

    public Text score;
    public Text newScore;
    public Text bestScore;

    public Image sound;

    bool isGameOver = false;

    bool coolingDown;
    public float waitTime = 2.0f;
    public Image timerBar;

    public Camera camera;

    private int nPrimID;
    private string lang;

    #endregion Declare

    #region init
    // Use this for initialization
    void Start()
    {
		if(cMgrCommon.GamePlay == 1)
		{
			lang = Util.GetLanguage();
		}
		else
		{
			lang = "Math";
		}

		init();
    }

    // Update is called once per frame
    void Update()
    {
        if (coolingDown == true)
        {
            //Reduce fill amount over 2 seconds
            timerBar.fillAmount -= 1.0f / waitTime * Time.deltaTime;

            if (timerBar.fillAmount <= 0f)
            {
                isGameOver = true;
                PlayerPrefs.SetInt(lang, nPrimID);
                PlayerPrefs.Save();
                coolingDown = false;
                soundManager.PlaySoundFaile();
                StartCoroutine(WaitGameOver());
                GoogleMobileAdsScript.bannerView.Show();
                GoogleMobileAdsScript.RequestAdsInter();
            }
        }
    }

    /// <summary>
    /// Init
    /// </summary>
    internal void init()
    {
        isGameOver = false;
        coolingDown = true;

        if (SoundManager.SoundIsOn())
        {
            sound.sprite = soundOn;
        }
        else
        {
            sound.sprite = soundOff;
        }

        panelGameOver.SetActive(false);

        imgLeft = string.Empty;
        imgRight = string.Empty;

        cMgrCommon.score = 0;
        cMgrCommon.bestScore = PlayerPrefs.GetInt("BestScore" + lang);

        sprites = Resources.LoadAll<Sprite>("Sprites");
        txtAsset = Resources.Load<TextAsset>("data");

        nPrimID = PlayerPrefs.GetInt(lang);
        if (nPrimID == 0)
        {
            nPrimID = 1;
        }

        this.NextQuestion();
    }

    #endregion init

    #region Method

    /// <summary>
    /// Set sprite for button
    /// </summary>
    /// <param name="img">Image of button</param>
    /// <param name="sprites">sprites</param>
    /// <param name="name">key name sprite</param>
    void SetSprite(Image img, Sprite[] sprites, string name)
    {
        foreach (Sprite stexture in sprites)
        {
            if (stexture.name == name)
            {
                img.sprite = stexture;

                break;
            }
        }
    }

    public void btnLeft_Event()
    {
        //Disable button if game over
        if (isGameOver)
        {
            return;
        }

		if (cMgrCommon.GamePlay == 1)
		{
			PlayerPrefs.SetInt(lang, nPrimID == MaxWordQuestion ? 1 : nPrimID + 1);
		}
		else
		{
			PlayerPrefs.SetInt(lang, nPrimID == MaxMathQuestion ? 1 : nPrimID + 1);
		}
		PlayerPrefs.Save();

        //Right Answer
        if (imgLeft.Equals(cMgrCommon.Answer))
        {
            soundManager.PlaySoundScore();
            this.CheckBestScore();

            timerBar.fillAmount = 1f;

            NextQuestion();
        }
        else
        {
            PlayerPrefs.SetInt(lang, nPrimID);
            PlayerPrefs.Save();
            isGameOver = true;
            coolingDown = false;
            soundManager.PlaySoundFaile();
            StartCoroutine(WaitGameOver());
            GoogleMobileAdsScript.bannerView.Show();
            GoogleMobileAdsScript.RequestAdsInter();
        }
    }

    /// <summary>
    /// Button Right Event
    /// </summary>
    public void btnRight_Event()
    {
        //Disable button if game over
        if (isGameOver)
        {
            return;
        }

		if(cMgrCommon.GamePlay == 1)
		{
			PlayerPrefs.SetInt(lang, nPrimID == MaxWordQuestion ? 1 : nPrimID + 1);
		}
		else
		{
			PlayerPrefs.SetInt(lang, nPrimID == MaxMathQuestion ? 1 : nPrimID + 1);
		}
		PlayerPrefs.Save();
        //Right Answer
        if (imgRight.Equals(cMgrCommon.Answer))
        {
            soundManager.PlaySoundScore();
            this.CheckBestScore();

            timerBar.fillAmount = 1f;

            NextQuestion();
        }
        else
        {
            isGameOver = true;
            coolingDown = false;
            soundManager.PlaySoundFaile();
            StartCoroutine(WaitGameOver());
            GoogleMobileAdsScript.bannerView.Show();
            GoogleMobileAdsScript.RequestAdsInter();
        }
    }

    /// <summary>
    /// Wait to show Game Over panel
    /// </summary>
    /// <returns>none</returns>
    IEnumerator WaitGameOver()
    {
        yield return new WaitForSeconds(2);
        panelGameOver.SetActive(true);
    }

    /// <summary>
    /// Increase score, set text score, best score,...
    /// </summary>
    void CheckBestScore()
    {
        cMgrCommon.score = cMgrCommon.score + 1;

        if (cMgrCommon.score > cMgrCommon.bestScore)
        {
            cMgrCommon.bestScore = cMgrCommon.score;
            PlayerPrefs.SetInt("BestScore" + lang, cMgrCommon.bestScore);
        }

        score.text = cMgrCommon.score.ToString();
        newScore.text = cMgrCommon.score.ToString();
        bestScore.text = cMgrCommon.bestScore.ToString();
    }

    /// <summary>
    /// Next Question
    /// </summary>
    public void NextQuestion()
    {
        this.SetBackgroundColor();

		ValidatePrimID();

		print("Question " + nPrimID);

        if (cMgrXml.ReadQuiz(txtAsset.text, nPrimID.ToString(), lang))
        {
            suggest.text = cMgrCommon.Suggest;

            this.SetSprite(imageLeft, sprites, cMgrCommon.ImageLeft);
            imgLeft = cMgrCommon.ImageLeft;
            this.SetSprite(imageRight, sprites, cMgrCommon.ImageRight);
            imgRight = cMgrCommon.ImageRight;
        }
        else
        {
            //Show message error
        }
    }

	private void ValidatePrimID()
	{
		if (cMgrCommon.GamePlay == 1)
		{
			nPrimID = nPrimID == MaxWordQuestion ? 1 : nPrimID + 1;
		}
		else
		{
			nPrimID = nPrimID == MaxMathQuestion ? 1 : nPrimID + 1;
		}
	}

    /// <summary>
    /// Set bg color
    /// </summary>
    void SetBackgroundColor()
    {
        int rnd = Random.Range(1, 6);
        switch (rnd)
        {
            case 1:
                camera.backgroundColor = new Color(214f / 255, 83f / 255, 0f / 255, 1f);
                break;

            case 2:
                camera.backgroundColor = new Color(6f / 255, 129f / 255, 197f / 255, 1f);
                break;

            case 3:
                camera.backgroundColor = new Color(44f / 255, 62f / 255, 82f / 255, 1f);
                break;

            case 4:
                camera.backgroundColor = new Color(77f / 255, 56f / 255, 197f / 255, 1f);
                break;

            case 5:
                camera.backgroundColor = new Color(197f / 255, 71f / 255, 71f / 255, 1f);
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// Button Home Event
    /// </summary>
    public void btnHome_Event()
    {
        soundManager.PlaySoundPush();
        StartCoroutine(WaitToEvent(1));
        GoogleMobileAdsScript.bannerView.Hide();
    }

    /// <summary>
    /// Button Replay Event
    /// </summary>
    public void btnReplay_Event()
    {
        soundManager.PlaySoundPush();
        StartCoroutine(WaitToEvent(2));
        GoogleMobileAdsScript.bannerView.Hide();
    }

    /// <summary>
    /// Wait 1.5s to next event
    /// </summary>
    /// <param name="index"></param>
    /// <returns>none</returns>
    IEnumerator WaitToEvent(int index)
    {
        yield return new WaitForSeconds(1.5f);
        switch (index)
        {
            case 1:
                //SceneManager.LoadScene("MenuGame"); //Home
                Application.LoadLevel("MenuGame");
                break;

            case 2:
                //SceneManager.LoadScene("MainGame"); //Replay
                Application.LoadLevel("MainGame");
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// Set Sound On/Off
    /// </summary>
    public void SetSoundOnOff()
    {
        if (SoundManager.SoundIsOn())
        {
            sound.sprite = soundOff;
            SoundManager.SetSoundOff();
        }
        else
        {
            sound.sprite = soundOn;
            SoundManager.SetSoundOn();
        }
    }

    #endregion Method
}
}