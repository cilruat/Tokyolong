using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RollSozu : MonoBehaviour {



	public RectTransform wheel;
	public Button rollingButton;
	bool rolling = false;

	public float initSpeed = 0f;
	public float breakSpeed = 0f;
	public float keepSpeedTimeMin, keepSpeedTimeMax;
	float currentTime = 0f;
	public float currentSpeed;
	public Image fillImage;

	private AudioSource SozuOn; //for use Audio source

	[SerializeField] private AudioClip sozuclip_1;

	void Start () {
		rollingButton.interactable = rolling == false;

		SozuOn = GetComponent<AudioSource> ();

	}

	void Update () 
	{		

		if (rolling) 
		{
			currentTime -= Time.deltaTime;


			if (currentTime <= 0) 
			{
				currentSpeed -= breakSpeed * Time.deltaTime;

			}

			wheel.Rotate(0, 0, -currentSpeed * Time.deltaTime);

			if (currentSpeed <= 0) //if Stopped
			{
				rolling = false;
				rollingButton.interactable = true;
			}


		}
	}

	public void SozuSE()
	{
		SozuOn.clip = sozuclip_1;
		SozuOn.Play ();
	}


	public void Roll()
		{

			rolling = true;
			currentSpeed = fillImage.fillAmount*initSpeed;
			
			currentTime = UnityEngine.Random.Range(keepSpeedTimeMin, keepSpeedTimeMax);


		} 

		//rollingButton.interactable = false;
		//rolling = true;
}
