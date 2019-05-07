using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(UITweenGroup))]
public class UITweenGroupInspector : UITweenInspector
{
	static TWTag selectedTag;
	static Color LIGHT_GRAY = new Color(.66f, .66f, .66f);
	UITweenGroup tweenGroup;
	float maxAnimLength;
	bool isTweenGroup;

	public override void OnInspectorGUI ()
	{
		if (EditorApplication.isPlaying)
		{
			base.OnInspectorGUI();
			return;
		}

		if (tweenGroup == null)
		{
			tweenGroup = target as UITweenGroup;
			tweenGroup.InitForInspector();

			if (tweenGroup.tweens == null)
				tweenGroup.tweens = new List<UITween>();
		}

		maxAnimLength = tweenGroup.GetTotalLength();

		GUILayout.Space(15);

		if (tweenGroup.transform.parent == null ||
			tweenGroup.transform.parent.GetComponentInParent<UITweenGroup>() == null)
			selectedTag = tweenGroup.startTag = (TWTag)EditorGUILayout.EnumPopup("* Group Type", tweenGroup.startTag);

		GUILayout.Space(6);

		GUI.color = Color.green;
		if (tweenGroup.AnimateEditor(EditorGUILayout.Slider("Total Animation", tweenGroup.elapsed, 0, maxAnimLength), false, selectedTag))
			GUI.changed = true;
		GUI.color = Color.white;

		GUILayout.Space(15);
		
		tweenGroup.autoStart = EditorGUILayout.Toggle("Auto Start", tweenGroup.autoStart);
		tweenGroup.param.Delay(EditorGUILayout.FloatField("Group Delay", tweenGroup.Delay));

		if (tweenGroup.gameObject.activeInHierarchy)
			tweenGroup.RegistTweens();

		foreach (UITween tween in tweenGroup.tweens)
			OnAnimationGUI(tween);

		if (GUI.changed)
		{
			EditorUtility.SetDirty(target);
			EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
		}
	}

	Dictionary<UITween, UITweenInspector> dicEditor = new Dictionary<UITween, UITweenInspector>();
	void OnAnimationGUI (UITween tween)
	{
		GUILayout.Space(10);

		isTweenGroup = tween.GetType() == typeof(UITweenGroup);

		EditorGUILayout.BeginHorizontal();

		GUI.color = (isTweenGroup || tween.startTag == selectedTag) ? Color.green : LIGHT_GRAY;
		tween.foldout = EditorGUILayout.Foldout(tween.foldout, string.Format("[{0}] {1}", (isTweenGroup ? "NESTED GROUP" : tween.GetType().ToString().Replace("UITween","")), tween.name));
		GUI.color = Color.white;

		if (dicEditor.ContainsKey(tween) == false)
			dicEditor.Add(tween, UITweenInspector.CreateEditor(tween) as UITweenInspector);
		else if (dicEditor[tween] == null)
			dicEditor[tween] = UITweenInspector.CreateEditor(tween) as UITweenInspector;

		if (tween.foldout == false)
		{
			dicEditor[tween].DrawElapsedSlider(tween, false, selectedTag);
			EditorGUILayout.EndHorizontal();
		}
		else
		{
			EditorGUILayout.EndHorizontal();

			if (isTweenGroup)
			{
				GUI.color = Color.yellow;
				GUILayout.Label("Start Nested Group >>>>>>>>>>>>>>>>>>>>>>>>>>>>", EditorStyles.boldLabel);
				GUI.color = Color.white;
			}

			dicEditor[tween].nestedGroup = true;
			dicEditor[tween].OnInspectorGUI();

			if (isTweenGroup)
			{
				GUI.color = Color.yellow;
				GUILayout.Label (">>>>>>>>>>>>>>>>>>>>>>>>>>>> End Nested Group", EditorStyles.boldLabel);
				GUI.color = Color.white;
				GUILayout.Space(20);
			}
		}
	}
}