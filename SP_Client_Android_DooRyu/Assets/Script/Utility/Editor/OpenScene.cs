using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System;

public class OpenScene : EditorWindow
{
	[MenuItem("Window/Quick Open Scene %q")]
	static void Init ()
	{
		if (Application.isPlaying)
			return;
		
		EditorWindow.GetWindow<OpenScene>( false );
	}

    static string currentKey = KeyCode.A.ToString();
    string RegisterKey { get { return "quick_" + currentKey + "_cnt"; } }
    string RegisterValue { get { return "quick_" + currentKey + "_"; } }

	static List<string> registered = new List<string>();
	static List<string> notRegistered = new List<string>();
	static string[] forComboBox = null;

	void Awake ()
	{
        _Load (currentKey);
	}

	void OnLostFocus()
	{
		Close ();
	}

	static string[] prop = {
		"",
		"Move Up",
		"Move Down",
		"Remove"
	};

	void OnGUI ()
	{
		if (Application.isPlaying) {			
			Close ();
			return;
		}

		GUI.color = new Color (2f, 2f, 2f, 1f);

		bool checkShortCut = Event.current != null &&
			Event.current.isKey && Event.current.type == EventType.KeyDown;

		GUILayout.Space (5f);
		GUIStyle styleBtn = new GUIStyle (EditorStyles.miniButton);
		styleBtn.fixedWidth = 213f;
		styleBtn.fixedHeight = 22f;
		styleBtn.alignment = TextAnchor.MiddleLeft;
		styleBtn.fontSize = 12;
		styleBtn.fontStyle = FontStyle.Bold;

		GUIStyle styleProp = new GUIStyle (EditorStyles.popup);
		styleProp.fixedWidth = 16f;
		styleProp.fixedHeight = 22f;
		styleProp.alignment = TextAnchor.MiddleCenter;
		styleProp.fontSize = 12;
		styleProp.fontStyle = FontStyle.Bold;

		if (GUILayout.Button ("0. Empty Scene", styleBtn) ||
		    (checkShortCut && Event.current.keyCode == KeyCode.Alpha0)) {
			EditorSceneManager.NewScene (NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
			Close ();
			return;
		}

		for (int i = 0; i < registered.Count; i++) {
			string r = registered [i];
			r = r.Substring (r.LastIndexOf ("/") + 1);
			r = r.Replace (".unity", "");
			r = (i+1).ToString () + ". " + r;
			GUILayout.BeginHorizontal ();
			if (GUILayout.Button (r, styleBtn) ||
				( checkShortCut && Event.current.keyCode == ( KeyCode.Alpha1 + i ) ) ) {
				EditorSceneManager.OpenScene (registered [i], OpenSceneMode.Single);
				Close ();
				return;
			}
			int cmd = EditorGUILayout.Popup (0, prop, styleProp);
			GUILayout.EndHorizontal ();

			if (cmd > 0) {
				switch (cmd) {
				case 1:
					if (i > 0) {
						string s = registered [i - 1];
						registered [i - 1] = registered [i];
						registered [i] = s;
						_Save ();
					}
					break;
				case 2:
					if (i < registered.Count - 1) {
						string s = registered [i + 1];
						registered [i + 1] = registered [i];
						registered [i] = s;
						_Save ();
					}			
					break;
				case 3:
					registered.RemoveAt (i);
					notRegistered.Clear ();
					_SizeChanged ();
					_Save ();
					break;
				}
				break;			
			}
		}

		if (notRegistered.Count == 0) 
        {
			string[] all = AssetDatabase.GetAllAssetPaths ();
			foreach (string a in all)
				if (a.EndsWith (".unity"))
					notRegistered.Add (a);

			for (int i = 0; i < notRegistered.Count; i++) {
				bool removeThis = false;

				string n = notRegistered [i];
				if (n.Contains ("(OldResource)"))
					removeThis = true;
				
				if (n.Contains ("PostProcessing"))
					removeThis = true;

				foreach (string r in registered)
					if (r == n) {
						removeThis = true;
						break;
					}					

				if (removeThis) {
					notRegistered.RemoveAt (i);
					i--;
				}
			}

			forComboBox = new string[notRegistered.Count+1];
			forComboBox [0] = "Register New Scene";
			for (int i = 0; i < notRegistered.Count; i++)
				forComboBox [i+1] =
					notRegistered [i].Substring (7);
		}

		GUI.enabled = registered.Count < 9;

		GUI.color = new Color (2f, 1f, 0, 1f);
		GUIStyle stylePopup = new GUIStyle (EditorStyles.popup);
		stylePopup.fixedHeight = 22f;
		stylePopup.fontSize = 12;
		stylePopup.fontStyle = FontStyle.Bold;
		stylePopup.alignment = TextAnchor.MiddleCenter;

		int selected = EditorGUILayout.Popup (0, forComboBox, stylePopup);
		if (selected > 0) {
			selected--;
			_AddList (selected);
			return;
		}

        GUI.enabled = true;
        GUILayout.Space(5);
        GUILayout.BeginHorizontal();
        GUILayout.Space(minSize.x * .5f - 50);
        if(GUILayout.Button("<", GUILayout.Width(40), GUILayout.Height(20)))
        {
            int prevKey = Convert.ToInt32(currentKey.ToCharArray()[0]) - 1;
            if (prevKey < 65)
                return;
            
            _Load(Convert.ToChar(prevKey).ToString());
        }

        GUIStyle styleLabel = new GUIStyle();
        styleLabel.alignment = TextAnchor.LowerCenter;
        GUILayout.Label(currentKey, styleLabel, GUILayout.Width(20), GUILayout.Height(20));

        if(GUILayout.Button(">",  GUILayout.Width(40), GUILayout.Height(20)))
        {
            int nextKey = Convert.ToInt32(currentKey.ToCharArray()[0]) + 1;
            if (nextKey > 90)
                return;

            _Load(Convert.ToChar(nextKey).ToString());
        }

        GUILayout.EndHorizontal();

        _DownKey();
	}

	void _AddList( int selected )
	{
		registered.Add (notRegistered [selected]);

		_SizeChanged ();
		_Save ();

		notRegistered.Clear ();
	}

    void _Load(string key)
    {
        registered.Clear();

        currentKey = key;

        int cnt = EditorPrefs.GetInt(RegisterKey, 0);
        for (int i = 0; i < cnt; i++)
            registered.Add(EditorPrefs.GetString(RegisterValue + i.ToString(), "Error"));

        _SizeChanged();
    }

    void _Save()
    {
        EditorPrefs.SetInt(RegisterKey, registered.Count);
        for (int i = 0; i < registered.Count; i++)
            EditorPrefs.SetString(RegisterValue + i.ToString(), registered[i]);
    }

	void _SizeChanged()
	{
		float height = (float)(registered.Count + 1) * 24f;
		minSize = new Vector2 (240f, 54f + height);
		maxSize = new Vector2 (240f, 55f + height);
	}

    void _DownKey()
    {
        bool downKey = Event.current != null && Event.current.type == EventType.KeyDown;
        if (downKey == false)
            return;

        switch (Event.current.keyCode)
        {
            case KeyCode.A:
            case KeyCode.B:         
            case KeyCode.C:
            case KeyCode.D:
            case KeyCode.E:
            case KeyCode.F: 
            case KeyCode.G:
            case KeyCode.H:
            case KeyCode.I:
            case KeyCode.J: 
            case KeyCode.K: 
            case KeyCode.L: 
            case KeyCode.M:
            case KeyCode.N:
            case KeyCode.O: 
            case KeyCode.P:
            case KeyCode.Q: 
            case KeyCode.R:
            case KeyCode.S: 
            case KeyCode.T: 
            case KeyCode.U: 
            case KeyCode.V:
            case KeyCode.W: 
            case KeyCode.X: 
            case KeyCode.Y: 
            case KeyCode.Z:     _Load(Event.current.keyCode.ToString());    break;
        }

        Event.current.Use();
    }
}
