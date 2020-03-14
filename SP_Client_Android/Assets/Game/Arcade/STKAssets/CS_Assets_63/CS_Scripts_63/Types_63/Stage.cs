using UnityEngine;
using System;

namespace StackGameTemplate.Types
{
	/// <summary>
	/// 
	/// </summary>
	[Serializable]
	public class Stage
	{
        //[Tooltip("The name of the stage, which is displayed in the Stage Selector")]
        //public string stageName = "Piggy Bank";

        [Tooltip("The object representing this stage, can be a 2D icon or a 3D object")]
        public Transform stageIcon;

        [Tooltip("The name of the scene that will be loaded when this stage is selected. This should correspond to the list of scenes in your Build Settings")]
        public string sceneName = "Level1";

        [Tooltip("The highest score we got in this stage. You gain stars based on your highscore in each stage")]
        internal int highscore = 0;

        [Tooltip("The number of stars needed to unlock this stage")]
        public int starsToUnlock = 0;
    }
}