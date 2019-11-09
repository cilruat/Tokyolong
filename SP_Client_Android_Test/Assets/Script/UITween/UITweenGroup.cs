using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UITweenGroup : UITween
{
	public List<UITween> tweens;
	public bool activeOnlyEnabled = false;

	protected override void SetVal (float ratio) {}

#if UNITY_EDITOR
	protected override void Awake ()
	{
		base.Awake();

		if (tweens != null)
		{
			for (int i = 0; i < tweens.Count; i++)
				tweens[i].upper = this;
		}
	}
#endif

	protected override void Tween ()
	{
		base.Tween();
		AddCallback(StartAllTween);
	}

	public override bool IsTweening ()
	{
		for (int i = 0; i < tweens.Count; i++)
		{
			if (tweens[i].IsTweening())
				return true;
		}

		return base.IsTweening();
	}

	void StartAllTween ()
	{
		for (int i = 0; i < tweens.Count; i++)
			if(tweens[i].gameObject.activeSelf)
				tweens[i].StartTween(this.startTag);
	}

	public void SetAllTime (float time)
	{
		for (int i = 0; i < tweens.Count; i++)
			tweens[i].SetTime(time);
	}

	public override void SetReverse (bool reverse)
	{
		for (int i = 0; i < tweens.Count; i++)
			tweens[i].SetReverse(reverse);
	}

#if UNITY_EDITOR
	float tempDelay;
	UITweenGroup tempParentGroup;
	public override bool AnimateEditor (float elapsedTo, bool forceUpdate, TWTag startTag)
	{
		if (elapsed == elapsedTo && forceUpdate == false)
			return false;

		elapsed = elapsedTo;

		gameObject.SetActive(true);

		tempDelay = 0f;
		foreach (UITween tween in tweens)
		{
			tempDelay = tween.param.delay;

			if (tween.prev != null)
			{
				tempDelay += tween.prev.TotalLength;

				tempParentGroup = tween.prev.upper;
				while (tempParentGroup != null)
				{
					tempDelay += tempParentGroup.Delay;
					tempParentGroup = tempParentGroup.upper;
				}
			}

			if (elapsed < tempDelay || (tween.isTweenGroup == false && tween.startTag != startTag))
			{
				tween.AnimateEditor(0f, false, startTag);
				continue;
			}

			tween.AnimateEditor(elapsed - tempDelay, true, startTag);
		}

		return true;
	}

	public void RegistTweens ()
	{
		if (gameObject.activeInHierarchy == false)
			return;
		
		tweens.Clear();

		UITween[] temp = GetComponentsInChildren<UITween>();
		foreach (UITween tween in temp)
		{
			if (tween == this)
				continue;

			if (tween.GetType() == typeof(UITweenGroup))
			{
				if (tween.transform.parent != null && tween.transform.parent.GetComponentInParent<UITweenGroup>() != this)
					continue;
				
				((UITweenGroup)tween).RegistTweens();
			}
			else if (tween.GetComponent<UITweenGroup>() != this && tween.GetComponentInParent<UITweenGroup>() != this)
			{
				continue;
			}

			tween.InitForInspector();
			tweens.Add(tween);
		}
	}

	float tempTotalLength;
	float tempLength;
	public float GetTotalLength ()
	{
		tempTotalLength = 0f;
		foreach (UITween tween in tweens)
		{
			if (tween == null)
				continue;

			if (tween.startTag != startTag)
				continue;

			tempLength = 0f;
			if (tween.GetType() == typeof(UITweenGroup))
				tempLength = ((UITweenGroup)tween).GetTotalLength() + tween.Delay;
			else
				tempLength = tween.TotalLength;

			if (tween.prev != null)
			{
				tempParentGroup = tween.prev.upper;
				while (tempParentGroup != null)
				{
					tempLength += tempParentGroup.Delay;
					tempParentGroup = tempParentGroup.upper;
				}
			}

			if (tempTotalLength < tempLength)
				tempTotalLength = tempLength;
		}

		return tempTotalLength;
	}

	public void FoldOutMe (UITween me)
	{
		gameObject.SetActive(true);
		RegistTweens();
		
		foreach (UITween tween in tweens)
			tween.foldout = tween == me;
	}
#endif
}
