using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using I2.MiniGames;

public class Example_3Rewards : MonoBehaviour 
{
	public MiniGame _Game;
	public UnityEventString _OnResult = new UnityEventString();

	public UnityEvent[] _OnStarEnabled;
	public UnityEvent[] _OnStarDisabled;

	int mNumStars;

	public void Setup()
	{
		mNumStars = GetNumStars();

		for (int i=0; i<mNumStars; ++i)
			_OnStarEnabled [i].Invoke ();

		for (int i=mNumStars; i<3; ++i)
			_OnStarDisabled [i].Invoke ();
	}

	int GetNumStars()
	{
		// Return here the num of stars the player got in the level
		// this next is just a random example 

		// gets a random number and tries to avoid repeting the previous numStars (also tries to avoid 0 stars)
		int prevNumStars = mNumStars;
		mNumStars = Random.Range(1, 4);

		if (mNumStars == prevNumStars)
			mNumStars = (mNumStars + 1) % 3;
		if (mNumStars == 0)
			mNumStars = 1;

		return mNumStars;
	}

	public void CollectReward( int Index )
	{
		int count = _Game.mRewards.Count;
		var ids = new int[3];
		//var rewards = new MiniGame_Reward[3];

		ids[0] = (Index+count-1)%count;
		ids[1] = Index;
		ids[2] = (Index+1)%count;

		string result = "";
		for (int i=0; i<mNumStars; ++i)
		{
			if (i>0) result += ", ";

			var reward =  _Game.mRewards [ids[i]];
			result += string.Format ("{0}({1})", ids[i], reward.name);
		}

		_OnResult.Invoke (result);
	}
}
