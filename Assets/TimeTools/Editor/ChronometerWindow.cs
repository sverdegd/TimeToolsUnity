//TimeTools by SVerde
//contact@sverdegd.com

using UnityEngine;
using UnityEditor;

namespace SVerdeTools.TimeTools
{
    public class ChronometerWindow : EditorWindow
    {
        //Singleton
        public static ChronometerWindow window;
        //Window
        static GUIContent wTitle = new GUIContent();
        //GUI
        GUISkin skin;
        Rect sectionRect = new Rect();
        Texture2D sectionTexture;
        Color sectionColor = new Color(0.1f, 0.1f, 0.1f);
        //Data
        double chronometerTime = 0;
        double chronometerTimePause = 0;

        [MenuItem("SVerdeTools/Time Tools/Chronometer", false, 1)]
        public static void OpenWindow()
        {
            if (window == null)
            {
                window = (ChronometerWindow)GetWindow(typeof(ChronometerWindow));

                wTitle.text = "Chronometer";
                wTitle.image = EditorGUIUtility.IconContent("UnityEditor.AnimationWindow").image;
                wTitle.tooltip = "Use this tool as a usual chronometer";
                window.minSize = new Vector2(300f, 110f);
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

            bool isRunning = EditorPrefs.GetBool("TimeTools." + Application.productName + ".ChronometerIsRunning", false);
            bool clear = EditorPrefs.GetBool("TimeTools." + Application.productName + ".ChronometerIsClear", true);
            double chronometerTimeRemove = double.Parse(EditorPrefs.GetString("TimeTools." + Application.productName + ".ChronometerTimeRemove", "0"));
            string lbl = string.Empty;
            string btn = string.Empty;

            chronometerTime = EditorApplication.timeSinceStartup - chronometerTimeRemove;

            if (clear)
            {
                lbl = "00:00:00";
                btn = "Start";
            }
            else if (isRunning)
            {
                lbl = TimeToolsUtils.TimeFormater(chronometerTime);
                btn = "Pause";
            }
            else
            {
                lbl = TimeToolsUtils.TimeFormater(chronometerTimePause);
                btn = "Continue";
            }

            GUILayout.Label(lbl, skin.GetStyle("Chronometer"));


            EditorGUILayout.BeginHorizontal();
            GUI.color = (isRunning) ? Color.yellow : GUI.color = Color.green;
            if (GUILayout.Button(btn, EditorStyles.miniButtonLeft, GUILayout.MaxWidth(Screen.width / 2)))
            {
                if (isRunning)
                {
                    EditorPrefs.SetBool("TimeTools." + Application.productName + ".ChronometerIsRunning", false);
                    chronometerTimePause = chronometerTime;
                }
                else
                {
                    EditorPrefs.SetBool("TimeTools." + Application.productName + ".ChronometerIsRunning", true);
                    if (clear)
                    {
                        chronometerTimeRemove = EditorApplication.timeSinceStartup;
                        EditorPrefs.SetString("TimeTools." + Application.productName + ".ChronometerTimeRemove", chronometerTimeRemove.ToString());
                        chronometerTimePause = 0;
                        EditorPrefs.SetBool("TimeTools." + Application.productName + ".ChronometerIsClear", false);
                    }
                    else
                    {
                        chronometerTimeRemove += chronometerTime - chronometerTimePause;
                        EditorPrefs.SetString("TimeTools." + Application.productName + ".ChronometerTimeRemove", chronometerTimeRemove.ToString());
                    }
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
            EditorPrefs.SetBool("TimeTools." + Application.productName + ".ChronometerIsRunning", false);
            EditorPrefs.SetBool("TimeTools." + Application.productName + ".ChronometerIsClear", true);
            EditorPrefs.SetString("TimeTools." + Application.productName + ".ChronometerTimeRemove", "0");
        }
    }
}
