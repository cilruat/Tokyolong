using UnityEngine;
using System.Collections;

/* 
 * This class was used primarily to show use of the Highscore system and displaying the stored information on screen
 * This class can either be removed and replaced with your own or other UI system, or used as a starting point and edited to your needs
 * 
 * NOTE: The OnGUI() code isn't very efficient, it is primarily there to show the use of each public function in a live scenario. 
 * This code could however be edited to work more efficiently - as well as many of the buttons being removed entirely
 */

public class OnScreenGUI : MonoBehaviour 
{
	private Highscore highScore;
	public Texture upButton;
	public Texture downButton;
	public Texture scoreBackground;

	// text label variables etc
	public GUIStyle guiStyle;
	public float fontRatio = 20.0f;
	private float screenWidth;
	private float screenHeight;

	// Amount of scores to show on screen at once, used for scrolling through the scoreboard - set this value in the Editor
	public int scoresToShow = 5;
	private int highScoreStart = 0;
	private string namesAndScores;
	private bool updateDisplayText = true;

	// Use this for initialization
	void Start () 
	{
		screenWidth = Screen.width;
		screenHeight = Screen.height;
		guiStyle.fontSize = (int)(Mathf.Min(screenWidth, screenHeight) / fontRatio);

		// find and reference Highscore.cs for future use
		highScore = GetComponent<Highscore>();

		// if the amount of scores to show is higher than the amount of scores allowed, restrict it that size
		// Only really necessary to have for editor purpose, if the amount can't be changed in game this statement doesn't need to exist
		if (scoresToShow > highScore.numberOfScores)
			scoresToShow = highScore.numberOfScores;
	}
	
	// Update is called once per frame
	void Update() 
	{			
		// change the scaling values if the Screen actually changes size - dynamic gui
		if (screenWidth != Screen.width || screenHeight != Screen.height)
		{
			screenWidth = Screen.width;
			screenHeight = Screen.height;
			guiStyle.fontSize = (int)(Mathf.Min(screenWidth, screenHeight) / fontRatio);
		}
	
		// if there are no scores stored, don't bother doing anything else
		if (highScore.GetAllStoredScores().Count == 0)
			return;

		if (updateDisplayText)
			GenerateTheNamesAndScoresToShow();
	}

	void OnGUI()
	{	
		float width = Screen.width * 0.1f;
		float height = Screen.height * 0.1f;
	
		// draw the background image so it scales correctly
		GUI.DrawTexture(new Rect((float)(screenWidth * 0.3), (float)(screenHeight * 0.01), (float)(screenWidth * 0.4), (float)(screenHeight * 0.8)), scoreBackground);

		GUI.Label(new Rect((float)(screenWidth * 0.4), (float)(screenHeight * 0.2), (float)(screenWidth * 0.3), (float)(screenHeight * 0.6)), namesAndScores, guiStyle);

		// simple GUI buttons to allow functionality within the leaderboard
		// this can allow a set amount of scores to be viewed at any one time (values set in the editor)
		if (GUI.Button(new Rect((float)((screenWidth * 0.37) - (width / 2)), (float)((screenHeight * 0.35) - (height / 2)), width / 2, height), upButton, guiStyle))
		{
			if (highScoreStart - 1 >= 0)
				highScoreStart -= 1;
			else
				highScoreStart = 0;

			updateDisplayText = true;
		}

		if (GUI.Button(new Rect((float)((screenWidth * 0.37) - (width / 2)), (float)((screenHeight * 0.55) - (height / 2)), width / 2, height), downButton, guiStyle))
		{
			if (highScoreStart + scoresToShow < highScore.numberOfScores)
				highScoreStart += 1;
			else
				highScoreStart = highScore.numberOfScores - scoresToShow;

			updateDisplayText = true;
		}
		
		if (GUI.Button(new Rect((float)((screenWidth * 0.15) - (width / 2)), (float)((screenHeight * 0.9) - (height / 2)), width, height), "Save Local"))
		{
			highScore.SaveValuesLocally();
		}
		
		if (GUI.Button(new Rect((float)((screenWidth * 0.27) - (width / 2)), (float)((screenHeight * 0.9) - (height / 2)), width, height), "Load Local"))
		{
			highScore.LoadValuesLocally();

			updateDisplayText = true;
		}
		
		if (GUI.Button(new Rect((float)((Screen.width * 0.39) - (width / 2)), (float)((screenHeight * 0.9) - (height / 2)), width, height), "Delete Local"))
		{
			highScore.DeleteAllLocallySavedData();
		}
		
		if (GUI.Button(new Rect((float)((screenWidth * 0.51) - (width / 2)), (float)((screenHeight * 0.9) - (height / 2)), width, height), "Clear Board"))
		{
			highScore.ClearScoreBoard();
			namesAndScores = "";
		}

		if (GUI.Button(new Rect((float)((screenWidth * 0.63) - (width / 2)), (float)((screenHeight * 0.9) - (height / 2)), width, height), "Save Online"))
		{
			highScore.SaveValuesToDatabase();
		}

		if (GUI.Button(new Rect((float)((screenWidth * 0.75) - (width / 2)), (float)((screenHeight * 0.9) - (height / 2)), width, height), "Load Online"))
		{
			highScore.LoadValuesFromDatabase();

			updateDisplayText = true;
		}

		if (GUI.Button(new Rect((float)((screenWidth * 0.87) - (width / 2)), (float)((screenHeight * 0.9) - (height / 2)), width, height), "Clear Online"))
		{
			highScore.ClearValuesFromDatabase();
			
			updateDisplayText = true;
		}
	}

	// This function is a possible method to show the scores and values within a GUI label or textfield etc.
	// There are various ways of completing this task, but we chose this way to get the example across.
	void GenerateTheNamesAndScoresToShow()
	{
		namesAndScores = "";
		int maxLength = 30;
		
		// calculate which scores to show based on where the focus is on the scoreboard (controlled by the up and down buttons) 
		int rangeToShow = highScoreStart + scoresToShow;
		// if the range is higher than the amount of values available, restrict it that amount
		if (rangeToShow > highScore.GetAllStoredScores().Count)
			rangeToShow = highScore.GetAllStoredScores().Count;
		
		// this for loop takes each value within the range of scores to show, displays the name and score and adds it to the guitext field to be displayed
		for (int i = highScoreStart; i < rangeToShow; i++)
		{
			ScoreHolder currentPlayer = highScore.GetAllStoredScores()[i];
			string tempString = "";
			
			// work out how many spaces should be inbetween the name and score (visual reasons)
			int nameLength = currentPlayer.Name.Length;
			int diff = maxLength - nameLength;
			
			// add the current name to the string
			tempString += currentPlayer.Name;
			
			// indent correctly
			for (int j = 0; j < diff; j++)
				tempString += " ";
			
			// add the score (value) with a line break to complete this current player
			tempString += currentPlayer.Value + "\n";
			
			// add it to the overall string
			namesAndScores += tempString;
		}
		updateDisplayText = false;
	}
}