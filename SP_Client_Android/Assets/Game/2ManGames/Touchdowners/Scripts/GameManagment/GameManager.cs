using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

namespace Touchdowners
{

    // This class is responsible for managing Game state (start/stop/resume/finish)
    public class GameManager : MonoBehaviour
    {
        [Header("- Game settings -")]
        [SerializeField] private int _countdownTime = 3;
        [SerializeField] private int _maxScore = 3;

        [Header("- Touchdown areas -")]
        [SerializeField] private float _disabilityTimeAfterTouchdown = 3f;
        [SerializeField] private TouchdownArea _leftPlayersArea;
        [SerializeField] private TouchdownArea _rightPlayersArea;

        [Header("- UI Text -")]
        [SerializeField] private Text _countdownText;
        [SerializeField] private Text _leftPlayersScoreText;
        [SerializeField] private Text _rightPlayersScoreText;

        [Header("- UI Buttons -")]
        [SerializeField] private GameObject _pauseButtonObject;
        [SerializeField] private GameObject _pauseObject;
        [SerializeField] private GameObject _gameFinishObject;

        private int _leftPlayersScore;
        private int _rightPlayersScore;

        private bool _canUpdateScore;

        public delegate void OnGameStarted();
        public delegate void OnGameStopped();
        public delegate void OnGameResumed();
        public delegate void OnBallScored(PlayerType playerTypeThatScored);
        public delegate void OnGameToStartState();
        public delegate void OnGameFinished(PlayerType playerTypeWinner);

        public event OnGameStarted OnGameStartedEvent;
        public event OnGameStopped OnGameStoppedEvent;
        public event OnGameResumed OnGameResumedEvent;
        public event OnBallScored OnBallScoredEvent;
        public event OnGameToStartState OnReturnToStartStateEvent;
        public event OnGameFinished OnGameFinishedEvent;

        #region MonoBehaviour

        private void Awake()
        {
            _leftPlayersArea.OnTouchdownEvent += IncreaseScore;
            _rightPlayersArea.OnTouchdownEvent += IncreaseScore;

            OnGameStartedEvent += () => _canUpdateScore = true;
            OnReturnToStartStateEvent += () => StartCoroutine(CountdownEnumerator());

            UpdateLeftPlayersScore();
            UpdateRightPlayersScore();
        }

        private void Start()
        {
            if (OnReturnToStartStateEvent != null)
                OnReturnToStartStateEvent();
        }

        #endregion

        private float _stopTimeScale;
        public void StopGame()
        {
            if (OnGameStoppedEvent != null)
                OnGameStoppedEvent();

            _stopTimeScale = Time.timeScale;
            Time.timeScale = 0;

            _pauseObject.SetActive(true);
            _pauseButtonObject.SetActive(false);
        }

        public void ResumeGame()
        {
            if (OnGameResumedEvent != null)
                OnGameResumedEvent();

            Time.timeScale = _stopTimeScale;

            _pauseObject.SetActive(false);
            _pauseButtonObject.SetActive(true);
        }

        public void Replay()
        {
            Time.timeScale = 1;

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void LoadHomeScreen()
        {
            Time.timeScale = 1;

            SceneManager.LoadScene(0);
        }

        private IEnumerator CountdownEnumerator()
        {
            float time = _countdownTime;
            _countdownText.text = Mathf.CeilToInt(time).ToString();

            while (time > 0)
            {
                time -= Time.deltaTime;
                _countdownText.text = Mathf.CeilToInt(time).ToString();
                yield return null;
            }

            _countdownText.text = "GO";

            if (OnGameStartedEvent != null)
                OnGameStartedEvent();
        }

        private void IncreaseScore(PlayerType playersTypeThatScored)
        {
            if (!_canUpdateScore)
                return;

            _canUpdateScore = false;

            switch(playersTypeThatScored)
            {
                case PlayerType.LeftPlayer:
                    _leftPlayersScore++;
                    UpdateLeftPlayersScore();
                    break;

                case PlayerType.RightPlayer:
                    _rightPlayersScore++;
                    UpdateRightPlayersScore();
                    break;
            }

            if(_leftPlayersScore == _maxScore)
                FinishGame(PlayerType.LeftPlayer);
            else if(_rightPlayersScore == _maxScore)
                FinishGame(PlayerType.RightPlayer);
            else
            {
                StartCoroutine(DisableTimeEnumerator());

                if (OnBallScoredEvent != null)
                    OnBallScoredEvent(playersTypeThatScored);
            }
        }

        private IEnumerator DisableTimeEnumerator()
        {
            float time = _disabilityTimeAfterTouchdown;
            while(time > 0)
            {
                time -= Time.deltaTime;
                yield return null;
            }

            if(OnReturnToStartStateEvent != null)
                OnReturnToStartStateEvent();
        }

        private void FinishGame(PlayerType playerWinnerType)
        {
            NotifyAboutGameFinish(playerWinnerType);

            _pauseButtonObject.SetActive(false);
            _gameFinishObject.SetActive(true);
        }

        private void NotifyAboutGameFinish(PlayerType playerWinnerType)
        {
            if(OnGameFinishedEvent != null)
                OnGameFinishedEvent(playerWinnerType);
        }

        private void UpdateLeftPlayersScore()
        {
            _leftPlayersScoreText.text = _leftPlayersScore.ToString();
        }

        private void UpdateRightPlayersScore()
        {
            _rightPlayersScoreText.text = _rightPlayersScore.ToString();
        }

    }

}