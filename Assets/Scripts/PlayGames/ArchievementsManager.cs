using UnityEngine;

#if UNITY_ANDROID
using GooglePlayGames;
using GooglePlayGames.BasicApi;
#endif

public class ArchievementsManager : MonoBehaviour
{
#if UNITY_ANDROID

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
#endif 
}
