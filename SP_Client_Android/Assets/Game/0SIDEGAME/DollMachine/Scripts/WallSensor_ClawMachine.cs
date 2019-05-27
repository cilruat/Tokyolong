using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSensor_ClawMachine : MonoBehaviour {

    public Manager_ClawMovement managerClawMovement;

    public void OnTriggerEnter(Collider other)
    {
        // If we've hit an inside wall
        if(other.CompareTag("InsideWall") && !managerClawMovement.stopMovement)
        {
            managerClawMovement.stopMovement = true;
        }
    }
}