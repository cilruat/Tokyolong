namespace GameBench
{
	using System.Collections;
	using UnityEngine;

	public class EndRewardObject : MonoBehaviour
	{

		public Animator _anim;
		//		public Transform actualParent;
		public SpriteRenderer bgSpriteRend, iconSpriteRend;
		public TextMesh quantText, quantText1;
		public SpriteRenderer bgSpriteRend1, iconSpriteRend1;

		public void AssignValues (Color clr, Sprite sp, float val)
		{
			bgSpriteRend.color = clr;
			iconSpriteRend.sprite = sp;
			quantText.text = val.ToString ();

//			bgSpriteRend1.color = clr;
			iconSpriteRend1.sprite = sp;
			quantText1.text = val.ToString ();
		}

		public void PlayAnim (bool coin)
		{
			SetActive (true);
			_anim.Play (coin ? "coinReward" : "prizeReward");

		}

		public void AnimEndCoin ()
		{
			UIManager.Instance.SpinCompleted ();
			gameObject.SetActive (false);
		}
		//
		//
		//		public Vector3 unitScaleVect { get { return Vector3.one; } }
		//
		//		public Vector3 zoomedScaleVect { get { return Vector3.one * 2; } }
		//
		//		float elapsedTime = 0.0f, nowTime = 0.0f;
		//
		//		public IEnumerator MoveToPosition (Transform obj, Vector3 startPos, Vector3 targetPos, float delay = 0.5f)
		//		{
		//			elapsedTime = 0.0f;
		//			while (elapsedTime < delay) {
		//				nowTime = elapsedTime / delay;
		//				obj.position = Vector3.Lerp (obj.position, targetPos, nowTime);
		////				obj.localScale = Vector3.Lerp (obj.localScale, zoomedScaleVect, nowTime);
		//				elapsedTime += Time.deltaTime;
		//				yield return null;
		//			}
		//			elapsedTime = 0.0f;
		//			obj.position = targetPos;
		////			obj.localScale = zoomedScaleVect;
		//			while (elapsedTime < delay) {
		//				nowTime = elapsedTime / delay;
		//				obj.position = Vector3.Lerp (obj.position, startPos, nowTime);
		////				obj.localScale = Vector3.Lerp (obj.localScale, unitScaleVect, nowTime);
		//				elapsedTime += Time.deltaTime;
		//				yield return null;
		//			}
		//			obj.localPosition = startPos;
		////			obj.localScale = unitScaleVect;
		////			transform.SetParent (actualParent);
		//			UIManager.Instance.SpinCompleted ();
		//			SetActive (false);
		//
		//		}

		public void SetActive (bool state)
		{
			gameObject.SetActive (state);
		}
	}
}