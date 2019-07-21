using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * This class provides all the functionality of the Highscore system,
 * The public functions are the ones that will be used to store, clear, load and save all additional information
 * The private functions are primarily there to help with functionaly within this class and do not need to be used outside Highscore.cs
 */

public class Highscore : MonoBehaviour 
{
	// set in the editor to limit how many different scores can be stored within the scoreboard
	public int numberOfScores = 0;
	// list of custom class, used to store the players name(string) and score(int)
	private List<ScoreHolder> listOfScores;
	// used for adding new randomly generated scores
	private int highestPlayerVal;
	// This can be toggled in the editor to automatically fill the list with random names and variables (primarily used for testing and proof of functionality)
	public bool autoFillList = false;

	// database values
	private bool savingToDatabase = false;
	private bool loadingFromDatabase = false;

	// website link to php pages for both loading and saving
	public string phpLoadingWebsiteLink = "";
	public string phpSavingWebsiteLink = "";

	// allows access to the player scores tracked through the gameplay
	private ScoreTracker scoreTracker;

	private void Start () 
	{
		if (numberOfScores == 0)
			Debug.Log("numberOfScores hasn't been set in the editor");
			 
		// set up the lists at the beginning
		listOfScores = new List<ScoreHolder>();

		// This can be removed if automatically filling the list is not needed
		if (autoFillList) 
			RandomlyFillTheList();

		scoreTracker = GetComponent<ScoreTracker>();
	}

	private void Update () 
	{
		// make sure the list is never equal to null
		if (listOfScores == null)
			listOfScores = new List<ScoreHolder>();
	}

	// Fills the list based on the number of scores decided in the editor, this creates an instance of PlayerScore.cs using a name and a random number between 1 and 1000
	// This function can be removed if the list never needs to be filled with random values
	private void RandomlyFillTheList()
	{	
		highestPlayerVal = numberOfScores;
		
		for (int i = 1; i <= numberOfScores; i++)
		{
			ScoreHolder score = new ScoreHolder("player_" + i, Random.Range(0, 1000));
			listOfScores.Add(score);
		}

		SortList(true);
	}

	// As the PlayerScore.cs is "Comparable", the list of scores can be sorted based on the values stored within them
	private void SortList(bool highToLow)
	{
		if (listOfScores.Count == 0)
			return;

		// sorts the list based on the integer value stored within them
		listOfScores.Sort(); 
		
		// flips the entire list from storing them using small to large -> large to small
		if (highToLow)
			listOfScores.Reverse();
	}

	// function called from outside the class to initially store ALL scores within the current score tracker class. Whether it is a team score or solo score
	public void PushScoresToBoard()
	{
		// load the currently saved values so they are not overwritten during the save process
		LoadValuesLocally();

		if (scoreTracker.storeScoreAsTeam) 
		{
			// loop through the list of the stored scores, add them and the team value/name
			int teamScore = 0;
			string teamName = "";

			foreach (PlayerScore score in scoreTracker.playerScores)
			{
				teamScore += score.GetCurrentScore();
				teamName = score.name;
			}

			StoreValue(teamName, teamScore);
		}
		else
		{
			foreach (PlayerScore score in scoreTracker.playerScores)
				StoreValue(score.name, score.GetCurrentScore());
		}
		SaveValuesLocally();
	}

	private void StoreValue(string name, int value)
	{
		// creates a new PlayerScore Object with the inserted values
		ScoreHolder newScore = new ScoreHolder(name, value);
		listOfScores.Add(newScore);

		// sorts the list with the new added value
		SortList(true);

		// cuts the list down to size
		ChopListToSize();
	}

	// If the total amount of stored scores is higher than the value set in the editor (numberOfScores) then remove the last value (lowest score)
	private void ChopListToSize()
	{
		// remove the values that exceed the number of scores
		if (listOfScores.Count > numberOfScores)
		{
			Debug.Log("There are more scores in the list than needed, removing the excess scores from the end.");
			int difference = listOfScores.Count - numberOfScores;
			listOfScores.RemoveRange(numberOfScores, difference);
		}
	}

	// This function should be called when wanting to save the data locally. Calling this after storing a value in the scoreboard would prevent loss of data 
	// Note: Try not to call this save function at important times, as it can become quite memory expensive to save data
	public void SaveValuesLocally()
	{
		for (int i = 0; i < listOfScores.Count; i++)
		{
			PlayerPrefs.SetString("ScorePlace_" + i + "_name", listOfScores[i].Name);
			PlayerPrefs.SetInt("ScorePlace_" + i + "_val", listOfScores[i].Value);
		}
		Debug.Log("All data was stored locally");
	}
	
	// This function should be called when the scoreboard data is needed. For example, loading the data on creation of the scoreboard 
	// Note: Try not to call this load function more than necessary. Like saving, it could become quite memory expensive
	public void LoadValuesLocally()
	{
		listOfScores.Clear ();

		for (int i = 0; i < numberOfScores; i++)
		{
			string name = "";
			name = PlayerPrefs.GetString("ScorePlace_" + i + "_name");
			
			int value = -1;
			value = PlayerPrefs.GetInt("ScorePlace_" + i + "_val");
			
			if (name == "" || value == -1)
				continue;
				
			listOfScores.Add(new ScoreHolder(name, value));
		}
		
		if (listOfScores.Count == 0)
			Debug.Log("No scores we're loaded from local storage, are you sure they were saved last time?");
	}

