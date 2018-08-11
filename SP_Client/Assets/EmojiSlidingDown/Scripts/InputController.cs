using UnityEngine;
using System.Collections;
using Emoji;

public class InputController : MonoBehaviour
{
    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (GameManager.Instance.GameState == GameState.Playing)
                GameManager.Instance.rotationDirection = GameManager.Instance.rotationDirection == 0 ? GameManager.Instance.firstRotationDirection : -GameManager.Instance.rotationDirection;
        }
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
