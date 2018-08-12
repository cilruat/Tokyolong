using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class Welcome : EditorWindow {

	bool groupEnabled;
	private GUIStyle welcomeStyle = null;
	[MenuItem("Window/Crazy ball game kit information panel")]

	public static void Initialize() {
		Welcome window = (Welcome)EditorWindow.GetWindow (typeof (Welcome), true, "Information panel");
		GUIStyle style = new GUIStyle();
		window.position = new Rect(196, 196, sizeWidth, sizeHeight);
		window.minSize = new Vector2(sizeWidth, sizeHeight);
		window.maxSize = new Vector2(sizeWidth, sizeHeight);
		window.welcomeStyle = style;
		window.Show();
	}
	
	static float sizeWidth = 630;
	static float sizeHeight = 650;
	void OnGUI() {
		if(welcomeStyle == null)
			return;

		if (GUI.Button(new Rect(10, 10, 300, 150), "OPEN CRAZY BALL DOCUMENTATION"))
			Application.OpenURL("http://www.finalbossgame.com/crazy-ball-game-template-unity3d/ReadMe.pdf");

		if (GUI.Button(new Rect(10, 300 + 30, 300, 150), "SEE THE YOUTUBE TRAILER"))
			Application.OpenURL("https://www.youtube.com/watch?v=ruCifWu76io");

		if (GUI.Button(new Rect(10, 150 + 20, 300, 150), "TEST THE ANDROID APK"))
			Application.OpenURL("http://www.finalbossgame.com/crazy-ball-game-template-unity3d/crazyball.apk");

		if (GUI.Button(new Rect(20 + 300, 150 + 20, 300, 150), "TEST THE WEBPLAYER "))
			Application.OpenURL("http://www.finalbossgame.com/crazy-ball-game-template-unity3d");

		if (GUI.Button(new Rect(20 + 300, 10, 300, 150), "RATE THIS ASSET"))
			Application.OpenURL("http://u3d.as/oeW");

		if (GUI.Button(new Rect(20 + 300, 300 + 30, 300, 150), "SEE OUR OTHER ASSETS"))
			Application.OpenURL("https://www.assetstore.unity3d.com/en/#!/search/page=1/sortby=popularity/query=publisher:11082");

		if (GUI.Button(new Rect(10, 450 + 40, 610, 150), "DON'T PROMPT ME AGAIN")) {
			DoClose();
			PlayerPrefs.SetInt("DocumentationOpened", 1);
			PlayerPrefs.Save();
		}
	}

	void DoClose() {
		Close();
	}
}

[InitializeOnLoad]
class StartupHelper {

	static StartupHelper() {
		EditorApplication.update += Startup;
	}

	static void Startup() {
		if(Time.realtimeSinceStartup < 1)
			return;

		EditorApplication.update -= Startup;
		if (!PlayerPrefs.HasKey("DocumentationOpened"))
			Welcome.Initialize();
	}
} 

