using UnityEngine;
using System.Collections;

namespace BlockJumpRabbit
{
public enum TypeSnake{
	TAIL,
	HEAD
}

public class PlatformSnake : Platform {

	public float timeToKillPlayer = 0.45f;
	public PlatformSnake platformSnakeHead;
	public Animator anim_head;
	public TypeSnake typeSnake;

	public bool active;

	private float timer = 0.0f;

	void OnTriggerStay2D(Collider2D other) {
		if(other.tag == "Player") {
			timer += Time.deltaTime;

			if (typeSnake == TypeSnake.TAIL) {
				anim_head.SetTrigger ("wake_up");
				platformSnakeHead.active = true;
				platformSnakeHead.timeToKillPlayer = 0.55f;
			}
			
			if (timer > timeToKillPlayer) {
				animPlatform.SetTrigger ("kill_player");

				if (typeSnake == TypeSnake.TAIL || !active)
					other.GetComponent<PlayerController> ().FallWater ();
				else
					other.GetComponent<PlayerController> ().EatPlayer ();

				timer = 0.0f;
			}
		}
	}

	public override void Deactive ()
	{
		timer = 0.0f;

		if(typeSnake == TypeSnake.TAIL)
			platformSnakeHead.Deactive ();
		else {
			active = false;
			timeToKillPlayer = 0.0f;
		}

		base.Deactive ();
	}

	void OnEnable(){
		if (typeSnake == TypeSnake.TAIL)
			platformSnakeHead.gameObject.SetActive (true);
	}
}
}