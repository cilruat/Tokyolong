using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace KnifeFlip
{
public class OnScreenButtons : MonoBehaviour {

		//This script displays two button textures and when you press on them, it will take you to the menu or restart the level.

		public GUIStyle HomeButtonTexture;
		public GUIStyle RestartButtonTexture;

		void  OnGUI (){
			if(GUI.Button(new Rect (8,Screen.height - 80,80,80), "", HomeButtonTexture)) {
				Application.LoadLevel(0);
			}

			if(GUI.Button(new Rect(105,Screen.height - 80,80,80), "", RestartButtonTexture)) {
				Application.LoadLevel(Application.loadedLevel);
			}
		}
	}
	}