using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Festival
{

    public class UIInGame : MonoBehaviour
    {

        public static UIInGame instance;

        public UISayManager sayManager;


        void Awake()
        {
            instance = this;
        }


        public void ViewSay(TalkBehaviour _talker, Talk _talk)
        {
            sayManager.View(_talker, _talk);
        }

    }
}
