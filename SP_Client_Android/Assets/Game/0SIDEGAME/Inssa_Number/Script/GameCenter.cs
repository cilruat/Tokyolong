/********************************************************************
 * Game : Freaking Game
 * Scene : Common
 * Description : Game Center iOS
 * History:
 *	2016/09/25	TungNguyen	First Edition
********************************************************************/
using UnityEngine;
using UnityEngine.SocialPlatforms;


namespace NumberTest
{
public class GameCenter : MonoBehaviour {
	
	static ILeaderboard m_Leaderboard;
	
	private string leaderboardName = "GAME_NAME";
	private string leaderboardID = "YOUR_ID";
	
	bool isAuth = false;
	
	// THIS MAKES SURE THE GAME CENTER INTEGRATION WILL ONLY WORK WHEN OPERATING ON AN APPLE IOS DEVICE (iPHONE, iPOD TOUCH, iPAD)
	//#if UNITY_IPHONE
	
	// Use this for initialization
	void Start () {
		if (isAuth == false) {
			isAuth = true;
						// AUTHENTICATE AND REGISTER A ProcessAuthentication CALLBACK
						// THIS CALL NEEDS OT BE MADE BEFORE WE CAN PROCEED TO OTHER CALLS IN THE Social API
						Social.localUser.Authenticate (ProcessAuthentication);
		
						// GET INSTANCE OF LEADERBOARD
						DoLeaderboard ();
				}
	}
	
	// Update is called once per frame
	void Update () {
		
		
	}
	
	// THE UI BELOW CONTAINING GUI BUTTONS IS USED TO DEMONSTRATE THE GAME CENTER INTEGRATION
	// HERE, YOU ARE ABLE TO:
	// (1) VIEW LEADERBOARDS 
	// (2) VIEW ACHIEVEMENTS
	// (3) SUBMIT HIGH SCORE TO LEADERBOARD
	// (4) REPORT ACHIEVEMENTS ACQUIRED
	// (5) RESET ACHIEVEMENTS.
	
	///////////////////////////////////////////////////
	// INITAL AUTHENTICATION (MUST BE DONE FIRST)
	///////////////////////////////////////////////////
	
	// THIS FUNCTION GETS CALLED WHEN AUTHENTICATION COMPLETES
	// NOTE THAT IF THE OPERATION IS SUCCESSFUL Social.localUser WILL CONTAIN DATA FROM THE GAME CENTER SERVER
	void ProcessAuthentication (bool success) {
		if (success) {
			Debug.Log ("Authenticated, checking achievements");
			
			// MAKE REQUEST TO GET LOADED ACHIEVEMENTS AND REGISTER A CALLBACK FOR PROCESSING THEM
			
			Social.LoadScores(leaderboardName, scores => {
				if (scores.Length > 0) {
					// SHOW THE SCORES RECEIVED
					Debug.Log ("Received " + scores.Length + " scores");
					string myScores = "Leaderboard: \n";
					foreach (IScore score in scores)
						myScores += "\t" + score.userID + " " + score.formattedValue + " " + score.date + "\n";
					Debug.Log (myScores);
				}
				else
					Debug.Log ("No scores have been loaded.");
			});
		}
		else
			Debug.Log ("Failed to authenticate with Game Center.");
	}
	

	
	/// <summary>
	/// Get the leaderboard.
	/// </summary>
	void DoLeaderboard () {
		m_Leaderboard = Social.CreateLeaderboard();
		m_Leaderboard.id = leaderboardID;  // YOUR CUSTOM LEADERBOARD NAME
		m_Leaderboard.LoadScores(result => DidLoadLeaderboard(result));
	}
	
	/// <summary>
	/// RETURNS THE NUMBER OF LEADERBOARD SCORES THAT WERE RECEIVED BY THE APP
	/// </summary>
	/// <param name="result">If set to <c>true</c> result.</param>
	void DidLoadLeaderboard (bool result) {
		Debug.Log("Received " + m_Leaderboard.scores.Length + " scores");
		foreach (IScore score in m_Leaderboard.scores) {
			Debug.Log(score);
		}
		//Social.ShowLeaderboardUI();
	}
	

}
}
//#endif