using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

        GUIStyle ContentStyle = new GUIStyle(GUI.skin.label);
        ContentStyle.normal.textColor = Color.white;
        ContentStyle.fontSize = 12;
        ContentStyle.alignment = TextAnchor.MiddleCenter;

        GUIStyle DivisionStyle = new GUIStyle(GUI.skin.label);

        DivisionStyle.normal.textColor = Color.white;
        DivisionStyle.fontSize = 15;
        DivisionStyle.fontStyle = FontStyle.Bold;


        GUILayout.Space(20);


        GUILayout.Label("FPS SHOOTING PACKAGE", TitleStyle);
        GUILayout.Label("by dt19iill", ContentStyle);
        GUILayout.Label("update 2023.02.14", ContentStyle);
        GUILayout.Space(20);


        GUILayout.Label("WALK", DivisionStyle);

        //player_controller.walkSpeed = EditorGUILayout.FloatField("Walk Speed", player_controller.walkSpeed);
        player_controller.walkSpeed = EditorGUILayout.Slider("Walk Speed", player_controller.walkSpeed, 0, 100);
        player_controller.HeadBobAmount = EditorGUILayout.Vector3Field("Head Bob Amount", player_controller.HeadBobAmount);

        GUILayout.Space(15);

        GUILayout.Label("SPRINT", DivisionStyle);
        player_controller.useSprint = EditorGUILayout.Toggle("Use Sprint", player_controller.useSprint);
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

        GUILayout.Label("Head Bob", DivisionStyle);
        player_controller.useHeadBob = EditorGUILayout.Toggle("Use Head Bob", player_controller.useHeadBob);
        EditorGUI.BeginDisabledGroup(!player_controller.useHeadBob);
        player_controller.headBobSpeed = EditorGUILayout.FloatField("Head Bob Speed", player_controller.headBobSpeed);
        EditorGUI.EndDisabledGroup();
        GUILayout.Space(15);

        GUILayout.Label("Jump", DivisionStyle);
        player_controller.useJump = EditorGUILayout.Toggle("Use Jump", player_controller.useJump);
        EditorGUI.BeginDisabledGroup(!player_controller.useJump);
        player_controller.JumpKey = (KeyCode)EditorGUILayout.EnumPopup("Jump Key", player_controller.JumpKey);
        player_controller.jumpForce = EditorGUILayout.FloatField("Jump Force", player_controller.jumpForce);
        //player_controller.groundCheckDistance = EditorGUILayout.FloatField("Ground Check Distance", player_controller.groundCheckDistance);
        EditorGUI.EndDisabledGroup();
        GUILayout.Space(15);

        GUILayout.Label("Camera", DivisionStyle);
        player_controller.useCameraRotationVerticality = EditorGUILayout.Toggle("Use Camera Rotation Verticality", player_controller.useCameraRotationVerticality);
        player_controller.useCameraRotationHorizontality = EditorGUILayout.Toggle("Use Camera Rotation Horizontality", player_controller.useCameraRotationHorizontality);
        EditorGUI.BeginDisabledGroup(!player_controller.useCameraRotationVerticality && !player_controller.useCameraRotationHorizontality);
        player_controller.theCamera = EditorGUILayout.ObjectField("Camera", player_controller.theCamera, typeof(Camera), true) as Camera;
        player_controller.lookSensitivity = EditorGUILayout.FloatField("Look Sensitivity", player_controller.lookSensitivity);
        player_controller.cameraRotationLimit = EditorGUILayout.FloatField("Camera Rotation Limit", player_controller.cameraRotationLimit);
        EditorGUI.EndDisabledGroup();
        GUILayout.Space(15);

        GUILayout.Label("Zoom", DivisionStyle);
        player_controller.useCameraZoom = EditorGUILayout.Toggle("use Camera Zoom", player_controller.useCameraZoom);
        EditorGUI.BeginDisabledGroup(!player_controller.useCameraZoom);
        player_controller.ZoomKey = (KeyCode)EditorGUILayout.EnumPopup("Zoom Key", player_controller.ZoomKey);
        player_controller.zoomSpeed = EditorGUILayout.FloatField("Zoom Speed", player_controller.zoomSpeed);
        EditorGUI.EndDisabledGroup();
        GUILayout.Space(15);

        GUILayout.Label("Gun", DivisionStyle);
        player_controller.useGun = EditorGUILayout.Toggle("Use Gun", player_controller.useGun);
        EditorGUI.BeginDisabledGroup(!player_controller.useGun);
        player_controller.GunModel = EditorGUILayout.ObjectField("Gun Model", player_controller.GunModel, typeof(GameObject), true) as GameObject;
        player_controller.GunRotationSpeed = EditorGUILayout.FloatField("Gun Rotation Speed", player_controller.GunRotationSpeed);
        EditorGUI.EndDisabledGroup();
        GUILayout.Space(15);

        GUILayout.Label("Fire", DivisionStyle);
        EditorGUI.BeginDisabledGroup(!player_controller.useGun);
        player_controller.FireKey = (KeyCode)EditorGUILayout.EnumPopup("Fire Key", player_controller.FireKey);
        player_controller.bulletEffect = EditorGUILayout.ObjectField("Bullet Effect", player_controller.bulletEffect, typeof(GameObject), true) as GameObject;
        player_controller.maxBulletNum = EditorGUILayout.IntField("Max Bullet Num", player_controller.maxBulletNum);
        player_controller.fireRate = EditorGUILayout.FloatField("Fire Rate", player_controller.fireRate);
        EditorGUI.EndDisabledGroup();
        GUILayout.Space(15);

        GUILayout.Label("Canvas", DivisionStyle);
        EditorGUI.BeginDisabledGroup(!player_controller.useGun);
        player_controller.crossHairText = EditorGUILayout.ObjectField("Cross Hair Text", player_controller.crossHairText, typeof(Text), true) as Text;
        player_controller.crosshairtype = (CrossHairType)EditorGUILayout.EnumPopup("Cross Hair Type", player_controller.crosshairtype);
        player_controller.BulletNumUI = EditorGUILayout.ObjectField("Bullet Num UI", player_controller.BulletNumUI, typeof(GameObject), true) as GameObject;
        player_controller.ReladTimerUI = EditorGUILayout.ObjectField("Relad Timer UI", player_controller.ReladTimerUI, typeof(GameObject), true) as GameObject;
        EditorGUI.EndDisabledGroup();
        GUILayout.Space(15);

        GUILayout.Label("Reload", DivisionStyle);
        player_controller.useReload = EditorGUILayout.Toggle("Use Reload", player_controller.useReload);
        EditorGUI.BeginDisabledGroup(!player_controller.useReload);
        player_controller.ReloadKey = (KeyCode)EditorGUILayout.EnumPopup("Reload Key", player_controller.ReloadKey);
        player_controller.reloadType = (reloadBulletType)EditorGUILayout.EnumPopup("Reload Type", player_controller.reloadType);
        player_controller.maxReloadTime = EditorGUILayout.FloatField("Max Reload Time", player_controller.maxReloadTime);
        player_controller.allReloadTime = EditorGUILayout.FloatField("All Reload Time", player_controller.allReloadTime);
        player_controller.reloadActionForce = EditorGUILayout.FloatField("Reload Action Force", player_controller.reloadActionForce);
        EditorGUI.EndDisabledGroup();
        GUILayout.Space(15);

        GUILayout.Label("Sound", DivisionStyle);
        player_controller.useFireSound = EditorGUILayout.Toggle("Use Fire Sound", player_controller.useFireSound);
        EditorGUI.BeginDisabledGroup(!player_controller.useFireSound);
        player_controller.FireSound = EditorGUILayout.ObjectField("Fire Sound", player_controller.FireSound, typeof(AudioClip), true) as AudioClip;
        EditorGUI.EndDisabledGroup();
        GUILayout.Space(5);

        player_controller.useReloadSound = EditorGUILayout.Toggle("Use Reload Sound", player_controller.useReloadSound);
        EditorGUI.BeginDisabledGroup(!player_controller.useReloadSound);
        player_controller.oneByOneReloadSound = EditorGUILayout.ObjectField("One By One Reload Sound", player_controller.oneByOneReloadSound, typeof(AudioClip), true) as AudioClip;
        player_controller.allReloadSound = EditorGUILayout.ObjectField("All Reload Sound", player_controller.allReloadSound, typeof(AudioClip), true) as AudioClip;
        EditorGUI.EndDisabledGroup();
    }
}
