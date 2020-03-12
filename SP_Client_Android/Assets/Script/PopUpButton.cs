using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class PopUpButton : MonoBehaviour {

	public void OnClickMyButton()
	{
		PopupSystem.instance.OpenPopUp(

			() =>
			{
				Debug.Log("okay");
			},
			() =>
			{
				Debug.Log("Cancel");
			}
			);

	}
		
}
