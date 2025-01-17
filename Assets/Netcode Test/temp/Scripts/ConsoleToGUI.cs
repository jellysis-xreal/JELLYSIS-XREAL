using UnityEngine;
     
namespace DebugStuff
{
    public class ConsoleToGUI : MonoBehaviour
    {
//#if !UNITY_EDITOR
        static string myLog = "";
        private string output;
        private string stack;

        void OnEnable()
        {
            Application.logMessageReceived += Log;
        }

        void OnDisable()
        {
            Application.logMessageReceived -= Log;
        }

        public void Log(string logString, string stackTrace, LogType type)
        {
            // Check if the logString contains the specific phrase you're looking for
            string Test = "[TEST]"; // Change this to the phrase you're looking for
            if (logString.Contains(Test))
            {
                output = logString;
                stack = stackTrace;
                myLog = output + "\n" + myLog;
                if (myLog.Length > 5000)
                {
                    myLog = myLog.Substring(0, 4000);
                }
            }
            // output = logString;
            // stack = stackTrace;
            // myLog = output + "\n" + myLog;
            // if (myLog.Length > 5000)
            // {
            //     myLog = myLog.Substring(0, 4000);
            // }
        }

        void OnGUI()
        {
            //if (!Application.isEditor) //Do not display in editor ( or you can use the UNITY_EDITOR macro to also disable the rest)
            {
                myLog = GUI.TextArea(new Rect(10, Screen.height * 2 / 3 , Screen.width / 2 , Screen.height / 3 - 10), myLog);
            }
        }
//#endif
    }
}