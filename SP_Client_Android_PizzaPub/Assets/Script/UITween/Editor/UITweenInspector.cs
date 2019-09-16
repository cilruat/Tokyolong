using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CustomPropertyDrawer(typeof(TWParam))]
public class TWParamDrawer : PropertyDrawer
{
	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
	{
		GUILayout.Space(-18);

		SerializedProperty durationProp = property.FindPropertyRelative("duration");
		SerializedProperty delayProp = property.FindPropertyRelative("delay");
		SerializedProperty loopTypeProp = property.FindPropertyRelative("loopType");
		SerializedProperty curveTypeProp = property.FindPropertyRelative("curveType");

		TWCurve curveType = (TWCurve)curveTypeProp.enumValueIndex;

		EditorGUILayout.PropertyField(delayProp);
		EditorGUILayout.PropertyField(durationProp);
		EditorGUILayout.PropertyField(loopTypeProp);
		EditorGUILayout.PropertyField(curveTypeProp);

		switch (curveType)
		{
			case TWCurve.Linear:
			case TWCurve.Punch:
			case TWCurve.Shake:
			case TWCurve.Spring:
				break;
			case TWCurve.UseAnimationCurve:
				EditorGUILayout.PropertyField(property.FindPropertyRelative("animationCurve"));
				break;
			default:
				EditorGUILayout.PropertyField(property.FindPropertyRelative("curveSpeed"));
				break;
		}

		//EditorGUILayout.PropertyField(property.FindPropertyRelative("initStartVal"), new GUIContent("Use Start Value"));
		EditorGUILayout.PropertyField(property.FindPropertyRelative("destroyOnFinish"));
		EditorGUILayout.PropertyField(property.FindPropertyRelative("disableOnFinish"));

		GUILayout.Space(10);
	}
}

[CustomEditor(typeof(UITween), true)]
[CanEditMultipleObjects]
public class UITweenInspector : Editor
{
	public bool nestedGroup = false;
	UITween tween;
	UITweenGroup parentTweenGroup;

	static Color BACK_BTN_COLOR = new Color(1f, .3f, .1f);

	public override void OnInspectorGUI ()
	{
		if (EditorApplication.isPlaying)
		{
			base.OnInspectorGUI();
			return;
		}

		if (tween == null)
		{
			tween = target as UITween;
			tween.InitForInspector();
		}
		
		if (Application.isPlaying == false)
		{
			parentTweenGroup = tween.GetComponentInParent<UITweenGroup>();
			if (parentTweenGroup == null)
				parentTweenGroup = tween.GetComponent<UITweenGroup> ();
			
			if (parentTweenGroup != null && nestedGroup == false)
			{
				GUILayout.Space(10);

				EditorGUILayout.LabelField("This UITween is under UITweenGroup");

				GUI.color = BACK_BTN_COLOR;
				if (GUILayout.Button("Go to UITweenGroup & Foldout ME!"))
				{
					parentTweenGroup.FoldOutMe(tween);
					Selection.activeGameObject = parentTweenGroup.gameObject;
				}
				GUI.color = Color.white;

				if (parentTweenGroup.gameObject == tween.gameObject)
					return;
			}

			if (nestedGroup && parentTweenGroup != tween &&
				Selection.activeGameObject != tween.gameObject)
			{
				GUILayout.Space (6);
				if (GUILayout.Button("Go to GameObject '" + tween.name + "'"))
				{
					Selection.activeGameObject = tween.gameObject;
					return;
				}
				GUILayout.Space (6);
			}

			GUILayout.Space(6);

			DrawElapsedSlider(tween);

			if (parentTweenGroup != null || tween.startTag != TWTag.TagA)
			{
				GUILayout.Space(6);
				tween.startTag = (TWTag)EditorGUILayout.EnumPopup("Set Tag", tween.startTag);
			}

			GUILayout.Space (15);
		}

		base.OnInspectorGUI();

		if (Application.isPlaying == false)
		{
			if (tween.ShowCopyButton())
			{
				GUILayout.BeginHorizontal();
				if (GUILayout.Button("Copy to Start"))
					tween.CopyTo(true);
				if (GUILayout.Button("Copy to End"))
					tween.CopyTo(false);
				GUILayout.EndHorizontal();
			}
		}

		if (GUI.changed)
		{
			serializedObject.ApplyModifiedProperties();
			EditorUtility.SetDirty(target);
		}
	}

	float duration = 0f;
	public void DrawElapsedSlider (UITween tween, bool showLabel = true, TWTag startTag = TWTag.TagA)
	{
		if (tween.GetType() == typeof(UITweenGroup))
			duration = ((UITweenGroup)tween).GetTotalLength();
		else
			duration = tween.Duration;

		GUI.color = Color.yellow;

		if (tween.AnimateEditor(EditorGUILayout.Slider(showLabel ? "Elapsed" : "    ", tween.elapsed, 0, duration), false, startTag))
			GUI.changed = true;

		GUI.color = Color.white;
	}
}