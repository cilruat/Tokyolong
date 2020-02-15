using UnityEngine;
using System.Collections;

namespace I2.MiniGames
{
	[AddComponentMenu("I2/MiniGames/Wheel/Reward")]
	public class PrizeWheel_Reward : MiniGame_Reward 
	{
		public RectTransform _Content;
		public bool _RotateContent = true;

		public RectTransform _Separator;

		public UnityEventFloat _OnBackgroundSetFillAmount = new UnityEventFloat();


		public virtual void ApplyLayout( float Angle, float SliceSize, PrizeWheel wheel )
		{
			float offsetAngle = 90 * (int)wheel._SelectorDirection;

			float rotAngle = -Angle - offsetAngle;
			var rot = Quaternion.Euler (0, 0, rotAngle);
			var half = Quaternion.Euler (0, 0, -SliceSize / 2);
			var contentRot = Quaternion.Euler (0, 0, _RotateContent ? offsetAngle-SliceSize / 2.0f : -rotAngle);

			var vOffset = rot * half * Vector3.up * wheel._Elements_Offset;

			transform.localRotation = rot;
			transform.localPosition = vOffset;

			/*if (_Background)
			{
				_Background.fillAmount = SliceSize/360f;
				_Background.fillMethod = Image.FillMethod.Radial360;
				_Background.fillOrigin = (int)Image.Origin360.Top;
			}*/
			_OnBackgroundSetFillAmount.Invoke (SliceSize / 360f);

			if (_Separator)
			{
				_Separator.localRotation = rot;
				_Separator.localPosition = vOffset;
			}

			if (_Content)
			{
				//CollapseTransform(_Content);

				float dist = _Content.localPosition.magnitude;
				_Content.localPosition = half * (Vector3.up * dist);
				_Content.localRotation = contentRot;
			}
		}

		public void CollapseTransform( RectTransform tr )
		{
			Rect r = tr.rect;
			Vector3 p = tr.localPosition;
			tr.offsetMin = tr.anchorMin;
			tr.offsetMax = tr.anchorMax;
			tr.anchorMin = new Vector2(0.5f,0.5f);
			tr.anchorMax = tr.anchorMin;
			tr.offsetMin = r.min+(Vector2)p;
			tr.offsetMax = r.max+(Vector2)p;
		}

		public virtual void SpinningUpdate()
		{
			if (!_RotateContent && _Content)
				_Content.rotation = Quaternion.identity;
		}

		public override void Hide (){}		
		public override void Show ( Transform parent ) {}
	}
}