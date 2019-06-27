using UnityEngine;

namespace Touchdowners
{

    public abstract class IPlayerInput: MonoBehaviour
    {
        public abstract bool MoveLeftPressed();
        public abstract bool MoveRightPressed();
        public abstract bool ThrowBallPressed();
    }

}