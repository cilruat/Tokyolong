using UnityEngine;

namespace CoinFrenzyGame
{
	/// <summary>
	/// Randomizes the color of an object
	/// </summary>
	public class CFGRandomizeColor : MonoBehaviour
	{
		// A list of possible colors for the object
		public Color[] colorList;
	
		/// <summary>
		/// Start is only called once in the lifetime of the behaviour.
		/// The difference between Awake and Start is that Start is only called if the script instance is enabled.
		/// This allows you to delay any initialization code, until it is really needed.
		/// Awake is always called before any Start functions.
		/// This allows you to order initialization of scripts
		/// </summary>
		void Start()
		{
			// Choose a random color from the list
			int randomColor = Mathf.FloorToInt(Random.Range(0, colorList.Length));
			Component[] comps = GetComponentsInChildren<Renderer>();

			// Set the color to all parts of the object
			foreach( Renderer part in comps )
            {
                part.GetComponent<Renderer>().material.color = colorList[randomColor];
            }
        }
	}
}