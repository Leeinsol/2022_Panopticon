using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class player_shooting : MonoBehaviour
{
    public Camera theCamera;
    public Text bulletText;

    public GameObject bulletEffect;
    ParticleSystem psBullet;

    Animator animator;

    public KeyCode FireKey = KeyCode.Mouse0;
    public KeyCode ReloadKey = KeyCode.R;

    public int maxBulletNum = 10;
    int bulletNum;

    public GameObject timer;
    public float maxReloadTime = 5f;
    float time;

    public float fireRate = 0.5f;
    float fireTimer;

    float OneBulletReloadTime;
    float currentReloadTime;

    bool isReload = false;
    bool isFire = false;

    // Start is called before the first frame update
    void Start()
    {
        theCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        animator = GetComponent<Animator>();
        bulletNum = maxBulletNum;
        OneBulletReloadTime = maxReloadTime / maxBulletNum;
        //center = new Vector3(cam.pixelWidth / 2, cam.pixelHeight / 2, 0);


        psBullet = bulletEffect.GetComponent<ParticleSystem>();

        timer.SetActive(false);
        time = maxReloadTime;
        currentReloadTime = time;
        timer.GetComponent<Slider>().maxValue = time;

    }

    // Update is called once per frame
    void Update()
    {
        bulletText.text = "√—æÀ ºˆ: " + bulletNum;

        if (Input.GetKey(FireKey))
        {
            if (bulletNum > 0)
            {
                if (isReload)
                {
                    isFire = false;
                    setReloadBulletUI(false);
                    return;
                }
                else
                {
                    isFire = true;
                    Fire();
                }
            }
            if (fireTimer < fireRate)
            {
                fireTimer += Time.deltaTime;
            }
        }
        if (Input.GetKeyDown(FireKey))
        {

            fireTimer = fireRate;

        }
        if (Input.GetKeyUp(FireKey))
        {
            isFire = false;
        }

        if (bulletNum == 0)
        {
            animator.SetBool("isShoot", false);
            timer.SetActive(true);
            currentReloadTime = time;

            isReload = true;
        }

        if (Input.GetKeyDown(ReloadKey) && !isReload)
        {
            isFire = false;
            isReload = true;
            timer.SetActive(true);

            //Debug.Log("press R button");
            time = OneBulletReloadTime * (maxBulletNum - bulletNum);
            currentReloadTime = time;
            timer.GetComponent<Slider>().maxValue = time;

        }

        if (isReload)
        {
            reloadBullet();
        }
    }
    void Fire()
    {

        if (fireTimer < fireRate)
        {
            animator.SetBool("isShoot", false);
            return;
        }

        Ray ray = new Ray(theCamera.transform.position, theCamera.transform.forward);
        RaycastHit hitInfo = new RaycastHit();
        bulletNum--;

        animator.SetBool("isShoot", true);
        Debug.DrawRay(ray.origin, ray.direction * 10f, Color.red, 2f);

        if (Physics.Raycast(ray, out hitInfo))
        {
            //if (hitInfo.collider.name == "Plane") return;
            bulletEffect.transform.position = hitInfo.point;
            bulletEffect.transform.forward = hitInfo.normal;
            Debug.Log("√— ∏¬¿Ω" + hitInfo.collider.gameObject.name);
            Instantiate(psBullet, bulletEffect.transform.position, Quaternion.Euler(bulletEffect.transform.forward));
            GameObject ob = hitInfo.collider.gameObject;

            if (hitInfo.collider.gameObject.GetComponent<Enemy>())
            {
                ob.GetComponent<Enemy>().hp--;
                ob.GetComponent<Enemy>().playHurtAnim();
            }
        }

        fireTimer = 0f;

    }

    void reloadBullet()
    {

        time -= Time.deltaTime;
        timer.GetComponent<Slider>().value = time;

        increaseBullet();
        if (time < 0)
        {
            bulletNum = maxBulletNum;

            setReloadBulletUI(false);
        }
    }

    void setReloadBulletUI(bool state)
    {
        timer.SetActive(state);
        time = maxReloadTime;
        isReload = state;
        timer.GetComponent<Slider>().maxValue = time;
    }

    void increaseBullet()
    {
        for (int i = 1; i < maxBulletNum - bulletNum + 1; i++)
        {
            if (currentReloadTime - OneBulletReloadTime > time)
            {
                //Debug.Log("¡ı∞°");
                bulletNum++;
                currentReloadTime -= OneBulletReloadTime;
            }
        }
    }
}