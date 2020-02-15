using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using I2;

namespace I2.MiniGames
{
	[AddComponentMenu("I2/MiniGames/Wheel/Game")]
	public class PrizeWheel : MiniGame 
	{
		#region Variables: Selector

		[Header("Selector")]
		public RectTransform _Selector;

		public enum eSelectorDirection { top, right, bottom, left };
		[Rename("Direction")]		public eSelectorDirection _SelectorDirection;

		[Rename("Anchor To Center")]public bool _AnchorSelectorToCenter = false;

		[Rename("Elastic Duration")]public float _SelectorElasticDuration=1;
		[Rename("Elastic Amplitud")]public float _SelectorElasticAmplitud;
		[Rename("Elastic Period")]	public float _SelectorElasticPeriod;

		#endregion

		#region Variables: Wheel

		[Header("Wheel")]
		public Transform _Wheel;
		public float _Elements_Spread; 	// Empty space between elements
		public float _Elements_Offset; 	// space between element and center
		[Rename("Equal Distribution")]
		public bool  _Elements_EqualDistribution; // equal or using probability

		[Header("Knots")]
		public Transform _Knots;
		public float _KnotsOffset;

		[Header("Knots Sounds")]
		public AudioClip[] _Sounds_KnotCollision;
		[Rename("Min Volume")]public float _Sound_KnotCollision_MinVolume=0.2f;
		[Rename("Max Volume")]public float _Sound_KnotCollision_MaxVolume=1;
		public UnityEventAudioClip _PlayKnotSound = new UnityEventAudioClip();


		#endregion
				
		#region Variables: Play	

		[Header("Simulation")]
		public bool _CenterOnElement = false;

		public bool _RotateSelector; 	// true = rotate selector, false = rotate wheel
		
		public float _Rotation_MinCycles = 2;  // how many full laps it does every time it plays
		public float _Rotation_MaxCycles = 4;
		
		public float _Rotation_MinTime = 3;  // how long it stays rotating
		public float _Rotation_MaxTime = 4;
		
		public float _SpeedUpTime = 0.2f;
		public float _Friction = 5;

		public UnityEvent _OnStartSpinning = new UnityEvent();		
		public UnityEventInt _OnFinishSpinning = new UnityEventInt();


		[Header("Timing")]
		public float _TimeCollectReward = 0;
		public float _TimeNewRound = 0;

		#endregion

		#region Variables: Internal

		protected List<RectTransform> mKnots = new List<RectTransform> ();

		private bool mIsPlaying = false;
		private float mSelectorMaxAngle=0;
		private float mWheelMaxAngleForKnot=0;

		private float mSelectorElasticStartTime;
		private float mSelectorElasticStartAngle;
		private int mForceReward = -1;

		#endregion

		#region Layout

		public override void ApplyLayout()
		{
			mForceReward = -1;
			base.ApplyLayout ();

			//--[ Find Distribution Length ]--------------------

			int NumElements = mRewards.Count;
			float TotalPriority = _Elements_EqualDistribution ? NumElements : mRewards.Sum(r=>r.Probability);

			//--[ Distribute ]----------------

			float Angle=0;
			bool first = true;
			float WheelStartAngle = 0;
			_Wheel.localRotation = Quaternion.identity;

			foreach (PrizeWheel_Reward reward in mRewards)
			{
				if (reward._Separator)
					reward._Separator.gameObject.SetActive(true);

				float priority = _Elements_EqualDistribution ? 1 : reward.Probability;
				float SliceSize = 360 * priority / TotalPriority;

				//--[ Adjust first Element so that Angle=0 becomes the center of the slice ]--------
				if (first) 
				{ 
					Angle -= SliceSize/2; 
					WheelStartAngle = Angle; 
					first = false; 
				}

				reward.ApplyLayout( Angle, SliceSize-_Elements_Spread, this );

				Angle += SliceSize;
			}

			if (_Selector) 
			{
				float offsetAngle = 90 * (int)_SelectorDirection; 

				if (_AnchorSelectorToCenter)
				{
					_Selector.localRotation = Quaternion.Euler(0,0, 180-offsetAngle);
					_Selector.localPosition = Vector3.zero;
				}
				else
				{
					_Selector.localRotation = Quaternion.Euler(0,0, -offsetAngle);
					_Selector.localPosition = Quaternion.Euler(0,0, -offsetAngle) * (Vector3.up * _Selector.localPosition.magnitude);
				}

				mSelectorElasticStartTime = -1000;
				mSelectorElasticStartAngle = _Selector.rotation.eulerAngles.z;
			}

			ApplyLayout_Knots (WheelStartAngle);
			GetSelectorAngles ();
		}

