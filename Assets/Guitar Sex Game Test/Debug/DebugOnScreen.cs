using UnityEngine;
using System.Collections.Generic;

public class DebugOnScreen : MonoBehaviour
{
    private static Dictionary<string, string> debugData = new Dictionary<string, string>();
    private static object lockObj = new object();

    public static void Set(string heading, string message)
    {
        lock (lockObj)
        {
            debugData[heading] = message;
        }
    }

    public static void Remove(string heading)
    {
        lock (lockObj)
        {
            if (debugData.ContainsKey(heading))
                debugData.Remove(heading);
        }
    }

    public static void ClearAll()
    {
        lock (lockObj)
        {
            debugData.Clear();
        }
    }

    void OnGUI()
    {
        GUIStyle headingStyle = new GUIStyle(GUI.skin.label);
        headingStyle.fontSize = 18;
        headingStyle.fontStyle = FontStyle.Bold;
        headingStyle.normal.textColor = Color.yellow;

        GUIStyle messageStyle = new GUIStyle(GUI.skin.label);
        messageStyle.fontSize = 16;
        messageStyle.normal.textColor = Color.white;

        GUILayout.BeginArea(new Rect(10, 10, Screen.width - 20, Screen.height / 2));
        lock (lockObj)
        {
            foreach (var entry in debugData)
            {
                GUILayout.Label(entry.Key, headingStyle);
                GUILayout.Label(entry.Value, messageStyle);
                GUILayout.Space(6);
            }
        }
        GUILayout.EndArea();
    }
}

