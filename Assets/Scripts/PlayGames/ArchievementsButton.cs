using UnityEngine;

#if UNITY_ANDROID
using GooglePlayGames;
using GooglePlayGames.BasicApi;
#endif

public class ArchievementsButton : MonoBehaviour
{
    public void ShowArchievements()
    {
#if UNITY_ANDROID
        PlayGamesPlatform.Instance.ShowAchievementsUI();
#endif
    }

    public void ShowLeaderboards()
    {
#if UNITY_ANDROID
        PlayGamesPlatform.Instance.ShowLeaderboardUI();
#endif
    }
}
