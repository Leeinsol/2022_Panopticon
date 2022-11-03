using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    NavMeshAgent agent;

    //[SerializeField]
    //Transform target;

    Vector3 targetPos = new Vector3(0, 0, 0);

    //public static int hp = 5;
    public int maxHp = 5;
    public int hp;


    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        hp = maxHp;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        agent.isStopped = true;
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(targetPos);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("스페이스");
            agent.isStopped = false;
            animator.SetBool("isRun", true);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            agent.isStopped = true;
            animator.SetBool("isRun", false);
        }
        //animator.SetBool("isAttack", false);
        deadCheck();

    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.tag == "Player")
    //    {
    //        animator.SetBool("isAttack",true);
    //    }
    //}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            animator.SetBool("isAttack", true);
        }

    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            animator.SetBool("isAttack", false);
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.transform.parent != null
            && collision.gameObject.transform.parent.name == "tower")
        {
            GameObject ob = collision.gameObject.transform.parent.gameObject;
            if(ob.GetComponent<tower>().hp > 0)
            {
                ob.GetComponent<tower>().hp--;
                Debug.Log(ob.GetComponent<tower>().hp);
            }
        }

    }

    //private void OnCollisionStay(Collision collision)
    //{
    //    if (collision.gameObject.tag == "Player")
    //    {
    //        animator.SetTrigger("isAttack1");
    //    }
    //}

    public void playHurtAnim()
    {
        //animator.SetBool("isHurt", true);
        animator.SetTrigger("isHurt");
        
    }


    void deadCheck()
    {
        if (hp == 0)
        {
            //Debug.Log("hp 0");
            agent.isStopped = true;
            //Debug.Log(agent.isStopped);
            //animator.SetBool("isDie1", true);
            animator.SetTrigger("isDie");
            Destroy(this.gameObject, 2f);
        }
    }
}
