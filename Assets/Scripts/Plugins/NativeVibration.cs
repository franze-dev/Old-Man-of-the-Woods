//using UnityEngine;

//public class NativeVibration : MonoBehaviour
//{
//    private static bool isInitialized = false;

//#if UNITY_ANDROID
//    private static AndroidJavaClass _pluginClass;

//    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
//    private static void Init()
//    {
//        if (isInitialized)
//            return;

//        try
//        {
//            AndroidJavaClass unityPlayer = new("com.unity3d.player.UnityPlayer");
//            AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
//            AndroidJavaObject context = activity.Call<AndroidJavaObject>("getApplicationContext");
//            _pluginClass = new AndroidJavaClass("com.example.vibrationlib.VibratorPlugin");

//            _pluginClass.CallStatic("init", context);

//            isInitialized = true;
//            Debug.Log("Vibration plugin initialized");
//        }
//        catch (System.Exception e)
//        {
//            Debug.LogError("Vibration init. failed: " + e.Message);
//        }
        
//    }
//#endif

//    public static void Vibrate(long milliSeconds)
//    {
//#if UNITY_ANDROID
//        if (!isInitialized)
//            return;

//        try
//        {
//            _pluginClass ??= new AndroidJavaClass("com.example.vibrationlib.VibratorPlugin");

//            _pluginClass.CallStatic("vibrate", milliSeconds);
//        }
//        catch (System.Exception e)
//        {
//            Debug.LogError("Vibration failed: " + e.Message);
//        }
//#endif
//    }
//}
