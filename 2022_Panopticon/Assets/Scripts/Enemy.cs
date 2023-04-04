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
    public int hp= 5;

    public bool isAngry = false;
    public bool isDamaged = false;
    public int power = 1;

    Rigidbody rb;
    Animator animator;

    GameObject enemyModel;

    // Start is called before the first frame update
    void Start()
    {
        enemyModel = transform.GetChild(0).gameObject;
        //hp = maxHp;
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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject.Find("CinemaCamera").GetComponent<cinema_moving>().isDoorOpen = true;
        }
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
        //Debug.Log(agent.speed);
        //Debug.Log(hp);

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
        if (collision.gameObject.tag == "Player" || (collision.gameObject.transform.parent != null
            && collision.gameObject.transform.parent.name == "tower"))
        {
            animator.SetBool("isAttack", true);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player" || (collision.gameObject.transform.parent != null
            && collision.gameObject.transform.parent.name == "tower"))
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
                ob.GetComponent<tower>().hp-= power;
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
            transform.GetComponent<CapsuleCollider>().enabled = false;
            transform.GetComponent<SphereCollider>().enabled = false;
            enemyModel.layer = 2;
            //Debug.Log(agent.isStopped);
            //animator.SetBool("isDie1", true);
            animator.SetTrigger("isDie");
            //PlayerPrefs.SetInt("remainEnemy", PlayerPrefs.GetInt("remainEnemy")-1);

            transform.GetComponent<ItemManager>().instantiateItem(transform.position);

            Destroy(this.gameObject, 2f);
        }
    }

    void checkAngryState()
    {

        if (GameObject.Find("Canvas").GetComponent<BlinkingText>().hideWarning && !isAngry)
        {
            isAngry = true;
            SetAngry();
            //GameObject.Find("Canvas").GetComponent<BlinkingText>().hideWarning = false;
        }
    }
    void SetAngry()
    {
        //isCheck = true;
        agent.speed *= 2;
        hp *= 2;
        power *= 2;
    }

    public void HitByBomb(Vector3 BombPos)
    {
        //Vector3 reactVector = transform.position - BombPos;
        //rb.AddForce(reactVector.normalized * -1f * 5f);
        //if (hp <= 0)
        //{
        //    Destroy(gameObject);
        //}

        //hp -= 1;

        if (isDamaged) return;
        isDamaged = true;
        GameObject.FindWithTag("Player").GetComponent<player_Controller>().ultimateGauge++;
        Debug.Log(transform.parent.gameObject.name + "HitByBomb");
        Debug.Log(GameObject.FindWithTag("Player").GetComponent<player_Controller>().ultimateGauge);

        hp--;
        Vector3 reactVector = transform.position - BombPos;
        StartCoroutine(Damage(reactVector));
    }

    IEnumerator Damage(Vector3 reactVec)
    {
        yield return new WaitForSeconds(0.1f);

        reactVec = reactVec.normalized;
        reactVec += Vector3.up * 3;

        rb.freezeRotation = false;
        rb.AddForce(reactVec * 2, ForceMode.Impulse);
        rb.AddTorque(reactVec * 2, ForceMode.Impulse);
    }
}
