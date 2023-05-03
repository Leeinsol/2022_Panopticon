using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDestroyTagObjects : TutorialBase
{
    [SerializeField]
    player_Controller player_Controller;
    [SerializeField]
    private GameObject[] objectList;
    [SerializeField]
    string tagName;

    public override void Enter()
    {
        for (int i = 0; i < objectList.Length; i++)
        {
            objectList[i].SetActive(true);
        }
    }

    public override void Execute(TutorialController controller)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tagName);

        if (objects.Length == 0)
        {
            controller.SetNextTutorial();
        }
    }

    public override void Exit()
    {

    }
}
