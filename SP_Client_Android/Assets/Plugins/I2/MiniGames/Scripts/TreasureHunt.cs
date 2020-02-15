using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace I2.MiniGames
{
	[AddComponentMenu("I2/MiniGames/TreasureHunt/Game")]
	public class TreasureHunt : MiniGame 
	{
		public Transform _Slots;

		public int _InitialSlots = -1;
		public int _MaxRewardsPerGame = -1;


		//--[ Internal Variables ]---------------

		protected List<TreasureHunt_Slot> mSlots = new List<TreasureHunt_Slot>();

		private TreasureHunt_Slot mCurrentSlot;

		#region Setup

		public override void ApplyLayout ()
		{
			base.ApplyLayout ();

			//--[ Slots ]----------

			var slots = _Slots.GetComponentsInChildren<TreasureHunt_Slot> (true);
			mSlots = slots.ToList();

			int numSlots = _InitialSlots < 0 ? slots.Length : Mathf.Min (_InitialSlots, slots.Length);
			FilterSlots (numSlots);

			foreach (var slot in slots)
			{
				bool show = mSlots.Contains(slot);
				slot.gameObject.SetActive(show);
				if (show)
					slot.Reset();
			}
		}

		// Remove the extra slots so that only numSlots be available
		// Here you can remove slots in between to make the shape you want
		// for example 
		//  123             123               123
		//  456  -(7,9)-->  456   instead of  456
		//  789              8                7
		public virtual void FilterSlots( int numSlots )
		{
			if (numSlots >= mSlots.Count)
				return;

			mSlots.RemoveRange (numSlots, mSlots.Count - numSlots);
		}

		public override void FilterRewards()
		{
			base.FilterRewards ();

            if (_MaxRewardsPerGame>=0 && mRewards.Count> _MaxRewardsPerGame)
                mRewards.RemoveRange(_MaxRewardsPerGame, mRewards.Count - _MaxRewardsPerGame);
        }

		public override int NumChoices() 
		{
			return mSlots.Count;
		}


		#endregion

		#region Round

		public override bool CanPlayAnotherRound()	
		{ 
			return mSlots.Count>0; 
		}

		public void OnSlotSelected( TreasureHunt_Slot slot )
		{
			mCurrentSlot = slot;
			TryPlayingRound ();
		}

        public override void StartRound()
        {
            //--[Open Current Slot and find what's inside ]--------

            mSlots.Remove(mCurrentSlot);
            mCurrentSlot.Open( _Controller, GetRandomReward() );
			mCurrentSlot = null;
        }

        #endregion
    }
}