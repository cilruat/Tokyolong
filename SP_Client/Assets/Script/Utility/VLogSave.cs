using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPLog
{
    public LogType type;
    public string condition;
    public string stackTrace;

    public SPLog(LogType type, string condition, string stackTrace)
    {
        this.type = type;
        this.condition = condition;
        this.stackTrace = stackTrace;
    }
}

public class VLogSave
{
    const int MAX_SAVE = 100;
    public static int lastIndex = -1;
    public static SPLog[] logs = new SPLog[MAX_SAVE];

    public delegate void OnSendLog (int index, SPLog log);
    public static OnSendLog onSendLog = null;

    static bool start = false;

    public static void Start()
    {
        if (start)
            return;

        start = true;
        Application.logMessageReceived += CallBackLog;
    }

    static void CallBackLog(string condition, string stackTrace, LogType type)
    {
        ++lastIndex;
        if (lastIndex >= MAX_SAVE)
            lastIndex = 0;

        SPLog newLog = new SPLog(type, condition, stackTrace);
        logs[lastIndex] = newLog;

        if (onSendLog != null)
            onSendLog(lastIndex, newLog);
    }
}
