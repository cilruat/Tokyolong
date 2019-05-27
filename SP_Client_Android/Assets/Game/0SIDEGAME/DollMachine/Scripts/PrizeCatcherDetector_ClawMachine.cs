using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrizeCatcherDetector_ClawMachine : MonoBehaviour {

    public bool isClawAbovePrizeCatcher = false;


    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("WallSensor"))
        {
            isClawAbovePrizeCatcher = true;
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("WallSensor"))
        {
            isClawAbovePrizeCatcher = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("WallSensor"))
        {
            isClawAbovePrizeCatcher = false;
        }
    }
}
