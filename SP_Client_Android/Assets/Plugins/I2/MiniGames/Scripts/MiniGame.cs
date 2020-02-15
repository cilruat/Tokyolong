using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

namespace I2.MiniGames
{
	[System.Serializable]public class UnityEventString : UnityEvent<string>{}
	[System.Serializable]public class UnityEventInt : UnityEvent<int>{}
	[System.Serializable]public class UnityEventFloat : UnityEvent<float>{}
	[System.Serializable]public class UnityEventAudioClip : UnityEvent<AudioClip, float>{}
	[System.Serializable]public class UnityEventTransform : UnityEvent<Transform>{}

	// Base class all of minigames, it implements the Rounds flow if there is a Controller assigned
	public class MiniGame : MonoBehaviour
	{
		public MiniGame_Controller _Controller;

		public Transform _Rewards;	// Every child of this object should have the MiniGames_Reward component assigned

		[Rename("Start On Enabled")]
		public bool _SetupGameOnEnable = false;  // If true, in Enable it will start the Game Initialization, if a controller is used, its recomended to use the controller._StartGameOnEnable instead of this one 

		public UnityEvent _OnStart = new UnityEvent();  // Event to allow calling functions when the game is initialized (can be used to enable/disable objects and effects)

		//--[ Internal Variables ]---------------
		
		[HideInInspector][NonSerialized] 
		public List<MiniGame_Reward> mRewards = new List<MiniGame_Reward> (); // This rewards are filled from the _Reward childs.


		// Apply the Layout and Initialize the game
		public virtual void SetupGame()				{ ApplyLayout (); _OnStart.Invoke (); }

		// Arrange the rewards and other game elements in the initial state
		public virtual void ApplyLayout()			
		{
			//--[ Read Rewards ]--------------
			var Rewards = _Rewards.GetComponentsInChildren<MiniGame_Reward> (true);

			mRewards = Rewards.ToList ();
			FilterRewards ();

			foreach (var Reward in Rewards)
				Reward.Hide ();
			mRewards.RemoveAll (r => r.Probability <= 0);
		}

		// From the mRewards, it removes the rewards that shoudn't be used according to the gameplay (some powerups, need to be unlocked, some gifts are not available yet, etc)
		public virtual void FilterRewards()
		{
			// Here remove any reward (Reward) that shoudn't be available based on the game progression
			
			//mRewards.RemoveAt (1);        // one way,
			//SetRewardPriority("Bomb", 0); // another way
		}

		// False, if the game is over, otherwise, the game is able to be played again (PrizeWheel: can spin, TreasureHunt: there are closed slots that can be opened, etc)
		public virtual bool CanPlayAnotherRound()	{ return true; }

		// Execute the next round if possible (e.g. there are free retries or enough currency to purchasable another chance)
		public void TryPlayingRound()
		{
			if (_Controller)
				_Controller.ValidateRound(); // this will call StartRound or CancelRound
			else
				StartRound();
		}

		// Starts a Retry (PrizeWheel: Spin, TreasureHunt: open a closed slot)
		public virtual void StartRound ()	{}

		// A round was requested, but the controller didn't allow it (not enough currency or not available options)
		public virtual void CancelRound ()	{}

		public void OnEnable()
		{
			if (_SetupGameOnEnable)
				Invoke("SetupGame", 0);
		}

		// Selects a random reward from the mRewards array by using their probabilities.
		// Can also return null if there are no more rewards or there are more choises than rewards (i.e. elements that don't contain a reward)
		public virtual MiniGame_Reward GetRandomReward()
		{
			if (mRewards.Count <= 0)
				return null;
			
			int numEmptySlots = Mathf.Max(0, NumChoices() - mRewards.Count);
			float total = numEmptySlots + mRewards.Sum( e=>e.Probability );
			
			float rnd = UnityEngine.Random.Range (0, total);
			var Reward = mRewards.Find( e=>{ rnd -= e.Probability; return e.Probability>0 && rnd<=0; } );
			
			if (Reward)
				mRewards.Remove (Reward);
			
			return Reward;
		}

		//All the posible options (TreasureHunt: Num of remaining closed chests)
		public virtual int NumChoices() 
		{
			return 0;
		}

		// Assigns a Probability to the reward. The reward can be in the mRewards list if the 
		// game has been initialized or as a child of _Reward in the Hierarchy 
		public void SetRewardPriority( string RewardName, float Probability )
		{
			MiniGame_Reward reward = mRewards.Find (r => r.name == RewardName);
			if (reward==null)
				reward = _Rewards.GetComponentsInChildren<MiniGame_Reward> (true).First (r => r.name == RewardName);

			if (reward != null)
			{
				reward.Probability = Probability;
				if (Probability>0 && !mRewards.Contains(reward))
					mRewards.Add(reward);

				if (Probability<=0 && mRewards.Contains(reward))
					mRewards.Remove(reward);
			}
		}

		// This functions are used in the NGUI prefabs to enable/disable objects from the Button's OnClick
		public void EnableGameObject( GameObject go )
		{
			go.SetActive (true);
		}
		public void DeactivateGameObject( GameObject go )
		{
			go.SetActive (false);
		}

	}
}