using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMovement : TutorialBase
{
    [SerializeField]
    RectTransform rectTransform;
    [SerializeField]
    private Vector3 endPosition;
    bool isCompleted = false;

    public override void Enter()
    {
        StartCoroutine(Movement());
    }

    public override void Execute(TutorialController controller)
    {
        if (isCompleted)
        {
            controller.SetNextTutorial();
        }
    }

    public override void Exit()
    {

    }

    IEnumerator Movement()
    {
        float current = 0;
        float percent = 0;
        float moveTime = 0.5f;
        Vector3 start = rectTransform.anchoredPosition;

        while (percent < 1) 
        {
            current += Time.deltaTime;
            percent = current / moveTime;

            rectTransform.anchoredPosition = Vector3.Lerp(start, endPosition, percent);

            yield return null;
        }
        isCompleted = true;
    }
}
