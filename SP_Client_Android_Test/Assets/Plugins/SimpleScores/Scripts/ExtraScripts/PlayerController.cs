using UnityEngine;
using System.Collections;

/*
 * 
 * THIS CLASS HAS BEEN DEVELOPED TO MOVE AROUND THE MAIN CUBE WITHIN THE EXAMPLE SCENE, THIS IS PURELY FOR EXAMPLE USE AND NOT NECESSARILY
 * USEFUL IN ANY OTHER MEANS. IT DOES HOWEVER CONTROL THE SCORE GATHERING USING OUR SYSTEM (See OnTriggerEnter() for example)
 * 
 */

public class PlayerController : MonoBehaviour 
{	
	// Allows us to store a reference to the score holding class on the player object
	private PlayerScore playerScore;
	private CharacterController cc;
	private int enemiesKilled = 0;
	private bool scoreSet = false;
	public bool changeScene = false;

	public float speed = 1.0f;

	// Use this for initialization
	void Start () 
	{
		cc = GetComponent<CharacterController>();
		playerScore = GetComponent<PlayerScore>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (scoreSet)
			return;

		if (enemiesKilled >= 4 && !scoreSet) 
		{
			// -- THIS CODE BELOW DOESN'T CHANGE THE SCENE BEFORE OPENING THE SCOREBOARD, IT JUST OVERLAYS THE GAME AND BREAKS THE UPDATE
			if (changeScene)
			{
				// -- THIS CODE WILL CHANGE THE SCENE AND THE OBJECT IN THE NEW SCENE HELPS TURN THE SCOREBOARD ON (Doesnt get destroyed on load, singleton-ish)
				Application.LoadLevel("SimpleScores_Scoreboard");
				scoreSet = true;
			}
			else
			{
				// upon finishing the level, grab the simple scores object from the scene and set up the scores
				GameObject scoreBoard = GameObject.Find("SimpleScores");
				scoreBoard.GetComponent<Highscore>().PushScoresToBoard(); // add the new scores to the leaderboard
				scoreBoard.GetComponent<OnScreenGUI>().enabled = true; // toggle the GUI script on so the leaderboard can be scene
				scoreSet = true;
			}
		}

		Vector3 direction = Vector3.zero;

		if (Input.GetKey (KeyCode.W))
			direction.z += 1;

		if (Input.GetKey (KeyCode.S))
			direction.z -= 1;

		if (Input.GetKey (KeyCode.A))
			direction.x -= 1;

		if (Input.GetKey (KeyCode.D))
			direction.x += 1;

		direction.y -= 9.81f;

		cc.Move (direction * Time.deltaTime * speed);
	}

	void OnTriggerEnter(Collider col)
	{
		// upon colliding with an entity grab our score holding component
		EntityScoreValue esv = col.GetComponent<EntityScoreValue>();

		// if the component isn't null (if it exists) then we know there are points available for the player
		if (esv != null) 
		{
			// using a refernce to the playerscore class on the playerobject we add the available points to the players current score
			playerScore.AddScore(esv.pointsWorth);
			enemiesKilled += 1; // increment the kill count to help end the level
			Destroy(col.gameObject); // destroy the object to prevent any bugs appearing
		}
	}
}
