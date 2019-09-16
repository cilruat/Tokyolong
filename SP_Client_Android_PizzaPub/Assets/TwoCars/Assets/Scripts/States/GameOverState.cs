using UnityEngine;
using System.Collections;

namespace TwoCars
{
	public class GameOverState : _StatesBase {

		#region implemented abstract members of _StatesBase
		public override void OnActivate ()
		{
	        Managers.Game.isGameActive = false;
	        Managers.Score.CheckHighScore();
	        Managers.Game.stats.numberOfGames++;
	        //Managers.UI.popUps.ActivateGameOverPopUp();
			Managers.UI.GameOver();
	        Managers.Audio.PlayLoseSound();
	        Managers.Spawner.StopSpawning();
	        Managers.Anal.SendScoreAnalytic();
	        Managers.Spawner.ClearObstacles();

	        Debug.Log ("<color=green>Game Over State</color> OnActive");	
		}

		public override void OnDeactivate ()
		{
	        Debug.Log ("<color=red>Game Over State</color> OnDeactivate");
		}

		public override void OnUpdate ()
		{
			Debug.Log ("<color=yellow>Game Over State</color> OnUpdate");
		}
		#endregion

	}
}