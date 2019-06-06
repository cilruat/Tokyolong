using UnityEngine;
using System.Collections;

namespace BlockJumpRabbit
{
// Use for turtle and snake enemy
public class PlatformTurtle : Platform {

	public float timeToKillPlayer = 0.2f;
	private float timer = 0.0f;

	void OnTriggerStay2D(Collider2D other) {

        if (other.tag == "Player") {
			timer += Time.deltaTime;
            animPlatform.SetTrigger("wake_up");
			if (timer > timeToKillPlayer) {
				if (animPlatform)
					animPlatform.SetTrigger ("kill_player");

                if (typePlatform == TypePlatform.Turtle)
                    other.GetComponent<PlayerController>().FallWater();
                else other.GetComponent<PlayerController>().EatPlayer();

                timer = 0.0f;
			}
		}
	}

    public override void Deactive ()
	{
		timer = 0.0f;
		base.Deactive ();
	}
}
}