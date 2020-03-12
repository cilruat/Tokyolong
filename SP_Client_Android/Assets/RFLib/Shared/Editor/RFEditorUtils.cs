using UnityEngine;
using System.Collections;
using UnityEditor;

namespace RFLibEditor
{
	public class RFEditorUtils
	{

		// Get the required layer int; prompt to add if not found.
		public static int GetRequiredLayer(string layerName)
		{
			int requiredLayer = LayerMask.NameToLayer(layerName);

			// Make sure the proper layer is set up!
			if( requiredLayer == -1)
			{
				if(EditorUtility.DisplayDialog("Layer Required", 
					"Components in the libarary require layer: " + layerName + "\n Add it now?", "Yes", "I'll do it later"))
				{

					AddRequiredLayerToManager(layerName);
				}
			}
			return requiredLayer;
		}

		// Add a layer to the layer manager
		public static int AddRequiredLayerToManager(string layerName)
		{

			int layerStart = 8;

			SerializedObject tagManager = new SerializedObject (AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
			SerializedProperty layers = tagManager.FindProperty("layers");
			bool layerAdded = false;

			int layerNumber = -1;

			if(layers != null && layers.isArray)
			{

				for(int cnt = layerStart; cnt < 32; cnt ++)
				{
					SerializedProperty layer = layers.GetArrayElementAtIndex(cnt);
					if(string.IsNullOrEmpty(layer.stringValue))
					{
						layer.stringValue = layerName;
						layerAdded = true;
						layerNumber = cnt;
						break;
					}

				}
			}

			string msg = "Layer added successfully";
			if(layerAdded == false)
			{
				msg = "Could not add required layer.";
			}
			else
			{
				tagManager.ApplyModifiedProperties();
			}
			EditorUtility.DisplayDialog("", msg, "OK");

			return layerNumber;

		}


		// Helper function useful in creating an instance of a prefab in the game hierarchy, but then removing the 
		// prefab connection
		public static GameObject MakePrefabCopy(string objectPath, string newObjectName, bool auto_select = true)
		{
			GameObject outgo = null;

			GameObject go = AssetDatabase.LoadAssetAtPath<GameObject>(objectPath);
			if(go != null)
			{
				GameObject tmpGo = PrefabUtility.CreatePrefab("Assets/RFASSET_TMP.prefab", go);
				outgo = PrefabUtility.InstantiatePrefab( tmpGo ) as GameObject;

				if(outgo)
				{
					outgo.name = newObjectName;
					if(auto_select)
						Selection.activeObject = outgo;
				}

				if(AssetDatabase.Contains(tmpGo))
					AssetDatabase.DeleteAsset("Assets/RFASSET_TMP.prefab");

				PrefabUtility.DisconnectPrefabInstance(outgo);

			}

			return outgo;
		}


		
	}

}
