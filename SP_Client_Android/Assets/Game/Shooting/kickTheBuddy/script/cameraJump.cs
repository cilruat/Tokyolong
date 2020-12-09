using UnityEngine;
using System.Collections;
namespace KickTheBuddy
{
    public class cameraJump : MonoBehaviour
    {
        public float jumpCD = 0.01f;
        private bool jumpDO = true;
        public float jumpLength = 0.5f;
        private bool cameraJumpDO = false;
        private Vector3 positionTemp;
        public float strength = 0.1f;//Amplitude of vibration 
                                     // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (cameraJumpDO == true)
            { //Camera vibration
                if (jumpDO == true)
                {
                    transform.localPosition = new Vector3(positionTemp.x + Random.Range(-strength, strength), positionTemp.y + Random.Range(-strength, strength), positionTemp.z);

                    jumpDO = false;
                    StartCoroutine(waitJumpCD());
                }
            }
        }

        IEnumerator waitJumpCD()
        {
            yield return new WaitForSeconds(jumpCD);
            jumpDO = true;
        }
        public void camera_jump()
        {
            cameraJumpDO = true;
            positionTemp = transform.localPosition;
            StartCoroutine(waitJumpLength());
        }

        IEnumerator waitJumpLength()
        {
            yield return new WaitForSeconds(jumpLength);
            cameraJumpDO = false;
            transform.localPosition = positionTemp;
        }
    }
}