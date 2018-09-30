using UnityEngine;
using System.Collections;

namespace TwoCars
{
	public class MenuState : _StatesBase {
		
		#region implemented abstract members of GameState

		public override void OnActivate ()
		{		
			Debug.Log ("<color=green>Menu State</color> OnActive");
	        Managers.UI.panel.SetActive(false);
	        Managers.UI.ActivateUI (Menus.MAIN);
	        Managers.Game.isGameActive = false;
	        Managers.Input.isActive = false;
	        Managers.Score.ResetScore();
	    }

		public override void OnDeactivate ()
		{
			Debug.Log ("<color=red>Menu State</color> OnDeactivate");
		}

		public override void OnUpdate ()
		{
			Debug.Log ("<color=yellow>Menu State</color> OnUpdate");
		}

		#endregion
	}
}