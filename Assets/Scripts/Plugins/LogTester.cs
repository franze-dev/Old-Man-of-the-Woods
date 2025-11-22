using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LogTester : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI _statusText;
    [SerializeField] private Button _clearLogsButton;
    [SerializeField] private Button _updateLogsButton;
    [SerializeField] private Button _testLogsButton;
    [SerializeField] private int _characterLimit = 500;

    private string currentMessage = "Here's where the logs are shown.";

    void Start()
    {
        _clearLogsButton.onClick.AddListener(ClearLogs);
        _updateLogsButton.onClick.AddListener(ShowLogs);
        _testLogsButton.onClick.AddListener(TestLog);

        UpdateStatus("Ready!");
    }

    void Update()
    {
        if (_statusText != null)
            _statusText.text = currentMessage;
    }

    void UpdateStatus(string message)
    {
        currentMessage += '\n' + message;
        Debug.Log($"UI: {message}");
    }

    public void ShowLogs()
    {
        string logs = AndroidLogManager.GetLogs();

        UpdateStatus("ALL SAVED LOGS\n" +
                      logs +
                      "\nEND LOGS");
    }

    public void ClearLogs()
    {
        AndroidLogManager.ClearLogs();
        ClearStatus();
    }

    private void ClearStatus()
    {
        currentMessage = "";
    }

    public void TestLog()
    {
        Debug.Log($"Test log at time: {Time.time}");
        ShowLogs();
    }

    [RuntimeInitializeOnLoadMethod]
    static void RegisterUnityLogCallback()
    {
        Application.logMessageReceived += HandleUnityLog;
    }

    private static void HandleUnityLog(string log, string stackTrace, LogType type)
    {
        AndroidLogManager.SendLog($"{type}: {log}");
    }
}

public static class AndroidLogManager
{
    private static AndroidJavaObject _logManagerInstance;
    private static AndroidJavaObject _currentActivity;
    private static AndroidJavaClass _unityPlayer;

    private static void Init()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (_logManagerInstance != null)
            return;

        try
        {
            _unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            _currentActivity = _unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

            AndroidJavaClass pluginClass = new AndroidJavaClass("com.example.logPlugin.LogManager");
            _logManagerInstance = pluginClass.CallStatic<AndroidJavaObject>("create", _currentActivity);
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to init log manager: " + e.Message);
        }
#endif
    }

    public static void SendLog(string message)
    {
        Init();
        _logManagerInstance?.Call("sendLog", message);
    }

    public static string GetLogs()
    {
        Init();
        return _logManagerInstance?.Call<string>("getLogs") ?? "Plugin not initialized. Could not get logs";
    }

    public static void ClearLogs()
    {
        Init();
        _logManagerInstance?.Call("clearLogs");
    }
}
