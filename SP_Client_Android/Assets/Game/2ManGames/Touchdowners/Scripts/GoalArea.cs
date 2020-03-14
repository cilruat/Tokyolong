using UnityEngine;

namespace Touchdowners
{

    public class GoalArea : MonoBehaviour
    {
        // PlayerType has to be equal to PlayerType that will get a score.
        public PlayerType playersWinArea;

        public delegate void OnGoal(PlayerType playersType);
        public event OnGoal OnGoalEvent;

        protected void NotifyAboutGoal()
        {
            if (OnGoalEvent != null)
                OnGoalEvent(playersWinArea);
        }
    }

}