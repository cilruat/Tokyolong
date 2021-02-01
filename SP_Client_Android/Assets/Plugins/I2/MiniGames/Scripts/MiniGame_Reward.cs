using UnityEngine.Events;
using UnityEngine;
using System.Collections;

namespace I2.MiniGames
{
	[System.Serializable]public class UnityEventTreasureHunt : UnityEvent<MiniGame_Reward>{}

	// This class represents a possible outcome of a MiniGame.
	// e.g. For a PrizeWheel, this are each of the elments distributed in the wheel
	// e.g For a TreasureHunt, it is the rewards that are hidden inside the slots
	[AddComponentMenu("I2/MiniGames/TreasureHunt/Reward")]
	public class MiniGame_Reward : MonoBehaviour 
	{
		public float Probability = 1;			// How likely is this reward to be chosen

		public bool _AttachToCaller = true;		// When then selected, should it move into the slot or element's position
		public bool _EndGame = false;			// When selected, it stops the game. (e.g. Treasure Hunt, player is searching for the key, when found, the game stops)

        public Animator anim;


		// Callback used for playing effects/sounds when the reward is selected
		public UnityEventTreasureHunt _OnRewarded = new UnityEventTreasureHunt();

		// When selected, shows the reward, stops the game if elected and runs effects and sounds by calling _OnRewarded
		public virtual void Execute( MiniGame game, Transform parent )
		{
			Show (parent);

			if (_EndGame && game && game._Controller)
				game._Controller.StopGame ();

			_OnRewarded.Invoke (this);
		}

		// This is called when the reward is selected and is meant to initialize the reward and move it into the correct location
		public virtual void Show ( Transform parent )
		{
			gameObject.SetActive (true);
			
			if (_AttachToCaller)
				transform.position = parent.position;
                anim.Play("Dog_Bark");

            //보물에 당첨되면 Show 스크립트 실행된다.. 여기서 애니메이션 달고, 그 애니메이션을 실행한다 문구정도만 추가하면 될거같다. 

        }

        public virtual void Hide ()
		{
			gameObject.SetActive (false);
            //다시하기 버튼 누르면 Hide나옴
            anim.Rebind();


        }
    }
}