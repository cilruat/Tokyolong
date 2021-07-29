using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Festival
{

    public class UISayManager : MonoBehaviour
    {

        public GameObject sayBoxPrefab;
        Queue<UISayBox> objectPoolList = new Queue<UISayBox>();

        void Awake()
        {
            UISayBox.sayManager = this;
        }

        public void View(TalkBehaviour _talker, Talk _talk)
        {
            UISayBox sayBox = GetObject();
            sayBox.gameObject.SetActive(true);
            sayBox.SetSay(_talker, _talk);
        }

        public void AddList(UISayBox _sayBox)
        {
            objectPoolList.Enqueue(_sayBox);
            _sayBox.gameObject.SetActive(false);
        }

        public UISayBox GetObject()
        {
            if (objectPoolList.Count < 1)
            {
                return Instantiate(sayBoxPrefab, transform).GetComponent<UISayBox>();
            }
            else
            {
                return objectPoolList.Dequeue();
            }
        }

    }
}