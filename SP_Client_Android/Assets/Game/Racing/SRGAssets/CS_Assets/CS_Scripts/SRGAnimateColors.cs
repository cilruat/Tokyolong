using UnityEngine;
using System.Collections;

namespace SpeederRunGame
{
	/// <summary>
	/// This script animates a material with several colors over time. You can set a list of colors, and the speed at which they change.
	/// </summary>
	public class SRGAnimateColors:MonoBehaviour 
	{
		//A list of the colors that will be animated
		public Color[] colorList;
		
		//The index number of the current color in the list
		public int colorIndex = 0;

		public bool randomStartColor = true;
		
		//How long the animation of the color change lasts, and a counter to track it
		public float changeTime = 1;
		public float changeTimeCount = 0;
		
		//How quickly the sprite animates from one color to another
		public float changeSpeed = 1;
		
		//Is the animation paused?
		public bool isPaused = false;
		
		//Is the animation looping?
		public bool isLooping = true;

		/// <summary>
		/// Start is only called once in the lifetime of the behaviour.
		/// The difference between Awake and Start is that Start is only called if the script instance is enabled.
		/// This allows you to delay any initialization code, until it is really needed.
		/// Awake is always called before any Start functions.
		/// This allows you to order initialization of scripts
		/// </summary>
		void Start() 
		{
			if ( randomStartColor )    colorIndex = Mathf.FloorToInt(Random.Range(0, colorList.Length));

			//Apply the chosen color to the sprite or text mesh
			SetColor(colorIndex);
		}
		
		// Update is called once per frame
		void Update() 
		{
			//If the animation isn't paused, animate it over time
			if ( isPaused == false )
			{
				if ( changeTime > 0 )
				{
					//Count down to the next color change
					if ( changeTimeCount > 0 )
					{
						changeTimeCount -= Time.deltaTime;
					}
					else
					{
						changeTimeCount = changeTime;
						
						//Switch to the next color
						if ( colorIndex < colorList.Length - 1 )
						{
							colorIndex++;
						}
						else
						{
							if ( isLooping == true )    colorIndex = 0;
						}
					}
				}

				// If we have a mesh renderer, animate the color change
				if ( GetComponent<MeshRenderer>() )
				{
					GetComponent<MeshRenderer>().material.color = Color.Lerp(GetComponent<MeshRenderer>().material.color, colorList[colorIndex], changeSpeed * Time.deltaTime);
				}
			}
			else
			{
				//Apply the chosen color to the sprite or text meshh
				SetColor(colorIndex);
			}
		}

		/// <summary>
		/// Applies the chosen color to the sprite based on the index from the list of colors
		/// </summary>
		void SetColor( int setValue )
		{
			//If you have a text mesh component attached to this object, set its color
			if ( GetComponent<TextMesh>() )
			{
				GetComponent<TextMesh>().color = colorList[setValue];
			}
			
			//If you have a sprite renderer component attached to this object, set its color
			if ( GetComponent<SpriteRenderer>() )
			{
				GetComponent<SpriteRenderer>().color = colorList[setValue];
			}


			if ( GetComponent<MeshRenderer>() )
			{
				GetComponent<MeshRenderer>().material.color = colorList[setValue];
			}
		}

		/// <summary>
		/// Sets the pause state of the color animation
		/// </summary>
		/// <param name="pauseState">Pause state, true paused, false unpaused</param>
		void Pause( bool pauseState )
		{
			isPaused = pauseState;
		}

	}
}
