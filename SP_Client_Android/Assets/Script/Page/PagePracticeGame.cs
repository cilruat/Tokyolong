using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PagePracticeGame : PageBase {

	public CanvasGroup[] cgBoard;

	protected override void Awake ()
	{
		base.boards = cgBoard;
		base.Awake ();

		Info.practiceGame = false;
	}

	public void OnGoPractice(int idx)
	{
		Info.practiceGame = true;
		Info.PlayGame (idx, curBoardObj ());
	}
}