		public override void FilterRewards()
		{
			base.FilterRewards ();
		
			mRewards.RemoveAll (r => !r.gameObject.activeInHierarchy);
		}

		public virtual void ApplyLayout_Knots( float StartAngle )
		{
			mKnots.Clear ();
			if (!_Knots)
				return;
			foreach (RectTransform knot in _Knots)
				if (knot.gameObject.activeInHierarchy)
					mKnots.Add (knot);
			//mKnots.AddRange ((IEnumerator<RectTransform>)_Knots);
			//mKnots.RemoveAll (k => !k.gameObject.activeInHierarchy);

			for (int i=0; i<mKnots.Count; ++i)
			{
				var knot = mKnots[i];
				float angle = StartAngle + _KnotsOffset - i*360/(float)mKnots.Count;

				knot.localRotation = Quaternion.Euler(0,0, angle);
				knot.localPosition = Quaternion.Euler(0,0, angle) * (Vector3.up * knot.localPosition.magnitude);
			}
		}

		#endregion

		#region Play

		public void StartSpinning() 
		{
			StartSpinning (-1);
		}

		public void StartSpinning( int forceReward )
		{
			mForceReward = forceReward;
			TryPlayingRound ();
		}

		public override void StartRound()
		{
			_OnStartSpinning.Invoke();

			float TotalAngle;
			GetRandomElement (ref mForceReward, out TotalAngle );

			StartCoroutine (DoPlay (mForceReward, TotalAngle));
			mForceReward = -1;
		}

		public void StopPlay()
		{
			mIsPlaying = false;
		}

		/*void Update()
		{
			int d = -1;
			UpdateSelector (ref d, false);
		}*/

		private void GetRandomElement( ref int ElementIdx, out float Angle )
		{
			int forceElement = ElementIdx;
			ElementIdx = 0;
			Angle = 0;
			
			//--[ Find Distribution Length ]--------------------
			
			float TotalPriority = mRewards.Sum(r=>r.Probability);

			//--[ Find random ]----------------
			
			float RndPos = forceElement>=0 ? TotalPriority+1 : Random.value * TotalPriority;
			
			float offsetAngle = 90 * (int)_SelectorDirection;

			if (_RotateSelector) 
				Angle = offsetAngle + (_AnchorSelectorToCenter?180:0);
			else 
				Angle = 0;//-offsetAngle;
			
			bool first = true;
			float TotalVisualPriority = _Elements_EqualDistribution ? mRewards.Count : TotalPriority;

			for (int i=0, imax=mRewards.Count; i<imax; ++i)
				{
					var element = mRewards[i];
					
					float priority = _Elements_EqualDistribution ? 1 : element.Probability;
					float SliceSize = 360 * priority / TotalVisualPriority;
					if (first) { Angle -= SliceSize/2; first = false; }
					
					RndPos -= element.Probability;
					
					if (RndPos<0 || (forceElement>=0 && i==forceElement))
					{
						Angle += mWheelMaxAngleForKnot;  // Don't allow too close to the begining in case there is a knot there
						if (_CenterOnElement)
							Angle += 0.5f * SliceSize;
						else
							Angle += Mathf.Lerp(0.2f, 0.8f, Random.value) * (SliceSize - _Elements_Spread-mWheelMaxAngleForKnot);
						ElementIdx = i;
						
						return;
					}
					else
						Angle += SliceSize;
				}

		}

