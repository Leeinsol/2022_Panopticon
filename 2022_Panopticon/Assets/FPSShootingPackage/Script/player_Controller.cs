using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public enum CrossHairType
{
    cross, circle, dot
}

public enum reloadBulletType
{
    oneByOneReload, allReload
}

public class player_Controller : MonoBehaviour
{
    private Rigidbody rb;

    // Walk
    public float walkSpeed;
    public Vector3 HeadBobAmount = new Vector3(0f, .05f, 0f);
    private bool iswalking = false;
    private float walkHeight;

    // Sprint
    public bool useSprint = true;
    public KeyCode SprintKey = KeyCode.LeftShift;
    public float sprintSpeed =50f;
    private bool isSprinting = false;

    // Stamina
    public bool useStaminaLimit = true;
    public GameObject StaminaBar;
    public float maxStamina = 5f;
    private float stamina = 5f;

    // Crouch
    public bool useCrouch = true;
    public KeyCode CrouchKey = KeyCode.LeftControl;
    public float crouchSpeed = 15f;
    public float crouchHeight = 0.5f;
    private bool isCrouching = false;

    // Head Bob
    public bool useHeadBob = true;
    public float headBobSpeed = 10f;

    // Jump
    public bool useJump = true;
    public KeyCode JumpKey = KeyCode.Space;
    public float jumpForce = 5f;
    float groundCheckDistance = .1f;
    private float bufferCheckDistance = .1f;
    private bool isGround = false;

    // Camera
    public bool useCameraRotationVerticality = true;
    public bool useCameraRotationHorizontality = true;
    public Camera theCamera;
    public float lookSensitivity = 2f;
    public float cameraRotationLimit = 60f;
    private float currentCameraRotationX;
    Transform camHandle;
    Vector3 camHandlePos;

    // Zoom
    public bool useCameraZoom = true;
    public KeyCode ZoomKey = KeyCode.Z;
    public float zoomSpeed = 2f;
    public bool isZooming = false;
    float defaultFOV = 60f;
    float ZoomMultipleNum = 2;

    // Gun
    public bool useGun = true;
    public GameObject GunModel;
    public GameObject BombModel;
    public GameObject EnergeDrinkModel;

    public float GunRotationSpeed = 3f;
    GameObject GunHandle;

    GameObject[] Weapon = new GameObject[4];

    public bool useItem = true;

    //GameObject Gun;
    //GameObject Bomb;
    //GameObject EnergeDrink;

    Vector3 GunOriginPos = new Vector3(0, 0, 0);
    Quaternion GunOriginRot = Quaternion.Euler(0, 180, 0);
    Vector3 GunSprintPos = new Vector3(-0.271f, 0.001f, 0.135f);
    Quaternion GunSprintRot = Quaternion.Euler(-0.133f, 123.826f, -0.249f);
    public Transform BombPostion;
    public float BombRadius = 10.0f;
    public float BombForce = 500f;
    public float throwForce = 1000f;

    int BombDamage;
    new Rigidbody rigidbody;

    // Fire
    public KeyCode FireKey = KeyCode.Mouse0;
    public GameObject bulletEffect;
    ParticleSystem psBullet;
    public int maxBulletNum = 10;
    int bulletNum;
    public float fireRate = 0.5f;
    float fireTimer;

    // Canvas
    public Text crossHairText;
    public CrossHairType crosshairtype;
    public GameObject BulletNumUI;
    public GameObject ReladTimerUI;

    //Reload
    public bool useReload = true;
    public KeyCode ReloadKey = KeyCode.R;
    public reloadBulletType reloadType;
    public float maxReloadTime = 5f;
    public float allReloadTime = 1f;
    public float reloadActionForce = 0.5f;
    float ReloadTimer;
    float OneBulletReloadTime;
    float currentReloadTime;
    bool isReload = false;

    // Sound
    public bool useFireSound = true;
    public bool useReloadSound = true;
    public AudioClip FireSound, oneByOneReloadSound, allReloadSound;
    AudioSource audioSource;

    // Timer
    private float shakeTimer;
    private float zoomTimer;
    float energyTimer;

    // current variable
    float currentSpeed;

    public float flightLengthFactor = 0f;
    public float IncreaseAmount = 2f;

    public Image BombGauge;

