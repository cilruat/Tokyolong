using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class InssaRandomGame : MonoBehaviour {

	public Animator anim;
	public bool PlayAnim = false;
	System.Random InssaRandomizer = new System.Random(); //여기 클래스에서 랜덤 선언?
	Transform RandomPanel;


	public Animator animTreasure;
	public bool TreasurePlayAnim = false;


	public GameObject animPanel;
	public GameObject TreasureanimPanel;


	public void OnClickRemoteControl()
	{

		if (PlayAnim == false) 
		{
			animPanel.SetActive (true);
			anim.Play ("TVAnim");
			PlayAnim = true;

		}
	}

	public void OnClickTreaure()
	{
		if (TreasurePlayAnim == false) {
			TreasureanimPanel.SetActive (true);
			animTreasure.Play ("InssaEffect");
			TreasurePlayAnim = true;
		}
	}


	public void OnAnimationComplete()
	{
		PlayAnim = false;
		RandomPanel = GameObject.Find ("Canvas").gameObject.transform.GetChild (0).gameObject.transform;

		for (int i = 0; i < RandomPanel.childCount; i++) {
			RandomPanel.GetChild (i).gameObject.SetActive (false);
		}

		switch (InssaRandomizer.Next (0, GameObject.Find ("Canvas").gameObject.transform.GetChild (0).gameObject.GetComponent<RandomGameGetList> ().ImageList.Count)) {
		case 0:
			StartCoroutine (RestartAnim ());
			RandomPanel.GetChild (0).gameObject.SetActive (true);
			break;

		case 1:
			StartCoroutine (RestartAnim ());

			RandomPanel.GetChild (1).gameObject.SetActive (true);
			break;

		case 2:
			StartCoroutine (RestartAnim ());

			RandomPanel.GetChild (2).gameObject.SetActive (true);
			break;

		case 3:
			StartCoroutine (RestartAnim ());

			RandomPanel.GetChild (3).gameObject.SetActive (true);
			break;

		case 4:
			StartCoroutine (RestartAnim ());

			RandomPanel.GetChild (4).gameObject.SetActive (true);
			break;

		case 5:
			StartCoroutine (RestartAnim ());

			RandomPanel.GetChild (5).gameObject.SetActive (true);
			break;

		case 6:
			StartCoroutine (RestartAnim ());

			RandomPanel.GetChild (6).gameObject.SetActive (true);
			break;

		case 7:
			StartCoroutine (RestartAnim ());

			RandomPanel.GetChild (7).gameObject.SetActive (true);
			break;


			case 8:
				StartCoroutine(RestartAnim());

				RandomPanel.GetChild(8).gameObject.SetActive(true);
				break;
			case 9:
				StartCoroutine(RestartAnim());

				RandomPanel.GetChild(9).gameObject.SetActive(true);
				break;
			case 10:
				StartCoroutine(RestartAnim());

				RandomPanel.GetChild(10).gameObject.SetActive(true);
				break;
			case 11:
				StartCoroutine(RestartAnim());

				RandomPanel.GetChild(11).gameObject.SetActive(true);
				break;
			case 12:
				StartCoroutine(RestartAnim());

				RandomPanel.GetChild(12).gameObject.SetActive(true);
				break;
			case 13:
				StartCoroutine(RestartAnim());

				RandomPanel.GetChild(13).gameObject.SetActive(true);
				break;
			case 14:
				StartCoroutine(RestartAnim());

				RandomPanel.GetChild(14).gameObject.SetActive(true);
				break;
			case 15:
				StartCoroutine(RestartAnim());

				RandomPanel.GetChild(15).gameObject.SetActive(true);
				break;
			case 16:
				StartCoroutine(RestartAnim());

				RandomPanel.GetChild(16).gameObject.SetActive(true);
				break;
			case 17:
				StartCoroutine(RestartAnim());

				RandomPanel.GetChild(17).gameObject.SetActive(true);
				break;
			case 18:
				StartCoroutine(RestartAnim());

				RandomPanel.GetChild(18).gameObject.SetActive(true);
				break;
			case 19:
				StartCoroutine(RestartAnim());

				RandomPanel.GetChild(19).gameObject.SetActive(true);
				break;
			case 20:
				StartCoroutine(RestartAnim());

				RandomPanel.GetChild(20).gameObject.SetActive(true);
				break;
			case 21:
				StartCoroutine(RestartAnim());

				RandomPanel.GetChild(21).gameObject.SetActive(true);
				break;
			case 22:
				StartCoroutine(RestartAnim());

				RandomPanel.GetChild(22).gameObject.SetActive(true);
				break;
			case 23:
				StartCoroutine(RestartAnim());

				RandomPanel.GetChild(23).gameObject.SetActive(true);
				break;
			case 24:
				StartCoroutine(RestartAnim());

				RandomPanel.GetChild(24).gameObject.SetActive(true);
				break;
			case 25:
				StartCoroutine(RestartAnim());

				RandomPanel.GetChild(25).gameObject.SetActive(true);
				break;
			case 26:
				StartCoroutine(RestartAnim());

				RandomPanel.GetChild(26).gameObject.SetActive(true);
				break;
			case 27:
				StartCoroutine(RestartAnim());

				RandomPanel.GetChild(27).gameObject.SetActive(true);
				break;
			case 28:
				StartCoroutine(RestartAnim());

				RandomPanel.GetChild(28).gameObject.SetActive(true);
				break;
			case 29:
				StartCoroutine(RestartAnim());

				RandomPanel.GetChild(29).gameObject.SetActive(true);
				break;
			case 30:
				StartCoroutine(RestartAnim());

				RandomPanel.GetChild(30).gameObject.SetActive(true);
				break;
			case 31:
				StartCoroutine(RestartAnim());

				RandomPanel.GetChild(31).gameObject.SetActive(true);
				break;
			case 32:
				StartCoroutine(RestartAnim());

				RandomPanel.GetChild(32).gameObject.SetActive(true);
				break;
			case 33:
				StartCoroutine(RestartAnim());

				RandomPanel.GetChild(33).gameObject.SetActive(true);
				break;
			case 34:
				StartCoroutine(RestartAnim());

				RandomPanel.GetChild(34).gameObject.SetActive(true);
				break;
			case 35:
				StartCoroutine(RestartAnim());

				RandomPanel.GetChild(35).gameObject.SetActive(true);
				break;
			case 36:
				StartCoroutine(RestartAnim());

				RandomPanel.GetChild(36).gameObject.SetActive(true);
				break;
			case 37:
				StartCoroutine(RestartAnim());

				RandomPanel.GetChild(37).gameObject.SetActive(true);
				break;
			case 38:
				StartCoroutine(RestartAnim());

				RandomPanel.GetChild(38).gameObject.SetActive(true);
				break;
			case 39:
				StartCoroutine(RestartAnim());

				RandomPanel.GetChild(39).gameObject.SetActive(true);
				break;
			case 40:
				StartCoroutine(RestartAnim());

				RandomPanel.GetChild(40).gameObject.SetActive(true);
				break;

			default:
			break;

		}

	}

	public void OnTreasureAnimComplete()
	{
		SceneManager.LoadScene ("InssaGatcha");
		//TreasurePlayAnim = false;
		//TreasureanimPanel.SetActive (false);
	}

	IEnumerator RestartAnim(){
		animPanel.SetActive (false);
		yield return new WaitForSeconds (2.0f);

	}


}