using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditorInternal;
using System.Reflection;
using System;

[CustomEditor(typeof(WaterFX))]
public class WaterFXEditor : Editor {

	SerializedProperty	m_targetCamera;
	SerializedProperty 	m_cullingMask;
	
	SerializedProperty	m_sortingLayer;
	SerializedProperty 	m_sortingOrderInLayer;
	
	SerializedProperty	m_qualityLevel;
	SerializedProperty 	m_yPosition;
	SerializedProperty	m_zOffset;
	SerializedProperty 	m_distorsionAmount;
	SerializedProperty	m_perspectiveLink;
	SerializedProperty	m_pixelated;
	SerializedProperty 	m_waterSpeed1;
	SerializedProperty	m_waterSpeed2;
	SerializedProperty 	m_displacementMap1;
	SerializedProperty 	m_displacementMap2;
	SerializedProperty 	m_transparency;
	SerializedProperty	m_waterTint;
	SerializedProperty	m_horizontalFlip;
	SerializedProperty	m_verticalFlip;
	SerializedProperty	m_waterHeight;

	void OnEnable()
	{
		m_targetCamera	= serializedObject.FindProperty("m_targetCamera");
		m_cullingMask	= serializedObject.FindProperty("m_cullingMask");
		m_sortingLayer			= serializedObject.FindProperty("m_sortingLayer");
		m_sortingOrderInLayer	= serializedObject.FindProperty("m_sortingOrderInLayer");	
		m_qualityLevel		= serializedObject.FindProperty("m_qualityLevel");
		m_yPosition			= serializedObject.FindProperty("m_yPosition");
		m_zOffset			= serializedObject.FindProperty("m_zOffset");
		m_distorsionAmount	= serializedObject.FindProperty("m_distorsionAmount");
		m_perspectiveLink	= serializedObject.FindProperty("m_perspectiveLink");
		m_pixelated			= serializedObject.FindProperty("m_pixelated");
		m_waterSpeed1		= serializedObject.FindProperty("m_waterSpeed1");
		m_waterSpeed2		= serializedObject.FindProperty("m_waterSpeed2");
		m_displacementMap1	= serializedObject.FindProperty("m_displacementMap1");
		m_displacementMap2	= serializedObject.FindProperty("m_displacementMap2");
		m_transparency		= serializedObject.FindProperty("m_transparency");
		m_waterTint			= serializedObject.FindProperty("m_waterTint");
		m_horizontalFlip			= serializedObject.FindProperty("m_horizontalFlip");
		m_verticalFlip			= serializedObject.FindProperty("m_verticalFlip");
		m_waterHeight			= serializedObject.FindProperty("m_waterHeight");
	}
	
	public override void OnInspectorGUI()
	{
		serializedObject.Update();
		EditorGUILayout.PropertyField(m_targetCamera);
		EditorGUILayout.PropertyField(m_cullingMask);
		// sorting Layer popup selection field
		{
			EditorGUILayout.BeginHorizontal();

			EditorGUILayout.BeginVertical(GUILayout.Width(EditorGUIUtility.labelWidth*0.98f));
			GUILayout.Label( m_sortingLayer.displayName);
			EditorGUILayout.EndVertical();
			//EditorGUILayout.Popup (_index, this.layer.sortingLayerNamess, GUILayout.ExpandWidth (true));
			//EditorGUILayout.LabelField(m_sortingLayer.displayName);
			EditorGUILayout.BeginVertical();
			string previousLayerName = m_sortingLayer.stringValue;
			String[] sortingLayers = GetSortingLayerNames();
			int previousLayerIndex = 0;
			bool layerFound = false;
			foreach(string name in sortingLayers)
			{
				if(name == previousLayerName)
				{
					layerFound = true;
					break;
				}
				previousLayerIndex++;
			}
			int index = 0;
			if(layerFound)
			{
				index = previousLayerIndex;
			}
			index = EditorGUILayout.Popup(index, sortingLayers);
			m_sortingLayer.stringValue = sortingLayers[index];
			EditorGUILayout.EndVertical();

			EditorGUILayout.EndHorizontal();

			//EditorGUILayout.LayerField("Layer for Objects:",previousLayerIndex);
		}

		EditorGUILayout.PropertyField(m_sortingOrderInLayer);
		EditorGUILayout.PropertyField(m_qualityLevel);
		EditorGUILayout.PropertyField(m_yPosition);
		EditorGUILayout.PropertyField(m_zOffset);
		EditorGUILayout.PropertyField(m_distorsionAmount);
		EditorGUILayout.PropertyField(m_perspectiveLink);
		EditorGUILayout.PropertyField(m_pixelated);
		EditorGUILayout.PropertyField(m_waterSpeed1);
		EditorGUILayout.PropertyField(m_waterSpeed2);
		EditorGUILayout.PropertyField(m_displacementMap1);
		EditorGUILayout.PropertyField(m_displacementMap2);
		EditorGUILayout.PropertyField(m_transparency);
		EditorGUILayout.PropertyField(m_waterTint);
		EditorGUILayout.PropertyField(m_horizontalFlip);
		EditorGUILayout.PropertyField(m_verticalFlip);
		EditorGUILayout.PropertyField(m_waterHeight);
		serializedObject.ApplyModifiedProperties();
	}


	public string[] GetSortingLayerNames() {
		Type internalEditorUtilityType = typeof(InternalEditorUtility);
		PropertyInfo sortingLayersProperty = internalEditorUtilityType.GetProperty("sortingLayerNames", BindingFlags.Static | BindingFlags.NonPublic);
		return (string[])sortingLayersProperty.GetValue(null, new object[0]);
	}

}
