using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeToolsUtils {

    public static string TimeFormater(double timer)
    {
        float aux = Mathf.Floor((float)timer / 60);

        string hours = Mathf.Floor(aux / 60).ToString("00");
        string minutes = Mathf.Floor(aux % 60).ToString("00");
        string seconds = (timer % 60).ToString("00");

        return hours + ":" + minutes + ":" + seconds;
    }

    public static double ToSeconds(int hours = 0, int minutes = 0, int seconds = 0)
    {
        return (double)hours * 3600 + minutes * 60 + seconds;
    }
}
