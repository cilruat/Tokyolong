using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KickTheBuddy
{
    public class autoFireCD : MonoBehaviour
    {
        [HideInInspector]
        public float autoCD;
        [HideInInspector]
        public GameObject autoFirePoint;
        // Use this for initialization
        public void begin()
        {
            StartCoroutine(waitAutoCD());//Automatic attack time setting
        }
        IEnumerator waitAutoCD()
        {
            autoFirePoint.SetActive(false);
            yield return new WaitForSeconds(autoCD);
            autoFirePoint.SetActive(true);
            Destroy(this.gameObject);
        }
        // Update is called once per frame
        void Update()
        {

        }
    }
}