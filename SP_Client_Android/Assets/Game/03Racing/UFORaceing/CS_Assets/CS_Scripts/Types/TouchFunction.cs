using System;

namespace SpeederRunGame.Types
{
	[Serializable]
	public class TouchFunction
	{
		// The name of the function that will run
		public string functionName = "ChangeScore";
		
		// The tag of the target that the function will run on
		public string targetTag = "GameController";
		
		// A parameter that is passed along with the function
		public float functionParameter = 0;
	}
}
