using UnityEngine;

namespace Touchdowners
{

    // Text that will be shown when Touchdown is happened
    [RequireComponent(typeof(Animator))]
    public class TouchdownText : MonoBehaviour
    {

        [SerializeField] private string _animationBool = "Play";
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();

            GameManager gameManager = GameObject.FindWithTag(Tags.GameManager).GetComponent<GameManager>();

            gameManager.OnBallScoredEvent += (PlayerHandType) => _animator.SetBool(_animationBool, true);
            gameManager.OnReturnToStartStateEvent += () => _animator.SetBool(_animationBool, false);
        }
    }

}