		#endregion

		#region Simulation

		private IEnumerator DoPlay( int TargetElementIdx, float ElementAngle )
		{
			if (mIsPlaying)
				yield break;
			mIsPlaying = true;
			
			float InitialAngle = 0;
			if (_RotateSelector && _Selector) 
			{
				InitialAngle = -_Selector.rotation.eulerAngles.z;
			}
			else
			{
				InitialAngle = _Wheel.rotation.eulerAngles.z;
			}
			
			float TotalRotation = Mathf.DeltaAngle (InitialAngle, ElementAngle);
			if (TotalRotation < 0)
				TotalRotation += 360;
			
			float NumCycles = Random.Range (_Rotation_MinCycles, _Rotation_MaxCycles);
			TotalRotation += Mathf.CeilToInt (NumCycles) * 360;
			
			float TotalTime = Random.Range (_Rotation_MinTime, _Rotation_MaxTime);
			float InitialTime = Time.time;
		

			bool rotateback = false;
			float centerTime = _SpeedUpTime;
			int currentKnot = -1;
			while(true)
			{
				bool finished = false;

				finished = UpdateRotation( InitialAngle, TotalRotation, InitialTime, TotalTime, centerTime );

				if (finished && !rotateback && _Selector && mKnots.Count>0) 
				{
					rotateback = true;

					float selectorDAngle = Vector3.Angle(_Selector.localPosition, _Selector.rotation*Vector3.down);
					//float selectorDAngle = Vector3.Angle(_Wheel.position-_Selector.position, _Selector.rotation*Vector3.down);
					float dangle = Mathf.Clamp01(selectorDAngle/mSelectorMaxAngle);

					TotalTime = 2;
					InitialAngle = ElementAngle;
					TotalRotation = -mWheelMaxAngleForKnot*dangle;

					InitialTime = Time.time;
					centerTime = TotalTime/2.0f;
					finished = false;
				}

				bool fin = UpdateSelector( ref currentKnot, rotateback );
				if (rotateback && fin)
					finished = true;

				foreach( var element in mRewards)
					((PrizeWheel_Reward)element).SpinningUpdate();

				if (finished)
					break;
				else
					yield return null;
				
				// The coroutine was asked to be stopped
				if (!mIsPlaying)
					yield break;
			}

			mIsPlaying = false;
			_OnFinishSpinning.Invoke(TargetElementIdx);

			// If should automatically open reward  (-1 disables this)
			if(_TimeCollectReward>=0)
			{
				if (_TimeCollectReward>0) 
					yield return new WaitForSeconds(_TimeCollectReward);

				var reward = mRewards[TargetElementIdx];
				reward.Execute(this, null);
			}

			// If should get ready for next round (-1 disables this)
			if (_TimeNewRound>=0 && _Controller) 
			{
				if (_TimeNewRound>0) 
					yield return new WaitForSeconds(_TimeNewRound);

				_Controller.OnReadyForNextRound ();
			}
		}



