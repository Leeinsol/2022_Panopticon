using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    Walk, Jump, Sprint, Crouch, Attack, Zoom, ChangeWeapon, Item
}
public class TutorialMovement : TutorialBase
{
    [SerializeField]
    RectTransform rectTransform;
    [SerializeField]
    private Vector3 endPosition;
    bool isCompleted = false;
    //public bool WInput = false;
    //public bool AInput = false;
    //public bool SInput = false;
    //public bool DInput = false;
    public PlayerState playerState;
    public GameObject player;
    float Timer;
    int num;

    public override void Enter()
    {
        if (playerState == PlayerState.Jump) StartCoroutine(Jump());
        else if (playerState == PlayerState.Attack) StartCoroutine(Attack());
        else if (playerState == PlayerState.ChangeWeapon) StartCoroutine(ChanegeWeapon());
        else if (playerState == PlayerState.Item) StartCoroutine(Item());
        else StartCoroutine(Movement());
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
        
        while (Timer < 5f)
        {
            Debug.Log("Timer: "+Timer);
            if (playerState == PlayerState.Walk && player.GetComponent<player_Controller>().iswalking)
            {
                Timer += Time.deltaTime;
            }
            if (playerState == PlayerState.Sprint && player.GetComponent<player_Controller>().isSprinting)
            {
                Timer += Time.deltaTime;
            }
            if (playerState == PlayerState.Crouch && player.GetComponent<player_Controller>().isCrouching)
            {
                Timer += Time.deltaTime;
            }
            if (playerState == PlayerState.Zoom && player.GetComponent<player_Controller>().isZooming)
            {
                Timer += Time.deltaTime;
            }


            yield return null;
        }
        isCompleted = true;

    }

    IEnumerator Jump()
    {
        while(Timer < 2.5f)
        {
            Debug.Log("Timer: "+Timer);
            if (!player.GetComponent<player_Controller>().isGround)
            {
                Timer += Time.deltaTime;
            }
            yield return null;
        }
        isCompleted = true;
    }

    IEnumerator Attack()
    {
        while(player.GetComponent<player_Controller>().maxBulletNum-3 < player.GetComponent<player_Controller>().bulletNum)
        {
            Debug.Log("num: " + player.GetComponent<player_Controller>().bulletNum);
            yield return null;
        }
        isCompleted = true;
    }

    IEnumerator ChanegeWeapon()
    {
        while (true)
        {
            Vector2 scrollDelta = Input.mouseScrollDelta;
            if (scrollDelta.y != 0)
            {
                isCompleted = true;
            }
            yield return null;

        }
    }
    
    IEnumerator Item()
    {
        while (true)
        {
            if (Input.GetMouseButtonUp(0))
            {
                isCompleted = true;
            }
            yield return null;

        }
    }

}
