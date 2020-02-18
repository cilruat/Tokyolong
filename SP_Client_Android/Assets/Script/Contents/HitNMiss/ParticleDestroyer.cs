using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDestroyer : MonoBehaviour 
{
    public ParticleSystem ps;

    void Update()
    {
        if (ps == null)
            return;

        if (ps.IsAlive() == false)
            Destroy(this.gameObject);
    }
}
