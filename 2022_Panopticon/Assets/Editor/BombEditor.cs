using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Bomb))]
public class BombEditor : Editor
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Bomb script = (Bomb)target;

        if(script.rayHits!=null && script.rayHits.Length > 0)
        {
            EditorGUILayout.LabelField("Raycast Hits");
            foreach(RaycastHit hit in script.rayHits)
            {
                EditorGUILayout.ObjectField(hit.collider, typeof(Collider), true);
            }
        }
    }
}
