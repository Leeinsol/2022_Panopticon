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
    public float GunRotationSpeed = 3f;
    GameObject GunHandle;
    GameObject Gun;
    GameObject Bomb;
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

    // current variable
    float currentSpeed;
      
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

        // gun instantiate
        if (useGun)
        {
            Gun = Instantiate(GunModel, GunHandle.transform) as GameObject;
            Gun.transform.parent = GunHandle.transform;

            // bomb instantiate
            Bomb = Instantiate(BombModel, GunHandle.transform) as GameObject;
            Bomb.transform.parent = GunHandle.transform;
            Destroy(Bomb.GetComponent<Bomb>());
            Destroy(Bomb.GetComponent<Rigidbody>());

            Bomb.SetActive(false);
        }
        else
        {
            crossHairText.enabled = false;
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

        if (Input.GetKeyDown(SprintKey) && iswalking)   isSprinting = true;
 
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
        if (useCrouch)  Crouch();

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
        if (useCameraRotationVerticality)   CameraRotationVerticality();

        if (useCameraRotationHorizontality) CameraRotationHorizontality();

        // jump
        if (useJump)    Jump();
    
        // head bob
        if (useHeadBob) HeadBob();

        // zoom camera
        if (useCameraZoom)  ZoomCamera();

        if (!isZooming && !isSprinting) theCamera.fieldOfView = defaultFOV;

        // Fire
        if (useGun && Gun.activeSelf)
        {
            Fire();
            if (useReload)
            {
                bulletUI();
                PressReloadKey();
                reloadBullet();
            }
        }

        if(useGun && Bomb.activeSelf)
        {
            bombFire();
        }

        changeWeapon();
    }

    void bombFire()
    {
        //bomb
        if (Input.GetKeyDown(FireKey))
        {
            //GameObject bomb = Instantiate(BombModel);
            //bomb.transform.position = GunHandle.transform.position;
            //bomb.transform.forward = GunHandle.transform.forward;


            Ray ray = new Ray(theCamera.transform.position, theCamera.transform.forward);
            RaycastHit hitInfo = new RaycastHit();

            // 건물에 던져도 던져지게 레이캐스트 변경 필요
            if (Physics.Raycast(ray, out hitInfo))
            {
                Vector3 nextVector = hitInfo.point - transform.position;
                nextVector.y = 5;

                GameObject bomb = Instantiate(BombModel,transform.position,transform.rotation);
                Rigidbody rigidBomb = bomb.GetComponent<Rigidbody>();
                rigidBomb.AddForce(nextVector, ForceMode.Impulse);
                rigidBomb.AddTorque(Vector3.back * 10, ForceMode.Impulse);

                
            }

        }
    }

    void changeWeapon()
    {
        Vector2 scrollDelta = Input.mouseScrollDelta;

        if (scrollDelta.y > 0 || scrollDelta.y < 0)
        {
            //Debug.Log("스크롤");

            if (!Gun.activeSelf)
            {
                setGun();
            }
            
            else if (!Bomb.activeSelf)
            {
                setBomb();
            }
        }
    }

    void setGun()
    {
        Gun.SetActive(true);
        Bomb.SetActive(false);
    }

    void setBomb()
    {
        Bomb.SetActive(true);
        Gun.SetActive(false);
    }

    void SetCrossHair()
    {
        if (crosshairtype == CrossHairType.cross)       crossHairText.text = "+";
        
        else if (crosshairtype == CrossHairType.circle) crossHairText.text = "○";
        
        else if (crosshairtype == CrossHairType.dot)    crossHairText.text = ".";
    }
    void SetReloadTimer()
    {
        if (reloadType == reloadBulletType.oneByOneReload)  ReloadTimer = OneBulletReloadTime * (maxBulletNum - bulletNum);

        else if (reloadType == reloadBulletType.allReload)  ReloadTimer = allReloadTime;
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
                Gun.transform.localPosition = Vector3.Slerp(Gun.transform.localPosition, GunSprintPos, GunRotationSpeed * Time.deltaTime);
                Gun.transform.localRotation = Quaternion.Slerp(Gun.transform.localRotation, GunSprintRot, GunRotationSpeed * Time.deltaTime);

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
        Gun.transform.localPosition = GunOriginPos;
        Gun.transform.localRotation = GunOriginRot;
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
                    collider.gameObject.GetComponent<Enemy>().hp--;

                    //Debug.Log(collider.gameObject.GetComponent<Enemy>().hp);
                    collider.gameObject.GetComponent<Enemy>().playHurtAnim();
                }
                if (collider is SphereCollider)
                {
                    //Debug.Log("머리");
                    //collider.gameObject.GetComponent<Enemy>().hp -= 2;

                    //Debug.Log(collider.gameObject.GetComponent<Enemy>().hp);
                    //collider.gameObject.GetComponent<Enemy>().playHurtAnim();

                    collider.gameObject.GetComponent<Enemy>().hp -= 2;

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
        Gun.transform.localPosition = GunOriginPos;

        while (Gun.transform.localPosition.z <= reloadActionForce - 0.02f)
        {
            Gun.transform.localPosition = Vector3.Lerp(Gun.transform.localPosition, reloadAction, 0.4f);
            yield return null;
        }

        while (Gun.transform.localPosition != GunOriginPos)
        {
            Gun.transform.localPosition = Vector3.Lerp(Gun.transform.localPosition, GunOriginPos, 0.1f);
            yield return null;
        }
    }
}

