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
		string sceneName = "";
		switch (idx) {
		case 0:		sceneName = "PicturePuzzle";		break;
		case 1:		sceneName = "PairCards";			break;
		case 2:		sceneName = "EmojiMain";			break;
		case 3:		sceneName = "Emoji2Main";			break;
		case 4:		sceneName = "FlappyBirdMasterMain";	break;
		case 5:		sceneName = "CrashCatStart";		break;
		case 6:		sceneName = "BallDuetMain";			break;
		case 7:		sceneName = "JumperStepUpMain";			break;			
		}

		Info.practiceGame = true;
		SceneChanger.LoadScene (sceneName, curBoardObj ());
	}
}
