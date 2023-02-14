using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    NavMeshAgent agent;
    public RuntimeAnimatorController enemy_controller;
    
    //[SerializeField]
    //Transform target;

    Vector3 targetPos = new Vector3(0, 0, 0);

    //public static int hp = 5;
    public int maxHp = 5;
    public int hp;


    Animator animator;

    GameObject enemyModel;

    // Start is called before the first frame update
    void Start()
    {
        enemyModel = transform.GetChild(0).gameObject;
        hp = maxHp;
        agent = GetComponent<NavMeshAgent>();
        animator = enemyModel.GetComponent<Animator>();
        if (PlayerPrefs.GetInt("isCinemaEnd") == 1)
        {
            animator.runtimeAnimatorController = enemy_controller;
        }
        agent.enabled = false;
        agent.enabled = true;
        agent.isStopped = true;
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(targetPos);

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    GameObject.Find("CinemaCamera").GetComponent<cinema_moving>().isDoorOpen = true;
        //}
        if (GameObject.Find("CinemaCamera").GetComponent<cinema_moving>().isDoorOpen
            || PlayerPrefs.GetInt("isCinemaEnd") == 1)
        {
            //Debug.Log("�����̽�");
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
                //Debug.Log(ob.GetComponent<tower>().hp);
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
        agent.isStopped = true;
        animator.SetTrigger("isHurt");
        Invoke("playNav", 0.167f);
    }

    void playNav()
    {
        agent.isStopped = false;
    }

    void deadCheck()
    {
        if (hp == 0)
        {
            //Debug.Log("hp 0");
            agent.isStopped = true;
            //enemyModel.GetComponent<CapsuleCollider>().isTrigger = true;
            enemyModel.layer = 2;
            //Debug.Log(agent.isStopped);
            //animator.SetBool("isDie1", true);
            animator.SetTrigger("isDie");
            Destroy(this.gameObject, 2f);
        }
    }
}
