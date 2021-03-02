using UnityEngine;
using UnityEngine.UI;

namespace ScratchCardAsset
{
	public class ScratchCardManager : MonoBehaviour
	{
		public enum ScratchCardRenderType
		{
			MeshRenderer,
			SpriteRenderer,
			CanvasRenderer
		}

		public Camera MainCamera;
		public ScratchCardRenderType RenderType;
		public Sprite ScratchSurfaceSprite;
		public bool ScratchSurfaceSpriteHasAlpha;
		public float MaskProgressCutOffValue = 0.33f;
		public Texture EraseTexture;
		public Vector2 EraseTextureScale = Vector2.one;
		public bool InputEnabled = true;
		public ScratchCard Card;
		public ScratchCard.ScratchMode Mode;
		public EraseProgress Progress;
		public GameObject MeshCard;
		public GameObject SpriteCard;
		public GameObject ImageCard;
		public Shader MaskShader;
		public Shader BrushShader;
		public Shader MaskProgressShader;
		public Shader MaskProgressCutOffShader;

		private Material eraserMaterial;
		private const string MaskProgressCutOffField = "_CutOff";

		void Awake()
		{
			if (Card == null)
			{
				Debug.LogError("ScratchCard is null!");
				return;
			}

			if (Card.MainCamera == null)
			{
				Card.MainCamera = MainCamera != null ? MainCamera : Camera.main;
			}

			Material scratchSurfaceMaterial = null;
			if (Card.ScratchSurface == null)
			{
				scratchSurfaceMaterial = new Material(MaskShader) {mainTexture = ScratchSurfaceSprite.texture};
				Card.ScratchSurface = scratchSurfaceMaterial;
			}

			if (Card.Eraser == null)
			{
				eraserMaterial = new Material(BrushShader) {mainTexture = EraseTexture};
				Card.Eraser = eraserMaterial;
			}

			Card.BrushScale = EraseTextureScale;
			Card.Mode = Mode;

			if (Card.Progress == null)
			{
				var shader = ScratchSurfaceSpriteHasAlpha ? MaskProgressCutOffShader : MaskProgressShader;
				var progressMaterial = new Material(shader);
				if (ScratchSurfaceSpriteHasAlpha)
				{
					progressMaterial.SetFloat(MaskProgressCutOffField, MaskProgressCutOffValue);
				}

				Card.Progress = progressMaterial;
			}

			if (RenderType == ScratchCardRenderType.MeshRenderer)
			{
				MeshCard.SetActive(true);
				if (SpriteCard != null)
				{
					SpriteCard.SetActive(false);
				}

				if (ImageCard != null)
				{
					ImageCard.SetActive(false);
				}

				Card.Surface = MeshCard.transform;

				var rendererComponent = MeshCard.GetComponent<Renderer>();
				if (rendererComponent != null)
				{
					rendererComponent.material = scratchSurfaceMaterial;
				}
				else
				{
					Debug.LogError("Can't find Renderer component on " + MeshCard.name + " GameObject!");
				}
			}
			else if (RenderType == ScratchCardRenderType.SpriteRenderer)
			{
				if (MeshCard != null)
				{
					MeshCard.SetActive(false);
				}

				SpriteCard.SetActive(true);
				if (ImageCard != null)
				{
					ImageCard.SetActive(false);
				}

				Card.Surface = SpriteCard.transform;
				var sprite = SpriteCard.GetComponent<SpriteRenderer>();
				if (sprite != null)
				{
					sprite.sprite = ScratchSurfaceSprite;
					sprite.material = scratchSurfaceMaterial;
				}
				else
				{
					Debug.LogError("Can't find SpriteRenderer component on " + SpriteCard.name + " GameObject!");
				}
			}
			else if (RenderType == ScratchCardRenderType.CanvasRenderer)
			{
				if (MeshCard != null)
				{
					MeshCard.SetActive(false);
				}

				if (SpriteCard != null)
				{
					SpriteCard.SetActive(false);
				}

				ImageCard.SetActive(true);
				Card.Surface = ImageCard.transform;
				var image = ImageCard.GetComponent<Image>();
				if (image != null)
				{
					image.sprite = ScratchSurfaceSprite;
					image.material = scratchSurfaceMaterial;
				}
				else
				{
					Debug.LogError("Can't find Image component on " + ImageCard.name + " GameObject!");
				}
			}
		}

		public void SetEraseTexture(Texture texture)
		{
			eraserMaterial.mainTexture = texture;
		}

		public void ResetScratchCard()
		{
			Card.ResetRenderTexture();
		}
	}
}