using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace JumperStepUp
{
// YOU BETTER RUN By BITBOYS STUDIO.
public class UiCoinSpawner : MonoBehaviour { // This script is a custom method created to "instantiate" fake coins that goes flying to the UI everytime the character collides with a real coin in the game.
	//This was created to avoid to instantiate and destroy many coins at the same time and save performance, reusing the same coins every time.
	//This script works together with the "CoinToUI" script.
	[Header("Fake Coin Pooling")]

	public List<GameObject> fakeCoin; // The list of coin models to use when we want to spawn a coin.
	public Transform player; // The player object.
	public List<GameObject> tenFakeCoins; // the list of coin models to use when we want to spawn 10 coins.


	void Update (){

		this.transform.position = Vector3.MoveTowards(transform.position, player.position, 0.5f); // with this line we copy the player position and make this object to follow the player.

	}


	public void SpawnFakeCoin(){ // When we call this function we make active the first coin in the list. If the first coin is not available we select the next coin in the list.

     fakeCoin [0].gameObject.SetActive (true);
		

     if (fakeCoin[0]) {

     fakeCoin[0] = fakeCoin[1];

     }
     if (fakeCoin[1]) {

     fakeCoin[1] = fakeCoin[2];

     }
     if (fakeCoin[2]) {

     fakeCoin[2] = fakeCoin[3];

     }

     if (fakeCoin[3]) {

     fakeCoin[3] = fakeCoin[4];

     }

     if (fakeCoin[4]) {

     fakeCoin[4] = fakeCoin[0];

}

	if (fakeCoin[5]) {

	fakeCoin[5] = fakeCoin[0];

		}
	}



	public void SpawnTenFakeCoin(){ // this function is called when the player collides with a big coin.


		StartCoroutine(SpawnCoins()); 

	}



	public IEnumerator SpawnCoins() // So simple, let's activate one coin behind another waiting a small amount of time before to activate the next coin.
	{


		tenFakeCoins [0].gameObject.SetActive (true);

		yield return new WaitForSeconds(0.05f);

		tenFakeCoins [1].gameObject.SetActive (true);

		yield return new WaitForSeconds(0.05f);

		tenFakeCoins [2].gameObject.SetActive (true);

		yield return new WaitForSeconds(0.05f);

		tenFakeCoins [3].gameObject.SetActive (true);

		yield return new WaitForSeconds(0.05f);

		tenFakeCoins [4].gameObject.SetActive (true);

		yield return new WaitForSeconds(0.05f);

		tenFakeCoins [5].gameObject.SetActive (true);

		yield return new WaitForSeconds(0.05f);

		tenFakeCoins [6].gameObject.SetActive (true);

		yield return new WaitForSeconds(0.05f);

		tenFakeCoins [7].gameObject.SetActive (true);

		yield return new WaitForSeconds(0.05f);

		tenFakeCoins [8].gameObject.SetActive (true);

		yield return new WaitForSeconds(0.05f);

		tenFakeCoins [9].gameObject.SetActive (true);


		yield return null;
}
	}
}
