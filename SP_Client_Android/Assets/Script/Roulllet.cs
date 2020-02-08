using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Roulllet : MonoBehaviour {


	public RectTransform wheel;
	public Image finalImg;
	List<Image> contents;
	public Button rollingButton;
	bool rolling = false;
	public float initSpeed = 0f;
	public float breakSpeed = 0f;
	public float keepSpeedTimeMin, keepSpeedTimeMax;
	float currentTime = 0f;
	float currentSpeed = 0f;

	private AudioSource ButtonOn; //for use Audio source
	private AudioSource Plause;

	[SerializeField] private AudioClip clip_1;
	[SerializeField] private AudioClip plause_1;


	void Awake()
	{
		finalImg.gameObject.SetActive (false);
	}


	void Start () {

		ButtonOn = GetComponent<AudioSource> ();
		Plause = GetComponent<AudioSource> ();

		rollingButton.interactable = rolling == false;
		contents = new List<Image>();

		for (int i = 0; i < wheel.childCount; i++) 
		{
			contents.Add(wheel.GetChild(i).GetComponent<Image>());

		}
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
				float halfAng = 360 / contents.Count;
				float minAng = 360;
				Image targetImg = null;

				for (int i = 0; i < contents.Count; i++) 
				{
					Vector3 localDir = Quaternion.Euler (0, 0, halfAng + (i * 360 / contents.Count)) * Vector3.up;

					float ang = Vector3.Angle(wheel.TransformDirection(localDir), Vector3.up);

					if (ang <= minAng) 
					{
						minAng = ang;
						targetImg = contents [i];
					}
				}
				Plause.clip = plause_1;
				Plause.Play ();
				finalImg.gameObject.SetActive (true);
				finalImg.sprite = targetImg.sprite;
				rolling = false;
				rollingButton.interactable = true;
			}


		}
    }

	public void PlaySE()
	{
		ButtonOn.clip = clip_1;
		ButtonOn.Play ();
	}

    public void Roll()
	{
		finalImg.gameObject.SetActive (false);
		rollingButton.interactable = false;
		rolling = true;
		currentSpeed = initSpeed;
		currentTime = UnityEngine.Random.Range(keepSpeedTimeMin, keepSpeedTimeMax);
	}

}
