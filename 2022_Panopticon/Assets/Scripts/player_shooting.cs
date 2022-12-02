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
    float nowReloadTime;

    bool reload = false;
    bool increaseBullet = false;
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
        nowReloadTime = time;
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
        bulletText.text = "√—æÀ ºˆ: " + bulletNum;
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
            //time = OneBulletReloadTime * (maxBulletNum - bulletNum);
            //Debug.Log(time);
            //timer.GetComponent<Slider>().maxValue = time;
            nowReloadTime = time;

            reload = true;
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
            nowReloadTime = time;
            timer.GetComponent<Slider>().maxValue = time;


        }

        if (reload)
        {
            reloadBullet2();
        }

        //if (increaseBullet)
        //{
        //    StartCoroutine(reloadIncreaseBullet());
        //    //increase();
        //}


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
            Debug.Log("√— ∏¬¿Ω"+hitInfo.collider.gameObject.name);
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

        //Debug.Log(nowReloadTime);
        //Debug.Log(OneBulletReloadTime);
        //Debug.Log(bulletNum);
        //Debug.Log(time);
        time -= Time.deltaTime;
        timer.GetComponent<Slider>().value = time;
        increaseBullet = true;

        //StartCoroutine(reloadIncreaseBullet());
        //Invoke("increase",0f);
        increase();
        if (time < 0)
        {
            bulletNum = maxBulletNum;
            //Debug.Log("reloadBullet2");
            timer.SetActive(false);
            time = maxReloadTime;
            reload = false;
            timer.GetComponent<Slider>().maxValue = time;

        }
        increaseBullet = false;
    }

    void increase()
    {
        //Debug.Log("increase");
        for (int i = 1; i < maxBulletNum - bulletNum + 1; i++)
        {
            //Debug.Log(i);
            //StartCoroutine(reloadIncreaseBullet());
            if (nowReloadTime - OneBulletReloadTime > time)
            {
                Debug.Log("¡ı∞°");
                bulletNum++;
                nowReloadTime -= OneBulletReloadTime;
            }
        }
    }

    IEnumerator reloadIncreaseBullet()
    {
        //Debug.Log("reloadIncreaseBullet");
        //increase();

        yield return new WaitForSeconds(5f);
        increaseBullet = false;
    }

    //IEnumerator Test()
    //{
    //    int i = 1;
    //    while (true)
    //    {
    //        yield return new WaitForSeconds(1.0f);
    //        Debug.Log(i + "√ ");
    //        i++;
    //    }
    //}
}