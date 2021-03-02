using UnityEngine;

namespace ScratchCardAsset.Core
{
	/// <summary>
	/// Process Input for ScratchCard
	/// </summary>
	public class ScratchCardInput
	{
		#region Events

		public event ScratchHandler OnScratch;
		public event ScratchStartHandler OnScratchStart;
		public event ScratchLineHandler OnScratchLine;
		public event ScratchHoleHandler OnScratchHole;
		public delegate Vector2 ScratchHandler(Vector2 position);
		public delegate void ScratchStartHandler();
		public delegate void ScratchLineHandler(Vector2 start, Vector2 end);
		public delegate void ScratchHoleHandler(Vector2 position);
		
		#endregion

		public bool IsScratching
		{
			get
			{
				if (isScratching != null)
				{
					foreach (var scratching in isScratching)
					{
						if (scratching)
							return true;
					}
				}
				return false;
			}
		}

		private ScratchCard scratchCard;
		private Vector2[] eraseStartPositions;
		private Vector2[] eraseEndPositions;
		private Vector2 erasePosition;
		private bool[] isScratching;
		private bool[] isStartPosition;
#if UNITY_WEBGL
	private bool isWebgl = true;
#else
		private bool isWebgl = false;
#endif

		private const int MaxTouchCount = 10;

		public ScratchCardInput(ScratchCard card)
		{
			scratchCard = card;
			isScratching = new bool[MaxTouchCount];
			isStartPosition = new bool[MaxTouchCount];
			eraseStartPositions = new Vector2[MaxTouchCount];
			eraseEndPositions = new Vector2[MaxTouchCount];
			for (var i = 0; i < isStartPosition.Length; i++)
			{
				isStartPosition[i] = true;
			}
		}

		public void Update()
		{
			if (!scratchCard.InputEnabled)
				return;

			if (Input.touchSupported && Input.touchCount > 0 && !isWebgl)
			{
				foreach (var touch in Input.touches)
				{
					var fingerId = touch.fingerId + 1;
					if (touch.phase == TouchPhase.Began)
					{
						isScratching[fingerId] = false;
						isStartPosition[fingerId] = true;
					}
					if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
					{
						TryScratch(fingerId, touch.position);
					}
					if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
					{
						isScratching[fingerId] = false;
					}
				}
			}
			else
			{
				if (Input.GetMouseButtonDown(0))
				{
					isScratching[0] = false;
					isStartPosition[0] = true;
				}
				if (Input.GetMouseButton(0))
				{
					TryScratch(0, Input.mousePosition);
				}
				if (Input.GetMouseButtonUp(0))
				{
					isScratching[0] = false;
				}
			}
		}

		private void TryScratch(int fingerId, Vector2 position)
		{
			if (OnScratch != null)
			{
				erasePosition = OnScratch(position);
			}

			if (isStartPosition[fingerId])
			{
				eraseEndPositions[fingerId] = eraseStartPositions[fingerId];
				eraseStartPositions[fingerId] = erasePosition;
			}
			else
			{
				eraseEndPositions[fingerId] = erasePosition;
			}

			isStartPosition[fingerId] = !isStartPosition[fingerId];

			if (!isScratching[fingerId])
			{
				eraseEndPositions[fingerId] = eraseStartPositions[fingerId];
				isScratching[fingerId] = true;
			}
		}
		
		public void Scratch()
		{
			for (var i = 0; i < isScratching.Length; i++)
			{
				var scratching = isScratching[i];
				if (scratching)
				{
					if (OnScratchStart != null)
					{
						OnScratchStart();
					}

					if (eraseStartPositions[i] == eraseEndPositions[i])
					{
						if (OnScratchHole != null)
						{
							OnScratchHole(erasePosition);
						}
					}
					else
					{
						if (OnScratchLine != null)
						{
							OnScratchLine(eraseStartPositions[i], eraseEndPositions[i]);
						}
					}
				}
			}
		}
	}
}