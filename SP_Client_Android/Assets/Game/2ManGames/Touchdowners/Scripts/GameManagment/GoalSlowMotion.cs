using UnityEngine;

namespace Touchdowners
{

    // Slow Motion effect after touchdown happened
    public class GoalSlowMotion : MonoBehaviour
    {

        [SerializeField] private float _slowMotionTimeScale = 0.4f;

        private void Awake()
        {
            GameManager gameManager = GetComponent<GameManager>();
            gameManager.OnBallScoredEvent += ActivateSlowMotion;
            gameManager.OnReturnToStartStateEvent += DeactivateSlowMotion;
        }

        private void ActivateSlowMotion(PlayerType type)
        {
            Time.timeScale = _slowMotionTimeScale;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
        }

        private void DeactivateSlowMotion()
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
        }
    }

}