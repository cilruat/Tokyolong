using UnityEngine.Events;
using UnityEngine;
using System.Collections;

namespace I2.MiniGames
{
	[AddComponentMenu("I2/MiniGames/TreasureHunt/Slot")]
	public class TreasureHunt_Slot : MonoBehaviour 
	{
		[Header("States")]
		public GameObject _ClosedState;
		[Rename("Open Empty")]public GameObject _OpenState_Empty;
		[Rename("Open Full")]public GameObject _OpenState_Full;

		[Space(10)]
		public UnityEvent _OnReset = new UnityEvent();
		public UnityEvent _OnOpenEmpty = new UnityEvent();
		public UnityEvent _OnOpenFull = new UnityEvent();

		[Header("Timing")]
		public float _TimeCollectReward = 0;
		public float _TimeNewRound = 0;

		private MiniGame_Reward mReward;

		public virtual void Reset ()
		{
			mReward = null;
			if (_ClosedState) 	  _ClosedState.SetActive (true);
			if (_OpenState_Empty) _OpenState_Empty.SetActive (false);
			if (_OpenState_Full)  _OpenState_Full.SetActive (false);

			_OnReset.Invoke ();
		}

		public virtual void Open ( MiniGame_Controller controller, MiniGame_Reward reward )
		{
			mReward = reward;
			var OpenedShown = reward == null ? _OpenState_Empty : _OpenState_Full;
			var OpenedHidden = reward == null ? _OpenState_Full : _OpenState_Empty;

			if (_ClosedState) _ClosedState.SetActive (false);
			if (OpenedHidden) OpenedHidden.SetActive (false);
			if (OpenedShown)  OpenedShown.SetActive (true);

            if (reward!=null && _OnOpenFull.GetPersistentEventCount() > 0)
                _OnOpenFull.Invoke();
			else
			if (reward==null && _OnOpenEmpty.GetPersistentEventCount() > 0)
				_OnOpenEmpty.Invoke();

			
			StartCoroutine (DelayedOpen (controller));
		}

		private IEnumerator DelayedOpen(MiniGame_Controller controller)
		{
			if (_TimeCollectReward>0) 
				yield return new WaitForSeconds(_TimeCollectReward);

			// If should automatically open reward  (-1 disables this)
			if(_TimeCollectReward>=0)
				OpenReward(controller);

			if (_TimeNewRound>0) 
				yield return new WaitForSeconds(_TimeNewRound);

			// If should get ready for next round (-1 disables this)
			if (_TimeNewRound>=0 && controller) 
				controller.OnReadyForNextRound ();
			yield break;
		}

		public virtual void OpenReward(MiniGame_Controller controller)
		{
			if (!mReward)
				return;

			mReward.Execute (controller._Game, transform);
			mReward = null;
		}
	}
}