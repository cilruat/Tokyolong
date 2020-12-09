using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KickTheBuddy
{
    public class bullet_fire : MonoBehaviour
    {

        [HideInInspector]
        public int scoreAtk = 10;
        private bool hitDO;
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        void OnTriggerEnter(Collider other)
        {
            if (hitDO)
                return;
            if (other.tag == "body")
            { //Fire weapon hit judgement
                GameObject.FindGameObjectWithTag("gameManage").GetComponent<gameManage>().coinAdd(scoreAtk);

                this.GetComponent<Collider>().enabled = false;

                other.GetComponent<Rigidbody>().AddRelativeForce(Vector3.back * 100);
                hitDO = true;
            }


            Destroy(this.gameObject, 2f);
        }
    }
}