    public int bulletPower = 1;
    public int currentBulletPower = 1;
    bool isPowerUp = false;

    public int Item2Num = 0;
    public int[] WeaponNum;
    private int currentIndex = 0;

    public GameObject RemainItemNumUI;
    public GameObject PowerTimeUI;
    public GameObject getItemModel;

    bool isPulling = false;

    // Start is called before the first frame update
    void Start()
    {
        // hide cursor
        Cursor.lockState = CursorLockMode.Locked;

        // initialize
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        camHandle = transform.Find("CamHandle");
        GunHandle = theCamera.transform.Find("GunHandle").gameObject;

        camHandlePos = camHandle.localPosition;
        walkHeight = GetComponent<CapsuleCollider>().height;
        currentSpeed = walkSpeed;
        zoomTimer = zoomSpeed;

        WeaponNum = new int[Weapon.Length]; 

        // gun instantiate
        if (useGun)
        {
            Weapon[0] = Instantiate(GunModel, GunHandle.transform) as GameObject;
            Weapon[0].transform.parent = GunHandle.transform;
            WeaponNum[0] = 1;

            // bomb instantiate
            Weapon[1] = Instantiate(BombModel, GunHandle.transform) as GameObject;
            Weapon[1].transform.parent = GunHandle.transform;
            WeaponNum[1] = 1;

            Destroy(Weapon[1].GetComponent<Bomb>());
            Destroy(Weapon[1].GetComponent<Rigidbody>());

            //Weapon[1].SetActive(false);
        }
        else
        {
            crossHairText.enabled = false;
        }

        if (useItem)
        {
            Weapon[2] = Instantiate(EnergeDrinkModel, GunHandle.transform) as GameObject;
            Weapon[2].transform.parent = GunHandle.transform;
            //Weapon[2].SetActive(false);
            WeaponNum[2] = 0;
        }

        // bullet instantiate
        bulletNum = maxBulletNum;
        OneBulletReloadTime = maxReloadTime / maxBulletNum;
        psBullet = bulletEffect.GetComponent<ParticleSystem>();

        // reload instantiate
        SetReloadTimer();
        ReladTimerUI.SetActive(false);
        currentReloadTime = ReloadTimer;
        ReladTimerUI.GetComponent<Slider>().maxValue = ReloadTimer;

        if (!useReload || !useGun) BulletNumUI.SetActive(false);

        // set SprintBar
        if (useSprint && useStaminaLimit)  StaminaBar.SetActive(true);
        else                               StaminaBar.SetActive(false);

        // cross hair instantiate
        SetCrossHair();
        BombGauge.transform.parent.gameObject.SetActive(false);

        for (int i = 0; i < Weapon.Length; i++)
        {
            if (WeaponNum[i] > 0)
            {
                currentIndex = i;
                break;
            }
        }
        //아이템 획득 도구
        Weapon[3] = Instantiate(getItemModel, GunHandle.transform) as GameObject;
        Weapon[3].transform.parent = GunHandle.transform;

        WeaponNum[3] = 1;
        
        
        offWeapon();
        Weapon[currentIndex].SetActive(true);

        setRemainEnergyDrinkUI(false);

    }
    
   
    public void setup(int damage, Vector3 rotation)
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.AddForce(rotation * throwForce);

        BombDamage = damage;
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    Instantiate(BombModel, transform.position, transform.rotation);

    //    Collider[] colliders = Physics.OverlapSphere(transform.position, BombRadius);
    //    foreach(Collider hit in colliders)
    //    {
    //        player_Controller
    //    }
    //}

    // Update is called once per frame
    void Update()
    {
        // move
        Move();

        if (Input.GetKeyDown(SprintKey) && iswalking) isSprinting = true;

        if (Input.GetKeyUp(SprintKey))
        {
            currentSpeed = walkSpeed;
            zoomTimer = zoomSpeed;
            isSprinting = false;
            setGunOrigin();
        }

        // sprint
        if (useSprint)
        {
            Sprint();
            StaminaUI();
        }

        // crouch
        if (useCrouch) Crouch();

        // set walk speed
        if (Input.GetKeyDown(CrouchKey))
        {
            currentSpeed = crouchSpeed;
            isCrouching = true;
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - 0.5f, transform.localPosition.z);

        }
        if (Input.GetKeyUp(CrouchKey))
        {
            currentSpeed = walkSpeed;
            isCrouching = false;
        }