		private bool UpdateRotation( float initialAngle, float totalRotation, float initialTime, float totalTime, float speedUpTime )
		{
			float elapsedTime = Time.time - initialTime;
			float dt = Mathf.Clamp01(elapsedTime / totalTime);
				
			float centerT = speedUpTime / totalTime; // Point at which it changes from EasyIn to EasyOut

			if (dt<centerT)	// Start slowly and speedup
			{
				float t = (dt/centerT);
				dt = Mathf.Pow(t, _Friction)*centerT;  // Easy Out
			}
			else
			{
				// Ends slowly
				float d = (1-centerT);
				float t = (dt-centerT)/d;
				dt = 1-Mathf.Pow(1-t,_Friction); // Easy In
				dt = dt*d+centerT;
			}
				
			float angle = initialAngle + dt*totalRotation;

			// Almost there, skip the rest (the slow down at the end makes the final part too slow)
			if (Mathf.Abs (angle - (initialAngle + totalRotation)) < 0.5f)
			{
				angle = initialAngle + totalRotation;
				dt = 1;
			}

			if (_RotateSelector && _Selector) 
			{
				float dist = _Selector.localPosition.magnitude;
				//float dist = Vector3.Distance(_Wheel.position, _Selector.position);

				var rot = Quaternion.Euler (0, 0, -angle);
				if (_AnchorSelectorToCenter)
					_Selector.rotation = rot;
				else
					_Selector.localPosition = rot * (Vector3.up * dist);
			} 
			else 
			{
				_Wheel.rotation = Quaternion.Euler (0, 0, angle);
			}

			return dt >= 1;
		}

		private bool UpdateSelector( ref int currentKnot, bool rotateback )
		{
			if (!_Selector || mKnots.Count<=0)
				return true;

			if (_AnchorSelectorToCenter)
				return true;

			Vector3 vSelectorPos = _Selector.localPosition;
			Vector3 vWheel2Sel = vSelectorPos;
			Vector3 vKnotPos = Vector3.zero;
			float KnotRadius = 0;

			float SelectorLength = _Selector.rect.height * _Selector.pivot.y;
			float SelectorWidth  = _Selector.rect.width * (1 - _Selector.pivot.x);

			int ClosestKnot = -1;

			for (int i=0; i<mKnots.Count; ++i)
			{
				var knot = mKnots[i];
				vKnotPos = _Wheel.rotation * knot.localPosition;
				float KnotW = knot.rect.width / 2f;
				float KnotH = knot.rect.height / 2f;
				KnotRadius = Mathf.Sqrt (KnotW * KnotW + KnotH * KnotH);

				Vector3 vWheel2Knot = vKnotPos;

				Vector3 side = Vector3.Cross(vWheel2Knot, Vector3.forward).normalized;
				float dir = Vector3.Dot ( side, vWheel2Sel );

				if (dir>(-SelectorWidth-KnotRadius) && 
			        Vector3.Distance(vSelectorPos, vKnotPos)<=(SelectorLength+KnotRadius) )
				{
					ClosestKnot = i;
					break;
				}
			}	

			if (ClosestKnot >= 0 && ClosestKnot != currentKnot && !rotateback) 
			{
				AudioClip clip = null;
				if (_Sounds_KnotCollision.Length>0)
					clip = _Sounds_KnotCollision[ Random.Range(0, _Sounds_KnotCollision.Length) ];
				float volumeScale = Random.Range(_Sound_KnotCollision_MinVolume, _Sound_KnotCollision_MaxVolume);
				_PlayKnotSound.Invoke (clip, volumeScale);
			}
				

			if (!rotateback)
				currentKnot = ClosestKnot;
			else
			if (ClosestKnot != currentKnot)
				ClosestKnot = -1;

			float currentSelectorAngle = _Selector.rotation.eulerAngles.z;
			bool finished = false;

			if (ClosestKnot>=0)
			{
				Vector3 side = Vector3.Cross(vKnotPos - vSelectorPos, Vector3.forward).normalized;
				vKnotPos += side * (KnotRadius+SelectorWidth);

				float selectorAngle = Vector3.Angle (vKnotPos - vSelectorPos, Vector3.down);
				float sign = Mathf.Sign( Vector3.Dot(vKnotPos - vSelectorPos, Vector3.right) );

				currentSelectorAngle = sign*selectorAngle;

				mSelectorElasticStartTime = Time.time;
				mSelectorElasticStartAngle = currentSelectorAngle;
			}
			else
			{
				float baseAngle = Vector3.Angle(-vWheel2Sel, Vector3.down);
				baseAngle *= Mathf.Sign( Vector3.Dot(-vWheel2Sel, Vector3.right ) );
				
				if (_SelectorElasticDuration<=0) _SelectorElasticDuration = 0.01f;
				float dt = Mathf.Clamp01((Time.time-mSelectorElasticStartTime)/_SelectorElasticDuration);
				currentSelectorAngle = GetElasticOut( dt, mSelectorElasticStartAngle, baseAngle, _SelectorElasticAmplitud, _SelectorElasticPeriod );
				finished = dt>=1;
			}

			_Selector.rotation = Quaternion.Euler(0,0, currentSelectorAngle);
			return finished;
		}

