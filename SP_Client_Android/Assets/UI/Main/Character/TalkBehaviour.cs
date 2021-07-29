using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Festival
{

    /// <summary>
    /// 대화를 하는 컴포넌트 
    /// </summary>
    public class TalkBehaviour : MonoBehaviour
    {
        /// <summary>
        /// 일반적인 대화
        /// </summary>
        public List<Talk> talkList;
        /// 대화 딜레이 시간 
        /// </summary>
        public float talkDelayTime = 0f;
        /// <summary>
        /// 대화 기본 딜레이 시간 
        /// </summary>
        public float talkDelayTimeBase = 10f;
        /// <summary>
        /// 대화 딜레이 시간 랜덤 범위 
        /// </summary>
        public float talkDelayTimeRange = 10f;

        void Start()
        {
            talkDelayTime = talkDelayTimeBase + Random.Range(0, talkDelayTimeRange);
        }

        /// <summary>
        /// 랜덤하게 대화합니다.
        /// </summary>
        public void Talk()
        {
            StartCoroutine(TalkCoroutine());
        }


        IEnumerator TalkCoroutine()
        {
            UIInGame.instance.ViewSay(this, talkList[Random.Range(0, talkList.Count)]);
            yield return new WaitForSeconds(talkDelayTime);
        }
    }
}