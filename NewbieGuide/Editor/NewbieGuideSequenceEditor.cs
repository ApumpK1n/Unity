using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(NewbieGuideSequence))]
public class NewbieGuideSequenceEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("刷新Mask"))
        { 
            NewbieGuideSequence.Instance.RefreshMask();
        }
    }
}