using UnityEngine;
using UnityEditor;
using System.Collections;
using RFLib;

namespace RFLibEditor
{


	public class RFHighScoreViewMenuOption:MonoBehaviour
	{
		[MenuItem("GameObject/RFLib/Create RFHighScores")]
		static void  CreateRFSceneChangeManager()
		{
			GameObject prefab = RFEditorUtils.MakePrefabCopy("Assets/RFLib/RFHighScores/Prefabs/RFHighScoresManager.prefab","RFHighScoresManager" );
			if(prefab == null)
				EditorUtility.DisplayDialog("RFHighScoresManager Problem", "RFHighScoresManager could not be created.", "OK");
		}

	}

	// RFHighScoreViewer custom editor to allow clearing and setting of default scores in addition to the 
	// standard editing behavior
	[CustomEditor(typeof(RFHighScoreViewer))]
	public class RFHighScoreViewEditor : Editor
	{


		public override void OnInspectorGUI ()
		{
			RFHighScoreViewer rfHsv = target as RFHighScoreViewer;
			base.OnInspectorGUI ();

			EditorGUILayout.BeginVertical();

			if( GUILayout.Button( "Clear Scores" ) )
			{
				if(rfHsv.HighScoresManager.LoadHighScores(rfHsv.HighscoresFile))
					rfHsv.ClearScores();
			}
			if( GUILayout.Button( "Set Default Scores" ) )
			{
				rfHsv.HighScoresManager.DefaultScoreList = rfHsv.DefaultScores;
				rfHsv.HighScoresManager.SetScoresAsDefault();
				rfHsv.HighScoresManager.SaveHighScores(rfHsv.HighscoresFile);
				rfHsv.LoadAndDisplayScores();

			}

			EditorGUILayout.EndVertical();
		}


	}

}