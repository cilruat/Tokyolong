using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace JumperStepUp
{
// YOU BETTER RUN By BITBOYS STUDIO.
public class FadeTexture : MonoBehaviour {

	[Header("Fade In/Out Texture")]

	public float FadeRate;// The fade velocity
	private Image image; // the image that will be faded
	public float targetAlpha; // used to know if the image is fading in or out.
	public bool isLogoScreen = false; // we use this to know if the fading image is the logo screen object.

	void Start () {
		
		this.image = this.GetComponent<Image>();
		if(this.image==null)
		{
			Debug.LogError("Error: No image on "+this.name);
		}
		this.targetAlpha = this.image.color.a;
	}

	// Update is called once per frame
	void Update () {
		Color curColor = this.image.color;
		float alphaDiff = Mathf.Abs(curColor.a-this.targetAlpha);
		if (alphaDiff>0.0001f)
		{
			curColor.a = Mathf.Lerp(curColor.a,targetAlpha,this.FadeRate*Time.unscaledDeltaTime);
			this.image.color = curColor;

		}


	}

	public void FadeOut()
	{
		this.targetAlpha = 0.0f;


		if (isLogoScreen) { // When perform the fade out to the logo screen image we start a coroutine that deactivates the object after 1 second.
			StartCoroutine (ImageOff ()); 
		}
	}

	public void FadeIn()
	{
		this.targetAlpha = 1.0f;

	}


	public IEnumerator ImageOff() // Disable the logo screen because we don't need it anymore.
	{

		yield return new WaitForSeconds(1f);


		this.gameObject.SetActive (false);

}
}
}