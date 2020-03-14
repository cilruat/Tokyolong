using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class PopupSystem : MonoBehaviour {

	public GameObject popup;
	Animator anim;
	public GameObject mainCanvas;

	private void Awake()
	{
		anim = popup.GetComponent<Animator> ();
	}
		

	public void OpenPopUp()
	{
		popup.SetActive (true);
	}

	public void OnnClickOkay(string sceneName)
	{
		SceneChanger.LoadScene (sceneName, mainCanvas);
		ClosePopup();
	}

	public void OnClickCancel()
	{
		ClosePopup();
	}

	void ClosePopup()
	{
		//anim.SetTrigger ("close");
		popup.SetActive (false);

	}


}
