using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[RequireComponent(typeof(Rigidbody))]
public class PlayerControl : MonoBehaviour
{
    public GameObject player;

    public KeyCode SprintKey;
    public KeyCode CrouchKey = KeyCode.LeftControl;
    public KeyCode JumpKey = KeyCode.Space;
    public KeyCode ZoomKey = KeyCode.Mouse1;
    public KeyCode FireKey = KeyCode.Mouse0;
    public KeyCode ReloadKey = KeyCode.R;

    Player playerComponent;
    Command move;
    Command sprint;
    Command crouch;
    Command camVertical;
    Command camHorizontal;
    Command jump;
    Command headBob;
    Command zoomCamera;
    Command fire;
    Command ultimateFire;
    Command getItem;
    Command bomb;
    Command energy;
    Command reload;

    public List<InputKey> myKeyList;
    string filePath;

    // Start is called before the first frame update
    void Start()
    {
        playerComponent = player.GetComponent<Player>();
        move = new MoveCommand(player);
        sprint = new SprintCommand(player);
        crouch = new CrouchCommand(player);
        camVertical = new CamVerticalCommand(player);
        camHorizontal = new CamHorizontalCommand(player);
        jump= new JumpCommand(player);
        headBob= new HeadBobCommand(player);
        zoomCamera = new ZoomCameraCommand(player);
        fire = new FireCommand(player);
        ultimateFire = new UltimateFireCommand(player);
        getItem = new getItemCommand(player);
        bomb = new BombCommand(player);
        energy = new EnergyCommand(player);
        reload = new ReloadCommand(player);


        filePath = Application.persistentDataPath + "/MyKeyList.txt";

        Load();

        SprintKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), myKeyList[0].KeyCode);
        CrouchKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), myKeyList[1].KeyCode);
        JumpKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), myKeyList[2].KeyCode);
        ZoomKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), myKeyList[3].KeyCode);
        FireKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), myKeyList[4].KeyCode);
        ReloadKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), myKeyList[5].KeyCode);

    }
    
    // Update is called once per frame
    void Update()
    {
        MovementInput();
        SprintInput();
        CrouchInput();
        CameraInput();
        JumpInput();
        HeadBob();
        ZoomCameraInput();
        GunInput();
        ItemInput();
    }

    void MovementInput()
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
    }
 
    void SprintInput()
    {
        if (Input.GetKeyDown(SprintKey) && playerComponent.iswalking)
        {
            sprint.Enter();
        }
        if (Input.GetKeyUp(SprintKey))
        {
            sprint.End();
        }
    }

    void CrouchInput()
    {
        if (Input.GetKeyDown(CrouchKey))
        {
            crouch.Enter();
        }
        if (Input.GetKeyUp(CrouchKey))
        {
            crouch.End();
        }
    }

    void CameraInput()
    {
        if (playerComponent.useCameraRotationHorizontality && Time.timeScale > 0) 
            camVertical.Execute();
        if (playerComponent.useCameraRotationVerticality && Time.timeScale > 0) 
            camHorizontal.Execute();
    }

    void JumpInput()
    {
        if (Input.GetKeyDown(JumpKey) && playerComponent.isGround)
            jump.Enter();
    }

    void HeadBob()
    {
        if (playerComponent.iswalking) headBob.Execute();

    }

    void ZoomCameraInput()
    {
        if (Input.GetKey(ZoomKey)) 
            zoomCamera.Execute();
        if (Input.GetKeyUp(ZoomKey)) 
            zoomCamera.End();
    }

    void GunInput()
    {
        if (playerComponent.Weapon[0].activeSelf && Time.timeScale > 0)
        {
            if (playerComponent.ultimateGauge < playerComponent.ultimateNum)
            {
                if (Input.GetKeyDown(FireKey))
                {
                    fire.Set();
                }
                if (Input.GetKey(FireKey))
                {
                    fire.Execute();
                }

                if (playerComponent.bulletNum == 0 && !playerComponent.isReload)
                    reload.Set();

                if (playerComponent.useReload)
                {
                    if (Input.GetKeyDown(ReloadKey))
                    {
                        reload.Enter();
                    }
                    reload.Execute();
                }
            }
            else
            {
                ultimateFire.Set();
                if (Input.GetKey(FireKey))
                {
                    ultimateFire.Execute();
                }
                if (Input.GetKeyDown(FireKey))
                    fire.Set();

            }
        }
    }

    void ItemInput()
    {
        if (playerComponent.Weapon[1].activeSelf && Time.timeScale > 0)
        {
            if (Input.GetKeyDown(FireKey))
            {
                getItem.Enter();
            }
            if (Input.GetKeyUp(FireKey))
            {
                getItem.End();
            }

            if (Input.GetKey(FireKey))
            {
                getItem.Execute();
            }
        }
        if (playerComponent.Weapon[2].activeSelf && Time.timeScale > 0)
        {
            if (Input.GetKey(FireKey))
            {
                bomb.Execute();
            }
            if (Input.GetKeyUp(FireKey))
            {
                bomb.End();
            }
        }

        if (playerComponent.Weapon[3].activeSelf && Time.timeScale > 0)
        {
            energy.Execute();
            if (Input.GetKeyDown(FireKey))
            {
                energy.Enter();
            }
        }
    }

    void Load()
    {
        string jdata = File.ReadAllText(filePath);
        myKeyList = JsonUtility.FromJson<Serialization<InputKey>>(jdata).target;
    }
}
