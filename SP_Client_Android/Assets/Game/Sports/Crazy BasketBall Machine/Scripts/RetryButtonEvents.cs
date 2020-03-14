using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class RetryButtonEvents : MonoBehaviour 
{

	public void playgame()
	{
	
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
	}

	public void Endgame()
	{
		SceneManager.LoadScene("ArcadeGame");
	}

}
