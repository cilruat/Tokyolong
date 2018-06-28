using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageOrder : PageBase {

	public CanvasGroup[] cgBoards;

	protected override void Awake ()
	{
		base.boards = this.cgBoards;
		base.Awake ();
	}
}