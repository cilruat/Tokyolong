using UnityEngine;
using UnityEditor;
using System.Collections;using RFLib;



namespace RFLibEditor
{
	//RFHighScoreRenderer Custom Editor to force update of high score color changes
	[CustomEditor(typeof(RFHighScoreRenderer))]
	public class RFHighScoreRendererEditor : Editor 
	{
		override public void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			RFHighScoreRenderer rfhs = target as RFHighScoreRenderer;
			rfhs.UpdateColorHighScoreColor();
		}
	}



}