namespace GameBench
{
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    public class SpinTurnTimer : MonoBehaviour
    {
        public Text timerText;
        private static TimeSpan nextFreeTurn = new TimeSpan(0, 30, 0);
        TimeSpan remainingTime;
        public const string TIMER_KEY = "TIMER_KEY";
        private DateTime timeStamp;
        private void OnEnable()
        {
            DateTime.TryParse(PlayerPrefs.GetString(TIMER_KEY, DateTime.Now.ToString()), out timeStamp);
        }
        void Update()
        {
            TimeSpan t = DateTime.Now - timeStamp;
            try
            {
                remainingTime = nextFreeTurn - t;
                timerText.text = string.Format("Next Free Spin: {0:D1}:{1:D2}:{2:D2}",
                    remainingTime.Hours, remainingTime.Minutes, remainingTime.Seconds);
                if (remainingTime.TotalMinutes <= 0)
                {
                    ActivateFreeSpin();
                }
            }
            catch (Exception e)
            {
                ActivateFreeSpin();
                print(e.StackTrace);
            }
        }

        public void ActivateFreeSpin()
        {
            timeStamp = DateTime.Now;
            UIManager.Instance.ActivateFreeSpin();
            gameObject.SetActive(false);
            Debug.Log("ActivateFreeSpin");
        }
        private void OnApplicationPause(bool pause)
        {
            if (pause)
            {
                PlayerPrefs.SetString(TIMER_KEY, timeStamp.ToString());
                PlayerPrefs.Save();
            }
        }
    }
}