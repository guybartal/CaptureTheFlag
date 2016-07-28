using System;
using System.IO;
using UnityEngine;

public class ExceptionHandler : MonoBehaviour
{
    //private StreamWriter m_Writer;

    void Awake()
    {
        Application.logMessageReceived += Application_logMessageReceived;
        //m_Writer = new StreamWriter(Path.Combine(Application.dataPath, "unityexceptions.txt"));
       // m_Writer.AutoFlush = true;
    }

    private void Application_logMessageReceived(string condition, string stackTrace, LogType type)
    {
        switch (type)
        {
            case LogType.Error:
                Debug.LogErrorFormat("{0}: {1}\n{2}", type, condition, stackTrace);
                break;
            case LogType.Assert:
                Debug.LogAssertionFormat("{0}: {1}\n{2}", type, condition, stackTrace);
                break;
            case LogType.Warning:
                Debug.LogWarningFormat("{0}: {1}\n{2}", type, condition, stackTrace);
                break;
            case LogType.Log:
                Debug.LogFormat("{0}: {1}\n{2}", type, condition, stackTrace);
                break;
            case LogType.Exception:
                Debug.LogErrorFormat("{0}: {1}\n{2}", type, condition, stackTrace);
                break;
            default:
                break;
        }
        //m_Writer.WriteLine("{0}: {1}\n{2}", type, condition, stackTrace);
    }
}