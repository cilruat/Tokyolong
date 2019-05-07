using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Responsible for determining the next color for the Block and Background gradient.
/// </summary>
public class ColorController : MonoBehaviour {

    [Tooltip("Game's Block and Background colors (Scriptable Object asset)")]
    [SerializeField]
    private ColorPalette colorPalette;

    /// <summary>storing the colors in this variable </summary>
    private List<ColorBP> colors = new List<ColorBP>();

    [Tooltip("The delta with which color transitions for the block")]
    [SerializeField]
    private float delta;

    ///<summary>variable to store lerp time </summary>
    private float time;

    ///<summary>Block's from and to transition colors </summary>
    private int blockIndexFrom, blockIndexTo;

    [Tooltip("The UI material for Gradient background")]
    public Material backgroundMat;

    [Tooltip("The transition speed for Gradient background")]
    public float bgTransitionSpeed;

    ///<summary>Top and bottom colors of the background gradient </summary>
    private Color bgTopColor, bgBottomColor;

    ///<summary>Variables to store the destination colors for transition </summary>
    private Color topBGTo, bottomBGTo;

    ///<summary>Color indexes for calculating the background destination colors </summary>
    private int bgIndexFrom, bgIndexTo;

    void Awake () {
        colors = Instantiate(colorPalette).colors;
    }

    /// <summary>
    /// Resets the Color state for a new game
    /// </summary>
    public void Reset()
    {
        Shuffle();
        time = -delta;

        bgIndexFrom = blockIndexFrom = 0;
        bgIndexTo = blockIndexTo = 1;

        bgTopColor = colors[0].TopColor;
        bgBottomColor = colors[0].BottomColor;
    }

    /// <summary>
    /// Shuffle the List of colors for every new Game
    /// </summary>
    void Shuffle(){
        int n = colors.Count;
        while (n > 1){
            n--;
            int i = Random.Range(0,n);
            ColorBP item = colors[i];
            colors[i] = colors[n];
            colors[n] = item;
        }
    }
	

    /// <summary>
    /// Gets the color for next block. Also sets the color for background transition.
    /// </summary>
    public Color BlockColor
    {
        get
        {
            time += delta;

            //If time is greater than 1, it means we have reached the destination color.
            //Reset it to 1 and set the new "from" and "to" destination colors
            if (time > 1)
            {
                time = 0;

                //Block Color
                blockIndexFrom++;
                blockIndexFrom = blockIndexFrom >= colors.Count ? 0 : blockIndexFrom; //If it is the last color in list, set it to 0.
                blockIndexTo = (blockIndexFrom == colors.Count - 1) ? 0 : blockIndexFrom + 1; //If "from" color is 0, set the "to" color to "from+1". If from is last color in the list, set it to 0. 

                //Background
                bgIndexFrom++;
                bgIndexFrom = bgIndexFrom >= colors.Count ? 0 : bgIndexFrom; //If it is the last color in list, set it to 0.
                bgIndexTo = bgIndexFrom == colors.Count - 1 ? 0 : bgIndexFrom + 1; //If "from" color is 0, set the "to" color to "from+1". If from is last color in the list, set it to 0.

            }
            //Get the next top and bottom for Background color gradient
            topBGTo = Color.Lerp(colors[bgIndexFrom].TopColor, colors[bgIndexTo].TopColor, time);
            bottomBGTo = Color.Lerp(colors[bgIndexFrom].BottomColor, colors[bgIndexTo].BottomColor, time);

            //return the next Block color
            return Color.Lerp(colors[blockIndexFrom].blockColor, colors[blockIndexTo].blockColor, time);
        }
    }


    void Update()
    {
        //Lerp to the destination colors
        bgTopColor = Color.Lerp(bgTopColor, topBGTo, Time.deltaTime * bgTransitionSpeed);
        bgBottomColor = Color.Lerp(bgBottomColor, bottomBGTo, Time.deltaTime * bgTransitionSpeed);

        //Set the Colors to the Background material
        backgroundMat.SetColor("_TopColor", bgTopColor);
        backgroundMat.SetColor("_BottomColor", bgBottomColor);

    }

}
