using UnityEngine;

namespace Touchdowners
{

    public class DesktopInput : IPlayerInput
    {
        [SerializeField] private KeyCode _moveLeftKey;
        [SerializeField] private KeyCode _moveRightKey;
        [SerializeField] private KeyCode _throwBallKey;

        public override bool MoveLeftPressed()
        {
            return Input.GetKey(_moveLeftKey);
        }

        public override bool MoveRightPressed()
        {
            return Input.GetKey(_moveRightKey);
        }

        public override bool ThrowBallPressed()
        {
            return Input.GetKeyDown(_throwBallKey);
        }
    }

}