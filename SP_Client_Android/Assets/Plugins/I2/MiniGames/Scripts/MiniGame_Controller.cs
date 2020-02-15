using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace I2.MiniGames
{
	[System.Serializable]public class UnityEventCurrencyCallbk : UnityEvent<int, System.Action<bool>>{}

	// This Class implements the flow and monetization logic for all MiniGames
	// It provides some free rounds and then allow paying for extra chances and purchasing currency if there is not enough. 
    [AddComponentMenu("I2/MiniGames/Controller")]
    public class MiniGame_Controller : MonoBehaviour
	{
		#region Variables

		public MiniGame _Game;						// Game this controllers manages
		public bool _StartGameOnEnable = false;		// If true, the game will be initialized when this object is enabled
		public float _TimeBeforeStartRound = 0;		// This delay can be used to start the round after some effects/sounds are finished

		//--[ Events to hide/show labels and update texts ]--------------

		public UnityEvent 		_OnStartPlaying 	= new UnityEvent();			// Show/Hide objects for the initial state, execute the entrance effect and call OnReadyForNextRound right after it finishes
		public UnityEvent 		_OnRoundStarted	    = new UnityEvent();
		public UnityEvent 		_OnGameOver 	    = new UnityEvent();

		//--[ Free Rounds ]---------------------
		[Header("Free Rounds")]
		public int _InitialFreeRounds;											// How many rounds are allowed without having to pay for them
		[Multiline] public string _FreeRoundsLabelFormat = "{0} left";			// The status text will show the num free rounds using this format {0} is replaced by the remaining rounds

		public UnityEventString _OnUpdateFreeRounds = new UnityEventString();	// Shows the Free Round button and updates the label's text following the _FreeRoundsLabelFormat

		//--[ Purchaseable Rounds ]------------------
		
		[Header("Purchasable Rounds")]
		[Multiline] public string _CostLabelFormat = "{0} coins for another chance";
		public int _InitialRoundCost = 10;	// how much cost the first round after the free ones are over
		public int _ExtraCostPerRound = 5;  // how much to increase the cost of the following rounds

		public UnityEventString _OnUpdateRoundCost 	= new UnityEventString();	// Hides the Free Round button, shows the Buy Chance

		//--[ Currency Callbacks ]----------
		
		[Header("Currency Management")]
		public UnityEvent 				_OnShouldBuyCurrency= new UnityEvent();				// Open the popup to buy currency and when finished calls ValidateRound again or DenyRound
		public UnityEventCurrencyCallbk _OnConsumeCurrency = new UnityEventCurrencyCallbk();// Consume currency and returns the result using the supplied callback


		//--[ Used to keep track of the game ]----------
		
		private int mNumFreeRounds;   // Remaining free tries
		private int mNumPayedRounds;  // Number of times the players has bought a spin
		private bool mIsPlaying;	  // Its true whenever the game is initialized and its possible to play another round

		#endregion

		#region Currency Management

		// Fake Currency (this is used only for the demo, when used in a game, the _OnConsumeCurrency event 
		// should be redirect to the game's resource to decrease the currency and call OnConsumeCurrencyResult
		// with true/false depending on whatever there was enough currency available
		private int CurrencyAmount;

		// function used to start the consume and purchase if necessary. It implements a fake approach for the demo
		// and a normal flow when _OnConsumeCurrency its set
		public void TryConsumeCurrency( int Amount )
		{
			if (_OnConsumeCurrency.GetPersistentEventCount () > 0) 
			{
				_OnConsumeCurrency.Invoke(Amount, OnConsumeCurrencyResult);
			}
			else
			{
				// Fake currnecy (just for the demo scene)

				if (CurrencyAmount < Amount)
				{
					OnConsumeCurrencyResult( false );
				}
				else
				{
					CurrencyAmount -= Amount;
					OnConsumeCurrencyResult( true );
				}
			}
		}

		// Example of ConsumeCurrency function
		public void TestConsumeCurrency( int Amount, System.Action<bool> result )
		{
			if (CurrencyAmount < Amount)
			{
				result( false );
			}
			else
			{
				CurrencyAmount -= Amount;
				result( true );
			}
		}
			
		// This is only used in the demo to simulate user's purchase
		public void AddCurrency( int Amount )
		{
			CurrencyAmount += Amount;
		}
		// This is only used in the demo to simulate user's purchase (this version is used as NGUI doesn't support passing an int to the Button's OnClick)
		public void AddTestCurrency()
		{
			AddCurrency (25);
		}

		// Returns the cost of the next round if there are no more rounds available
		// The first payed round will cost _InitialRoundCost and further rounds will increment the cost by _ExtraCostPerRound amount
		public virtual int GetCurrentRoundCost()
		{
			return mNumFreeRounds > 0 ? 0 : _InitialRoundCost + mNumPayedRounds * _ExtraCostPerRound;
		}

		#endregion

		#region Setup and State Management

		public void OnEnable()
		{
			if (_StartGameOnEnable)
				Invoke("StartGame", 0);
		}

		// Initializes the game and starts the first round
		public void StartGame()
		{
			mNumFreeRounds = _InitialFreeRounds;
			mNumPayedRounds = 0;
			mIsPlaying = true;

			_Game.SetupGame ();

			_OnStartPlaying.Invoke ();  // should call OnReadyForNextRound when finishing the entrance effect;

			if (_TimeBeforeStartRound > 0)
				Invoke ("OnReadyForNextRound", _TimeBeforeStartRound);
			else
			if (_TimeBeforeStartRound >= 0)
				OnReadyForNextRound ();
		}

		// Stops the game and prevent any further play intent until the game is reseted
		public virtual void StopGame()
		{
			mNumFreeRounds = mNumPayedRounds = 0;
			mIsPlaying = false;
			_OnGameOver.Invoke();
		}

		// Restarts the game from the begining
		public virtual void ResetGame()
		{
			StopGame ();
			StartGame ();
		}

		// Execute _OnGameOver, _OnUpdateFreeRounds or _OnUpdateRoundCost based on game state
		public void OnReadyForNextRound()
		{
			if (!mIsPlaying)
				return;

			if (!_Game.CanPlayAnotherRound())
			{
				StopGame();
			}
			else
			if (mNumFreeRounds > 0)
			{
				_OnUpdateFreeRounds.Invoke (string.Format (_FreeRoundsLabelFormat, mNumFreeRounds));
			}
			else
			{
				_OnUpdateRoundCost.Invoke (string.Format (_CostLabelFormat, GetCurrentRoundCost()));
			}
		}

		#endregion

		#region Validation

		// Checks if there are free rounds or enough currency to buy a new round. 
		// If so, it calls AllowRound, otherwise it shows the BuyCurrency popup or denies the round
		public void ValidateRound()
		{
			//--[ validate costs and call AllowRound/DenyRound accordinly ]-----

			if (!mIsPlaying)
			{
				DenyRound();
				return;
			}

			if (mNumFreeRounds <= 0)
			{
				// if suficient funds
				//    decrement cost from your inventory and then continue, 
				// else
				//    ask for a purchase or offer seeing an incentivized video ad or buying and 
				//    call this function again when the task is done so the cost gets verified again

				TryConsumeCurrency(GetCurrentRoundCost());  // this will call OnConsumeCurrencyResult
				return;
			}

			AllowRound ();
		}

		// This should be called after the ValidateRound tried to buy currency, 
		//  If there are not enough available the BuyCurrency popup is executed
		private void OnConsumeCurrencyResult( bool Success )
		{
			if (Success) 
			{
				AllowRound ();
			}
			else // there was not enough currency
			{
				// If there is not BuyCurrency popup, then deny the round
				if (_OnShouldBuyCurrency.GetPersistentEventCount()<=0)
					DenyRound();
				else
					_OnShouldBuyCurrency.Invoke();
			}
		}

		// After the round is validated, this function continues the game flow to play another round (PrizeWheel: spins the wheel, etc)
		public void AllowRound()
		{
			if (mNumFreeRounds > 0)
				mNumFreeRounds--;
			else
				mNumPayedRounds++;

			_OnRoundStarted.Invoke ();

			_Game.StartRound ();
		}

		// The round validation failed, so this function stops the current play attempt 
		public void DenyRound()
		{
			_Game.CancelRound ();
		}

		#endregion
	}
}