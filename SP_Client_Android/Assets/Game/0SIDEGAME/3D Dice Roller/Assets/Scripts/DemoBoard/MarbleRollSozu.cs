using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class MarbleRollSozu : MonoBehaviour {



    public RectTransform wheel;
    public Button rollingButton;
    bool rolling = false;

    public float initSpeed = 0f;
    public float breakSpeed = 0f;
    public float keepSpeedTimeMin, keepSpeedTimeMax;
    float currentTime = 0f;
    public float currentSpeed;


    void Start()
    {
        rollingButton.interactable = rolling == false;

    }

    void Update()
    {

        if (rolling)
        {
            currentTime -= Time.deltaTime;


            if (currentTime <= 0)
            {
                currentSpeed -= breakSpeed * Time.deltaTime;

            }

            wheel.Rotate(0, 0, -currentSpeed * Time.deltaTime);

            if (currentSpeed <= 0) //if Stopped
            {
                rolling = false;
                rollingButton.interactable = true;
            }


        }
    }



    public void Roll()
    {
        currentSpeed = initSpeed;
        rolling = true;
        currentTime = UnityEngine.Random.Range(keepSpeedTimeMin, keepSpeedTimeMax);


    }

}
