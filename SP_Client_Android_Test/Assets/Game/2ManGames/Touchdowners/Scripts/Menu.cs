using UnityEngine;

namespace Touchdowners
{

    public class Menu : MonoBehaviour
    {
     
        // Used by UI Button
        public void LoadScene(int sceneIndex)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneIndex);
        }
    }

}