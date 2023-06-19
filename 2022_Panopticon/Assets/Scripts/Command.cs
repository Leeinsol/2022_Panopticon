using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Command
{
    protected GameObject actor;

    public Command(GameObject actor)
    {
        this.actor = actor;
    }

    abstract public void Set();
    abstract public void Enter();
    abstract public void Execute();
    abstract public void End();
}

public class MoveCommand : Command
{
    public MoveCommand(GameObject actor) : base(actor) { }
    
    public override void Enter()
    {
        
    }
    public override void End() {

        actor.GetComponent<Player>().iswalking = false;
    }

    public override void Set()
    {
    }

    public override void Execute()
    {
        actor.GetComponent<Player>().Move();
    }
}

public class SprintCommand : Command
{
    public SprintCommand(GameObject actor) : base(actor) { }

    public override void Enter()
    {
        actor.GetComponent<Player>().isSprinting=true;
    }

    public override void End()
    {
        actor.GetComponent<Player>().endSprint();
    }
    public override void Set()
    {
    }

    public override void Execute()
    {
    }
}

public class CrouchCommand : Command
{
    public CrouchCommand(GameObject actor) : base(actor) { }

    public override void Enter()
    {
        actor.GetComponent<Player>().setCrouch();
    }

    public override void End()
    {
        actor.GetComponent<Player>().EndCrouch();
    }
    public override void Set()
    {
    }

    public override void Execute()
    {
    }
}

public class CamVerticalCommand : Command
{
    public CamVerticalCommand(GameObject actor) : base(actor) { }

    public override void Enter()
    {
    }
    public override void End()
    {

    }
    public override void Set()
    {
    }

    public override void Execute()
    {
        actor.GetComponent<Player>().CameraRotationVerticality();
    }
}
public class CamHorizontalCommand : Command
{
    public CamHorizontalCommand(GameObject actor) : base(actor) { }

    public override void Enter()
    {
    }

    public override void End()
    {
    }
    public override void Set()
    {
    }

    public override void Execute()
    {
        actor.GetComponent<Player>().CameraRotationHorizontality();
    }
}

public class JumpCommand : Command
{
    public JumpCommand(GameObject actor) : base(actor) { }

    public override void Enter()
    {
        actor.GetComponent<Player>().Jump();
    }

    public override void End()
    {
    }
    public override void Set()
    {
    }

    public override void Execute()
    {
    }
}


public class HeadBobCommand : Command
{
    public HeadBobCommand(GameObject actor) : base(actor) { }

    public override void Enter()
    {
    }

    public override void End()
    {
    }
    public override void Set()
    {
    }

    public override void Execute()
    {
        actor.GetComponent<Player>().HeadBob();
    }
}
public class ZoomCameraCommand : Command
{
    public ZoomCameraCommand(GameObject actor) : base(actor) { }

    public override void Enter()
    {
    }

    public override void End()
    {
        actor.GetComponent<Player>().EndZoomCamera();
    }
    public override void Set()
    {
    }

    public override void Execute()
    {
        actor.GetComponent<Player>().ZoomCamera();
    }
}

public class FireCommand : Command
{
    public FireCommand(GameObject actor) : base(actor) { }

    public override void Enter()
    {
    }

    public override void End()
    {
    }
    public override void Set()
    {
        actor.GetComponent<Player>().setFireTimer();
    }

    public override void Execute()
    {
        actor.GetComponent<Player>().Fire();
    }
}

public class UltimateFireCommand : Command
{
    public UltimateFireCommand(GameObject actor) : base(actor) { }

    public override void Enter()
    {
    }

    public override void End()
    {
    }
    public override void Set()
    {
        actor.GetComponent<Player>().setUltimateState();
    }

    public override void Execute()
    {
        actor.GetComponent<Player>().ultimateFire();
    }
}

public class getItemCommand : Command
{
    public getItemCommand(GameObject actor) : base(actor) { }

    public override void Enter()
    {
        actor.GetComponent<Player>().getItem();
    }

    public override void End()
    {
        actor.GetComponent<Player>().setGunOrigin();
    }
    public override void Set()
    {
    }

    public override void Execute()
    {
        actor.GetComponent<Player>().pullItemMotion();
        actor.GetComponent<Player>().setRemainItemUI(false);
    }
}

public class BombCommand : Command
{
    public BombCommand(GameObject actor) : base(actor) { }

    public override void Enter()
    {
    }

    public override void End()
    {
        actor.GetComponent<Player>().endBombFire();
    }
    public override void Set()
    {
    }

    public override void Execute()
    {
        actor.GetComponent<Player>().bombFire();
    }
}

public class EnergyCommand : Command
{
    public EnergyCommand(GameObject actor) : base(actor) { }

    public override void Enter()
    {
        actor.GetComponent<Player>().eatEnergyDrink();
        actor.GetComponent<Player>().RemainEnergyDrinkNum();
    }

    public override void End()
    {

    }
    public override void Set()
    {
    }

    public override void Execute()
    {
        actor.GetComponent<Player>().RemainEnergyDrinkNum();
        actor.GetComponent<Player>().setRemainItemUI(true);
    }
}

public class ReloadCommand : Command
{
    public ReloadCommand(GameObject actor) : base(actor) { }

    public override void Enter()
    {
        actor.GetComponent<Player>().PressReloadKey();
    }

    public override void End()
    {
    }
    public override void Set()
    {
        actor.GetComponent<Player>().SetReload();
    }

    public override void Execute()
    {
        actor.GetComponent<Player>().reloadBullet();
    }
}