	// This function should be called when the scoreboard data needs to be saved online. As this function uses a "yield return" Enumerator, the thread will not wait for the response
	// Note: Try not to call this function more than mecessary as it could be come memory expensive, and will spend networking allowance / website bandwidth
	// ALSO: Make sure the PHP scripts included have been edited correctly to use your personal webserver data
	public void SaveValuesToDatabase()
	{		
		if (!savingToDatabase)
		{
			// make sure it can't be overloaded with PHP requests on press
			savingToDatabase = true;

			string url = phpSavingWebsiteLink;

			for (int i = 0; i < listOfScores.Count; i++)
			{
				// Create a form to attach to the web request with the name and score value of the specific scoreboard entry, allowing it to be stored to the online database
				WWWForm form = new WWWForm();
				form.AddField("name", listOfScores[i].Name);
				form.AddField("score", listOfScores[i].Value);

				WWW www = new WWW(url, form);
				StartCoroutine(WaitForRequest(www));
			}
			savingToDatabase = false;
		}
	}
	
	// This function should be called when the scoreboard data needs to be saved online. As this function uses a "yield return" Enumerator, the thread will not wait for the response
	// Note: Try not to call this function more than mecessary as it could be come memory expensive, and will spend networking allowance / website bandwidth
	// ALSO: Make sure the PHP scripts included have been edited correctly to use your personal webserver data
	public void LoadValuesFromDatabase()
	{
		if (!loadingFromDatabase)
		{
			loadingFromDatabase = true;

			string url = phpLoadingWebsiteLink;

			// Create a form to attach to the web request, with a $_POST value to make sure the PHP calls the script to load the data and not delete it all
			WWWForm form = new WWWForm();
			form.AddField("clearData", 0);

			WWW www = new WWW(url, form);
			StartCoroutine(WaitForRequest(www, true));
			
			// allow to be loaded again afer completion
			loadingFromDatabase = false;
		}
	}

	// This function has been set up to call part of a PHP script that deletes the entire table that is referenced
	// NOTE: Make sure this is only called IF they database table data is no longer needed OR you have a solid back up. This function can not be reverted
	public void ClearValuesFromDatabase()
	{
		string url = phpLoadingWebsiteLink;

		// Create a form to attach to the Web request, with a $_POST value of to allow the PHP to clear the database
		WWWForm form = new WWWForm();
		form.AddField("clearData", 1);

		WWW www = new WWW(url, form);
		StartCoroutine(WaitForRequest(www));
	}

	// This IEnumerator prevents the game thread from waiting until the request has complete, but still returns the required data when needed
	private IEnumerator WaitForRequest(WWW www, bool load = false)
	{		
		yield return www;

		// check for errors
		if (www.error == null)
		{
			Debug.Log("WWW Ok!: " + www.text);
			
			// If the boolean load doesn't default to false, then the text from the PHP will be returned and parsed into usable data within the C#
			if (load)
			{
				string returnedFromPHP = www.text;
			
				// clear the current list
				ClearScoreBoard();
				
				// Because the PHP returns text with the tag <br> splitting each name and score up, this will work correctly. If the PHP side is changed, this will need to be changed too
				string[] brSplit = {"<br>"};
				string[] scores = returnedFromPHP.Split(brSplit, System.StringSplitOptions.None);

				// length is equal to length - 1 as the final element in scores is a blank field due to the tags from the PHP script
				int length = scores.Length - 1;
				for (int i = 0; i < length; i++)
				{
					char[] commaSplit = {','};
					string[] nameAndScore = scores[i].Split(commaSplit);
					
					// After splitting the name and score into two different strings, store the value to the local score system normally
					StoreValue(nameAndScore[0], System.Convert.ToInt32(nameAndScore[1]));
				}
			}
		} 
		else 
		{
			Debug.Log("WWW Error: "+ www.error);
		}  
	}
	
	// deletes all of the current scoreboard data
	public void ClearScoreBoard()
	{
		listOfScores.Clear();
		highestPlayerVal = 0;
	}
	
	// deletes all locally stored data from the scoreboard - This can be used comfortably with clearing and saving
	// WARNING - This function is irreversible, once the data is gone it cannot be retrieved
	public void DeleteAllLocallySavedData()
	{
		PlayerPrefs.DeleteAll();
		Debug.Log("All locally saved data was deleted");
	}


	// returns the current value of the highestPlayer integer (used within the GUI class when adding a new value) - currently used for random naming might be unused in normal situation
	public int GetHighestPlayerVal()
	{
		return highestPlayerVal;
	}
	
	// sets the current value of the highestPlayer integer (used within the GUI class when adding a new value) - currently used for random naming might be unused in normal situation
	public void SetHighestPlayerVal(int val)
	{
		highestPlayerVal = val;
	}
	
	// returns the entire list of the scoreboard for use outside of the class. (used within the GUI class for showing information on screen)
	public List<ScoreHolder> GetAllStoredScores()
	{
		return listOfScores;
	}
}