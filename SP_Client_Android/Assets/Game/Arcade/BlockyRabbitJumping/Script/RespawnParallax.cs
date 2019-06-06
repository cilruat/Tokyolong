using UnityEngine;
using System.Collections;

namespace BlockJumpRabbit
{
public class RespawnParallax : MonoBehaviour {
	private float valueSpawn ;

	private SpriteRenderer sprite;

	private float distanceRespawn;
	private float currentPosCamera;
	private float distance;
	private float worldWildthScreeGame;

	void Awake()
	{
		worldWildthScreeGame = Camera.main.orthographicSize * Screen.width / Screen.height * 2;

		currentPosCamera = Camera.main.transform.position.x;
	}

	void Update()
	{
		float xMoveDelta = Camera.main.transform.position.x - currentPosCamera;
		bool updateLookAheadTarget = Mathf.Abs(xMoveDelta) > 0.2f;

		if (updateLookAheadTarget)
			distance = Camera.main.transform.position.x - transform.position.x + 0.5f;
		else
			distance = Camera.main.transform.position.x - transform.position.x;

        if (distance >= worldWildthScreeGame / 2 + 2.0f)
            Spawn();

		currentPosCamera = Camera.main.transform.position.x;
	}

//	void OnBecameInvisible()
//	{
//		if (tree)
//			Spawn();
//	}

	//void Spawn()
	//{
	//	transform.position = new Vector3 (transform.position.x + sprite.bounds.size.x * 2, transform.position.y, transform.position.z);
	//}

	void Spawn() {
		float realWidth = Camera.main.orthographicSize * Screen.width / Screen.height * 2;
        valueSpawn = realWidth + 4;
		transform.position = new Vector3 (transform.position.x + valueSpawn - 0.3f, transform.position.y, transform.position.z);
	}
}
}