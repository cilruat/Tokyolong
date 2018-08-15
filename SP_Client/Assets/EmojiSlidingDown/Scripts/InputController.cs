using UnityEngine;
using System.Collections;

namespace Emoji
{
    public class InputController : MonoBehaviour
    {
        public void Update()
        {
			if (GameManager.Instance.GameState != GameState.Playing)
				return;

			if (Input.GetKeyDown(KeyCode.LeftArrow))
				GameManager.Instance.rotationDirection = 1;
			if (Input.GetKeyDown (KeyCode.RightArrow))
				GameManager.Instance.rotationDirection = -1;
        }

        public void OnEnable()
        {
            GameManager.GameStateChanged += OnGameStateChanged;
        }

        public void OnDisable()
        {
            GameManager.GameStateChanged -= OnGameStateChanged;
        }

        private void OnGameStateChanged(GameState newState, GameState oldState)
        {
            if (oldState == GameState.Prepare && newState == GameState.Playing)
            {
                GameManager.Instance.rotationDirection = 0;
            }
        }
    }
}