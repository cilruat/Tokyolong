using UnityEngine;
using System;
using System.Collections;

namespace OnefallGames
{
    public class ScoreManager : MonoBehaviour
    {
        public static ScoreManager Instance { get; private set; }

        public int Score { get; private set; }

        public int BestScore { get; private set; }

        public bool HasNewBestScore { get; private set; }

        public static event Action<int> ScoreUpdated = delegate {};
        public static event Action<int> BestScoreUpdated = delegate {};

        private const string BESTSCORE = "BESTSCORE";
        // key name to store high score in PlayerPrefs

        void Awake()
        {
            if (Instance)
            {
                DestroyImmediate(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        void Start()
        {
            Reset();
        }

        public void Reset()
        {
            // Initialize score
            Score = 0;

            // Initialize highscore
            BestScore = PlayerPrefs.GetInt(BESTSCORE, 0);
            HasNewBestScore = false;
        }

        public void AddScore(int amount)
        {
            Score += amount;

            // Fire event
            ScoreUpdated(Score);
            if (Score > BestScore)
            {
                UpdateHighScore(Score);
                HasNewBestScore = true;
            }
            else
            {
                HasNewBestScore = false;
            }
        }

        public void UpdateHighScore(int newHighScore)
        {
            // Update highscore if player has made a new one
            if (newHighScore > BestScore)
            {
                BestScore = newHighScore;
                PlayerPrefs.SetInt(BESTSCORE, BestScore);
                BestScoreUpdated(BestScore);
            }
        }
    }
}
