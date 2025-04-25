using System.IO;
using UnityEngine;

public class Logger : MonoBehaviour
{
    private string logPath;

    void Awake()
    {
        logPath = Path.Combine(Application.persistentDataPath, "EmotionalVibrationLog.txt");
        Log("---- App Start ----");
    }

    public void Log(string message)
    {
        string timestamp = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        string fullMessage = $"[{timestamp}] {message}\n";

        File.AppendAllText(logPath, fullMessage);
        Debug.Log(fullMessage);
    }
}