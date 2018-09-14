using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour {

	public GameObject objBoard;
	public CountDown countDown;

	void Start()
	{
		countDown.Set(3, () => _ReturnHome());
	}

	void _ReturnHome()
	{
		SceneChanger.LoadScene (Info.practiceGame ? "PracticeGame" : "Main", objBoard);
	}
}
