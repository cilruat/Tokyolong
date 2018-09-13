using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PagePracticeGame : PageBase {

	public CanvasGroup[] cgBoard;

	protected override void Awake ()
	{
		base.boards = cgBoard;
		base.Awake ();
	}
}
