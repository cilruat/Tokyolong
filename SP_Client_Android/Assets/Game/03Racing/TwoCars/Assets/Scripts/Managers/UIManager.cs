//  /*********************************************************************************
//   *********************************************************************************
//   *********************************************************************************
//   * Produced by Skard Games										                  *
//   * Facebook: https://goo.gl/5YSrKw											      *
//   * Contact me: https://goo.gl/y5awt4								              *											
//   * Developed by Cavit Baturalp Gürdin: https://tr.linkedin.com/in/baturalpgurdin *
//   *********************************************************************************
//   *********************************************************************************
//   *********************************************************************************/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace TwoCars
{
	public enum Menus
	{
		MAIN,
		INGAME,
		GAMEOVER
	}

	public class UIManager : MonoBehaviour {

		public MainMenu mainMenu;
		public InGameUI inGameUI;
	    public PopUp popUps;
	    public GameObject activePopUp;
	    public GameObject panel;

		public GameObject tapToStart;
		public RawImage imgVictory;
		public GameObject objVictory;
		public GameObject objSendServer;
		public GameObject objGameOver;
		public GameObject objReady;
		public GameObject objGo;
		public GameObject objBoard;
		public Text scoreTxt;

		int finishPoint = 0;
		public bool isStop = false;

		void Awake()
		{
			Managers.Audio.PlayGameMusic ();

			finishPoint = Info.TWO_CARS_FINISH_POINT;
			scoreTxt.text = Info.practiceGame ? "0" : "0 / " + finishPoint.ToString ();
		}

	    public void ActivateUI(Menus menutype)
		{
			if (menutype.Equals(Menus.MAIN))
			{
	            //StartCoroutine("ActivateMainMenu");          
			}
			else if(menutype.Equals(Menus.INGAME))
			{
	            //StartCoroutine("ActivateInGameUI");
				StartCoroutine (_StartGame ());
			}
		}

		IEnumerator _StartGame()
		{
			UITweenAlpha.Start (tapToStart, 1f, 0f, TWParam.New (.5f).Curve (TWCurve.CurveLevel2).DisableOnFinish ());
			yield return new WaitForSeconds (.5f);

			UITweenAlpha.Start(objReady, 0f, 1f, TWParam.New(.5f).Curve(TWCurve.CurveLevel2));

			yield return new WaitForSeconds (1f);
			UITweenAlpha.Start(objReady, 1f, 0f, TWParam.New(.5f).Curve(TWCurve.CurveLevel2));

			yield return new WaitForSeconds (.25f);
			UITweenAlpha.Start(objGo, 0f, 1f, TWParam.New(.5f).Curve(TWCurve.CurveLevel2));

			yield return new WaitForSeconds (1f);
			UITweenAlpha.Start(objGo, 1f, 0f, TWParam.New(.5f).Curve(TWCurve.CurveLevel2));

			yield return new WaitForSeconds (.3f);

			Managers.Difficulty.ResetDifficulty();
			Managers.Game.isGameActive = true;
			Managers.Input.isActive= true;
			Managers.Score.ResetScore();
			Managers.Spawner.StartSpawning();
		}

		public void GameOver()
		{
			isStop = true;
			objGameOver.SetActive (true);
		}

		public void SetScore(int score)
		{
			if (Info.practiceGame)
				scoreTxt.text = score.ToString ();
			else {
				scoreTxt.text = score.ToString () + " / " + finishPoint.ToString ();

				if (Info.practiceGame == false && score >= finishPoint)
					StartCoroutine (_SuccessEndGame ());
			}
		}

		IEnumerator _SuccessEndGame()
		{
			isStop = true;
			Managers.Game.isGameActive = false;
			Managers.Input.isActive= false;
			Managers.Spawner.StopSpawning();
			Managers.Spawner.ClearObstacles();

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

		public void ReturnHome()
		{
			SceneChanger.LoadScene ("Main", objBoard);
		}


	    IEnumerator ActivateMainMenu()
	    {
	        Managers.UI.inGameUI.gameOverPopUp.SetActive(false);

	        inGameUI.InGameUIEndAnimation();
	        inGameUI.gameObject.SetActive(false);
	        yield return new WaitForSeconds(0.3f);
	        mainMenu.gameObject.SetActive(true);
	        mainMenu.MainMenuStartAnimation();
	    }

	    IEnumerator ActivateInGameUI()
	    {
	        mainMenu.MainMenuEndAnimation();   
	        mainMenu.gameObject.SetActive(false);
	        yield return new WaitForSeconds(0.3f);
	        inGameUI.gameObject.SetActive(true);
	        inGameUI.InGameUIStartAnimation();
	    }

	    void Update()
	    {
	        if (activePopUp != null)
	            HideIfClickedOutside(activePopUp);
	    }

	    private void HideIfClickedOutside(GameObject outsidePanel)
	    {
	        if (outsidePanel.gameObject.name.Equals("GameOver"))
	            return;

	        if (Input.GetMouseButton(0) && outsidePanel.activeSelf &&
	            !RectTransformUtility.RectangleContainsScreenPoint(
	                outsidePanel.GetComponent<RectTransform>(),
	                Input.mousePosition,
	                Camera.main))
	        {
	            print("Clicked Outside");

	            outsidePanel.SetActive(false);
	            outsidePanel.transform.parent.gameObject.SetActive(false);
	            Managers.UI.panel.SetActive(false);
	            activePopUp = null;
	        }
	    }

	}
}