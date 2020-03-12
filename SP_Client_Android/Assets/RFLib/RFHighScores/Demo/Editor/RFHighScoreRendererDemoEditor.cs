using UnityEngine;
using UnityEditor;
using System.Collections;

using RFLibEditor;

namespace RFLibDemo
{
	// Same basic editor as the High Score Renderer, but specifically for the demo renderer class
	[CustomEditor(typeof(RFHighScoreRendererDemo))]
	public class RFHighScoreRendererDemoEditor : RFHighScoreRendererEditor{	}
}