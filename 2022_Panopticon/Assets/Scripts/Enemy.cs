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

    //Vector3 targetPos = new Vector3(0, -6.5f, 0);
    //public GameObject targetPos;
    Vector3 destination;
    public Transform targetPos;
    public float towerRadius = 8.0f;
    private Vector3 currentDestination;
    private bool hasReachedDestination = false;

    //public static int hp = 5;
    public int maxHp = 5;
    public int hp;

    Rigidbody rb;
    Animator animator;

    GameObject enemyModel;

    // Start is called before the first frame update
    void Start()
    {
        enemyModel = transform.GetChild(0).gameObject;
        hp = maxHp;
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        //destination = agent.destination;
        SetDestination();
        animator = enemyModel.GetComponent<Animator>();
        if (PlayerPrefs.GetInt("isCinemaEnd") == 1)
        {
            animator.runtimeAnimatorController = enemy_controller;
        }

        agent.enabled = true;
        agent.isStopped = true;
    }
    // Update is called once per frame
    void Update()
    {
        //rb.velocity = Vector3.zero;
        agent.isStopped = false;
        //SetDestination();

        if (!hasReachedDestination && agent.remainingDistance <= agent.stoppingDistance)
        {
            hasReachedDestination = true;
            SetDestination();
        }

        //agent.SetDestination(targetPos.transform.position);
        //agent.destination = destination;
        //agent.destination = targetPos.transform.position;
        //agent.destination = targetPos;

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
        checkAngryState();


    }

    void SetDestination()
    {
        Vector3 directionToCenter = targetPos.position - transform.position;
        directionToCenter.Normalize();

        Vector3 destination = targetPos.position + directionToCenter * towerRadius;

        agent.SetDestination(destination);

        currentDestination = destination;
        hasReachedDestination = false;
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
        if (hp < 1)
        {
            //Debug.Log("hp 0");
            agent.isStopped = true;
            //enemyModel.GetComponent<CapsuleCollider>().isTrigger = true;
            //transform.GetComponent<CapsuleCollider>().enabled = false;
            enemyModel.layer = 2;
            //Debug.Log(agent.isStopped);
            //animator.SetBool("isDie1", true);
            animator.SetTrigger("isDie");
            Destroy(this.gameObject, 2f);
        }
    }

    void checkAngryState()
    {
        if (GameObject.Find("tower").GetComponent<tower>().isHalfHP)
        {
            agent.speed *= 2;
            hp *= 2;
        }
    }
}
