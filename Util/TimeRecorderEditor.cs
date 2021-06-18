using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(TimeRecorder))]
public class TimeRecorderEditor : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Set end of day date"))
        {
            TimeRecorder timeController = (TimeRecorder)target;
            timeController.SetTimeToEndOfDay(1);
        }

        if (GUILayout.Button("Add 60 seconds"))
        {
            TimeRecorder timeController = (TimeRecorder)target;
            timeController.AddExtraSeconds(60);
        }

        if (GUILayout.Button("Add 3600 seconds"))
        {
            TimeRecorder timeController = (TimeRecorder)target;
            timeController.AddExtraSeconds(3600);
        }
    }
}