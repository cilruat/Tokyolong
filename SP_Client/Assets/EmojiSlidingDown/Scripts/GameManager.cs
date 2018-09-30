using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Emoji
{
    public enum GameState
    {
        Prepare,
        Playing,
        Paused,
        PreGameOver,
        GameOver
    }

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public static event System.Action<GameState, GameState> GameStateChanged = delegate { };

        public GameState GameState
        {
            get
            {
                return _gameState;
            }
            private set
            {
                if (value != _gameState)
                {
                    GameState oldState = _gameState;
                    _gameState = value;

                    GameStateChanged(_gameState, oldState);
                }
            }
        }

        [SerializeField]
        private GameState _gameState = GameState.Prepare;

        public static int GameCount
        {
            get { return _gameCount; }
            private set { _gameCount = value; }
        }

        private static int _gameCount = 0;

        [Header("Set the target frame rate for this game")]
        [Tooltip("Use 60 for games requiring smooth quick motion, set -1 to use platform default frame rate")]
        public int targetFrameRate = 30;

        // List of public variable for gameplay tweaking
        [Header("Gameplay Config")]

        [Range(0f, 1f)]
        public float coinFrequency = 0.1f;
        [Range(0f, 1f)]
        public float obstacleFrequency = 0.1f;
        public float playerGravityScale;
        public float cloudVerticleOffset;
        public float cloudHorizontalOffset;
        public float cloudInititalPosY;
        public int maxRotatingAngle = 20;
        public float rotationDelta = 10;
        [HideInInspector]
        public float rotationDirection = 0;
        [HideInInspector]
        public float firstRotationDirection;
        // List of public variables referencing other objects
        [Header("Object References")]
        public PlayerController playerController;
        public Rigidbody2D playerRigidbody;
        public DeathPlane playerDeathPlane;
        public GameObject cloudPrefab;
		public GameObject objBoard;

        void OnEnable()
        {
            PlayerController.PlayerDied += PlayerController_PlayerDied;
        }

        void OnDisable()
        {
            PlayerController.PlayerDied -= PlayerController_PlayerDied;
        }

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                DestroyImmediate(Instance.gameObject);
                Instance = this;
            }
        }

        void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }

        // Use this for initialization
        void Start()
        {
            // Initial setup
            //Application.targetFrameRate = targetFrameRate;
            ScoreManager.Instance.Reset();

            PrepareGame();
        }

        // Listens to the event when player dies and call GameOver
        void PlayerController_PlayerDied()
        {
            GameOver();
        }

        // Make initial setup and preparations before the game can be played
        public void PrepareGame()
        {
            GameState = GameState.Prepare;
        }

        // A new game officially starts
        public void StartGame()
        {
            GameState = GameState.Playing;
            if (SoundManager.Instance.background != null)
            {
                SoundManager.Instance.PlayMusic(SoundManager.Instance.background);
            }
        }

        // Called when the player died
        public void GameOver()
        {
            if (SoundManager.Instance.background != null)
            {
                SoundManager.Instance.StopMusic();
            }

            SoundManager.Instance.PlaySound(SoundManager.Instance.gameOver);
            GameState = GameState.GameOver;
            GameCount++;

            // Add other game over actions here if necessary
        }

        // Start a new game
        public void RestartGame(float delay = 0)
        {
            StartCoroutine(CRRestartGame(delay));
        }

        IEnumerator CRRestartGame(float delay = 0)
        {
            yield return new WaitForSeconds(delay);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

		public void ReturnHome()
		{
			SceneChanger.LoadScene ("Main", objBoard);
		}
    }
}