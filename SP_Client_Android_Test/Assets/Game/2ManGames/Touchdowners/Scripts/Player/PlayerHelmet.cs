using UnityEngine;

namespace Touchdowners
{

    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CircleCollider2D))]
    public class PlayerHelmet : MonoBehaviour
    {
        [SerializeField] private float _detachMinBallVelocity = 10f;

        private bool _isDetachedFromHead;

        private Collider2D _collider2D;
        private Rigidbody2D _rb2D;

        #region MonoBehaviour

        private void Awake()
        {
            _rb2D = GetComponent<Rigidbody2D>();
            _collider2D = GetComponent<Collider2D>();

            _rb2D.isKinematic = true;
            _collider2D.isTrigger = true;

            SubscribeToGameManagerEvents();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (CanBeDetached(collision))
            {
                _isDetachedFromHead = true;
                DetachFromHead();
            }
        }

        #endregion

        private Transform _initialParent;
        private Vector3 _initialLocalPosition;
        private Quaternion _initialLocalRotation;

        private void SubscribeToGameManagerEvents()
        {
            if (GameObject.FindWithTag(Tags.GameManager) == null)
            {
                Debug.Log("Input: no GameManager found in the scene.");
                return;
            }

            _initialParent = transform.parent;
            _initialLocalPosition = transform.localPosition;
            _initialLocalRotation = transform.localRotation;

            GameManager gameManager = GameObject.FindWithTag(Tags.GameManager).GetComponent<GameManager>();
            gameManager.OnReturnToStartStateEvent += AttachToHead;
        }

        private bool CanBeDetached(Collider2D collision)
        {
            if (_isDetachedFromHead)
                return false;

            if (!collision.CompareTag(Tags.Ball))
                return false;

            Ball ball = collision.GetComponent<Ball>();

            if (!ball.CanInteract)
                return false;

            if (collision.GetComponent<Rigidbody2D>().velocity.magnitude < _detachMinBallVelocity)
                return false;

            if (ball.PlayerHolderType != PlayerType.None)
                return false;

            return true;
        }

        private void DetachFromHead()
        {
            _collider2D.isTrigger = false;
            _rb2D.isKinematic = false;

            transform.parent = null;
        }

        public void AttachToHead()
        {
            transform.parent = _initialParent;
            transform.localPosition = _initialLocalPosition;
            transform.localRotation = _initialLocalRotation;

            _collider2D.isTrigger = true;
            _rb2D.isKinematic = true;

            _isDetachedFromHead = false;
        }
    }

}