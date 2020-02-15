using UnityEngine;
using System.Collections;
using I2.MiniGames;

public class Example_Reward : MonoBehaviour 
{
	public MiniGame _Game;
	public UnityEventString _OnResult = new UnityEventString();

	public void CollectReward( int Index )
	{
		MiniGame_Reward r1 = _Game.mRewards [Index];
		string result = string.Format ("{0}({1})", Index, r1.name);

		_OnResult.Invoke (result);
	}
}
