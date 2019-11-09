using UnityEngine;
using System.Collections;

/*
 * 
 * THIS CLASS IS ATTACHED TO THE OBJECT IN THE NEW SCENE TO TOGGLE THE VISUALS AND PUSH THE CORRECT DATA TO THE SCOREBOARD OBJECT SHOWN
 * 
 */
 
public class NewSceneController : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
	{
		// grabs the object bought across scenes and toggles it on
		GameObject scoreBoard = GameObject.Find("SimpleScores"); // not the most optimised method of doing this, but it's an option
		scoreBoard.GetComponent<Highscore>().PushScoresToBoard(); // add the previously added scores to the leaderboard
		scoreBoard.GetComponent<OnScreenGUI>().enabled = true; // toggle the GUI script on so the scoreboard can be seen
	}
	
	// Update is called once per frame
	void Update () 
	{
		// don't do anything
	}
}
