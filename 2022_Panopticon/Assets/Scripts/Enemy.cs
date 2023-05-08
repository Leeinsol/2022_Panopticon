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

    bool isInstantiate = false;
    int RandomNum = 0;
    float Timer;

    public AudioClip[] hurtSound = new AudioClip[3];
    public AudioClip deathSound;
    AudioSource audioSource;

    public GameObject bloodEffect;
    public GameObject deathEffect;

    float walkSpeed;
    float runSpeed;


    // Start is called before the first frame update
    void Start()
    {
        if (StageSetting.Instance.getStage() == "easyMode")
        {
            walkSpeed = 0.4f;
            runSpeed = 0.8f;
            hp = 3;
        }
        else if (StageSetting.Instance.getStage() == "normalMode")
        {
            walkSpeed = 0.5f;
            runSpeed = 1f;
            hp = 3;
        }
        else if (StageSetting.Instance.getStage() == "hardMode")
        {
            walkSpeed = 0.5f;
            runSpeed = 1f;
            hp = 5;
        }

        enemyModel = transform.GetChild(0).gameObject;
        //hp = maxHp;
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
        //destination = agent.destination;
        SetDestination();
        animator = enemyModel.GetComponent<Animator>();
        if (PlayerPrefs.GetInt("isCinemaEnd") == 1)
        {
            animator.runtimeAnimatorController = enemy_controller;
        }

        agent.enabled = true;
        agent.isStopped = true;

        setRandomState();
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
            //SetDestination();
            StartCoroutine(SetDestination());
        }

        //agent.SetDestination(targetPos.transform.position);
        //agent.destination = destination;
        //agent.destination = targetPos.transform.position;
        //agent.destination = targetPos;

        if (GameObject.Find("CinemaCamera") != null)
        {
            if (GameObject.Find("CinemaCamera").GetComponent<cinema_moving>().isDoorOpen
            || PlayerPrefs.GetInt("isCinemaEnd") == 1)
            {
                //Debug.Log("�����̽�");
                agent.isStopped = false;
                animator.SetBool("isRun", true);
            }
        }
        


        //animator.SetBool("isAttack", false);
        deadCheck();
        checkAngryState();
        //Debug.Log(agent.speed);
        //Debug.Log(hp);
        speedTimer();
        //Debug.Log(animator.GetBool("isAttack"));

        //Debug.Log("speed: " + walkSpeed + runSpeed);
        //Debug.Log("Enemy hp: " + hp);
    }

    void speedTimer()
    {
        //Debug.Log("speedTiemr" + Timer);
        Timer += Time.deltaTime;
        if (Timer > 10)
        {
            setRandomState();
            Timer = 0;
        }
    }

    void setRandomState()
    {
        RandomNum = Random.Range(0, 2);
        animator.SetInteger("randomNum", RandomNum);
        //Debug.Log(animator.GetInteger("randomNum"));
        if (RandomNum == 0)
        {
            agent.speed = walkSpeed;
            //agent.speed = 2;
        }
        else
        {
            agent.speed = runSpeed;
            //agent.speed = 4;

        }
    } 

    IEnumerator SetDestination()
    {
        yield return new WaitForSeconds(3f);
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
            if (!animator.GetBool("isAttack"))
            {
                animator.SetBool("isAttack", true);
            }
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
                //ob.GetComponent<tower>().hp-= power;
                ob.GetComponent<tower>().DecreaseHp(power);
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
        int index = Random.Range(0, 3);
        PlaySoundEffects(hurtSound[index]);
        Invoke("playNav", 0.167f);
    }

    void PlaySoundEffects(AudioClip audioClip)
    {
        //audioSource.clip = audioClip;
        audioSource.PlayOneShot(audioClip);
    }

    public void playBloodEffect(RaycastHit hitInfo)
    {
        bloodEffect.transform.position = hitInfo.point;
        bloodEffect.transform.forward = hitInfo.normal;

        Instantiate(bloodEffect.GetComponent<ParticleSystem>(), bloodEffect.transform.position, Quaternion.Euler(bloodEffect.transform.forward));
        bloodEffect.GetComponent<ParticleSystem>().Play();
    }
    public void playDeathEffect()
    {
        Instantiate(deathEffect.GetComponent<ParticleSystem>(), transform.position, Quaternion.identity);
        deathEffect.GetComponent<ParticleSystem>().Play();
        Debug.Log("playDeathEffect");
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
            //transform.GetComponent<SphereCollider>().enabled = false;
            if (!audioSource.isPlaying)
            {
                playDeathEffect();
                PlaySoundEffects(deathSound);
            }


            Collider[] colliders = gameObject.GetComponents<Collider>();

            foreach(Collider collider in colliders)
            {
                collider.enabled = false;
            }

            rb.constraints = RigidbodyConstraints.FreezeAll;
            //enemyModel.layer = 2;
            //Debug.Log(agent.isStopped);
            //animator.SetBool("isDie1", true);
            animator.SetTrigger("isDie");
            //PlayerPrefs.SetInt("remainEnemy", PlayerPrefs.GetInt("remainEnemy")-1);
            if (!isInstantiate)
            {
                GameObject.Find("GameManager").GetComponent<ItemManager>().instantiateItem(transform.position);
                isInstantiate = true;
            }
            //GameObject.Find("GameManager").GetComponent<ItemManager>().isInstantiate = false;

            //transform.GetComponent<ItemManager>().instantiateItem(transform.position);

            Destroy(this.gameObject, 2f);
        }
    }
    
    

    void checkAngryState()
    {

        if (GameObject.Find("Canvas").GetComponent<BlinkingText>()!=null &&
            GameObject.Find("Canvas").GetComponent<BlinkingText>().hideWarning && !isAngry)
        {
            isAngry = true;
            SetAngry();
            //GameObject.Find("Canvas").GetComponent<BlinkingText>().hideWarning = false;
        }
    }
    void SetAngry()
    {
        //isCheck = true;
        agent.speed *= 1.5f;
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
        Debug.Log("HitByBomb" + transform.parent.gameObject.name);

        if (isDamaged) return;
        isDamaged = true;
        GameObject.FindWithTag("Player").GetComponent<player_Controller>().ultimateGauge++;
        Debug.Log(transform.parent.gameObject.name + "HitByBomb");
        //Debug.Log(GameObject.FindWithTag("Player").GetComponent<player_Controller>().ultimateGauge);

        hp -= GameObject.FindWithTag("Player").GetComponent<player_Controller>().currentBulletPower;
        Debug.Log(GameObject.FindWithTag("Player").GetComponentInChildren<player_Controller>().currentBulletPower);

        //hp--;
        Vector3 reactVector = transform.position - BombPos;
        StartCoroutine(Damage(reactVector));
        //rb.freezeRotation = true;
        //isDamaged = false;
        Invoke("setIsDamaged", 0.3f);
    }

    void setIsDamaged()
    {
        isDamaged = false;
    }

    IEnumerator Damage(Vector3 reactVec)
    {
        yield return new WaitForSeconds(0.1f);

        reactVec = reactVec.normalized;
        reactVec += Vector3.up * 3;

        //rb.freezeRotation = false;
        rb.AddForce(reactVec * 2, ForceMode.Impulse);
        rb.AddTorque(reactVec * 2, ForceMode.Impulse);
        playHurtAnim();
    }

    public void decreaseHP(int demage)
    {
        hp -= demage;
        Debug.Log("decreaseHP: " + hp);
    }
}
