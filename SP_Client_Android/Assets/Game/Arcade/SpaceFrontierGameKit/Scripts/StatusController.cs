using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RocketSpace
{

    public class StatusController : MonoBehaviour
    {

        /// <summary>
        /// We need to show a message after each successful body detach. This status controller receives the ID
        /// and displays a message accordingly.
        /// </summary>

        internal int statusID;
        private string statusText;
        public GameObject myLabel;

        void Start()
        {

            switch (statusID)
            {
                case 0:
                    statusText = "분발해";
                    break;
                case 1:
                    statusText = "보통이야";
                    break;
                case 2:
                    statusText = "좋았어";
                    break;
                case 3:
                    statusText = "훌륭해!";
                    break;
                case 4:
                    statusText = "개쩔어!!";
                    break;
            }

            myLabel.GetComponent<TextMesh>().text = statusText;

            Destroy(gameObject, 1.5f);
        }

    }
}