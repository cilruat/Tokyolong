using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KickTheBuddy
{
    public class atkTankGun : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
            if (GameObject.FindGameObjectWithTag("atkTank") == null)
            { //When there are no tanks in the scene, new tanks can be created.
                Transform tankPoint = GameObject.FindGameObjectWithTag("tankPoint").transform;
                Instantiate(Resources.Load("atkTank") as GameObject, tankPoint.position, tankPoint.rotation);
            }

        }

        public void mouseUp()
        {

            Destroy(this.gameObject);
        }
    }
}