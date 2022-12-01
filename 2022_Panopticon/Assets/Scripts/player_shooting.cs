using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class player_shooting : MonoBehaviour
{
    public Camera cam;
    //private Vector3 center;
    //public GameObject bullet;
    public GameObject bulletEffect;
    public Text bulletText;

    public GameObject timer;


    ParticleSystem psBullet;

    Animator animator;

    public int maxBulletNum = 10;
    int bulletNum;
    public float maxReloadTime = 5f;
    float time;
    public float fireRate = 0.5f;
    float fireTimer;

    public float OneBulletReloadTime;

    bool reload = false;

    //Transform firePos;

    // Start is called before the first frame update
    void Start()
    {
        bulletNum = maxBulletNum;
        OneBulletReloadTime = maxReloadTime / maxBulletNum;
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        //center = new Vector3(cam.pixelWidth / 2, cam.pixelHeight / 2, 0);

        
        psBullet = bulletEffect.GetComponent<ParticleSystem>();
        animator = GetComponent<Animator>();
     
        timer.SetActive(false);
        time = maxReloadTime;
        timer.GetComponent<Slider>().maxValue = time;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Shooting");
        //Ray ray = cam.ScreenPointToRay(center);
        //Debug.DrawRay(transform.position, transform.forward * 10f, Color.red);
        //Vector3 pos = new Vector3(cam.transform.position.x, cam.transform.position.y, cam.transform.position.z + 3);

        //Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        //RaycastHit hit = new RaycastHit();
        bulletText.text = "�Ѿ� ��: " + bulletNum;
        //Debug.DrawRay(this.transform.position+Vector3.up, Vector3.forward * 15, Color.red);


        if (Input.GetMouseButton(0))
        {
            if (bulletNum > 0)
            {
                Fire();
            }
            if (fireTimer < fireRate)
            {
                fireTimer += Time.deltaTime;
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            fireTimer = fireRate;

        }

        if (bulletNum == 0)
        {
            animator.SetBool("isShoot", false);
            timer.SetActive(true);
            reloadBullet();
        }

        //Instantiate(bullet, firePos.position, Quaternion.Euler(0,0,0));
        //GameObject bullet_clone = Instantiate(bulletFactory);
        //bulletFactory.transform.position=

        if (Input.GetKeyDown(KeyCode.R))
        {
            reload = true;
            timer.SetActive(true);

            //Debug.Log("press R button");
            time = OneBulletReloadTime * (maxBulletNum - bulletNum);
            
        }

        if (reload)
        {
            reloadBullet2();
        }
    }
    void Fire()
    {
        if (fireTimer < fireRate)
        {
            animator.SetBool("isShoot", false);
            return;
        }

        //Debug.Log("Bullet Num: "+ bulletNum);
        Debug.Log("Fire");
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hitInfo = new RaycastHit();
        bulletNum--;

        animator.SetBool("isShoot", true);
        Debug.DrawRay(ray.origin, ray.direction * 10f, Color.red,2f);

        int layerMask = (1 << LayerMask.NameToLayer("JumpLayer"));
        layerMask = ~layerMask;

        if (Physics.Raycast(ray, out hitInfo))
        {
            //if (hitInfo.collider.name == "Plane") return;

            bulletEffect.transform.position = hitInfo.point;
            bulletEffect.transform.forward = hitInfo.normal;
            Debug.Log("�� ����"+hitInfo.collider.gameObject.name);
            Instantiate(psBullet, bulletEffect.transform.position, Quaternion.Euler(bulletEffect.transform.forward));
            GameObject ob = hitInfo.collider.gameObject;

            if (ob.layer != 6)
            {
                ob.GetComponent<Enemy>().hp--;
                ob.GetComponent<Enemy>().playHurtAnim();
            }

            //Debug.Log(ob.GetComponent<Enemy>().hp);
            //Destroy(hitInfo.collider.gameObject);
        }

        fireTimer = 0f;
    }


    void reloadBullet()
    {
        time -= Time.deltaTime;
        //Debug.Log("time: " + time);
        timer.GetComponent<Slider>().value = time;
        
        if (time < 0)
        {
            bulletNum = maxBulletNum;
            Debug.Log("reloadBullet");
            timer.SetActive(false);
            time = maxReloadTime;
        }
    }

    void reloadBullet2()
    {

        //Debug.Log(reloadTime);
        time -= Time.deltaTime;
        timer.GetComponent<Slider>().value = maxReloadTime-time;

        if (time < 0)
        {
            bulletNum = maxBulletNum;
            Debug.Log("reloadBullet");
            timer.SetActive(false);
            time = maxReloadTime;
            reload = false;
        }
    }
}