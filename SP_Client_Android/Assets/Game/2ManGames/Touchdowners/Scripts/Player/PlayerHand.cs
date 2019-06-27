﻿using UnityEngine;

namespace Touchdowners
{

    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(DistanceJoint2D))]
    public class PlayerHand : MonoBehaviour
    {
        [Header("- Player type -")]
        [SerializeField] private PlayerType _playerType;

        [Header("- Ball settings -")]
        [SerializeField] private Transform _ballPosition;
        [SerializeField] private Vector2 _throwSpeed;

        private float _handRotationSpeed = 700f;

        private Rigidbody2D _rb2D;

        private IPlayerInput _input;
        private bool _containsBall;
        private Ball _ball;

        public PlayerType HandType { get { return _playerType; } }
        public Transform BallPosition { get { return _ballPosition; } }

        #region MonoBehaviour

        private void Awake()
        {
            _rb2D = GetComponent<Rigidbody2D>();
            GetComponent<Collider2D>().isTrigger = true;

            _rb2D.mass = 0;
            _rb2D.gravityScale = 0;

            _input = transform.root.GetComponent<Player>().Input;
        }

        private void Update()
        {
            if (_input.ThrowBallPressed() && _containsBall)
                ThrowBall();

            if (_input.MoveLeftPressed())
                SetHandAngularVelocity(_handRotationSpeed, 10);
            else if (_input.MoveRightPressed())
                SetHandAngularVelocity(-_handRotationSpeed, 10);
            else
                SetHandAngularVelocity(0, 0.2f);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(Tags.Ball))
            {
                Ball triggeredBall = collision.GetComponent<Ball>();

                if (triggeredBall.CanInteract && _playerType != triggeredBall.PlayerHolderType)
                    AttachBall(ref triggeredBall);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag(Tags.Ball))
                _containsBall = false;
        }

        #endregion

        private void SetHandAngularVelocity(float velocity, float lerpSpeed)
        {
            _rb2D.angularVelocity = Mathf.Lerp(_rb2D.angularVelocity, velocity, Time.deltaTime * lerpSpeed);
        }

        private void AttachBall(ref Ball ball)
        {
            ball.AttachToHand(this);
            _ball = ball;
            _containsBall = true;
        }

        private void ThrowBall()
        {
            _ball.Throw(_throwSpeed);
            _ball = null;
            _containsBall = false;
        }

    }

}