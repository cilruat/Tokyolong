using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotMachineElt : MonoBehaviour {

	const float MOVE_SPEED = 800f;

	public RectTransform[] rtElts;

	bool startRoll = false;
	int stopIdx = -1;
	float StartPosY = 0f;
	float CutlinePosY = 0f;

	void Start()
	{
		if (rtElts.Length == 0)
			return;
		
		_SettingPos ();
	}

	void Update () {

		if (startRoll == false)
			return;

		if (rtElts == null || rtElts.Length == 0)
			return;

		for (int i = 0; i < rtElts.Length; i++) {
			float y = rtElts [i].anchoredPosition.y;
			float move = MOVE_SPEED * Time.fixedDeltaTime;
			float down = y - move;
			if (down <= CutlinePosY)
				down = StartPosY;

			rtElts [i].anchoredPosition = new Vector2 (0f, down);
		}	
	}

	void _SettingPos()
	{
		if (rtElts == null || rtElts.Length == 0)
			return;

		StartPosY = rtElts [rtElts.Length - 1].anchoredPosition.y;
		CutlinePosY = rtElts [1].anchoredPosition.y * -1;
	}

	public void SetElts(RectTransform[] rtElts)
	{
		this.rtElts = rtElts;
		_SettingPos ();
	}

	public void StartSlot(int stopIdx)
	{
		startRoll = true;
		this.stopIdx = stopIdx;

		if (rtElts == null || rtElts.Length == 0)
			return;

		for (int i = 0; i < rtElts.Length; i++)
			rtElts [i].gameObject.SetActive (true);
	}

	public void StopSlot()
	{
		
	}
}