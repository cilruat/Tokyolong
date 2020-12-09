using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace KickTheBuddy
{
    public class knife : MonoBehaviour
    {
        public float forceCount;
        public int scoreAtk = 10;
        private GameObject[] playerPoint;
        private int playerPointNum;
        private bool mouseUpDO;
        public GameObject fxHit;
        private bool hitDO;
        // Use this for initialization
        void Start()
        {
            playerPoint = GameObject.FindGameObjectsWithTag("playerPoint");
            playerPointNum = Random.Range(0, playerPoint.Length);
            //Debug.Log (playerPoint.Length );

        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (mouseUpDO)
                return;

            transform.LookAt(playerPoint[playerPointNum].transform.position);
        }
        public void mouseUp()
        {//When the fingers are loose, the flying knife will shoot out.
            this.GetComponent<Collider>().enabled = true;
            Rigidbody rig = gameObject.GetComponent<Rigidbody>();
            rig.isKinematic = false;
            rig.AddRelativeForce(Vector3.forward * forceCount);

            mouseUpDO = true;
        }

        void OnCollisionEnter(Collision collisionInfo)
        { //Flying cutter hit judgement
            if (hitDO)
                return;
            if (collisionInfo.transform.tag == "body")
            {
                this.GetComponent<Collider>().enabled = false;
                Destroy(this.GetComponent<Rigidbody>());
                //Debug.Log (collisionInfo.gameObject.name );

                GameObject.FindGameObjectWithTag("gameManage").GetComponent<gameManage>().coinAdd(scoreAtk);
                this.transform.SetParent(collisionInfo.transform);
                hitDO = true;
            }
            ContactPoint contact = collisionInfo.contacts[0];
            //Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
            Vector3 pos = contact.point;
            Instantiate(fxHit, pos, Quaternion.identity);
            //Debug.Log ("hit");
            Destroy(this.gameObject, 5);
        }

    }
}