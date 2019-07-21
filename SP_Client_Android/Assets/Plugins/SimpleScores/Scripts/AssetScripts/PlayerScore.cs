using UnityEngine;
using System.Collections;

public class PlayerScore : MonoBehaviour 
{
	private int currentScore;
	public string name;

	// Use this for initialization
	void Start () 
	{
		if (name == "" || name == null)
			Debug.Log ("Name not set in script: PlayerScore.cs, is this correct?");

		currentScore = 0;
	}
	
	// Update is called once per frame
	void Update () 
	{
	}

	public void AddScore(int score)
	{
		currentScore += score;
	}

	public int GetCurrentScore()
	{
		return currentScore;
	}
}
