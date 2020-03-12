using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class PopupSystem : MonoBehaviour {


	public GameObject popup;
	Animator anim;

	public static PopupSystem instance{ get; private set; }
	//public Text txtTitle, txtContent;
	Action onClickOkay, onClickCancel;


	private void Awake()
	{
		instance = this;
		anim = popup.GetComponent<Animator> ();

	}

	/*private void Update()
	{
		if (anim.GetCurrentAnimatorStateInfo (0).IsName ("close")) 
		{
			if (anim.GetCurrentAnimatorStateInfo (0).normalizedTime >= 1) 
			{
				popup.SetActive (false);
			}
		}
	}*/


	public void OpenPopUp(/*string title, string contents,*/Action onClickOkay, Action onClickCancel)
	{
		//txtTitle.text = title;
		//txtContent.text = contents;
		/*realImg = real;
		gameImg = game;
		BackImg = Back;*/
		this.onClickOkay = onClickOkay;
		this.onClickCancel = onClickCancel;
		popup.SetActive (true);
	}

	public void OnnClickOkay()
	{
		if (onClickOkay != null) 		
		{
			onClickOkay ();
		}
		ClosePopup();
	}

	public void OnClickCancel()
	{
		if (onClickCancel != null) 
		{
			onClickCancel ();
		}
		ClosePopup();
	}

	void ClosePopup()
	{
		//anim.SetTrigger ("close");
		popup.SetActive (false);

	}

}
