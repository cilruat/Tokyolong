using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Festival
{

    public class UISayBox : MonoBehaviour
    {
        public static UISayManager sayManager;

        RectTransform tr;
        UITargetTracker targetTracker;

        public TalkBehaviour talker;
        public Text sayText;
        string viewSay;
        public Vector3 margin;

        void OnEnable()
        {
            tr.SetAsFirstSibling();
            sayText.text = "";
            talker = null;
        }

        void Awake()
        {
            tr = GetComponent<RectTransform>();
            targetTracker = GetComponent<UITargetTracker>();
        }

        public void SetSay(TalkBehaviour _talker, Talk _talk)
        {
            talker = _talker;
            viewSay = _talk.say;

            targetTracker.SetTarget(talker.transform, _talker.transform.position, margin);

            if (say != null)
            {
                StopCoroutine(say);
            }

            say = StartCoroutine(Say());
        }

        Coroutine say = null;

        IEnumerator Say()
        {
            for (int i = 0; i < viewSay.Length; ++i)
            {
                sayText.text += viewSay[i];
                yield return new WaitForSeconds(0.1f);
            }
            yield return new WaitForSeconds(1f);
            say = null;
            sayManager.AddList(this);
        }

    }
}