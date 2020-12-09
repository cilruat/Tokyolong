using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace KickTheBuddy
{
    public class control : MonoBehaviour
    {

        //public Transform player;
        [HideInInspector]
        public GameObject[] atkObj;
        [HideInInspector]
        public int objNum;
        private Transform atkObjTemp;

        private Camera cameraObj;
        // Use this for initialization
        void Start()
        {
            cameraObj = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {//Execute when pressing the screen.

                //Record the location of the time pressed.
                Ray rayDown = cameraObj.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitDown;
                if (Physics.Raycast(rayDown, out hitDown))
                {//Execution of radiographic impact		
                    if (hitDown.transform.tag == "groud")
                    {
                        if (atkObjTemp)
                            atkObjTemp.gameObject.SendMessage("mouseUp", gameObject, SendMessageOptions.DontRequireReceiver);
                        atkObjTemp = Instantiate(atkObj[objNum], hitDown.point, Quaternion.identity).transform;


                    }
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {//Execute when pressing the screen.


                Ray rayUp = cameraObj.ScreenPointToRay(Input.mousePosition);//Definition of rays
                RaycastHit hitUp;
                if (Physics.Raycast(rayUp, out hitUp))
                {//Execution of radiographic impact			
                    if (hitUp.transform.tag == "groud")
                    {

                        if (atkObjTemp)
                        {
                            atkObjTemp.gameObject.SendMessage("mouseUp", gameObject, SendMessageOptions.DontRequireReceiver);
                            atkObjTemp = null;
                        }
                    }
                }
            }
            else if (Input.GetMouseButton(0))
            {


                Ray ray = cameraObj.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.tag == "groud")
                    {

                        if (atkObjTemp)
                        {
                            atkObjTemp.position = hit.point;

                        }
                    }
                }
            }

        }
    }
}