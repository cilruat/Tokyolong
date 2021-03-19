using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace PhotonGame
{

    [RequireComponent(typeof(InputField))]
    public class PlayerNameInputField : MonoBehaviour
    {
        #region Private Variables
        // Store the PlayerPref Key to avoid typos
        static string playerNamePrefKey = "PlayerName";
        #endregion

        #region MonoBehaviour CallBacks
        void Start()
        {
            string defaultName = "";
            InputField _inputField = this.GetComponent<InputField>();
            if (_inputField != null)
            {
                if (PlayerPrefs.HasKey(playerNamePrefKey))
                {
                    defaultName = PlayerPrefs.GetString(playerNamePrefKey);
                    _inputField.text = defaultName;
                }
            }
            PhotonNetwork.playerName = defaultName;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Sets the name of the player, and save it in the PlayerPrefs for future sessions.
        /// </summary>
        /// <param name="value">The name of the Player</param>
        public void SetPlayerName(string value)
        {
            // #Important
            PhotonNetwork.playerName = value + " "; // force a trailing space string in case value is an empty string, else playerName would not be updated.
            PlayerPrefs.SetString(playerNamePrefKey, value);
        }
        #endregion
    }
}