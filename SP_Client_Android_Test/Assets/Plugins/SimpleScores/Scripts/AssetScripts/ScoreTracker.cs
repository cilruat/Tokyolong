using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScoreTracker : MonoBehaviour 
{
	public List<PlayerScore> playerScores = new List<PlayerScore>();
	public bool storeScoreAsTeam = false;

	// Use this for initialization
	void Awake() 
	{
		DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void Update () 
	{
	}
}
