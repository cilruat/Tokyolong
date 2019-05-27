using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrizeCatcher_ClawMachine : MonoBehaviour {

    [Header("Score Settings")]
    public ParticleSystem coinExplosion;
    public Manager_ClawMovement managerClawMachine;

    public void OnTriggerEnter(Collider other)
    {
        // If we found a prize
        if(other.GetComponent<Item_ClawMachine>())
        {
            coinExplosion.Play();

            // Add the coins
            managerClawMachine.playerCoins += other.GetComponent<Item_ClawMachine>().value;
                     
            // Destroy the prize
            Destroy(other.gameObject);
        }
    }
}
