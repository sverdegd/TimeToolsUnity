//TimeTools by SVerde
//contact@sverdegd.com

using UnityEngine;
using UnityEditor;

namespace SVerdeTools.TimeTools
{
    [InitializeOnLoad]
    public class TimeToolsUpdater
    {

        static TimeToolsUpdater()
        {
            Start();

            EditorApplication.update += Update;
        }

        static void Start()
        {
            ChronometerWindow.ClearData();
            TimerWindow.ClearData();
        }

        static void Update()
        {
            bool timerIsRunning = EditorPrefs.GetBool("TimeTools." + Application.productName + ".TimerIsRunning", false);
            if (timerIsRunning)
            {
                double timerTimeRemove = double.Parse(EditorPrefs.GetString("TimeTools." + Application.productName + ".TimerTimeRemove", "0"));

                double timerTime = TimerWindow.timeToWait - timerTimeRemove - (EditorApplication.timeSinceStartup - TimerWindow.startCoundownTime);
                if (timerTime <= 0)
                {
                    TimerWindow.FireAlarm();
                }
            }

        }
    }
}