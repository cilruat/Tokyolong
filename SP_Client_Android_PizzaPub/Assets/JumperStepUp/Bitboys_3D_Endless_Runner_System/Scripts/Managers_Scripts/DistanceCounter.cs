using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// 3D ENDLESS RUNNER SYSTEM By BITBOYS STUDIO.
public class DistanceCounter : MonoBehaviour {

	[Header("Meters Distance Counter")]

	public float distance; // Used to store the travelled meters distance.
	public float bestDistance;// Used to store the best distance.
	public int bestDistanceInt; // Distance Conversion to use in gooogle play game leaderboard
	public Text distanceText; // The distance text component used in the gameplay scene (top panel)
	public Text bestDistanceText;// The best distance text component used in the Pause Menu panel.
	public Text resultDistanceText; // The distance text component used in the Game over scene.
	public Text resultBestDistanceText;// The best distance text component used in the Game over scene.
	public float metersPerSecond = 2f; // The amount of meters that we will count per second in real time.
	public bool meterIncresing; // Sets when the distance counter starts working.
	private int achievement01Unlocked = 0; // Store if we have already unlocked the achievement in google play games. If set to 0 it's not unlocked, we change it to 1 when we unlock the achievement.
	private int achievement02Unlocked = 0; // """""
	private int achievement03Unlocked = 0;// """""
	private int achievement04Unlocked = 0;// """""
	private int achievement05Unlocked = 0;// """""



	void Start () 
	{

		if (PlayerPrefs.HasKey("PlayerBestDistance")) { // Save the player's best distance in the playerprefs (device)

			bestDistance = PlayerPrefs.GetFloat ("PlayerBestDistance");
		}


		achievement01Unlocked = PlayerPrefs.GetInt ("UnlockedAchievement01"); // Save if we have already unlocked an achievement in the playerprefs.
		achievement02Unlocked = PlayerPrefs.GetInt ("UnlockedAchievement02"); 
		achievement03Unlocked = PlayerPrefs.GetInt ("UnlockedAchievement03"); 
		achievement04Unlocked = PlayerPrefs.GetInt ("UnlockedAchievement04"); 
		achievement05Unlocked = PlayerPrefs.GetInt ("UnlockedAchievement05"); 


	}

	void Update () 
	{

		if (meterIncresing) { // when game starts we start counting meters with this simple formula.


			distance += metersPerSecond * Time.deltaTime; 

		}

		if (distance > bestDistance) { // When the game is over, if the actual reached distance is bigger than the old best distance, we convert the distance to the new best distance.

			bestDistance = distance;

			bestDistanceInt = (int)bestDistance; // This is the same as above but converted to int instead of float to can use it on google play games services.


			PlayerPrefs.SetFloat ("PlayerBestDistance", bestDistance);  // Save the actual player's best distance in the Playerprefs (device)

			GameOverMenu.newBest = true; // if we reach a new best distance, we call the game over menu script to spawn some particle effects (fireworks)



		}



		// METERS DISTANCE TEXT
		distanceText.text =  Mathf.Round(distance)+ "m";
		resultDistanceText.text =  Mathf.Round(distance) + "m";


		// BEST METERS DISTANCE
		bestDistanceText.text = Mathf.Round(bestDistance )+ "m";
		resultBestDistanceText.text = Mathf.Round(bestDistance)+ "m";

		//GOOGLE PLAY ACHIEVEMENTS UNLOCK AND SAVE:

		// Unlock the 100 meters google play achievment if reach 100 meters disitance.
		if (distance >= 100 && achievement01Unlocked == 0) {

			achievement01Unlocked = 1;

			PlayerPrefs.SetInt ("UnlockedAchievement01", achievement01Unlocked);

			// From here you can call the googleplayleaderboard to unlock the achievement if your game use the google play games services.
		}

		// Unlock the 200 meters google play achievment if reach 200 meters disitance.
		if (distance >= 200 && achievement02Unlocked == 0) {

			achievement02Unlocked = 1;

			PlayerPrefs.SetInt ("UnlockedAchievement02", achievement02Unlocked);

			// From here you can call the googleplayleaderboard to unlock the achievement if your game use the google play games services.

		}
		// Unlock the 300 meters google play achievment if reach 300 meters disitance.
		if (distance >= 300 && achievement03Unlocked == 0) {

			achievement02Unlocked = 1;

			PlayerPrefs.SetInt ("UnlockedAchievement03", achievement03Unlocked);

			// From here you can call the googleplayleaderboard to unlock the achievement if your game use the google play games services.

		}
		// Unlock the 400 meters google play achievment if reach 400 meters disitance.
		if (distance >= 400 && achievement04Unlocked == 0) {

			achievement04Unlocked = 1;

			PlayerPrefs.SetInt ("UnlockedAchievement04", achievement04Unlocked);

			// From here you can call the googleplayleaderboard to unlock the achievement if your game use the google play games services.

		}
		// Unlock the 500 meters google play achievment if reach 500 meters disitance.
		if (distance >= 500 && achievement05Unlocked == 0) {

			achievement05Unlocked = 1;

			PlayerPrefs.SetInt ("UnlockedAchievement05", achievement05Unlocked);

			// From here you can call the googleplayleaderboard to unlock the achievement if your game use the google play games services.

		}
	}
		
}