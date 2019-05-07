﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotMachineElt : MonoBehaviour {

	const float MOVE_SPEED = 800f;

	public PageGame page;
	public ESlotType eType;
	public RawImage imgFrame;
	public RectTransform[] rtElts;
	public CanvasGroup cgJackpot;

	bool isAllAnimating = false;
	bool isStopAnimating = false;
	int stopIdx = 0;
	float StartPosY = 0f;
	float CutlinePosY = 0f;

	void Start()
	{
		if (rtElts.Length == 0)
			return;
		
		_SettingPos (false);
	}

	IEnumerator _StartSlotAni () {

		if (rtElts == null || rtElts.Length == 0)
			yield break;

		while (true) {
			if (isStopAnimating)
				break;
			
			for (int i = 0; i < rtElts.Length; i++) {
				float y = rtElts [i].anchoredPosition.y;
				float move = MOVE_SPEED * Time.deltaTime;
				float down = y - move;
				if (down <= CutlinePosY)
					down = StartPosY;

				rtElts [i].anchoredPosition = new Vector2 (0f, down);
			}
			yield return null;
		}			
	}

	void _SettingPos(bool firstSet)
	{
		if (rtElts == null || rtElts.Length == 0)
			return;

		if (firstSet)
			_resetPos ();

		StartPosY = rtElts [rtElts.Length - 1].anchoredPosition.y;
		CutlinePosY = rtElts [1].anchoredPosition.y * -1;
	}

	void _resetPos()
	{
		for (int i = 0; i < rtElts.Length; i++) {				
			float y = (i * 60f);
			rtElts [i].anchoredPosition = new Vector2 (0f, y);
		}
	}

	public void SetElts(RectTransform[] rtElts)
	{
		if (rtElts == null)
			isAllAnimating = false;

		if (this.rtElts != null && this.rtElts.Length > 0) {
			for (int i = 0; i < this.rtElts.Length; i++)
				this.rtElts [i].gameObject.SetActive (false);
		}

		this.rtElts = rtElts;
		_SettingPos (true);
	}

	public void StartSlot(int stopIdx)
	{
		if (rtElts == null || rtElts.Length == 0)
			return;

		isAllAnimating = true;
		this.stopIdx = stopIdx;

		_resetPos ();
		for (int i = 0; i < rtElts.Length; i++)
			rtElts [i].gameObject.SetActive (true);

		StartCoroutine (_StartSlotAni ());
	}

	float _DynamicTime(float time, float fullTime)
	{
		return Mathf.SmoothStep (-1, 1, .5f + (time * .5f / fullTime));
	}

	IEnumerator _StopAni(bool isJackpot)
	{
		if (rtElts.Length > 0 && stopIdx >= rtElts.Length)
			stopIdx = rtElts.Length - 1;

		isStopAnimating = true;

		if (rtElts != null) {
			for (int i = 0; i < rtElts.Length; i++)
				rtElts [i].gameObject.SetActive (i == stopIdx && isJackpot == false);
		}

		cgJackpot.gameObject.SetActive (isJackpot);

		float during = .5f;
		for (float t = 0; t < during; t += Time.fixedDeltaTime) 
        {
			if (isJackpot) 
            {
				float a = Mathf.Lerp (0f, 1f, _DynamicTime (t, during));
				cgJackpot.alpha = a;
			} else 
            {
				Vector2 pos = Vector2.Lerp (
					             new Vector2 (0f, 100f),
					             Vector2.zero, 
					             _DynamicTime (t, during));

				rtElts [stopIdx].anchoredPosition = pos;

				if (pos.y <= 0f)
					break;
			}				

			yield return null;
		}			

		isStopAnimating = false;
		isAllAnimating = false;

		yield return new WaitForSeconds (.1f);
		ShiningGraphic.Start (imgFrame);

		yield return new WaitForSeconds (1f);

        if (eType == ESlotType.eGame)
            page.ShowPopup();
	}

	public void StopSlot(bool isJackpot) 
	{
		if (isJackpot == false && (rtElts == null || rtElts.Length == 0))
			return;
		
		StartCoroutine (_StopAni (isJackpot));
	}

	public void SetStopIdx(int stopIdx) { this.stopIdx = stopIdx; }
	public bool Animating() { return isAllAnimating; }
	public bool StopAnimating() { return isStopAnimating; }
}