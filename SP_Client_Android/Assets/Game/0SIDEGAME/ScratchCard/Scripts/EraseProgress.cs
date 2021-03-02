using ScratchCardAsset.Tools;
using UnityEngine;
using UnityEngine.Rendering;

namespace ScratchCardAsset
{
	public class EraseProgress : MonoBehaviour
	{
		public ScratchCard Card;
		public event ProgressHandler OnProgress;
		public event ProgressHandler OnCompleted;

		public delegate void ProgressHandler(float progress);

		private ScratchCard.ScratchMode scratchMode;
		private RenderTexture percentRenderTexture;
		private RenderTargetIdentifier rti;
		private CommandBuffer commandBuffer;
		private Mesh mesh;
		private float currentProgress;
		private bool isCompleted;

		#region MonoBehaviour Methods

		void Start()
		{
			Init();
		}

		void OnDestroy()
		{
			if (percentRenderTexture != null && percentRenderTexture.IsCreated())
			{
				percentRenderTexture.Release();
				Destroy(percentRenderTexture);
			}

			if (mesh != null)
			{
				Destroy(mesh);
			}

			if (commandBuffer != null)
			{
				commandBuffer.Release();
			}
		}

		void Update()
		{
			if (Card.Mode != scratchMode)
			{
				scratchMode = Card.Mode;
				ResetProgress();
			}

			if (Card.IsScratched && !isCompleted)
			{
				UpdateProgress();
			}
		}

		#endregion

		private void Init()
		{
			scratchMode = Card.Mode;
			commandBuffer = new CommandBuffer {name = "EraseProgress"};
			percentRenderTexture = new RenderTexture(1, 1, 0, RenderTextureFormat.ARGB32);
			rti = new RenderTargetIdentifier(percentRenderTexture);
			mesh = MeshGenerator.GenerateQuad(Vector3.one, Vector3.zero);
		}

		/// <summary>
		/// Calculates scratch progress
		/// </summary>
		private void CalcProgress()
		{
			if (!isCompleted)
			{
				var prevRenderTextureT = RenderTexture.active;
				RenderTexture.active = percentRenderTexture;
				var progressTexture = new Texture2D(percentRenderTexture.width, percentRenderTexture.height,
					TextureFormat.ARGB32, false, true);
				progressTexture.ReadPixels(new Rect(0, 0, percentRenderTexture.width, percentRenderTexture.height), 0,
					0);
				progressTexture.Apply();
				RenderTexture.active = prevRenderTextureT;
				var red = progressTexture.GetPixel(0, 0).r;
				currentProgress = red;
				if (OnProgress != null)
				{
					OnProgress(red);
					var completeValue = Card.Mode == ScratchCard.ScratchMode.Erase ? 1f : 0f;
					if (red == completeValue)
					{
						if (OnCompleted != null)
						{
							OnCompleted(red);
						}

						isCompleted = true;
					}
				}
			}
		}

		#region Public Methods

		public float GetProgress()
		{
			return currentProgress;
		}

		public void UpdateProgress()
		{
			GL.LoadOrtho();
			commandBuffer.Clear();
			commandBuffer.SetRenderTarget(rti);
			commandBuffer.ClearRenderTarget(false, true, Color.clear);
			commandBuffer.DrawMesh(mesh, Matrix4x4.identity, Card.Progress);
			Graphics.ExecuteCommandBuffer(commandBuffer);
			CalcProgress();
		}

		public void ResetProgress()
		{
			isCompleted = false;
		}

		#endregion
	}
}