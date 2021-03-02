using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.UI;

namespace ScratchCardAsset.Editor
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(ScratchCardManager))]
	public class ScratchCardManagerInspector : UnityEditor.Editor
	{
		private SerializedProperty camera;
		private SerializedProperty renderType;
		private SerializedProperty card;
		private SerializedProperty mode;
		private SerializedProperty scratchSprite;
		private SerializedProperty eraseTexture;
		private SerializedProperty eraseTextureScale;
		private SerializedProperty inputEnabled;
		private SerializedProperty progress;
		private SerializedProperty mesh;
		private SerializedProperty sprite;
		private SerializedProperty image;
		private SerializedProperty hasAlpha;
		private SerializedProperty maskProgressCutOffValue;
		private SerializedProperty maskShader;
		private SerializedProperty brushShader;
		private SerializedProperty maskProgressShader;
		private SerializedProperty maskProgressCutOffShader;
		private ScratchCard scratchCard;
		private EraseProgress eraseProgress;
		private Object scratchSurfaceSprite;
		private int frameId;

		void OnEnable()
		{
			camera = serializedObject.FindProperty("MainCamera");
			renderType = serializedObject.FindProperty("RenderType");
			card = serializedObject.FindProperty("Card");
			mode = serializedObject.FindProperty("Mode");
			scratchSprite = serializedObject.FindProperty("ScratchSurfaceSprite");
			hasAlpha = serializedObject.FindProperty("ScratchSurfaceSpriteHasAlpha");
			maskProgressCutOffValue = serializedObject.FindProperty("MaskProgressCutOffValue");
			eraseTexture = serializedObject.FindProperty("EraseTexture");
			eraseTextureScale = serializedObject.FindProperty("EraseTextureScale");
			inputEnabled = serializedObject.FindProperty("InputEnabled");
			progress = serializedObject.FindProperty("Progress");
			mesh = serializedObject.FindProperty("MeshCard");
			sprite = serializedObject.FindProperty("SpriteCard");
			image = serializedObject.FindProperty("ImageCard");
			maskShader = serializedObject.FindProperty("MaskShader");
			brushShader = serializedObject.FindProperty("BrushShader");
			maskProgressShader = serializedObject.FindProperty("MaskProgressShader");
			maskProgressCutOffShader = serializedObject.FindProperty("MaskProgressCutOffShader");
		}

		public override bool RequiresConstantRepaint()
		{
			return card.objectReferenceValue != null && scratchCard != null && scratchCard.RenderTexture != null &&
			       scratchCard.IsScratched && Time.frameCount > frameId;
		}

		private bool focused;
		public override void OnInspectorGUI()
		{
			frameId = Time.frameCount;
			serializedObject.Update();

			#region ScratchCardManagerParameters
			
			EditorGUI.BeginDisabledGroup(Application.isPlaying);
			EditorGUI.BeginChangeCheck();
			
			EditorGUILayout.PropertyField(card, new GUIContent("Scratch Card"));
			if (card.objectReferenceValue == null)
			{
				EditorGUILayout.HelpBox("Scratch Card is null, please set reference to Scratch Card", MessageType.Warning);
			}
			
			EditorGUILayout.PropertyField(progress, new GUIContent("Erase Progress"));
			if (progress.objectReferenceValue == null)
			{
				EditorGUILayout.HelpBox("Erase Progress is null, please set reference to Erase Progress", MessageType.Warning);
			}
			
			EditorGUILayout.PropertyField(camera, new GUIContent("Main Camera"));
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(renderType, new GUIContent("Render Type"));
			if (EditorGUI.EndChangeCheck())
			{
				var renderObjects = new []
				{
					mesh.objectReferenceValue,
					sprite.objectReferenceValue, 
					image.objectReferenceValue
				};
				for (var i = 0; i < renderObjects.Length; i++)
				{
					if (renderObjects[i] != null)
					{
						((GameObject) renderObjects[i]).SetActive(i == renderType.enumValueIndex);
					}
				}
			}
			
			#region CardType

			var cannotSetNativeSize = false;
			if (renderType.enumValueIndex == 0)
			{
				EditorGUILayout.PropertyField(mesh, new GUIContent("Mesh Card"));
				if (mesh.objectReferenceValue == null)
				{
					EditorGUILayout.HelpBox("Mesh Card is null, please set reference to Mesh Card", MessageType.Warning);
					cannotSetNativeSize = true;
				}
			}
			if (renderType.enumValueIndex == 1)
			{
				EditorGUILayout.PropertyField(sprite, new GUIContent("Sprite Card"));
				if (sprite.objectReferenceValue == null)
				{
					EditorGUILayout.HelpBox("Sprite Card is null, please set reference to Sprite Card", MessageType.Warning);
					cannotSetNativeSize = true;
				}
			}
			if (renderType.enumValueIndex == 2)
			{
				EditorGUILayout.PropertyField(image, new GUIContent("Image Card"));
				if (image.objectReferenceValue == null)
				{
					EditorGUILayout.HelpBox("Image Card is null, please set reference to Image Card", MessageType.Warning);
					cannotSetNativeSize = true;
				}
			}
			var cardManagerChanged = EditorGUI.EndChangeCheck();
			
			#endregion

			#region CardImage

			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(scratchSprite, new GUIContent("Sprite"));
			if (EditorGUI.EndChangeCheck())
			{
				var cardRenderType = (ScratchCardManager.ScratchCardRenderType)renderType.enumValueIndex;
				switch (cardRenderType)
				{
					case ScratchCardManager.ScratchCardRenderType.MeshRenderer:
						var meshRendererComponent = ((GameObject) mesh.objectReferenceValue).GetComponent<MeshRenderer>();
						if (meshRendererComponent != null && meshRendererComponent.sharedMaterial != null)
						{
							Undo.RecordObject(meshRendererComponent.transform, "Set Sprite");
							meshRendererComponent.sharedMaterial.mainTexture = ((Sprite) scratchSprite.objectReferenceValue).texture;
						}
						break;
					case ScratchCardManager.ScratchCardRenderType.SpriteRenderer:
						var spriteRendererComponent = ((GameObject) sprite.objectReferenceValue).GetComponent<SpriteRenderer>();
						if (spriteRendererComponent != null)
						{
							Undo.RecordObject(spriteRendererComponent.transform, "Set Sprite");
							spriteRendererComponent.sprite = scratchSprite.objectReferenceValue as Sprite;
						}
						break;
					case ScratchCardManager.ScratchCardRenderType.CanvasRenderer:
						var imageComponent = ((GameObject) image.objectReferenceValue).GetComponent<Image>();
						if (imageComponent != null)
						{
							Undo.RecordObject(imageComponent.rectTransform, "Set Sprite");
							imageComponent.sprite = scratchSprite.objectReferenceValue as Sprite;
						}
						break;
				}
			}
			
			#endregion

			#region AlphaCutOff
			
			if (hasAlpha.boolValue)
			{
				EditorGUI.BeginDisabledGroup(true);
				EditorGUILayout.PropertyField(hasAlpha, new GUIContent("Sprite Has Alpha"));
				EditorGUI.EndDisabledGroup();
				EditorGUILayout.Slider(maskProgressCutOffValue, 0f, 1f, new GUIContent("Mask Progress Cut Off"));
			}
			
			#endregion
			
			#region Shaders

			if (maskShader.objectReferenceValue == null)
			{
				EditorGUILayout.PropertyField(maskShader, new GUIContent("Mask Shader"));
			}

			if (brushShader.objectReferenceValue == null)
			{
				EditorGUILayout.PropertyField(brushShader, new GUIContent("Brush Shader"));
			}

			if (maskProgressShader.objectReferenceValue == null)
			{
				EditorGUILayout.PropertyField(maskProgressShader, new GUIContent("Mask Progress Shader"));
			}

			if (maskProgressCutOffShader.objectReferenceValue == null)
			{
				EditorGUILayout.PropertyField(maskProgressCutOffShader, new GUIContent("Mask Progress Cut Off Shader"));
			}
			
			#endregion
			
			EditorGUI.EndDisabledGroup();
			
			#endregion

			DrawHorizontalLine();

			#region ScartchCardParams

			var hasScratchCardReference = card.objectReferenceValue == null;
			EditorGUI.BeginDisabledGroup(hasScratchCardReference);
			EditorGUILayout.LabelField("Scratch Card Parameters:");
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(eraseTexture, new GUIContent("Brush Texture"));
			var brushTextureChanged = EditorGUI.EndChangeCheck();
			var brushScaleChanged = false;
			if (eraseTexture.objectReferenceValue != null)
			{
				if (scratchCard != null)
				{
					eraseTextureScale.vector2Value = scratchCard.BrushScale;
				}
				EditorGUI.BeginChangeCheck();
				EditorGUILayout.PropertyField(eraseTextureScale, new GUIContent("Brush Texture Scale"));
				brushScaleChanged = EditorGUI.EndChangeCheck();
			}
			
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(inputEnabled, new GUIContent("Input Enabled"));
			var inputEnableChanged = EditorGUI.EndChangeCheck();
			
			if (scratchCard != null)
			{
				mode.enumValueIndex = (int) scratchCard.Mode;
			}

			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(mode, new GUIContent("Scratch Mode"));
			var scratchModeChanged = EditorGUI.EndChangeCheck();
			EditorGUI.EndDisabledGroup();

			#endregion

			DrawHorizontalLine();

			#region SetNativeSize
			
			EditorGUI.BeginDisabledGroup(cannotSetNativeSize);
			if (GUILayout.Button("Set Native Size"))
			{
				var cardRenderType = (ScratchCardManager.ScratchCardRenderType)renderType.enumValueIndex;
				switch (cardRenderType)
				{
					case ScratchCardManager.ScratchCardRenderType.MeshRenderer:
						var meshRendererComponent = (mesh.objectReferenceValue as GameObject).GetComponent<MeshRenderer>();
						if (meshRendererComponent != null && meshRendererComponent.sharedMaterial != null && meshRendererComponent.sharedMaterial.mainTexture != null)
						{
							Undo.RecordObject(meshRendererComponent.transform, "Set Native Size");
							var texture = meshRendererComponent.sharedMaterial.mainTexture;
							meshRendererComponent.transform.localScale = new Vector3(texture.width / 100f, texture.height / 100f, 1f);
						}
						break;
					case ScratchCardManager.ScratchCardRenderType.SpriteRenderer:
						var spriteRendererComponent = (sprite.objectReferenceValue as GameObject).GetComponent<SpriteRenderer>();
						if (spriteRendererComponent != null)
						{
							Undo.RecordObject(spriteRendererComponent.transform, "Set Native Size");
							spriteRendererComponent.transform.localScale = Vector3.one;
						}
						break;
					case ScratchCardManager.ScratchCardRenderType.CanvasRenderer:
						var imageComponent = (image.objectReferenceValue as GameObject).GetComponent<Image>();
						if (imageComponent != null)
						{
							Undo.RecordObject(imageComponent.rectTransform, "Set Native Size");
							imageComponent.SetNativeSize();
						}
						break;
				}
			}
			EditorGUI.EndDisabledGroup();
			
			#endregion

			#region CheckForAlpha

			if (scratchSurfaceSprite != scratchSprite.objectReferenceValue && scratchSprite.objectReferenceValue != null)
			{
				var path = AssetDatabase.GetAssetPath(scratchSprite.objectReferenceValue);
				var importer = (TextureImporter) AssetImporter.GetAtPath(path);
				if (importer != null)
				{
					hasAlpha.boolValue = importer.DoesSourceTextureHaveAlpha();
					scratchSurfaceSprite = scratchSprite.objectReferenceValue;
				}
			}
			
			#endregion

			#region Apply

			if (card.objectReferenceValue != null)
			{
				scratchCard = card.objectReferenceValue as ScratchCard;
				if (scratchCard != null)
				{
					var scratchCardChanged = false;
					if (brushTextureChanged && scratchCard.Eraser != null)
					{
						scratchCard.Eraser.mainTexture = eraseTexture.objectReferenceValue as Texture2D;
						scratchCardChanged = true;
					}
					
					if (brushScaleChanged)
					{
						Undo.RecordObject(scratchCard, "Set Brush Scale");
						scratchCard.BrushScale = eraseTextureScale.vector2Value;
						scratchCardChanged = true;
					}

					if (inputEnableChanged)
					{
						scratchCard.InputEnabled = inputEnabled.boolValue;
						scratchCardChanged = true;
					}

					if (scratchModeChanged)
					{
						scratchCard.Mode = (ScratchCard.ScratchMode) mode.enumValueIndex;
						scratchCardChanged = true;
					}

					if (scratchCard.RenderTexture != null)
					{
						DrawHorizontalLine();
						var rect = GUILayoutUtility.GetRect(160, 120, GUILayout.ExpandWidth(true));
						GUI.DrawTexture(rect, scratchCard.RenderTexture, ScaleMode.ScaleToFit);
						DrawHorizontalLine();

						if (Application.isPlaying)
						{
							if (eraseProgress == null)
							{
								eraseProgress = progress.objectReferenceValue as EraseProgress;
							}

							if (eraseProgress != null)
							{
								EditorGUILayout.LabelField(string.Format("Erase progress: {0}", eraseProgress.GetProgress()));
							}

							if (GUILayout.Button("Clear"))
							{
								scratchCard.ClearInstantly();
								if (progress.objectReferenceValue != null)
								{
									if (eraseProgress != null)
									{
										eraseProgress.ResetProgress();
										eraseProgress.UpdateProgress();
									}
								}
							}

							if (GUILayout.Button("Fill"))
							{
								scratchCard.FillInstantly();
								if (progress.objectReferenceValue != null)
								{
									if (eraseProgress != null)
									{
										eraseProgress.UpdateProgress();
									}
								}
							}
						}
					}

					if (cardManagerChanged)
					{
						MarkAsDirty(target);
					}

					if (scratchCardChanged)
					{
						MarkAsDirty(scratchCard);
					}
				}
			}
			serializedObject.ApplyModifiedProperties();
			
			#endregion
		}

		private void MarkAsDirty(Object objectTarget)
		{
			if (!Application.isPlaying)
			{
				var component = objectTarget as Component;
				if (component != null)
				{
					EditorUtility.SetDirty(component);
					EditorSceneManager.MarkSceneDirty(component.gameObject.scene);
				}
			}
		}

		private void DrawHorizontalLine()
		{
			GUILayout.Space(5f);
			EditorGUI.DrawRect(EditorGUILayout.GetControlRect(false, 2f), Color.gray);
			GUILayout.Space(5f);
		}
	}
}