using UnityEngine;
using System.Collections;
using SgLib;

namespace Emoji
{
    public class PlayerController : MonoBehaviour
    {
        public PlayerAnim anim;

        public static event System.Action PlayerDied;

        void OnEnable()
        {
            GameManager.GameStateChanged += OnGameStateChanged;
        }

        void OnDisable()
        {
            GameManager.GameStateChanged -= OnGameStateChanged;
        }

        void Start()
        {
            Character selectedCharacter = CharacterManager.Instance.characters[CharacterManager.Instance.CurrentCharacterIndex];
            GetComponentInChildren<SpriteRenderer>().sprite = selectedCharacter.sprite;
        }

        // Listens to changes in game state
        void OnGameStateChanged(GameState newState, GameState oldState)
        {
			if (newState == GameState.Playing) {
				GetComponentInChildren<SpriteRenderer> ().enabled = true;
				GetComponentInChildren<Rigidbody2D> ().gravityScale = GameManager.Instance.playerGravityScale;
			} else if (newState == GameState.GameOver) {
				GetComponentInChildren<SpriteRenderer> ().enabled = false;
				GetComponentInChildren<Rigidbody2D> ().gravityScale = 0f;
			}
        }

        // Calls this when the player dies and game over
        public void Die()
        {
            // Fire event
            PlayerDied();
        }
    }
}