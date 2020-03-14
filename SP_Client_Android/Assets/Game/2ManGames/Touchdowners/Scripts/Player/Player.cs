using UnityEngine;
using System.Collections;

namespace Touchdowners
{

    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CircleCollider2D))]
    public class Player : MonoBehaviour
    {
        [Header("- Input -")]
        public IPlayerInput input;

        [Header("- Moving Right -")]
        public PlayerData playerData;

        private float _minAngleForBalanceInGround = 5;
        private float _currentMinAngleForBalance;

        private float _jumpingForceTime = 0.1f;

        private bool _isGrounded = true;

        private Rigidbody2D _rb2D;

        public bool AbleToMove { get; set; }
        public bool AbleToJump { get; set; }

        #region MonoBehaviour

        private void Awake()
        {
            _rb2D = GetComponent<Rigidbody2D>();

            SubscribeToGameManagerEvents();

            _currentMinAngleForBalance = _minAngleForBalanceInGround;

            AbleToMove = true;
            AbleToJump = true;
        }

        private void FixedUpdate()
        {
            FindBalance();

            Move();

            if (!_isGrounded)
            {
                _jumpingAirTime += Time.deltaTime;
                _rb2D.angularVelocity = Mathf.Clamp(_rb2D.angularVelocity, -100, 100);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag(Tags.Ground))
                AnulateGrounding();
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (!_isGrounded && collision.gameObject.CompareTag(Tags.Ground))
                AnulateGrounding();
        }

        #endregion

        private Vector3 _startPosition;
        private Quaternion _startRotation;

        private void SubscribeToGameManagerEvents()
        {
            if (GameObject.FindWithTag(Tags.GameManager) == null)
            {
                Debug.Log("Player: no GameManager found in the scene.");
                return;
            }

            _startPosition = transform.position;
            _startRotation = transform.rotation;

            GameManager gameManager = GameObject.FindWithTag(Tags.GameManager).GetComponent<GameManager>();
            gameManager.OnReturnToStartStateEvent += ReturnToStartState;
            gameManager.OnReturnToStartStateEvent += () => AbleToMove = false;
            gameManager.OnReturnToStartStateEvent += () => AbleToJump = false;
            gameManager.OnGameStartedEvent += () => AbleToMove = true;
            gameManager.OnGameStartedEvent += () => AbleToJump = true;
        }

        private void ReturnToStartState()
        {
            transform.position = _startPosition;
            transform.rotation = _startRotation;

            _rb2D.angularVelocity = 0;
            _rb2D.velocity = Vector2.zero;

            _isGrounded = true;

            StartCoroutine(MoveToBalance());
        }

        private void Move()
        {
            if (input.MoveLeftPressed())
            {
                if (AbleToJump)
                    Jump(playerData.velocityLeft.y);
                if (AbleToMove)
                    SetVelocityX(playerData.velocityLeft.x);
            }
            else if (input.MoveRightPressed())
            {
                if (AbleToJump)
                    Jump(playerData.velocityRight.y);
                if (AbleToMove)
                    SetVelocityX(playerData.velocityRight.x);
            }
            else
            {
                SetVelocityX(0);
            }
        }

        private float _jumpingAirTime;
        private void Jump(float velocityY)
        {
            if (_jumpingAirTime > _jumpingForceTime)
                return;

            _currentMinAngleForBalance = playerData.minAngleForBalanceInJump;

            _isGrounded = false;
            _rb2D.velocity = new Vector2(_rb2D.velocity.x, velocityY);
        }

        private void SetVelocityX(float velocityX)
        {
            _rb2D.velocity = new Vector2(Mathf.Lerp(_rb2D.velocity.x, velocityX, Time.deltaTime * 4f), _rb2D.velocity.y);
        }

        private void AnulateGrounding()
        {
            _isGrounded = true;
            _jumpingAirTime = 0;

            _currentMinAngleForBalance = _minAngleForBalanceInGround;

            _wasInBalance = true;
        }

        private IEnumerator MoveToBalance()
        {
            while (_isGrounded)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, 10f * Time.deltaTime);
                yield return null;
            }
        }

        private Quaternion rightQuaternion = Quaternion.Euler(0, 0, -90);
        private Quaternion _desiredRotation;

        private bool _isMovingRight;
        private bool _wasInBalance = true;

        private void FindBalance()
        {
            float angleBetweenPlayerNormal = Quaternion.Angle(transform.rotation, Quaternion.identity);
            float angleBetweenDesiredPositionNormal = Quaternion.Angle(_desiredRotation, Quaternion.identity);

            if (angleBetweenPlayerNormal < _currentMinAngleForBalance)
            {
                if (angleBetweenDesiredPositionNormal < _currentMinAngleForBalance && !_wasInBalance)
                {
                    _rb2D.angularVelocity = 0;
                    _wasInBalance = true;
                    StartCoroutine(MoveToBalance());
                }
            }
            else
            {
                if (_wasInBalance)
                {
                    _wasInBalance = false;

                    _desiredRotation = Quaternion.Inverse(transform.rotation);

                    if (Quaternion.Angle(transform.rotation, rightQuaternion) < 90)
                        _isMovingRight = false;
                    else
                        _isMovingRight = true;
                }
            }

            if (!_wasInBalance)
            {
                // change moving direction
                float angleBetweenRotationDesiredRotation = Quaternion.Angle(transform.rotation, _desiredRotation);
                if (angleBetweenRotationDesiredRotation < _currentMinAngleForBalance)
                {
                    _desiredRotation = Quaternion.Inverse(transform.rotation);
                    _isMovingRight = !_isMovingRight;
                }

                if (_isMovingRight)
                    LerpAngularVelocity(-_angularVelocityToBalance);
                else
                    LerpAngularVelocity(_angularVelocityToBalance);
            }
        }
        private float _angularVelocityToBalance = 300;

        private float _currentVelocity;
        private void LerpAngularVelocity(float desiredVelocity)
        {
            _currentVelocity = Mathf.Lerp(_currentVelocity, desiredVelocity, Time.deltaTime * 10f);
            _rb2D.angularVelocity = _currentVelocity;
        }

    }

}