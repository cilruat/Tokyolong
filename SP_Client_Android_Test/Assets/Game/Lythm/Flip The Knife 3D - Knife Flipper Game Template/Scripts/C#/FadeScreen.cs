using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KnifeFlip
{
public class FadeScreen : MonoBehaviour {
	
		//This script plays a fade effect when you start the game on the Main Camera.

		public Texture imageFade;
		public float timerFade = 5;
		private bool  fadeaway = false;
		private float tempTime;
		private float time = 0.0f;

		void  Start (){
			fadeaway = true;
		}
		void  Update (){
			if (fadeaway){
				if(time < timerFade) time += Time.deltaTime;
				tempTime = Mathf.InverseLerp(0.0f, timerFade, time);
			}

			if(tempTime >= 1.0f) enabled = false;
		}

		void  OnGUI (){
			if(fadeaway){

				Color thisColor = GUI.color;
				thisColor.a = 1f - tempTime;
				GUI.color = thisColor;
				GUI.DrawTexture( new Rect(0, 0, Screen.width, Screen.height), imageFade);
			}
		}
	}
}