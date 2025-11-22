using UnityEngine;
using System;
using UnityEngine.SocialPlatforms.Impl;



#if UNITY_ANDROID
using GooglePlayGames;
using GooglePlayGames.BasicApi;
#endif

public class ArchievementsManager : MonoBehaviour
{
#if UNITY_ANDROID

    int archievementsCount = 0;

    private void Awake()
    {
        ServiceProvider.SetService(this);
    }

    private void Start()
    {
        PlayGamesPlatform.Instance.Authenticate(ProcessAuth);
    }

    private void ProcessAuth(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {
            Debug.Log("Logged in to Google Play Games Services");
            PlayGamesPlatform.Instance.IncrementAchievement(GPGSIds.achievement_firstopen, 5, (bool success) =>
            {
                Debug.Log("IncrementAchievement firstopen: " + success);
            });
        }
        else
            Debug.LogWarning("Failed to log in to Google Play Games Services: " + status);
    }

    public void ArchieveLevel(int level)
    {
        string achievementId = null;

        if (level == 1) achievementId = GPGSIds.achievement_level_1;
        else if (level == 2) achievementId = GPGSIds.achievement_level_2;
        else if (level == 3) achievementId = GPGSIds.achievement_level_3;
        else return;

        PlayGamesPlatform.Instance.IncrementAchievement(achievementId, 5, success =>
        {
            Debug.Log("IncrementAchievement level " + level + ": " + success);
            if (success) UpdateLeaderboard();
        });
    }

    private void UpdateLeaderboard()
    {
        archievementsCount++;

        PlayGamesPlatform.Instance.ReportScore(archievementsCount, GPGSIds.leaderboard_amount_of_archievements, success =>
        {
            Debug.Log($"ReportScore to {GPGSIds.leaderboard_amount_of_archievements}: {success} \n(score: {archievementsCount})");
        });
    }

#endif
}
