using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace JumperStepUp
{
// 3D ENDLESS RUNNER SYSTEM By BITBOYS STUDIO.
public class BigCoin : MonoBehaviour {

	[Header("Big Coin (X 10 Coins)")]

	public float SpinSpeed = 360.0f;// The spin speed
	public GameObject coinEffects; // the particles prefab to spawn when the player touch this.
	// static int numberOfCoins = 0; // the amount of tokens that are recorded when we pick them.
	private CoinCounter counter;

	void Awake()
	{
		counter = FindObjectOfType<CoinCounter> ();
	}

	void Update()
	{
		this.transform.rotation *= Quaternion.AngleAxis(SpinSpeed * Time.deltaTime, transform.up);
	}

	void OnTriggerEnter(Collider col)// If collides with the player.
	{


		if (col.GetComponent<CharController> () == null)
			return;

		Instantiate(coinEffects, transform.position, Quaternion.identity);// instantiate particle effects prefab

		counter.AddTenCoins();// the amount of tokens that we have + 1.
		Destroy (this.gameObject);// Finally we destroy de crate object to make it dissapear.
	}
}
}