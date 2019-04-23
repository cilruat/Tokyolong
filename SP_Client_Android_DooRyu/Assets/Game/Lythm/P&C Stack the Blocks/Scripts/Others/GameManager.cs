using UnityEngine;
using PnCCasualGameKit;
/// <summary>
/// Consits of Game state events and other General Game specific functionalities. 
/// </summary>
public class GameManager : LazySingleton<GameManager>
{
    /// <summary>
    ///  Game state events
    /// </summary>
    public System.Action GameInitialized, GameStarted, GameOver;

    [SerializeField]
    private string playStoreURL, appStoreURL;

    private void Awake()
    {
        PlayerData.Create();
    }

    /// <summary>
    /// Init Game in Start because other classes register for events in Awake
    /// </summary>
    void Start()
    {
        InitGame();
    }

    /// <summary>
    /// Initialise game
    /// </summary>
    public void InitGame()
    {
        if (GameInitialized != null)
            GameInitialized();
    }

    /// <summary>
    /// Start Game
    /// </summary>
    public void StartGame()
    {
        if (GameStarted != null)
            GameStarted();
    }

    /// <summary>
    /// Opens the store pages
    /// </summary>
    public void RateGame()
    {
#if UNITY_ANDROID
        Application.OpenURL(playStoreURL);
#elif UNITY_IOS
        Application.OpenURL(appStoreURL);
#endif
    }

    /// <summary>
    ///For testing.  Editor Buttons available in the inspector.
    /// </summary>
#region TESTING

    /// <summary>
    /// Increases player cash
    /// </summary>
    public void IncreasePlayerCash()
    {
        float increaseBy = 100;
        PlayerData.Instance.cash += increaseBy;
        PlayerData.Instance.SaveData();
        Debug.Log("player cash increased by " + increaseBy);
    }

   /// <summary>
   /// Decreases the player cash.
   /// </summary>
    public void DecreasePlayerCash()
    {
        float decreaseBy = 100;
        PlayerData.Instance.cash -= decreaseBy;
        PlayerData.Instance.SaveData();
        Debug.Log("player cash decreased by " + decreaseBy);

    }

    /// <summary>
    /// Deletes and resets player data
    /// </summary>
    public void ClearPlayerData()
    {
        PlayerData.Clear();
        Debug.Log("player data cleared");

    }
#endregion

}