        // camera
        if (useCameraRotationVerticality) CameraRotationVerticality();

        if (useCameraRotationHorizontality) CameraRotationHorizontality();

        // jump
        if (useJump) Jump();

        // head bob
        if (useHeadBob) HeadBob();

        // zoom camera
        if (useCameraZoom) ZoomCamera();

        if (!isZooming && !isSprinting) theCamera.fieldOfView = defaultFOV;

        // Fire
        if (useGun && Weapon[0].activeSelf)
        {
            Fire();
            if (useReload)
            {
                bulletUI();
                PressReloadKey();
                reloadBullet();
            }
        }

        if (useGun && Weapon[1].activeSelf)
        {
            bombFire();
            if (useReload)
            {
                reloadBomb();
            }
        }

        if (useGun && Weapon[2].activeSelf)
        {
            setRemainEnergyDrinkUI(true);
            eatEnergyDrink();
            RemainEnergyDrinkNum();
        }
        powerUp();
        changeWeapon();

        //Debug.Log(energyTimer);
        //Debug.Log(isPowerUp);

        if (Weapon[3].activeSelf)
        {
            getItem();
        }

    }

    void getItem()
    { 
        if (Input.GetKeyDown(FireKey)){
            Ray ray = new Ray(theCamera.transform.position, theCamera.transform.forward);

            //RaycastHit hitInfo = new RaycastHit();
            RaycastHit[] hits = Physics.RaycastAll(ray);

            for(int i=0; i<hits.Length; i++)
            {
                RaycastHit hit = hits[i];
                if (hit.transform.parent.gameObject.tag == "EnergyDrink")
                {
                    //Debug.Log("에너지 드링크");
                    isPulling = true;
                    StartCoroutine(PullEnergyDrink(hit.transform.parent.gameObject));
                    break;
                }
            }
        }

        if (Input.GetKeyUp(FireKey))
        {
            isPulling = false;
        }
    }
    IEnumerator PullEnergyDrink(GameObject drinkObject)
    {
        float t = 0;
        Vector3 originalPosition = drinkObject.transform.position;
        while (isPulling)
        {
            t += Time.deltaTime * 1f;
            drinkObject.transform.position = Vector3.Lerp(originalPosition, transform.position, t);
            yield return null;
        }
    }

    void setRemainEnergyDrinkUI(bool isShow)
    {
        RemainItemNumUI.SetActive(isShow);
        //Debug.Log("실행 " + RemainItemNumUI.activeSelf);

    } 

    void RemainEnergyDrinkNum()
    {
        RemainItemNumUI.transform.GetChild(0).GetComponent<Text>().text = WeaponNum[2].ToString();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // get EnergyDrink
        if (collision.transform.parent.gameObject.tag == "EnergyDrink")
        {
            WeaponNum[2]++;
            Destroy(collision.gameObject);
        }
    }
    void eatEnergyDrink()
    {
        if (Input.GetKeyDown(FireKey))
        {
            //Debug.Log("에너지 드링크 사용");
            currentBulletPower= Weapon[2].GetComponent<Item_energyDrink>().energyDrink.getPower();

            //Debug.Log("Time: " + Weapon[2].GetComponent<Item_energyDrink>().energyDrink.getTime());
            isPowerUp = true;
            energyTimer = Weapon[2].GetComponent<Item_energyDrink>().energyDrink.getTime();
            //powerUp();

            WeaponNum[2]--;

            if (WeaponNum[2] <= 0)
            {
                //Debug.Log("다 사용했어요" + currentIndex );
                currentIndex++;
                changeWeaponNext(currentIndex);
                setRemainEnergyDrinkUI(false);

                //changeWeaponNext(currentIndex, Input.mouseScrollDelta);
                //offWeapon();
                //Weapon[currentIndex].SetActive(true);
            }
        }
    }

    void powerUp()
    {
        if (!isPowerUp) return;
        //Debug.Log("powerUP");

        PowerTimeUI.transform.GetChild(0).GetComponent<Text>().text = energyTimer.ToString();
        energyTimer -= Time.deltaTime;
        //Debug.Log(energyTimer);
        //Debug.Log(currentBulletPower);


        if (energyTimer < 0) 
        {
            //Debug.Log("끝");

            energyTimer = Weapon[2].GetComponent<Item_energyDrink>().energyDrink.getTime();
            currentBulletPower = bulletPower;
            Debug.Log(currentBulletPower);

            isPowerUp = false;
        }
    }


    void BombUI()
    {
        BombGauge.transform.parent.gameObject.SetActive(true);

        BombGauge.fillAmount = flightLengthFactor;
    }

    void bombFire()
    {
        //Debug.Log(ReloadTimer);

        //bomb
        if (Input.GetKey(FireKey) && !ReladTimerUI.activeSelf)
        {
            BombUI();
            flightLengthFactor += IncreaseAmount * Time.deltaTime;
            if (flightLengthFactor >= 1f)
            {
                flightLengthFactor = 1f;
                IncreaseAmount = -IncreaseAmount;
                //Debug.Log("check");
            }
            else if (flightLengthFactor <= 0f)
            {
                flightLengthFactor = 0f;
                IncreaseAmount = -IncreaseAmount;
            }

            //Debug.Log(flightLengthFactor);

        }


        if (Input.GetKeyUp(FireKey) && !ReladTimerUI.activeSelf)
        {
            Ray ray = new Ray(theCamera.transform.position, theCamera.transform.forward);
            RaycastHit hitInfo = new RaycastHit();

            
            if (Physics.Raycast(ray, out hitInfo))
            {
                Vector3 forwardPosition = theCamera.transform.position + theCamera.transform.forward * 2f;
                Vector3 nextVector = hitInfo.point - transform.position;
                nextVector.y = 5;

                GameObject bomb = Instantiate(BombModel, forwardPosition, transform.rotation);
                Rigidbody rigidBomb = bomb.GetComponent<Rigidbody>();


                rigidBomb.AddForce(nextVector * flightLengthFactor, ForceMode.Impulse);
                rigidBomb.AddTorque(Vector3.back * 10, ForceMode.Impulse);

                //Collider playerCollider = GetComponent<Collider>();
                //Collider bombCollider = bomb.transform.GetChild(0).GetComponent<Collider>();
                //Physics.IgnoreCollision(playerCollider, bombCollider);
                flightLengthFactor = 0f;
                BombGauge.transform.parent.gameObject.SetActive(false);
                //reloadBomb();
                SetReload();
                //reloadBomb();
            }
        }
    }

    void changeWeapon()
    {
        int oldIndex = currentIndex;

        Vector2 scrollDelta = Input.mouseScrollDelta;

        //changeWeaponPrevious(oldIndex, scrollDelta);
        //changeWeaponNext(oldIndex, scrollDelta);

        if(scrollDelta.y != 0)
        {
            if (scrollDelta.y > 0)
            {
                // Scroll up
                changeWeaponPrevious(oldIndex);
            }
            else if (scrollDelta.y < 0)
            {
                // Scroll down
                changeWeaponNext(oldIndex);
            }
            //Debug.Log(currentIndex);

        }
        offWeapon();
        Weapon[currentIndex].SetActive(true);
    }

    void changeWeaponPrevious(int oldIndex)
    {
        do
        {
            currentIndex--;
            if (currentIndex < 0)
            {
                currentIndex = WeaponNum.Length - 1;
            }
        } while (WeaponNum[currentIndex] == 0 && currentIndex != oldIndex);
    }

    void changeWeaponNext(int oldIndex)
    {
        do
        {
            currentIndex++;
            if (currentIndex >= WeaponNum.Length)
            {
                currentIndex = 0;
            }
        } while (WeaponNum[currentIndex] == 0 && currentIndex != oldIndex);
    }
    void offWeapon()
    {
        for (int i = 0; i < Weapon.Length; i++)
        {
            Weapon[i].SetActive(false);
        }
    }


    void SetCrossHair()
    {
        if (crosshairtype == CrossHairType.cross)       crossHairText.text = "+";
        
        else if (crosshairtype == CrossHairType.circle) crossHairText.text = "○";
        
        else if (crosshairtype == CrossHairType.dot)    crossHairText.text = ".";
    }
    void SetReloadTimer()
    {
        if (reloadType == reloadBulletType.oneByOneReload) ReloadTimer = OneBulletReloadTime * (maxBulletNum - bulletNum);

        else if (reloadType == reloadBulletType.allReload) ReloadTimer = allReloadTime;

        if (Weapon[1].activeSelf) ReloadTimer = 2f;
        //else ReloadTimer = 2f;
    }

    private void Move()
    {
        float _moveDirX = Input.GetAxisRaw("Horizontal");
        float _moveDirZ = Input.GetAxisRaw("Vertical");
        Vector3 _moveHorizontal = transform.right * _moveDirX;
        Vector3 _moveVertical = transform.forward * _moveDirZ;
        
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical")!=0)
        {
            Vector3 _velocity;
            _velocity = (_moveHorizontal + _moveVertical).normalized * currentSpeed;
            
            rb.MovePosition(transform.position + _velocity * Time.deltaTime);
            iswalking = true;
        }
        else
        {
            iswalking = false;
        }
    }

    void Sprint()
    {
        if (isSprinting)
        {
            SetFOVSmooth(65);
            
            currentSpeed = sprintSpeed;

            if (useGun)
            {
                Weapon[0].transform.localPosition = Vector3.Slerp(Weapon[0].transform.localPosition, GunSprintPos, GunRotationSpeed * Time.deltaTime);
                Weapon[0].transform.localRotation = Quaternion.Slerp(Weapon[0].transform.localRotation, GunSprintRot, GunRotationSpeed * Time.deltaTime);

            }

            if (useStaminaLimit) stamina -= Time.deltaTime;
            if (stamina < 0)
            {
                stamina = 0;
                isSprinting = false;
                setGunOrigin();
            }
        }
        else if(stamina < maxStamina)   stamina += Time.deltaTime;
    }

    void setGunOrigin()
    {
        Weapon[0].transform.localPosition = GunOriginPos;
        Weapon[0].transform.localRotation = GunOriginRot;
    }
    void Crouch()
    {
        if (isCrouching)    GetComponent<CapsuleCollider>().height = crouchHeight;
        
        else   GetComponent<CapsuleCollider>().height = walkHeight;
         
    }

    void StaminaUI()
    {
        float ratio = stamina / maxStamina;

        StaminaBar.GetComponent<Slider>().value = ratio;
    }

    private void CameraRotationVerticality()
    {
        float _xRotation = Input.GetAxisRaw("Mouse Y");

        float _cameraRotationX = _xRotation * lookSensitivity;
        
        currentCameraRotationX -= _cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }

    void CameraRotationHorizontality()
    {
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _cameraRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;
        rb.MoveRotation(rb.rotation * Quaternion.Euler(_cameraRotationY));
    }

    void HeadBob()
    {
        if (iswalking)
        {
            shakeTimer += Time.deltaTime * headBobSpeed;
            camHandle.localPosition = new Vector3(camHandlePos.x + Mathf.Sin(shakeTimer) * HeadBobAmount.x,
                camHandlePos.y + Mathf.Sin(shakeTimer) * HeadBobAmount.y,
                camHandlePos.z + Mathf.Sin(shakeTimer) * HeadBobAmount.z);
        }
        else
        {
            shakeTimer = 0;
            camHandle.localPosition = new Vector3(Mathf.Lerp(camHandle.localPosition.x, camHandlePos.x, Time.deltaTime * 7f),
              Mathf.Lerp(camHandle.localPosition.y, camHandlePos.y, Time.deltaTime * 10f),
              Mathf.Lerp(camHandle.localPosition.z, camHandlePos.z, Time.deltaTime * 10f));
        }
    }

    void Jump()
    {
        if (!isCrouching) groundCheckDistance = (GetComponent<CapsuleCollider>().height / 2) + bufferCheckDistance;
        else groundCheckDistance = (GetComponent<CapsuleCollider>().height * 2) + bufferCheckDistance;

        if (Input.GetKeyDown(JumpKey) && isGround)  GetComponent<Rigidbody>().AddForce(transform.up * jumpForce, ForceMode.Impulse);

        RaycastHit hit;

        if (Physics.Raycast(transform.position, -transform.up, out hit, groundCheckDistance))   isGround = true;
        else isGround = false;
    }

    void ZoomCamera()
    {
        if (Input.GetKey(ZoomKey))
        {
            isZooming = true;
            SetFOVSmooth(defaultFOV / ZoomMultipleNum);

        }

        if (Input.GetKeyUp(ZoomKey))
        {
            isZooming = false;
            zoomTimer = zoomSpeed;
            SetFOVSmooth(defaultFOV);
            //Debug.Log(defaultFOV);
        }
    }

    void SetFOVSmooth(float target)
    {
        zoomTimer -= Time.deltaTime;
        theCamera.fieldOfView = Mathf.Lerp(target, theCamera.fieldOfView, zoomTimer * 0.5f);
    }

    void Fire()
    {
        if (Input.GetKey(FireKey))
        {
            if (!useReload) ShootBullet();

            if (bulletNum > 0 && useReload)
            {
                if (isReload)
                {
                    setReloadBulletUI(false);
                    return;
                }
                else
                {
                    ShootBullet();
                }
            }
            if (fireTimer < fireRate) fireTimer += Time.deltaTime;
        }
        if (Input.GetKeyDown(FireKey)) fireTimer = fireRate;

        if (bulletNum == 0 && !isReload) SetReload();
    }
    void PlaySoundEffects(AudioClip audioClip)
    {
        audioSource.clip = audioClip;
        audioSource.Play();
    }
    void ShootBullet()
    {
        if (fireTimer < fireRate)
        {
            return;
        }

        Ray ray = new Ray(theCamera.transform.position, theCamera.transform.forward);
        RaycastHit hitInfo = new RaycastHit();
        Debug.DrawRay(ray.origin, ray.direction * 10f, Color.red, 2f); 
        bulletNum--;
        
        if(useFireSound) PlaySoundEffects(FireSound);

        StopAllCoroutines();
        StartCoroutine(reloadActionCoroutine());


        if (Physics.Raycast(ray, out hitInfo))
        {
            bulletEffect.transform.position = hitInfo.point;
            bulletEffect.transform.forward = hitInfo.normal;

            // Particle: Looping -> False / Stop Action -> Destroy
            Instantiate(psBullet, bulletEffect.transform.position, Quaternion.Euler(bulletEffect.transform.forward));
            psBullet.Play();
            Collider collider = hitInfo.collider;


            //if (collider.transform.parent.parent != null)
            //{
            //    if (collider.transform.parent.parent.gameObject.GetComponent<Enemy>())
            //    {

            //        //if (collider is BoxCollider)
            //        //{
            //        //    Debug.Log("박스");
            //        //    collider.transform.parent.parent.gameObject.GetComponent<Enemy>().hp--;

            //        //    Debug.Log(collider.transform.parent.parent.gameObject.GetComponent<Enemy>().hp);
            //        //    collider.transform.parent.parent.gameObject.GetComponent<Enemy>().playHurtAnim();
            //        //}
            //        if (collider is BoxCollider)
            //        {
            //            Debug.Log("박스");
            //            collider.transform.parent.parent.gameObject.GetComponent<Enemy>().hp--;

            //            Debug.Log(collider.transform.parent.parent.gameObject.GetComponent<Enemy>().hp);
            //            collider.transform.parent.parent.gameObject.GetComponent<Enemy>().playHurtAnim();
            //        }
            //        if (collider is SphereCollider)
            //        {
            //            Debug.Log("머리");
            //            //collider.gameObject.GetComponent<Enemy>().hp -= 2;

            //            //Debug.Log(collider.gameObject.GetComponent<Enemy>().hp);
            //            //collider.gameObject.GetComponent<Enemy>().playHurtAnim();

            //            collider.transform.parent.parent.gameObject.GetComponent<Enemy>().hp -= 2;

            //            Debug.Log(collider.transform.parent.parent.gameObject.GetComponent<Enemy>().hp);
            //            collider.transform.parent.parent.gameObject.GetComponent<Enemy>().playHurtAnim();
            //        }
            //    }
            //}



            // 총 맞았을 때
            if (collider.gameObject.GetComponent<Enemy>())
            {

                if (collider is CapsuleCollider)
                {
                    //Debug.Log("캡슐");
                    collider.gameObject.GetComponent<Enemy>().hp -= currentBulletPower;

                    //Debug.Log(collider.gameObject.GetComponent<Enemy>().hp);
                    collider.gameObject.GetComponent<Enemy>().playHurtAnim();
                }
                if (collider is SphereCollider)
                {
                    //Debug.Log("머리");
                    //collider.gameObject.GetComponent<Enemy>().hp -= 2;

                    //Debug.Log(collider.gameObject.GetComponent<Enemy>().hp);
                    //collider.gameObject.GetComponent<Enemy>().playHurtAnim();

                    collider.gameObject.GetComponent<Enemy>().hp -= (currentBulletPower * 2);

                    Debug.Log(collider.gameObject.GetComponent<Enemy>().hp);
                    collider.gameObject.GetComponent<Enemy>().playHurtAnim();
                }

                //else
                //{
                //    Debug.Log("다리맞음");
                //    collider.gameObject.GetComponent<Enemy>().hp--;
                //}

                //if(collider.GetType() == typeof(SphereCollider))


                //Debug.Log(collider.name + collider.gameObject.GetComponent<Enemy>().hp);

            }
        }
        fireTimer = 0f;
    }

    void PressReloadKey()
    {
        if (Input.GetKeyDown(ReloadKey) && !isReload && bulletNum != maxBulletNum)  SetReload();
    }

    void SetReload()
    {
        isReload = true;
        if (useReload) ReladTimerUI.SetActive(true);

        SetReloadTimer();
        currentReloadTime = ReloadTimer;
        //Debug.Log(ReloadTimer);
        ReladTimerUI.GetComponent<Slider>().maxValue = ReloadTimer;
    }

    void reloadBullet()
    {
        if (!isReload) return;

        ReloadTimer -= Time.deltaTime;
        ReladTimerUI.GetComponent<Slider>().value = ReloadTimer;

        if (reloadType == reloadBulletType.oneByOneReload) increaseBullet();
  
        if (ReloadTimer < 0)
        {
            bulletNum = maxBulletNum;

            if (reloadType == reloadBulletType.allReload && useReloadSound) PlaySoundEffects(allReloadSound);

            setReloadBulletUI(false);
        }
    }

    void reloadBomb()
    {
        if (!isReload) return;
       
        ReloadTimer -= Time.deltaTime;
        ReladTimerUI.GetComponent<Slider>().value = ReloadTimer;

        //Debug.Log("reload Time: " + ReloadTimer);

        if (ReloadTimer < 0)
        {
            //Debug.Log("reload Time: " + ReloadTimer);

            PlaySoundEffects(allReloadSound);
            setReloadBulletUI(false);
        }
    }

    void setReloadBulletUI(bool state)
    {
        ReladTimerUI.SetActive(state);
        ReloadTimer = maxReloadTime;
        isReload = state;
        ReladTimerUI.GetComponent<Slider>().maxValue = ReloadTimer;
    }

    void increaseBullet()
    {
        for (int i = 1; i < maxBulletNum - bulletNum + 1; i++)
        {
            if (currentReloadTime - OneBulletReloadTime > ReloadTimer)
            {
                bulletNum++;
                if (reloadType == reloadBulletType.oneByOneReload && useReloadSound) PlaySoundEffects(oneByOneReloadSound);

                currentReloadTime -= OneBulletReloadTime;
            }
        }
    }

    void bulletUI()
    {
        BulletNumUI.transform.Find("bulletText").gameObject.GetComponent<Text>().text = bulletNum.ToString();
        BulletNumUI.transform.Find("MaxBulletText").gameObject.GetComponent<Text>().text = maxBulletNum.ToString();
    }


    IEnumerator reloadActionCoroutine()
    {
        Vector3 reloadAction = new Vector3(GunOriginPos.x, GunOriginPos.y, reloadActionForce);
        Weapon[0].transform.localPosition = GunOriginPos;

        while (Weapon[0].transform.localPosition.z <= reloadActionForce - 0.02f)
        {
            Weapon[0].transform.localPosition = Vector3.Lerp(Weapon[0].transform.localPosition, reloadAction, 0.4f);
            yield return null;
        }

        while (Weapon[0].transform.localPosition != GunOriginPos)
        {
            Weapon[0].transform.localPosition = Vector3.Lerp(Weapon[0].transform.localPosition, GunOriginPos, 0.1f);
            yield return null;
        }
    }
}

