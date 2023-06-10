using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerControl : MonoBehaviour
{
    public GameObject player;

    public KeyCode SprintKey = KeyCode.LeftShift;
    public KeyCode CrouchKey = KeyCode.LeftControl;
    public KeyCode JumpKey = KeyCode.Space;
    public KeyCode ZoomKey = KeyCode.Mouse1;
    public KeyCode FireKey = KeyCode.Mouse0;
    public KeyCode ReloadKey = KeyCode.R;

    Player playerScript;
    Command move;
    Command sprint;
    Command crouch;
    Command camVertical;
    Command camHorizontal;
    Command jump;
    Command headBob;
    Command zoomCamera;


    // Start is called before the first frame update
    void Start()
    {
        //player = new Player();
        playerScript = player.GetComponent<Player>();
        move = new MoveCommand(player);
        sprint = new SprintCommand(player);
        crouch = new CrouchCommand(player);
        camVertical = new CamVerticalCommand(player);
        camHorizontal = new CamHorizontalCommand(player);
        jump= new JumpCommand(player);
        headBob= new HeadBobCommand(player);
        zoomCamera = new ZoomCameraCommand(player);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A)
            || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            move.Execute();
        }
        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.A)
            || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D))
        {
            move.End();
        }

        if (Input.GetKeyDown(SprintKey) && playerScript.iswalking)
        {
            sprint.Execute();
        }
        if (Input.GetKeyUp(SprintKey))
        {
            sprint.End();
        }

        if (Input.GetKeyDown(CrouchKey))
        {
            crouch.Execute();
        }
        if (Input.GetKeyUp(CrouchKey))
        {
            crouch.End();
        }

        if (playerScript.useCameraRotationHorizontality && Time.timeScale > 0) camVertical.Execute();
        if (playerScript.useCameraRotationHorizontality && Time.timeScale > 0) camHorizontal.Execute();
        if (Input.GetKeyDown(JumpKey) && playerScript.isGround)   jump.Execute();
        if (playerScript.iswalking) headBob.Execute();
        if (Input.GetKey(ZoomKey)) zoomCamera.Execute();
        if (Input.GetKeyUp(ZoomKey)) zoomCamera.End();

        if (playerScript.Weapon[0].activeSelf && Time.timeScale > 0)
        {

        }
    }
}
