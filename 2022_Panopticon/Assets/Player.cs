using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private Rigidbody rb;

    // Walk
    public float walkSpeed;
    public Vector3 HeadBobAmount = new Vector3(0f, .05f, 0f);
    public bool iswalking = false;
    private float walkHeight;

    // Sprint
    public bool useSprint = true;
    public float sprintSpeed = 50f;
    public bool isSprinting = false;

    // Stamina
    public bool useStaminaLimit = true;
    public GameObject StaminaBar;
    public float maxStamina = 5f;
    private float stamina = 5f;

    // Crouch
    public bool useCrouch = true;
    public float crouchSpeed = 15f;
    public float crouchHeight = 0.5f;
    public bool isCrouching = false;

    // Head Bob
    public bool useHeadBob = true;
    public float walkHeadBobSpeed = 5f;
    public float sprintHeadBobSpeed = 10f;

    // Jump
    public bool useJump = true;
    public float jumpForce = 5f;
    float groundCheckDistance = .1f;
    private float bufferCheckDistance = .1f;
    public bool isGround = false;

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

    public GameObject[] Weapon = new GameObject[4];

    public bool useItem = true;

    //GameObject Gun;
    //GameObject Bomb;
    //GameObject EnergeDrink;

    Vector3 GunOriginPos = new Vector3(0, 0, 0);
    Quaternion GunOriginRot = Quaternion.Euler(0, 180, 0);
    Vector3 GunSprintPos = new Vector3(-0.271f, 0.001f, 0.135f);
    Quaternion GunSprintRot = Quaternion.Euler(-0.133f, 123.826f, -0.249f);
    Vector3 GetItemPos = new Vector3(0, 0.2f, 0);
    Quaternion GetItemRot = Quaternion.Euler(-15f, 180, 0);
    public Transform BombPostion;
    public float BombRadius = 10.0f;
    public float BombForce = 500f;
    public float throwForce = 1000f;

    int BombDamage;
    new Rigidbody rigidbody;

    // Fire
    public GameObject bulletEffect;
    ParticleSystem psBullet;
    public int maxBulletNum = 10;
    public int bulletNum;
    public float fireRate = 0.5f;
    public float fireTimer;

    // Canvas
    public Text crossHairText;
    public CrossHairType crosshairtype;
    public GameObject BulletNumUI;
    public GameObject ReloadTimerUI;
    public GameObject ItemNumUI;

    //Reload
    public bool useReload = true;
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
    public AudioClip FireSound, oneByOneReloadSound, allReloadSound, getItemSound, changeWaeponSound, energySound;
    AudioSource audioSource;

    // Timer
    private float shakeTimer;
    private float zoomTimer;
    float energyTimer;
    float ultimateTimer;
    public float ultimateTime = 7f;

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

    public GameObject BombInstantiate;

    bool isPulling = false;
    float distanceThreshold = 1f;

    public int ultimateGauge = 0;
    public float radius = 1f;
    public Transform middleAimPoint;
    List<Collider> colliders = new List<Collider>();

    public GameObject ultimateCrossHair;
    public Canvas playerCanvas;
    Collider closestCollider = null;

    float RayCastDis = 21f;
    int ultimateNum = 100;

    public GameObject getItemEffect;
    
    private void Start()
    {
        ultimateNum = 100;
        // hide cursor
        Cursor.lockState = CursorLockMode.Locked;

        // initialize
        rb = GetComponent<Rigidbody>();
        audioSource = StageSetting.Instance.gameObject.transform.GetChild(0).GetComponent<AudioSource>();
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
            // Gun instantiate
            Weapon[0] = Instantiate(GunModel, GunHandle.transform) as GameObject;
            Weapon[0].transform.parent = GunHandle.transform;
            WeaponNum[0] = 1;

            // bomb instantiate
            Weapon[2] = Instantiate(BombModel, GunHandle.transform) as GameObject;
            Weapon[2].transform.parent = GunHandle.transform;
            WeaponNum[2] = 0;

            Destroy(Weapon[2].GetComponent<Bomb>());
            Destroy(Weapon[2].GetComponent<Rigidbody>());

            //Weapon[1].SetActive(false);
        }
        else
        {
            crossHairText.enabled = false;
        }

        if (useItem)
        {
            // energydrink instantiate
            Weapon[3] = Instantiate(EnergeDrinkModel, GunHandle.transform) as GameObject;
            Weapon[3].transform.parent = GunHandle.transform;
            //Weapon[2].SetActive(false);
            WeaponNum[3] = 0;
        }

        // bullet instantiate
        bulletNum = maxBulletNum;
        OneBulletReloadTime = maxReloadTime / maxBulletNum;
        psBullet = bulletEffect.GetComponent<ParticleSystem>();

        // reload instantiate
        SetReloadTimer();
        ReloadTimerUI.SetActive(false);
        currentReloadTime = ReloadTimer;
        ReloadTimerUI.GetComponent<Slider>().maxValue = ReloadTimer;

        if (!useReload || !useGun) BulletNumUI.SetActive(false);

        // set SprintBar
        if (useSprint && useStaminaLimit) StaminaBar.SetActive(true);
        else StaminaBar.SetActive(false);

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
        // getItem instantiate
        Weapon[1] = Instantiate(getItemModel, GunHandle.transform) as GameObject;
        Weapon[1].transform.parent = GunHandle.transform;

        WeaponNum[1] = 1;


        offWeapon();
        Weapon[currentIndex].SetActive(true);

        setRemainItemUI(false);
        ultimateTimer = ultimateTime;


        if (StageSetting.Instance.getStage() == "easy")
        {
            ultimateNum = 30;
            ultimateTime = 12;
        }
        else if (StageSetting.Instance.getStage() == "normal")
        {
            ultimateNum = 50;
            ultimateTime = 10;
        }
        else if (StageSetting.Instance.getStage() == "hard")
        {
            ultimateNum = 100;
            ultimateTime = 7;
        }
    }

    private void Update()
    {
        if (useSprint)
        {
            Sprint();
            StaminaUI();
        }
        if (!isZooming && !isSprinting) theCamera.fieldOfView = defaultFOV;
        Crouch();
        checkGroundDistance();


    }

    public void endSprint()
    {
        currentSpeed = walkSpeed;
        zoomTimer = zoomSpeed;
        isSprinting = false;
        setGunOrigin();
    }

    public void Move()
    {
        float _moveDirX = Input.GetAxisRaw("Horizontal");
        float _moveDirZ = Input.GetAxisRaw("Vertical");
        Vector3 _moveHorizontal = transform.right * _moveDirX;
        Vector3 _moveVertical = transform.forward * _moveDirZ;

        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            Vector3 _velocity;
            _velocity = (_moveHorizontal + _moveVertical).normalized * currentSpeed;

            rb.MovePosition(transform.position + _velocity * Time.deltaTime);
            iswalking = true;

        }

    }

    public void Sprint()
    {
        if (isSprinting)
        {
            SetFOVSmooth(65);

            currentSpeed = sprintSpeed;

            if (useGun)
            {
                if (currentIndex == 0 || currentIndex == 1)
                {

                    Weapon[currentIndex].transform.localPosition = Vector3.Slerp(Weapon[currentIndex].transform.localPosition, GunSprintPos, GunRotationSpeed * Time.deltaTime);
                    Weapon[currentIndex].transform.localRotation = Quaternion.Slerp(Weapon[currentIndex].transform.localRotation, GunSprintRot, GunRotationSpeed * Time.deltaTime);
                }
            }

            if (useStaminaLimit) stamina -= Time.deltaTime;
            if (stamina < 0)
            {
                stamina = 0;
                isSprinting = false;
                setGunOrigin();
            }
        }
        else if (stamina < maxStamina) stamina += Time.deltaTime;
    }

    public void setCrouch()
    {
        if (!useCrouch) return;
        currentSpeed = crouchSpeed;
        isCrouching = true;
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - 0.5f, transform.localPosition.z);
    }

    public void EndCrouch()
    {
        currentSpeed = walkSpeed;
        isCrouching = false;
    }
    public void Crouch()
    {
        if (!useCrouch) return;
        if (isCrouching) GetComponent<CapsuleCollider>().height = crouchHeight;

        else GetComponent<CapsuleCollider>().height = walkHeight;

    }

    public void CameraRotationVerticality()
    {
        float _xRotation = Input.GetAxisRaw("Mouse Y");

        float _cameraRotationX = _xRotation * lookSensitivity;

        currentCameraRotationX -= _cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }

    public void CameraRotationHorizontality()
    {
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _cameraRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;
        rb.MoveRotation(rb.rotation * Quaternion.Euler(_cameraRotationY));
    }

    public void checkGroundDistance()
    {
        if (!useJump) return;
        if (!isCrouching) groundCheckDistance = (GetComponent<CapsuleCollider>().height / 2) + bufferCheckDistance;
        else groundCheckDistance = (GetComponent<CapsuleCollider>().height * 2) + bufferCheckDistance;
        RaycastHit hit;

        if (Physics.Raycast(transform.position, -transform.up, out hit, groundCheckDistance)) isGround = true;
        else isGround = false;
    }
    public void Jump()
    {
        GetComponent<Rigidbody>().AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    public void HeadBob()
    {
        if (!useHeadBob) return;
        if (iswalking && isSprinting)
        {
            shakeTimer += Time.deltaTime * sprintHeadBobSpeed;

            camHandle.localPosition = new Vector3(camHandlePos.x + Mathf.Sin(shakeTimer) * HeadBobAmount.x,
                camHandlePos.y + Mathf.Sin(shakeTimer) * HeadBobAmount.y,
                camHandlePos.z + Mathf.Sin(shakeTimer) * HeadBobAmount.z);

        }
        else if (iswalking && !isSprinting)
        {
            shakeTimer += Time.deltaTime * walkHeadBobSpeed;

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

    public void ZoomCamera()
    {
        isZooming = true;
        SetFOVSmooth(defaultFOV / ZoomMultipleNum);
    }

    public void EndZoomCamera()
    {
        isZooming = false;
        zoomTimer = zoomSpeed;
        SetFOVSmooth(defaultFOV);
    }
    void SetFOVSmooth(float target)
    {
        zoomTimer -= Time.deltaTime;
        theCamera.fieldOfView = Mathf.Lerp(target, theCamera.fieldOfView, zoomTimer * 0.5f);
    }

    void setGunOrigin()
    {
        Weapon[currentIndex].transform.localPosition = GunOriginPos;
        Weapon[currentIndex].transform.localRotation = GunOriginRot;
    }

    void SetReloadTimer()
    {
        if (reloadType == reloadBulletType.oneByOneReload) ReloadTimer = OneBulletReloadTime * (maxBulletNum - bulletNum);

        else if (reloadType == reloadBulletType.allReload) ReloadTimer = allReloadTime;

        //Bomb
        if (Weapon[2].activeSelf) ReloadTimer = 2f;
        //else ReloadTimer = 2f;
    }

    void offWeapon()
    {
        for (int i = 0; i < Weapon.Length; i++)
        {
            Weapon[i].SetActive(false);
        }
        BulletNumUI.SetActive(false);
        ItemNumUI.SetActive(false);
    }

    void SetCrossHair()
    {
        if (crosshairtype == CrossHairType.cross) crossHairText.text = "+";

        else if (crosshairtype == CrossHairType.circle) crossHairText.text = "¡Û";

        else if (crosshairtype == CrossHairType.dot) crossHairText.text = ".";
    }

    void setReloadBulletUI(bool state)
    {
        ReloadTimerUI.SetActive(state);
        ReloadTimer = maxReloadTime;
        isReload = state;
        ReloadTimerUI.GetComponent<Slider>().maxValue = ReloadTimer;
    }
    void setRemainItemUI(bool isShow)
    {
        ItemNumUI.SetActive(isShow);
        //Debug.Log("½ÇÇà " + RemainItemNumUI.activeSelf);

    }

    void StaminaUI()
    {
        float ratio = stamina / maxStamina;

        StaminaBar.GetComponent<Slider>().value = ratio;
    }
}
