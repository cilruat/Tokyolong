using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

// 3D ENDLESS RUNNER SYSTEM By BITBOYS STUDIO.
public class SaveManager : MonoBehaviour {

	public static SaveManager manager;


	void Start(){

		if (manager == null) {
			//DontDestroyOnLoad (gameObject);
			manager = this;
		} else if(manager != this){

			Destroy (gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Save(){

		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "/playerInfo.dat");

		PlayerData data = new PlayerData ();

		data.coinsAmount = CoinCounter.coinCount; // Coin counter connection


		bf.Serialize (file, data);
		file.Close ();

		Debug.Log ("Game Data Saved");

	}

	public void Load(){

		if (File.Exists (Application.persistentDataPath + "/playerInfo.dat")) {

			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/playerInfo.dat",FileMode.Open);
			PlayerData data = (PlayerData)bf.Deserialize (file);
			file.Close ();

			CoinCounter.coinCount = data.coinsAmount; // Convert the coin counter coins to the saved coins data.

			Debug.Log ("Game Data Loaded");
		}
	}

	void OnApplicationQuit (){

		Save ();
		Debug.Log ("Game was closed");

	}


}

[Serializable]
class PlayerData
{

	public int coinsAmount;

}