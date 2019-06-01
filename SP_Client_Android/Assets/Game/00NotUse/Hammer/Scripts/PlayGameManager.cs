using UnityEngine;
#if ENABLE_SOCIAL
using GooglePlayGames;
using GooglePlayGames.BasicApi;
#endif
public class PlayGameManager
{
    private static PlayGameManager sInstance = new PlayGameManager();

    PlayGameManager()
    {
#if ENABLE_SOCIAL
        // recommended for debugging:
        PlayGamesPlatform.DebugLogEnabled = true;
        // Activate the Google Play Games platform
        PlayGamesPlatform.Activate();
#endif
    }
    //private bool mAuthenticating = false;
    //private string mAuthProgressMessage = "Signing In.";

    public static PlayGameManager Instance
    {
        get
        {
            return sInstance;
        }
    }

    public void UnlockAchievement(string achId)
    {
#if ENABLE_SOCIAL
        if (Authenticated)
        {
            Social.ReportProgress(achId, 100.0f, (bool success) =>
                {
                });
        }
#endif
    }
    public void Authenticate()
    {
#if ENABLE_SOCIAL
        if (Authenticated || mAuthenticating)
        {
            Debug.LogWarning("Ignoring repeated call to Authenticate().");
            return;
        }

        // Enable/disable logs on the PlayGamesPlatform
        PlayGamesPlatform.DebugLogEnabled = true;

        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
            .EnableSavedGames()
            .Build();
        PlayGamesPlatform.InitializeInstance(config);

        // Activate the Play Games platform. This will make it the default
        // implementation of Social.Active
        PlayGamesPlatform.Activate();

        // Set the default leaderboard for the leaderboards UI
        ((PlayGamesPlatform)Social.Active).SetDefaultLeaderboardForUI(GPGSIds.leaderboard_top_scorers);

        // Sign in to Google Play Games
        mAuthenticating = true;
        Social.localUser.Authenticate((bool success) =>
            {
                mAuthenticating = false;
                if (success)
                {
                    // if we signed in successfully, load data from cloud
                    Debug.Log("Login successful!");
                }
                else
                {
                    // no need to show error message (error messages are shown automatically
                    // by plugin)
                    Debug.LogWarning("Failed to sign in with Google Play Games.");
                }
            });
#endif
    }
#if ENABLE_SOCIAL
    public bool Authenticating
    {
        get
        {
            return mAuthenticating;
        }
    }

    public bool Authenticated
    {
        get
        {
            return Social.Active.localUser.authenticated;
        }
    }

    public string AuthProgressMessage
    {
        get
        {
            return mAuthProgressMessage;
        }
    }
#endif
    public void ShowLeaderboardUI()
    {
#if ENABLE_SOCIAL
        if (Authenticated)
        {
            Social.ShowLeaderboardUI();
            //((PlayGamesPlatform)Social.Active).ShowLeaderboardUI(GPGSIds.leaderboard_top_scorers);
        }
        else
            Authenticate();
#endif
    }

    public void ShowAchievementsUI()
    {
#if ENABLE_SOCIAL
        if (Authenticated)
        {
            Social.ShowAchievementsUI();
        }
        else
            Authenticate();
#endif
    }

    public void PostToLeaderboard(int score)
    {
#if ENABLE_SOCIAL
        if (Authenticated)
        {
            // post score to the leaderboard
            Social.ReportScore(score, GPGSIds.leaderboard_top_scorers, (bool success) =>
                {
                    Debug.Log("Posted ? " + success);
                });
        }
        else
        {
            Authenticate();
            Debug.LogWarning("Not reporting score, auth = " + Authenticated);
        }
#endif
    }
}