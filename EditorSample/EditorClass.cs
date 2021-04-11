using UnityEngine;
using UnityEditor;


// À©Õ¹C#Àà·¶Àý

[CustomEditor(typeof(QuestBoss)), CanEditMultipleObjects]
public class QuestBossEditor : Editor
{
    SerializedProperty state_Prop;
    SerializedProperty rebirthOtherBoss_Prop;
    SerializedProperty isBirth_Prop;
    SerializedProperty rebirthCount_Prop;
    SerializedProperty rebirthVFX_Prop;

    void OnEnable()
    {
        // Setup the SerializedProperties
        state_Prop = serializedObject.FindProperty("rebirth");
        isBirth_Prop = serializedObject.FindProperty("IsBirth");
        rebirthOtherBoss_Prop = serializedObject.FindProperty("rebirthOtherBoss");
        rebirthCount_Prop = serializedObject.FindProperty("rebirthCount");
        rebirthVFX_Prop = serializedObject.FindProperty("rebirthVFX");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();
        EditorGUILayout.PropertyField(isBirth_Prop);

        if (isBirth_Prop.boolValue)
        {
            QuestBoss.Rebirth st = (QuestBoss.Rebirth)state_Prop.enumValueIndex;

            switch (st)
            {
                case QuestBoss.Rebirth.Oneself:
                    EditorGUILayout.PropertyField(state_Prop, new GUIContent("rebirth"));
                    break;

                case QuestBoss.Rebirth.Other:
                    EditorGUILayout.PropertyField(state_Prop, new GUIContent("rebirth"), true);
                    EditorGUILayout.PropertyField(rebirthOtherBoss_Prop, new GUIContent("rebirthOtherBoss"));
                    break;
            }
            EditorGUILayout.PropertyField(rebirthCount_Prop, new GUIContent("rebirthCount"));
            EditorGUILayout.PropertyField(rebirthVFX_Prop, new GUIContent("rebirthVFX"));
        }

        serializedObject.ApplyModifiedProperties();
    }
}