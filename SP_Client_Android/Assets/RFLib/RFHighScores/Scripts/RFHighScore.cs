using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;


namespace RFLib
{
	/// <summary>
	/// RF High Scores data management class
	/// Loads and saves data to storage using the Binary Serializer
	/// Class 
	/// </summary>
	public class RFHighScoresManager
	{
		// Filename to store scores as; change as needed
		string highscoreFilename = "rf_highscores.dat";

		int maxListSize = 10;	// Maximum number of high scores to "save";

		// List of high scores, internally managed
		List<RFHighScore> highScoresData;	

		// Default high scores list;  If the high scores data is loaded, but has no
		// scores, then the default list may be used to prefill highScoresData
		RFHighScore[] defaultScoresList;		

		// Setter for default high scores
		public RFHighScore[] DefaultScoreList
		{
			set { defaultScoresList = value; }
		}

		// Set the maximum number of scores allowed.  Cull the list if the new max count is less than
		// the current max cound (maxListSize)
		public int MaximumScoresCount
		{
			get { return maxListSize; }

			set { 
					int oldvalue = maxListSize;
					maxListSize = value; 
					// Can't have less than 0 high scores!
					if( maxListSize < 0 )	maxListSize = 0;

					// Shrunk the list; cull it
					if(maxListSize < oldvalue)
						cullList();				// If the list is greater than the max size, trim off the extra
				}
		}

		//  File name used to store high scores; the file is located
		//  in the Application.persistentDataPath for the particular OS (see unity docs for specifics)
		public string HighScoreFilename
		{
			get { return highscoreFilename; }
			set
			{ 
				string scoreFile = value;
				if( string.IsNullOrEmpty( scoreFile ) )
					scoreFile = "rf_highscores.dat";

				//Force it to start with a /
				if( scoreFile[ 0 ] != '/' )
					scoreFile = "/" + scoreFile;
				highscoreFilename = scoreFile;
			}

		}

		// HighScoresList getter; if null, auto-create the managed list
		public List<RFHighScore> HighScoresList
		{
			get
			{
				if( highScoresData == null )
					highScoresData = new List<RFHighScore>();
				return highScoresData;
			}
		}


		// Default Constructor
		public RFHighScoresManager(){	}


		// Save the High Scores list out to a file.
		public bool SaveHighScores(string filename)
		{

			return RFLib.RFDataStore.SaveBinaryDataFile( filename, highScoresData );

		}

		// Load the high scores from a file
		public bool LoadHighScores(string filename, bool autoSetDefaults = false)
		{
			bool highScoresLoadedOK  = false;
			// The file requested does not exist.
			if( File.Exists( RFLib.RFDataStore.GetFullPath(filename) ) == false )
			{
				// Just set default scores (if they exist..)
				if(autoSetDefaults)
					SetScoresAsDefault();
			}
			// Fileexists, load it up!
			else
			{
				object scoresData = RFLib.RFDataStore.LoadBinaryDataFile( filename );
				if( scoresData != null )
				{
					highScoresData = scoresData  as List<RFHighScore>;
					highScoresLoadedOK = true;
				}

				if( (highScoresData == null || highScoresData.Count == 0) && autoSetDefaults)
					SetScoresAsDefault();
			}

			return highScoresLoadedOK;

		}

		// Set the scores data to the default values indicated.
		public void SetScoresAsDefault()
		{
			if(this.defaultScoresList != null)
			{
				highScoresData = new List<RFHighScore>(defaultScoresList);
			}
		}

		// Is the score a high one? 
		public bool IsHighScore(int score)
		{
			// No actual data:yes, it's automatically a high score.
			// Or, there is data, but the data count is less than the maximum
			if( highScoresData == null || (highScoresData != null && highScoresData.Count < maxListSize) ) return true;

			// Sort the list; high to low
			sortHighToLow();
			// Return the in-score being greater than the lowest high score (based on sorted values)
			return score > highScoresData[highScoresData.Count-1].HighScore;
		}

		// Add a high score, only if it is actually a high score
		// Returns the new high score data instance with the score
		// value set
		public RFHighScore AddNewScore(int score)
		{
			RFHighScore newScore = null;
			if( IsHighScore( score ) )
			{
				newScore = new RFHighScore();
				newScore.HighScore = score;
				HighScoresList.Add( newScore );
				sortHighToLow();
				cullList();
			}

			return newScore;
		}
		// Add a high score, only if it is actually a high score
		// Returns the new high score data instance with the score
		// and initials values set
		public RFHighScore AddNewScore(int score, string initials)
		{
			RFHighScore newScore = AddNewScore(score);
			if( newScore != null )
			{
				newScore.Initials = initials;
			}

			return newScore;
		}

		// Remove all high scores; 
		public void ClearAllScores()
		{
			if( highScoresData == null )	return;
			
			highScoresData.Clear();
		}

		// Check the list; reduce its size if necessary
		private void cullList()
		{
			if( highScoresData != null )
			{
				// as long as the list element count is larger than the max list size, remove the last element
				if( highScoresData.Count > this.maxListSize )
					highScoresData.RemoveRange( maxListSize, highScoresData.Count - maxListSize );
				

//				while( highScoresData.Count > maxListSize )
//				{
//					highScoresData.RemoveAt( highScoresData.Count - 1 );
//				}

			}
		}

		private void sortHighToLow()
		{
			if( highScoresData != null )
			{
				highScoresData.Sort( (x,y) => y.HighScore.CompareTo(x.HighScore));
			}
		}



	}







	/// <summary>
	/// Serializable class used to store high score data
	/// </summary>
	[Serializable]
	public class RFHighScore
	{

		public int 		HighScore;	// The score attained by the player
		public string 	Initials;	// Players Initials
		public DateTime	Timestamp;	// Timestamp to capture when the high score was created

		public RFHighScore()
		{
			HighScore 	= -1;
			Timestamp 	= DateTime.Now;
			Initials 	= "___";		
		}

		public RFHighScore(int score, string initials)
		{
			HighScore 	= score;
			Timestamp 	= DateTime.Now;
			Initials 	= initials;

		}

		// Get an Initial / Letter at a specific index;  if the index
		// is out of bounds, set the index appropriately
		public string GetInitialAt(int index)
		{
			if( index < 0 )
				index = 0;
			if( index >= Initials.Length )
				index = Initials.Length - 1;
			return Initials.Substring( index, 1 );
		}

		// Set an Initial / Letter at a specific index;  if the index
		// is out of bounds, set the index appropriately
		public void SetInitialAt(int index, string initial)
		{
			if( index < 0 )
				index = 0;
			if( index >= Initials.Length )
				index = Initials.Length - 1;

			Initials = Initials.Remove( index, 1 );
			Initials = Initials.Insert( index, initial );
			
		}

	}
}
