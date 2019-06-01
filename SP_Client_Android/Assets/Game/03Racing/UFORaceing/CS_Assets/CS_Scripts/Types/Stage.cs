using UnityEngine;
using System;

namespace SpeederRunGame.Types
{
	/// <summary>
	/// 
	/// </summary>
	[Serializable]
	public class Stage
	{
        //[Tooltip("The name of the stage, which is displayed in the Stage Selector")]
        //public string stageName = "Piggy Bank";

        [Tooltip("The name of this stage. This is not the name of the scene that will be loaded")]
        public string stageName;

        [Tooltip("The name of the scene that will be loaded when this stage is selected. This should correspond to the list of scenes in your Build Settings")]
        public string sceneName = "Level1";

        [Tooltip("The highest score we got in this stage. You gain stars based on your highscore in each stage")]
        internal float highscore = 0;

        [Tooltip("The number of stars needed to unlock this stage")]
        public int starsToUnlock = 0;
    }
}