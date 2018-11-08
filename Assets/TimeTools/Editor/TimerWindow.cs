//TimeTools by SVerde
//contact@sverdegd.com

using UnityEngine;
using UnityEditor;

namespace SVerdeTools.TimeTools
{
    public class TimerWindow : EditorWindow
    {

        //Singleton
        public static TimerWindow window;
        //Window
        static GUIContent wTitle = new GUIContent();
        //GUI
        GUISkin skin;
        Rect sectionRect = new Rect();
        Texture2D sectionTexture;
        Color sectionColor = new Color(0.1f, 0.1f, 0.1f);
        //Data
        int hours = 0, minutes = 0, seconds = 0;
        double timerTime = 0;
        public static double timeToWait = 0;
        public static double startCoundownTime = 0;
        double timerTimePause = 0;

        [MenuItem("SVerdeTools/Time Tools/Timer", false, 1)]
        public static void OpenWindow()
        {
            if (window == null)
            {
                window = (TimerWindow)GetWindow(typeof(TimerWindow));

                wTitle.text = "Timer";
                wTitle.image = EditorGUIUtility.IconContent("UnityEditor.ProfilerWindow").image;
                wTitle.tooltip = "Use this tool as a usual timer";
                window.minSize = new Vector2(300f, 130f);
                window.maxSize = window.minSize;
                window.titleContent = wTitle;
            }

            window.Show();
        }

        void OnEnable()
        {
            skin = Resources.Load<GUISkin>("ClockSkin");

            sectionTexture = new Texture2D(1, 1);
            sectionTexture.SetPixel(0, 0, sectionColor);
            sectionTexture.Apply();
        }


        void OnGUI()
        {

            sectionRect.x = 0;
            sectionRect.y = 0;
            sectionRect.width = Screen.width;
            sectionRect.height = Screen.height;

            GUI.DrawTexture(sectionRect, sectionTexture);



            bool isRunning = EditorPrefs.GetBool("TimeTools." + Application.productName + ".TimerIsRunning", false);
            bool clear = EditorPrefs.GetBool("TimeTools." + Application.productName + ".TimerIsClear", true);
            double timerTimeRemove = double.Parse(EditorPrefs.GetString("TimeTools." + Application.productName + ".TimerTimeRemove", "0"));
            string lbl = string.Empty;
            string btn = string.Empty;
            float percent = 0;

            timerTime = timeToWait - timerTimeRemove - (EditorApplication.timeSinceStartup - startCoundownTime);

            if (clear)
            {
                lbl = "00:00:00";
                btn = "Start";
            }
            else if (isRunning)
            {
                lbl = TimeToolsUtils.TimeFormater(timerTime);
                btn = "Pause";
                percent = Mathf.FloorToInt((float)(timerTime / timeToWait * 100));
            }
            else
            {
                lbl = TimeToolsUtils.TimeFormater(timerTimePause);
                btn = "Continue";
                percent = Mathf.FloorToInt((float)(timerTimePause / timeToWait * 100));
            }


            EditorGUI.ProgressBar(new Rect(0, 0, position.width, 15), percent / 100, percent.ToString() + "%");
            GUILayout.Label(lbl, skin.GetStyle("Timer"));

            GUI.skin = null;


            EditorGUILayout.BeginHorizontal();
            hours = EditorGUILayout.IntField(hours);
            minutes = EditorGUILayout.IntField(minutes);
            seconds = EditorGUILayout.IntField(seconds);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUI.color = (isRunning) ? Color.yellow : GUI.color = Color.green;
            if (GUILayout.Button(btn, EditorStyles.miniButtonLeft, GUILayout.MaxWidth(Screen.width / 2)))
            {
                if (!isRunning)
                {
                    timeToWait = TimeToolsUtils.ToSeconds(hours, minutes, seconds);
                    if (timeToWait > 0)
                    {
                        EditorPrefs.SetBool("TimeTools." + Application.productName + ".TimerIsRunning", true);
                        if (clear)//start
                        {
                            startCoundownTime = EditorApplication.timeSinceStartup;
                            timerTimePause = 0;
                            EditorPrefs.SetString("TimeTools." + Application.productName + ".TimerTimeRemove", "0");
                            EditorPrefs.SetBool("TimeTools." + Application.productName + ".TimerIsClear", false);
                        }
                        else//continue
                        {
                            timerTimeRemove += timerTime - timerTimePause;
                            EditorPrefs.SetString("TimeTools." + Application.productName + ".TimerTimeRemove", timerTimeRemove.ToString());
                        }
                    }
                }
                else//pause
                {
                    EditorPrefs.SetBool("TimeTools." + Application.productName + ".TimerIsRunning", false);
                    timerTimePause = timerTime;
                }

            }
            GUI.color = Color.red;
            if (GUILayout.Button("Stop", EditorStyles.miniButtonRight, GUILayout.MaxWidth(Screen.width / 2)))
            {
                ClearData();
            }
            EditorGUILayout.EndHorizontal();
            GUI.color = Color.white;

            Repaint();
        }
        public static void ClearData()
        {
            EditorPrefs.SetBool("TimeTools." + Application.productName + ".TimerIsRunning", false);
            EditorPrefs.SetBool("TimeTools." + Application.productName + ".TimerIsClear", true);
            EditorPrefs.SetString("TimeTools." + Application.productName + ".TimerTimeRemove", "0");
        }

        public static void FireAlarm()
        {
            OpenWindow();
            EditorApplication.Beep();
            ClearData();
        }
    }
}
