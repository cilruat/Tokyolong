using UnityEngine;

namespace Touchdowners
{

    [RequireComponent(typeof(Collider2D))]
    public class TouchdownArea : GoalArea
    {

        private void Awake()
        {
            GetComponent<Collider2D>().isTrigger = true;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(Tags.Ball))
            {
                Ball ball = other.GetComponent<Ball>();
                if (ball.PlayerHolderType == playersWinArea)
                {
                    NotifyAboutGoal();
                }
            }
        }

    }

}