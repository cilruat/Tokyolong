using UnityEngine;
using System.Collections;
using UnityEditor;

namespace I2.MiniGames
{
	[InitializeOnLoad]
	public class MiniGamesEditorManager
	{
		static MiniGamesEditorManager()
		{
			I2.I2Analytics.PluginsVersion["I2 MiniGames"] = MiniGame_Inspector.GetVersion();
			I2.I2Analytics.SendAnalytics("I2 MiniGames", MiniGame_Inspector.GetVersion()); // Tracks Unity version usage to know when is safe to discontinue support to old unity versions
		}
	}
	[CanEditMultipleObjects]
	public class MiniGame_Inspector : Editor
	{
		public virtual string HelpURL_Documentation { get { return  "http://goo.gl/cpeVTV"; } }

		public static string MainHelpURL_Documentation  = "http://inter-illusion.com/assets/MiniGamesManual/I2MiniGames.html";
		public static string HelpURL_forum 			= "http://inter-illusion.com/forum/minigames";
		public static string HelpURL_Tutorials		= "http://inter-illusion.com/tools/i2-minigames";
		public static string HelpURL_ReleaseNotes	= "http://inter-illusion.com/forum/minigames/433-release-notes";

		public static string GetVersion()
		{
			return "1.0.3 f1";
		}

		[MenuItem("Tools/I2 MiniGames/Help", false, 1 )]
		[MenuItem("Help/I2 MiniGames")]
		public static void MainHelp()
		{
			Application.OpenURL(MainHelpURL_Documentation);
		}

		[MenuItem("Tools/I2 MiniGames/Forum", false, 12 )]
		public static void URL_Forum(){	Application.OpenURL(HelpURL_forum);	}

		[MenuItem("Tools/I2 MiniGames/Tutorials", false, 13 )]
		public static void URL_Tutorials(){	Application.OpenURL(HelpURL_Tutorials);	}

		[MenuItem("Tools/I2 MiniGames/Release Notes", false, 14 )]
		public static void URL_ReleaseNotes(){	Application.OpenURL(HelpURL_ReleaseNotes);	}


		[MenuItem("Tools/I2 MiniGames/About", false, 31 )]
		public static void AboutWindow()
		{
			I2AboutWindow.DoShowScreen();
		}

		#region GUI
		
		public override void OnInspectorGUI()
		{
			DrawDefaultInspector ();
			
			GUILayout.Space (10);
			
			I2AboutWindow.OnGUI_Footer("I2 MiniGames", MiniGame_Inspector.GetVersion(), HelpURL_forum, HelpURL_Documentation);
		}

		#endregion


		#region Styles
		
		public static GUIStyle GUIStyle_Header {
			get{
				if (mGUIStyle_Header==null)
				{
					mGUIStyle_Header = new GUIStyle("HeaderLabel");
					mGUIStyle_Header.fontSize = 25;
					mGUIStyle_Header.normal.textColor = Color.Lerp(Color.white, Color.gray, 0.5f);
					mGUIStyle_Header.fontStyle = FontStyle.BoldAndItalic;
					mGUIStyle_Header.alignment = TextAnchor.UpperCenter;
				}
				return mGUIStyle_Header;
			}
		}
		static GUIStyle mGUIStyle_Header;
		
		public static GUIStyle GUIStyle_SubHeader {
			get{
				if (mGUIStyle_SubHeader==null)
				{
					mGUIStyle_SubHeader = new GUIStyle("HeaderLabel");
					mGUIStyle_SubHeader.fontSize = 13;
					mGUIStyle_SubHeader.fontStyle = FontStyle.Normal;
					mGUIStyle_SubHeader.margin.top = -50;
					mGUIStyle_SubHeader.alignment = TextAnchor.UpperCenter;
				}
				return mGUIStyle_SubHeader;
			}
		}
		static GUIStyle mGUIStyle_SubHeader;
		
		public static GUIStyle GUIStyle_Background {
			get{
				if (mGUIStyle_Background==null)
				{
					mGUIStyle_Background = new GUIStyle("AS TextArea");
					mGUIStyle_Background.overflow.left = 50;
					mGUIStyle_Background.overflow.right = 50;
					mGUIStyle_Background.overflow.top = -5;
					mGUIStyle_Background.overflow.bottom = 0;
				}
				return mGUIStyle_Background;
			}
		}
		static GUIStyle mGUIStyle_Background;
		
		#endregion

	}

	[CanEditMultipleObjects]
	[CustomEditor(typeof(TreasureHunt))]
	public class TreasureHunt_Inspector : MiniGame_Inspector
	{
		public override string HelpURL_Documentation { get { return  "http://inter-illusion.com/assets/MiniGamesManual/I2MiniGames.html?TreasureHunt.html"; } }
	}

	[CanEditMultipleObjects]
	[CustomEditor(typeof(PrizeWheel))]
	public class PrizeWheel_Inspector : MiniGame_Inspector
	{
		public override string HelpURL_Documentation { get { return  "http://inter-illusion.com/assets/MiniGamesManual/I2MiniGames.html?PrizeWheel.html"; } }
	}

}