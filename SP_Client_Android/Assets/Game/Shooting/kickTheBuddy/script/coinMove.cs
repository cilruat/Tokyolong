using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KickTheBuddy
{

    public class coinMove : MonoBehaviour
    {
        private Transform coinObj;
        private Transform coinPoint;
        // Use this for initialization
        void Start()
        {
            coinPoint = GameObject.FindGameObjectWithTag("coinPoint").transform;
            transform.eulerAngles += new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
            coinObj = this.transform.Find("coin");
            Destroy(this.gameObject, 1.5f);
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            //Gold flying animation
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(coinPoint.position - transform.position), 7 * Time.deltaTime);//缓慢转向目标
            transform.Translate(Vector3.forward * 5 * Time.deltaTime);
            coinObj.transform.Rotate(Vector3.up * 1500 * Time.deltaTime);
        }

    }
}