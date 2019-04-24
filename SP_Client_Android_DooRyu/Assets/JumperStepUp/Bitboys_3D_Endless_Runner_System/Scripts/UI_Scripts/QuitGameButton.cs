using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
namespace JumperStepUp
{
// YOU BETTER RUN By BITBOYS STUDIO.
public class QuitGameButton : MonoBehaviour {

	public void QuitGame(){

		#if UNITY_EDITOR // If we are using the unity editor, press the button Stop playback of the scene.

		UnityEditor.EditorApplication.isPlaying = false;

		#else // If we are using the game buil the game will closes when you press the quit button.

		Application.Quit();
		#endif

	}
}
}