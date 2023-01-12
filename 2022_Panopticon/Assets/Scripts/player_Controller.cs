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
    // Walk
    public float walkSpeed =30f;
    public Vector3 HeadBobAmount = new Vector3(0f, .05f, 0f);
    private bool iswalking = false;
    private float walkHeight;

    // Sprint
    public KeyCode SprintKey = KeyCode.LeftShift;
    public bool useSprint = true;
    public float sprintSpeed =50f;
    private bool isSprinting = false;

    // Stamina
    public GameObject StaminaBar;
    public float maxStamina = 5f;
    private float stamina = 5f;

    // Crouch
    public KeyCode CrouchKey = KeyCode.LeftControl;
    public bool useCrouch = true;
    public float crouchSpeed = 15f;
    public float crouchHeight= 0.5f;
    private bool isCrouching = false;


    // Jump
    public KeyCode JumpKey = KeyCode.Space;
    public bool useJump = true;
    public float jumpForce = 5f;
    public float groundCheckDistance = .1f;
    //public string LayerName = "JumpLayer";
    private bool isGround = false;
    private float bufferCheckDistance = 0.1f;

    // Camera
    public bool useCameraRotationVerticality = true;
    public bool useCameraRotationHorizontality = true;
    public Camera theCamera;
    public float lookSensitivity = 2f;
    public float cameraRotationLimit = 60f;
    private float currentCameraRotationX;
    Transform camHandle;
    Vector3 camHandlePos;

    // Head Bob
    public bool useHeadBob = true;
    public float headBobSpeed = 10f;

    // Rigidbody, Animator
    private Rigidbody rb;
    //private Animator animator;

    // Timer
    private float shakeTimer;
    private float zoomTimer;

    // RaycastHit
    private RaycastHit Hit;

    // current variable
    float currentSpeed;
    float currentHeight;

    // Gun
    public GameObject GunModel;
    GameObject GunHandle;
    GameObject Gun;
    public Vector3 GunOriginPos = new Vector3(0, 0, 0);

    // Zoom
    public KeyCode ZoomKey = KeyCode.Z;
    public bool useCameraZoom = true;
    public float zoomSpeed = 2f;
    public bool isZooming = false;
    float defaultFOV = 60f;
    float ZoomMultipleNum = 2;

    // Canvas
    public Text crossHairText;
    public CrossHairType crosshairtype;
    public Text bulletText;
    public GameObject ReladTimerUI;

    // Fire
    public KeyCode FireKey = KeyCode.Mouse0;
    public GameObject bulletEffect;
    ParticleSystem psBullet;
    public int maxBulletNum = 10;
    int bulletNum;
    public float fireRate = 0.5f;
    float fireTimer;
    //bool isFire = false;
    public bool useFire = true;

    //Reload
    public KeyCode ReloadKey = KeyCode.R;
    public reloadBulletType reloadType;
    public float maxReloadTime = 5f;
    public float allReloadTime = 1f;
    float ReloadTimer;
    float OneBulletReloadTime;
    float currentReloadTime;
    bool isReload = false;
    public bool useReload = true;
    public float reloadActionForce = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        // hide cursor
        Cursor.lockState = CursorLockMode.Locked;

        rb = GetComponent<Rigidbody>();
        //animator = GetComponent<Animator>();
        camHandle = transform.Find("CamHandle");
        GunHandle = theCamera.transform.Find("GunHandle").gameObject;

        // initialize
        camHandlePos = camHandle.localPosition;
        walkHeight = theCamera.transform.localPosition.y;
        currentSpeed = walkSpeed;
        currentHeight = walkHeight;
        zoomTimer = zoomSpeed;

        // gun instantiate
        Gun = Instantiate(GunModel, GunHandle.transform) as GameObject;
        Gun.transform.parent = GunHandle.transform;

        // bullet instantiate
        bulletNum = maxBulletNum;
        OneBulletReloadTime = maxReloadTime / maxBulletNum;
        psBullet = bulletEffect.GetComponent<ParticleSystem>();

        // reload instantiate
        SetReloadTimer();
        ReladTimerUI.SetActive(false);
        currentReloadTime = ReloadTimer;
        ReladTimerUI.GetComponent<Slider>().maxValue = ReloadTimer;

        if (!useReload || !useFire)  bulletText.enabled = false;

        if (!useFire)
        {
            Gun.SetActive(false);
            crossHairText.enabled = false;
        }

        // set SprintBar
        if (useSprint)  StaminaBar.SetActive(true);
        else            StaminaBar.SetActive(false);

        // cross hair instantiate
        SetCrossHair();
    }

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
 
        // Fire
        if (useFire)
        {
            Fire();
            if (useReload)
            {
                bulletUI();
                PressReloadKey();
                //reloadBullet();
                reloadBullet();
            }
        }
        //Fire();
        //bulletUI();
        //// reload
        //if (Input.GetKeyDown(ReloadKey) && !isReload)
        //{
        //    PressReloadKey();
        //}
        ////reloadAllBullet();
        //if (isReload)
        //{
        //    reloadBullet();

        //}
        //Debug.Log(bulletNum);
        //Debug.Log(ReloadTimer);
        //Debug.Log(fireTimer);
        //Debug.Log(Gun.transform.localPosition);
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
            transform.localScale = new Vector3(transform.localScale.x, walkHeight, transform.localScale.z);

            rb.MovePosition(transform.position + _velocity * Time.deltaTime);
            //animator.SetBool("isRun", true);
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
            SetFOVSmooth(theCamera.fieldOfView + 5);
            currentSpeed = sprintSpeed;
            stamina -= Time.deltaTime;
            if (stamina < 0)
            {
                stamina = 0;
                isSprinting = false;
            }
        }
        else if(stamina < maxStamina){
            stamina += Time.deltaTime;
        }
    }

    void Crouch()
    {
        if (isCrouching)
        {
            //nowSpeed = crouchSpeed;
            currentHeight = crouchHeight;
            transform.localScale = new Vector3(transform.localScale.x, currentHeight, transform.localScale.z);
            //float center = crouchHeight / 2;
            //GetComponent<CapsuleCollider>().height = Mathf.Lerp(GetComponent<CapsuleCollider>().height, crouchHeight, 0.1f);
            //GetComponent<CapsuleCollider>().center = Vector3.Lerp(GetComponent<CapsuleCollider>().center, new Vector3(0, center, 0), 0.3f);
        }
        else
        {
            //nowSpeed = walkSpeed;
            currentHeight = walkHeight;
            //Debug.Log("move: " + moveHeight);
            //Debug.Log("now: " + nowHeight);
            transform.localScale = new Vector3(transform.localScale.x, currentHeight, transform.localScale.z);
            //float center = moveHeight;
            //GetComponent<CapsuleCollider>().height = Mathf.Lerp(GetComponent<CapsuleCollider>().height, nowHeight, 0.1f);
            //GetComponent<CapsuleCollider>().center = Vector3.Lerp(GetComponent<CapsuleCollider>().center, new Vector3(0, 0, 0), 0.3f);

        }
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

        // Debug.Log(myRigid.rotation);  // 쿼터니언
        // Debug.Log(myRigid.rotation.eulerAngles); // 벡터
    }

    void CameraRotationHorizontality()
    {
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _cameraRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;
        rb.MoveRotation(rb.rotation * Quaternion.Euler(_cameraRotationY)); // 쿼터니언 * 쿼터니언
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
        //Debug.Log("groundcheck");
        groundCheckDistance = (GetComponent<CapsuleCollider>().height / 2) + bufferCheckDistance;

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
            //SetFOVSmooth(defaultFOV);
        }
    }

    void SetFOVSmooth(float target)
    {
        //float time = zoomSpeed;
        zoomTimer -= Time.deltaTime;
        //float angle = Mathf.Abs((defaultFOV / ZoomMultipleNum) - defaultFOV);
        //theCamera.fieldOfView = Mathf.MoveTowards(target, theCamera.fieldOfView, Time.deltaTime * (angle/0.15f));
        //theCamera.fieldOfView = Mathf.MoveTowards(target, theCamera.fieldOfView, Time.deltaTime * 0.15f);
        //Debug.Log(theCamera.fieldOfView);
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
                    //isFire = false;
                    setReloadBulletUI(false);
                    return;
                }
                else
                {
                    //isFire = true;
                    ShootBullet();
                }
            }
            if (fireTimer < fireRate) fireTimer += Time.deltaTime;
        }
        if (Input.GetKeyDown(FireKey)) fireTimer = fireRate;

        if (bulletNum == 0 && !isReload) SetReload();
    }
    void ShootBullet()
    {
        if (fireTimer < fireRate)
        {
            return;
        }

        Ray ray = new Ray(theCamera.transform.position, theCamera.transform.forward);
        RaycastHit hitInfo = new RaycastHit();
        bulletNum--;

        //animator.SetBool("isShoot", true);
        //Debug.DrawRay(ray.origin, ray.direction * 10f, Color.red, 2f);

        StopAllCoroutines();
        StartCoroutine(reloadActionCoroutine());


        if (Physics.Raycast(ray, out hitInfo))
        {
            //if (hitInfo.collider.name == "Plane") return;
            bulletEffect.transform.position = hitInfo.point;
            bulletEffect.transform.forward = hitInfo.normal;
            //Debug.Log("총 맞음" + hitInfo.collider.gameObject.name);
            Instantiate(psBullet, bulletEffect.transform.position, Quaternion.Euler(bulletEffect.transform.forward));
            GameObject ob = hitInfo.collider.gameObject;

            // 총 맞았을 때
            if (ob.GetComponent<Enemy>())
            {
                ob.GetComponent<Enemy>().hp--;
                ob.GetComponent<Enemy>().playHurtAnim();
            }
        }
        fireTimer = 0f;
    }

    void PressReloadKey()
    {
        if (Input.GetKeyDown(ReloadKey) && !isReload && bulletNum != maxBulletNum)
        {
            SetReload();
        }
    }

    void SetReload()
    {
        //isFire = false;
        isReload = true;
        if (useReload) ReladTimerUI.SetActive(true);

        //ReloadTimer = OneBulletReloadTime * (maxBulletNum - bulletNum);
        SetReloadTimer();
        currentReloadTime = ReloadTimer;
        ReladTimerUI.GetComponent<Slider>().maxValue = ReloadTimer;
    }

    //void reloadBullet()
    //{
    //    if (reloadType == reloadBulletType.oneByOneReload)
    //    {
    //        reloadOneByOneBullet();
    //    }

    //    else if (reloadType == reloadBulletType.allReload)
    //    {
    //        reloadAllBullet();
    //    }
    //}
    void reloadBullet()
    {
        if (!isReload) return;
        //Debug.Log("reload bullet");
        ReloadTimer -= Time.deltaTime;
        ReladTimerUI.GetComponent<Slider>().value = ReloadTimer;

        if (reloadType == reloadBulletType.oneByOneReload) increaseBullet();
        if (ReloadTimer < 0)
        {
            bulletNum = maxBulletNum;

            setReloadBulletUI(false);
        }
    }
    //void reloadAllBullet()
    //{
    //    if (!isReload) return;

    //    ReloadTimer -= Time.deltaTime;
    //    ReladTimerUI.GetComponent<Slider>().value = ReloadTimer;

    //    if (ReloadTimer < 0)
    //    {
    //        bulletNum = maxBulletNum;

    //        setReloadBulletUI(false);
    //    }
    //}

    void setReloadBulletUI(bool state)
    {
        ReladTimerUI.SetActive(state);
        ReloadTimer = maxReloadTime;
        //ReloadTimer = allReloadTime;
        isReload = state;
        ReladTimerUI.GetComponent<Slider>().maxValue = ReloadTimer;
    }

    void increaseBullet()
    {
        //Debug.Log("increase bullet");

        for (int i = 1; i < maxBulletNum - bulletNum + 1; i++)
        {
            //Debug.Log(i);
            if (currentReloadTime - OneBulletReloadTime > ReloadTimer)
            {
                //Debug.Log(currentReloadTime);
                //Debug.Log("증가");
                bulletNum++;
                currentReloadTime -= OneBulletReloadTime;
            }
        }
    }

    void bulletUI()
    {
        bulletText.text = bulletNum + " / " + maxBulletNum;
    }


    IEnumerator reloadActionCoroutine()
    {
        Vector3 reloadAction = new Vector3(GunOriginPos.x, GunOriginPos.y, reloadActionForce);
        Gun.transform.localPosition = GunOriginPos;
        //Debug.Log("reloadAction: "+reloadAction);

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

