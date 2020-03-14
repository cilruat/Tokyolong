using UnityEngine;
using System.Collections;

namespace Touchdowners
{

    [RequireComponent(typeof(FixedJoint2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class Ball : MonoBehaviour
    {
        private PlayerType _playerHolderType = PlayerType.None;

        // Used when ball attached to the hand or thrown.
        // After some of these action happens, 
        // the ball can not make actions (attach / hit helmet) for the period of time _minTimeBetweenInteractaion.
        public bool CanInteract { get; private set; }
        private float _minTimeBetweenInteractaion = 0.1f;

        // Essential components
        private SpriteRenderer _spriteRenderer;
        private Collider2D _collider2D;
        private Rigidbody2D _rb2D;
        private Joint2D _joint2D;

        // Start settings
        private Vector3 _startPosition;
        private Quaternion _startRotation;

        private int _startSortingLayer;
        private int _startSortingLayerIndex;

        private float _startMass;
        private float _startGravityScale;

        // Holder
        private PlayerHand _currentPlayerHand;

        public PlayerType PlayerHolderType { get { return _playerHolderType; } }

        private void Awake()
        {
            if (!gameObject.CompareTag(Tags.Ball))
                Debug.LogError("Ball: ball has to be tagged as Ball.");

            // Essential components initialization
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _collider2D = GetComponent<Collider2D>();
            _rb2D = GetComponent<Rigidbody2D>();
            _joint2D = GetComponent<Joint2D>();

            // Setup essential components
            _rb2D.isKinematic = false;
            _collider2D.isTrigger = false;
            _joint2D.enabled = false;

            SubscribeToGameEvents();
        }

        private void SubscribeToGameEvents()
        {
            if (GameObject.FindWithTag(Tags.GameManager) == null)
            {
                Debug.Log("Ball: No GameManager in the scene");
                return;
            }

            SaveStartSettings();

            GameManager gameManager = GameObject.FindWithTag(Tags.GameManager).GetComponent<GameManager>();

            gameManager.OnReturnToStartStateEvent += OnGameReturnedToStartState;
            gameManager.OnGameStartedEvent += OnGameStarted;

            gameManager.OnBallScoredEvent += (playerTypeThatScored) => StopAllCoroutines();
            gameManager.OnBallScoredEvent += (playerTypeThatScored) => CanInteract = false;
        }

        private void SaveStartSettings()
        {
            // Position
            _startPosition = transform.position;
            _startRotation = transform.rotation;

            // SpriteRenderer settings
            _startSortingLayer = _spriteRenderer.sortingLayerID;
            _startSortingLayerIndex = _spriteRenderer.sortingOrder;

            // Rigidbody settings
            _startMass = _rb2D.mass;
            _startGravityScale = _rb2D.gravityScale;
        }

        private void OnGameReturnedToStartState()
        {
            // Position
            transform.position = _startPosition;
            transform.rotation = _startRotation;

            DetachFromHand();

            // Velocity
            _rb2D.angularVelocity = 0;
            _rb2D.velocity = Vector2.zero;

            CanInteract = true;

            _rb2D.isKinematic = true;
        }

        private void OnGameStarted()
        {
            _rb2D.isKinematic = false;
        }

        public void AttachToHand(PlayerHand playerHand)
        {
            DetachFromHand();

            // Hand
            _currentPlayerHand = playerHand;
            _playerHolderType = playerHand.HandType;

            // Rigidbody
            _rb2D.velocity = Vector2.zero;
            _rb2D.angularVelocity = 0;

            _rb2D.mass = 0;
            _rb2D.gravityScale = 0;

            // Position
            transform.parent = playerHand.BallPosition;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;

            // Joint
            _joint2D.enabled = true;
            _joint2D.connectedBody = playerHand.GetComponent<Rigidbody2D>();

            // Sprite Renderer
            SpriteRenderer handSprite = playerHand.GetComponent<SpriteRenderer>();
            _spriteRenderer.sortingLayerID = handSprite.sortingLayerID;
            _spriteRenderer.sortingOrder = handSprite.sortingOrder + 1;

            // Can interact
            StartCoroutine(EnableInteraction());
        }

        public void DetachFromHand()
        {
            if (_currentPlayerHand == null)
                return;

            if (_currentPlayerHand != null)
            {
                // Hand
                _currentPlayerHand.DetachBall();
                _currentPlayerHand = null;
                _playerHolderType = PlayerType.None;

                // Transform parent
                transform.parent = null;

                // Joint
                _joint2D.enabled = false;

                // Rigidbody settings
                _rb2D.mass = _startMass;
                _rb2D.gravityScale = _startGravityScale;

                // Sprite Renderer
                _spriteRenderer.sortingLayerID = _startSortingLayer;
                _spriteRenderer.sortingOrder = _startSortingLayerIndex;
            }
        }

        public void Throw(Vector2 velocity)
        {
            DetachFromHand();

            _rb2D.velocity = velocity;

            StartCoroutine(EnableInteraction());
        }

        private IEnumerator EnableInteraction()
        {
            CanInteract = false;

            float time = _minTimeBetweenInteractaion;
            while (time > 0)
            {
                time -= Time.deltaTime;
                yield return null;
            }

            CanInteract = true;
        }
    }

}