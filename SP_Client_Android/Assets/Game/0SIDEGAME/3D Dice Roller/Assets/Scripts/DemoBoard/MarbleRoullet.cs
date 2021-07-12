using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class MarbleRoullet : MonoBehaviour {

    public RectTransform wheel;
    List<Image> contents;
    public Button rollingButton;
    bool rolling = false;
    public float initSpeed = 0f;
    public float breakSpeed = 0f;
    public float keepSpeedTimeMin, keepSpeedTimeMax;
    float currentTime = 0f;
    float currentSpeed = 0f;





    void Start()
    {
        rollingButton.interactable = rolling == false;
        contents = new List<Image>();

        for (int i = 0; i < wheel.childCount; i++)
        {
            contents.Add(wheel.GetChild(i).GetComponent<Image>());

        }
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
                float halfAng = 360 / contents.Count;
                float minAng = 360;
                Image targetImg = null;

                for (int i = 0; i < contents.Count; i++)
                {
                    Vector3 localDir = Quaternion.Euler(0, 0, halfAng + (i * 360 / contents.Count)) * Vector3.up;

                    float ang = Vector3.Angle(wheel.TransformDirection(localDir), Vector3.up);

                    if (ang <= minAng)
                    {
                        minAng = ang;
                        targetImg = contents[i];
                    }
                }
                rolling = false;
                rollingButton.interactable = true;
            }


        }
    }

    public void Roll()
    {
        rollingButton.interactable = false;
        rolling = true;
        currentSpeed = initSpeed;
        currentTime = UnityEngine.Random.Range(keepSpeedTimeMin, keepSpeedTimeMax);
    }
}
