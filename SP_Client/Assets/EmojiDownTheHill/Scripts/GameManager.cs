using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using SgLib;

namespace Emoji2
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

        [Header("Gameplay Config")]
        [Range(0.0025f, 0.25f)]
        public float refillCubeFrequency = 0.02f;

        [Range(0.0025f, 0.25f)]
        public float treeFrequency = 0.1f;

        [Range(0.0025f, 0.25f)]
        public float springFrequency = 0.1f;

        [Range(0.1f, 2f)]
        public float gameSpeed = 1;

        // store the bounds.x & bounds.y value of the sprite ( in this case , the  green cube - the ground)
        [HideInInspector]   public float boundsX;
        [HideInInspector]   public float boundsY;

        void Awake()
        {
            Instance = this;
        }

        void OnEnable()
        {
            PlayerController.PlayerDied += PlayerController_PlayerDied;
        }

        void OnDisable()
        {
            PlayerController.PlayerDied -= PlayerController_PlayerDied;
        }

        void Start()
        {
            GameState = GameState.Prepare;
            ScoreManager.Instance.Reset();
            Time.timeScale = gameSpeed;	
        }

        void PlayerController_PlayerDied()
        {
            GameOver();
        }

        public void StartGame()
        {
            GameState = GameState.Playing;
        }

        public void GameOver()
        {
            GameState = GameState.GameOver;
            SoundManager.Instance.PlaySound(SoundManager.Instance.gameOver);
        }

        public void RestartGame(float delay = 0)
        {
            StartCoroutine(CRRestartGame(delay));
        }

        IEnumerator CRRestartGame(float delay = 0)
        {
            yield return new WaitForSeconds(delay);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}