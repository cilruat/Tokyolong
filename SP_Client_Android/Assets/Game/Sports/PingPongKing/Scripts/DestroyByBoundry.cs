using UnityEngine;
using System.Collections;

namespace Takgu
{
    /// <summary>
    /// This script destroys the ball game object when it leaves game area
    /// </summary>
    public class DestroyByBoundry : MonoBehaviour
    {



        void OnTriggerEnter2D(Collider2D other)
        {

            //On entering the boundries outside the game area the ball prefab will destroy
			Destroy(other.gameObject);

        }

    }
}