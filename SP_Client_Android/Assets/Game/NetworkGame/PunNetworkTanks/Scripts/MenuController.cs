using UnityEngine;
using UnityEngine.SceneManagement;


namespace PhotonGame
{

    public class MenuController : MonoBehaviour
    {
        public void LoadLobbyMenu()
        {
            SceneManager.LoadScene("LobbyMenu");
        }

        public void LoadMainMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}