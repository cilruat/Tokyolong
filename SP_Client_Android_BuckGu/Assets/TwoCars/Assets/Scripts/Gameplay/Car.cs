//  /*********************************************************************************
//   *********************************************************************************
//   *********************************************************************************
//   * Produced by Skard Games										                 *
//   * Facebook: https://goo.gl/5YSrKw											     *
//   * Contact me: https://goo.gl/y5awt4								             *
//   * Developed by Cavit Baturalp Gürdin: https://tr.linkedin.com/in/baturalpgurdin *
//   *********************************************************************************
//   *********************************************************************************
//   *********************************************************************************/

using UnityEngine;
using DG.Tweening;

public class Car : MonoBehaviour {

    public float leftLane; // My Left Lane
    public float rightLane; // My Right Lane

    public float rotationInterval; //passed time between start and end of rotation of car
    public float moveInterval; //passed time between start and end of position of car

    public float rotationDegree=25f; 

    bool rotateRight = false;
    bool rotateLeft = false;

    float lrSign; // Rotates cars according to their screen position l&r

    void Awake()
    {
        lrSign = -Mathf.Sign(transform.position.x);
    }


    /// <summary>
    /// Changes lane in left road or right road
    /// </summary>
    public void ChangeLane()
    {        
        if(transform.position.x == leftLane)
        {
            transform.DOMoveX(rightLane, moveInterval).OnStart(RotateCarRight).OnComplete(RotateCarRight);
        }
        else if (transform.position.x == rightLane)
        {
            transform.DOMoveX(leftLane, moveInterval).OnStart(RotateCarLeft).OnComplete(RotateCarLeft);
        }
    }


    /// <summary>
    /// Rotates car to left lane
    /// </summary>
    void RotateCarLeft()
    {
        if (rotateLeft)
        {
            transform.DORotate(new Vector3(0, 0, 0), rotationInterval);
            rotateLeft = false;
        }
        else
        {
            transform.DORotate(new Vector3(0, 0, rotationDegree * lrSign), rotationInterval);
            rotateLeft = true;
        }
    }

    /// <summary>
    /// Rotates car to right lane
    /// </summary>
    void RotateCarRight()
    {
        if (rotateRight)
        {
            transform.DORotate(new Vector3(0, 0, 0), rotationInterval);
            rotateRight = false;
        }
        else
        {
            transform.DORotate(new Vector3(0, 0, -rotationDegree * lrSign), rotationInterval);
            rotateRight = true;
        }
    }
}
