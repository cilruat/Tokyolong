using UnityEngine;
using System.Collections;
namespace Stealth
{
public class ExitArrowHelper : MonoBehaviour {
		
	///*************************************************************************///
	/// In a live example, we might need to show the exit route to the player.
	/// This animated texture object can be positioned over the exit to help player
	/// locate the exit.  
	///*************************************************************************///

	public float delayToStart = 2.0f; 		//in seconds
	public float showTime = 3.0f;			//how many seconds to show the arrow
	public int cycleStep = 30;				//delay between each appearance

	private int cycleDelay;			
	private bool  canShow = false;			//control flag

	void Awake (){
		cycleDelay = cycleStep;
		transform.GetComponent<Renderer>().enabled = false;			//hide the helper
		//make it transparent
		transform.GetComponent<Renderer>().material.color = new Color(transform.GetComponent<Renderer>().material.color.r,
		                                                transform.GetComponent<Renderer>().material.color.g,
		                                                transform.GetComponent<Renderer>().material.color.b,
		                                                0);
	}

	IEnumerator Start (){
		yield return new WaitForSeconds(delayToStart);			//Wait
		transform.GetComponent<Renderer>().enabled = true;						//show the helper
		canShow = true;											//set the flag
		StartCoroutine(Stop());									//Stop the animation and cycle again.
	}

	void Update (){
		helperCycle();
		ManageCycles();
	}
		
	void ManageCycles (){
		if(Time.timeSinceLevelLoad > cycleDelay) {
			//show the helper again
			print("Show Helper Again at Second: #" + cycleDelay);
			transform.GetComponent<Renderer>().enabled = true;
			canShow = true;
			StartCoroutine(Stop());
			
			//cycle again
			cycleDelay += cycleStep;
		}
	}
		
	void helperCycle (){
		if(canShow) {
			if(transform.GetComponent<Renderer>().material.color.a < 1)
				transform.GetComponent<Renderer>().material.color += new Color(0,0,0, Time.deltaTime * 2.0f);
		}
		else {
			if(transform.GetComponent<Renderer>().material.color.a > 0)
				transform.GetComponent<Renderer>().material.color -= new Color(0,0,0, Time.deltaTime * 2.0f);
		}
	}

	IEnumerator Stop (){
		yield return new WaitForSeconds(showTime);
		canShow = false;
		yield return new WaitForSeconds(delayToStart);
		transform.GetComponent<Renderer>().enabled = false;
	}
	}
}