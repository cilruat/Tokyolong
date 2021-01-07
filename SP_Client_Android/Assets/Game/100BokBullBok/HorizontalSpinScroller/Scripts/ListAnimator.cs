namespace GameBench
{
    using System.Collections;
    using UnityEngine;

    public class ListAnimator : MonoBehaviour
    {
        public AnimationCurve[] animCurves;
        AnimationCurve animCurve;
        internal float tileSizeX;
        private Vector3 startPosition;
        //public Rigidbody2D rb;

        void Start()
        {
            startPosition = transform.position;//rb.position;
        }

        public void StartSpin()
        {
            animCurve = animCurves[Random.Range(0, animCurves.Length)];
            float t = 10;
            float val = (Config.Instance.speed * tileSizeX) +
                        (Random.Range(0, Config.Instance.rewarItem.Length) * (SpinScroller.Instance.unitXSize + 3));
            StartCoroutine(SpinNow(val, t));
        }

        float newPosition;

        IEnumerator SpinNow(float val, float animDur = 3f, float delayBeforeAnim = 2)
        {
            float timer = 0f;
            while (timer < animDur)
            {
                newPosition = Mathf.Repeat(val * animCurve.Evaluate(timer / animDur), tileSizeX);
                transform.position = startPosition + Vector3.left * newPosition;
                //				rb.MovePosition (startPosition + Vector3.left * newPosition);
                timer += Time.deltaTime;
                yield return 0;
            }
            SpinScroller.Instance.RewardPlayer();
        }
    }
}