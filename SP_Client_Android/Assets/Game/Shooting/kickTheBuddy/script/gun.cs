using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace KickTheBuddy
{
    public class gun : MonoBehaviour
    {
        public float atkCD;
        public GameObject bullet;
        public Transform firePoint;
        public float forceCount;
        public float piancha;
        public int scoreAtk = 10;
        public GameObject fxFire;

        private float timeTemp;
        private bool overDO;
        private GameObject[] playerPoint;
        private int playerPointNum;
        // Use this for initialization
        void Start()
        {
            playerPoint = GameObject.FindGameObjectsWithTag("playerPoint");

            playerPointNum = Random.Range(0, playerPoint.Length);

        }

        // Update is called once per frame
        void FixedUpdate()
        {

            if (overDO)
                return;

            //Weapon orientation and firing
            transform.LookAt(playerPoint[playerPointNum].transform.position);
            if (Time.time > timeTemp)
            {

                Instantiate(fxFire, firePoint.position, firePoint.rotation);
                GameObject bulletObj = Instantiate(bullet, firePoint.position, firePoint.rotation) as GameObject;
                bulletObj.transform.localEulerAngles += new Vector3(Random.Range(-piancha, piancha), Random.Range(-piancha, piancha), Random.Range(-piancha, piancha));
                bullet b = bulletObj.GetComponent<bullet>();
                if (b)
                {
                    b.forceCount = forceCount;
                    b.scoreAtk = scoreAtk;
                }
                else
                {
                    bullet_light bL = bulletObj.GetComponent<bullet_light>();
                    if (bL)
                    {
                        bL.scoreAtk = scoreAtk;
                    }
                    else
                    {
                        bullet_fire bF = bulletObj.GetComponent<bullet_fire>();
                        if (bF)
                        {
                            bF.scoreAtk = scoreAtk;
                        }
                    }
                }
                timeTemp = Time.time + atkCD;
            }

        }
        public void mouseUp()
        {
            this.GetComponent<Rigidbody>().isKinematic = false;
            overDO = true;
            Destroy(this.gameObject, 5);
        }

    }
}