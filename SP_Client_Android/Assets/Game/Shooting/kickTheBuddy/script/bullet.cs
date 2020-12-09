using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KickTheBuddy
{
    public class bullet : MonoBehaviour
    {

        [HideInInspector]
        public float forceCount;
        [HideInInspector]
        public int scoreAtk = 10;
        public GameObject fxHit;
        private bool hitDO;
        // Use this for initialization
        void Start()
        {
            Destroy(this.gameObject, 5);
            this.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * forceCount);
        }

        // Update is called once per frame
        void Update()
        {

        }
        void OnCollisionEnter(Collision collisionInfo)
        {
            if (hitDO)
                return;
            if (collisionInfo.transform.tag == "body")
            {//Bullet hit judgement
                GameObject.FindGameObjectWithTag("gameManage").GetComponent<gameManage>().coinAdd(scoreAtk);

                this.GetComponent<Collider>().enabled = false;
                this.GetComponent<Rigidbody>().useGravity = true;
                Instantiate(fxHit, transform.position, Quaternion.identity);
                Destroy(this.gameObject);
                hitDO = true;
            }

        }
    }
}