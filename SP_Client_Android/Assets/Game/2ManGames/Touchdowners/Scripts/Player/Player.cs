using UnityEngine;
using System.Collections;

namespace Touchdowners
{

    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CircleCollider2D))]
    public class Player : MonoBehaviour
    {
        [Header("- Input -")]
        [SerializeField] private IPlayerInput _input;

        [Header("- Moving Right -")]
        [SerializeField] private float _jumpVelocityRight;
        [SerializeField] private float _velocityRight;

        [Header("- Moving Left -")]
        [SerializeField] private float _jumpVelocityLeft;
        [SerializeField] private float _velocityLeft;

        // how much time does _jumpVelocityRight/Left will be applied
        private float _jumpingForceTime = 0.1f;

        private bool _canMove = true;
        private bool _isGrounded = true;

        private Rigidbody2D _rb2D;

        public IPlayerInput Input { get { return _input; } }

        #region MonoBehaviour

        private void OnValidate()
        {
            if (_jumpVelocityLeft < 0)
                _jumpVelocityLeft = 0;
            if (_jumpVelocityRight < 0)
                _jumpVelocityRight = 0;
        }

        private void Awake()
        {
            _rb2D = GetComponent<Rigidbody2D>();

            SubscribeToGameManagerEvents();
        }

        private void FixedUpdate()
        {
            if (_canMove)
            {
                Move();

                if (_isGrounded)
                    FindBalance();
                else
                {
                    _jumpingAirTime += Time.deltaTime;
                    _rb2D.angularVelocity = Mathf.Clamp(_rb2D.angularVelocity, -100, 100);
                }
            }
        }
        
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag(Tags.GroundTag))
                AnulateGrounding();
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (!_isGrounded && collision.gameObject.CompareTag(Tags.GroundTag))
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
            gameManager.OnReturnToStartStateEvent += () => _canMove = false;
            gameManager.OnGameStartedEvent += () => _canMove = true;
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
            if (_input.MoveLeftPressed())
            {
                Jump(_jumpVelocityLeft);
                SetVelocityX(_velocityLeft);
            }
            else if (_input.MoveRightPressed())
            {
                Jump(_jumpVelocityRight);
                SetVelocityX(_velocityRight);
            }
            else
            {
                SetVelocityX(0);
            }
        }

        private float _jumpingAirTime;
        public void Jump(float velocityY)
        {
            if (_jumpingAirTime > _jumpingForceTime)
                return;

            _isGrounded = false;
            _rb2D.AddForce(velocityY * Vector2.up);

            if (Mathf.Abs(_rb2D.angularVelocity) < 10)
                _rb2D.angularVelocity = Random.Range(-200, 200);
        }

        private void SetVelocityX(float velocityX)
        {
            _rb2D.velocity = new Vector2(Mathf.Lerp(_rb2D.velocity.x, velocityX, Time.deltaTime * 4f), _rb2D.velocity.y);
        }

        private void AnulateGrounding()
        {
            _isGrounded = true;
            _jumpingAirTime = 0;

            _wasInBalance = true;
        }

        private IEnumerator MoveToBalance()
        {
            while(_isGrounded)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, 10f * Time.deltaTime);
                yield return null;
            }
        }

        private Quaternion rightQuaternion = Quaternion.Euler(0, 0, -90);
        private Quaternion _desiredRotation;

        private float _minAngleForBalance = 5;
        private bool _isMovingRight;
        private bool _wasInBalance = true;

        private void FindBalance()
        {
            float angleBetweenPlayerNormal = Quaternion.Angle(transform.rotation, Quaternion.identity);
            float angleBetweenDesiredPositionNormal = Quaternion.Angle(_desiredRotation, Quaternion.identity);

            if (angleBetweenPlayerNormal < _minAngleForBalance)
            {
                if(angleBetweenDesiredPositionNormal < _minAngleForBalance && !_wasInBalance)
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

            if(!_wasInBalance)
            {
                // change moving direction
                float angleBetweenRotationDesiredRotation = Quaternion.Angle(transform.rotation, _desiredRotation);
                if(angleBetweenRotationDesiredRotation < _minAngleForBalance)
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