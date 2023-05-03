    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : TutorialBase
{
    [SerializeField]
    player_Controller player_Controller;
    [SerializeField]
    private Transform triggerObject;

    public bool isTrigger { set; get; } = false;

    public override void Enter()
    {
        triggerObject.gameObject.SetActive(true);
    }

    public override void Execute(TutorialController controller)
    {
        if ((triggerObject.position - player_Controller.transform.position).sqrMagnitude < 0.1f)
        {
            controller.SetNextTutorial();
        }
    }

    public override void Exit()
    {
        triggerObject.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.Equals(triggerObject)){
            isTrigger = true;

            other.gameObject.SetActive(false);
        }
    }
}
