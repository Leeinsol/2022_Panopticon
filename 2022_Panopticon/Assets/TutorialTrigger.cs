    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : TutorialBase
{
    [SerializeField]
    GameObject player;
    [SerializeField]
    private Transform triggerObject;

    public bool isTrigger { set; get; } = false;

    public override void Enter()
    {
        triggerObject.gameObject.SetActive(true);
    }

    public override void Execute(TutorialController controller)
    {
        float distance = Vector3.Distance(triggerObject.position, player.transform.position);
        if (distance < 1.1f)
        {
            controller.SetNextTutorial();
        }
    }

    public override void Exit()
    {
        //triggerObject.gameObject.SetActive(false);
    }

}
