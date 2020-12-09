using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace KickTheBuddy
{
    public class bullet_light : MonoBehaviour
    {
        public float scaleSpeed;
        private bool scaleOverDO;
        [HideInInspector]
        public int scoreAtk = 10;
        public GameObject fxHit;
        private bool hitDO;
        // Use this for initialization
        void Start()
        {
            Destroy(this.gameObject, 0.5f);
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (scaleOverDO)
                return;
            transform.localScale += new Vector3(0, 0, scaleSpeed);
        }
        void OnTriggerEnter(Collider other)
        {
            if (hitDO)
                return;
            if (other.tag == "body")
            {//Hit judgement of laser weapon
                GameObject.FindGameObjectWithTag("gameManage").GetComponent<gameManage>().coinAdd(scoreAtk);
                scaleOverDO = true;
                this.GetComponent<Collider>().enabled = false;

                other.GetComponent<Rigidbody>().AddRelativeForce(Vector3.back * 100);
                hitDO = true;
            }

            Instantiate(fxHit, this.transform.Find("hitPoint").position, Quaternion.identity);
            Destroy(this.gameObject, 0.05f);
        }

    }
}