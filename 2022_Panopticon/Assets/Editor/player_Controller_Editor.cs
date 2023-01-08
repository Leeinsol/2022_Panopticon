//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;

//[CustomEditor(typeof(player_Controller))]
//public class player_Controller_Editor : Editor
//{
//    player_Controller player_controller;

//    private void OnEnable()
//    {
//        player_controller = (player_Controller)target;
//    }

//    public override void OnInspectorGUI()
//    {
//        //SerFPC.Update();

//        //SerFPC.Update();

//        //EditorGUILayout.LabelField("Walk Speed", test.walkSpeed.ToString());
//        player_controller.walkSpeed = EditorGUILayout.FloatField("Walk Speed", player_controller.walkSpeed);
//        player_controller.SprintKey = (KeyCode)EditorGUILayout.EnumPopup(new GUIContent("Sprint Key", "Determines what key is used to zoom."), player_controller.SprintKey);

//    }
//}

////[CustomEditor(typeof(player_Controller))]
////public class player_Controller_Editor:Editor
////{


////    public override void OnInspectorGUI()
////    {
////        base.OnInspectorGUI();

////        player_Controller test = (player_Controller)target;
////        //EditorGUILayout.LabelField("Walk Speed", test.walkSpeed.ToString());
////        //test.SprintKey = (KeyCode)EditorGUILayout.EnumPopup(new GUIContent("Sprint Key", "Determines what key is used to zoom."), test.SprintKey);

////    }
////}