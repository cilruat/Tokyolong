using UnityEngine;

namespace Touchdowners
{

    [RequireComponent(typeof(Animator))]
    public class Airship : MonoBehaviour
    {
        [SerializeField] private string _boolAtStartPosition = "AtStart";

        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();

            GameManager gameManager = GameObject.FindWithTag(Tags.GameManager).GetComponent<GameManager>();
            gameManager.OnGameStartedEvent += () => _animator.SetBool(_boolAtStartPosition, false);
            gameManager.OnReturnToStartStateEvent += () => _animator.SetBool(_boolAtStartPosition, true);
        }
    }

}