		private float GetElasticOut( float t, float a, float b, float amplitude, float period )
		{
			float s;
			if (t <= 0)
				return a;

			if (t >= 1)
				return b;

			if (period == 0)
				period = 0.3f;

			float dvalue = Mathf.DeltaAngle(a, b);
			if (amplitude == 0 || (dvalue > 0 && amplitude < dvalue) || (dvalue < 0 && amplitude < -dvalue))
			{
				amplitude = dvalue;
				s = period/4f;
			}
			else
			{
				s = period/(2*Mathf.PI)*(float)Mathf.Asin(dvalue/amplitude);
			}
			return (amplitude*(float)Mathf.Pow(2, -10*t)*(float)Mathf.Sin((t - s)*(2*Mathf.PI)/period) + dvalue + a);
		}


		// Find selectorAngle: highest angle the selector reaches because of the knots
		void GetSelectorAngles ()
		{
			//--[ Find wheelAngle and selectorAngle if knot its a point and selector its a line

			//float Wheel2Selector = _Selector ? (_Wheel.position - _Selector.position).magnitude : 0;
			float Wheel2Selector = _Selector ? (_Selector.localPosition).magnitude : 0;

			if (mKnots.Count <= 0 || _AnchorSelectorToCenter || Wheel2Selector<=0)
			{
				mSelectorMaxAngle = 0;
				mWheelMaxAngleForKnot = 0;
				return;
			}

			Vector3 vSelectorPivot = Vector3.up * Wheel2Selector;

			var knot = mKnots [0];
			Vector3 vKnot = knot.localPosition;
			float Wheel2KnotSq = vKnot.sqrMagnitude;
			float Wheel2SelectorSq = Wheel2Selector * Wheel2Selector;

			float mSelectorLength = _Selector.rect.height * _Selector.pivot.y;
			float SelectorLengthSq = mSelectorLength * mSelectorLength;

			// equation from combining:
			//  dist([0, Wheel2Selector], (x,y)) = SelectorLength
			//  x*x + y*y = Wheel2Knot*Wheel2Knot

			float y = (Wheel2KnotSq + Wheel2SelectorSq - SelectorLengthSq) / (2 * vSelectorPivot.y);
			float x = Mathf.Sqrt (Mathf.Abs(Wheel2KnotSq - y * y));

			Vector3 vFarthestPoint = new Vector3 (x, y, vSelectorPivot.z);

			mSelectorMaxAngle = Vector3.Angle (-vSelectorPivot, vFarthestPoint - vSelectorPivot);

			//--[ Take into account the Selector Width and Knot Radius ]--------------

			float KnotW = knot.rect.width / 2f;
			float KnotH = knot.rect.height / 2f;
			float KnotRadius = Mathf.Sqrt (KnotW * KnotW + KnotH * KnotH);

			float SelectorW = _Selector.rect.width * (1 - _Selector.pivot.x);

			mSelectorMaxAngle += Mathf.Atan ((SelectorW + KnotRadius) / mSelectorLength) * Mathf.Rad2Deg;

			mWheelMaxAngleForKnot = Mathf.Atan ((vFarthestPoint.x + SelectorW+KnotRadius) / vKnot.magnitude) * Mathf.Rad2Deg;
			//Debug.Log (mWheelMaxAngleForKnot + " " + mSelectorMaxAngle);
		}

		#endregion


	}
}