using UnityEngine;

namespace Touchdowners
{

    public class InputAI : IPlayerInput
    {

        [SerializeField] private PlayerType _playerType;
        [SerializeField] private Transform _playerTransform;
        [SerializeField] private Transform _ballTransform;
        // How much time AI can hold the ball
        // When this time expires, AI throws the ball
        [SerializeField] private float _minTime = 3.5f;
        [SerializeField] private Transform _destination;

        private Ball _ball;
        private bool _isMovingLeft;

        private bool _isBallHold;
        private float _ballHoldTime;

        private bool _canThrow;

        #region MonoBehaviour

        private void Start()
        {
            _ball = _ballTransform.GetComponent<Ball>();
        }

        private void Update()
        {
            if(_ball.PlayerHolderType == _playerType)
            {
                if (_destination.position.x < _playerTransform.position.x)
                    _isMovingLeft = true;
                else
                    _isMovingLeft = false;

                if(!_isBallHold)
                    _isBallHold = true;

                if (_isBallHold)
                    _ballHoldTime += Time.deltaTime;

                if (_ballHoldTime > _minTime)
                {
                    _ballHoldTime = 0;
                    _canThrow = true;
                }

                _inPlace = false;
            }
            else
            {
                if (_isBallHold)
                    _isBallHold = false;

                if (_ballTransform.position.x < _playerTransform.position.x)
                    _isMovingLeft = true;
                else
                    _isMovingLeft = false;

                if (Mathf.Abs(_ballTransform.position.x - _playerTransform.position.x) < 0.2f)
                    _inPlace = true;
                else
                    _inPlace = false;
            }
        }

        #endregion

        // when distance between AI and the ball becomes small, AI stops moving
        private bool _inPlace;

        public override bool MoveLeftPressed()
        {
            return _isMovingLeft && !_inPlace;
        }

        public override bool MoveRightPressed()
        {
            return !_isMovingLeft && !_inPlace;
        }

        public override bool ThrowBallPressed()
        {
            if (_canThrow)
            {
                _canThrow = false;
                return true;
            }

            return false;
        }
    }

}