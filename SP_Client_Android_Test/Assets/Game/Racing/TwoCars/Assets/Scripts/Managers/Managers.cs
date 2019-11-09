//  /*********************************************************************************
//   *********************************************************************************
//   *********************************************************************************
//   * Produced by Skard Games										                  *
//   * Facebook: https://goo.gl/5YSrKw											      *
//   * Contact me: https://goo.gl/y5awt4								              *											
//   * Developed by Cavit Baturalp Gürdin: https://tr.linkedin.com/in/baturalpgurdin *
//   *********************************************************************************
//   *********************************************************************************
//   *********************************************************************************/

using UnityEngine;
using System.Collections;

namespace TwoCars
{
	[RequireComponent(typeof(GameManager))]
	[RequireComponent(typeof(DifficultyManager))]
	[RequireComponent(typeof(UIManager))]
	[RequireComponent(typeof(AudioManager))]
	[RequireComponent(typeof(ScoreManager))]
	[RequireComponent(typeof(PlayerInputManager))]
	[RequireComponent(typeof(AnalyticsManager))]
	[RequireComponent(typeof(AdvertisementManager))]
	[RequireComponent(typeof(SpawnManager))]
	public class Managers : MonoBehaviour
	{
		private static GameManager _gameManager;
		public static GameManager Game
		{
			get { return _gameManager; }
		}

		private static UIManager _uiManager;
		public static UIManager UI
		{
			get { return _uiManager; }
		}

	    private static DifficultyManager _difficultyManager;
	    public static DifficultyManager Difficulty
	    {
	        get { return _difficultyManager; }
	    }

	    private static AudioManager _audioManager;
		public static AudioManager Audio
		{
			get { return _audioManager; }
		}

		private static ScoreManager _scoreManager;
		public static ScoreManager Score
		{
			get { return _scoreManager; }
		}

		private static PlayerInputManager _inputManager;
		public static PlayerInputManager Input
		{
			get { return _inputManager; }
		}

		private static AdvertisementManager _advertisementManager;
		public static AdvertisementManager Adv
		{
			get { return _advertisementManager; }
		}

		private static AnalyticsManager _analyticManager;
		public static AnalyticsManager Anal
		{
			get { return _analyticManager; }
		}

		private static CameraManager _cameraManager;
		public static CameraManager Cam
		{
			get { return _cameraManager; }
		}

		private static SpawnManager _spawnManager;
		public static SpawnManager Spawner
		{
			get { return _spawnManager; }
		}


		void Awake ()
		{
			_gameManager = GetComponent<GameManager>();
			_uiManager = GetComponent<UIManager>();
			_audioManager = GetComponent<AudioManager>();
			_scoreManager = GetComponent<ScoreManager> ();
			_inputManager = GetComponent<PlayerInputManager> ();
			_advertisementManager = GetComponent<AdvertisementManager> ();
			_analyticManager = GetComponent<AnalyticsManager> ();
			_cameraManager = GetComponent<CameraManager> ();
			_spawnManager = GetComponent<SpawnManager> ();
	        _difficultyManager = GetComponent<DifficultyManager>();

			//DontDestroyOnLoad(gameObject);
		}
	}
}