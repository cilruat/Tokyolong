using UnityEngine;

namespace Touchdowners
{

    [RequireComponent(typeof(Collider2D))]
    public class TouchdownArea : MonoBehaviour
    {
        // Players area has to be equal to Player type that can touchdown on this area.
        [SerializeField] private PlayerType _playersArea;

        public delegate void OnTouchdown(PlayerType playersType);
        public event OnTouchdown OnTouchdownEvent;

        private void Awake()
        {
            GetComponent<Collider2D>().isTrigger = true;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(other.CompareTag(Tags.Ball))
            {
                Ball ball = other.GetComponent<Ball>();
                if(ball.PlayerHolderType == _playersArea && OnTouchdownEvent != null)
                        OnTouchdownEvent(_playersArea);
            }
        }

    }

}