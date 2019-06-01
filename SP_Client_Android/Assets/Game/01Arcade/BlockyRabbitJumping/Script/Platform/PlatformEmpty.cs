using System;
using System.Collections;
using UnityEngine;

namespace BlockJumpRabbit
{
public class PlatformEmpty:Platform
{
    public ParticleSystem par;
    public override void PlatformAction(GameObject player)
    {
		base.PlatformAction (player);

        if (!animPlatform)
            player.GetComponent<PlayerController>().FallWater();
        else {
            animPlatform.SetTrigger("break");
            player.GetComponent<PlayerController>().FallWater();
            if (par)
                par.Play();
        }
    }
}
}
