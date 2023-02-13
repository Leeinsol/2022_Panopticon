using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(player_Controller))]
public class player_Controller_Editor : Editor
{
    player_Controller player_controller;

    private void OnEnable()
    {
        player_controller = (player_Controller)target;

    }

    public override void OnInspectorGUI()
    {
        //SerFPC.Update();

        //SerFPC.Update();

        //EditorGUILayout.LabelField("Walk Speed", test.walkSpeed.ToString());
        //EditorGUILayout.LabelField("FPS SHOOTING PACKAGE", EditorStyles.boldLabel)
        //GUILayout.Label("FPS SHOOTING PACKAGE",new GUIStyle() {fontSize=20,fontStyle=FontStyle.Bold,  alignment=TextAnchor.MiddleCenter});
        GUIStyle TitleStyle = new GUIStyle(GUI.skin.label);

        TitleStyle.normal.textColor = Color.yellow;
        TitleStyle.fontSize = 20;
        TitleStyle.fontStyle = FontStyle.Bold;
        TitleStyle.alignment = TextAnchor.MiddleCenter;

        GUIStyle DivisionStyle = new GUIStyle(GUI.skin.label);

        DivisionStyle.normal.textColor = Color.white;
        DivisionStyle.fontSize = 15;
        DivisionStyle.fontStyle = FontStyle.Bold;


        GUILayout.Space(20);


        GUILayout.Label("FPS SHOOTING PACKAGE", TitleStyle);
        GUILayout.Space(20);


        GUILayout.Label("WALK", DivisionStyle);

        //player_controller.walkSpeed = EditorGUILayout.FloatField("Walk Speed", player_controller.walkSpeed);
        player_controller.walkSpeed = EditorGUILayout.Slider("Walk Speed", player_controller.walkSpeed, 0, 100);
        player_controller.HeadBobAmount = EditorGUILayout.Vector3Field("Head Bob Amount", player_controller.HeadBobAmount);

        GUILayout.Space(15);

        GUILayout.Label("SPRINT", DivisionStyle);
        player_controller.useSprint = EditorGUILayout.Toggle("USe Sprint", player_controller.useSprint);
        EditorGUI.BeginDisabledGroup(!player_controller.useSprint);
        player_controller.SprintKey = (KeyCode)EditorGUILayout.EnumPopup("Sprint Key", player_controller.SprintKey);
        player_controller.sprintSpeed = EditorGUILayout.Slider("Sprint Speed", player_controller.sprintSpeed, 0, 100);
        EditorGUI.EndDisabledGroup();
        GUILayout.Space(15);

        GUILayout.Label("STAMINA", DivisionStyle);
        player_controller.useStaminaLimit = EditorGUILayout.Toggle("Use Stamina", player_controller.useStaminaLimit);
        EditorGUI.BeginDisabledGroup(!player_controller.useStaminaLimit);
        player_controller.StaminaBar = EditorGUILayout.ObjectField("Stamina Bar", player_controller.StaminaBar, typeof(GameObject), true) as GameObject;
        player_controller.maxStamina = EditorGUILayout.FloatField("Max Stamina", player_controller.maxStamina);
        EditorGUI.EndDisabledGroup();
        GUILayout.Space(15);

        GUILayout.Label("Crouch", DivisionStyle);
        player_controller.useCrouch = EditorGUILayout.Toggle("Use Crouch", player_controller.useCrouch);
        EditorGUI.BeginDisabledGroup(!player_controller.useCrouch);
        player_controller.CrouchKey = (KeyCode)EditorGUILayout.EnumPopup("Crouch Key", player_controller.CrouchKey);
        player_controller.crouchSpeed = EditorGUILayout.Slider("Crouch Speed", player_controller.crouchSpeed, 0, 100);
        player_controller.crouchHeight = EditorGUILayout.FloatField("Crouch Height", player_controller.crouchHeight);
        EditorGUI.EndDisabledGroup();
        GUILayout.Space(15);

        GUILayout.Label("Jump", DivisionStyle);
        player_controller.useJump = EditorGUILayout.Toggle("Use Jump", player_controller.useJump);
        EditorGUI.BeginDisabledGroup(!player_controller.useJump);
        player_controller.JumpKey = (KeyCode)EditorGUILayout.EnumPopup("Jump Key", player_controller.JumpKey);
        player_controller.jumpForce = EditorGUILayout.FloatField("Jump Force", player_controller.jumpForce);
        player_controller.groundCheckDistance = EditorGUILayout.FloatField("Ground Check Distance", player_controller.groundCheckDistance);
        EditorGUI.EndDisabledGroup();
        GUILayout.Space(15);

        GUILayout.Label("Camera", DivisionStyle);

    }
}
