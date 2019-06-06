using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace BlockJumpRabbit
{
public class SoundButtons : MonoBehaviour {

	public Sprite spriteButtonOn;
	public Sprite spriteButtonOff;

	private SoundController soundController;
	private Image imageButton;

	void Awake(){
		soundController = FindObjectOfType<SoundController>();
		imageButton = GetComponent<Image> ();
	}

	// Method call from UI
	public void ChangeSound(){
		if (soundController.isSoundOn)
			SoundOff ();
		else
			SoundOn ();
	}
		
	private void SoundOn(){
		imageButton.sprite = spriteButtonOn;
		soundController.SoundOn ();
	}

	private void SoundOff(){
		imageButton.sprite = spriteButtonOff;
		soundController.SoundOff ();
	}

	void OnEnable(){
		if (soundController.isSoundOn)
			imageButton.sprite = spriteButtonOn;
		else
			imageButton.sprite = spriteButtonOff;
	}
}
}