using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Command
{
    //protected Transform actor;

    //public Command(Transform actor)
    //{
    //    this.actor = actor;
    //}
    abstract public void Execute();
    abstract public void End();

}

public class MoveCommand : Command
{
    private GameObject player;

    public MoveCommand(GameObject player) { this.player = player; }
    
    public override void Execute()
    {
        player.GetComponent<Player>().Move();
    }
    public override void End() {
   
        player.GetComponent<Player>().iswalking = false;

    }

}

public class SprintCommand : Command
{
    private GameObject player;

    public SprintCommand(GameObject player) { this.player = player; }

    public override void Execute()
    {
        player.GetComponent<Player>().isSprinting=true;
    }

    public override void End()
    {
        player.GetComponent<Player>().endSprint();
    }
}

public class CrouchCommand : Command
{
    private GameObject player;

    public CrouchCommand(GameObject player) { this.player = player; }

    public override void Execute()
    {
        player.GetComponent<Player>().setCrouch();
    }

    public override void End()
    {
        player.GetComponent<Player>().EndCrouch();
    }
}

public class CamVerticalCommand : Command
{
    private GameObject player;

    public CamVerticalCommand(GameObject player) { this.player = player; }

    public override void Execute()
    {
        player.GetComponent<Player>().CameraRotationVerticality();
    }

    public override void End()
    {

    }
}
public class CamHorizontalCommand : Command
{
    private GameObject player;

    public CamHorizontalCommand(GameObject player) { this.player = player; }

    public override void Execute()
    {
        player.GetComponent<Player>().CameraRotationHorizontality();
    }

    public override void End()
    {

    }
}

public class JumpCommand : Command
{
    private GameObject player;

    public JumpCommand(GameObject player) { this.player = player; }

    public override void Execute()
    {
        player.GetComponent<Player>().Jump();

    }

    public override void End()
    {

    }
}


public class HeadBobCommand : Command
{
    private GameObject player;

    public HeadBobCommand(GameObject player) { this.player = player; }

    public override void Execute()
    {
        player.GetComponent<Player>().HeadBob();

    }

    public override void End()
    {

    }
}
public class ZoomCameraCommand : Command
{
    private GameObject player;

    public ZoomCameraCommand(GameObject player) { this.player = player; }

    public override void Execute()
    {
        player.GetComponent<Player>().ZoomCamera();

    }

    public override void End()
    {
        player.GetComponent<Player>().EndZoomCamera();

    }
}

public class FireCommand : Command
{
    private GameObject player;

    public FireCommand(GameObject player) { this.player = player; }

    public override void Execute()
    {
        player.GetComponent<Player>().Fire();

    }

    public override void End()
    {

    }
}

public class UltimateFireCommand : Command
{
    private GameObject player;

    public UltimateFireCommand(GameObject player) { this.player = player; }

    public override void Execute()
    {
        player.GetComponent<Player>().ultimateFire();

    }

    public override void End()
    {

    }
}

public class getItemCommand : Command
{
    private GameObject player;

    public getItemCommand(GameObject player) { this.player = player; }

    public override void Execute()
    {
        player.GetComponent<Player>().getItem();

    }

    public override void End()
    {
        player.GetComponent<Player>().setGunOrigin();

    }
}

public class BombCommand : Command
{
    private GameObject player;

    public BombCommand(GameObject player) { this.player = player; }

    public override void Execute()
    {
        player.GetComponent<Player>().bombFire();

    }

    public override void End()
    {
        player.GetComponent<Player>().endBombFire();

    }
}

public class EnergyCommand : Command
{
    private GameObject player;

    public EnergyCommand(GameObject player) { this.player = player; }

    public override void Execute()
    {
        player.GetComponent<Player>().eatEnergyDrink();
        player.GetComponent<Player>().RemainEnergyDrinkNum();
    }

    public override void End()
    {

    }
}