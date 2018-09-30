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

	    public void ActivateUI(Menus menutype)
		{
			if (menutype.Equals(Menus.MAIN))
			{
	            StartCoroutine("ActivateMainMenu");          
			}
			else if(menutype.Equals(Menus.INGAME))
			{
	            StartCoroutine("ActivateInGameUI");
			}	
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