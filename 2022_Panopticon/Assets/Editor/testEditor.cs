using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(test2))]
//[CanEditMultipleObjects]
public class testEditor : Editor
{

    SerializedProperty lookAtPoint;

    void OnEnable()
    {
        lookAtPoint = serializedObject.FindProperty("lookAtPoint");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(lookAtPoint);
        serializedObject.ApplyModifiedProperties();
    }
}
