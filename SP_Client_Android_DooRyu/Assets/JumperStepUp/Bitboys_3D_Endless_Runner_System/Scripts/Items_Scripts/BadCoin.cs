using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace JumperStepUp
{
// 3D ENDLESS RUNNER SYSTEM By BITBOYS STUDIO.
public class BadCoin : MonoBehaviour {

	[Header("Bad Coin")]

	public float SpinSpeed = 360.0f; // The spin speed
	public GameObject badCoinEffects; // the particles prefab to spawn when the player touch this.
	private CoinCounter counter; // Call the coin counter script to substract coins from the meter.

	void Awake()
	{
		counter = FindObjectOfType<CoinCounter> ();
	}

	void Update()
	{
		this.transform.rotation *= Quaternion.AngleAxis(SpinSpeed * Time.deltaTime, transform.up); // The constant rotation.
	}

	void OnTriggerEnter(Collider col) // If collides with the player.
	{
		if (col.GetComponent<CharController> () == null)
			return;

		Instantiate(badCoinEffects, transform.position, Quaternion.identity);// instantiate particle effects prefab

		counter.QuitCoins();// the amount of tokens that we have + 1.
		Destroy (this.gameObject);// Finally we destroy de crate object to make it dissapear.
	}
}
}