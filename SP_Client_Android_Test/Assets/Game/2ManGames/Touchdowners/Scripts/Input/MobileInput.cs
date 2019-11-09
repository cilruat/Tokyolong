namespace Touchdowners
{

    public class MobileInput : IPlayerInput
    {

        private bool _isMovingLeft;
        private bool _isMovingRight;
        private bool _isThrowBallPressed;

        public void MovingLeftTrue() { _isMovingLeft = true; }
        public void MovingLeftFalse() { _isMovingLeft = false; }

        public void MovingRightTrue() { _isMovingRight = true; }
        public void MovingRightFalse() { _isMovingRight = false; }

        public void ThrowBallTrue() { _isThrowBallPressed = true; }
        public void ThrowBallFalse() { _isThrowBallPressed = false; }

        public override bool MoveLeftPressed()
        {
            return _isMovingLeft;
        }

        public override bool MoveRightPressed()
        {
            return _isMovingRight;
        }

        public override bool ThrowBallPressed()
        {
            return _isThrowBallPressed;
        }
    }

}