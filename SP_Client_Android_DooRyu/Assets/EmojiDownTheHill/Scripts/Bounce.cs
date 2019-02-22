using UnityEngine;
using System.Collections;

public class Bounce : MonoBehaviour
{

    public Animator anime;

    void OnTriggerEnter2D(Collider2D coll)
    {
        // if the the player steps on a cube , it bounces
        if (coll.tag == "Player")
        {
            anime.SetTrigger("StampedOn");
        }
    }
}
