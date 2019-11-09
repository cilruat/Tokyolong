using UnityEngine;
using System.Collections;

namespace Touchdowners
{

    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class Ball : MonoBehaviour
    {
        private PlayerType _playerHolderType = PlayerType.None;

        // Used when ball attached to the hand or thrown.
        // After some of these action happens, 
        // the ball can not make actions (attach / hit helmet) for the period of time _minTimeBetweenInteractaion.
        private bool _canInteract = true;
        private float _minTimeBetweenInteractaion = 0.1f;

        private Rigidbody2D _rb2D;
        private Collider2D _collider2D;
        private SpriteRenderer _spriteRenderer;

        private int _startSortingLayer;
        private int _startSortingLayerIndex;

        public PlayerType PlayerHolderType { get { return _playerHolderType; } }
        public bool CanInteract { get { return _canInteract; } }

        private void Awake()
        {
            if(!gameObject.CompareTag(Tags.Ball))
                Debug.LogError("Ball: ball has to be tagged as Ball.");

            _rb2D = GetComponent<Rigidbody2D>();
            _collider2D = GetComponent<Collider2D>();

            _rb2D.isKinematic = false;
            _collider2D.isTrigger = false;

            _spriteRenderer = GetComponent<SpriteRenderer>();
            _startSortingLayer = _spriteRenderer.sortingLayerID;
            _startSortingLayerIndex = _spriteRenderer.sortingOrder;

            SubscribeToGameEvents();
        }

        private void SubscribeToGameEvents()
        {
            if(GameObject.FindWithTag(Tags.GameManager) == null)
            {
                Debug.Log("Ball: No GameManager in the scene");
                return;
            }

            SaveStartPosition();

            GameManager gameManager = GameObject.FindWithTag(Tags.GameManager).GetComponent<GameManager>();

            gameManager.OnGameStartedEvent += OnGameStarted;
            gameManager.OnReturnToStartStateEvent += OnGameReturnedToStartState;
        }

        private Vector3 _startPosition;
        private Quaternion _startRotation;

        private void SaveStartPosition()
        {
            _startPosition = transform.position;
            _startRotation = transform.rotation;
        }

        private void OnGameReturnedToStartState()
        {
            transform.parent = null;

            transform.position = _startPosition;
            transform.rotation = _startRotation;

            _playerHolderType = PlayerType.None;

            _rb2D.isKinematic = true;
            _collider2D.isTrigger = false;

            _rb2D.angularVelocity = 0;
            _rb2D.velocity = Vector2.zero;

            _canInteract = true;
        }

        private void OnGameStarted()
        {
            _rb2D.isKinematic = false;
        }

        public void AttachToHand(PlayerHand playerHand)
        {
            _playerHolderType = playerHand.HandType;
           
            _rb2D.isKinematic = true;
            _collider2D.isTrigger = true;

            _rb2D.velocity = Vector2.zero;
            _rb2D.angularVelocity = 0;

            transform.parent = playerHand.BallPosition;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;

            SpriteRenderer handSprite = playerHand.GetComponent<SpriteRenderer>();
            _spriteRenderer.sortingLayerID = handSprite.sortingLayerID;
            _spriteRenderer.sortingOrder = handSprite.sortingOrder + 1;

            _canInteract = false;
            StartCoroutine(AbleToInteract());
        }

        public void Throw(Vector2 throwVelocity)
        {
            _playerHolderType = PlayerType.None;

            _rb2D.isKinematic = false;
            _collider2D.isTrigger = false;

            _rb2D.velocity = throwVelocity;

            _spriteRenderer.sortingLayerID = _startSortingLayer;
            _spriteRenderer.sortingOrder = _startSortingLayerIndex;

            StartCoroutine(AbleToInteract());
        }

        private IEnumerator AbleToInteract()
        {
            float time = _minTimeBetweenInteractaion;
            while(time > 0)
            {
                time -= Time.deltaTime;
                yield return null;
            }

            _canInteract = true;
        }
    }

}