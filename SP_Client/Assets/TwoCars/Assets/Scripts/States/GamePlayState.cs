using UnityEngine;
using System.Collections;

namespace TwoCars
{
	public class GamePlayState : _StatesBase {

	    private float gamePlayDuration;

		#region implemented abstract members of _StatesBase
		public override void OnActivate ()
		{
	        Managers.UI.panel.SetActive(false);
	        Managers.UI.ActivateUI(Menus.INGAME);
	        Managers.Difficulty.ResetDifficulty();
	        Managers.Game.isGameActive = true;
	        Managers.Input.isActive= true;
	        Managers.Score.ResetScore();
	        Managers.Spawner.StartSpawning();

	        gamePlayDuration = Time.time;

	        Debug.Log ("<color=green>Gameplay State</color> OnActive");	
		}
		public override void OnDeactivate ()
		{
	        Managers.Game.stats.timeSpent += Time.time - gamePlayDuration;
			Debug.Log ("<color=red>Gameplay State</color> OnDeactivate");
		}

		public override void OnUpdate ()
		{
			Debug.Log ("<color=yellow>Gameplay State</color> OnUpdate");
		}
		#endregion

	}
}